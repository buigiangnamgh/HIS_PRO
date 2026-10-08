import { BrowserModule } from '@angular/platform-browser';
import { NgModule, CUSTOM_ELEMENTS_SCHEMA  } from '@angular/core';
import { LocationStrategy, HashLocationStrategy, CommonModule } from '@angular/common';
import { HttpClientModule, HTTP_INTERCEPTORS, HttpClient } from '@angular/common/http';
import { HttpModule, BrowserXhr, Http, XHRBackend, RequestOptions } from '@angular/http';
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { PERFECT_SCROLLBAR_CONFIG } from 'ngx-perfect-scrollbar';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';
import { TranslateModule, TranslateService, TranslateLoader, TranslateStaticLoader } from 'ng2-translate';
import {NgxPaginationModule} from 'ngx-pagination'; // <-- import the module
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
const DEFAULT_PERFECT_SCROLLBAR_CONFIG: PerfectScrollbarConfigInterface = {
  suppressScrollX: true
};

import { AppComponent } from './app.component';
import { DefaultLayoutComponent } from './containers';
import { EmptyLayoutComponent } from './containers/empty-layout';
import { P404Component } from './modules/error/404.component';
import { P500Component } from './modules/error/500.component';
import {CustExtBrowserXhr} from './CustExtBrowserXhr';
import { AuthenService } from './authen/authen.service';
import { MenuService } from './containers/default-layout/menu.service';
import { WebApiClientService } from './core/webApiClient.service';
import { GridsterModule } from 'angular-gridster2';


const APP_CONTAINERS = [
  DefaultLayoutComponent,
  EmptyLayoutComponent
];

import {
  AppAsideModule,
  AppBreadcrumbModule,
  AppHeaderModule,
  AppFooterModule,
  AppSidebarModule,
} from '@coreui/angular'

// Import routing module
import { AppRoutingModule } from './app.routing';

// Import 3rd party components
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { TabsModule } from 'ngx-bootstrap/tabs';
// import { ChartsModule } from 'ng2-charts';
import { AuthInterceptor } from './auth.interceptor';
import { AuthGuard } from './authen/authen.guard';
import { FormsModule, ReactiveFormsModule, Validators, FormBuilder } from '@angular/forms';
import {APP_CONFIG, AppConfig} from './config/app.config';
import {SharedModule} from './shared/shared.module';
import { DoiMatKhauComponent } from './modules/doimatkhau/doimatkhau.dialog.component';
import { ChonNgayComponent } from './modules/chonngay/chonngay.dialog.component';
//import { TrangChuComponent } from './modules/home/home.component';
import { MatDialogModule } from '@angular/material/dialog';
import { MaterialModule } from './shared/material.module';
import { ExcelService } from './core/excel.service ';
import { PdfViewerModule, PdfViewerComponent } from 'ng2-pdf-viewer';
import { InterceptorService } from 'ng2-interceptors';

import { ToastrModule } from 'ngx-toastr';

import { LoadingBarHttpClientModule } from '@ngx-loading-bar/http-client';
 
// for Http import LoadingBarHttpModule:
// import { LoadingBarHttpModule } from '@ngx-loading-bar/http';
 
// for Router import LoadingBarRouterModule:
import { LoadingBarRouterModule } from '@ngx-loading-bar/router';
 
// for Core import LoadingBarModule:
import { LoadingBarModule, LoadingBarService } from '@ngx-loading-bar/core';
import { ModalModule } from 'ngx-bootstrap/modal';
import { AlertComponent } from './shared/alert/_directives/index';
import { AlertService } from './shared/alert/_services/index';
import { AngularMyDatePickerModule } from 'angular-mydatepicker';

// import { ChartComponent } from '@syncfusion/ej2-angular-charts';

@NgModule({
  imports: [
    BrowserModule,
    AppRoutingModule,
    AppAsideModule,
    AppBreadcrumbModule.forRoot(),
    AppFooterModule,
    AppHeaderModule,
    AppSidebarModule,
    PerfectScrollbarModule,
    BsDropdownModule.forRoot(),
    TabsModule.forRoot(),
    SharedModule.forRoot(),
    TranslateModule.forRoot({
      provide: TranslateLoader,
      useFactory: createTranslateLoader,
      deps: [Http]
    }),
    // ChartsModule,
	
    HttpClientModule,
    HttpModule,
    ReactiveFormsModule,
    FormsModule, 
    BrowserAnimationsModule,
    ToastrModule.forRoot(), // ToastrModule added
    MatDialogModule,
    MaterialModule,   
    NgxPaginationModule,
    ModalModule.forRoot(),
    LoadingBarModule,
    CommonModule,
    // for HttpClient use:
    // LoadingBarHttpModule,
 
    // for Router use:
    LoadingBarRouterModule,
 
    // for HttpClient use:
     LoadingBarHttpClientModule,
	 AngularMyDatePickerModule ,
	 GridsterModule,
	 // NgxSpinnerModule,
    // for Core use:
    //LoadingBarHttpClientModule.forRoot()

  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  declarations: [
    AppComponent,
    ...APP_CONTAINERS,
    P404Component,
    P500Component,
    DoiMatKhauComponent, 
	ChonNgayComponent,
    AlertComponent,
	
  ],
  entryComponents: [
    DoiMatKhauComponent,
	ChonNgayComponent
  ],
  providers: [
    {provide: LocationStrategy,
      useClass: HashLocationStrategy,    
    },
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    {provide: APP_CONFIG, useValue: AppConfig},
    LoadingBarService,
    AuthenService,
    MenuService,
    WebApiClientService,
    AlertService,
    ExcelService,
    AuthGuard,
    {provide: BrowserXhr, useClass:CustExtBrowserXhr},
    {
      provide: InterceptorService,
      useFactory: interceptorFactory,
      deps: [XHRBackend, RequestOptions]
    },	
    HttpClientModule],
  bootstrap: [ AppComponent ,
  ]
})
export class AppModule { }

export function createTranslateLoader(http: Http) {
  return new TranslateStaticLoader(http, 'assets/i18n', '.json');
}

export function interceptorFactory(xhrBackend: XHRBackend, requestOptions: RequestOptions){
  let service = new InterceptorService(xhrBackend, requestOptions);
  // Add interceptors here with service.addInterceptor(interceptor)
  return service;
}
