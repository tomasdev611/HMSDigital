using System.Collections.Generic;

namespace NotificationSDK.ViewModels
{
    public class OrderNotification
    {
        public string OrderNumber { get; set; }

        public string MemberName { get; set; }

        public string HospiceLocationName { get; set; }

        public string OrderStatus { get; set; }

        public string OrderPriority { get; set; }

        public string OrderDashboardUrl { get; set; }

        public IEnumerable<OrderNote> OrderNotes { get; set; }

        public IEnumerable<OrderItem> OrderItems { get; set; }
    }
}
