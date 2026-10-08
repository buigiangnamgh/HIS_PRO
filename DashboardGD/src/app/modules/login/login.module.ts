import { NgModule } from '@angular/core';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, Validators, FormBuilder } from '@angular/forms';
import { LoginComponent } from './login.component';
import { LoginRoutingModule } from './login-routing.module';
import { CoreModule } from '../../core/core.module';
import { TranslateModule } from 'ng2-translate';
import { ProfileService } from '../login/login.service';

@NgModule({
  imports: [
    ReactiveFormsModule,
    FormsModule,
    CommonModule,
    BsDropdownModule,
    TranslateModule,
    LoginRoutingModule,
    CoreModule,
  ],
  declarations: [ LoginComponent ],
  providers: [ProfileService]
})
export class LoginModule { }
