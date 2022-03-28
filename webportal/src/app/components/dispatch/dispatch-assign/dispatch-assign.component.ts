import {
  Component,
  OnInit,
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  HostBinding,
  ViewChild,
} from '@angular/core';
import {initMap, htmlMarker, getLineLayer, getSymbolLayer} from 'src/app/utils/map.utils';
import {data as mapdata, getSubscriptionKey, source} from 'azure-maps-control';
import * as atlasRest from 'azure-maps-rest';

import {
  CalendarEvent,
  CalendarEventTimesChangedEvent,
  CalendarView,
  CalendarEventAction,
} from 'angular-calendar';
import {Subject, from, of} from 'rxjs';
import {
  buildFilterString,
  convertMinToTime,
  changeToISO,
  sortBy,
  getEnum,
  getUniqArray,
  getFormattedPhoneNumber,
  IsPermissionAssigned,
  getUTCDateAsLocalDate,
  calculateWarehouseTime,
  isFeatureEnabled,
} from 'src/app/utils';
import {
  RouteOptimizationService,
  ToastService,
  DispatchService,
  PatientService,
  OrderHeadersService,
  SitesService,
} from 'src/app/services';
import {SieveOperators, EnumNames} from 'src/app/enums';
import {finalize, mergeMap} from 'rxjs/operators';
import {PaginationResponse} from 'src/app/models';
import {DispatchSchedulerComponent} from './dispatch-scheduler/dispatch-scheduler.component';
import {ModalComponent} from 'src/app/common';
import {OrderTypes} from 'src/app/enum-constants';
import {Location} from '@angular/common';

@Component({
  selector: 'app-dispatch-assign',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './dispatch-assign.component.html',
  styleUrls: ['./dispatch-assign.component.scss'],
})
export class DispatchAssignComponent implements OnInit {
  @ViewChild(DispatchSchedulerComponent)
  dispatchScheduler: DispatchSchedulerComponent;
  @ViewChild('selectOrdermodal')
  selectOrdermodal: ModalComponent;
  @ViewChild('changeOrderDateModal')
  changeOrderDateModal: ModalComponent;
  @ViewChild('checkRescheduleOrderModal')
  checkRescheduleOrderModal: ModalComponent;
  @ViewChild('optimizationStatusModal')
  optimizationStatusModal: ModalComponent;

  orders = [];
  currentOrder: any;
  backupOrders = [];
  map: any;
  site: any;
  view: CalendarView = CalendarView.Day;
  CalendarView = CalendarView;
  today: Date = new Date();
  viewDate: Date = new Date();
  refresh: Subject<any> = new Subject();
  events: CalendarEvent[] = [];
  trucks: any;
  trucksAvailableTimes = [];
  loading = false;
  loadingScheduler = false;
  currentDroppedIndex = null;
  optimizedRoutes: any;
  routeOptimizerResponse: any;
  unAssignedRoutes: any;
  optimizedRoutesVisible = false;
  assignOrderVisible = false;
  optimizedForTruckId: number;
  assignToTruckId: number;
  orderTypes = getEnum(EnumNames.OrderTypes);
  patientMoveId = null;
  pickupId = null;
  loadingRouteOptimzer = false;
  eventsForTruck = [];
  loadingOrderAssignment = false;
  detailsViewOpen = false;
  dispatchInsRes = [];
  selectOrdersVisible = false;
  orderDateChangeVisible = false;
  actions: CalendarEventAction[] = [
    {
      label: '<i class="pi pi-trash"></i>',
      a11yLabel: 'Delete',
      onClick: ({event}: {event: CalendarEvent}): void => {
        this.events = this.events.filter(evt => evt.id !== event.id);
        event.meta.order.event = event;
        delete event.meta.order.event.meta.truck;
        this.orders = [...this.orders, event.meta.order];
      },
    },
  ];
  fulfilledItems = null;
  formatPhoneNumber = getFormattedPhoneNumber;
  @HostBinding('class.modalClass') containerClass = true;

  rescheduleOrderPassed = false;
  selectedTrucks = [];
  selectedOrders = [];
  selectAllTrucks = false;
  selectAllOrders = false;
  optimizeLoading = false;
  reschedulableOrderVisbile = false;
  optimizationStatusModalVisible = false;
  driverOptimizationFeatureFlag = isFeatureEnabled('DriverOptimizationFeature');

  constructor(
    private routeOptimizationService: RouteOptimizationService,
    private cd: ChangeDetectorRef,
    private toasterService: ToastService,
    private dispatchService: DispatchService,
    private patientService: PatientService,
    private orderHeaderService: OrderHeadersService,
    private sitesService: SitesService,
    private location: Location
  ) {
    const data = history.state?.data?.orders ?? [];
    if (data?.length) {
      this.orders = data;
      this.backupOrders = JSON.parse(JSON.stringify(data));
    }
    const site = sessionStorage.getItem('currentSite');
    this.site = site ? JSON.parse(site) : null;
    if (this.site) {
      this.getVehicles(this.site.netSuiteLocationId);
    }
    this.patientMoveId = this.orderTypes.find(x => x.name === 'Patient_Move')?.id;
    this.pickupId = this.orderTypes.find(x => x.name === 'Pickup')?.id;
  }

  getDispatchInstructions(vehicleId) {
    const viewDate = new Date(this.viewDate);
    const filters = [
      {field: 'vehicleId', operator: SieveOperators.Equals, value: vehicleId},
      {
        field: 'dispatchStartDateTime',
        operator: SieveOperators.GreaterThanEqualTo,
        value: [new Date(viewDate.setHours(0, 0, 0, 0)).toISOString()],
      },
      {
        field: 'dispatchEndDateTime',
        operator: SieveOperators.LessThanEqualTo,
        value: [new Date(viewDate.setHours(24, 0, 0, 0)).toISOString()],
      },
    ];
    const dispatchInsReq = {
      filters: buildFilterString(filters),
    };
    this.dispatchInsRes = [];
    this.dispatchService
      .getAllDispatchInstructions(dispatchInsReq)
      .pipe(
        mergeMap((response: any) => {
          this.dispatchInsRes = response?.records || [];
          const orderNumberIds = getUniqArray(
            response.records.flatMap((order: any) => {
              return order.orderHeader.orderNumber;
            })
          );

          if (!orderNumberIds || !orderNumberIds.length) {
            return of(null);
          }
          const filter = [
            {
              field: 'orderNumber',
              operator: SieveOperators.Equals,
              value: orderNumberIds,
            },
          ];
          const orderHeadersFilter = {
            filters: buildFilterString(filter),
          };
          return this.orderHeaderService.getAllOrderHeaders(orderHeadersFilter);
        })
      )
      .subscribe(
        (orderRes: any) => {
          if (orderRes) {
            this.dispatchInsRes.forEach((x: any) => {
              const order = orderRes?.records.find(y => y.id === x.orderHeaderId);
              if (order) {
                x.orderHeader.pickupAddress = order.pickupAddress;
                x.orderHeader.deliveryAddress = order.deliveryAddress;
                x.orderHeader.orderNotes = order.orderNotes;
              }
              x.orderHeader?.orderLineItems.forEach(oli => {
                oli.action = OrderTypes.findOrderTypeLabel(oli.actionId);
              });
            });
          }
          this.formatOrders(this.dispatchInsRes);

          const vehicleIds = getUniqArray(
            this.dispatchInsRes.flatMap((order: any) => {
              return order.vehicleId;
            })
          );

          this.trucks = this.trucks?.map((truck: any) => {
            return {
              ...truck,
              hasAssignedOrders: vehicleIds.includes(truck.id),
            };
          });
        },
        (error: any) => {
          throw error;
        }
      );
  }
  getVehicles(locationId) {
    this.loading = true;
    this.loadingScheduler = true;

    this.sitesService
      .getVehiclesByLocationId(locationId)
      .pipe(
        finalize(() => {
          this.loading = false;
        })
      )
      .subscribe(
        (response: any) => {
          this.trucks = this.getOnlyVehiclesFromLocation(response);

          const vehicleIds = this.trucks.flatMap((vehicle: any) => {
            return [vehicle.id];
          });

          this.getDispatchInstructions(vehicleIds);
          this.cd.markForCheck();
        },
        (error: any) => {
          throw error;
        }
      );
  }

  getOnlyVehiclesFromLocation(locationArray: any[]): any[] {
    const auxiliarArray = [
      ...locationArray
        .filter(x => x.vehicles.length > 0 || x.locationType === 'Vehicle')
        .flatMap(location => {
          if (location.locationType === 'Vehicle') {
            return location;
          } else {
            return this.getOnlyVehiclesFromLocation(location.vehicles);
          }
        }),
    ];

    return auxiliarArray;
  }

  calculateProcessingTime(orderLineItems, orderType) {
    const processingTime = orderLineItems.reduce((a, b) => {
      b.processingTimeSum =
        b.itemCount * (b.item.avgDeliveryProcessingTime + b.item.avgPickUpProcessingTime);
      return a + b.processingTimeSum;
    }, 0);
    return processingTime;
  }

  formatOrders(instructions?) {
    this.events = [];
    let index = 1;
    instructions.forEach((orderEntries: any) => {
      const start = orderEntries.dispatchStartDateTime;
      const end = orderEntries.dispatchEndDateTime;
      const vehicle = orderEntries.vehicle;
      const order = orderEntries.orderHeader;
      order.orderLineItems.map(x => (x.orderType = order.orderType));
      order.processingTime = this.calculateProcessingTime(order.orderLineItems, order.orderType);
      order.event = {
        id: index++,
        title: this.getOrderTitle(order),
        meta: {
          processingTime: order.processingTime,
          orderId: order.id,
          order,
          truck: {id: vehicle.id, name: vehicle.name},
        },
        draggable: true,
        start: start ? new Date(start) : null,
        end: end ? new Date(end) : null,
        actions: this.actions,
      };
      this.events = [...this.events, order.event];
      const orderIdx = this.orders.findIndex(x => x.id === order.id);
      if (orderIdx > -1) {
        this.orders.splice(orderIdx, 1);
      }
    });
    this.orders = [
      ...this.orders.filter(order => {
        order.orderLineItems.map(x => (x.orderType = order.orderType));
        order.event = {
          id: index++,
          title: this.getOrderTitle(order),
          meta: {
            processingTime: order.processingTime,
            orderId: order.id,
            order,
            address:
              order.orderTypeId === this.patientMoveId || order.orderTypeId === this.pickupId
                ? order.pickupAddress
                : order.deliveryAddress,
          },
          draggable: true,
          actions: this.actions,
        };
        if (order.dispatchStatus !== 'Scheduled') {
          return order;
        }
      }),
    ];

    if (this.events.length > 0) {
      const patientUuids = this.events.flatMap(o =>
        !o.meta.order.patient ? [o.meta.order.patientUuid] : []
      );
      if (patientUuids.length > 0) {
        const filters = [
          {
            field: 'uniqueId',
            operator: SieveOperators.Equals,
            value: patientUuids,
          },
        ];
        const patientRequest = {
          filters: buildFilterString(filters),
        };
        this.patientService
          .getPatients(patientRequest)
          .pipe(
            finalize(() => {
              this.loadingScheduler = false;
            })
          )
          .subscribe((response: PaginationResponse) => {
            this.events.map(x => {
              x.meta.order.patient = response.records.find(
                p => p.uniqueId === x.meta.order.patientUuid
              );
              if (x.meta.order.patient && x.meta.order.patient.uniqueId) {
                x.title = this.getOrderTitle(x.meta.order);
              }
              return x;
            });
            this.cd.markForCheck();
            this.dispatchScheduler?.truckColumnsBackupOrdersAssignedList(true);
          });
      } else {
        this.loadingScheduler = false;
      }
    } else {
      this.loadingScheduler = false;
    }
    this.cd.markForCheck();
  }
  getOrderTitle(order: any) {
    const {
      orderNumber,
      patient,
      statOrder,
      orderType,
      processingTime,
      deliveryAddress,
      pickupAddress,
    } = order;
    const address =
      order?.orderTypeId === this.patientMoveId || order?.orderTypeId === this.pickupId
        ? pickupAddress
        : deliveryAddress;
    return `#${orderNumber}| ${patient?.firstName ? patient.firstName : ''} ${
      patient?.lastName ? patient.lastName : ''
    } | (${statOrder ? 'STAT-' : ''}${orderType}) | ${processingTime} mins ${this.getAddress(
      address
    )}`;
  }
  getAddress(address) {
    if (!address || (!address.addressLine1 && !address.state)) {
      return ``;
    }
    return `| ${address.addressLine1 ? address.addressLine1 + ',' : ','}
     ${address.addressLine2 ? address.addressLine2 + ',' : ''} ${
      address.city ? address.city + ',' : ''
    }
    ${address.state ? address.state : ''}`;
  }
  ngOnInit(): void {
    if (this.orders.length === 0) {
      const orders = sessionStorage.getItem('assignOrderList');
      this.orders = orders ? JSON.parse(orders) : [];
      this.backupOrders = orders ? JSON.parse(orders) : [];
    }
    this.initMap();
  }

  initMap() {
    setTimeout(() => {
      this.map = initMap(
        document.getElementById('orderMap'),
        [this.site?.address?.longitude ?? 0, this.site?.address?.latitude ?? 0],
        9
      );
      // Wait until the map resources are ready to apply markers.
      this.map.events.add('ready', () => {
        this.backupOrders.map(order => {
          const primaryMarker = htmlMarker(this.site?.address, '#d96329', true, this.site?.name);
          this.map.markers.add(primaryMarker);
          const title = this.getOrderTitle(order);
          const secondaryMarker = htmlMarker(
            order.orderTypeId === this.patientMoveId || order.orderTypeId === this.pickupId
              ? order.pickupAddress
              : order.deliveryAddress,
            '#0570af',
            false,
            title
          );
          this.map.markers.add(secondaryMarker);
        });
      });
    });
  }

  eventDropped({event, newStart, newEnd, allDay}: CalendarEventTimesChangedEvent): void {
    const externalIndex = this.events.indexOf(event);
    if (event.meta && !event.meta.truck) {
      const truck = this.trucks[this.currentDroppedIndex];
      event.meta.truck = {
        id: truck.id,
        name: truck.name,
      };
      const orderIndex = this.orders.findIndex(x => x.id === event.meta.orderId);
      if (orderIndex > -1) {
        this.orders.splice(orderIndex, 1);
      }
    }
    if (typeof allDay !== 'undefined') {
      event.allDay = allDay;
    }
    if (externalIndex === -1) {
      this.events.push(event);
    } else {
      this.events[externalIndex] = event;
    }
    event.start = newStart;
    if (newEnd) {
      event.end = newEnd;
    }

    this.events = [...this.events];
    this.orders = [...this.orders];
    this.cd.markForCheck();
    setTimeout(() => {
      this.mapRoutes();
    }, 500);
  }

  mapRoutes() {
    const mappedLayers = this.map.layers.getLayers();
    mappedLayers.forEach(lay => {
      if (lay.options) {
        this.map.layers.remove(lay.id);
      }
    });
    this.trucks.forEach(truck => {
      const events = sortBy(
        this.events.filter(y => y.meta?.truck?.id === truck.id),
        'start'
      );
      if (events.length) {
        const startEvent = events[0];
        const endEvent = events[events.length - 1];

        const startPoint = new mapdata.Feature(
          new mapdata.Point([startEvent.meta.address.longitude, startEvent.meta.address.latitude])
        );
        const endPoint = new mapdata.Feature(
          new mapdata.Point([endEvent.meta.address.longitude, endEvent.meta.address.latitude])
        );
        this.calculateRouteDirections(startPoint, endPoint);
      }
    });
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
  }

  truckChanged({event, newTruck}) {
    event.color = newTruck.color;
    event.meta.truck = newTruck;
    this.events = [...this.events];
  }

  getOptimizedRoutes(truckId) {
    this.eventsForTruck = this.events.filter((event: CalendarEvent) => {
      return event.meta.truck.id === truckId;
    });

    this.eventsForTruck.sort((a: any, b: any) => {
      if (a.start < b.start) {
        return -1;
      }
      if (a.start > b.start) {
        return 1;
      }
      return 0;
    });

    if (!this.eventsForTruck.length) {
      this.toasterService.showError('Please assign some orders to the truck first');
      return;
    }

    const orderIds = this.events.flatMap((event: CalendarEvent) => {
      return event.meta.truck.id === truckId ? [event.meta.orderId] : [];
    });
    let items = this.backupOrders.filter((order: any) => {
      return orderIds.includes(order.id);
    });
    items = items.map((item: any) => {
      const address =
        item.orderTypeId === this.patientMoveId || item.orderTypeId === this.pickupId
          ? item.pickupAddress
          : item.deliveryAddress;

      const formatStartEndResponse = this.formatStartAndEndTime(item);
      const requestedStartDateTime = formatStartEndResponse.startTime;
      const requestedEndDateTime = formatStartEndResponse.endTime;

      return {
        openingTime: requestedStartDateTime,
        closingTime: requestedEndDateTime,
        dwellTime: convertMinToTime(item.processingTime),
        priority: 1,
        name: item.id,
        title: item.title,
        location: {
          latitude: address.latitude,
          longitude: address.longitude,
        },
      };
    });
    const currentTruck = this.trucks.find((truck: any) => truck.id === truckId);
    const startDate = new Date(this.viewDate.setHours(0, 0, 0));
    const endDate = new Date(this.viewDate);
    endDate.setDate(endDate.getDate() + 1);
    endDate.setHours(0, 0, 0);
    const orders = {
      agents: [
        {
          name: currentTruck?.id,
          shifts: [
            {
              startTime: startDate.toISOString(),
              startLocation: {
                latitude: this.site.address.latitude,
                longitude: this.site.address.longitude,
              },
              endTime: endDate.toISOString(),
              endLocation: {
                latitude: this.site.address.latitude,
                longitude: this.site.address.longitude,
              },
            },
          ],
        },
      ],
      itineraryItems: items,
    };
    this.getOptimizedRoutesFromService(orders, truckId);
  }

  getMultiOptimizedRoutes(selectedOrders, selectedTrucks) {
    const agents = [];
    selectedTrucks.forEach(truck => {
      const startDate = new Date(this.viewDate.setHours(0, 0, 0));
      const endDate = new Date(this.viewDate);
      endDate.setDate(endDate.getDate() + 1);
      endDate.setHours(0, 0, 0);
      const agent = {
        name: truck.name,
        shifts: [
          {
            startTime: startDate.toISOString(),
            startLocation: {
              latitude: this.site.address.latitude,
              longitude: this.site.address.longitude,
            },
            endTime: endDate.toISOString(),
            endLocation: {
              latitude: this.site.address.latitude,
              longitude: this.site.address.longitude,
            },
          },
        ],
      };
      agents.push(agent);
    });

    const itineraryItems = [];
    selectedOrders.forEach(order => {
      const address =
        order.orderTypeId === this.patientMoveId || order.orderTypeId === this.pickupId
          ? order.pickupAddress
          : order.deliveryAddress;

      const formatStartEndResponse = this.formatStartAndEndTime(order);
      const requestedStartDateTime = formatStartEndResponse.startTime;
      const requestedEndDateTime = formatStartEndResponse.endTime;

      const item = {
        openingTime: requestedStartDateTime.toISOString(),
        closingTime: requestedEndDateTime.toISOString(),
        dwellTime: convertMinToTime(order.processingTime),
        priority: 1,
        location: {
          latitude: address.latitude,
          longitude: address.longitude,
        },
        name: order.id,
      };
      itineraryItems.push(item);
    });

    const body = {
      agents,
      itineraryItems,
    };
    this.optimizeLoading = true;

    this.routeOptimizationService
      .getOptimizedRoutes(body)
      .pipe(
        finalize(() => {
          this.selectOrdermodal.hideDialog();
          this.optimizeLoading = false;
        })
      )
      .subscribe((response: any) => {
        this.applyMultipleOptimizedRoutesToTruck(response);
        this.getTruckAvailableTimes();
      });
  }

  getOptimizedRoutesFromService(orders, truckId) {
    this.showRouteConfirmDialog();
    this.loadingRouteOptimzer = true;
    this.optimizedForTruckId = truckId;
    this.routeOptimizationService
      .getOptimizedRoutes(orders)
      .pipe(
        finalize(() => {
          this.loadingRouteOptimzer = false;
          this.cd.markForCheck();
        })
      )
      .subscribe(
        (response: any) => {
          response = response.resourceSets[0].resources[0];
          const unAssignedOrders = [];
          response.unscheduledItems?.forEach(item => {
            unAssignedOrders.push(this.backupOrders.find(o => o.id.toString() === item.name));
          });

          this.routeOptimizerResponse = {
            driverItineraries: response.agentItineraries,
            unAssignedOrders,
            unUsedTrucks: [],
          };
          this.unAssignedRoutes = response.unscheduledItems?.map((uI: any) => ({
            ...uI,
            order: this.backupOrders.find((o: any) => o.id === Number(uI.name)),
          }));
          let optimizedRoutes = [];
          if (response.agentItineraries.length > 0) {
            optimizedRoutes = response.agentItineraries[0].instructions.map(instruction => {
              if (instruction.instructionType === 'VisitLocation' && instruction.itineraryItem) {
                return instruction;
              }
              return [];
            });
            optimizedRoutes = optimizedRoutes.flat();
          }
          this.optimizedRoutes = optimizedRoutes.map((route: any) => {
            return {
              ...route,
              title: '',
              startTime: new Date(route.startTime),
              endTime: new Date(route.endTime),
              order: this.backupOrders.find(
                (or: any) => or.id === Number(route.itineraryItem.name)
              ),
            };
          });

          if (this.optimizedRoutes.length > 0) {
            const warehousePickup = this.events.find(
              x => x.title === 'Warehouse Pickup' && x.meta?.truck?.id === truckId
            );
            const warehouseDrop = this.events.find(
              x => x.title === 'Warehouse Drop' && x.meta?.truck?.id === truckId
            );
            this.optimizedRoutes.unshift({
              title: warehousePickup.title,
              startTime: warehousePickup.start,
              endTime: warehousePickup.end,
              order: warehousePickup.meta.order,
            });
            this.optimizedRoutes.push({
              title: warehouseDrop.title,
              startTime: warehouseDrop.start,
              endTime: warehouseDrop.end,
              order: warehouseDrop.meta.order,
            });
          }

          this.applyOptimizedRoutesToTruck();
        },
        (error: any) => {
          throw error;
        }
      );
  }

  showRouteConfirmDialog() {
    this.optimizedRoutesVisible = true;
    this.cd.markForCheck();
  }

  applyOptimizedRoutesToTruck() {
    const truckId = this.optimizedForTruckId;
    this.events.forEach((event: any, index: number) => {
      if (event.meta.truck.id === truckId) {
        const order = this.optimizedRoutes.find(
          route => Number(route.itineraryItem?.name) === event.meta.orderId
        );
        if (order) {
          this.events[index].start = new Date(order.startTime);
          this.events[index].end = new Date(order.endTime);
        }
      }
    });
    this.unAssignedRoutes.map((r: any) => {
      const index = this.events.findIndex((e: any) => e.meta.orderId === Number(r.name));
      if (index >= 0) {
        this.events.splice(index, 1);
      }
    });
    this.events = [...this.events];
    this.cd.markForCheck();
    this.closeOptimizedRoutesPreview();
  }

  closeOptimizedRoutesPreview() {
    this.optimizedRoutesVisible = false;
    this.eventsForTruck = [];
    this.optimizedRoutes = [];
    this.unAssignedRoutes = [];
  }

  getLocalTimeString(date) {
    return date.toLocaleTimeString();
  }

  closeAssignOrderPreview() {
    this.assignOrderVisible = false;
    this.eventsForTruck = [];
  }

  activeDroppedIndex(event) {
    this.currentDroppedIndex = event.index;
  }

  getTruckAssignment(truckId) {
    this.eventsForTruck = this.events.filter((event: CalendarEvent) => {
      return event.meta.truck.id === truckId;
    });
    this.eventsForTruck.sort((a: any, b: any) => {
      if (a.start < b.start) {
        return -1;
      }
      if (a.start > b.start) {
        return 1;
      }
      return 0;
    });
    this.assignToTruckId = truckId;
    this.assignOrderVisible = true;
  }
  assignOrderToTruck() {
    let ordersToAssign = [];
    this.loadingOrderAssignment = true;
    this.events.filter((event: any) => {
      if (event.meta.truck.id === this.assignToTruckId) {
        const dispatchDetail = {
          orderHeaderId: event.meta.orderId,
          transferRequestId: null,
          dispatchStartDateTime: changeToISO(event.start),
          dispatchEndDateTime: changeToISO(event.end),
        };
        ordersToAssign = [...ordersToAssign, dispatchDetail];
      }
    });
    let sequence = 0;
    ordersToAssign = sortBy(ordersToAssign, 'dispatchStartDateTime').map(x => {
      if (x.orderHeaderId) {
        x.sequenceNumber = ++sequence;
      }
      return x;
    });
    const body = {
      vehicleId: this.assignToTruckId,
      dispatchDetails: ordersToAssign,
    };
    this.dispatchService
      .dispatchAssign(body)
      .pipe(
        finalize(() => {
          this.loadingOrderAssignment = false;
          this.closeAssignOrderPreview();
          this.cd.markForCheck();
        })
      )
      .subscribe(
        (response: any) => {
          this.toasterService.showSuccess('Order has been successfully assigned to the truck');
          this.trucks = this.trucks?.map((truck: any) =>
            truck.id !== this.assignToTruckId
              ? truck
              : {
                  ...truck,
                  hasAssignedOrders: ordersToAssign.length ? true : false,
                }
          );

          this.dispatchScheduler?.truckColumnBackupById(this.assignToTruckId);
        },
        (error: any) => {
          throw error;
        }
      );
  }
  removeFromScheduler(order) {
    if (order.meta.truck) {
      this.events = this.events.filter(event => event.id !== order.id);
      order.meta.order.event = order;
      delete order.meta.order.event.meta.truck;
      this.orders = [...this.orders, order.meta.order];
    }
  }
  closeOrderDetails(event) {
    this.detailsViewOpen = false;
  }
  showOrderDetails(event: any) {
    this.currentOrder = event.order;
    this.currentOrder.patient.name =
      this.currentOrder.patient.firstName + ' ' + this.currentOrder.patient.lastName;
    this.getSiteInfo();
    this.getOrderDetailInformation();
    this.detailsViewOpen = true;
  }
  getOrderDetailInformation() {
    this.fulfilledItems = [];
    this.orderHeaderService
      .getOrderHeaderById(this.currentOrder.id, true)
      .subscribe((response: any) => {
        this.currentOrder.orderNotes = response?.orderNotes ?? [];
        this.currentOrder.nurse = response?.orderingNurse ?? '';
        this.fulfilledItems = response?.orderFulfillmentLineItems ?? [];
        this.currentOrder.createdByUser = response?.createdByUser ?? '';
        this.currentOrder.modifiedByUser = response?.modifiedByUser ?? '';
        this.currentOrder.assignedDriver = response?.assignedDriver ?? '';
      });
    this.patientService
      .getPatientNotes(this.currentOrder.patient?.id)
      .subscribe((response: any) => {
        this.currentOrder.patientNotes = response;
      });
  }
  getSiteInfo() {
    if (this.checkPermission('Site', 'Read')) {
      this.sitesService.searchSites({searchQuery: ''}).subscribe(res => {
        const site = res.records.find(x => x.id === this.currentOrder.siteId);
        this.currentOrder.site = site;
      });
    }
  }

  onChangeDayView() {
    const vehicleId = this.trucks?.flatMap((driver: any) => {
      return [driver.id];
    });
    this.getDispatchInstructions(vehicleId);
  }
  getCount(status) {
    return this.dispatchInsRes.filter(x => x.orderHeader.orderStatus === status).length;
  }
  checkPermission(module, action) {
    return IsPermissionAssigned(module, action);
  }

  openOrdersSelection() {
    this.orders = this.orders.map(order => {
      return {
        ...order,
        optimizeSelected: false,
      };
    });
    this.trucks = this.trucks?.map(truck => {
      return {
        ...truck,
        optimizeSelected: false,
      };
    });
    this.selectAllTrucks = false;
    this.selectAllOrders = false;
    this.selectOrdersVisible = true;
  }

  closeOrdersSelection() {
    this.selectOrdersVisible = false;
    this.selectedOrders = [];
    this.selectedTrucks = [];
  }

  selectAllTrucksOptimization(event) {
    this.selectAllTrucks = event.checked;
    this.trucks = this.trucks?.map(truck => {
      return {
        ...truck,
        optimizeSelected: truck.hasAssignedOrders ? false : event.checked,
      };
    });
    this.selectedTrucks = this.trucks?.filter(truck => truck.optimizeSelected);
  }

  selectTruckOptimization(event, truck) {
    this.trucks = this.trucks?.map(e =>
      e.id === truck.id ? {...e, optimizeSelected: event.checked} : e
    );
    this.selectedTrucks = this.trucks?.filter(t => t.optimizeSelected);
    this.selectAllTrucks = this.trucks?.length === this.selectedTrucks;
  }

  selectAllOrdersOptimization(event) {
    this.selectAllOrders = event.checked;
    this.orders = this.orders.map(order => {
      return {
        ...order,
        optimizeSelected: event.checked,
      };
    });
    this.selectedOrders = this.orders.filter(order => order.optimizeSelected);
  }

  selectOrderOptimization(event, order) {
    this.orders = this.orders.map(e =>
      e.id === order.id ? {...e, optimizeSelected: event.checked} : e
    );
    this.selectedOrders = this.orders.filter(o => o.optimizeSelected);
  }

  closeOrderDateChangeModal() {
    this.orderDateChangeVisible = false;
  }

  optimizeOrders() {
    if (!this.rescheduleOrderPassed) {
      const dateDiffOrders = this.orders.filter(
        e =>
          e.optimizeSelected &&
          getUTCDateAsLocalDate(this.viewDate) !== getUTCDateAsLocalDate(e.orderDateTime)
      );
      if (dateDiffOrders.length === 0) {
        this.performOptimization();
        this.selectOrdermodal.hideDialog();
      } else {
        this.orderDateChangeVisible = true;
      }
    } else {
      this.getMultiOptimizedRoutes(this.selectedOrders, this.selectedTrucks);
    }
  }

  confirmOrdersDateChange() {
    this.changeOrderDateModal.hideDialog();
    this.rescheduleOrderPassed = true;
  }

  performOptimization() {
    this.changeOrderDateModal.hideDialog();
    this.rescheduleOrderPassed = true;
  }

  checkOptimizeIsPossible() {
    let isTruckSelected = false;
    let isOrderSelected = false;

    this.trucks?.forEach(truck => {
      if (truck.optimizeSelected) {
        isTruckSelected = true;
      }
    });
    this.orders?.forEach(order => {
      if (order.optimizeSelected) {
        isOrderSelected = true;
      }
    });

    return !(isTruckSelected && isOrderSelected);
  }

  formatStartAndEndTime(order) {
    const requestedStartDateTime = new Date(order.requestedStartDateTime);
    const requestedEndDateTime = new Date(order.requestedEndDateTime);

    const day = new Date(this.viewDate).getDate();
    const month = new Date(this.viewDate).getMonth();
    const year = new Date(this.viewDate).getFullYear();

    if (requestedStartDateTime.getDate() !== day) {
      requestedStartDateTime.setDate(day);
    }
    if (requestedStartDateTime.getMonth() !== month) {
      requestedStartDateTime.setMonth(month);
    }
    if (requestedStartDateTime.getFullYear() !== year) {
      requestedStartDateTime.setFullYear(year);
    }

    if (requestedEndDateTime.getDate() !== day) {
      requestedEndDateTime.setDate(day);
    }
    if (requestedEndDateTime.getMonth() !== month) {
      requestedEndDateTime.setMonth(month);
    }
    if (requestedEndDateTime.getFullYear() !== year) {
      requestedEndDateTime.setFullYear(year);
    }

    const processingHour = Math.floor(order.processingTime / 60);
    const processingMin = order.processingTime - processingHour * 60;
    requestedStartDateTime.setHours(requestedStartDateTime.getHours() - processingHour);
    requestedStartDateTime.setMinutes(requestedStartDateTime.getMinutes() - processingMin);

    if (requestedStartDateTime.getTime() > requestedEndDateTime.getTime()) {
      requestedEndDateTime.setHours(requestedStartDateTime.getHours() + 24);
    }

    return {
      startTime: requestedStartDateTime,
      endTime: requestedEndDateTime,
    };
  }

  applyMultipleOptimizedRoutesToTruck(response) {
    response = response.resourceSets[0].resources[0];

    const unAssignedOrders = [];
    response.unscheduledItems?.forEach(item => {
      unAssignedOrders.push(this.backupOrders.find(o => o.id.toString() === item.name));
    });

    const unUsedTrucks = [];
    response.unusedAgents?.forEach(agent => {
      unUsedTrucks.push(this.trucks?.find(t => t.cvn.toString() === agent.name));
    });

    const optimizedRouteResponse = {
      driverItineraries: response.agentItineraries,
      unAssignedOrders,
      unUsedTrucks,
    };

    this.routeOptimizerResponse = optimizedRouteResponse;

    const optimizedRoutes = [];
    optimizedRouteResponse.driverItineraries.forEach(itinerary => {
      const truck = this.trucks?.find(selectedTruck => selectedTruck.name === itinerary.agent.name);
      let routes = itinerary.instructions.map(instruction => {
        if (instruction.instructionType === 'VisitLocation' && instruction.itineraryItem) {
          return instruction;
        }
        return [];
      });
      routes = routes.flat();
      routes = routes.map(route => {
        return {
          ...route,
          title: '',
          startTime: new Date(route.startTime),
          endTime: new Date(route.endTime),
          order: this.backupOrders.find((or: any) => or.id === Number(route.itineraryItem.name)),
        };
      });
      if (routes.length > 0) {
        const events = [];
        routes.forEach((route, index) => {
          const event = {
            actions: this.actions,
            allDay: false,
            draggable: true,
            end: route.endTime,
            id: this.events.length + index,
            meta: {
              address: {...route.order.deliveryAddress},
              order: {...route.order},
              orderId: route.order.id,
              processingTime: route.order.processingTime,
              truck: {
                id: truck.id,
                name: truck.name,
              },
            },
            pickupTime: 0,
            start: route.startTime,
            title: this.getOrderTitle(route.order),
          };
          events.push(event);

          // remove from dispatch list
          const orderIndex = this.orders.findIndex(x => x.id === event.meta.orderId);
          if (orderIndex > -1) {
            this.orders.splice(orderIndex, 1);
          }
          this.orders = [...this.orders];
        });

        // calculate pickup time
        if (events.length > 0) {
          const nonWarehouseOrders = this.events.filter(
            e =>
              e.title !== 'Warehouse Pickup' &&
              e.title !== 'Warehouse Drop' &&
              e.title !== 'Driving Block' &&
              e.meta?.truck?.id === truck.id
          );
          if (nonWarehouseOrders.length === 0) {
            const warehousePickup = calculateWarehouseTime(
              events,
              'pickup',
              this.events.length + events.length,
              truck
            );
            const warehouseDrop = calculateWarehouseTime(
              events,
              'drop',
              this.events.length + events.length + 1,
              truck
            );
            events.push(warehousePickup);
            events.push(warehouseDrop);
          } else {
            const pickupIdx = this.events.findIndex(
              y => y.meta?.truck?.id === truck.id && y.title === 'Warehouse Pickup'
            );
            if (pickupIdx > -1) {
              this.events[pickupIdx] = calculateWarehouseTime(
                nonWarehouseOrders,
                'pickup',
                this.events[pickupIdx].id,
                truck
              );
            } else {
              const warehousePickup = calculateWarehouseTime(
                nonWarehouseOrders,
                'pickup',
                this.events.length + events.length,
                truck
              );
              events.push(warehousePickup);
            }

            const dropIdx = this.events.findIndex(
              y => y.meta?.truck?.id === truck.id && y.title === 'Warehouse Drop'
            );
            if (dropIdx > -1) {
              this.events[dropIdx] = calculateWarehouseTime(
                nonWarehouseOrders,
                'drop',
                this.events[dropIdx].id,
                truck
              );
            } else {
              const warehousePickup = calculateWarehouseTime(
                nonWarehouseOrders,
                'drop',
                this.events.length,
                truck
              );
              events.push(warehousePickup);
            }
          }
        }
        this.events = [...this.events, ...events];
      }
      optimizedRoutes.push({
        truckID: truck.id,
        routes,
      });
    });
    this.optimizedRoutes = optimizedRoutes;

    if (
      this.routeOptimizerResponse.unAssignedOrders.length > 0 ||
      this.routeOptimizerResponse.unUsedTrucks.length > 0
    ) {
      this.openOptimizatinoStatusModal();
    }
  }

  onChangeOrderDate($event) {
    this.viewDate = new Date($event);
    this.onChangeDayView();
  }

  isToday(date) {
    return date.toDateString() === this.today.toDateString();
  }

  getTruckAvailableTimes() {
    this.trucksAvailableTimes = [];
    this.trucks.forEach(truck => {
      const unAvailableTimes = [];
      let assignedEventsToTruck = this.events.filter(event => event?.meta?.truck?.id === truck.id);
      assignedEventsToTruck = assignedEventsToTruck.sort(
        (a, b) => a.start.getTime() - b.start.getTime()
      );

      assignedEventsToTruck.forEach(event => {
        unAvailableTimes.push({
          start: event.start,
          end: event.end,
        });
      });

      const availableTimes = [];
      const startDay = new Date(this.viewDate.setHours(0, 0, 0));
      const nextDay = new Date(this.viewDate);
      nextDay.setDate(nextDay.getDate() + 1);
      nextDay.setHours(0, 0, 0);

      unAvailableTimes.forEach((unavaliable, index) => {
        if (index === 0) {
          availableTimes.push({
            start: startDay,
            end: unavaliable.start,
          });
        } else {
          if (index !== unAvailableTimes.length - 1) {
            if (unAvailableTimes[index - 1].end !== unavaliable.start) {
              availableTimes.push({
                start: unAvailableTimes[index - 1].end,
                end: unavaliable.start,
              });
            }
          } else {
            if (unavaliable.end.getTime() - nextDay.getTime() < 0) {
              availableTimes.push({
                start: unavaliable.end,
                end: nextDay,
              });
            }
          }
        }
      });
      if (availableTimes.length === 0) {
        availableTimes.push({
          start: startDay,
          end: nextDay,
        });
      }
      this.trucksAvailableTimes.push({
        id: truck.id,
        cvn: truck.cvn,
        driverName: truck.currentDriverName,
        availableTimes,
      });
    });
  }

  openCheckRescheduleOrderModal() {
    this.reschedulableOrderVisbile = true;
  }

  closeCheckRescheduleOrderModal() {
    this.reschedulableOrderVisbile = false;
  }

  showPossibleTime() {
    this.getTruckAvailableTimes();
    this.openCheckRescheduleOrderModal();
  }

  openOptimizatinoStatusModal() {
    this.optimizationStatusModalVisible = true;
  }

  closeOptimizationStatusModal() {
    this.optimizationStatusModalVisible = false;
  }

  goBack() {
    this.location.back();
  }
}
