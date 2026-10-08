import { ProfileData } from '../../model/danhsachbaocao.model';
import { reportDatas } from "../../model/report";
import { ReportIntegrate } from "../../model/ReportIntegrate";
import { SearchReport } from '../../model/Search.model';
import { ParamData } from "../../model/apidata.model";
import {Observable, of, throwError as observableThrowError} from 'rxjs';
import {AppConfig} from '../../config/app.config';
import { Injectable } from '@angular/core';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/catch';
import {WebApiClientService} from '../../core/webApiClient.service';
import { String, StringBuilder } from 'typescript-string-operations';

@Injectable()
export class HomePageService {    
    constructor(private webApiClientService: WebApiClientService) {        
    }
    	
		
	GetCrosshairData(): any {
        let series1: Object[] = [];
        let series2: Object[] = [];
        let point1: Object;
        let point2: Object;
        let value: number = 60;
        let value1: number = 50;
        let i: number;
        for (i = 1; i < 250; i++) {

            if (Math.random() > .5) {
                value += Math.random();
                value1 += Math.random();
            } else {
                value -= Math.random();
                value1 -= Math.random();
            }
            point1 = { x: new Date(2000, i, 1), y: value };
            point2 = { x: new Date(2000, i, 1), y: value1 };
            series1.push(point1);
            series2.push(point2);
        }
        return { 'series1': series1, 'series2': series2 };
    }
    GetScatterData(): any {
        let series1: Object[] = [];
        let series2: Object[] = [];
        let point1: Object;
        let value: number = 80;
        let value1: number = 70;
        let i: number;
        for (i = 1; i < 120; i++) {
            if (Math.random() > 0.5) {
                value += Math.random();
            } else {
                value -= Math.random();
            }
            value = value < 60 ? 60 : value > 90 ? 90 : value;
            point1 = { x: (145 + (i / 3)).toFixed(1), y: value.toFixed(1) };
            series1.push(point1);
        }
        for (i = 1; i < 120; i++) {
            if (Math.random() > 0.5) {
                value1 += Math.random();
            } else {
                value1 -= Math.random();
            }
            value1 = value1 < 60 ? 60 : value1 > 90 ? 90 : value1;
            point1 = { x: (145 + (i / 3)).toFixed(1), y: value1.toFixed(1) };
            series2.push(point1);
        }
        return { 'series1': series1, 'series2': series2 };
    }
    GetLocalData(): any {
        let series1: Object[] = [];
        let series2: Object[] = [];
        let point1: Object;
        let point2: Object;
        let value: number = 80;
        let value1: number = 90;
        let i: number;
        for (i = 1; i < 500; i++) {

            if (Math.random() > .5) {
                value += Math.random();
                value1 += Math.random();
            } else {
                value -= Math.random();
                value1 -= Math.random();
            }
            point1 = { x: new Date(1960, (i + 1), i), y: Math.round(value) };
            point2 = { x: new Date(1960, (i + 1), i), y: Math.round(value1) };
            series1.push(point1);
            series2.push(point2);
        }
        return { 'series1': series1, 'series2': series2 };
    }
    GetZoomingData(): any {
        let series1: Object[] = [];
        let point1: Object;
        let value: number = 80;
        let i: number;
        for (i = 1; i < 500; i++) {
            if (Math.random() > .5) {
                value += Math.random();
            } else {
                value -= Math.random();
            }
            point1 = { x: new Date(1950, i + 2, i), y: value.toFixed(1) };
            series1.push(point1);
        }
        return { 'series1': series1 };
    }
    GetPolarSplineData(): any {
        let cardData: Object[] = [];
        let biDirData: Object[] = [];
        let omniDirData: Object[] = [];
        let point1: Object;
        let point2: Object;
    
        for (let x: number = -180; x < 180; x++) {
          point1 = { x: x, y: -12.6 * (1 - Math.cos(x * 3.14 / 180)) };
          cardData.push(point1);
          point2 = { x: x, y: -3};
          omniDirData.push(point2);
        }
    
        for (let x: number = -180; x < -90; x++) {
          point1 = { x: x, y: -26 * (1 + Math.cos(x * 3.14 / 180))};
          biDirData.push(point1);
        }
    
        for (let x: number = -90; x < 90; x++) {
          point1 = { x: x, y: -26 * (1 - Math.cos(x * 3.14 / 180))};
          biDirData.push(point1);
        }
    
        for (let x: number = 90; x < 180; x++) {
          point1 = { x: x, y: -26 * (1 + Math.cos(x * 3.14 / 180))};
          biDirData.push(point1);
        }
        return { 'series1': cardData, 'series2': omniDirData, 'series3': biDirData };
    }	
		
		
		
	getParamDateRangeIntegrateSystem(search: SearchReport): string
	{
		let filter = '/' + search.TIME_FROM.substr(6, 2) + '-' + search.TIME_FROM.substr(4, 2) + '-' + search.TIME_FROM.substr(0, 4) + '/' + search.TIME_TO.substr(6, 2) + '-' + search.TIME_TO.substr(4, 2) + '-' + search.TIME_TO.substr(0, 4);
				
		return filter;
	}		
		
	loadDataReportIntegrate(search: SearchReport): Observable<reportDatas> {
		let filter ='';
		if(search.TIME_FROM!=undefined && search.TIME_FROM!=null && search.TIME_FROM != '' && search.TIME_TO!=undefined && search.TIME_TO!=null && search.TIME_TO != '')
		    filter = ',"FromTime":"' + search.TIME_FROM.substr(6, 2) + '-' + search.TIME_FROM.substr(4, 2) + '-' + search.TIME_FROM.substr(0, 4) + '","ToTime":"' + search.TIME_TO.substr(6, 2) + '-' + search.TIME_TO.substr(4, 2) + '-' + search.TIME_TO.substr(0, 4)+'"';
		if(search.ReportTypeCode == "tongvaovien"){			
		}
		
		const param = '{ "ApiData":  { "Uri": "'+AppConfig.domain.integrate+'","Action":"'+search.ReportTypeCode+'"'  + filter + '}, "CommonParam": {} }';		
        return this.webApiClientService.post<reportDatas>(AppConfig.domain.mrs, AppConfig.endpoints.MrsReportGetApi, param);
	}	
	
	loadDataReport(search: SearchReport): Observable<reportDatas> {
        let filter = '{"CREATE_TIME_FROM":'+search.CREATE_TIME_FROM+',"CREATE_TIME_TO":'+search.CREATE_TIME_TO+'}';    
		if(search.ReportTemplateCode=="TKB0002101_1")
			filter ='{"CREATE_TIME_FROM":'+search.CREATE_TIME_FROM+',"CREATE_TIME_TO":'+search.CREATE_TIME_TO+',"KCC_DEPARTMENT_CODE":"", "KCC_ROOM_CODE":"MA_PHONG","IS_INTRUCTION_TIME_OR_FINISH_TIME":null}, "ListKeyAllow":"Report0"';
		else if(search.ReportTemplateCode=="MRS00012TKB0003601"){
			filter ='{"TIME_FROM":'+search.TIME_FROM+',"TIME_TO":'+search.TIME_TO+',"KCC_DEPARTMENT_CODE":"", "KCC_ROOM_CODE":"MA_PHONG","IS_INTRUCTION_TIME_OR_FINISH_TIME":null,"CHOOSE_TIME":true}, "ListKeyAllow":"Report0,Report1"';
		}
		else if(search.ReportTemplateCode=="TKB0001602"){
			filter ='{"TIME_FROM":'+search.TIME_FROM+',"TIME_TO":'+search.TIME_TO+'}, "ListKeyAllow":"Report1"';
		}
		else if(search.ReportTemplateCode=="MRS0048204"){
			filter ='{"TIME_FROM":'+search.TIME_FROM+',"TIME_TO":'+search.TIME_TO+',"KCC_DEPARTMENT_CODE":"", "KCC_ROOM_CODE":"MA_PHONG","IS_INTRUCTION_TIME_OR_FINISH_TIME":null}, "ListKeyAllow":"Report"';
		}
		else if(search.ReportTemplateCode=="MRS0055501"){
			filter ='{"TIME_FROM":'+search.TIME_FROM+',"TIME_TO":'+search.TIME_TO+',"KCC_DEPARTMENT_CODE":"", "KCC_ROOM_CODE":"MA_PHONG","IS_TREAT_IN":true}, "ListKeyAllow":"Report"';
		}
		else if(search.ReportTemplateCode=="MRS0055509_1"){
			filter ='{"TIME_FROM":'+search.TIME_FROM+',"TIME_TO":'+search.TIME_TO+',"IS_TREAT_IN":true,"KCC_DEPARTMENT_CODE":"", "KCC_ROOM_CODE":"MA_PHONG","IS_INTRUCTION_TIME_OR_FINISH_TIME":null}, "ListKeyAllow":"Report"';
		}
		else if(search.ReportTemplateCode=="MRS0000206_1"){
			filter ='{"INTRUCTION_TIME_FROM":'+search.TIME_FROM+',"INTRUCTION_TIME_TO":'+search.TIME_TO+',"IS_INTRUCTION_TIME_OR_FINISH_TIME":false}, "ListKeyAllow":"Report0"';
		}
		else if(search.ReportTemplateCode=="MRS0000201"){
			filter ='{"INTRUCTION_TIME_FROM":'+search.TIME_FROM+',"INTRUCTION_TIME_TO":'+search.TIME_TO+',"IS_INTRUCTION_TIME_OR_FINISH_TIME":false}, "ListKeyAllow":"Department"';
			const param = '{ "ApiData":  { "ReportTypeCode": "'+search.ReportTypeCode+'","ReportTemplateCode":"'+search.ReportTemplateCode+'", "Filter": ' + filter + '}, "CommonParam": {} }';
		}		
        const param = '{ "ApiData":  { "ReportTypeCode": "'+search.ReportTypeCode+'","ReportTemplateCode":"'+search.ReportTemplateCode+'", "Filter": ' + filter + '}, "CommonParam": {} }';
		//console.log(param);
		//return this.webApiClientService.post<reportDatas>(AppConfig.domain.mrs, AppConfig.endpoints.MrsReportCreateApi, param);
        return this.webApiClientService.post<reportDatas>(AppConfig.domain.dbd, AppConfig.endpoints.DbdReportCreateSdoApi, param);
    }
	
	
	sleepExample(ms: number) {
		return new Promise( resolve => setTimeout(resolve, ms) );
	}
	
	delay(ms: number)
	{
	   ms = 200;
	   const date = new Date();	
	   let currentDate = new Date();
	   do {
		 currentDate = new Date();
	   } while (currentDate.getTime() - date.getTime() < ms);
	}
}