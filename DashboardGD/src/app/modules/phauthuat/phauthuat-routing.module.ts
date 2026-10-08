import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { phauthuatComponent } from './phauthuat.component';

const routes: Routes = [
  {
    path: '',
    component: phauthuatComponent,
    data: {
      title: 'Trang chủ'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class phauthuatRoutingModule {}
