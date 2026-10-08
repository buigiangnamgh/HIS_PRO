import { NgModule } from '@angular/core';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, Validators, FormBuilder } from '@angular/forms';
import { ForgotPasswordComponent } from './forgotpassword.component';
import { ForgotPasswordRoutingModule } from './forgotpassword-routing.module';
import { CoreModule } from '../../core/core.module';
import { TranslateModule } from 'ng2-translate';

@NgModule({
  imports: [
    ReactiveFormsModule,
    FormsModule,
    CommonModule,
    BsDropdownModule,
    TranslateModule,
    ForgotPasswordRoutingModule,
    CoreModule,
  ],
  declarations: [ ForgotPasswordComponent ],
  providers: []
})
export class ForgotPasswordModule { }
