import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {SieveOperators, EnumNames} from 'src/app/enums';
import {
  buildFilterString,
  calculateWarehouseTime,
  getSum,
  convertObjectToArray,
  getEnum,
  IsPermissionAssigned,
  scrollTo,
} from 'src/app/utils';
import {DispatchService, PatientService} from 'src/app/services';
import {initMap, htmlMarker, getLineLayer, getSymbolLayer, mapData} from 'src/app/utils/map.utils';
import * as atlasRest from 'azure-maps-rest';
import {from} from 'rxjs';
import {source, getSubscriptionKey, data as mapdata} from 'azure-maps-control';
import {CalendarEvent} from 'calendar-utils';
import {mergeMap} from 'rxjs/operators';
import {OrderProcessingTime, PaginationResponse} from 'src/app/models';

@Component({
  selector: 'app-dispatch-order-detail',
  templateUrl: './dispatch-order-detail.component.html',
  styleUrls: ['./dispatch-order-detail.component.scss'],
})
export class DispatchOrderDetailComponent implements OnInit {
  orderId: number;
  map: any;
  site: any;
  order: any;
  dispatchInstructions = [];
  viewDate: Date;
  events: CalendarEvent[] = [];
  truck: any;
  processingTime = new OrderProcessingTime();
  breakAfterDelivery = 0;
  completedOrderLineStatusId = getEnum(EnumNames.orderLineItemStatusTypes).find(
    x => x.name === 'Completed'
  )?.id;
  fulfilledItems = 0;
  unfulfilledItems = 0;
  orderTypes = getEnum(EnumNames.OrderTypes);
  patientMoveId = null;
  pickupId = null;
  constructor(
    private route: ActivatedRoute,
    private dispatchService: DispatchService,
    private patientService: PatientService
  ) {
    const {paramMap} = this.route.snapshot;
    this.orderId = Number(paramMap.get('orderId'));
    if (this.orderId) {
      this.getDispatchInstruction();
    }
    const site = sessionStorage.getItem('currentSite');
    this.site = site ? JSON.parse(site) : null;
    this.patientMoveId = this.orderTypes.find(x => x.name === 'Patient_Move');
    this.pickupId = this.orderTypes.find(x => x.name === 'Pickup');
  }

  ngOnInit(): void {
    this.initMap();
  }

  initMap() {
    setTimeout(() => {
      this.map = initMap(
        document.getElementById('orderMap'),
        [this.site?.address?.longitude ?? 0, this.site?.address?.latitude ?? 0],
        7
      );
      // Wait until the map resources are ready to apply markers.
      this.map.events.add('ready', () => {
        const primaryMarker = htmlMarker(this.site?.address, '#d96329', true, this.site?.name);
        this.map.markers.add(primaryMarker);
      });
      const collection = document.getElementsByClassName('cal-hour');
      if (collection.length === 24) {
        scrollTo(collection[14]);
      }
    });
  }

  getDispatchInstruction() {
    const filters = [
      {
        field: 'orderId',
        operator: SieveOperators.Equals,
        value: [this.orderId],
      },
    ];
    const dispatchInsReq = {
      filters: buildFilterString(filters),
    };
    this.dispatchService
      .getAllDispatchInstructions(dispatchInsReq)
      .pipe(
        mergeMap((response: any) => {
          this.dispatchInstructions = response.records;
          this.formatOrders(this.dispatchInstructions);
          this.order = this.dispatchInstructions[0]?.orderHeader;
          this.order.orderLineItems = this.order.orderLineItems.map(x => {
            x.displayName = x.item.name;
            return x;
          });

          const patientFilters = [
            {
              field: 'uniqueId',
              operator: SieveOperators.Equals,
              value: [this.order.patientUuid],
            },
          ];
          const patientRequest = {
            filters: buildFilterString(patientFilters),
          };
          return this.patientService.getPatients(patientRequest);
        })
      )
      .subscribe(
        (patient: PaginationResponse) => {
          this.order.patient = patient.records[0] || null;
          const address =
            this.order.orderTypeId === this.patientMoveId ||
            this.order.orderTypeId === this.pickupId
              ? this.order.pickupAddress
              : this.order.deliveryAddress;
          const title = `${this.order.id} (${this.order.statOrder ? 'STAT-' : ''}${
            this.order.orderType
          })`;
          const secondaryMarker = htmlMarker(address, '#0570af', false, title);
          this.map.events.add('ready', () => {
            this.map.markers.add(secondaryMarker);
            const startPoint = mapData(
              this.site?.address?.longitude ?? 0,
              this.site?.address?.latitude ?? 0
            );
            const endPoint = mapData(address?.longitude ?? 0, address?.latitude ?? 0);
            this.calculateRouteDirections(startPoint, endPoint);
          });
        },
        (error: any) => {
          throw error;
        }
      );
  }

  calculateProcessingTime(orderLineItems, orderType) {
    const processingTime = orderLineItems.reduce((a, b) => {
      b.processingTimeSum =
        b.itemCount * (b.item.avgDeliveryProcessingTime + b.item.avgPickUpProcessingTime);
      return a + b.processingTimeSum;
    }, 0);
    return processingTime;
  }

  formatOrders(instructions) {
    const orderEntries = instructions[0];
    if (orderEntries) {
      const start = orderEntries.dispatchStartDateTime;
      const end = orderEntries.dispatchEndDateTime;
      const order = orderEntries.orderHeader;
      order.orderLineItems.map(x => (x.orderType = order.orderType));
      order.processingTime = this.calculateProcessingTime(order.orderLineItems, order.orderType);
      this.truck = orderEntries.vehicle;
      this.processingTime.offloading = order.processingTime;
      order.event = {
        id: 1,
        title: `${order.id} | (${order.statOrder ? 'STAT-' : ''}${order.orderType}) | ${
          order.processingTime
        } mins`,
        meta: {
          processingTime: order.processingTime,
          orderId: order.id,
          order,
          truck: {id: this.truck.id, name: this.truck.name},
        },
        draggable: false,
        start: start ? new Date(start) : null,
        end: end ? new Date(end) : null,
      };
      this.fulfilledItems = order.orderLineItems.filter(
        x => x.statusId === this.completedOrderLineStatusId
      ).length;
      this.unfulfilledItems = order.orderLineItems.length - this.fulfilledItems;
      this.events = [...this.events, order.event];
      this.viewDate = start ? new Date(start) : new Date();
      this.addWarehouseEvents();
    }
  }

  addWarehouseEvents() {
    const warehousePickup = calculateWarehouseTime(
      this.events,
      'pickup',
      this.events.length + 1,
      this.truck
    );
    const warehouseDrop = calculateWarehouseTime(
      this.events,
      'drop',
      this.events.length + 2,
      this.truck
    );
    this.processingTime.warehouseLoading = warehousePickup.meta.processingTime;
    this.events = [...this.events, warehousePickup, warehouseDrop];
    this.calculateTotalProcessingTime();
  }
  calculateTotalProcessingTime() {
    this.processingTime.total = getSum(convertObjectToArray(this.processingTime));
  }

  calculateRouteDirections(startPoint, endPoint) {
    const pipeline = atlasRest.MapsURL.newPipeline(
      new atlasRest.SubscriptionKeyCredential(getSubscriptionKey())
    );
    const routeURL = new atlasRest.RouteURL(pipeline);
    const coordinates = [
      [startPoint.geometry.coordinates[0], startPoint.geometry.coordinates[1]],
      [endPoint.geometry.coordinates[0], endPoint.geometry.coordinates[1]],
    ];
    const datasource = new source.DataSource();
    this.map.events.add('ready', () => {
      this.map.sources.add(datasource);
      from(
        routeURL.calculateRouteDirections(atlasRest.Aborter.timeout(10000), coordinates, {
          TravelMode: 'truck',
        })
      ).subscribe(directions => {
        const data = directions.geojson.getFeatures();
        const routeLine = data.features[0];
        const symbolLayerConf = {
          lineSpacing: 20,
          placement: 'center',
          image: 'triangle-arrow-left',
          size: 0.6,
        };
        datasource.add(new mapdata.LineString(routeLine.geometry.coordinates[0]));
        this.map.layers.add(
          getLineLayer(datasource, '#3f923f', 6),
          getSymbolLayer(datasource, symbolLayerConf)
        );
      });
    });
  }

  checkPermission(module, action) {
    return IsPermissionAssigned(module, action);
  }

  saveFulfillmentFrom(route) {
    sessionStorage.setItem('orderFulfillmentFrom', route);
  }
}
