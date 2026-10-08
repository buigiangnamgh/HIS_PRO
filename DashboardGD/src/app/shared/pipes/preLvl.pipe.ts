import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'preLvl' })
export class PreLvlPipe implements PipeTransform {
    
    transform(value: number, args: string[]): any {
        let preLvl = '';
        for (let i = 1; i < value; i++) {
            preLvl += args[0];
        }
        return preLvl;
    }
}
