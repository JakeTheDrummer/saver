using Saver.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Saver.Model;
using Saver.DataAccess.Interfaces;
using Saver.Repositories.Services.Interfaces;
using Saver.Repositories.Attributes;

namespace Saver.Repositories.Implementations.Transaction
{
    /// <summary>
    /// Provides a standard implementation for the transaction repository
    /// </summary>
    public class TransactionRepository : SqlRepositoryBase, ITransactionRepository
    {
        private readonly ITypedDataAccess typedDataAccess;

        /// <summary>
        /// Creates a new Transaction Repository
        /// </summary>
        /// <param name="dataAccess">The data access for the tranaction repository</param>
        /// <param name="sqlStringService">The service providing SQL strings when required</param>
        public TransactionRepository(ITypedDataAccess dataAccess, ISqlStringService sqlStringService) 
            : base(dataAccess, sqlStringService)
        {
            typedDataAccess = dataAccess;
        }

        /// <summary>
        /// Creates a transaction on the system
        /// </summary>
        /// <param name="transaction">The transaction on the system</param>
        /// <param name="targetGoalId">The goal Id to which the transaction should be posted</param>
        /// <returns>The transaction that was created</returns>
        [SqlResource("Transaction/Create")]
        public Model.Transaction Create(Model.Transaction transaction, int? targetGoalId)
        {
            //Set the parameters and
            string sql = LoadSqlResources().First().Value;
            Dictionary<string, object> parameters = ConvertToParameters(transaction);
            parameters.Add("TargetGoalId", targetGoalId);
            
            return typedDataAccess.ExecuteQuery<Model.Transaction>(sql, parameters).First();
        }

        /// <summary>
        /// Returns the transaction by the ID
        /// </summary>
        /// <param name="id">The ID of the transaction</param>
        /// <returns>The transaction with the given ID</returns>
        [SqlResource("Transaction/GetById")]
        public Model.Transaction GetById(int id)
        {
            string sql = LoadSqlResources().First().Value;
            var parameters = ConvertToParameters(new { Id = id });

            return typedDataAccess
                .ExecuteQuery<Model.Transaction>(sql, parameters)
                .FirstOrDefault();
        }

        /// <summary>
        /// Returns all transactions for a given goal account
        /// </summary>
        /// <param name="goalId">The ID of the goal</param>
        /// <returns>All transactions for the goal</returns>
        [SqlResource("Transaction/GetTransactionsForGoal")]
        public IEnumerable<Model.Transaction> GetTransactionsForGoal(int goalId)
        {
            string sql = LoadSqlResources().First().Value;
            var parameters = ConvertToParameters(new { GoalId = goalId });

            return typedDataAccess.ExecuteQuery<Model.Transaction>(sql, parameters);
        }

        /// <summary>
        /// Returns all transactions for a given user
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <returns>All transactions for the user</returns>
        [SqlResource("Transaction/GetTransactionsForUser")]
        public IEnumerable<Model.Transaction> GetTransactionsForUser(int userId)
        {
            string sql = LoadSqlResources().First().Value;
            var parameters = ConvertToParameters(new { UserId = userId });

            return typedDataAccess.ExecuteQuery<Model.Transaction>(sql, parameters);
        }
    }
}