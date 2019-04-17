import { Transaction } from "./transaction";

/**
 *Represents a goal for a user
 * */
export class Goal {
    id: number;
    name: string;
    target: any;

  /**
   * Creates a new Goal
   * @param id The ID of the Goal
   * @param name The name of the Goal
   * @param target The target of the goal
   */
  constructor(id: number, name: string, target: number) {
    this.id = id;
    this.name = name;
    this.target = target;
  }

  //Returns the transactions
  public transactions:Transaction[];
}
