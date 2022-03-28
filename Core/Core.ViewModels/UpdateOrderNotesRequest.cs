using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels
{
    public class UpdateOrderNotesRequest : OrderNotesRequest
    {
        public int? Id { get; set; }
    }
}
