import { String, StringBuilder } from 'typescript-string-operations';
import * as $ from "jquery";
import { Goal } from '../model/goal';
import { ISaverService } from './saverserviceinterface';

/**
 * Provides access to the saver service*/
export class SaverService implements ISaverService {
  baseURI: string;

  //Provides a collection of API endpoints for the API
  private methodEndpoints: { [key: string]: string; } = {
    'getGoalsForUser': 'users/{userId}/goals'
  };

  /**
    * Creates a new saver service client with the given
    * endpoint as the communication base
    * @param baseAddressUri The base address of the service
    */
  constructor(baseAddressUri: string) {
    this.baseURI = baseAddressUri;
  }
  /**
    * Returns the base URI of this saver service proxy
    * */
  public baseUri(): string {
    return this.baseURI;
  }

  /**
    * Returns the goals for a given user
    * @param userId The ID of the user that has the goals
    */
  public getGoalsFor(userId: number) : Goal[] {

    //Collect the endpoint
    var params = { "userId": userId };
    var endpoint = this.getEndpoint("getGoalsFor", params);

    var goals = this.get<Goal[]>(endpoint);
    return goals;
  }

  /**
    * Returns data of the given TResult type from the endpoint
    * @param endpoint The endpoint from which we should load data
    */
  private get<TResult>(endpoint: string) {
    var result: TResult;
    $.getJSON(endpoint, res => {
      result = <TResult>res;
    });

    return result;
  }

  /**
    * Returns the API endpoint that should be called with the
    * params for the given method name
    * @param methodName The method name that we are calling from
    * @param params The collection of parameters for the query
    */
  private getEndpoint(methodName, params) {

    //Parameterise and return
    var methodEndpoint = this.methodEndpoints[methodName];
    methodEndpoint = this.parameterise(methodEndpoint, params);
    return this.baseURI + "/" + methodEndpoint;
  }

  /**
    * Parameterises the endpoint that is given with the parameters
    * @param apiEndpoint The endpoint that should be parameterised
    * @param params The parameters that should be applied to the endpoint
    */
  private parameterise(apiEndpoint, params: string[]) {

    //Parameterise each of the elements
    apiEndpoint = String.Format(apiEndpoint, params);
    return apiEndpoint;
  }
}
