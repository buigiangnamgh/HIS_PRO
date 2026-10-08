import { ProfileData } from '../../model/danhsachbaocao.model';
import {Observable, of, throwError as observableThrowError} from 'rxjs';
import {AppConfig} from '../../config/app.config';
import { Injectable } from '@angular/core';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/catch';
import {WebApiClientService} from '../../core/webApiClient.service';
import { String, StringBuilder } from 'typescript-string-operations';

@Injectable()
export class ProfileService {    
    constructor(private webApiClientService: WebApiClientService) {        
    }

    loadProfile(): Observable<ProfileData> {     
        const mathe = localStorage.getItem('mathe');
        const macanhan = localStorage.getItem('macanhan');  
        let param = "";
        if (macanhan !== undefined && macanhan != null && macanhan != ''){
            param = String.Format('{   "ApiData": \'{0}\' }', macanhan);
        }
        else if (mathe !== undefined && mathe == null || mathe == ''){
            param = String.Format('{   "ApiData": \'{0}\' }', mathe);
        }            
        
        return this.webApiClientService.get<ProfileData>(AppConfig.domain.hid, AppConfig.endpoints.HidPersonGetByPersonCode, param);
    }
    loadProfileByCode(personCode:string): Observable<ProfileData> {     
        let param = String.Format('{   "ApiData": \'{0}\' }', personCode); 
        return this.webApiClientService.get<ProfileData>(AppConfig.domain.hid, AppConfig.endpoints.HidPersonGetByPersonCode, param);
    }

    //   async getAsync2C() {
    //     const mathe = localStorage.getItem('mathe');
    //     const macanhan = localStorage.getItem('macanhan');
    //     let param = String.Format('{   "ApiData": \'{0}\' }', mathe);
    //     if (mathe == null || mathe == ''){
    //         param = String.Format('{   "ApiData": \'{0}\' }', macanhan);
    //     }
    //     this.profileData = await this.webApiClientService.getAsync2(AppConfig.domain.hid, AppConfig.endpoints.HidPersonGetByPersonCode, param);   
    //     console.log(this.profileData);
    // }
}