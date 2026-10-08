import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ChiTietDoanhThuComponent } from './chitietdoanhthu.component';

const routes: Routes = [
  {
    path: '',
    component: ChiTietDoanhThuComponent,
    data: {
      title: 'Số liệu chi tiết doanh thu'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ChiTietDoanhThuRoutingModule {}
