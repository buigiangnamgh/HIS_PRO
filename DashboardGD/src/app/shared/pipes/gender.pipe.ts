import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'gender', pure: true })
export class Gender implements PipeTransform {
    transform(value: number, args: string[]): any {
        return value == 0 ? 'Nam' : (value == 1 ? 'Nữ' : 'Chưa biết');
    }
}
