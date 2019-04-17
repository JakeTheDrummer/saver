import { expect } from 'chai';
import { } from 'jasmine';
import { Goal } from '../../../src/model/goal'

//Unit testing the model goal
describe('Goal Testing - Testing the client side model', ()=> {
  it('Should create a Goal and set values', () => {

    //Arrange
    let expectedId:number = 1;
    let expectedName:string = "Testing Goal";
    let expectedTarget:number = 150.00;

    //Act
    let testGoal = new Goal(expectedId, expectedName, expectedTarget);

    //Assert
    expect(testGoal.id).to.equal(expectedId, 'Goal did not correctly set the ID');
    expect(testGoal.name).to.equal(expectedName, 'Goal did not correctly set the Name');
    expect(testGoal.target).to.equal(expectedTarget, 'Goal did not correctly set the Target');
  });
});
