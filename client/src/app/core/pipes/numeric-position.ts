import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'numericPosition'
})
export class NumericPositionPipe implements PipeTransform {

  transform(number: number, args?: any): any {
    if (isNaN(number)) return null; // will only work value is a number
    if (number === null) return null;
    if (number === 1) return '1st';
    if (number === 2) return '2nd';
    if (number === 3) return '3rd';
    return `${number}th`;
  }
}
