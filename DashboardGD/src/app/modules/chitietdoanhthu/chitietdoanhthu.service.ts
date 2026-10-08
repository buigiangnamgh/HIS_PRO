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
export class ChiTietDoanhThuService {    
    constructor(private webApiClientService: WebApiClientService) {        
    }    
	loadData(search: SearchReport, page: number, paramData: ParamData): Observable<reportDatas> {
		let other = '';
		if(search.ReportTemplateCode=="MRS00012TKB0003601")
			other = ',"IS_TREAT_IN":null, "CHOOSE_TIME":true';
        let filter = '{"CREATE_TIME_FROM":'+search.CREATE_TIME_FROM+',"CREATE_TIME_TO":'+search.CREATE_TIME_TO+',"TIME_FROM":'+search.TIME_FROM+',"TIME_TO":'+search.TIME_TO + other +'}';        
        const param = '{ "ApiData":  { "ReportTypeCode": "'+search.ReportTypeCode+'","ReportTemplateCode":"'+search.ReportTemplateCode+'", "Filter": ' + filter + '}, "CommonParam": {} }';
		console.log(param);
        return this.webApiClientService.post<reportDatas>(AppConfig.domain.dbd, AppConfig.endpoints.DbdReportCreateSdoApi, param);
    }
    
}