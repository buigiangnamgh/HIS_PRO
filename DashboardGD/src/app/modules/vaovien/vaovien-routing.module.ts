import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { VaoVienComponent } from './vaovien.component';

const routes: Routes = [
  {
    path: '',
    component: VaoVienComponent,
    data: {
      title: 'Vào viện'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class VaoVienRoutingModule {}
