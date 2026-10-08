import {NgModule, Optional, SkipSelf} from '@angular/core';
import {CommonModule} from '@angular/common';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {throwIfAlreadyLoaded} from './module-import-guard';
import {LoggerService} from './log/logger.service';
import {WebApiClientService} from './webApiClient.service';
import {SharedModule} from '../shared/shared.module';
import {RouterModule} from '@angular/router';
import { HttpModule } from '@angular/http';
import { ExcelService } from './excel.service ';
//import { P404Component } from '../modules/error/404.component';
// import {NavComponent} from './nav/nav.component';
// import {FooterComponent} from './footer/footer.component';
// import {SearchBarComponent} from './search-bar/search-bar.component';
// import {ProgressBarService} from './shared/progress-bar.service';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    RouterModule,
    SharedModule,
    ReactiveFormsModule
  ],
  exports: [
    // NavComponent,
    // FooterComponent
  ],
  declarations: [
    // NavComponent,
    // FooterComponent,
    // SearchBarComponent,
    //P404Component
  ],
  providers: [
    LoggerService,
    WebApiClientService,
    ExcelService
  ]
})

export class CoreModule {
  constructor(@Optional() @SkipSelf() parentModule: CoreModule) {
    throwIfAlreadyLoaded(parentModule, 'CoreModule');
  }
}
