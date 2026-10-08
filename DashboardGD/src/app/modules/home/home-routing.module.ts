import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { TrangChuComponent } from './home.component';

const routes: Routes = [
  {
    path: '',
    component: TrangChuComponent,
    data: {
      title: 'Trang chủ'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TrangChuRoutingModule {}
