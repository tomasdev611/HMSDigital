using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.BusinessLayer.Validations
{
    public class ValidatedItem<T>
    {
        public T Value { get; set; }

        public IEnumerable<string> Errors { get; set; }

        public int RowIndex { get; set; }
    }
}
