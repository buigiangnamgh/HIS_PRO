import { NgModule } from '@angular/core';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { RaVienComponent } from './ravien.component';
import { RaVienRoutingModule } from './ravien-routing.module';
import { RaVienService } from './ravien.service';
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
    RaVienRoutingModule,
    TranslateModule,
    CoreModule,
    NgxPaginationModule,
  ],
  declarations: [ RaVienComponent ],
  providers: [RaVienService]
})
export class RaVienModule { }
