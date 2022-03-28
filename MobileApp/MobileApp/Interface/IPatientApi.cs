using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MobileApp.Models;
using MobileApp.ViewModels;
using Refit;

namespace MobileApp.Interface
{
    public interface IPatientApi
    {
        [Get("/api/patients")]
        Task<ApiResponse<PaginatedList<PatientDetail>>> GetPatientDetailsAsync(string filters);
    }
}
