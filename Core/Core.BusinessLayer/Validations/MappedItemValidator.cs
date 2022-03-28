using FluentValidation;
using HMSDigital.Common.BusinessLayer.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HMSDigital.Core.BusinessLayer.Validations
{
    public class MappedItemValidator : AbstractValidator<ViewModels.MappedItemBase>
    {
        public MappedItemValidator()
        {
            RuleFor(m => m.Type).Must(CheckTypeinTypeList)
               .WithMessage(m => $"{m.Type} type is not allowed");
        }

        public bool CheckTypeinTypeList(string mappedItemType)
        {
            var typeList = new List<string>() { "numeric", "date", "alphanumeric", "string" };
            return typeList.Contains(mappedItemType);
        }
    }
}
