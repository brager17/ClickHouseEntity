using System;

namespace ExpressionTreeVisitor
{
    public class NameAttribute : Attribute, IHasKey<string>
    {
        public string Key { get; set; }

        public NameAttribute(string name)
        {
            Key = name;
            ObjectKey = name;
        }


        public object ObjectKey { get; set; }
    }
}