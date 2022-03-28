using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace HMSDigital.Common.BusinessLayer.Config
{
    public class DbConfigOptions
    {
        public Func<IDbConnection> CreateDbConnection { get; set; }
        public string TableName { get; set; }
        public bool ReloadOnChange { get; set; } = false;
        public TimeSpan? ReloadInterval { get; set; }
    }
}
