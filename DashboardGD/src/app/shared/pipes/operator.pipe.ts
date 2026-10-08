
import { Pipe, PipeTransform } from '@angular/core';


@Pipe({ name: 'operator', pure: true })
export class OperatorPipe implements PipeTransform {
    transform(value: number, args: string[]): any {
        return value == 1 ? 'Android' : (value == 2 ? 'iOS' : '');
    }
}