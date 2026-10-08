import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { noisoiComponent } from './noisoi.component';

const routes: Routes = [
  {
    path: '',
    component: noisoiComponent,
    data: {
      title: 'Trang chủ'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class noisoiRoutingModule {}
