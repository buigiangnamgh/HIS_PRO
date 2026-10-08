import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { RaVienComponent } from './ravien.component';

const routes: Routes = [
  {
    path: '',
    component: RaVienComponent,
    data: {
      title: 'Ra viện'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RaVienRoutingModule {}
