import {Injectable} from '@angular/core';
import {CalendarUtils} from 'angular-calendar';
import {WeekView, GetWeekViewArgs} from 'calendar-utils';
import {addMinutes} from './date.utils';
export interface Truck {
  id: number;
  name: string;
}
export interface DayViewScheduler extends WeekView {
  trucks: Truck[];
}
interface GetWeekViewArgsWithTrucks extends GetWeekViewArgs {
  trucks: Truck[];
}
@Injectable()
export class DayViewSchedulerCalendarUtils extends CalendarUtils {
  getWeekView(args: GetWeekViewArgsWithTrucks): DayViewScheduler {
    const {period} = super.getWeekView(args);
    const view: DayViewScheduler = {
      period,
      allDayEventRows: [],
      hourColumns: [],
      trucks: [...args.trucks],
    };
    view.trucks.forEach((truck, columnIndex) => {
      const events = args.events.filter(event => {
        if (event.meta?.truck?.id === truck.id) {
          return event;
        }
      });
      const columnView = super.getWeekView({
        ...args,
        events,
      });
      view.hourColumns.push(columnView.hourColumns[0]);
      columnView.allDayEventRows.forEach(({row}, rowIndex) => {
        view.allDayEventRows[rowIndex] = view.allDayEventRows[rowIndex] || {
          row: [],
        };
        view.allDayEventRows[rowIndex].row.push({
          ...row[0],
          offset: columnIndex,
          span: 1,
        });
      });
    });
    return view;
  }
}
