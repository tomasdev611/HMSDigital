using HMSDigital.Fulfillment.ViewModels;
using RestEase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMSDigital.Fulfillment.API
{
    public interface IBingMapsAPI
    {
        [Query("key")]
        string AccessKey { get; set; }

        [Header("Content-Type", "application/json")]
        [Post("Routes/OptimizeItinerary")]
        Task<Response<Root>> GetOptimizedItinerary([Body] OptimizeRouteRequest optimizeRouteRequest);
    }
}
