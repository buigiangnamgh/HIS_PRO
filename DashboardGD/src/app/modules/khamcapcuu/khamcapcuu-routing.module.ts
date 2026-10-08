import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { KhamCapCuuComponent } from './khamcapcuu.component';

const routes: Routes = [
  {
    path: '',
    component: KhamCapCuuComponent,
    data: {
      title: 'Thống kê khám cấp cứu'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class KhamCapCuuRoutingModule {}
