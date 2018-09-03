using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saver.Model
{
    /// <summary>
    /// Represents a monetary transaction in and out of 
    /// a goal "account." If the source is empty, it's
    /// "new money"
    /// </summary>
    public class Transaction
    {
        /// <summary>
        /// Used by Dapper only
        /// </summary>
        public Transaction()
        {
        }

        /// <summary>
        /// Creates a new Transaction
        /// </summary>
        /// <param name="id">The ID of the transaction</param>
        /// <param name="amount">The value of the transaction</param>
        /// <param name="sourceGoalId">The source (if any) of the transaction</param>
        /// <param name="timestamp">The timestamp of the transaction</param>
        public Transaction(double amount, int? sourceGoalId, DateTime timestamp)
            : this(-1, amount, sourceGoalId, timestamp)
        {
        }

        /// <summary>
        /// Creates a new Transaction
        /// </summary>
        /// <param name="id">The ID of the transaction</param>
        /// <param name="amount">The value of the transaction</param>
        /// <param name="sourceGoalId">The source (if any) of the transaction</param>
        /// <param name="timestamp">The timestamp of the transaction</param>
        public Transaction(int id, double amount, int? sourceGoalId, DateTime timestamp)
        {
            Id = id;
            Amount = amount;
            SourceGoalId = sourceGoalId;
            Timestamp = timestamp;
        }

        /// <summary>
        /// Gets or Sets the ID of the transaction
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or Sets the Amount
        /// </summary>
        public double Amount { get; set; }

        /// <summary>
        /// Returns the ID of the goal from which this money originated
        /// </summary>
        public int? SourceGoalId { get; set; }

        /// <summary>
        /// Gets or Sets when this transaction occured
        /// </summary>
        public DateTime Timestamp { get; set; }
    }
}
