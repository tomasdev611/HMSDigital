import {SieveOperators} from '../enums';
const sieveOperators = SieveOperators;

export function buildFilterString(filterValues) {
  filterValues = filterValues.filter(filter => filter.value && filter.value.length);
  const filterByField = filterValues.map((filter: any) => {
    if (filter.value[0] && !Array.isArray(filter.value[0]) && typeof filter.value[0] === 'object') {
      return buildFilterString(filter.value);
    }
    if (Array.isArray(filter.fields) && filter.fields.length) {
      return filter.fields.join('|') + filter.operator + filter.value.join('|');
    }
    return filter.field + filter.operator + filter.value.join('|');
  });
  return filterByField.join(',');
}

export function getObjectFromFilterString(filterString: any, rangeFilterField: string) {
  Object.values(sieveOperators).map(x => {
    const regex = new RegExp(x, 'g');
    filterString = filterString.replace(regex, '||');
  });
  if (filterString) {
    const filterArray = filterString.split(',').map(x => x.split('||'));
    const dateRangeValues = filterArray.filter(item => item[0] === rangeFilterField).map(e => e[1]);
    const values = Object.fromEntries(
      filterArray
        .filter(item => item[0] !== rangeFilterField)
        .concat([[rangeFilterField, dateRangeValues.join('|')]])
    );
    return values;
  } else {
    return null;
  }
}
