import { NgModule } from "@angular/core";
import { BsDropdownModule } from "ngx-bootstrap/dropdown";
import { ButtonsModule } from "ngx-bootstrap/buttons";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";

import { noisoiComponent } from "./noisoi.component";
import { noisoiRoutingModule } from "./noisoi-routing.module";
import { noisoiService } from "./noisoi.service";
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
    noisoiRoutingModule,
    TranslateModule,
    CoreModule,
    NgxPaginationModule,
    ButtonsModule.forRoot(),
  ],
  declarations: [noisoiComponent],
  providers: [noisoiService],
})
export class noisoiModule {}
