using Saver.Model;
using Saver.Repositories.Interfaces;
using Saver.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saver.Services.Implementations
{
    /// <summary>
    /// Provides the standard implementation for all transaction
    /// operations on the saver system
    /// </summary>
    public class TransactionService : ServiceBase, ITransactionService
    {
        IGoalRepository goalRepository = null;
        ITransactionRepository transactionRepository = null;
        IMilestoneRepository milestoneRepository = null;

        /// <summary>
        /// Creates a new Transaction Service allowing us to deal with transactions
        /// </summary>
        /// <param name="transactionRepository">The repository to access stored transactions</param>
        /// <param name="goalRepository">The repository to access stored goals</param>
        /// <param name="milestoneRepository">The repository to access stored milestones</param>
        public TransactionService(ITransactionRepository transactionRepository, IGoalRepository goalRepository, IMilestoneRepository milestoneRepository)
        {
            this.transactionRepository = transactionRepository;
            this.goalRepository = goalRepository;
            this.milestoneRepository = milestoneRepository;
        }
        
        /// <summary>
        /// Returns all transactions posted to a particular goal
        /// </summary>
        /// <param name="goalId">The ID of the goal</param>
        /// <returns>Returns the transactions for a goal</returns>
        public IEnumerable<Transaction> GetTransactionsForGoal(int goalId)
        {
            return ExecuteThenOrderBy(() => transactionRepository.GetTransactionsForGoal(goalId), trans => trans.Id);
        }

        /// <summary>
        /// Returns all transactions ever posted by a user
        /// across any number of goals
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <returns>The transactions for the user</returns>
        public IEnumerable<Transaction> GetAllTransactionsForUser(int userId)
        {
            return ExecuteThenOrderBy(() => transactionRepository.GetTransactionsForUser(userId), trans => trans.Id);
        }

        /// <summary>
        /// Creates the transaction on the system that can either
        /// go between two goals (if possible) or from a null sourced
        /// goal -- i.e. new funds against a user account
        /// </summary>
        /// <param name="userId">The ID of the user we are expecting for the goals</param>
        /// <param name="transaction">The transaction containing new information</param>
        /// <param name="targetGoalId">The ID of the target goal</param>
        /// <returns>The transaction that has been created on the system</returns>
        public Transaction CreateTransaction(int userId, Transaction transaction, int? targetGoalId)
        {
            //Validate we can create the transaction
            ValidateTransaction(transaction, targetGoalId);

            ValidateTransactionGoals(userId, out IEnumerable<Goal> userGoals, transaction.SourceGoalId, targetGoalId);

            //If we are withdrawing, make sure we have the funds
            bool userWithdrawingFunds = transaction.SourceGoalId.HasValue && !targetGoalId.HasValue && userGoals != null;
            double currentGoalAmount = 0;
            if (userWithdrawingFunds)
                ValidateRemainingFundsForGoalAgainstTransactionAmount(transaction, userGoals, out currentGoalAmount);

            //If we reach here, we can add the transaction
            Transaction createdTransaction = transactionRepository.Create(transaction, targetGoalId);
            if (createdTransaction == null)
                throw new Exception("An error occurred when creating the transaction on the system.");

            //Calculate any milestones now passed
            if (!userWithdrawingFunds)
            {
                currentGoalAmount += transaction.Amount;
                UpdateAnyNewlySurpassedMilestones(targetGoalId, currentGoalAmount, createdTransaction);
            }

            return createdTransaction;
        }

        /// <summary>
        /// Updates any milestones that we have now surpassed by creating this transaction
        /// with the current amount -- this updates the database
        /// </summary>
        /// <param name="targetGoalId">The target goal Id</param>
        /// <param name="currentGoalAmount">The current amount of the goal (after the created transaction)</param>
        /// <param name="createdTransaction">The transaction that we have created</param>
        private void UpdateAnyNewlySurpassedMilestones(int? targetGoalId, double currentGoalAmount, Transaction createdTransaction)
        {
            IEnumerable<Milestone> newlySurpassedMilestones = GetNewlySurpassedUpdatedMilestones(createdTransaction, targetGoalId, currentGoalAmount);
            if (newlySurpassedMilestones.Any())
            {
                foreach (var milestone in newlySurpassedMilestones)
                {
                    milestoneRepository.Update(milestone.Id, milestone);
                }
            }
        }

        /// <summary>
        /// Returns the newly surpassed milestones with the date of the transaction
        /// </summary>
        /// <param name="transaction">The transaction that we have created</param>
        /// <param name="targetGoalId">The target goal ID</param>
        /// <param name="currentGoalAmount">The current amount of the goal</param>
        /// <returns>All milestones that have been surpassed by creating this transaction</returns>
        private IEnumerable<Milestone> GetNewlySurpassedUpdatedMilestones(Transaction transaction, int? targetGoalId, double currentGoalAmount)
        {
            //Get the milestones that have been surpassed
            IEnumerable<Milestone> newlySurpassedMilestones = null;
            IEnumerable<Milestone> surpassedMilestones = GetSurpassedMilestonesForGoal(targetGoalId.Value, currentGoalAmount);
            if (surpassedMilestones != null && surpassedMilestones.Any())
            {
                //Update the date with now
                newlySurpassedMilestones = surpassedMilestones.Where(milestone => !milestone.DateMet.HasValue);
                foreach (var milestone in newlySurpassedMilestones)
                {
                    milestone.DateMet = transaction.Timestamp;
                }
            }
            return newlySurpassedMilestones;
        }

        /// <summary>
        /// Returns the milestones that have been surpased for this goal
        /// given the amount that has been provided to the method
        /// </summary>
        /// <param name="goalId">The ID of the goal we are checking</param>
        /// <param name="currentGoalAmount">The amount of the goal at present</param>
        /// <returns>The milestones that have been surpassed</returns>
        private IEnumerable<Milestone> GetSurpassedMilestonesForGoal(int goalId, double currentGoalAmount)
        {
            IEnumerable<Milestone> goalMilestones = milestoneRepository.GetForGoal(goalId);
            return goalMilestones.Where(ms => ms.Target <= currentGoalAmount);
        }

        /// <summary>
        /// Validates that we have the appropriate amount posted against the goal
        /// to be able to withdraw this amount
        /// </summary>
        /// <param name="transaction">The transaction we are attempting to create</param>
        /// <param name="userGoals">The goals for the user</param>
        /// <param name="currentGoalAmount">The current goal amount</param>
        private void ValidateRemainingFundsForGoalAgainstTransactionAmount(Transaction transaction, IEnumerable<Goal> userGoals, out double currentGoalAmount)
        {
            Goal sourceGoal = userGoals.First(g => g.Id == transaction.SourceGoalId.Value);
            IEnumerable<Transaction> goalTransactions = GetTransactionsForGoal(sourceGoal.Id);

            double postedToGoal = goalTransactions
                                    .Where(trans => !trans.SourceGoalId.HasValue || trans.SourceGoalId != sourceGoal.Id)
                                    .Sum(trans => trans.Amount);
            double withdrawnFromGoal = goalTransactions
                                        .Where(trans => trans.SourceGoalId.HasValue && trans.SourceGoalId == sourceGoal.Id)
                                        .Sum(trans => trans.Amount);

            currentGoalAmount = postedToGoal - withdrawnFromGoal;
            if (currentGoalAmount < transaction.Amount)
                throw new ArgumentOutOfRangeException(nameof(transaction.Amount), "The amount being withdrawn exceeds the available funds for this goal");
        }

        /// <summary>
        /// Validates that all goals provided exist for the same user
        /// </summary>
        /// <param name="userId">The user whose goals are being checked</param>
        /// <param name="goalIds">The IDs of any goals that should be checked</param>
        private void ValidateTransactionGoals(int userId, out IEnumerable<Goal> userGoals, params int?[] goalIds)
        {
            if (userId <= 0)
                throw new ArgumentOutOfRangeException(nameof(userId), "The user ID should be greater than zero");

            int[] testGoalIds = goalIds.Where(id => id.HasValue).Select(id => id.Value).ToArray();

            //Ensure that all goals exist in the user's goal collection
            userGoals = goalRepository.GetGoalsForUser(userId);
            IEnumerable<int> userGoalIds = userGoals.Select(g => g.Id);
            if (!(userGoalIds.Intersect(testGoalIds).Count() == testGoalIds.Length))
                throw new ArgumentOutOfRangeException("One or more goals do not exist in the target user's goal collection");
        }

        /// <summary>
        /// Validates that the transaction and throws the appropriate exception
        /// if the details of the transaction are not valid in any way
        /// </summary>
        /// <param name="transaction">The transaction to be validated</param>
        /// <param name="targetGoalId">The target goal ID</param>
        private static void ValidateTransaction(Transaction transaction, int? targetGoalId)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction), "Please ensure a transaction is provided to be posted");

            if (transaction.Amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(transaction.Amount), "Please ensure that the amount provided is always positive and non-zero");

            if (!transaction.SourceGoalId.HasValue && !targetGoalId.HasValue)
                throw new ArgumentOutOfRangeException("Please ensure that the source, the target or both are provided to post transactions");

            if (targetGoalId.HasValue && transaction.SourceGoalId.HasValue && transaction.SourceGoalId == targetGoalId.Value)
                throw new ArgumentOutOfRangeException("Please ensure that the source and the target goal are NOT the same");
        }

        /// <summary>
        /// Attempts to reverse a transaction
        /// </summary>
        /// <param name="transactionId">The ID of the transaction to reverse</param>
        /// <returns>The resulting reversal transaction</returns>
        public Transaction ReverseTransaction(int transactionId)
        {
            throw new NotImplementedException();
            //Get the transaction
            /*Transaction transaction = transactionRepository.GetById(transactionId);
            if (transaction == null)
                throw new ArgumentOutOfRangeException(nameof(transactionId), "The transaction with this ID does not exist");

            */
        }

        /// <summary>
        /// Withdraws the amount from the user's goal if
        /// the funds are present. If they are not, then
        /// the request is ignored with an error thrown
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <param name="amount">The amount to be withdrawn</param>
        /// <param name="goalId">The ID of the goal to withdraw from</param>
        /// <returns>The resulting transaction</returns>
        public Transaction Withdraw(int userId, int amount, int goalId)
        {
            //Create the appropriate transaction
            Transaction transaction = new Transaction(amount, goalId, DateTime.Now);
            return CreateTransaction(userId, transaction, null);
        }

        /// <summary>
        /// Deposits the amount from into user's goal.
        /// This only works if the goal has not been completed
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <param name="amount">The amount to be deposited</param>
        /// <param name="goalId">The ID of the goal to which we will add</param>
        /// <returns>The resulting transaction</returns>
        public Transaction Deposit(int userId, int amount, int goalId)
        {
            //Create the appropriate transaction
            Transaction transaction = new Transaction(amount, null, DateTime.Now);
            return CreateTransaction(userId, transaction, goalId);
        }
    }
}