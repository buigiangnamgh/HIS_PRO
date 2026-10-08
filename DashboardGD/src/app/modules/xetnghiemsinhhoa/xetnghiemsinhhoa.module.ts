import { NgModule } from "@angular/core";
import { BsDropdownModule } from "ngx-bootstrap/dropdown";
import { ButtonsModule } from "ngx-bootstrap/buttons";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";

import { xetnghiemsinhhoaComponent } from "./xetnghiemsinhhoa.component";
import { xetnghiemsinhhoaRoutingModule } from "./xetnghiemsinhhoa-routing.module";
import { xetnghiemsinhhoaService } from "./xetnghiemsinhhoa.service";
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
    xetnghiemsinhhoaRoutingModule,
    TranslateModule,
    CoreModule,
    NgxPaginationModule,
    ButtonsModule.forRoot(),
  ],
  declarations: [xetnghiemsinhhoaComponent],
  providers: [xetnghiemsinhhoaService],
})
export class xetnghiemsinhhoaModule {}
