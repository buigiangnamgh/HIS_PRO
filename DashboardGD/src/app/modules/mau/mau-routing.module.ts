import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { mauComponent } from './mau.component';

const routes: Routes = [
  {
    path: '',
    component: mauComponent,
    data: {
      title: 'Trang chủ'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class mauRoutingModule {}
