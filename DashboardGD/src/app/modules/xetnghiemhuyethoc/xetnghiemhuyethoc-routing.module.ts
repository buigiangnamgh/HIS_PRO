import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { xetnghiemhuyethocComponent } from './xetnghiemhuyethoc.component';

const routes: Routes = [
  {
    path: '',
    component: xetnghiemhuyethocComponent,
    data: {
      title: 'Trang chủ'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class xetnghiemhuyethocRoutingModule {}
