import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ForgotPasswordComponent } from './forgotpassword.component';

const routes: Routes = [
  {
    path: '',
    component: ForgotPasswordComponent,
    data: {
      title: 'Tìm lại mật khẩu'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ForgotPasswordRoutingModule {}
