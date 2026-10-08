import { Pipe, PipeTransform } from '@angular/core';


@Pipe({ name: 'notification', pure: true })
export class NotificationMobilePipe implements PipeTransform {
    transform(value: number, args: string[]): any {
        return value == 0 ? 'Thông báo' : 'Cảnh báo nâng cấp';
    }
}