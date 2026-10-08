import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-dashboard',
  templateUrl: './empty-layout.component.html'
})
export class EmptyLayoutComponent {

  constructor(public router: Router) {    
  }  
}
