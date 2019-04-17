import { NgModule, ErrorHandler } from '@angular/core';
import { IonicApp, IonicModule, IonicErrorHandler } from 'ionic-angular';
import { MyApp } from './app.component';
import { Dashboard } from '../pages/dashboard/dashboard';
import { SaverService } from '../services/saverservice';
import { ISaverService } from '../services/saverserviceinterface';

@NgModule({
  declarations: [
    MyApp,
    Dashboard
  ],
  imports: [
    IonicModule.forRoot(MyApp)
  ],
  bootstrap: [IonicApp],
  entryComponents: [
    MyApp,
    Dashboard
  ],
  providers: [
    { provide: ErrorHandler, useClass: IonicErrorHandler },
    { provide: SaverService, useFactory: () => { new SaverService('http://localhost/saverservice/api') } }
  ]
})
export class AppModule {}
