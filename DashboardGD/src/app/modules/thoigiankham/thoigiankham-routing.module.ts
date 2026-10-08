import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { thoigiankhamComponent } from './thoigiankham.component';

const routes: Routes = [
  {
    path: '',
    component: thoigiankhamComponent,
    data: {
      title: 'Thời gian chờ & thực hiện khám'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class thoigiankhamRoutingModule {}
