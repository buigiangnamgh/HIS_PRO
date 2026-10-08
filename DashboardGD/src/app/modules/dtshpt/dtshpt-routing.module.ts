import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { dtshptComponent } from './dtshpt.component';

const routes: Routes = [
  {
    path: '',
    component: dtshptComponent,
    data: {
      title: 'Trang chủ'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class dtshptRoutingModule {}
