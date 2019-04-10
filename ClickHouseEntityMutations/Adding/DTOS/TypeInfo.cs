using System;

namespace EntityTracking
{
    public class TypeInfo
    {
        public override bool Equals(object obj)
        {
            return ((TypeInfo) obj).Type.Name == Type.Name;
        }

        public override int GetHashCode()
        {
            if (Type == null)return 0;
            return Type.ToString().GetHashCode();
        }

        public Type Type { get; set; }
    }
}