import { Observable, of, throwError as observableThrowError } from "rxjs";
import { AppConfig } from "../../config/app.config";
import { Injectable } from "@angular/core";
import "rxjs/add/operator/toPromise";
import "rxjs/add/operator/catch";
import { WebApiClientService } from "../../core/webApiClient.service";
import { String, StringBuilder } from "typescript-string-operations";
import { reportDatas } from "../../model/report";
import { SearchReport } from "../../model/Search.model";
import { ParamData } from "../../model/apidata.model";

@Injectable()
export class xetnghiemsinhhoaService {
  constructor(private webApiClientService: WebApiClientService) {}
 

}
