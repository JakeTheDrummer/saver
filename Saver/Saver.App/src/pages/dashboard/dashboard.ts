import { Component, Inject, OnInit } from '@angular/core';
import { Nav, NavController } from 'ionic-angular';
import { Goal } from '../../model/goal';
import { SaverService } from '../../services/saverservice';

@Component({
  selector: 'page-dashboard',
  templateUrl: 'dashboard.html'
})
export class Dashboard {
  goals: Goal[];

  constructor(public navController: Nav, private saverService: SaverService) {
  }
  
  /**
   * Loads the goals for the user of the application*/
  public loadGoals() : void {
    this.goals = this.saverService.getGoalsFor(1);
  }
  
  onLink(url: string) {
    window.open(url);
  }
}
