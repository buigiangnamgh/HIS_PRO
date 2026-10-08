import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'required' })
export class RequiredPipe implements PipeTransform {
  transform(value: string) {
    return value + " " + "*".fontcolor('#CC3300');
  }
}