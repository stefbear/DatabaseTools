using System;
using System.Collections.Generic;
using System.Text;

namespace Database.BusinessObjectHelper
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class DataMappingAttribute : System.Attribute
    {
        private string _dataFieldName;
        private object _nullValue;

        public DataMappingAttribute(string dataFieldName, object nullValue)
            : base()
        {
            _dataFieldName = dataFieldName;
            _nullValue = nullValue;
        }

        public DataMappingAttribute(object nullValue) : this(string.Empty, nullValue) { }


        #region Public Properties

        public string DataFieldName
        {
            get { return _dataFieldName; }
        }

        public object NullValue
        {
            get { return _nullValue; }
        }
        #endregion
    }
}
