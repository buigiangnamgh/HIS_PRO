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
export class KhamCapCuuService {    
    constructor(private webApiClientService: WebApiClientService) {        
    }    
	loadData(search: SearchReport, page: number, paramData: ParamData): Observable<reportDatas> {
        let filter = '{"CREATE_TIME_FROM":'+search.CREATE_TIME_FROM+',"CREATE_TIME_TO":'+search.CREATE_TIME_TO+',"INTRUCTION_TIME_FROM":null,"INTRUCTION_TIME_TO":null,"RoomCodes":[]}';        
        const param = '{ "ApiData":  { "ReportTypeCode": "TKB00021","ReportTemplateCode":"TKB0002101_1", "Filter": ' + filter + '}, "CommonParam": {} }';
		console.log(param);
        return this.webApiClientService.post<reportDatas>(AppConfig.domain.dbd, AppConfig.endpoints.DbdReportCreateSdoApi, param);
    }
    
}