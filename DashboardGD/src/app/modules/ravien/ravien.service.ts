import {Observable, of, throwError as observableThrowError} from 'rxjs';
import {AppConfig} from '../../config/app.config';
import { Injectable } from '@angular/core';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/catch';
import {WebApiClientService} from '../../core/webApiClient.service';
import { String, StringBuilder } from 'typescript-string-operations';
import { reportDatas } from "../../model/report";
import { SearchReport } from '../../model/Search.model';
import { ParamData } from "../../model/apidata.model";

@Injectable()
export class RaVienService {    
    constructor(private webApiClientService: WebApiClientService) {        
    }    
	loadData(search: SearchReport, page: number, paramData: ParamData): Observable<reportDatas> {
        let filter = '{"TIME_FROM":'+search.TIME_FROM+',"TIME_TO":'+search.TIME_TO+',"IS_TREAT_IN":true,"INTRUCTION_TIME_FROM":null,"INTRUCTION_TIME_TO":null,"RoomCodes":[]}';        
        const param = '{ "ApiData":  { "ReportTypeCode": "MRS00555","ReportTemplateCode":"MRS0055509_1", "Filter": ' + filter + ', "ListKeyAllow":"Report"}, "CommonParam": {} }';
		console.log(param);
        return this.webApiClientService.post<reportDatas>(AppConfig.domain.dbd, AppConfig.endpoints.DbdReportCreateSdoApi, param);
    }
    
}