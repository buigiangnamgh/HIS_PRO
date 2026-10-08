import { ModuleWithProviders, NgModule } from '@angular/core';
import {MaterialModule} from './material.module';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TranslateModule } from 'ng2-translate';
import { RequiredPipe } from './pipes/required.pipe';
import { NvlPipe } from './pipes/nvl.pipe';
import { PreLvlPipe } from './pipes/preLvl.pipe';
import { Gender } from './pipes/gender.pipe';
import { StatusPipe } from './pipes/status.pipe';
import { NotificationMobilePipe } from './pipes/notificationMobile.pipe';
import { OperatorPipe } from './pipes/operator.pipe';
import { NumberCardStatus } from './pipes/numberCardRequest.pipe';
import { GroupByPipe } from './pipes/groupByPipe ';
import { OrderPipe } from './pipes/orderPipe';

@NgModule({
    imports: [MaterialModule],
    declarations: [
        RequiredPipe,
        NvlPipe,
        PreLvlPipe,      
        Gender,
        StatusPipe,
		GroupByPipe,
        OrderPipe,
        // ICheckDirective,
        // ImageLoaded,
        // FocusDirective,
        // EqualToValidator,
        // NotEqualToValidator,
        // NumberValidatorDirective,
        // ChangeTypeRequiredDirecitve,
        // RequiredSelectValidatorDirective,
        // GtDateToDirective,
        // TrimDirective,
        // MinDirective,
        // MinDateNowDirective,
        // RadiusDirecitve,
        // MaxToDirecitve,      
        NotificationMobilePipe,
        OperatorPipe,
        NumberCardStatus,
    ],
    exports: [
        RequiredPipe,
        NvlPipe,
        PreLvlPipe,       
        Gender,
        StatusPipe,
		GroupByPipe,
        OrderPipe,
        // ICheckDirective,
        // ImageLoaded,
        // FocusDirective,
        // //FileSelectDirective,
        // EqualToValidator,
        // NotEqualToValidator,
        // NumberValidatorDirective,
        // ChangeTypeRequiredDirecitve,
        // GtDateToDirective,
        // TrimDirective,
        // MinDirective,
        // MinDateNowDirective,
        // RadiusDirecitve,
        // MaxToDirecitve,
        // RequiredSelectValidatorDirective,       
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        MaterialModule,
        //NgbModule,
        TranslateModule,
        NotificationMobilePipe,
        OperatorPipe,
        NumberCardStatus,
    ],
    providers: [NvlPipe]
})
export class SharedModule {
    static forRoot(): ModuleWithProviders {
      return {
        ngModule: SharedModule,
        providers: [
          
        ]
      };
    }
  }