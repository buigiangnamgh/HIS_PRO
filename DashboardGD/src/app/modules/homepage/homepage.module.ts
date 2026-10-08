import { NgModule } from '@angular/core';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { HomePageComponent } from './homepage.component';
import { HomePageRoutingModule } from './homepage-routing.module';
import { HomePageService } from './homepage.service';
import { CoreModule } from '../../core/core.module';
import { TranslateModule } from 'ng2-translate';
import {NgxPaginationModule} from 'ngx-pagination'; // <-- import the module
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { ChartModule } from '@syncfusion/ej2-angular-charts';

import { ChartAllModule, AccumulationChartAllModule, RangeNavigatorAllModule } from '@syncfusion/ej2-angular-charts';

// import { DropDownListAllModule } from '@syncfusion/ej2-angular-dropdowns';

import { DatePickerModule } from '@syncfusion/ej2-angular-calendars';

import { NumericTextBoxModule } from '@syncfusion/ej2-angular-inputs';

import { ButtonModule } from '@syncfusion/ej2-angular-buttons';


@NgModule({
  imports: [
    ReactiveFormsModule,
	  BsDatepickerModule.forRoot(),
    FormsModule,
    CommonModule,
    BsDropdownModule,
    HomePageRoutingModule,
    TranslateModule,
    CoreModule,
    NgxPaginationModule,
	ButtonsModule.forRoot(),
	//ChartModule,
	ChartAllModule, RangeNavigatorAllModule,  ButtonModule, AccumulationChartAllModule, NumericTextBoxModule, DatePickerModule
  ],
  declarations: [ HomePageComponent ],
  providers: [HomePageService]
})
export class HomePageModule { }
