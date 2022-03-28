export enum EditableTableInputTypes {
  inputText = 'text',
  inputNumeric = 'numeric',
  inputDropdown = 'dropdown',
  inputSwitch = 'switch',
}

export interface DropdownOptions {
  label: string;
  value: string;
}

/*
Schema used to create editable table:
{
  readOnly: boolean
  label: disaply label for table,
  key: key to search for value in data schema
  (optional Field) type: can be one of the follwoings [text, numeric, dropdown, switch]
  (optional Field) options: array of values for dropdown [{label: xyz, value: zyx}, ...],
}
*/
export interface EditableTableColumn {
  readOnly: boolean;
  label: string;
  key: string;
  type?:
    | EditableTableInputTypes.inputText
    | EditableTableInputTypes.inputNumeric
    | EditableTableInputTypes.inputDropdown
    | EditableTableInputTypes.inputSwitch;
  options?: DropdownOptions[];
  size?: string;
}
