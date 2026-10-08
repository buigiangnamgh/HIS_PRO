import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { DangNamVienComponent } from './dangnamvien.component';

const routes: Routes = [
  {
    path: '',
    component: DangNamVienComponent,
    data: {
      title: 'Đang nằm viện (nội trú)'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DangNamVienRoutingModule {}
