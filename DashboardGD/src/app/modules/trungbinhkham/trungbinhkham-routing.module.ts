import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { trungbinhkhamComponent } from './trungbinhkham.component';

const routes: Routes = [
  {
    path: '',
    component: trungbinhkhamComponent,
    data: {
      title: 'Trang chủ'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class trungbinhkhamRoutingModule {}
