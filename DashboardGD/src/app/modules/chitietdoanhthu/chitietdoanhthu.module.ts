import { NgModule } from '@angular/core';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { ChiTietDoanhThuComponent } from './chitietdoanhthu.component';
import { ChiTietDoanhThuRoutingModule } from './chitietdoanhthu-routing.module';
import { ChiTietDoanhThuService } from './chitietdoanhthu.service';
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
    ChiTietDoanhThuRoutingModule,
    TranslateModule,
    CoreModule,
    NgxPaginationModule,
  ],
  declarations: [ ChiTietDoanhThuComponent ],
  providers: [ChiTietDoanhThuService]
})
export class ChiTietDoanhThuModule { }
