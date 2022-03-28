export function calculateWarehouseTime(orders, type, id, truck) {
  const site = JSON.parse(sessionStorage.getItem('currentSite'));
  let title;
  let start;
  let end;
  let meta;
  if (type === 'pickup') {
    title = 'Warehouse Pickup';
    const baseTime = orders.reduce((a, b) => {
      b.pickupTime = b.meta.order.orderLineItems.reduce((x, y) => {
        y.itemPickupTime =
          y.orderType === 'Delivery' ? y.itemCount * y.item.avgPickUpProcessingTime : 0;
        return x + y.itemPickupTime;
      }, 0);
      return a + b.pickupTime;
    }, 15);
    start = new Date(orders[0].start.getTime() - 1000 * 60 * baseTime);
    end = orders[0].start;
    meta = {
      processingTime: baseTime,
    };
  } else {
    title = 'Warehouse Drop';
    const baseTime = orders.reduce((a, b) => {
      b.pickupTime = b.meta.order.orderLineItems.reduce((x, y) => {
        y.itemPickupTime =
          y.orderType !== 'Delivery' ? y.itemCount * y.item.avgDeliveryProcessingTime : 0;
        return x + y.itemPickupTime;
      }, 0);
      return a + b.pickupTime;
    }, 15);
    start = orders[orders.length - 1].end;
    end = new Date(orders[orders.length - 1].end.getTime() + 1000 * 60 * baseTime);
    meta = {
      processingTime: baseTime,
    };
  }
  const warehouse = {
    id,
    title,
    start,
    end,
    meta: {
      processingTime: meta?.processingTime ?? 20,
      orderId: null,
      order: {
        id,
        statOrder: null,
        orderType: null,
      },
      truck: {
        id: truck.id,
        name: truck.name,
      },
      address: site?.address,
    },
    color: {
      primary: '#FF7F50',
      secondary: '#f9cab8',
    },
    draggable: false,
  };
  return warehouse;
}

export function displayOrderStatus(status) {
  if (status) {
    switch (status) {
      case 'Planned':
        return 'Order Received';
      case 'Scheduled':
        return 'Assigned to Driver';
      case 'OutForFulfillment':
        return 'Out for Fulfillment';
      case 'Pending_Approval':
        return 'Pending Approval';
      case 'Loading_Truck':
        return 'Loading Truck';
      case 'Partial_Fulfillment':
        return 'Partial Fulfillment';
      case 'OnSite':
        return 'On Site';
      case 'OnTruck':
        return 'On Truck';
      case 'BackOrdered':
        return 'Back Ordered';
      default:
        return status;
    }
  } else {
    return '';
  }
}
