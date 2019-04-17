
/**
 * Represents a transaction in or out of a goal account*/
export class Transaction {

  id: number;
  amount: number;
  sourcegoal: number;
  timestamp: Date;

  /**
   * Creates a new Transaction
   * @param id The ID of the transaction
   * @param amount The amount of the transaction
   * @param sourcegoal The source of the transaction
   * @param timestamp The timestamp of the transaction
   */
  constructor(id: number, amount: number, sourcegoal: number, timestamp: Date) {
    this.id = id;
    this.amount = amount;
    this.sourcegoal = sourcegoal;
    this.timestamp = timestamp;
  }
}
