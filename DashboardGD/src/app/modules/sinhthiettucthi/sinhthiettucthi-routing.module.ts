import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { sinhthiettucthiComponent } from './sinhthiettucthi.component';

const routes: Routes = [
  {
    path: '',
    component: sinhthiettucthiComponent,
    data: {
      title: 'Trang chủ'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class sinhthiettucthiRoutingModule {}
