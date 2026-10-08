import { ProfileData } from "../../model/danhsachbaocao.model";
import { reportDatas } from "../../model/report";
import { ReportIntegrate } from "../../model/ReportIntegrate";
import { SearchReport } from "../../model/Search.model";
import { ParamData } from "../../model/apidata.model";
import { Observable, of, throwError as observableThrowError } from "rxjs";
import { AppConfig } from "../../config/app.config";
import { Injectable } from "@angular/core";
import "rxjs/add/operator/toPromise";
import "rxjs/add/operator/catch";
import { WebApiClientService } from "../../core/webApiClient.service";
import { String, StringBuilder } from "typescript-string-operations";

@Injectable()
export class HomeService {
  constructor(private webApiClientService: WebApiClientService) {}

  getParamDateRangeIntegrateSystem(search: SearchReport): string {
    let filter =
      "/" +
      search.TIME_FROM.substr(6, 2) +
      "-" +
      search.TIME_FROM.substr(4, 2) +
      "-" +
      search.TIME_FROM.substr(0, 4) +
      "/" +
      search.TIME_TO.substr(6, 2) +
      "-" +
      search.TIME_TO.substr(4, 2) +
      "-" +
      search.TIME_TO.substr(0, 4);

    return filter;
  }

  loadDataReportIntegrate(search: SearchReport): Observable<reportDatas> {
    let filter = "";
    if (
      search.TIME_FROM != undefined &&
      search.TIME_FROM != null &&
      search.TIME_FROM != "" &&
      search.TIME_TO != undefined &&
      search.TIME_TO != null &&
      search.TIME_TO != ""
    )
      filter =
        ',"FromTime":"' +
        search.TIME_FROM.substr(6, 2) +
        "-" +
        search.TIME_FROM.substr(4, 2) +
        "-" +
        search.TIME_FROM.substr(0, 4) +
        '","ToTime":"' +
        search.TIME_TO.substr(6, 2) +
        "-" +
        search.TIME_TO.substr(4, 2) +
        "-" +
        search.TIME_TO.substr(0, 4) +
        '"';
    if (search.ReportTypeCode == "tongvaovien") {
    }

    const param =
      '{ "ApiData":  { "Uri": "' +
      AppConfig.domain.integrate +
      '","Action":"' +
      search.ReportTypeCode +
      '"' +
      filter +
      '}, "CommonParam": {} }';
    return this.webApiClientService.post<reportDatas>(
      AppConfig.domain.mrs,
      AppConfig.endpoints.MrsReportGetApi,
      param
    );
  }

  loadDataReport(search: SearchReport): Observable<reportDatas> {
    let filter =
      '{"TIME_FROM":' + search.TIME_FROM + ',"TIME_TO":' + search.TIME_TO + "}";
      if(search.ReportTemplateCode=="TKB0004204") 
			filter ='{"TIME_FROM":'+search.CREATE_TIME_FROM+',"TIME_TO":'+search.CREATE_TIME_TO+'}, "ListKeyAllow":"Report0"';
		else if(search.ReportTemplateCode=="TKB1223401") 
		filter ='{"TIME_FROM":'+search.CREATE_TIME_FROM+',"TIME_TO":'+search.CREATE_TIME_TO+', "KCC_ROOM_CODE":"MA_PHONG"}, "ListKeyAllow":"Report0,Report4"';
	else if (search.ReportTemplateCode == "TKB0002101")
      filter =
        '{"CREATE_TIME_FROM":' +
        search.CREATE_TIME_FROM +
        ',"CREATE_TIME_TO":' +
        search.CREATE_TIME_TO +
        ', "KCC_ROOM_CODE":"MA_PHONG"}, "ListKeyAllow":"Report0"';
    else if (search.ReportTemplateCode == "MRS00012TKB0003601") {
      filter =
        '{"TIME_FROM":' +
        search.TIME_FROM +
        ',"TIME_TO":' +
        search.TIME_TO +
        ', "KCC_ROOM_CODE":"MA_PHONG","CHOOSE_TIME":true}, "ListKeyAllow":"Report0,Report1"';
    } else if (search.ReportTemplateCode == "TKB0001602") {
      filter =
        '{"TIME_FROM":' +
        search.TIME_FROM +
        ',"TIME_TO":' +
        search.TIME_TO +
        '}, "ListKeyAllow":"Report1"';
    } else if (search.ReportTemplateCode == "MRS0048204") {
      filter =
        '{"TIME_FROM":' +
        search.TIME_FROM +
        ',"TIME_TO":' +
        search.TIME_TO +
        ', "KCC_ROOM_CODE":"MA_PHONG"}, "ListKeyAllow":"Report"';
    } else if (search.ReportTemplateCode == "MRS0055501") {
      filter =
        '{"TIME_FROM":' +
        search.TIME_FROM +
        ',"TIME_TO":' +
        search.TIME_TO +
        ', "KCC_ROOM_CODE":"MA_PHONG","IS_TREAT_IN":true}, "ListKeyAllow":"Report"';
    } else if (search.ReportTemplateCode == "MRS0055509") {
      filter =
        '{"TIME_FROM":' +
        search.TIME_FROM +
        ',"TIME_TO":' +
        search.TIME_TO +
        ',"KCC_ROOM_CODE":"MA_PHONG","IS_TREAT_IN":true}, "ListKeyAllow":"Report"';
    } else if (search.ReportTemplateCode == "MRS0000206") {
      filter =
        '{"INTRUCTION_TIME_FROM":' +
        search.TIME_FROM +
        ',"INTRUCTION_TIME_TO":' +
        search.TIME_TO +
        ',"IS_INTRUCTION_TIME_OR_FINISH_TIME":false}, "ListKeyAllow":"Report0"';
    } else if (search.ReportTemplateCode == "MRS0000201") {
      filter =
        '{"INTRUCTION_TIME_FROM":' +
        search.TIME_FROM +
        ',"INTRUCTION_TIME_TO":' +
        search.TIME_TO +
        ',"IS_INTRUCTION_TIME_OR_FINISH_TIME":false}, "ListKeyAllow":"Department"';
    } else if (search.ReportTemplateCode == "MRS0000203") {
      search.ReportTemplateCode = "MRS0000201";
      filter =
        '{"INTRUCTION_TIME_FROM":' +
        search.TIME_FROM +
        ',"INTRUCTION_TIME_TO":' +
        search.TIME_TO +
        ',"IS_INTRUCTION_TIME_OR_FINISH_TIME":true}, "ListKeyAllow":"Department"';
    } else if (search.ReportTemplateCode == "TKB0006101") {
      filter =
        '{"TIME_FROM":' +
        search.TIME_FROM +
        ',"TIME_TO":' +
        search.TIME_TO +
        '}, "ListKeyAllow":""';
    } else if (search.ReportTemplateCode == "MRS0029601") {
      filter =
        '{"TIME_FROM":' +
        search.TIME_FROM +
        ',"TIME_TO":' +
        search.TIME_TO +
        ',"SERVICE_TYPE_ID":1}, "ListKeyAllow":"ReportSum"';
    } else if (search.ReportTemplateCode == "MRS0024901") {
      filter =
        '{"CREATE_TIME_FROM":' +
        search.TIME_FROM +
        ',"CREATE_TIME_TO":' +
        search.TIME_TO +
        '}, "ListKeyAllow":"ReportSum"';
    }
    if (search.Keyword != undefined && search.Keyword != "") {
      filter = '{"ListTemplateAllow":"' + search.Keyword + '"}';
    }
    const param =
      '{ "ApiData":  { "ReportTypeCode": "' +
      search.ReportTypeCode +
      '","ReportTemplateCode":"' +
      search.ReportTemplateCode +
      '", "Filter": ' +
      filter +
      '}, "CommonParam": {} }';
    //console.log(param);
    //return this.webApiClientService.post<reportDatas>(AppConfig.domain.mrs, AppConfig.endpoints.MrsReportCreateApi, param);
    return this.webApiClientService.post<reportDatas>(
      AppConfig.domain.dbd,
      AppConfig.endpoints.DbdReportCreateSdoApi,
      param
    );
  }

  loadDataReportMrs(search: SearchReport): Observable<reportDatas> {
    let filter =
      '{"CREATE_TIME_FROM":' +
      search.CREATE_TIME_FROM +
      ',"CREATE_TIME_TO":' +
      search.CREATE_TIME_TO +
      "}";
    if (search.ReportTemplateCode == "TKB0002101")
      filter =
        '{"CREATE_TIME_FROM":' +
        search.CREATE_TIME_FROM +
        ',"CREATE_TIME_TO":' +
        search.CREATE_TIME_TO +
        ',"KCC_DEPARTMENT_CODE":"", "KCC_ROOM_CODE":"MA_PHONG","IS_INTRUCTION_TIME_OR_FINISH_TIME":null}, "ListKeyAllow":"Report0"';
    else if (search.ReportTemplateCode == "MRS00012TKB0003601") {
      filter =
        '{"TIME_FROM":' +
        search.TIME_FROM +
        ',"TIME_TO":' +
        search.TIME_TO +
        ',"KCC_DEPARTMENT_CODE":"", "KCC_ROOM_CODE":"MA_PHONG","IS_INTRUCTION_TIME_OR_FINISH_TIME":null,"CHOOSE_TIME":true}, "ListKeyAllow":"Report0,Report1"';
    } else if (search.ReportTemplateCode == "TKB0001602") {
      filter =
        '{"TIME_FROM":' +
        search.TIME_FROM +
        ',"TIME_TO":' +
        search.TIME_TO +
        '}, "ListKeyAllow":"Report1"';
    } else if (search.ReportTemplateCode == "MRS0048204") {
      filter =
        '{"TIME_FROM":' +
        search.TIME_FROM +
        ',"TIME_TO":' +
        search.TIME_TO +
        ',"KCC_DEPARTMENT_CODE":"", "KCC_ROOM_CODE":"MA_PHONG","IS_INTRUCTION_TIME_OR_FINISH_TIME":null}, "ListKeyAllow":"Report"';
    } else if (search.ReportTemplateCode == "MRS0055501") {
      filter =
        '{"TIME_FROM":' +
        search.TIME_FROM +
        ',"TIME_TO":' +
        search.TIME_TO +
        ',"KCC_DEPARTMENT_CODE":"", "KCC_ROOM_CODE":"MA_PHONG","IS_TREAT_IN":true}, "ListKeyAllow":"Report"';
    } else if (search.ReportTemplateCode == "MRS0055509") {
      filter =
        '{"TIME_FROM":' +
        search.TIME_FROM +
        ',"TIME_TO":' +
        search.TIME_TO +
        ',"IS_TREAT_IN":true,"KCC_DEPARTMENT_CODE":"", "KCC_ROOM_CODE":"MA_PHONG","IS_INTRUCTION_TIME_OR_FINISH_TIME":null}, "ListKeyAllow":"Report"';
    } else if (search.ReportTemplateCode == "MRS0000206") {
      filter =
        '{"INTRUCTION_TIME_FROM":' +
        search.TIME_FROM +
        ',"INTRUCTION_TIME_TO":' +
        search.TIME_TO +
        ',"IS_INTRUCTION_TIME_OR_FINISH_TIME":false}, "ListKeyAllow":"Report0"';
    } else if (search.ReportTemplateCode == "MRS0000201") {
      filter =
        '{"INTRUCTION_TIME_FROM":' +
        search.TIME_FROM +
        ',"INTRUCTION_TIME_TO":' +
        search.TIME_TO +
        ',"IS_INTRUCTION_TIME_OR_FINISH_TIME":false}, "ListKeyAllow":"Department"';
    } else if (search.ReportTemplateCode == "TKB0006101") {
      filter =
        '{"TIME_FROM":' +
        search.TIME_FROM +
        ',"TIME_TO":' +
        search.TIME_TO +
        '}, "ListKeyAllow":""';
    }
    const param =
      '{ "ApiData":  { "ReportTypeCode": "' +
      search.ReportTypeCode +
      '","ReportTemplateCode":"' +
      search.ReportTemplateCode +
      '", "Filter": ' +
      filter +
      '}, "CommonParam": {} }';
    //console.log(param);
    return this.webApiClientService.post<reportDatas>(
      AppConfig.domain.mrs,
      AppConfig.endpoints.MrsReportCreateApi,
      param
    );
  }
  sleepExample(ms: number) {
    return new Promise((resolve) => setTimeout(resolve, ms));
  }

  delay(ms: number) {
    ms = 200;
    const date = new Date();
    let currentDate = new Date();
    do {
      currentDate = new Date();
    } while (currentDate.getTime() - date.getTime() < ms);
  }
}
