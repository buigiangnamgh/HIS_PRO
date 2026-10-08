import { NgModule } from "@angular/core";
import { BsDropdownModule } from "ngx-bootstrap/dropdown";
import { ButtonsModule } from "ngx-bootstrap/buttons";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";

import { sieuamComponent } from "./sieuam.component";
import { sieuamRoutingModule } from "./sieuam-routing.module";
import { sieuamService } from "./sieuam.service";
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
    sieuamRoutingModule,
    TranslateModule,
    CoreModule,
    NgxPaginationModule,
    ButtonsModule.forRoot(),
  ],
  declarations: [sieuamComponent],
  providers: [sieuamService],
})
export class sieuamModule {}
