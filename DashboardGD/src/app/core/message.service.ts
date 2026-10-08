import { Component } from '@angular/core';
import { AlertComponent } from 'ngx-bootstrap/alert/alert.component';
import { AppConfig } from '../config/app.config';
 
@Component({
  selector: 'message-alert-timeout',
  template: '<div *ngFor="let alert of alerts"><alert [type]="alert.type" [dismissOnTimeout]="alert.timeout" (onClosed)="onClosed(alert)">{{ alert.msg }}</alert></div>'
})
export class MessageService {
    public alerts: any[];

    public add(): void {
        this.alerts.push({
        type: 'info',
        msg: `This alert will be closed in 5 seconds (added: ${new Date().toLocaleTimeString()})`,
        timeout: 5000
        });
    }

    onClosed(dismissedAlert: AlertComponent): void {
        this.alerts = this.alerts.filter(alert => alert !== dismissedAlert);
    }
}