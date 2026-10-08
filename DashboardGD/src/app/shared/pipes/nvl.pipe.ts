import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'nvl', pure: true })
export class NvlPipe implements PipeTransform {
    transform(value: any, args: string[]): any {
        return value != null ? value : args[0];
    }
}
