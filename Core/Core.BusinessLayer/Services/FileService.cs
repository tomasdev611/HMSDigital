using CsvHelper;
using CsvHelper.Configuration;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using SystemException = System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using HMSDigital.Core.BusinessLayer.Formatter;

namespace HMSDigital.Core.BusinessLayer.Services
{
    public class FileService : IFileService
    {
        private readonly ILogger<FileService> _logger;
        public FileService(
            ILogger<FileService> logger)
        {
            _logger = logger;
        }
        public IEnumerable<T> GetRecords<T>(IFormFile formFile) where T : class
        {
            var csvRecords = GetMappedRecords<T>(formFile, null);
            return csvRecords;
        }
        public IEnumerable<T> GetMappedRecords<T>(IFormFile formFile, CsvHeaderMap<T> csvHeaderMap) where T : class
        {
            try
            {
                var fileReader = new StreamReader(formFile.OpenReadStream());
                var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HeaderValidated = null,
                    MissingFieldFound = null
                };
                var csv = new CsvReader(fileReader, csvConfiguration);
                if (csvHeaderMap != null)
                {
                    if(csvHeaderMap.MemberMaps == null || csvHeaderMap.MemberMaps.Count() == 0)
                    {
                        throw new SystemException.ValidationException($"No file mappings found, Please contact support");
                    }
                    csv.Configuration.RegisterClassMap(csvHeaderMap);
                }
                var csvRecords = csv.GetRecords<T>().ToList();

                if (csvRecords == null)
                {
                    throw new SystemException.ValidationException($"Invalid request or Data is not well formatted");
                }
                return csvRecords;
            }
            catch (CsvHelper.ReaderException rx)
            {
                throw new SystemException.ValidationException("Column headers For given file are Invalid");
            }
            catch (SystemException.ValidationException vx)
            {
                _logger.LogWarning($"Exception Occurred while Read record from file: {vx.Message}");
                throw vx;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Exception Occurred while Read record from file: {ex.Message}");
                throw ex;
            }
        }
    }
}
