import { Injectable } from '@angular/core';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/catch';
import {Observable, of, throwError as observableThrowError} from 'rxjs';
import { AutData } from '../model/authData.model';
import { ApiData, ParamData } from '../model/apidata.model';
import {WebApiClientService} from '../core/webApiClient.service';
import {AppConfig} from '../config/app.config';
import { ChangePasswordADO } from '../model/changePassword.model';
import { ApiParam } from '../model/apiParam';

@Injectable()
export class AuthenService {
    changePasswordADO: ChangePasswordADO = new ChangePasswordADO();
    apiParam:ApiParam = new ApiParam();
    constructor(private webApiClientService: WebApiClientService) { }

    requestAuthInfo(): Observable<ApiData> {
          return this.webApiClientService.get<ApiData>(AppConfig.domain.acs, AppConfig.endpoints.AcsTokenGetAuthenticated, '');
    }

    login(username, password): Observable<AutData> {
        const applicationCode = AppConfig.common.AppLICATION_CODE;
        const authdata = btoa(unescape(encodeURIComponent(applicationCode + ':' + username + ':' + password)));// btoa(applicationCode + ':' + username + ':' + password);
        localStorage.setItem('loginAuthData', authdata);    
        return this.webApiClientService.get<AutData>(AppConfig.domain.acs, AppConfig.endpoints.AcsTokenLogin, "");
    }

    logout(token): Observable<ApiData> {       
        return this.webApiClientService.post<ApiData>(AppConfig.domain.acs, AppConfig.endpoints.AcsTokenLogout,"");
    }

    forgotPassword(forgotPassword): Observable<ApiData> {
        let data = '';    
        if(forgotPassword != null)
        data = '{"email":"' + forgotPassword.email + '"}';    
        return this.webApiClientService.post<ApiData>(AppConfig.domain.acs, AppConfig.endpoints.AcsTokenForgotPassword, data);
    }

    changePassword(changepassword): Observable<ApiData> {
        //let data = '';  
        let tokenData;
        const strTokenData = localStorage.getItem('tokendata');
        if(strTokenData != null)
        tokenData = JSON.parse(strTokenData);
        if(changepassword != null)
        {
            this.changePasswordADO = new ChangePasswordADO();
            this.changePasswordADO.LOGIN_NAME = tokenData.User.LoginName;
            this.changePasswordADO.PASSWORD__OLD = changepassword.oldpassword;
            this.changePasswordADO.PASSWORD__NEW = changepassword.newpassword;
        }
        this.apiParam.ApiData = this.changePasswordADO;
        this.apiParam.CommonParam = new ParamData();
        return this.webApiClientService.post<ApiData>(AppConfig.domain.acs, AppConfig.endpoints.AcsTokenChangePassword, this.apiParam);
    }
}