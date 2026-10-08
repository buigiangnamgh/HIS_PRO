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
export class ThuTienKhamService {    
    constructor(private webApiClientService: WebApiClientService) {        
    }    
	loadData(search: SearchReport, page: number, paramData: ParamData): Observable<reportDatas> {
        let filter = '{"TIME_FROM":'+search.CREATE_TIME_FROM+',"TIME_TO":'+search.CREATE_TIME_TO+',"SERVICE_TYPE_ID":1}';        
        const param = '{ "ApiData":  { "ReportTypeCode": "MRS00296","ReportTemplateCode":"MRS0029601", "Filter": ' + filter + '}, "CommonParam": {} }';
		console.log(param);
        return this.webApiClientService.post<reportDatas>(AppConfig.domain.mrs, AppConfig.endpoints.MrsReportCreateApi, param);
    }
    
}