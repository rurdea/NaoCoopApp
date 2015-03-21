using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NaoCoopApp.Helpers;

namespace NaoCoopApp.Validators
{
    public class ModelValidatorBase<T> : ModelBase<T>, IDataErrorInfo
        where T : NaoCoopObjects.Classes.NaoCoopObject
    {
        #region Members
        private readonly Dictionary<string, PropertyInfo> validatingProperties;
        private readonly Dictionary<string, ValidationAttribute[]> validators;
        #endregion

        #region Constructor
        public ModelValidatorBase() : this (null)
        {
        }

        public ModelValidatorBase(T data)
            : base(data)
        {
            validatingProperties = this.GetType().GetProperties()
                                  .Where(p => GetValidations(p).Length != 0)
                                  .ToDictionary(p => p.Name, p => p);
            validators = this.GetType().GetProperties()
                             .Where(p => GetValidations(p).Length != 0)
                             .ToDictionary(p => p.Name, p => GetValidations(p));
        }
        #endregion

        #region IDataErrorInfo
        public string Error
        {
            get
            {
                var errors = from i in validators
                             from v in i.Value
                             where !v.IsValid(validatingProperties[i.Key].GetValue(this))
                             select v.ErrorMessage;
                return string.Join(Environment.NewLine, errors.ToArray());
            }
        }

        public string this[string columnName]
        {
            get
            {
                if (validatingProperties.ContainsKey(columnName))
                {
                    var value = validatingProperties[columnName].GetValue(this);
                    var errors = validators[columnName].Where(v => !v.IsValid(value))
                        .Select(v => v.ErrorMessage).ToArray();
                    this.OnPropertyChanged(() => Error);
                    return string.Join(Environment.NewLine, errors);
                }

                this.OnPropertyChanged(() => Error);
                return string.Empty;
            }
        }
        #endregion

        #region Methods
        private ValidationAttribute[] GetValidations(PropertyInfo property)
        {
            return (ValidationAttribute[])property.GetCustomAttributes(typeof(ValidationAttribute), true);
        }
        #endregion
    }
}
