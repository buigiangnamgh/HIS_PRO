import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { KhamBenhComponent } from './khambenh.component';

const routes: Routes = [
  {
    path: '',
    component: KhamBenhComponent,
    data: {
      title: 'Thống kê khám bệnh'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class KhamBenhRoutingModule {}
