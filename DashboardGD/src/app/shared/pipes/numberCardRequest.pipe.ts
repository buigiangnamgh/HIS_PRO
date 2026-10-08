import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'numberCardStatus', pure: true })
export class NumberCardStatus implements PipeTransform {
    transform(value: number, args: string[]): any {
        // Them so 0 vao sau
        var length =  value.toString().length;
        var returnValue = "";
        for(var c = 0; c < 9 - length; c++) {
            returnValue = returnValue + "0";
        }
        returnValue = returnValue + value.toString();
        return returnValue;
    }
}
