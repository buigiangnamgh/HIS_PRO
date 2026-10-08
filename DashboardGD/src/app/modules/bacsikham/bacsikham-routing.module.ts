import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { bacsikhamComponent } from './bacsikham.component';

const routes: Routes = [
  {
    path: '',
    component: bacsikhamComponent,
    data: {
      title: 'Thống kê khám bệnh'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class bacsikhamRoutingModule {}
