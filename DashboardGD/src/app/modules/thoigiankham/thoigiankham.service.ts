import {Observable} from 'rxjs';
import {AppConfig} from '../../config/app.config';
import { Injectable } from '@angular/core';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/catch';
import {WebApiClientService} from '../../core/webApiClient.service';
import { reportDatas } from "../../model/report";
import { SearchReport } from '../../model/Search.model';
import { ParamData } from "../../model/apidata.model";

@Injectable()
export class thoigiankhamService {
    constructor(private webApiClientService: WebApiClientService) {
    }

    // Trung bình thời gian chờ & thực hiện khám theo phòng (SQL trực tiếp qua TKBJRTYE03)
    // Kết quả: t.Data.DATA_DETAIL.Report0 -> ROOM_CODE, ROOM_NAME, IS_CSSK, CNT, AVG_WAIT, AVG_EXEC (phút)
    //   thời gian chờ      = start_time - intruction_time
    //   thời gian thực hiện = finish_time - start_time
    loadData(search: SearchReport, page: number, paramData: ParamData): Observable<reportDatas> {
        const sql = `*/
select
  exro.room_code,
  exro.room_name,
  (case when upper(exro.room_name) like '%YÊU CẦU%' then 1 else 0 end) is_cssk,
  count(*) cnt,
  round(avg((to_date(to_char(sr.start_time),'yyyymmddhh24miss') - to_date(to_char(sr.intruction_time),'yyyymmddhh24miss')) * 1440)) avg_wait,
  round(avg((to_date(to_char(sr.finish_time),'yyyymmddhh24miss') - to_date(to_char(sr.start_time),'yyyymmddhh24miss')) * 1440)) avg_exec
from his_service_req sr
join v_his_room exro on exro.id = sr.execute_room_id
where sr.is_delete = 0
  and sr.is_no_execute is null
  and sr.service_req_type_id = 1
  and sr.service_req_stt_id = 3
  and sr.intruction_time between ${search.TIME_FROM} and ${search.TIME_TO}
  and sr.start_time is not null
  and sr.finish_time is not null
  and sr.start_time >= sr.intruction_time
  and sr.finish_time >= sr.start_time
group by exro.room_code, exro.room_name,
  (case when upper(exro.room_name) like '%YÊU CẦU%' then 1 else 0 end)
order by exro.room_name
/*`;
        const filter = '{"TIME_FROM":' + search.TIME_FROM + ',"TIME_TO":' + search.TIME_TO + ',"SQL":"' + sql + '"}';
        const param = '{ "ApiData":  { "ReportTypeCode": "TKBJRTYE","ReportTemplateCode":"TKBJRTYE03", "Filter": ' + filter + '}, "CommonParam": {} }';
        return this.webApiClientService.post<reportDatas>(AppConfig.domain.dbd, AppConfig.endpoints.DbdReportReloadSdoApi, param);
    }
}
