import { SearchReport, SearchRangeDate } from '../../model/Search.model';
import {Observable, of, throwError as observableThrowError} from 'rxjs';
import {AppConfig} from '../../config/app.config';
import { Injectable } from '@angular/core';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/catch';
import {WebApiClientService} from '../../core/webApiClient.service';
import { String, StringBuilder } from 'typescript-string-operations';
import { ParamData } from "../../model/apidata.model";
import { reportDatas } from "../../model/report";

@Injectable()
export class KhamBenhService {    
    constructor(private webApiClientService: WebApiClientService) {        
    }    
	loadkhambenh(search: SearchReport, page: number, paramData: ParamData): Observable<reportDatas> {
        let filter = '{"INTRUCTION_TIME_FROM":'+search.CREATE_TIME_FROM+',"INTRUCTION_TIME_TO":'+search.CREATE_TIME_TO+',"IS_INTRUCTION_TIME_OR_FINISH_TIME":false}';        
        const param = '{ "ApiData":  { "ReportTypeCode": "MRS00002","ReportTemplateCode":"MRS0000201_1", "Filter": ' + filter + '}, "CommonParam": {} }';
        return this.webApiClientService.post<reportDatas>(AppConfig.domain.dbd, AppConfig.endpoints.DbdReportCreateSdoApi, param);
    }
    
}