import { NgModule } from "@angular/core";
import { BsDropdownModule } from "ngx-bootstrap/dropdown";
import { ButtonsModule } from "ngx-bootstrap/buttons";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";

import { xetnghiemvisinhComponent } from "./xetnghiemvisinh.component";
import { xetnghiemvisinhRoutingModule } from "./xetnghiemvisinh-routing.module";
import { xetnghiemvisinhService } from "./xetnghiemvisinh.service";
import { CoreModule } from "../../core/core.module";
import { TranslateModule } from "ng2-translate";
import { NgxPaginationModule } from "ngx-pagination"; // <-- import the module
import { BsDatepickerModule } from "ngx-bootstrap/datepicker";

@NgModule({
  imports: [
    ReactiveFormsModule,
    BsDatepickerModule.forRoot(),
    FormsModule,
    CommonModule,
    BsDropdownModule,
    xetnghiemvisinhRoutingModule,
    TranslateModule,
    CoreModule,
    NgxPaginationModule,
    ButtonsModule.forRoot(),
  ],
  declarations: [xetnghiemvisinhComponent],
  providers: [xetnghiemvisinhService],
})
export class xetnghiemvisinhModule {}
