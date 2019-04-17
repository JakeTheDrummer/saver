import { Goal } from "../model/goal";

/**
 * Represents the saver service
 * */
export interface ISaverService {
  /*
   * Returns the base URI
   */
  baseUri() : string;

  /**
   * Returns the goals for a given user
   * @param userId The ID of the user that has the goals
   */
  getGoalsFor(userId: number): Goal[];
}
