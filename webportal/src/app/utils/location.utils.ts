import {getStatesOfCountry, getCitiesOfState} from '@tgrx/country-state-city';
import states from 'states-us';
const list = {
  city: [],
  state: [],
};
list.city = [
  ...new Set(
    getStatesOfCountry(231).reduce((cities, val) => {
      // 231 is countryCode Id of US in the packageJson of library
      return [...cities, ...getCitiesOfState(val.id).map(y => y.name)];
    }, [])
  ),
];
list.state = states.map(x => {
  return {label: x.name, value: x.abbreviation};
});
export function getDistinctList(field, query) {
  return list[field].filter(x => x && x.toString().toLowerCase().includes(query.toLowerCase()));
}
export function getStates(query) {
  return list.state.filter(
    x =>
      x &&
      (x.label.toString().toLowerCase().includes(query.toLowerCase()) ||
        x.value.toString().toLowerCase().includes(query.toLowerCase()))
  );
}
