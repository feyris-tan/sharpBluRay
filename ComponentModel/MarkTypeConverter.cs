using System;
using System.ComponentModel;
using moe.yo3explorer.sharpBluRay.Model;
using moe.yo3explorer.sharpBluRay.Model.PlaylistModel;

namespace moe.yo3explorer.sharpBluRay.ComponentModel
{
    class MarkTypeConverter : TypeConverter
    {
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            return TypeDescriptor.GetProperties(typeof(Mark));
        }
    }
}
