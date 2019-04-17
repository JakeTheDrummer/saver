import { Dashboard } from '../../../pages/dashboard/dashboard';
import { SaverService } from '../../../services/saverservice';
import { mock, instance, when, verify } from 'ts-mockito';
import { Nav } from 'ionic-angular';

//Unit testing the dashboard functionality
describe('Dashboard Testing', () => {

  let mockSaverService: SaverService = mock(SaverService);
  let mockNavController: Nav = mock(Nav);

  it('Should load expected user goals for user', () => {

    //Arrange
    let expectedUserId = 1;
    let dashboard = new Dashboard(instance(mockNavController), instance(mockSaverService));

    //Act
    dashboard.loadGoals();

    //Assert
    verify(mockSaverService.getGoalsFor(expectedUserId)).called();
  });
});
