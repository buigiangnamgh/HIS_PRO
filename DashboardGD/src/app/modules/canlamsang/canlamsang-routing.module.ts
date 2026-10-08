import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { canlamsangComponent } from './canlamsang.component';

const routes: Routes = [
  {
    path: '',
    component: canlamsangComponent,
    data: {
      title: 'Trang chủ'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class canlamsangRoutingModule {}
