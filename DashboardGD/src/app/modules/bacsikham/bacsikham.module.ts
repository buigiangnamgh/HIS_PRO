import { NgModule } from '@angular/core';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { bacsikhamComponent } from './bacsikham.component';
import { bacsikhamRoutingModule } from './bacsikham-routing.module';
import { bacsikhamService } from './bacsikham.service';
import { CoreModule } from '../../core/core.module';
import { TranslateModule } from 'ng2-translate';
import {NgxPaginationModule} from 'ngx-pagination'; // <-- import the module
import { LoadingBarModule } from '@ngx-loading-bar/core';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
@NgModule({
  imports: [
    ReactiveFormsModule,
    BsDatepickerModule.forRoot(),
    FormsModule,
    CommonModule,
    BsDropdownModule,
    LoadingBarModule,
    bacsikhamRoutingModule,
    TranslateModule,
    CoreModule,
    NgxPaginationModule,
  ],
  declarations: [ bacsikhamComponent ],
  providers: [bacsikhamService]
})
export class bacsikhamModule { }
