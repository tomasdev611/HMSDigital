import {
  Component,
  OnChanges,
  Input,
  Output,
  EventEmitter,
  ChangeDetectorRef,
  Inject,
  LOCALE_ID,
  SimpleChanges,
} from '@angular/core';
import {
  DayViewSchedulerCalendarUtils,
  DayViewScheduler,
  Truck,
  sortBy,
  addMinutes,
  calculateWarehouseTime,
  scrollTo,
} from 'src/app/utils';
import {DateAdapter, CalendarWeekViewComponent, getWeekViewPeriod} from 'angular-calendar';
import {WeekViewTimeEvent, WeekViewAllDayEvent, CalendarEvent} from 'calendar-utils';
import {DragEndEvent, DragMoveEvent} from 'angular-draggable-droppable';

@Component({
  selector: 'app-dispatch-scheduler',
  templateUrl: './dispatch-scheduler.component.html',
  styleUrls: ['./dispatch-scheduler.component.scss'],
  providers: [DayViewSchedulerCalendarUtils],
})
export class DispatchSchedulerComponent extends CalendarWeekViewComponent implements OnChanges {
  @Input() trucks: Truck[] = [];
  @Input() events;
  @Output() truckChanged = new EventEmitter();
  @Output() optimize = new EventEmitter<any>();
  @Output() changeDroppedIndex = new EventEmitter();
  @Output() assignToTruck = new EventEmitter<any>();
  @Output() showOrderDetails = new EventEmitter<any>();

  view: DayViewScheduler;
  daysInWeek = 1;
  centered = false;
  truckHaveOrderList = [];

  showFlyout = false;
  selectedVehicle: any;

  constructor(
    protected cdr: ChangeDetectorRef,
    protected utils: DayViewSchedulerCalendarUtils,
    @Inject(LOCALE_ID) locale: string,
    protected dateAdapter: DateAdapter
  ) {
    super(cdr, utils, locale, dateAdapter);
  }

  trackByTruckId = (index: number, row: Truck) => row.id;

  ngOnChanges(changes: SimpleChanges): void {
    if (!this.centered) {
      const element = document.getElementById('hour14');
      if (element) {
        this.centered = true;
        scrollTo(element);
      }
    }
    super.ngOnChanges(changes);
    if (changes.trucks) {
      this.refreshBody();
      this.emitBeforeViewRender();

      this.trucks.forEach(truck => {
        const truckAux = {
          id: truck.id,
          disable: true,
          ordersIdList: [],
        };

        this.truckHaveOrderList.push(truckAux);
      });
    }
  }

  getDayColumnWidth(eventRowContainer: HTMLElement): number {
    return Math.floor(eventRowContainer.offsetWidth / this.trucks.length);
  }

  dragMove(dayEvent: WeekViewTimeEvent, dragEvent: DragMoveEvent) {
    if (this.snapDraggedEvents) {
      const newTruck = this.getDraggedTruckColumn(dayEvent, dragEvent.x);
      const newEventTimes = this.getDragMovedEventTimes(
        dayEvent,
        {...dragEvent, x: 0},
        this.dayColumnWidth,
        true
      );
      const originalEvent = dayEvent.event;
      const adjustedEvent = {
        ...originalEvent,
        ...newEventTimes,
        meta: {...originalEvent.meta, truck: newTruck},
      };
      const tempEvents = this.events.map(event => {
        if (event === originalEvent) {
          return adjustedEvent;
        }
        return event;
      });
      this.restoreOriginalEvents(tempEvents, new Map([[adjustedEvent, originalEvent]]));
    }
    this.dragAlreadyMoved = true;
  }

  dragEnded(
    weekEvent: WeekViewAllDayEvent | WeekViewTimeEvent,
    dragEndEvent: DragEndEvent,
    dayWidth: number,
    useY = false
  ) {
    super.dragEnded(
      weekEvent,
      {
        ...dragEndEvent,
        x: 0,
      },
      dayWidth,
      useY
    );
    const newTruck = this.getDraggedTruckColumn(weekEvent, dragEndEvent.x);
    if (newTruck && newTruck !== weekEvent.event.meta.truck) {
      this.truckChanged.emit({event: weekEvent.event, newTruck});
    }
  }

  protected getWeekView(events: CalendarEvent[]) {
    const tempEvents = events;

    this.trucks.map(x => {
      const eventTrucks = sortBy(
        tempEvents.filter(y => {
          if (y.meta?.truck?.id === x.id) {
            y.end = addMinutes(y.start, y.meta.processingTime).toDate();
            return y;
          }
        }),
        'start'
      );

      if (eventTrucks && eventTrucks.length > 0) {
        const nonWarehouseOrders = eventTrucks.filter(
          y =>
            y.title !== 'Warehouse Pickup' &&
            y.title !== 'Warehouse Drop' &&
            y.title !== 'Driving Block'
        );

        if (nonWarehouseOrders.length === 0) {
          const pickupIdx = tempEvents.findIndex(
            y => y.meta?.truck?.id === x.id && y.title === 'Warehouse Pickup'
          );
          if (pickupIdx > -1) {
            tempEvents.splice(pickupIdx, 1);
          }
          const dropIdx = tempEvents.findIndex(
            y => y.meta?.truck?.id === x.id && y.title === 'Warehouse Drop'
          );
          if (dropIdx > -1) {
            tempEvents.splice(dropIdx, 1);
          }
        } else {
          const pickupIdx = tempEvents.findIndex(
            y => y.meta?.truck?.id === x.id && y.title === 'Warehouse Pickup'
          );
          if (pickupIdx > -1) {
            tempEvents[pickupIdx] = calculateWarehouseTime(
              nonWarehouseOrders,
              'pickup',
              tempEvents[pickupIdx].id,
              x
            );
          } else {
            const warehousePickup = calculateWarehouseTime(
              nonWarehouseOrders,
              'pickup',
              tempEvents.length + 1,
              x
            );
            tempEvents.push(warehousePickup);
          }
          const dropIdx = tempEvents.findIndex(
            y => y.meta?.truck?.id === x.id && y.title === 'Warehouse Drop'
          );
          if (dropIdx > -1) {
            tempEvents[dropIdx] = calculateWarehouseTime(
              nonWarehouseOrders,
              'drop',
              tempEvents[dropIdx].id,
              x
            );
          } else {
            const warehouseDrop = calculateWarehouseTime(
              nonWarehouseOrders,
              'drop',
              tempEvents.length + 1,
              x
            );
            tempEvents.push(warehouseDrop);
          }
        }
      }

      if (this.events.length > 0) {
        this.truckAssignDisableByView(tempEvents, x.id);
      }
    });

    return this.utils.getWeekView({
      events,
      trucks: this.trucks,
      viewDate: this.viewDate,
      weekStartsOn: this.weekStartsOn,
      excluded: this.excludeDays,
      precision: this.precision,
      absolutePositionedEvents: true,
      hourSegments: this.hourSegments,
      dayStart: {
        hour: 0,
        minute: 0,
      },
      dayEnd: {
        hour: 23,
        minute: 59,
      },
      segmentHeight: this.hourSegmentHeight,
      weekendDays: this.weekendDays,
      ...getWeekViewPeriod(
        this.dateAdapter,
        this.viewDate,
        this.weekStartsOn,
        this.excludeDays,
        this.daysInWeek
      ),
    });
  }

  private getDraggedTruckColumn(
    dayEvent: WeekViewTimeEvent | WeekViewAllDayEvent,
    xPixels: number
  ) {
    const columnsMoved = Math.round(xPixels / this.dayColumnWidth);
    const currentColumnIndex = this.view.trucks.findIndex(
      truck => truck.id === dayEvent.event.meta.truck.id
    );
    const newIndex = currentColumnIndex + columnsMoved;
    return this.view.trucks[newIndex];
  }

  onClickOrders(timeEvent: any) {
    const order = timeEvent.event.meta;
    if (!!order.orderId) {
      this.showOrderDetails.emit(order);
    }
  }

  optimizeRoute(truckId) {
    this.optimize.emit(truckId);
  }
  dragEnter(time) {}
  dragLeave(time) {}
  changeDropIndex(index) {
    this.changeDroppedIndex.emit({index});
  }
  assignOrder(truckId) {
    this.assignToTruck.emit(truckId);
  }

  truckAssignDisableByView(eventlist, truckId) {
    const truckColumn = this.truckHaveOrderList.find(truck => truck.id === truckId);
    let isDisable = true;

    if (truckColumn.ordersIdList.length === 0) {
      const orderAdded = eventlist.find(x => x.meta.truck.id === truckId);

      if (orderAdded) {
        isDisable = false;
      }
    } else {
      isDisable = false;
    }

    truckColumn.disable = isDisable;
  }

  truckColumnsBackupOrdersAssignedList(setDisableTruckColumn: boolean) {
    this.truckHaveOrderList.map(truck => {
      truck.ordersIdList = [];
    });

    this.events.forEach(eventOrder => {
      const eventMeta = eventOrder.meta;
      this.truckHaveOrderList.forEach(truck => {
        if (truck.id === eventMeta.truck.id) {
          truck.ordersIdList.push(eventMeta.order.id);
        }
      });
    });

    if (setDisableTruckColumn) {
      this.truckHaveOrderList.forEach(truck => {
        this.truckAssignDisableByView(this.events, truck.id);
      });
    }
  }

  truckColumnBackupById(truckId: number) {
    const truckColumn = this.truckHaveOrderList.find(x => x.id === truckId);
    truckColumn.ordersIdList = [];

    this.events.forEach(eventOrder => {
      const eventMeta = eventOrder.meta;
      if (truckId === eventMeta.truck.id) {
        truckColumn.ordersIdList.push(eventMeta.order.id);
      }
    });

    this.truckAssignDisableByView(this.events, truckId);
  }

  showInventoryFlyout(vehicle) {
    this.selectedVehicle = vehicle;
    this.showFlyout = true;
  }

  closeInventoryFlyout() {
    this.showFlyout = false;
    this.selectedVehicle = null;
  }
  isActionEnabled(truckId) {
    const assignedOrders = this.events.filter(
      e =>
        e.title !== 'Warehouse Drop' &&
        e.title !== 'Warehouse Pickup' &&
        e.meta?.truck?.id === truckId
    );
    if (assignedOrders.length) {
      return true;
    }
    return false;
  }
}
