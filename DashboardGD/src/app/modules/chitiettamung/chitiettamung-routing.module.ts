import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ChiTietTamUngComponent } from './chitiettamung.component';

const routes: Routes = [
  {
    path: '',
    component: ChiTietTamUngComponent,
    data: {
      title: 'Số liệu chi tiết tạm ứng'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ChiTietTamUngRoutingModule {}
