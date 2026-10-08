import { Observable, of, throwError as observableThrowError } from "rxjs";
import { reportDatas } from "../../model/report";
import { SearchReport } from "../../model/Search.model";
import { ParamData } from "../../model/apidata.model";
import { AppConfig } from "../../config/app.config";
import { Injectable } from "@angular/core";
import "rxjs/add/operator/toPromise";
import "rxjs/add/operator/catch";
import { WebApiClientService } from "../../core/webApiClient.service";
import { String, StringBuilder } from "typescript-string-operations";
import {
  AuthorizeResultModel,
  AuthorizeApiResult,
} from "../../model/authorize.result.model";
import { TokenData } from "../../model/authData.model";

@Injectable()
export class MenuService {
  public tokenData: TokenData;
  constructor(private webApiClientService: WebApiClientService) {}

  loadMenu(): Observable<AuthorizeApiResult> {
    const strTokenData = localStorage.getItem("tokendata");
    if (strTokenData != null) this.tokenData = JSON.parse(strTokenData);
    let loginName = "";
    if (this.tokenData != undefined && this.tokenData != null) {
      loginName = this.tokenData.User.LoginName;
    }
    const applicationCode = AppConfig.common.AppLICATION_CODE;
    let param = String.Format(
      "{   \"ApiData\": { LOGIN_NAME: '{0}', APPLICATION_CODE: '{1}' } }",
      loginName,
      applicationCode
    );

    return this.webApiClientService.get<AuthorizeApiResult>(
      AppConfig.domain.acs,
      AppConfig.endpoints.AcsTokenAuthorize,
      param
    );
  }
  RefeshCacheServer() : Observable<Boolean>{
    return this.webApiClientService.post<Boolean>(
      AppConfig.domain.dbd,
      AppConfig.endpoints.DbdReportRefreshApi,
      '{ "ApiData":  { "ReportTypeCode": "TKBJRTYE","ReportTemplateCode":"TKBJRTYE03", "Filter": {}, "CommonParam": {} }'
    );
  }
  loadDataCls(
    search: SearchReport,
    page: number,
    paramData: ParamData
  ): Observable<reportDatas> {
    search.SQL = `*/
			select 
      sr.service_req_stt_id,
	(case when pr.service_code='CHA10' then 1 end) is_clvt32,
	(case when pr.service_code='CHA09' then 1 end) is_clvt64,
	(case when sv.pacs_type_code='MR' then 1 end) is_mri,
	(case when pr.service_code in ('CHA08','CHA07') or sv.service_code='CHU1829' then 1 end) is_xq,
	(case when ss.tdl_service_type_id =10 then 1 end) is_sa,
	(case when ss.tdl_service_type_id =10 and ss.patient_type_id=1 then 1 end) is_sa_bh,
	(case when ss.tdl_service_type_id =9 and (exro.department_code in ('K05') or exro.room_code in ('TDCN1') or exro.room_code in ('TDCN2')) then 1 end) is_nsth,
	(case when ss.tdl_service_type_id =9 and (exro.room_code in ('TDCN1') or exro.room_code in ('TDCN2')) then 1 end) is_nsth_out,
	sum(ss.amount) amount 
	from his_sere_serv ss
  join his_service_req sr on sr.id=ss.service_req_id and sr.is_delete=0 and sr.is_no_execute is null
	join his_sere_serv_ext sse on sse.sere_serv_id=ss.id
	join his_service sv on sv.id=ss.service_id
	left join his_service pr on pr.id=sv.parent_id
	left join v_his_room exro on exro.id=ss.tdl_execute_room_id
	where ss.is_delete=0
	and ss.is_no_execute is null
	and ss.service_req_id is not null
	and sse.end_time between {0} and {1}
	and ss.tdl_service_type_id in (3,9,10)
	group by
  sr.service_req_stt_id,
	(case when pr.service_code='CHA10' then 1 end),
	(case when pr.service_code='CHA09' then 1 end),
	(case when sv.pacs_type_code='MR' then 1 end),
	(case when pr.service_code in ('CHA08','CHA07') or sv.service_code='CHU1829' then 1 end),
	(case when ss.tdl_service_type_id =10 then 1 end),
	(case when ss.tdl_service_type_id =10 and ss.patient_type_id=1 then 1 end),
	(case when ss.tdl_service_type_id =9 and (exro.department_code in ('K05') or exro.room_code in ('TDCN1') or exro.room_code in ('TDCN2')) then 1 end),
	(case when ss.tdl_service_type_id =9 and (exro.room_code in ('TDCN1') or exro.room_code in ('TDCN2')) then 1 end)
	/*`;
    search.SQL = search.SQL.replace("{0}", search.TIME_FROM).replace(
      "{1}",
      search.TIME_TO
    );
    let filter =
      '{"TIME_FROM":' +
      search.TIME_FROM +
      ',"TIME_TO":' +
      search.TIME_TO +
      ',"SQL":"' +
      search.SQL +
      '"';

    const param =
      '{ "ApiData":  { "ReportTypeCode": "TKBJRTYE","ReportTemplateCode":"TKBJRTYE03", "Filter": ' +
      filter +
      '}, "CommonParam": {} }';
    console.log(param);
    return this.webApiClientService.post<reportDatas>(
      AppConfig.domain.dbd,
      AppConfig.endpoints.DbdReportCreateSdoApi,
      param
    );
  }
  loadDatasieuam(
    search: SearchReport,
    page: number,
    paramData: ParamData
  ): Observable<reportDatas> {
    let dopplerMach = `'SIE8601','SIE5856','SIE5956','SIE8615','SIE8600','DOP8616','SIE8604','SIE8612'`;
    let tim = `'SIE8620','SIE6601','SIE9954','SIE5663','SIE8617','SIE5857','SIE5859','SIE5860','SIE8618','SIE8619','SIE5858'`;
    let danhoimo = `'SIE9960','SIE9961'`;
    let chuyenkhoasan = `'SIE8603','SIE8611','SIE8597','SIE5955','SIE8621','SIE8610','SIE8614','SIE8609','SIE8605','SIE8606','SIE5995','SIE8594','M-S034','SIE8584','SIE8593','SIE8595'`;
    search.SQL =
      `*/
        select 
(case when sv.service_code in (` +
      dopplerMach +
      `) then 1 end) is_doppler_mach,
(case when sv.service_code in (` +
      tim +
      `) then 1 end) is_tim,
(case when sv.service_code in (` +
      danhoimo +
      `) then 1 end) is_danhoimo,
(case when sv.service_code in (` +
      chuyenkhoasan +
      `) then 1 end) is_chuyenkhoasan,
sum(ss.amount) amount 
from his_sere_serv ss
join his_sere_serv_ext sse on sse.sere_serv_id=ss.id
join his_service sv on sv.id=ss.service_id
left join his_service pr on pr.id=sv.parent_id
left join v_his_room exro on exro.id=ss.tdl_execute_room_id
where ss.is_delete=0
and ss.is_no_execute is null
and ss.service_req_id is not null
        `;
    search.SQL +=
      "and sse.end_time between " +
      search.TIME_FROM +
      " and " +
      search.TIME_TO +
      "";
    search.SQL +=
      `

        and ss.tdl_service_type_id in (10)
        group by
        (case when sv.service_code in (` +
      dopplerMach +
      `) then 1 end),
        (case when sv.service_code in (` +
      tim +
      `) then 1 end),
        (case when sv.service_code in (` +
      danhoimo +
      `) then 1 end),
        (case when sv.service_code in (` +
      chuyenkhoasan +
      `) then 1 end)
/*`;
    let filter =
      '{"TIME_FROM":' +
      search.TIME_FROM +
      ',"TIME_TO":' +
      search.TIME_TO +
      ',"SQL":"' +
      search.SQL +
      '"';

    const param =
      '{ "ApiData":  { "ReportTypeCode": "TKBJRTYE","ReportTemplateCode":"TKBJRTYE03", "Filter": ' +
      filter +
      '}, "CommonParam": {} }';
    console.log(param);
    return this.webApiClientService.post<reportDatas>(
      AppConfig.domain.dbd,
      AppConfig.endpoints.DbdReportCreateSdoApi,
      param
    );
  }
  loadDataGpbl(
    search: SearchReport,
    page: number,
    paramData: ParamData
  ): Observable<reportDatas> {
    let sinhThiet = `'XET9366','XET9350'`;
    let tebao = `'XET064','XET9365','CEL9364','TEB9348','NHU9362','TEB9344','TEB9345','TEB9346','XET9063','XET9363','TEB9343','TEB9347'`;
    search.SQL =
      `*/
        select 
(case when sv.service_code in (` +
      sinhThiet +
      `) then 1 end) is_sinhthiet,
null is_noisoi,
(case when ss.tdl_service_type_id =15 and (rero.department_code in ('K05')) and sv.service_code not in (` +
      tebao +
      `) and sv.service_code not in (` +
      sinhThiet +
      `) then 1 end) is_noitieuhoa,
(case when ss.tdl_service_type_id =15 and (rero.room_code in ('TDCN1') or rero.room_code in ('TDCN2')) then 1 end) is_tdcn,
(case when ss.tdl_service_type_id =15 and (rero.department_code in ('K26')) then 1 end) is_nhamo,
(case when sv.service_code in (` +
      tebao +
      `) then 1 end) is_tebao,
sum(ss.amount) amount 
from his_sere_serv ss
left join his_sere_serv_ext sse on sse.sere_serv_id=ss.id
join his_service sv on sv.id=ss.service_id
left join v_his_room rero on rero.id=ss.tdl_request_room_id
where ss.is_delete=0
and ss.is_no_execute is null
and ss.service_req_id is not null
        `;
    search.SQL +=
      "and sse.end_time between " +
      search.TIME_FROM +
      " and " +
      search.TIME_TO +
      "";
    search.SQL +=
      `

        and ss.tdl_service_type_id in (15)
        group by
        (case when sv.service_code in (` +
      sinhThiet +
      `) then 1 end),
        
        (case when ss.tdl_service_type_id =15 and (rero.department_code in ('K05')) and sv.service_code not in (` +
      tebao +
      `) and sv.service_code not in (` +
      sinhThiet +
      `) then 1 end),
        (case when ss.tdl_service_type_id =15 and (rero.room_code in ('TDCN1') or rero.room_code in ('TDCN2')) then 1 end),
        (case when ss.tdl_service_type_id =15 and (rero.department_code in ('K26')) then 1 end),
        (case when sv.service_code in (` +
      tebao +
      `) then 1 end)
/*`;
    let filter =
      '{"TIME_FROM":' +
      search.TIME_FROM +
      ',"TIME_TO":' +
      search.TIME_TO +
      ',"SQL":"' +
      search.SQL +
      '"';

    const param =
      '{ "ApiData":  { "ReportTypeCode": "TKBJRTYE","ReportTemplateCode":"TKBJRTYE03", "Filter": ' +
      filter +
      '}, "CommonParam": {} }';
    console.log(param);
    return this.webApiClientService.post<reportDatas>(
      AppConfig.domain.dbd,
      AppConfig.endpoints.DbdReportCreateSdoApi,
      param
    );
  }
  loadDataSinhThietTucThi(
    search: SearchReport,
    page: number,
    paramData: ParamData
  ): Observable<reportDatas> {
    let sinhThiet = `'XET9366','XET9350'`;
    search.SQL = `*/
        select 
sr.tdl_patient_name,
sr.tdl_patient_dob||' ' tdl_patient_dob,
sse.end_time||' ' end_time,
rero.department_name,
exro.room_name,
ss.tdl_service_name,
sse.conclude
from his_sere_serv ss
join his_service_req sr on sr.id=ss.service_req_id
left join his_sere_serv_ext sse on sse.sere_serv_id=ss.id
join his_service sv on sv.id=ss.service_id
left join v_his_room rero on rero.id=ss.tdl_request_room_id
left join v_his_room exro on exro.id=ss.tdl_execute_room_id
where ss.is_delete=0
and ss.is_no_execute is null
and ss.service_req_id is not null
        `;
    search.SQL +=
      "and sse.end_time between " +
      search.TIME_FROM +
      " and " +
      search.TIME_TO +
      "";
    search.SQL +=
      `
        and ss.tdl_service_type_id in (15)
        and sv.service_code in (` +
      sinhThiet +
      `)
/*`;
    let filter =
      '{"TIME_FROM":' +
      search.TIME_FROM +
      ',"TIME_TO":' +
      search.TIME_TO +
      ',"SQL":"' +
      search.SQL +
      '"';

    const param =
      '{ "ApiData":  { "ReportTypeCode": "TKBJRTYE","ReportTemplateCode":"TKBJRTYE03", "Filter": ' +
      filter +
      '}, "CommonParam": {} }';
    console.log(param);
    return this.webApiClientService.post<reportDatas>(
      AppConfig.domain.dbd,
      AppConfig.endpoints.DbdReportCreateSdoApi,
      param
    );
  }
  loadDataXNSH(
    search: SearchReport,
    page: number,
    paramData: ParamData
  ): Observable<reportDatas> {
    search.SQL = `*/
        select 
        (case when sv.service_code='XET9195' then 1 end) is_khimau,
        (case when sv.service_code='DIN9179' then 1 end) is_miendich,
        (case when sv.service_code in ('TON9262','HER001') then 1 end) is_nuoctieu,
        count(distinct(sr.id)) amount,
        count(distinct(case when sst.value is not null then sr.id end)) amount_result 
        from his_sere_serv ss
        left join his_sere_serv_tein sst on sst.sere_serv_id=ss.id
        join his_service_req sr on sr.id=ss.service_req_id
        join his_service sv on sv.id=ss.service_id
        join his_service pr on pr.id=sv.parent_id
        where ss.is_delete=0
        and ss.is_no_execute is null
        and ss.service_req_id is not null
        `;
    search.SQL +=
      "and sr.finish_time between " +
      search.TIME_FROM +
      " and " +
      search.TIME_TO +
      "";
    search.SQL += `
        and ss.tdl_service_type_id in (2)
        and pr.service_code='CHA42'
        group by
        (case when sv.service_code='XET9195' then 1 end),
        (case when sv.service_code='DIN9179' then 1 end),
        (case when sv.service_code in ('TON9262','HER001') then 1 end)
/*`;
    let filter =
      '{"TIME_FROM":' +
      search.TIME_FROM +
      ',"TIME_TO":' +
      search.TIME_TO +
      ',"SQL":"' +
      search.SQL +
      '"';

    const param =
      '{ "ApiData":  { "ReportTypeCode": "TKBJRTYE","ReportTemplateCode":"TKBJRTYE03", "Filter": ' +
      filter +
      '}, "CommonParam": {} }';
    console.log(param);
    return this.webApiClientService.post<reportDatas>(
      AppConfig.domain.dbd,
      AppConfig.endpoints.DbdReportCreateSdoApi,
      param
    );
  }
  loadDataXNHH(
    search: SearchReport,
    page: number,
    paramData: ParamData
  ): Observable<reportDatas> {
    var khoihongcau = `'XET9374','XET9375'`;
    var httdl = `'DỊ9088','DỊ9089','DỊ9090'`;
    var htgtc = `'DIN9087'`;
    var tcm = `'XET9052'`;
    var tuaviii = `'NGH9100'`;
    var tebaodich = `'VI004','CHU9064','DEM9333','GAN9115','TIM9060','TON9037','TON9038','XET063','XET068','XET069','XET070','XET9046','XET9066'`;
    var huyetdo = `'HUY9040','HUY9041','HUY9042'`;
    var tuydo = `'XET9062'`;
    var tuyxuong = `'PHA10010'`;
    search.SQL =
      `*/
    select 
(case when sv.service_code in (` +
      khoihongcau +
      `) or sv.service_code in (` +
      httdl +
      `) or sv.service_code in (` +
      htgtc +
      `) or sv.service_code in (` +
      tcm +
      `) or sv.service_code in (` +
      tuaviii +
      `) then 1 end) is_truyenmau,
(case when sv.service_code in (` +
      khoihongcau +
      `) then 1 end) is_khc,
(case when sv.service_code in (` +
      httdl +
      `) then 1 end) is_httdl,
(case when sv.service_code in (` +
      htgtc +
      `) then 1 end) is_htgtc,
(case when sv.service_code in (` +
      tcm +
      `) then 1 end) is_tcm,
(case when sv.service_code in (` +
      tuaviii +
      `) then 1 end) is_tuaviii,
(case when sv.service_code in (` +
      tebaodich +
      `) then 1 end) is_tebaodich,
(case when sv.service_code in (` +
      huyetdo +
      `) then 1 end) is_huyetdo,
(case when sv.service_code in (` +
      tuydo +
      `) then 1 end) is_tuydo,
(case when sv.service_code in (` +
      tuyxuong +
      `) then 1 end) is_tuyxuong,
count(distinct(sr.id)) amount 
from his_sere_serv ss
left join his_sere_serv_tein sst on sst.sere_serv_id=ss.id
join his_service_req sr on sr.id=ss.service_req_id
join his_service sv on sv.id=ss.service_id
join his_service pr on pr.id=sv.parent_id
where ss.is_delete=0
and ss.is_no_execute is null
and ss.service_req_id is not null

    `;
    search.SQL +=
      "and sr.finish_time between " +
      search.TIME_FROM +
      " and " +
      search.TIME_TO +
      "";
    search.SQL +=
      `
    and ss.tdl_service_type_id in (2)
and pr.service_code='CHA41'
group by
(case when sv.service_code in (` +
      khoihongcau +
      `) or sv.service_code in (` +
      httdl +
      `) or sv.service_code in (` +
      htgtc +
      `) or sv.service_code in (` +
      tcm +
      `) or sv.service_code in (` +
      tuaviii +
      `) then 1 end),
(case when sv.service_code in (` +
      khoihongcau +
      `) then 1 end),
(case when sv.service_code in (` +
      httdl +
      `) then 1 end),
(case when sv.service_code in (` +
      htgtc +
      `) then 1 end),
(case when sv.service_code in (` +
      tcm +
      `) then 1 end),
(case when sv.service_code in (` +
      tuaviii +
      `) then 1 end),
(case when sv.service_code in (` +
      tebaodich +
      `) then 1 end),
(case when sv.service_code in (` +
      huyetdo +
      `) then 1 end),
(case when sv.service_code in (` +
      tuydo +
      `) then 1 end),
(case when sv.service_code in (` +
      tuyxuong +
      `) then 1 end)
/*`;
    let filter =
      '{"TIME_FROM":' +
      search.TIME_FROM +
      ',"TIME_TO":' +
      search.TIME_TO +
      ',"SQL":"' +
      search.SQL +
      '"';

    const param =
      '{ "ApiData":  { "ReportTypeCode": "TKBJRTYE","ReportTemplateCode":"TKBJRTYE03", "Filter": ' +
      filter +
      '}, "CommonParam": {} }';
    console.log(param);
    return this.webApiClientService.post<reportDatas>(
      AppConfig.domain.dbd,
      AppConfig.endpoints.DbdReportCreateSdoApi,
      param
    );
  }
  loadDataXNVS(
    search: SearchReport,
    page: number,
    paramData: ParamData
  ): Observable<reportDatas> {
    var testnhanh = `'HBS007','HCV9303','HBE9300','CHL9289','HEV9311','PLA002','TES018','RUB9325','VIK9277','HBS9298','HEV9310','HEL9290','TES5971','HAV9307','VIR9297','ROT9324','HIV9314','DEN9317'`;
    var nhuomsoi = `'VIB9285','NEI9287','VIN9335','VIK9276','DON9328'`;
    var miendich = `'HBS008','HCV006','HCV9305','ANT028','HIV003','DEN9318','HAV9308','RIC9291','HSV9322','CMV9320','HAV9309','DEN9319','FAS9331','HEV9312','HEV9313','HSV9321','STR001','HIV006','TOX005','HBS006'`;
    var nuoicay = `'NEI9288','VIK9278','VIB9286','VIN9336'`;
    search.SQL =
      `*/
select 
(case when sv.service_code in (` +
      testnhanh +
      `) then 1 end) is_testnhanh,
(case when sv.service_code in (` +
      nhuomsoi +
      `) then 1 end) is_nhuomsoi,
(case when sv.service_code in (` +
      miendich +
      `) then 1 end) is_miendich,
(case when sv.service_code in (` +
      nuoicay +
      `) then 1 end) is_nuoicay,
count(distinct(sr.id)) amount,
count(distinct(case when sst.value is not null and lower(sst.value) like '%dương%' then sr.id end)) amount_positive 
from his_sere_serv ss
left join his_sere_serv_tein sst on sst.sere_serv_id=ss.id
join his_service_req sr on sr.id=ss.service_req_id
join his_service sv on sv.id=ss.service_id
join his_service pr on pr.id=sv.parent_id
where ss.is_delete=0
and ss.is_no_execute is null
and ss.service_req_id is not null
and ss.tdl_service_type_id in (2)
`;
    search.SQL +=
      "and sr.finish_time between " +
      search.TIME_FROM +
      " and " +
      search.TIME_TO +
      "";
    search.SQL +=
      `
and pr.service_code='CHA43'
group by
(case when sv.service_code in (` +
      testnhanh +
      `) then 1 end),
(case when sv.service_code in (` +
      nhuomsoi +
      `) then 1 end),
(case when sv.service_code in (` +
      miendich +
      `) then 1 end),
(case when sv.service_code in (` +
      nuoicay +
      `) then 1 end)
/*`;
    let filter =
      '{"TIME_FROM":' +
      search.TIME_FROM +
      ',"TIME_TO":' +
      search.TIME_TO +
      ',"SQL":"' +
      search.SQL +
      '"';

    const param =
      '{ "ApiData":  { "ReportTypeCode": "TKBJRTYE","ReportTemplateCode":"TKBJRTYE03", "Filter": ' +
      filter +
      '}, "CommonParam": {} }';
    console.log(param);
    return this.webApiClientService.post<reportDatas>(
      AppConfig.domain.dbd,
      AppConfig.endpoints.DbdReportCreateSdoApi,
      param
    );
  }
  loadDataMau(
    search: SearchReport,
    page: number,
    paramData: ParamData
  ): Observable<reportDatas> {
    var hongcau = `'KHC250','KHC250212','KHC350','KHC35011','KHC50','test_mau1'`;
    var huyettuong = `'HTGTC100','HTGTC150','HTTĐL150','HTTĐL200','HTTĐL250','HTĐL150','HTĐL200','HTĐL250'`;
    search.SQL =
      `*/
select 
(case when blty.blood_type_code in (` +
      hongcau +
      `) then 1 end) is_hongcau,
(case when blty.blood_type_code in (` +
      huyettuong +
      `) then 1 end) is_huyettuong,
blty.blood_abo_code,
blty.volume,
sum((select count(1) from v_his_imp_mest_blood where imp_mest_stt_id=5 and is_delete=0 and blood_id=blty.id and imp_time <` +
      search.TIME_FROM +
      ` )
-(select count(1) from v_his_exp_mest_blood where is_export=1 and is_delete=0 and blood_id=blty.id and exp_time <` +
      search.TIME_FROM +
      ` )) begin_amount,
sum((select count(1) from v_his_imp_mest_blood where imp_mest_stt_id=5 and is_delete=0 and blood_id=blty.id and imp_time between ` +
      search.TIME_FROM +
      ` and ` +
      search.TIME_TO +
      `)) imp_amount,
sum((select count(1) from v_his_exp_mest_blood where is_export=1 and is_delete=0 and blood_id=blty.id and exp_time between ` +
      search.TIME_FROM +
      ` and ` +
      search.TIME_TO +
      `)) exp_amount,
sum((select count(1) from v_his_imp_mest_blood where imp_mest_stt_id=5 and is_delete=0 and blood_id=blty.id and imp_time <` +
      search.TIME_TO +
      ` )
-(select count(1) from v_his_exp_mest_blood where is_export=1 and is_delete=0 and blood_id=blty.id and exp_time <` +
      search.TIME_TO +
      ` )) end_amount
from v_his_blood blty
where blood_type_code in (` +
      hongcau +
      `,` +
      huyettuong +
      `)
group by
(case when blty.blood_type_code in (` +
      hongcau +
      `) then 1 end),
(case when blty.blood_type_code in (` +
      huyettuong +
      `) then 1 end),
blty.blood_abo_code,
blty.volume
/*
`;

    let filter =
      '{"TIME_FROM":' +
      search.TIME_FROM +
      ',"TIME_TO":' +
      search.TIME_TO +
      ',"SQL":"' +
      search.SQL +
      '"';

    const param =
      '{ "ApiData":  { "ReportTypeCode": "TKBJRTYE","ReportTemplateCode":"TKBJRTYE03", "Filter": ' +
      filter +
      '}, "CommonParam": {} }';
    console.log(param);
    return this.webApiClientService.post<reportDatas>(
      AppConfig.domain.dbd,
      AppConfig.endpoints.DbdReportCreateSdoApi,
      param
    );
  }

  loadDataDTSHPT(
    search: SearchReport,
    page: number,
    paramData: ParamData
  ): Observable<reportDatas> {
    var dinhluong = `'CON010','CON011','DIN195','HBV9301','HBV9302','HCV005','HCV9306','HPV9323','MYC9282','MYC9283','PCR003','PCR004','PHA10000','PHA10001','TT5015','XAC001','XAC002','XAC003','XET9367','XET9368','XET9370','[VI001'`;
    var chandoan = `'CON001','CON002','GIA001','HIV007','PBO','PHA001','PHA002','PHA003','PHA004','PHA005','PHA006','PHA007','PHA008','PHA009','PHA010','PHA011','PHA012','SC2B1','SC2B10','SC2B5','VIK9317','VRC0002','VRI0001','VRN0001','VRN0003','VRN0004','VRN0005','VSR0001','XAC004','XAC005','XAC006','XAC007','XAC008','XAC009','XAC010','XAC011','XAC012','XAC013','XAC014','XAC015','XAC016','XAC018','XN9318','XN9319','XN9321','XN9322','XN9324','XN9325','XN9327','XN9330','XN9333','cor003'`;
    var corona = `'COR001','CORO001','CORO002','CORO003','CORO004','CORO005','CORO006','CORO007','CORO008','CORO009','CORO010','CRN0005','Corona'`;
    search.SQL =
      `*/
select 
(case when sv.service_code in (` +
      dinhluong +
      `) then 1 end) is_dinhluong,
(case when sv.service_code in (` +
      chandoan +
      `) then 1 end) is_chandoan,
(case when sv.service_code in (` +
      corona +
      `) then 1 end) is_corona,
count(distinct(sr.id)) amount,
count(distinct(case when sst.value is not null and lower(sst.value) like '%dương%' then sr.id end)) amount_positive 
from his_sere_serv ss
left join his_sere_serv_tein sst on sst.sere_serv_id=ss.id
join his_service_req sr on sr.id=ss.service_req_id
join his_service sv on sv.id=ss.service_id
join v_his_room exro on exro.id=sr.execute_room_id
where ss.is_delete=0
and ss.is_no_execute is null
and ss.service_req_id is not null
and ss.tdl_service_type_id in (2)
`;
    search.SQL +=
      "and sr.finish_time between " +
      search.TIME_FROM +
      " and " +
      search.TIME_TO +
      "";
    search.SQL +=
      `
and exro.room_code in ('SHPT02','SHPT','DT')
group by
(case when sv.service_code in (` +
      dinhluong +
      `) then 1 end) ,
(case when sv.service_code in (` +
      chandoan +
      `) then 1 end) ,
(case when sv.service_code in (` +
      corona +
      `) then 1 end)
/*`;
    let filter =
      '{"TIME_FROM":' +
      search.TIME_FROM +
      ',"TIME_TO":' +
      search.TIME_TO +
      ',"SQL":"' +
      search.SQL +
      '"';

    const param =
      '{ "ApiData":  { "ReportTypeCode": "TKBJRTYE","ReportTemplateCode":"TKBJRTYE03", "Filter": ' +
      filter +
      '}, "CommonParam": {} }';
    console.log(param);
    return this.webApiClientService.post<reportDatas>(
      AppConfig.domain.dbd,
      AppConfig.endpoints.DbdReportCreateSdoApi,
      param
    );
  }
  loadDataCanThiep(
    search: SearchReport,
    page: number,
    paramData: ParamData
  ): Observable<reportDatas> {
    search.SQL = `*/
select 
ss.tdl_service_name,
(case when exro.room_code in ('CTXQ') then 1 end) is_ctxq,
(case when exro.room_code in ('CTTM1') then 1 end) is_cttm,
(case when exro.room_code in ('ĐQ01') then 1 end) is_ctdq,
sum(ss.amount) amount
from his_sere_serv ss
join his_service_req sr on sr.id=ss.service_req_id
join v_his_room exro on exro.id=sr.execute_room_id
where ss.is_delete=0
and ss.is_no_execute is null
and ss.service_req_id is not null
`;
    search.SQL +=
      "and sr.finish_time between " +
      search.TIME_FROM +
      " and " +
      search.TIME_TO +
      "";
    search.SQL += `
and exro.room_code in ('CTXQ','CTTM1','ĐQ01')
group by
ss.tdl_service_name,
(case when exro.room_code in ('CTXQ') then 1 end),
(case when exro.room_code in ('CTTM1') then 1 end),
(case when exro.room_code in ('ĐQ01') then 1 end)
order by ss.tdl_service_name
/*`;
    let filter =
      '{"TIME_FROM":' +
      search.TIME_FROM +
      ',"TIME_TO":' +
      search.TIME_TO +
      ',"SQL":"' +
      search.SQL +
      '"';

    const param =
      '{ "ApiData":  { "ReportTypeCode": "TKBJRTYE","ReportTemplateCode":"TKBJRTYE03", "Filter": ' +
      filter +
      '}, "CommonParam": {} }';
    console.log(param);
    return this.webApiClientService.post<reportDatas>(
      AppConfig.domain.dbd,
      AppConfig.endpoints.DbdReportCreateSdoApi,
      param
    );
  }
  loadDataPhauthuat(
    search: SearchReport,
    page: number,
    paramData: ParamData
  ): Observable<reportDatas> {
    var giamdau = `'GIA7878','XOA6564','XOA6563','PHA6662','CT2','PHU13','M353/246SYT/21','NT0051','TH00078','DIEN0058','DIEN0059','VT127/1034/23','VT127/2/1034/23'`;
    var sttt = `'XET9366','XET9350'`;
    search.SQL =
      `*/
      select
      rdp.department_name,
      (case when pp.pttt_priority_code='01' then 1 end) is_kh,
      (case when ss.tdl_service_type_id=4 then 1 end) is_tt,
      (case when pp.pttt_priority_code='02' then 1 end) is_cc,
      (case when pp.pttt_priority_code='03' then 1 end) is_yc,
      ss1.is_giamdau,
      (case when sr.icd_code between  'C00' and 'D48.99' then 1 end) is_bnk,
      ss1.is_gpb,
      ss1.is_sttt,
      sum(ss.amount) amount
      from his_sere_serv ss
      join his_service_req sr on sr.id=ss.service_req_id
      join his_department rdp on rdp.id=sr.request_department_id and rdp.department_code in ('K02','K19','K20','K21','K22','K23','K24','K26')
      left join his_sere_serv_pttt ssp on ssp.sere_serv_id=ss.id
      join his_sere_serv_ext sse on sse.sere_serv_id=ss.id
      left join his_pttt_priority pp on pp.id=ssp.pttt_priority_id
      left join lateral
      (
      select
      (case when count(case when ss1.tdl_service_code in (` +
      giamdau +
      `) then 1 end)>0 then 1 end) is_giamdau,
      (case when count(case when ss1.tdl_service_type_id in (15) and ss1.tdl_service_code not in (` +
      sttt +
      `) then 1 end)>0 then 1 end) is_gpb,
      (case when count(case when ss1.tdl_service_code in (` +
      sttt +
      `) then 1 end)>0 then 1 end) is_sttt
      from his_sere_serv ss1
      where ss1.is_delete=0 
      and ss1.is_no_execute is null
      and ss1.service_req_id is not null
      and ss1.tdl_treatment_id=ss.tdl_treatment_id
      and ss1.tdl_request_room_id=ss.tdl_execute_room_id
      and (ss1.tdl_service_code in (` +
      giamdau +
      `) or ss1.tdl_service_type_id in (15))
      ) ss1 on 1=1
      where ss.is_delete=0 and ss.is_no_execute is null
      and ss.tdl_service_type_id in (4,11)
     
`;
    search.SQL +=
      "and sse.end_time between " +
      search.TIME_FROM +
      " and " +
      search.TIME_TO +
      "";
    search.SQL += `
      group by
      rdp.department_name,
      (case when pp.pttt_priority_code='01' then 1 end),
      (case when pp.pttt_priority_code='02' then 1 end),
      (case when pp.pttt_priority_code='03' then 1 end),
      (case when ss.tdl_service_type_id=4 then 1 end),
      ss1.is_giamdau,
      (case when sr.icd_code between  'C00' and 'D48.99' then 1 end),
      ss1.is_gpb,
      ss1.is_sttt
/*`;
    let filter =
      '{"TIME_FROM":' +
      search.TIME_FROM +
      ',"TIME_TO":' +
      search.TIME_TO +
      ',"SQL":"' +
      search.SQL +
      '"';

    const param =
      '{ "ApiData":  { "ReportTypeCode": "TKBJRTYE","ReportTemplateCode":"TKBJRTYE03", "Filter": ' +
      filter +
      '}, "CommonParam": {} }';
    console.log(param);
    return this.webApiClientService.post<reportDatas>(
      AppConfig.domain.dbd,
      AppConfig.endpoints.DbdReportCreateSdoApi,
      param
    );
  }
  loadDataNgoaiPhongMo(
    search: SearchReport,
    page: number,
    paramData: ParamData
  ): Observable<reportDatas> {
    var thaybangbong = `'GAY0001','GAY0002','GAY0003','GAY0004'`;
    var noisoi = `'NOI5802','NOI5803','NOI5804','NOI5808','NOI5814','NOI5816','NOI5818','NOI5820','NOI8293','NOI8368','NOI8370','NOI8938','NOI8941','NOI9881','NOI9882','NOI9885','NOI9892','NOI9894','NỌ8289','NỌ8300','NỌ8310','PHA8257','PHA8319','PHA8320','PHA8327','PHA8329'`;
    var choctrung = `'CHO7990','CHO7932','CHO052_D','CHO7966'`;
    var canthiepmach = `'NOI0500','CHU8875','CHU8871','PHA6717','PHA6708','DAT0468'`;
    search.SQL =
      `*/
      select
      (case when ss.tdl_service_code in (` +
      thaybangbong +
      `) then 1 end) is_thaybangbong,
      (case when ss.tdl_service_code in (` +
      noisoi +
      `) then 1 end) is_noisoi,
      (case when ss.tdl_service_code in (` +
      choctrung +
      `) then 1 end) is_choctrung,
      (case when ss.tdl_service_code in (` +
      canthiepmach +
      `) then 1 end) is_canthiepmach,
      sum(ss.amount) amount
      from his_sere_serv ss
      join his_service_req sr on sr.id=ss.service_req_id
      left join his_department rdp on rdp.id=sr.request_department_id
      left join his_sere_serv_pttt ssp on ssp.sere_serv_id=ss.id
      join his_sere_serv_ext sse on sse.sere_serv_id=ss.id
      join v_his_room ero on ero.id=ss.tdl_execute_room_id
      where ss.is_delete=0 and ss.is_no_execute is null
      and ss.tdl_service_type_id in (4,11)
      `;
    search.SQL +=
      "and sse.end_time between " +
      search.TIME_FROM +
      " and " +
      search.TIME_TO +
      "";
    search.SQL +=
      `
      and ero.room_code not in ('GMHS1')
      and ss.tdl_service_code in (` +
      thaybangbong +
      `,` +
      noisoi +
      `,` +
      choctrung +
      `,` +
      canthiepmach +
      `)
      group by
      (case when ss.tdl_service_code in (` +
      thaybangbong +
      `) then 1 end),
      (case when ss.tdl_service_code in (` +
      noisoi +
      `) then 1 end),
      (case when ss.tdl_service_code in (` +
      choctrung +
      `) then 1 end),
      (case when ss.tdl_service_code in (` +
      canthiepmach +
      `) then 1 end)

/*`;
    let filter =
      '{"TIME_FROM":' +
      search.TIME_FROM +
      ',"TIME_TO":' +
      search.TIME_TO +
      ',"SQL":"' +
      search.SQL +
      '"';

    const param =
      '{ "ApiData":  { "ReportTypeCode": "TKBJRTYE","ReportTemplateCode":"TKBJRTYE03", "Filter": ' +
      filter +
      '}, "CommonParam": {} }';
    console.log(param);
    return this.webApiClientService.post<reportDatas>(
      AppConfig.domain.dbd,
      AppConfig.endpoints.DbdReportCreateSdoApi,
      param
    );
  }

  loadDataChuyenVien(
    search: SearchReport
  ): Observable<reportDatas> {
    search.SQL =
      `*/
      select
      count(1) COUNT_CHUYENVIEN
      from his_treatment trea
      where trea.tdl_treatment_type_id=1
      and trea.is_pause=1
      and trea.treatment_end_type_id=2
      `;
    search.SQL +=
      "and trea.out_time between " +
      search.TIME_FROM +
      " and " +
      search.TIME_TO +
      "";
    search.SQL +=
      `/*`;
    let filter =
      '{"TIME_FROM":' +
      search.TIME_FROM +
      ',"TIME_TO":' +
      search.TIME_TO +
      ',"SQL":"' +
      search.SQL +
      '"';

    const param =
      '{ "ApiData":  { "ReportTypeCode": "TKBJRTYE","ReportTemplateCode":"TKBJRTYE03", "Filter": ' +
      filter +
      '}, "CommonParam": {} }';
    console.log(param);
    return this.webApiClientService.post<reportDatas>(
      AppConfig.domain.dbd,
      AppConfig.endpoints.DbdReportCreateSdoApi,
      param
    );
  }

  loadDataNhaThuoc(
    search: SearchReport
  ): Observable<reportDatas> {
    search.SQL =
      `*/
      select sum(tran.amount) AMOUNT 
      from his_transaction tran 
      where 1=1
      and tran.transaction_type_id=3 
      and tran.sale_type_id=1 
      and tran.is_cancel is null
      `;
    search.SQL +=
      "and tran.transaction_date between " +
      search.TIME_FROM +
      " and " +
      search.TIME_TO +
      "";
    search.SQL +=
      `/*`;
    let filter =
      '{"TIME_FROM":' +
      search.TIME_FROM +
      ',"TIME_TO":' +
      search.TIME_TO +
      ',"SQL":"' +
      search.SQL +
      '"';

    const param =
      '{ "ApiData":  { "ReportTypeCode": "TKBJRTYE","ReportTemplateCode":"TKBJRTYE03", "Filter": ' +
      filter +
      '}, "CommonParam": {} }';
    console.log(param);
    return this.webApiClientService.post<reportDatas>(
      AppConfig.domain.dbd,
      AppConfig.endpoints.DbdReportCreateSdoApi,
      param
    );
  }

  loadDataHinhThucThu(
    search: SearchReport
  ): Observable<reportDatas> {
    search.SQL =
      `*/
      select sum(case when pf.pay_form_code='01' then tran.amount end) SUM_TIEN_MAT,
      sum(case when pf.pay_form_code='02' then tran.amount end) SUM_CK,
      sum(case when pf.pay_form_code='08' then tran.amount end) SUM_QR
      from his_transaction tran 
      join his_pay_form pf on pf.id=tran.pay_form_id
      and tran.is_cancel is null
      `;
    search.SQL +=
      "and tran.transaction_date between " +
      search.TIME_FROM +
      " and " +
      search.TIME_TO +
      "";
    search.SQL +=
      `/*`;
    let filter =
      '{"TIME_FROM":' +
      search.TIME_FROM +
      ',"TIME_TO":' +
      search.TIME_TO +
      ',"SQL":"' +
      search.SQL +
      '"';

    const param =
      '{ "ApiData":  { "ReportTypeCode": "TKBJRTYE","ReportTemplateCode":"TKBJRTYE03", "Filter": ' +
      filter +
      '}, "CommonParam": {} }';
    console.log(param);
    return this.webApiClientService.post<reportDatas>(
      AppConfig.domain.dbd,
      AppConfig.endpoints.DbdReportCreateSdoApi,
      param
    );
  }

  loadDataDonMau(
    search: SearchReport
  ): Observable<reportDatas> {
    search.SQL =
      `*/
      select sr.service_req_stt_id,count(1) amount 
      from his_service_req sr 
      left join his_exp_mest_blood eb on eb.tdl_service_req_id=sr.id and eb.is_delete=0
      where sr.service_req_type_id=16 
      and sr.is_delete=0 and sr.is_no_execute is null
      `;
    search.SQL +=
      "and sr.intruction_date between " +
      search.TIME_FROM +
      " and " +
      search.TIME_TO +
      " group by sr.service_req_stt_id";
    search.SQL +=
      `/*`;
    let filter =
      '{"TIME_FROM":' +
      search.TIME_FROM +
      ',"TIME_TO":' +
      search.TIME_TO +
      ',"SQL":"' +
      search.SQL +
      '"';

    const param =
      '{ "ApiData":  { "ReportTypeCode": "TKBJRTYE","ReportTemplateCode":"TKBJRTYE03", "Filter": ' +
      filter +
      '}, "CommonParam": {} }';
    console.log(param);
    return this.webApiClientService.post<reportDatas>(
      AppConfig.domain.dbd,
      AppConfig.endpoints.DbdReportCreateSdoApi,
      param
    );
  }

  loadDataXNGPB(
    search: SearchReport
  ): Observable<reportDatas> {
    search.SQL =
      `*/
      select er.department_code,sr.service_req_stt_id,
count(1) amount
from his_service_req sr
join v_his_execute_room er on er.room_id=sr.execute_room_id
where sr.is_delete=0
and sr.service_req_type_id in (2,13)
and sr.is_no_execute is null
and er.department_code in ('','K36','K37','K38','K42','K46')
      `;
    search.SQL +=
      "and sr.intruction_date between " +
      search.TIME_FROM +
      " and " +
      search.TIME_TO +
      ` group by
      er.department_code,sr.service_req_stt_id`
    search.SQL +=
      `/*`;
    let filter =
      '{"TIME_FROM":' +
      search.TIME_FROM +
      ',"TIME_TO":' +
      search.TIME_TO +
      ',"SQL":"' +
      search.SQL +
      '"';

    const param =
      '{ "ApiData":  { "ReportTypeCode": "TKBJRTYE","ReportTemplateCode":"TKBJRTYE03", "Filter": ' +
      filter +
      '}, "CommonParam": {} }';
    console.log(param);
    return this.webApiClientService.post<reportDatas>(
      AppConfig.domain.dbd,
      AppConfig.endpoints.DbdReportCreateSdoApi,
      param
    );
  }

  loadDataPttt(
    search: SearchReport
  ): Observable<reportDatas> {
    search.SQL =
      `*/
      select count(1) amount,
      (case when ssp.pttt_priority_id=2 then 1 end) is_emergency,
      (case when sr.execute_room_id=983 then 1 end) is_gmhs,
      ss.tdl_service_type_id,
      sr.service_req_stt_id
      from his_service_req sr 
      join his_sere_serv ss on ss.service_req_id=sr.id and ss.is_delete=0 and ss.is_no_execute is null
      left join his_sere_serv_pttt ssp on ssp.sere_serv_id=ss.id and ssp.is_delete=0
      where sr.is_delete=0 and sr.is_no_execute is null
      and ss.tdl_service_type_id in (4,11)
      
      `;
    search.SQL +=
      "and sr.intruction_date between " +
      search.TIME_FROM +
      " and " +
      search.TIME_TO +
      ` group by
      (case when ssp.pttt_priority_id=2 then 1 end),
      (case when sr.execute_room_id=983 then 1 end),
      ss.tdl_service_type_id,
      sr.service_req_stt_id`
    search.SQL +=
      `/*`;
    let filter =
      '{"TIME_FROM":' +
      search.TIME_FROM +
      ',"TIME_TO":' +
      search.TIME_TO +
      ',"SQL":"' +
      search.SQL +
      '"';

    const param =
      '{ "ApiData":  { "ReportTypeCode": "TKBJRTYE","ReportTemplateCode":"TKBJRTYE03", "Filter": ' +
      filter +
      '}, "CommonParam": {} }';
    console.log(param);
    return this.webApiClientService.post<reportDatas>(
      AppConfig.domain.dbd,
      AppConfig.endpoints.DbdReportCreateSdoApi,
      param
    );
  }
  
  loadDataCthiep(
    search: SearchReport
  ): Observable<reportDatas> {
    search.SQL = `*/
select 
sr.service_req_stt_id,
(case when exro.room_code in ('CTXQ') then 1 end) is_ctxq,
(case when exro.room_code in ('CTTM1') then 1 end) is_cttm,
(case when exro.room_code in ('ĐQ01') then 1 end) is_ctdq,
sum(ss.amount) amount
from his_sere_serv ss
join his_service_req sr on sr.id=ss.service_req_id
join v_his_room exro on exro.id=sr.execute_room_id
where ss.is_delete=0
and ss.is_no_execute is null
and ss.service_req_id is not null
`;
    search.SQL +=
      "and sr.intruction_date between " +
      search.TIME_FROM +
      " and " +
      search.TIME_TO +
      "";
    search.SQL += `
and exro.room_code in ('CTXQ','CTTM1','ĐQ01')
group by
sr.service_req_stt_id,
(case when exro.room_code in ('CTXQ') then 1 end),
(case when exro.room_code in ('CTTM1') then 1 end),
(case when exro.room_code in ('ĐQ01') then 1 end)
/*`;
    let filter =
      '{"TIME_FROM":' +
      search.TIME_FROM +
      ',"TIME_TO":' +
      search.TIME_TO +
      ',"SQL":"' +
      search.SQL +
      '"';

    const param =
      '{ "ApiData":  { "ReportTypeCode": "TKBJRTYE","ReportTemplateCode":"TKBJRTYE03", "Filter": ' +
      filter +
      '}, "CommonParam": {} }';
    console.log(param);
    return this.webApiClientService.post<reportDatas>(
      AppConfig.domain.dbd,
      AppConfig.endpoints.DbdReportCreateSdoApi,
      param
    );
  }

  loadDataTbKham(search: SearchReport, page: number, paramData: ParamData): Observable<reportDatas> {
    let filter = '{"TIME_FROM":'+search.TIME_FROM+',"TIME_TO":'+search.TIME_TO+'}';        
    const param = '{ "ApiData":  { "ReportTypeCode": "MRS00181","ReportTemplateCode":"MRS0018101", "Filter": ' + filter + '}, "CommonParam": {} }';
console.log(param);
    return this.webApiClientService.post<reportDatas>(AppConfig.domain.dbd, AppConfig.endpoints.DbdReportCreateSdoApi, param);
}

loadDataTopIcd(
  search: SearchReport
): Observable<reportDatas> {
  search.SQL = `*/
  select trea.icd_code x,
  count(1) y
  from his_treatment trea
  where trea.icd_code is not null
 
`;
  search.SQL +=
    "and trea.in_date between " +
    search.TIME_FROM +
    " and " +
    search.TIME_TO +
    "";
  search.SQL += `
  group by trea.icd_code
  order by y desc
  fetch first 20 rows only
/*`;
  let filter =
    '{"TIME_FROM":' +
    search.TIME_FROM +
    ',"TIME_TO":' +
    search.TIME_TO +
    ',"SQL":"' +
    search.SQL +
    '"';

  const param =
    '{ "ApiData":  { "ReportTypeCode": "TKBJRTYE","ReportTemplateCode":"TKBJRTYE03", "Filter": ' +
    filter +
    '}, "CommonParam": {} }';
  console.log(param);
  return this.webApiClientService.post<reportDatas>(
    AppConfig.domain.dbd,
    AppConfig.endpoints.DbdReportReloadSdoApi,
    param
  );
}

loadDataTrongBuong(
  search: SearchReport
): Observable<reportDatas> {
  search.SQL = `*/
 select ro.department_name x,
count(1) y 
from his_treatment_bed_room tbr
join his_treatment trea on trea.id=tbr.treatment_id
join v_his_bed_room ro on ro.id=tbr.bed_room_id 
where trea.tdl_treatment_type_id=2 and tbr.remove_time >20260925000000 and tbr.add_time<20260925000000 
group by ro.department_name
/*`;
  let filter =
    '{"TIME_FROM":' +
    search.TIME_FROM +
    ',"TIME_TO":' +
    search.TIME_TO +
    ',"SQL":"' +
    search.SQL +
    '"';

  const param =
    '{ "ApiData":  { "ReportTypeCode": "TKBJRTYE","ReportTemplateCode":"TKBJRTYE03", "Filter": ' +
    filter +
    '}, "CommonParam": {} }';
  console.log(param);
  return this.webApiClientService.post<reportDatas>(
    AppConfig.domain.dbd,
    AppConfig.endpoints.DbdReportReloadSdoApi,
    param
  );
}

loadDataVao(
  search: SearchReport
): Observable<reportDatas> {
  search.SQL = `*/
 select dp.department_name x,
count(1) y 
from his_treatment trea
join his_department dp on dp.id=trea.last_department_id 
where trea.tdl_treatment_type_id=2 
`;
  search.SQL +=
    "and trea.in_date between 20260925000000" +
    // search.TIME_FROM +
    " and 20260925000000" +
    // search.TIME_TO +
    "";
  search.SQL += `
group by dp.department_name
/*`;
  let filter =
    '{"TIME_FROM":' +
    search.TIME_FROM +
    ',"TIME_TO":' +
    search.TIME_TO +
    ',"SQL":"' +
    search.SQL +
    '"';

  const param =
    '{ "ApiData":  { "ReportTypeCode": "TKBJRTYE","ReportTemplateCode":"TKBJRTYE03", "Filter": ' +
    filter +
    '}, "CommonParam": {} }';
  console.log(param);
  return this.webApiClientService.post<reportDatas>(
    AppConfig.domain.dbd,
    AppConfig.endpoints.DbdReportReloadSdoApi,
    param
  );
}
}
