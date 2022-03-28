import {Component, OnInit, Input, Output, EventEmitter} from '@angular/core';
import {initMap, htmlMarker, getListPopupTemplate, markerPopup} from 'src/app/utils/map.utils';
import {getEnum} from 'src/app/utils';
import {EnumNames} from 'src/app/enums';

@Component({
  selector: 'app-dispatch-map',
  templateUrl: './dispatch-map.component.html',
  styleUrls: ['./dispatch-map.component.scss'],
})
export class DispatchMapComponent implements OnInit {
  @Input() loading = false;
  @Input() selectedOrders = [];
  @Input() site = null;
  @Output() markerSelected = new EventEmitter();
  map: any;
  orderTypes = getEnum(EnumNames.OrderTypes);
  patientMoveId = null;
  pickupId = null;
  constructor() {
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
        10
      );
      // Wait until the map resources are ready to apply markers.
      this.map.events.add('ready', () => {
        const primaryMarker = htmlMarker(this.site?.address, '#d96329', true, this.site?.name);
        this.map.markers.add(primaryMarker);
        this.selectedOrders.map(order => {
          const title = `${order.id} (${order.statOrder ? 'STAT-' : ''}${order.orderType}) : ${
            order.processingTime
          } mins`;
          const secondaryMarker = htmlMarker(
            order.orderTypeId === this.patientMoveId || order.orderTypeId === this.pickupId
              ? order.pickupAddress
              : order.deliveryAddress,
            '#0570af',
            false,
            title,
            order.id
          );
          this.map.events.add('click', secondaryMarker, event => {
            this.highlightOrder(event);
          });
          this.map.markers.add(secondaryMarker);
          this.addMarkerPopup(secondaryMarker, order);
        });
      });
    });
  }

  addMarkerPopup(secondaryMarker, order) {
    const listValues = [
      {title: 'Order', value: `#${order.orderNumber}`},
      {
        title: 'Patient Name',
        value: `${order.patient.firstName} ${order.patient.lastName}`,
      },
      {
        title: 'Address',
        value: `${order.deliveryAddress.addressLine1} ${order.deliveryAddress.city}, ${order.deliveryAddress.state}, ${order.deliveryAddress.zipCode}`,
      },
      {
        title: 'Items',
        value: this.getOrderList(order.orderLineItems),
      },
    ];
    if (order.driver) {
      listValues.push({title: 'Driver', value: `${order.driver}`});
    }
    const content = getListPopupTemplate(listValues);
    const popup = markerPopup();
    this.map.events.add('mouseover', secondaryMarker, e => {
      popup.setOptions({
        content,
        position: e.target.options.position,
      });
      popup.open(this.map);
    });

    this.map.events.add('mouseleave', secondaryMarker, () => {
      popup.close();
    });
  }

  getOrderList(orderList) {
    return `${orderList?.map(x => {
      return ' ' + x?.item?.name;
    })}`;
  }

  highlightOrder(event) {
    this.markerSelected.emit(parseInt(event.target.element.children[0].id, 10));
  }
}
