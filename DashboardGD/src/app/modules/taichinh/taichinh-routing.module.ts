import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { TaiChinhComponent } from './taichinh.component';

const routes: Routes = [
  {
    path: '',
    component: TaiChinhComponent,
    data: {
      title: 'Số liệu tài chính'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TaiChinhRoutingModule {}
