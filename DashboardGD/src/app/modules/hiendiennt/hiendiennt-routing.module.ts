import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { hiendienntComponent } from './hiendiennt.component';

const routes: Routes = [
  {
    path: '',
    component: hiendienntComponent,
    data: {
      title: 'Ra viện'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class hiendienntRoutingModule {}
