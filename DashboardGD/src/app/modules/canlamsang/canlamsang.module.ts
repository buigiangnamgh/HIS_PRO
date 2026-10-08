import { NgModule } from "@angular/core";
import { BsDropdownModule } from "ngx-bootstrap/dropdown";
import { ButtonsModule } from "ngx-bootstrap/buttons";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";

import { canlamsangComponent } from "./canlamsang.component";
import { canlamsangRoutingModule } from "./canlamsang-routing.module";
import { canlamsangService } from "./canlamsang.service";
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
    canlamsangRoutingModule,
    TranslateModule,
    CoreModule,
    NgxPaginationModule,
    ButtonsModule.forRoot(),
  ],
  declarations: [canlamsangComponent],
  providers: [canlamsangService],
})
export class canlamsangModule {}
