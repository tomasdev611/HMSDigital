using HMSDigital.Core.BusinessLayer.Formatter;
using HMSDigital.Core.Data.Models;
using Microsoft.AspNetCore.Http;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HMSDigital.Core.BusinessLayer.Services.Interfaces
{
    public interface IFileService 
    {
        IEnumerable<T> GetRecords<T>(IFormFile formFile) where T : class;

        IEnumerable<T> GetMappedRecords<T>(IFormFile formFile, CsvHeaderMap<T> csvHeaderMap) where T : class;
    }
}
