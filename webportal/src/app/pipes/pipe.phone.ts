import {Pipe, PipeTransform} from '@angular/core';

@Pipe({name: 'formatPhoneNumber'})
export class PhonePipe implements PipeTransform {
  transform(num: number) {
    const s = ('' + num).replace(/\D/g, '');
    const m = s.match(/^(\d{3})(\d{3})(\d{4})$/);
    return !m ? null : '(' + m[1] + ') ' + m[2] + '-' + m[3];
  }
}
