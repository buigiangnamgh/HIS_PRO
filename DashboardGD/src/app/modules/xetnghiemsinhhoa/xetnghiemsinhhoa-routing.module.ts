import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { xetnghiemsinhhoaComponent } from './xetnghiemsinhhoa.component';

const routes: Routes = [
  {
    path: '',
    component: xetnghiemsinhhoaComponent,
    data: {
      title: 'Trang chủ'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class xetnghiemsinhhoaRoutingModule {}
