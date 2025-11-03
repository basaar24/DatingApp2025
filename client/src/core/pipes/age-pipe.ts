import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'age'
})
export class AgePipe implements PipeTransform {

  transform(value: string): number {
    const today = new Date();
    const birthDay = new Date(value);

    let age = today.getFullYear() - birthDay.getFullYear();
    const monthDiff = today.getMonth() - birthDay.getMonth();

    if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < birthDay.getDate())) {
      age--;
    }

    return age;
  }
}
