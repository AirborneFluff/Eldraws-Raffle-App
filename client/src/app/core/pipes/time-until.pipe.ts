import { Pipe, PipeTransform } from '@angular/core';

enum Periods {
  Second = 1000,
  Minute = 60000,
  Hour = 3600000,
  Day = 86400000
}

@Pipe({
  name: 'timeUntil'
})
export class TimeUntilPipe implements PipeTransform {

  transform(value: string | Date, ...args: unknown[]): unknown {
    const date = Date.parse(value.toString());

    const timeDiff = date - new Date().getTime();

    if (timeDiff >= 2 * Periods.Day) return `${(timeDiff / Periods.Day).toFixed()} days`
    if (timeDiff >= 2 * Periods.Hour) return `${(timeDiff / Periods.Hour).toFixed()} hours`
    if (timeDiff >= 5 * Periods.Minute) return `${(timeDiff / Periods.Minute).toFixed()} minutes`

    return `${(timeDiff / Periods.Second).toFixed()} seconds`;
  }

}
