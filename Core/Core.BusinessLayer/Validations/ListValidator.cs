using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace HMSDigital.Core.BusinessLayer.Validations
{
    public class ListValidator<T, K> where T : AbstractValidator<K>
    {
        private readonly T _validator;

        public ListValidator()
        {
            _validator = Activator.CreateInstance<T>();
        }

        public ValidatedList<K> Validate(IEnumerable<K> itemList)
        {

            var validItems = new List<ValidatedItem<K>>();
            var invalidItems = new List<ValidatedItem<K>>();

            var index = 0;
            var validList = true;
            foreach (var item in itemList)
            {
                var result = _validator.Validate(item);
                if (!result.IsValid)
                {
                    validList = false;

                    var invalidItem = new ValidatedItem<K>()
                    {
                        Value = item,
                        Errors = (result.Errors.Select(er => er.ErrorMessage).ToList()),
                        RowIndex = ++index
                    };
                    invalidItems.Add(invalidItem);

                }
                else
                {
                    var validItem = new ValidatedItem<K>()
                    {
                        Value = item,
                        RowIndex = ++index
                    };
                    validItems.Add(validItem);
                }
            }
            var validatedList = new ValidatedList<K>()
            {
                ValidItems = validItems,
                InvalidItems = invalidItems,
                IsValid = validList
            };
            return validatedList;
        }

        public ValidatedList<U> GetFormatedList<U>(IEnumerable<U> itemList, string message, IEnumerable<string> idList, Expression<Func<U, object>> uniqueIdPredicate) where U : class
        {
            var validItems = new List<ValidatedItem<U>>();
            var invalidItems = new List<ValidatedItem<U>>();
            ValidatedItem<U> listItem;
            var uniqueIdFunc = uniqueIdPredicate.Compile();
            var index = 0;
            foreach (var item in itemList)
            {
                listItem = new ValidatedItem<U>()
                {
                    Value = item,
                    RowIndex = ++index
                };

                if (idList.Any(i => i.Equals(uniqueIdFunc(item))))
                {
                    listItem.Errors = new List<string>() { message };
                    invalidItems.Add(listItem);
                }
                else
                {
                    validItems.Add(listItem);
                }
            }

            var validatedList = new ValidatedList<U>()
            {
                ValidItems = validItems,
                InvalidItems = invalidItems,
                IsValid = false
            };
            return validatedList;
        }
    }
}
