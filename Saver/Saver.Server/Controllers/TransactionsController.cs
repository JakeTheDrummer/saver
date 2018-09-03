using Saver.Model;
using Saver.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Saver.Server.Controllers
{
    /// <summary>
    /// Provides the controller endpoints for all Goals
    /// </summary>
    [RoutePrefix("api")]
    [Route("users/{userId:int:min(1)}/goals/{goalId:int:min(1)}/transactions")]
    public class TransactionsController : ApiController
    {
        private readonly ITransactionService transactionService = null;
        /// <summary>
        /// Creates a new Transaction Controller that will use the given transaction service
        /// </summary>
        /// <param name="transactionService">The service that should be used</param>
        public TransactionsController(ITransactionService transactionService)
        {
            this.transactionService = transactionService;
        }

        /// <summary>
        /// Returns all transactions on the system for the given goal
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <param name="goalId">The ID of the goal</param>
        /// <returns>The transactions on the system for the user and goal</returns>
        [HttpGet]
        public IEnumerable<Transaction> Get(int userId, int goalId)
        {
            return transactionService.GetTransactionsForGoal(goalId);
        }

        /// <summary>
        /// Attempts to create a new transaction on the system for the
        /// user with the given Id and the Goal
        /// </summary>
        /// <param name="userId">The ID of the User</param>
        /// <param name="goalId">The target goal of the tranaction</param>
        /// <param name="transaction">The </param>
        /// <returns></returns>
        [HttpPost]
        public Transaction Post(int userId, int? goalId, [FromBody]Transaction transaction)
        {
            return transactionService.CreateTransaction(userId, transaction, goalId);
        }

        /// <summary>
        /// Attempts to withdraw from the goal the given amount
        /// </summary>
        /// <param name="userId">The user Id of the goal</param>
        /// <param name="amount">The amount we wish to withdraw</param>
        /// <param name="goalId">The goal from which we are withdrawing</param>
        /// <returns>The transaction that has been created</returns>
        [HttpPut]
        [Route("users/{userId:int:min(1)}/goals/{goalId:int:min(1)}/withdrawals")]
        public Transaction Withdraw(int userId, int goalId, double? amount)
        {
            if (!amount.HasValue)
                throw new ArgumentOutOfRangeException(nameof(amount), "Please provide an amount");

            return transactionService.Withdraw(userId, amount.Value, goalId);
        }

        /// <summary>
        /// Attempts to deposit the amount to the goal
        /// </summary>
        /// <param name="userId">The user Id of the goal</param>
        /// <param name="amount">The amount we wish to deposit</param>
        /// <param name="goalId">The goal to which we are depositing</param>
        /// <returns>The transaction that has been created</returns>
        [HttpPut]
        [Route("users/{userId:int:min(1)}/goals/{goalId:int:min(1)}/deposits")]
        public Transaction Deposit(int userId, int goalId, double? amount = null)
        {
            if (!amount.HasValue)
                throw new ArgumentOutOfRangeException(nameof(amount), "Please provide an amount");

            return transactionService.Deposit(userId, amount.Value, goalId);
        }
    }
}
