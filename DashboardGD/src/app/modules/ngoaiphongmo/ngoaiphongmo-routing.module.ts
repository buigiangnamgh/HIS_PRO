import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ngoaiphongmoComponent } from './ngoaiphongmo.component';

const routes: Routes = [
  {
    path: '',
    component: ngoaiphongmoComponent,
    data: {
      title: 'Trang chủ'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ngoaiphongmoRoutingModule {}
