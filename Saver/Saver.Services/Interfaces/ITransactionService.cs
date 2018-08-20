using Saver.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saver.Services.Interfaces
{
    /// <summary>
    /// Provides methods allowing us to post, view and move
    /// transactions between goals.
    /// </summary>
    public interface ITransactionService
    {
        /// <summary>
        /// Returns all transactions posted to a particular goal
        /// </summary>
        /// <param name="goalId">The ID of the goal</param>
        /// <returns>Returns the transactions for a goal</returns>
        IEnumerable<Transaction> GetTransactionsForGoal(int goalId);

        /// <summary>
        /// Returns all transactions ever posted by a user
        /// across any number of goals
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <returns>The transactions for the user</returns>
        IEnumerable<Transaction> GetAllTransactionsForUser(int userId);

        /// <summary>
        /// Creates the transaction on the system that can either
        /// go between two goals (if possible) or from a null sourced
        /// goal -- i.e. new funds against a user account
        /// </summary>
        /// <param name="userId">The ID of the user we are expecting for the goals</param>
        /// <param name="transaction">The transaction containing new information</param>
        /// <param name="targetGoalId">The goal that should be the target if required</param>
        /// <returns>The transaction that has been created on the system</returns>
        Transaction CreateTransaction(int userId, Transaction transaction, int? targetGoalId);
        
        /// <summary>
        /// Attempts to reverse a transaction
        /// </summary>
        /// <param name="transactionId">The ID of the transaction to reverse</param>
        /// <returns>The resulting reversal transaction</returns>
        Transaction ReverseTransaction(int transactionId);

        /// <summary>
        /// Withdraws the amount from the user's goal if
        /// the funds are present. If they are not, then
        /// the request is ignored with an error thrown
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <param name="amount">The amount to be withdrawn</param>
        /// <param name="goalId">The ID of the goal to withdraw from</param>
        /// <returns>The resulting transaction</returns>
        Transaction Withdraw(int userId, double amount, int goalId);

        /// <summary>
        /// Deposits the amount from into user's goal.
        /// This only works if the goal has not been completed
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <param name="amount">The amount to be deposited</param>
        /// <param name="goalId">The ID of the goal to which we will add</param>
        /// <returns>The resulting transaction</returns>
        Transaction Deposit(int userId, double amount, int goalId);
    }
}
