using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.BusinessLayer.Validations
{
    public class ValidatedList<T>
    {
        public bool IsValid { get; set; }

        public IEnumerable<ValidatedItem<T>> ValidItems { get; set; }

        public IEnumerable<ValidatedItem<T>> InvalidItems { get; set; }
    }
}
