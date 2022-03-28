import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {environment} from 'src/environments/environment';
import {setQueryParams, saveAllFeatureFlags, updateFeatureFlag} from '../utils';
import {CoreApiService} from './http/core-api.service';
import {tap} from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class SystemService {
  constructor(private http: CoreApiService, private httpClient: HttpClient) {}

  private ISSUES = {
    MISSING_IDENTITY: 'missing-identity',
    NOT_VERIFIED: 'not-verified',
    MISSING_EMAIL: 'missing-email',
    MISSING_NET_SUITE_CONTACT: 'missing-net-suite-contact',
    PENDING_NETSUITE_CONFIRMATION: 'pending-confirmation-at-netsuite',
    WITHOUT_SITES: 'without-site',
    INVALID_STATUS: 'invalid-status',
    INACTIVE_PATIENT_WITH_CONSUMABLE_ONLY: 'consumable-inventory-only',
    PATIENT_WITH_INVALID_STATUS: 'patient-with-invalid-status-only',
    PATIENT_WITH_INVALID_STATUS_ALL: 'patient-with-invalid-status-all',
    NON_VERIFIED_HOME_ADDRESS: 'non-verified-home-address',
    NON_VERIFIED_ADDRESS: 'non-verified-address',
    MISSING_FHIR_ORGANIZATION: 'missing-fhir-organization',
    PATIENT_MISSING_FHIR: 'missing-fhir-patient',
  };

  private ACTIONS = {
    FIX: 'fix',
  };

  getMissingIdentityCount(queryParam?: any) {
    const query = setQueryParams({
      ...queryParam,
      issue: this.ISSUES.MISSING_IDENTITY,
    });
    return this.http.get(`system/users`, query);
  }

  fixMissingIdentityUsers() {
    const body = {
      issue: this.ISSUES.MISSING_IDENTITY,
      action: this.ACTIONS.FIX,
    };
    return this.http.post(`system/users`, body);
  }

  getUsersWithoutEmail(queryParam?: any) {
    const query = setQueryParams({
      ...queryParam,
      issue: this.ISSUES.MISSING_EMAIL,
    });
    return this.http.get(`system/users`, query);
  }

  getNonVerifiedAddress(isHomeAddress, queryParam?: any) {
    const query = setQueryParams({
      ...queryParam,
      issue: isHomeAddress
        ? this.ISSUES.NON_VERIFIED_HOME_ADDRESS
        : this.ISSUES.NON_VERIFIED_ADDRESS,
    });
    return this.http.get(`system/address`, query);
  }

  getPatientsWithoutFhirRecord(queryParam?: any) {
    const query = setQueryParams({
      ...queryParam,
      issue: this.ISSUES.PATIENT_MISSING_FHIR,
    });
    return this.http.get(`system/patients`, query);
  }

  fixNonVerifiedAddress(addressId, isHomeAddress) {
    const issue = isHomeAddress
      ? this.ISSUES.NON_VERIFIED_HOME_ADDRESS
      : this.ISSUES.NON_VERIFIED_ADDRESS;
    const query = `?issue=${issue}`;
    return this.http.put(`system/address/${addressId}${query}`, null);
  }

  fixNonVerifiedAddresses(isHomeAddress) {
    const body = {
      issue: this.ISSUES.NOT_VERIFIED,
      action: this.ACTIONS.FIX,
    };
    const issue = isHomeAddress
      ? this.ISSUES.NON_VERIFIED_HOME_ADDRESS
      : this.ISSUES.NON_VERIFIED_ADDRESS;
    const query = `?issue=${issue}`;
    return this.http.post(`system/address${query}`, body);
  }

  fixWithoutFhirRecord() {
    const body = {
      issue: this.ISSUES.PATIENT_MISSING_FHIR,
      action: this.ACTIONS.FIX,
    };
    return this.http.post('system/patients/fhir-patient', body);
  }

  getCountWithMissingNetSuiteContact(queryParam?: any) {
    const query = setQueryParams({
      ...queryParam,
      issue: this.ISSUES.MISSING_NET_SUITE_CONTACT,
    });
    return this.http.get(`system/members`, query);
  }

  fixMembersWithMissingNetSuiteContact() {
    const body = {
      issue: this.ISSUES.MISSING_NET_SUITE_CONTACT,
      action: this.ACTIONS.FIX,
    };
    return this.http.post(`system/members`, body);
  }

  getInternalUsersCountWithMissingNetSuiteContact(queryParam?: any) {
    const query = setQueryParams({
      ...queryParam,
      issue: this.ISSUES.MISSING_NET_SUITE_CONTACT,
    });
    return this.http.get(`system/internal-users`, query);
  }

  fixInternalUsersWithMissingNetSuiteContact() {
    const body = {
      issue: this.ISSUES.MISSING_NET_SUITE_CONTACT,
      action: this.ACTIONS.FIX,
    };
    return this.http.post(`system/internal-users`, body);
  }

  getUnconfirmedOrders(queryParam?: any) {
    const query = setQueryParams({
      ...queryParam,
      issue: this.ISSUES.PENDING_NETSUITE_CONFIRMATION,
    });
    return this.http.get(`system/orders`, query);
  }

  fixUnconfirmedOrder(orderId) {
    return this.http.put(`system/orders/unconfirmed-order/${orderId}`, null);
  }

  fixUnconfirmedOrders(fixValue) {
    const body = {
      issue: this.ISSUES.PENDING_NETSUITE_CONFIRMATION,
      action: this.ACTIONS.FIX,
    };
    const query = `?dispatchOnly=${fixValue.dispatchOnly}&batchSize=${fixValue.batchSize}&stopOnFirstError=${fixValue.stopOnFirstError}`;
    return this.http.post(`system/orders${query}`, body);
  }

  getApiLogs(queryParam?: any) {
    return this.http.post(`system/core-api-logs`, queryParam);
  }

  getOrdersWithoutAssignedSites(queryParam?: any) {
    const query = setQueryParams({
      ...queryParam,
      issue: this.ISSUES.WITHOUT_SITES,
    });
    return this.http.get(`system/orders`, query);
  }

  fixOrdersWithoutAssignedSites() {
    const body = {issue: this.ISSUES.WITHOUT_SITES, action: this.ACTIONS.FIX};
    return this.http.post(`system/orders`, body);
  }

  getOrdersWithoutStatus(queryParam?: any) {
    const query = setQueryParams({
      ...queryParam,
      issue: this.ISSUES.INVALID_STATUS,
    });
    return this.http.get(`system/orders`, query);
  }

  fixOrderWithoutStatus(orderId, previewChanges = false) {
    const query = setQueryParams({previewChanges});
    return this.http.put(`system/orders/${orderId}`, null, query);
  }

  getHealthCheck() {
    return this.httpClient.get(environment.apiServerURL + '/health');
  }

  getNetsuiteLogs(queryParam?: any) {
    return this.http.post(`system/netsuite-hms-logs`, queryParam);
  }

  getNetsuiteLogDetail(id) {
    return this.http.get(`system/netsuite-hms-logs/${id}`);
  }

  getPatientInventoryWithIssues(queryParam: any) {
    const query = setQueryParams({
      ...queryParam,
    });
    return this.http.get(`system/patient-inventory`, query);
  }

  fixPatientInventoryWithIssues(data: any, issueType) {
    return this.http.put(`system/patient-inventory?issue=${issueType}`, data);
  }

  getDeletedPatientInventory(queryParam?) {
    const query = setQueryParams({
      ...queryParam,
    });
    return this.http.get(`system/patient-inventory/deleted`, query);
  }

  getInactivePatientsWithConsumablesOnly(queryParam?) {
    const query = setQueryParams({
      ...queryParam,
      issue: this.ISSUES.INACTIVE_PATIENT_WITH_CONSUMABLE_ONLY,
    });
    return this.http.get(`system/patients`, query);
  }

  fixInactivePatientsWithConsumablesOnly(patientUuid, previewChanges = false) {
    const body = {
      issue: this.ISSUES.INACTIVE_PATIENT_WITH_CONSUMABLE_ONLY,
      action: this.ACTIONS.FIX,
    };
    return this.http.post(`system/patients/${patientUuid}?previewChanges=${previewChanges}`, body);
  }

  getPatientsWithInvalidStatus(queryParam?) {
    const query = setQueryParams({
      ...queryParam,
      issue: this.ISSUES.PATIENT_WITH_INVALID_STATUS,
    });
    return this.http.get(`system/patients`, query);
  }

  fixPatientWithInvalidStatus(patientUuid) {
    const body = {
      issue: this.ISSUES.PATIENT_WITH_INVALID_STATUS,
      action: this.ACTIONS.FIX,
    };
    return this.http.post(`system/patients/${patientUuid}`, body);
  }

  fixAllPatientWithInvalidStatus() {
    const body = {
      issue: this.ISSUES.PATIENT_WITH_INVALID_STATUS_ALL,
      action: this.ACTIONS.FIX,
    };
    return this.http.post(`system/patients/00000000-0000-0000-0000-000000000000`, body);
  }

  getDispatchOrders(queryParam?: any) {
    return this.http.post(`system/netsuite-hms-dispatch`, queryParam);
  }

  getAllSystemFeatures() {
    const features = this.http.get('features').pipe(
      tap((res: any) => {
        saveAllFeatureFlags(res.records);
      })
    );
    return features;
  }

  updateSystemFeature(body: any) {
    const feature = this.http.put('features', body).pipe(
      tap(res => {
        updateFeatureFlag(res);
      })
    );
    return feature;
  }

  getAllHospicesWithoutFHIR(queryParam?) {
    const query = setQueryParams({
      ...queryParam,
      issue: this.ISSUES.MISSING_FHIR_ORGANIZATION,
    });
    return this.http.get(`system/hospices`, query);
  }

  fixHospicesWithoutFHIR() {
    const body = {
      issue: this.ISSUES.MISSING_FHIR_ORGANIZATION,
      action: this.ACTIONS.FIX,
    };
    return this.http.post(`system/hospices`, body);
  }
}
