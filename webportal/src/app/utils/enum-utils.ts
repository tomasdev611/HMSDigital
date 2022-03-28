import {EnumNames} from '../enums';

export function getEnum(name: string) {
  const enums = JSON.parse(localStorage.getItem('enum'));
  let result = enums && enums[name] ? enums[name] : [];
  if (EnumNames.OrderTypes === name) {
    result = result.filter(r => r.name !== 'Respite');
  }
  return result;
}
