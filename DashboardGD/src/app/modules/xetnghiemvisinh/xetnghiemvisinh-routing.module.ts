import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { xetnghiemvisinhComponent } from './xetnghiemvisinh.component';

const routes: Routes = [
  {
    path: '',
    component: xetnghiemvisinhComponent,
    data: {
      title: 'Trang chủ'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class xetnghiemvisinhRoutingModule {}
