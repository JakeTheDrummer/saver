using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Saver.Model;
using Saver.Repositories.Interfaces;
using Saver.Services.Implementations;
using Saver.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saver.Services.Tests
{
    /// <summary>
    /// Tests all service methods for transactions
    /// </summary>
    [TestClass()]
    public class TransactionServiceTests
    {
        Mock<ITransactionRepository> mockTransactionRepository = new Mock<ITransactionRepository>();
        Mock<IGoalRepository> mockGoalRepository = new Mock<IGoalRepository>();
        Mock<IMilestoneRepository> mockMilestoneRepository = new Mock<IMilestoneRepository>();

        /// <summary>
        /// Creates a new instance of the service tests
        /// </summary>
        public TransactionServiceTests()
        {
            this.mockTransactionRepository = new Mock<ITransactionRepository>();
            this.mockGoalRepository = new Mock<IGoalRepository>();
            this.mockMilestoneRepository = new Mock<IMilestoneRepository>();
        }

        /// <summary>
        /// Test that we can return all transactisons for a given goal
        /// </summary>
        [TestMethod]
        public void ShouldGetTransactionsForGoal()
        {
            //Arrange
            int knownGoalId = 10;
            List<Transaction> knownGoalTransactions = new List<Transaction>()
            {
                new Transaction(121, 40, null, DateTime.Now),
                new Transaction(122, 40, knownGoalId, DateTime.Now),
                new Transaction(123, 40, knownGoalId, DateTime.Now),
                new Transaction(124, 40, null, DateTime.Now),
            };

            mockTransactionRepository.Setup(tr => tr.GetTransactionsForGoal(It.Is<int>(id => id == knownGoalId))).Returns(knownGoalTransactions);
            ITransactionService transactionService = new TransactionService(mockTransactionRepository.Object, mockGoalRepository.Object, mockMilestoneRepository.Object);

            //Act
            IEnumerable<Transaction> transactions = transactionService.GetTransactionsForGoal(knownGoalId);

            //Assert
            transactions.Should().BeEquivalentTo(knownGoalTransactions);
        }

        /// <summary>
        /// Test that we recieve null back when there are no tranasactions for a goal
        /// </summary>
        [TestMethod]
        public void ShouldReturnNullWhenThereAreNoTransactionsForAGoal()
        {
            //Arrange
            int knownGoalId = 10;
            List<Transaction> knownGoalTransactions = null;
            mockTransactionRepository.Setup(tr => tr.GetTransactionsForGoal(It.Is<int>(id => id == knownGoalId))).Returns(knownGoalTransactions);
            ITransactionService transactionService = new TransactionService(mockTransactionRepository.Object, mockGoalRepository.Object, mockMilestoneRepository.Object);

            //Act
            IEnumerable<Transaction> transactions = transactionService.GetTransactionsForGoal(knownGoalId);

            //Assert
            transactions.Should().BeNull();
        }

        /// <summary>
        /// Tests that we can return all transactions for a given user
        /// </summary>
        [TestMethod]
        public void ShouldGetAllTransactionsForUser()
        {
            //Arrange
            int knownUserId = 10;
            List<Transaction> knownUserTransactions = new List<Transaction>()
            {
                new Transaction(121, 40, null, DateTime.Now),
                new Transaction(122, 43, 131, DateTime.Now),
                new Transaction(123, 21, 324, DateTime.Now),
                new Transaction(124, 54, null, DateTime.Now),
            };

            mockTransactionRepository.Setup(tr => tr.GetTransactionsForUser(It.Is<int>(id => id == knownUserId))).Returns(knownUserTransactions);
            ITransactionService transactionService = new TransactionService(mockTransactionRepository.Object, mockGoalRepository.Object, mockMilestoneRepository.Object);

            //Act
            IEnumerable<Transaction> transactions = transactionService.GetAllTransactionsForUser(knownUserId);

            //Assert
            transactions.Should().BeEquivalentTo(knownUserTransactions);
        }

        /// <summary>
        /// Tests that if we have no transactions for a user that we recieve null
        /// </summary>
        [TestMethod]
        public void ShouldReturnNullWhenThereAreNoTransactionsForAGivenUser()
        {
            //Arrange
            int knownUserId = 10;
            List<Transaction> knownUserTransactions = null;
            mockTransactionRepository.Setup(tr => tr.GetTransactionsForUser(It.Is<int>(id => id == knownUserId))).Returns(knownUserTransactions);
            ITransactionService transactionService = new TransactionService(mockTransactionRepository.Object, mockGoalRepository.Object, mockMilestoneRepository.Object);

            //Act
            IEnumerable<Transaction> transactions = transactionService.GetAllTransactionsForUser(knownUserId);

            //Assert
            transactions.Should().BeNull();
        }

        /// <summary>
        /// Tests that if we fail to create the transaction within the database
        /// that we cannot continue and fail with an appropriate error
        /// </summary>
        [TestMethod]
        public void ShouldHaltWithExceptionWhenCreationOfTransactionFails()
        {
            //Arrange
            int knownUserId = 150;
            int knownTargetGoalId = 15;
            Transaction transaction = new Transaction(120, null, DateTime.Now);

            //When creating, assume that the creation has failed and we have a null value
            mockTransactionRepository.Setup(tr => tr.Create(It.Is<Transaction>(t => t.Equals(transaction)), It.Is<int?>(id => id.GetValueOrDefault() == knownTargetGoalId))).Returns<Transaction, int?>(null);

            //Assume the goals exist for the user
            List<Goal> userGoals = new List<Goal>() { new Goal(knownTargetGoalId, "Testing Goal", "Testing", 4000, GoalStatus.Open, true) };
            mockGoalRepository.Setup(gr => gr.GetGoalsForUser(It.Is<int>(id => id == knownUserId))).Returns(userGoals);

            ITransactionService transactionService = new TransactionService(mockTransactionRepository.Object, mockGoalRepository.Object, mockMilestoneRepository.Object);

            //Act
            Action failToCreateAction = () => transactionService.CreateTransaction(knownUserId, transaction, knownTargetGoalId);

            //Assert
            failToCreateAction.Should().Throw<Exception>();
        }

        /// <summary>
        /// Tests that we are able to create transactions for a given user and target goal
        /// </summary>
        [TestMethod]
        public void ShouldCreateADepositTransaction()
        {
            //Arrange
            int knownUserId = 150;
            int knownTargetGoalId = 15;
            int expectedTransactionId = 1154;
            DateTime expectedTransactionCreateTime = DateTime.Now;
            Transaction transaction = new Transaction(120, null, expectedTransactionCreateTime);
            Transaction expectedTransaction = new Transaction(expectedTransactionId, 120, null, expectedTransactionCreateTime);

            //When creating, simply assign the ID as if this has been created on the server
            mockTransactionRepository.Setup(tr => tr.Create(It.Is<Transaction>(t => t.Equals(transaction)), It.Is<int?>(id => id.GetValueOrDefault() == knownTargetGoalId))).Returns<Transaction, int?>
            (
                (t, id) =>
                {
                    t.Id = expectedTransactionId;
                    return t;
                }
            );

            //Assume the goals exist for the user
            List<Goal> userGoals = new List<Goal>() { new Goal(knownTargetGoalId, "Testing Goal", "Testing", 4000, GoalStatus.Open, true) };
            mockGoalRepository.Setup(gr => gr.GetGoalsForUser(It.Is<int>(id => id == knownUserId))).Returns(userGoals);

            ITransactionService transactionService = new TransactionService(mockTransactionRepository.Object, mockGoalRepository.Object, mockMilestoneRepository.Object);

            //Act
            Transaction createdTransaction = transactionService.CreateTransaction(knownUserId, transaction, knownTargetGoalId);

            //Assert
            createdTransaction.Should().BeEquivalentTo(expectedTransaction);
        }

        /// <summary>
        /// Tests that we are able to create transactions for a given user and target goal
        /// and that we update the relevant milestones (and no others)
        /// </summary>
        [TestMethod]
        public void ShouldCreateADepositTransactionAndUpdateRelevantMilestones()
        {
            //Arrange
            int knownUserId = 150;
            int knownTargetGoalId = 15;
            int expectedTransactionId = 1154;
            DateTime expectedTransactionCreateTime = DateTime.Now;
            const double depositAmount = 1250;
            Transaction transaction = new Transaction(depositAmount, null, expectedTransactionCreateTime);
            Transaction expectedTransaction = new Transaction(expectedTransactionId, depositAmount, null, expectedTransactionCreateTime);

            //When creating, simply assign the ID as if this has been created on the server
            mockTransactionRepository.Setup(tr => tr.Create(It.Is<Transaction>(t => t.Equals(transaction)), It.Is<int?>(id => id.GetValueOrDefault() == knownTargetGoalId))).Returns<Transaction, int?>
            (
                (t, id) =>
                {
                    t.Id = expectedTransactionId;
                    return t;
                }
            );

            //Assume the goals exist for the user
            List<Goal> userGoals = new List<Goal>() { new Goal(knownTargetGoalId, "Testing Goal", "Testing", 2500, GoalStatus.Open, true) };
            mockGoalRepository.Setup(gr => gr.GetGoalsForUser(It.Is<int>(id => id == knownUserId))).Returns(userGoals);

            //We have £500 posted against the goal
            IEnumerable<Transaction> transactionsForTargetGoal = new List<Transaction>()
            {
                new Transaction(123, 600.00, null, DateTime.Now.AddDays(-7)),
                new Transaction(175, 150.00, knownTargetGoalId, DateTime.Now.AddDays(-3)),
                new Transaction(251, 50.00, null, DateTime.Now.AddDays(-1)),
            };
            mockTransactionRepository.Setup(tr => tr.GetTransactionsForGoal(It.Is<int>(id => id == knownTargetGoalId))).Returns(transactionsForTargetGoal);

            //We have achieved "£500" already
            List<Milestone> milestonesForGoal = new List<Milestone>()
            {
                new Milestone(100, 500, "£500", DateTime.Now.AddDays(-7)),
                new Milestone(101, 1000, "£1000", null),
                new Milestone(102, 1500, "£1500", null),
                new Milestone(103, 2000, "£2000", null),
                new Milestone(104, 2500, "£2500", null)
            };
            mockMilestoneRepository.Setup(mr => mr.GetForGoal(It.Is<int>(id => id == knownTargetGoalId))).Returns(milestonesForGoal);

            //Ensure that we update ONLY the ones we know to have passed
            List<Milestone> updatedMilestones = new List<Milestone>();
            IEnumerable<Milestone> expectedAchievedMilestones = new List<Milestone>() { milestonesForGoal[1], milestonesForGoal[2] };
            mockMilestoneRepository
                .Setup(mr => mr.Update(It.IsAny<int>(), It.IsIn(expectedAchievedMilestones)))
                .Callback<int, Milestone>((id, milestone) =>
                {
                    //Add these to the list to be checked later
                    updatedMilestones.Add(milestone);
                })
                .Returns<int, Milestone>((id, ms) => ms);
            ITransactionService transactionService = new TransactionService(mockTransactionRepository.Object, mockGoalRepository.Object, mockMilestoneRepository.Object);

            //Act
            Transaction createdTransaction = transactionService.CreateTransaction(knownUserId, transaction, knownTargetGoalId);

            //Assert
            createdTransaction.Should().BeEquivalentTo(expectedTransaction);
            
            //Check that the expected milestones have been reached
            updatedMilestones.Count.Should().Be(2);
            updatedMilestones.All(ms => ms.DateMet.Value.Equals(expectedTransactionCreateTime)).Should().BeTrue();

            //Should only have called update twice
            mockMilestoneRepository.Verify(mr => mr.Update(It.IsAny<int>(), It.IsIn(expectedAchievedMilestones)), Times.Exactly(2));
        }
        
        /// <summary>
        /// Tests that we cannot create the transaction is null
        /// </summary>
        [TestMethod]
        public void ShouldNotCreateTransactionIfTransactionIsNull()
        {
            //Arrange
            int knownUserId = 130;
            int? knownGoalId = 15;
            Transaction transaction = null;
            ITransactionService transactionService = new TransactionService(mockTransactionRepository.Object, mockGoalRepository.Object, mockMilestoneRepository.Object);

            //Act
            Action failAction = () => transactionService.CreateTransaction(knownUserId, transaction, knownGoalId);

            //Assert
            failAction.Should().Throw<ArgumentNullException>();
        }

        /// <summary>
        /// Tests that we cannot create the transaction value is less than zero
        /// </summary>
        [TestMethod]
        public void ShouldNotCreateATransactionWithNegativeAmount()
        {
            //Arrange
            int knownUserId = 130;
            int? knownGoalId = 15;
            double amount = -450;
            Transaction transaction = new Transaction(amount, null, DateTime.Now);
            ITransactionService transactionService = new TransactionService(mockTransactionRepository.Object, mockGoalRepository.Object, mockMilestoneRepository.Object);

            //Act
            Action failAction = () => transactionService.CreateTransaction(knownUserId, transaction, knownGoalId);

            //Assert
            failAction.Should().Throw<ArgumentOutOfRangeException>();
        }

        /// <summary>
        /// Tests that we cannot create the transaction without a source or a target
        /// </summary>
        [TestMethod]
        public void ShouldNotCreateATransactionIfSourceAndTargetGoalsAreEmpty()
        {
            //Arrange
            int knownUserId = 130;
            int? knownGoalId = null;
            double amount = 25;
            Transaction transaction = new Transaction(amount, null, DateTime.Now);
            ITransactionService transactionService = new TransactionService(mockTransactionRepository.Object, mockGoalRepository.Object, mockMilestoneRepository.Object);

            //Act
            Action failAction = () => transactionService.CreateTransaction(knownUserId, transaction, knownGoalId);

            //Assert
            failAction.Should().Throw<ArgumentOutOfRangeException>();
        }

        /// <summary>
        /// Tests that we cannot create the transaction if the source and target at the same
        /// </summary>
        [TestMethod]
        public void ShouldNotCreateATransactionIfSourceAndTargetGoalsAreEqual()
        {
            //Arrange
            int knownUserId = 130;
            int? knownGoalId = 15;
            double amount = 25;
            Transaction transaction = new Transaction(amount, knownGoalId, DateTime.Now);
            ITransactionService transactionService = new TransactionService(mockTransactionRepository.Object, mockGoalRepository.Object, mockMilestoneRepository.Object);

            //Act
            Action failAction = () => transactionService.CreateTransaction(knownUserId, transaction, knownGoalId);

            //Assert
            failAction.Should().Throw<ArgumentOutOfRangeException>();
        }

        /// <summary>
        /// Tests that we cannot create the transaction if the source goal does not belong to the user
        /// </summary>
        [TestMethod]
        public void ShouldNotCreateTransactionIfTheSourceGoalDoesNotBelongToUser()
        {
            //Arrange
            int knownUserId = 130;
            int? invalidGoalId = 15;
            double amount = 25;
            Transaction transaction = new Transaction(amount, invalidGoalId, DateTime.Now);

            List<Goal> userGoals = new List<Goal>()
            {
                new Goal(100, "Testing A", null, 230, GoalStatus.Open, false),
                new Goal(250, "Testing B", null, 1500, GoalStatus.Cancelled, false),
                new Goal(500, "Testing C", null, 95, GoalStatus.Open, false),
            };
            mockGoalRepository.Setup(gr => gr.GetGoalsForUser(It.Is<int>(id => id == knownUserId))).Returns(userGoals);
            ITransactionService transactionService = new TransactionService(mockTransactionRepository.Object, mockGoalRepository.Object, mockMilestoneRepository.Object);

            //Act
            Action failAction = () => transactionService.CreateTransaction(knownUserId, transaction, invalidGoalId);

            //Assert
            failAction.Should().Throw<ArgumentOutOfRangeException>();
        }


        [TestMethod()]
        public void ShouldReverseTransaction()
        {
            Assert.Inconclusive("We are not able to test this method yet");
        }

        /// <summary>
        /// Tests that we can withdraw funds from a user's goal if they have the available funds
        /// </summary>
        [TestMethod()]
        public void ShouldWithdraw()
        {
            const double withdrawAmount = 30;
            int knownUserId = 78;
            int knownGoalId = 540;
            int expectedTransactionId = 1185;
            DateTime expectedTransactionCreateTime = DateTime.Now;
            Transaction expectedTransaction = new Transaction(expectedTransactionId, withdrawAmount, knownGoalId, It.IsAny<DateTime>());

            //When creating, simply assign the ID as if this has been created on the server
            mockTransactionRepository.Setup(tr => tr.Create(It.IsAny<Transaction>(), It.Is<int?>(id => !id.HasValue))).Returns<Transaction, int?>
            (
                (t, id) =>
                {
                    t.Id = expectedTransactionId;
                    return t;
                }
            );

            //Setup the ability to get user goals
            List<Goal> userGoals = new List<Goal>()
            {
                new Goal(100, "Testing A", null, 230, GoalStatus.Open, false),
                new Goal(250, "Testing B", null, 1500, GoalStatus.Cancelled, false),
                new Goal(540, "Testing C", null, 95, GoalStatus.Open, false),
            };
            mockGoalRepository.Setup(gr => gr.GetGoalsForUser(It.Is<int>(id => id == knownUserId))).Returns(userGoals);

            //Exceeds withrawal amount
            IEnumerable<Transaction> knownGoalTransactions = new List<Transaction>()
            {
                new Transaction(130, 10, null, DateTime.Now),
                new Transaction(450, 25, null, DateTime.Now),
                new Transaction(758, 150, null, DateTime.Now),
                new Transaction(1001, 10, null, DateTime.Now),
            };
            mockTransactionRepository.Setup(tr => tr.GetTransactionsForGoal(It.Is<int>(id => id == knownGoalId))).Returns(knownGoalTransactions);
            ITransactionService transactionService = new TransactionService(mockTransactionRepository.Object, mockGoalRepository.Object, mockMilestoneRepository.Object);

            //Act
            Transaction withdrawal = transactionService.Withdraw(knownUserId, withdrawAmount, knownGoalId);

            //Assert
            withdrawal.Should().BeEquivalentTo(expectedTransaction, options => options.Excluding(transaction => transaction.Timestamp));
        }

        /// <summary>
        /// Tests that we can not withdraw more from a goal than we have in the total amount
        /// </summary>
        [TestMethod()]
        public void ShouldNotWithdrawIfUserGoalHasInsufficientPostedFunds()
        {
            const double withdrawAmount = 30;
            int knownUserId = 78;
            int knownGoalId = 540;

            //Setup the ability to get user goals
            List<Goal> userGoals = new List<Goal>()
            {
                new Goal(100, "Testing A", null, 230, GoalStatus.Open, false),
                new Goal(250, "Testing B", null, 1500, GoalStatus.Cancelled, false),
                new Goal(540, "Testing C", null, 95, GoalStatus.Open, false),
            };
            mockGoalRepository.Setup(gr => gr.GetGoalsForUser(It.Is<int>(id => id == knownUserId))).Returns(userGoals);

            //Does not meet withrawal amount
            IEnumerable<Transaction> knownGoalTransactions = new List<Transaction>()
            {
                new Transaction(130, 10, null, DateTime.Now),
                new Transaction(450, 25, null, DateTime.Now),
                new Transaction(758, 6, knownGoalId, DateTime.Now)
            };
            mockTransactionRepository.Setup(tr => tr.GetTransactionsForGoal(It.Is<int>(id => id == knownGoalId))).Returns(knownGoalTransactions);
            ITransactionService transactionService = new TransactionService(mockTransactionRepository.Object, mockGoalRepository.Object, mockMilestoneRepository.Object);

            //Act
            Action failToWithdrawAction = () => transactionService.Withdraw(knownUserId, withdrawAmount, knownGoalId);

            //Assert
            failToWithdrawAction.Should().Throw<ArgumentOutOfRangeException>();
        }

        /// <summary>
        /// Tests that we can create a deposit transaction for a user goal
        /// and recieve the appropriate object back from the service
        /// </summary>
        [TestMethod]
        public void ShouldDeposit()
        {
            //Arrange
            int knownUserId = 150;
            int knownTargetGoalId = 15;
            int expectedTransactionId = 1154;
            double depositAmount = 150;
            Transaction expectedTransaction = new Transaction(expectedTransactionId, depositAmount, null, DateTime.Now);

            //When creating, simply assign the ID as if this has been created on the server
            mockTransactionRepository.Setup(tr => tr.Create(It.IsAny<Transaction>(), It.Is<int?>(id => id.GetValueOrDefault() == knownTargetGoalId))).Returns<Transaction, int?>
            (
                (t, id) =>
                {
                    t.Id = expectedTransactionId;
                    return t;
                }
            );

            //Assume the goals exist for the user
            List<Goal> userGoals = new List<Goal>() { new Goal(knownTargetGoalId, "Testing Goal", "Testing", 4000, GoalStatus.Open, true) };
            mockGoalRepository.Setup(gr => gr.GetGoalsForUser(It.Is<int>(id => id == knownUserId))).Returns(userGoals);

            ITransactionService transactionService = new TransactionService(mockTransactionRepository.Object, mockGoalRepository.Object, mockMilestoneRepository.Object);

            //Act
            Transaction createdTransaction = transactionService.Deposit(knownUserId, depositAmount, knownTargetGoalId);

            //Assert
            createdTransaction.Should().BeEquivalentTo(expectedTransaction, options => options.Excluding(transaction => transaction.Timestamp));
        }
    }
}