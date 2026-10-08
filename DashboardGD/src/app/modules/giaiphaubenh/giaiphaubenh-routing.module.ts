import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { giaiphaubenhComponent } from './giaiphaubenh.component';

const routes: Routes = [
  {
    path: '',
    component: giaiphaubenhComponent,
    data: {
      title: 'Trang chủ'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class giaiphaubenhRoutingModule {}
