using System;
using System.Reflection;

namespace MediaHost.Domain.Repository
{
    // create custom attribute to be assigned to class members
    [AttributeUsage(
        AttributeTargets.Property,
        AllowMultiple = false)]
    public class IgnorePropertyAttribute : System.Attribute
    {
        public IgnorePropertyAttribute()
        {

        }
    }
}