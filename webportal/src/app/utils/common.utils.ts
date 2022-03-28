import {compare} from 'fast-json-patch';
import {
  cloneDeep,
  groupBy,
  takeRight,
  get,
  keys,
  uniq,
  sum,
  values,
  xor,
  filter,
  mergeWith,
  merge,
  isArray,
  orderBy,
  isEqual,
} from 'lodash-es';
import * as forms from '../constants/constant.forms';
import {EnumNames} from '../enums';
import {getEnum} from './enum-utils';

function mergeCustomizer(obj, src) {
  if (isArray(obj)) {
    const ret = [];
    src.forEach((srcItem, index) => {
      const merged = obj.length - index > 0 ? merge({}, obj[index], srcItem) : srcItem;
      ret.push(merged);
    });
    return ret;
  }
}

export function removeDuplicatesInArray(array, field) {
  const uniqueList = [];
  array.filter(item => {
    const i = uniqueList.findIndex(x => x[field] === item[field]);
    if (i <= -1) {
      uniqueList.push(item);
    }
  });
  return uniqueList;
}

export function deepCloneObject(srcObj) {
  return cloneDeep(srcObj);
}
// Create Patch
export function createPatch(actualdata, editeddata) {
  return compare(actualdata, editeddata);
}

export function getObjectValueByKey(object: any, key: string) {
  return get(object, key);
}

export function getObjectKeys(ojbect: any) {
  return keys(ojbect);
}

export function deepMerge(actualdata, editeddata) {
  return mergeWith({}, actualdata, editeddata, mergeCustomizer);
}

export function sortBy(list, field, fieldType = 'normal') {
  return list.sort((a: any, b: any) => {
    if (fieldType === 'normal') {
      if (a[field] < b[field]) {
        return -1;
      }
      if (a[field] > b[field]) {
        return 1;
      }
      return 0;
    }
    if (fieldType === 'date') {
      const date1 = new Date(a[field]);
      const date2 = new Date(b[field]);
      if (date2 < date1) {
        return -1;
      }
      if (date2 > date1) {
        return 1;
      }
      return 0;
    }
  });
}

export function groupByField(list, field: string) {
  return groupBy(list, field);
}
export function sliceArrayRight(list, nos: number) {
  return takeRight(list, nos);
}

export function getUniqArray(array) {
  return uniq(array);
}

export function showRequiredFields(form, formConstant, extraRequiredFields = []) {
  const formModel = forms[formConstant];
  let errorFields = [];
  errorFields = getInvalidFields(formModel, form.controls, []).flat(Infinity);
  errorFields = getUniqArray(errorFields);
  let content = ``;
  if (errorFields.length > 0 || extraRequiredFields?.length) {
    content += `<span>Required fields are not complete</span><ul>`;
    errorFields.map(field => {
      content += `<li>${field}</li>`;
    });
    extraRequiredFields.map(exField => {
      content += `<li>${exField}</li>`;
    });
    content += `</ul>`;
  }
  return content;
}

function getInvalidFields(formModel, controls, errors) {
  for (const key in controls) {
    if (controls.hasOwnProperty(key)) {
      const currentControl = controls[key];
      if (currentControl.controls && Object.keys(currentControl.controls).length > 0) {
        errors.push(getInvalidFields(formModel, currentControl.controls, []));
      } else if (currentControl.errors) {
        const field = formModel.find(x => x.value === key);
        if (field) {
          errors.push(field?.label);
        }
      }
    }
  }
  return errors;
}

export function getSum(value) {
  return sum(value);
}

export function convertObjectToArray(object) {
  return values(object);
}

export function scrollTo(element, block = 'center', behavior = 'smooth') {
  element.scrollIntoView({
    block,
    behavior,
  });
}

export function getDifferenceArray(array1, array2) {
  return xor(array1, array2);
}

export function getFormattedPhoneNumber(phone) {
  if (Number.isInteger(phone)) {
    const phoneString = phone.toString();
    return `(${phoneString.slice(0, 3)}) ${phoneString.slice(3, 6)}-${phoneString.slice(6)}`;
  }
  return '';
}

export function filterByString(list, field, query) {
  return filter(list, item => item[field].toLowerCase().indexOf(query.toLowerCase()) > -1);
}

export function filterByFields(list, fields, query) {
  let filteredList = [];
  if (list.length > 0) {
    query = query.toLowerCase();
    // IF THERE IS PROPERTY LIST, FILTER USING ONLY THE PROPERTIES ON THE LIST
    if (fields && fields.length > 0) {
      list.forEach(item => {
        for (const key in item) {
          // LOOP THROUGH THE PROPERTY TYPES OF EACH ITEM
          if (fields.indexOf(key) > -1) {
            if (item[key] && item[key].toString().toLowerCase().indexOf(query) > -1) {
              filteredList = [...filteredList, item];
              break;
            }
          }
        }
      });
    }
  } else {
    return list;
  }
  return filteredList;
}

export function convertArrayToTree(list, id, parentId, childPropertyName) {
  const arrMap = new Map(list.map(item => [item[id], item]));
  let tree = [];
  list.map(item => {
    if (item[parentId]) {
      const parentItem = arrMap.get(item[parentId]);
      if (parentItem) {
        if (parentItem[childPropertyName]) {
          parentItem[childPropertyName] = [...parentItem[childPropertyName], item];
        } else {
          parentItem[childPropertyName] = [item];
        }
      }
    } else {
      tree = [...tree, item];
    }
  });
  return tree;
}

export function getIsInternalUser() {
  const isInternalUser = localStorage.getItem('isInternalUser');
  if (isInternalUser) {
    return isInternalUser === 'true';
  } else {
    return checkInternalUser();
  }
}

export function checkInternalUser() {
  let myInfo: any = localStorage.getItem('me');
  let roles: any = localStorage.getItem('roles');
  let internalUser = false;
  roles = roles ? JSON.parse(roles) : null;
  roles = roles ? Object.values(roles) : [];
  myInfo = myInfo ? JSON.parse(myInfo) : null;
  const userRoles = myInfo?.userRoles ?? [];
  userRoles.map(x => {
    const userRole = roles.find(role => role?.name?.toLowerCase() === x?.roleName?.toLowerCase());
    if (userRole?.roleType === 'Internal') {
      internalUser = true;
      return;
    }
  });
  return internalUser;
}

export function getRoleById(id) {
  let roles: any = localStorage.getItem('roles');
  roles = roles ? JSON.parse(roles) : null;
  roles = roles ? Object.values(roles) : [];

  const role = roles.find(r => r.id === id);
  return role;
}

export function SortByField(list: any, fields: []) {
  const sortFields: any = [];
  const sortingOrder: any = [];
  fields.forEach((field: any) => {
    const order = field.charAt(0) === '-' ? 'desc' : 'asc';
    sortFields.push(order === 'desc' ? field.substring(1) : field);
    sortingOrder.push(order);
  });
  return orderBy(list, sortFields, sortingOrder);
}

export function stringEqualsIgnoreCase(string1: string, string2: string) {
  if (typeof string1 === 'string' && typeof string2 === 'string') {
    return string1.localeCompare(string2, undefined, {sensitivity: 'accent'}) === 0;
  }
  return string1 === string2;
}

export function encode(data: any) {
  return btoa(data);
}

export function decode(encodedString: string) {
  return atob(encodedString);
}

export function checkEqualArray(array1, array2) {
  return isEqual(array1.sort(), array2.sort());
}
