export class SystemTools {
  userTools = [
    {
      title: 'Without Cognito Account',
      action: 'getIdentityMissingUsers',
      headerBtnAction: 'fixIdentityMissingUsers',
      tableHeaderLabel: 'Users Without Cognito Account',
    },
    {
      title: 'Without Email',
      action: 'getUsersCount',
      tableHeaderLabel: 'Users Without Email',
    },
    {
      title: 'Member without Netsuite Contact',
      action: 'getMembersWithoutNetsuiteContactId',
      headerBtnAction: 'fixMembersWithoutNetsuiteId',
      tableHeaderLabel: 'Member without Netsuite Contact',
    },
    {
      title: 'Internal Users without Netsuite Contact',
      action: 'getInternalUsersWithoutNetsuiteContactId',
      headerBtnAction: 'fixInternalUsersWithoutNetsuiteId',
      tableHeaderLabel: 'Internal Users without Netsuite Contact',
    },
  ];
  orderTools = [
    {
      title: 'Without Assigned Sites',
      action: 'getOrdersWithoutSites',
      tableHeaderLabel: 'Orders Without Assigned Sites',
      headerBtnAction: 'fixOrdersWithoutAssignedSites',
    },
    {
      title: 'With Incorrect Status',
      action: 'getOrdersWithoutStatuses',
      tableHeaderLabel: 'Orders With Incorrect Status',
      actionBtn: 'previewOrderStatus',
    },
    {
      title: 'Unconfirmed In Netsuite',
      action: 'getUnConfirmedOrderFulfillments',
      tableHeaderLabel: 'Unconfirmed Orders In Netsuite',
      headerBtnAction: 'fixUnConfirmedOrderFulfillments',
      actionBtn: 'fixUnConfirmedOrderFulfillment',
    },
  ];
  patientTools = [
    {
      title: 'With Invalid Status',
      headerBtnAction: 'fixAllPatientWithInvalidStatus',
      action: 'getPatientWithInvalidStatus',
      tableHeaderLabel: 'Patients With Invalid Status',
      actionBtn: 'fixPatientWithInvalidStatus',
    },
    {
      title: 'With Only consumable Inventory',
      action: 'getInactivePatientsWithOnlyConsumables',
      tableHeaderLabel: 'Patients With Only consumable Inventory',
      actionBtn: 'previewInactivePatientWithConsumable',
    },
    {
      title: 'With Non-verified Addresses',
      action: 'getNonVerifiedAddresses',
      headerBtnAction: 'fixNonVerifiedAddresses',
      tableHeaderLabel: 'Patients With Non-verified Addresses',
      actionBtn: 'fixNonVerifiedAddress',
    },
    {
      title: 'With Non-verified Home Addresses',
      action: 'getNonVerifiedHomeAddresses',
      headerBtnAction: 'fixNonVerifiedHomeAddresses',
      tableHeaderLabel: 'Patients With Non-verified Home Addresses',
      actionBtn: 'fixNonVerifiedHomeAddress',
    },
    {
      title: 'Deleted Patient Inventory',
      action: 'fetchDeletedPatientInventory',
      tableHeaderLabel: 'Deleted Patient Inventory',
      checkbox: 'Is AssetTagged',
    },
    {
      title: 'Without FHIR record',
      action: 'getPatientsWithoutFhirRecord',
      headerBtnAction: 'fixWithoutFhirRecord',
      tableHeaderLabel: 'Patient without FHIR record',
    },
  ];
  hospiceTools = [
    {
      title: 'Without FHIR Organization Created',
      action: 'getHospiceWithoutFHIR',
      tableHeaderLabel: 'Hospices Without FHIR Organization Created',
      headerBtnAction: 'fixHospiceWithoutFHIROrganization',
    },
  ];
}

export class UnconfirmedInNetsuiteFixConfig {
  dispatchOnly = true;
  batchSize = 10;
  stopOnFirstError = true;
}
