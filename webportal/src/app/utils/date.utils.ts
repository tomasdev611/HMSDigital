import * as moment from 'moment';
const offset = new Date().getTimezoneOffset() * 60000;

export function formatDateToString(date, format: string) {
  return moment(date).format(format);
}

export function addDays(date, noOfDays) {
  return moment(date).add(noOfDays, 'days');
}

export function changeToISO(date) {
  return date ? new Date(date).toISOString() : null;
}

export function changeToISOPreservingTimezone(date) {
  return date ? new Date(date.getTime() - offset).toISOString() : null;
}

export function addMinutes(date, minutes) {
  return moment(date).add(minutes, 'm');
}

export function convertMinToTime(minutes) {
  return `0${Math.floor(minutes / 60)}`.slice(-2) + ':' + ('0' + (minutes % 60)).slice(-2) + ':00';
}

export function subtractHours(date, hours) {
  return moment(date).subtract(hours, 'h');
}

export function getUTCDateAsLocalDate(date) {
  const d = moment.utc(date).format('YYYY-MM-DD');
  return moment(d).toDate();
}

export function getSeconds(time: string) {
  return moment.duration(time).asSeconds();
}

export function getDifference(dateA, dateB, format, precise = false) {
  return moment(dateA).diff(moment(dateB), format, precise);
}

export function getUTCDate(date, format = 'YYYY-MM-DD') {
  return moment.utc(date).format(format);
}

export function setDateFromInlineInput(date) {
  const dateTime = date.split(' ');
  const dateOnly = dateTime[0].split('/');
  const timeOnly = dateTime[1] ?? null;
  const year = dateOnly[dateOnly.length - 1];
  if (date && !timeOnly && dateOnly.length > 2 && year?.length === 4) {
    const dateToAppend = new Date(`${date} 09:00 AM`);
    if (!isNaN(dateToAppend.valueOf())) {
      return dateToAppend;
    }
  }
  return null;
}
