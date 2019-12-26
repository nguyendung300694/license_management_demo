using System;
using System.Linq;
using System.Reflection;

namespace LicenseManagement.Helpers
{
    public static class EnumHelper
    {
        /// <summary>
        /// Gets an attribute on an enum field value
        /// </summary>
        /// <typeparam name="T">The type of the attribute you want to retrieve</typeparam>
        /// <param name="enumVal">The enum value</param>
        /// <returns>The attribute of type T that exists on the enum value</returns>
        public static T GetAttributeOfType<T>(this Enum enumVal) where T : System.Attribute
        {
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return (attributes.Length > 0) ? (T)attributes[0] : null;
        }


        public static TExpected GetAttributeValue<T, TExpected>(this Enum enumeration, Func<T, TExpected> expression) where T : Attribute
        {
            var firstOrDefault =
                enumeration.GetType().GetMember(enumeration.ToString()).FirstOrDefault(member => member.MemberType == MemberTypes.Field);
            if (firstOrDefault != null)
            {
                T attribute =
                    firstOrDefault
                        .GetCustomAttributes(typeof (T), false)
                        .Cast<T>()
                        .SingleOrDefault();

                if (attribute == null)
                    return default(TExpected);

                return expression(attribute);
            }
            return default(TExpected);
        }     
    }
}