export const PatientForm = [
  {label: 'First Name', value: 'firstName'},
  {label: 'Last Name', value: 'lastName'},
  {label: 'Parent Company', value: 'hospiceId'},
  {label: 'Date Of Birth', value: 'dateOfBirth'},
  {label: 'Address Type', value: 'addressType'},
  {label: 'Patient Height', value: 'patientHeight'},
  {label: 'Patient Height Feet', value: 'patientHeightFeet'},
  {label: 'Patient Height Inch', value: 'patientHeightInch'},
  {label: 'Patient Weight', value: 'patientWeight'},
  {label: 'Primary Number', value: 'number'},
  {label: 'Status', value: 'status'},
  {label: 'Status Changed Date', value: 'statusChangedDate'},
  {label: 'Emergency OptOut', value: 'emergencyOptOut'},
  {label: 'Emergency Contact Name', value: 'emergencyContactName'},
  {label: 'Emergency Contact Phone', value: 'emergencyContactNumber'},
  {label: 'Infectious Patient', value: 'isInfectious'},
  {label: 'Hospice Location', value: 'hospiceLocationId'},
  {label: 'Facility', value: 'facilityId'},
  {label: 'Patient Notes', value: 'patientNotes'},
  {label: 'Patient Address', value: 'patientAddress'},
  {label: 'Street Address', value: 'addressLine1'},
  {label: 'Apt / Unit / Building', value: 'addressLine2'},
  {label: 'Country', value: 'country'},
  {label: 'State', value: 'state'},
  {label: 'City', value: 'city'},
  {label: 'Zip Code', value: 'zipCode'},
  {label: 'Plus 4 Code', value: 'plus4Code'},
  {label: 'Latitude', value: 'latitude'},
  {label: 'Longitude', value: 'longitude'},
];

export const VehicleForm = [
  {label: 'Name', value: 'name'},
  {label: 'License Plate', value: 'licensePlate'},
  {label: 'VIN', value: 'vin'},
  {label: 'CVN', value: 'cvn'},
  {label: 'Capacity', value: 'capacity'},
  {label: 'Length', value: 'length'},
  {label: 'Site', value: 'site'},
  {label: 'Street Address', value: 'addressLine1'},
  {label: 'Apt / Unit / Building', value: 'addressLine2'},
  {label: 'Country', value: 'country'},
  {label: 'State', value: 'state'},
  {label: 'City', value: 'city'},
  {label: 'Zip Code', value: 'zipCode'},
  {label: 'Plus 4 Code', value: 'plus4Code'},
  {label: 'Latitude', value: 'latitude'},
  {label: 'Longitude', value: 'longitude'},
];

export const OrderForm = [
  {label: 'Vehicle', value: 'vehicleId'},
  {label: 'Driver', value: 'driverId'},
  {label: 'Start Date', value: 'fulfillmentStartDateTime'},
  {label: 'End Date', value: 'fulfillmentEndDateTime'},
];

export const FeedbackForm = [
  {label: 'Email', value: 'email'},
  {label: 'Name', value: 'name'},
  {label: 'Subject', value: 'subject'},
  {label: 'Comments', value: 'comments'},
  {label: 'Type', value: 'type'},
  {label: 'Hospice Location', value: 'hospiceLocation'},
];

export const CreditHoldForm = [
  {label: 'Hold Credit', value: 'isCreditOnHold'},
  {label: 'Note', value: 'creditHoldNote'},
];

export const createMoveForm = [
  {label: 'Ordering Nurse', value: 'hospiceMemberId'},
  {label: 'Pickup Date', value: 'requestedDate'},
  {label: 'Delivery Address', value: 'deliveryAddress'},
  {label: 'Pickup Address', value: 'pickupAddress'},
];

export const editMoveForm = [...createMoveForm, {label: 'Move Notes', value: 'newOrderNotes'}];

export const createPickupForm = [
  {label: 'Ordering Nurse', value: 'hospiceMemberId'},
  {label: 'Order Date', value: 'requestedDate'},
  {label: 'Pickup Address', value: 'pickupAddress'},
  {label: 'Pickup Reason', value: 'pickupReason'},
];

export const editPickupForm = [...createPickupForm, {label: 'Order Notes', value: 'newOrderNotes'}];

export const DeliveryDetailsForm = [
  {label: 'Delivery Address', value: 'deliveryAddress'},
  {label: 'Delivery Timing', value: 'deliveryHours'},
  {label: 'Delivery Timing', value: 'deliveryTime'},
  {label: 'Ordering Nurse', value: 'hospiceMemberId'},
  {label: 'Order Note', value: 'orderNote'},
];

export const createExchangeForm = [
  {label: 'Ordering Nurse', value: 'hospiceMemberId'},
  {label: 'Order Date', value: 'requestedDate'},
  {label: 'Pickup Address', value: 'pickupAddress'},
  {label: 'Pickup Reason', value: 'pickupReason'},
];

export const editExchangeForm = [
  ...createExchangeForm,
  {label: 'Order Notes', value: 'newOrderNotes'},
];
