import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { sieuamComponent } from './sieuam.component';

const routes: Routes = [
  {
    path: '',
    component: sieuamComponent,
    data: {
      title: 'Trang chủ'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class sieuamRoutingModule {}
