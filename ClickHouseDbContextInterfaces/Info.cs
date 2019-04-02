using System;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace ExpressionTreeVisitor
{
    // marker
    public interface IHasName
    {
        string Name { get; set; }
    }

    public class NameAttribute : Attribute, IHasName
    {
        public string Name { get; set; }

        public NameAttribute(string name)
        {
            Name = name;
        }
    }

    public class InfoAttribute : NameAttribute
    {
        public string Name { get; set; }

        public InfoAttribute(string name) : base(name)
        {
            Name = name;
        }
    }

    public static class IHasNameExtensions
    {
        public static string GetName(this Enum @enum)
        {
            var enumType = @enum.GetType();
            var enumMember = enumType.GetMember(@enum.ToString()).Single();
            var attribute = enumMember.GetCustomAttribute<NameAttribute>();
            var name = attribute?.Name ?? enumMember.Name;
            return name;
        }
    }
}