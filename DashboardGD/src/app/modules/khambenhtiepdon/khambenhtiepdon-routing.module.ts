import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { khambenhtiepdonComponent } from './khambenhtiepdon.component';

const routes: Routes = [
  {
    path: '',
    component: khambenhtiepdonComponent,
    data: {
      title: 'Thống kê khám bệnh'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class khambenhtiepdonRoutingModule {}
