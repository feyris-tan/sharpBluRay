using System;
using System.ComponentModel;
using moe.yo3explorer.sharpBluRay.Model;

namespace moe.yo3explorer.sharpBluRay.ComponentModel
{
    class IdTypeConverter : TypeConverter
    {
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            return TypeDescriptor.GetProperties(typeof(Id));
        }
    }
}
