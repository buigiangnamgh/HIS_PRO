import {InjectionToken} from '@angular/core';
import {IAppConfig} from './iapp.config';
export let APP_CONFIG = new InjectionToken('app.config');

export const AppConfig: IAppConfig = {
  routes: {
    default: 'home',
    error404: '404'
  },
  endpoints: {
    SarReport: '/api/SarReport/GetView',    
    MrsReportCreateApi:'/api/MrsReport/CreateData',
	DbdReportCreateApi:'/api/BdbReport/Create',
	DbdReportCreateSdoApi:'/api/BdbReport/CreateSdo',
	DbdReportReloadSdoApi:'/api/BdbReport/ReloadSdo',
	DbdReportRefreshApi:'/api/BdbReport/Refresh',
	MrsReportGetApi:'/api/MrsReport/IntegrateReport',
    AcsTokenLogin: '/api/Token/Login',
    AcsTokenLogout: '/api/Token/Logout',
    AcsTokenGetAuthenticated: '/api/Token/GetAuthenticated',
    AcsTokenChangePassword: '/api/AcsUser/ChangePassword',
    AcsTokenForgotPassword: '/api/Token/ForgotPassword',
    AcsTokenAuthorize:  '/api/AcsToken/Authorize',
  },
  pageSize: 10000,
  domain: {
	integrate: '',	
dbd: 'http://192.168.10.100:51433',
   acs: 'http://192.168.10.100:1401',
  },
  common:{
    AppLICATION_CODE: 'DAB',
	TimerInterval: 200000,//đơn vị giây, thời gian tự động tải dữ liệu
	IsAllowCustomDateRange: 1,
	IsAllowCustomDateRangeHomePage: 1,
  ListAllowReportTemplateCode: 'TKBJRTYE01,TKBJRTYE02,TKBJRTYE03,MRS0000201,MRS0000206,TKB0003601,MRS0055501,MRS0055509,TKB0002101,MRS0029601,TKB0006101'//'MRS0000201,MRS0000206,MRS0048204,MRS0055501,MRS0055509,TKB0001602,TKB0002101,MRS00012TKB0003601,TKB0006101',//
  }
};
