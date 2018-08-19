using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Saver.Model;
using Saver.Repositories.Implementations.Transaction;
using Saver.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saver.Repositories.Tests.Implementations.Transactions
{
    /// <summary>
    /// Tests all functionality of the transaction repository
    /// </summary>
    [TestClass]
    public class TransactionRepositoryTests : SqlRepositoryTestBase
    {
        private const string DEFAULT_SQL_STATEMENT = "INTERACT_WITH_TRANSACTION_TABLE";

        /// <summary>
        /// Creates a new instance of the repository tests
        /// with the default SQL statement set
        /// </summary>
        public TransactionRepositoryTests(): base(DEFAULT_SQL_STATEMENT)
        {
        }

        /// <summary>
        /// Tests that we can inject "new funds" in to an account
        /// </summary>
        [TestMethod()]
        public void ShouldCreateATransactionForAGivenGoalWithoutASource()
        {
            DateTime expectedPostTime = DateTime.Now;
            int expectedNewId = 10;
            int targetGoalId = 3;
            int expectedAmount = 10;
            Transaction createTransaction = new Transaction(expectedAmount, null, expectedPostTime);

            //Mocks the insert of a transaction
            mockDataAccess.Setup(s => s.ExecuteQuery<Transaction>(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()))
            .Returns<string, Dictionary<string, object>>((sql, parameters) =>
            {
                createTransaction.Id = expectedNewId;
                return new List<Transaction>() { createTransaction };
            });
            ITransactionRepository service = new TransactionRepository(mockDataAccess.Object, mockSqlStringService.Object);


            //Act
            Transaction createdTransaction = service.Create(createTransaction, targetGoalId);

            //Assert
            createdTransaction.Id.Should().Be(expectedNewId);
            createdTransaction.Amount.Should().Be(expectedAmount);
            createdTransaction.Timestamp.Should().Be(expectedPostTime);
            createdTransaction.SourceGoalId.Should().BeNull();
        }

        /// <summary>
        /// Tests that we can return a transaction from the system
        /// by the given ID
        /// </summary>
        [TestMethod]
        public void ShouldGetById()
        {
            //Arrange
            int knownId = 10;
            Transaction expectedTransaction = new Transaction(10, 100, null, DateTime.Now);

            mockDataAccess.Setup(s => s.ExecuteQuery<Transaction>(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()))
            .Returns<string, Dictionary<string, object>>((sql, parameters) =>
            {
                return new List<Transaction>() { expectedTransaction };
            });
            ITransactionRepository service = new TransactionRepository(mockDataAccess.Object, mockSqlStringService.Object);

            //Act
            Transaction transaction = service.GetById(knownId);

            //Assert
            transaction.Should().BeEquivalentTo(expectedTransaction);
        }

        /// <summary>
        /// Tests that we are able to get transactions for a goal
        /// </summary>
        [TestMethod()]
        public void ShouldGetTransactionsForGoal()
        {
            //Arrange
            int goalId = 10;
            List<Transaction> expectedTransactions = new List<Transaction>()
            {
                new Transaction(10, 100, null, DateTime.Now),
                new Transaction(20, 200, null, DateTime.Now),
                new Transaction(30, 1300, null, DateTime.Now),
                new Transaction(40, 2400, null, DateTime.Now),
            };

            mockDataAccess.Setup(s => s.ExecuteQuery<Transaction>(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).Returns(expectedTransactions);
            ITransactionRepository service = new TransactionRepository(mockDataAccess.Object, mockSqlStringService.Object);

            //Act
            IEnumerable<Transaction> transactions = service.GetTransactionsForGoal(goalId);

            //Assert
            transactions.Should().BeEquivalentTo(expectedTransactions);
        }

        /// <summary>
        /// Tests that we are able to get the transactions for a given user
        /// </summary>
        [TestMethod]
        public void ShouldGetTransactionsForUser()
        {            
            //Arrange
            int userId = 10;
            List<Transaction> expectedTransactions = new List<Transaction>()
            {
                new Transaction(10, 100, null, DateTime.Now),
                new Transaction(20, 200, null, DateTime.Now),
                new Transaction(30, 1300, null, DateTime.Now),
                new Transaction(40, 2400, null, DateTime.Now),
            };

            mockDataAccess.Setup(s => s.ExecuteQuery<Transaction>(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).Returns(expectedTransactions);
            ITransactionRepository service = new TransactionRepository(mockDataAccess.Object, mockSqlStringService.Object);

            //Act
            IEnumerable<Transaction> transactions = service.GetTransactionsForUser(userId);

            //Assert
            transactions.Should().BeEquivalentTo(expectedTransactions);
        }
    }
}