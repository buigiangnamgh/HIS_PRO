
import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'statusNews', pure: true })
export class StatusPipe implements PipeTransform {
  transform(value: number): any {
    if (value === 0) {
      return '<span class="label label-warning">Chờ duyệt</span>';
    } else if (value === 1) {
      return '<span class="label label-success">Đã duyệt</span>';
    } else {
      return '<span class="label label-danger">Không duyệt</span>';
    }
  }
}