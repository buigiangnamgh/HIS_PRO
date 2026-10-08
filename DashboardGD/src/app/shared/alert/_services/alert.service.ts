import { Injectable } from '@angular/core';
import { Router, NavigationStart } from '@angular/router';
import { Observable } from 'rxjs';
import { Subject } from 'rxjs/Subject';
import 'rxjs/add/operator/filter';
import { ToastrService } from 'ngx-toastr';
import { Alert, AlertType } from '../_models/alert';
import { AppConfig } from '../../../config/app.config';
import { ParamData } from '../../../model/apidata.model';

@Injectable()
export class AlertService {
    private subject = new Subject<Alert>();
    private keepAfterRouteChange = false;

    constructor(private router: Router,private toastr: ToastrService) {
        // clear alert messages on route change unless 'keepAfterRouteChange' flag is true
        router.events.subscribe(event => {
            let othis = this;
            if (event instanceof NavigationStart) {
                if (this.keepAfterRouteChange) {
                    // only keep for a single route change
                    this.keepAfterRouteChange = false;
                    setTimeout(function(){othis.clear();}, AppConfig.common.AlertTimeOut);
                } else {
                    // clear alert messages
                    this.clear();
                }
            }
        });
    }

    // subscribe to alerts
    getAlert(alertId?: string): Observable<any> {
        return this.subject.asObservable().filter((x: Alert) => x && x.alertId === alertId);
    }

    showMessage(param: ParamData, success: boolean) {
        let message: string = "";
        if (param != null){
            if (param.Messages.length>0)
            {
                message += param.Messages.join("br/");
            } 
            if (param.BugCodes.length>0)
            {
                message += param.BugCodes.join(";");
            }
        }

        if(success)
        {
            message = "Xử lý thành công." + message;            
        }
        else{
            message = "Xử lý thất bại." + message;          
        }

        this.alert(new Alert({ message, type: (success? AlertType.Success: AlertType.Error) }));
    }
    
    showMessageToas(param: ParamData, success: boolean) {
        let message: string = "";
        if (param != null){
            if (param.Messages.length>0)
            {
                message += param.Messages.join("br/");
            } 
            if (param.BugCodes.length>0)
            {
                message += param.BugCodes.join(";");
            }
        }
        if(success)
        {
            message = "Xử lý thành công." + message;
            this.toastr.success(message, "Thông báo");
        }
        else{
            message = "Xử lý thất bại." + message;
            this.toastr.error(message, "Thông báo");
        }

    }

    show(message: string, success: boolean) {        
        this.alert(new Alert({ message, type: (success? AlertType.Success: AlertType.Error) }));
    }

    // convenience methods
    success(message: string) {
        this.alert(new Alert({ message, type: AlertType.Success }));
    }

    error(message: string) {
        this.alert(new Alert({ message, type: AlertType.Error }));
    }

    info(message: string) {
        this.alert(new Alert({ message, type: AlertType.Info }));
    }

    warn(message: string) {
        this.alert(new Alert({ message, type: AlertType.Warning }));
    }

    // main alert method    
    alert(alert: Alert) {
        if(alert.keepAfterRouteChange === undefined)
            this.keepAfterRouteChange = true;
        else
            this.keepAfterRouteChange = alert.keepAfterRouteChange;
        this.subject.next(alert);
    }

    // clear alerts
    clear(alertId?: string) {
        this.subject.next(new Alert({ alertId }));
    }
}