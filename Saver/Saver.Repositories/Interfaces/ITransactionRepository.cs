using Saver.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saver.Repositories.Interfaces
{
    /// <summary>
    /// Provides all interfae methods required for a transaction repository
    /// </summary>
    public interface ITransactionRepository
    {
        /// <summary>
        /// Creates a transaction on the system
        /// </summary>
        /// <param name="transaction">The transaction on the system</param>
        /// <param name="targetGoalId">The goal Id to which the transaction should be posted</param>
        /// <returns>The transaction that was created</returns>
        Transaction Create(Model.Transaction transaction, int? targetGoalId);

        /// <summary>
        /// Returns the transaction by the ID
        /// </summary>
        /// <param name="id">The ID of the transaction</param>
        /// <returns>The transaction with the given ID</returns>
        Transaction GetById(int id);

        /// <summary>
        /// Returns all transactions for a given user
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <returns>All transactions for the user</returns>
        IEnumerable<Transaction> GetTransactionsForUser(int userId);

        /// <summary>
        /// Returns all transactions for a given goal account
        /// </summary>
        /// <param name="goalId">The ID of the goal</param>
        /// <returns>All transactions for the goal</returns>
        IEnumerable<Transaction> GetTransactionsForGoal(int goalId);
    }
}