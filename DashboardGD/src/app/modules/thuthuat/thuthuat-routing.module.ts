import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { thuthuatComponent } from './thuthuat.component';

const routes: Routes = [
  {
    path: '',
    component: thuthuatComponent,
    data: {
      title: 'Trang chủ'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class thuthuatRoutingModule {}
