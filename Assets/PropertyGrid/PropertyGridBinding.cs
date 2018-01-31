using System;
using UnityEngine;

namespace namudev
{
    public class PropertyGridBinding : MonoBehaviour
    {
        public event EventHandler ValueChanged;

        public string Caption { get; private set; }
        public object Value { get; private set; }
        public Type ValueType { get; private set; }

        private object targetObject;
        private PropertyData propertyInfo;

        public void Initialize(string caption, object value, Type valueType)
        {
            Caption = caption;
            Value = value;
            ValueType = valueType;
        }

        public void Initialize(object targetObject, PropertyData propertyInfo)
        {
            this.targetObject = targetObject;
            this.propertyInfo = propertyInfo;
            Caption = propertyInfo.PropertyName;
            Value = propertyInfo.GeneralValue;
            ValueType = propertyInfo.PropertyType;
        }

        public void SetValue(object value)
        {
            if (!Equals(this.Value, value))
            {
                Value = value;
                if (ValueChanged != null)
                {
                    ValueChanged(this, EventArgs.Empty);
                }
                if ((targetObject != null) && (propertyInfo != null))
                {
                    propertyInfo.GeneralValue = Value;
                }
            }
        }
    }
}
