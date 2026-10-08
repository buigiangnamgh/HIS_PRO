import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ThuTienKhamComponent } from './thutienkham.component';

const routes: Routes = [
  {
    path: '',
    component: ThuTienKhamComponent,
    data: {
      title: 'Danh sách thu tiền khám'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ThuTienKhamRoutingModule {}
