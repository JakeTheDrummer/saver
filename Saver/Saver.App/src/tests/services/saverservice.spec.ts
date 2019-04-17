import { expect } from 'chai';
import { Goal } from '../../../src/model/goal';
import { SaverService } from '../../../src/services/saver';

//Unit testing the saver service
describe('Unit Testing the Saver Service', () => {

  const BASE_URI: string = 'http://somebaseuri/api';
  it('Should create a saver service with the correct base API', () => {

    //Act
    let service = new SaverService(BASE_URI);

    //Assert
    expect(service).not.to.be(null, 'The service was not created');
    expect(service.baseUri).to.equal(BASE_URI, 'The service was not created with the correct base URI');
  });

  it('Should return a collection of Goals for a User', () => {

    //Arrange
    let userId: number = 1;
    let service = new SaverService(BASE_URI);

    //Act
    let testGoals: Goal[] = service.getGoalsFor(userId);
    

    //Assert
    expect(testGoals.length).to.equal(5, 'Not all goals were returned');
  });
});
