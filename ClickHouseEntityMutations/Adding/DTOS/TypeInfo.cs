using System;

namespace EntityTracking
{
    public class TypeInfo
    {
        public override bool Equals(object obj)
        {
            var typeInfo = ((TypeInfo) obj).Type;
            return typeInfo.Name == Type.Name;
        }

        public override int GetHashCode()
        {
            if (Type == null) return 0;
            return Type.GetHashCode();
        }

        public Type Type { get; set; }
    }
}