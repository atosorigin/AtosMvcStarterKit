using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Customer.Project.Domain.ValueObjects
{
    /// <summary>
    /// Indicates that the given class is a ValueObject. ValueObjects are mapped by NHibernate despite 
    /// that they do not implement the IEntityId interface
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ValueObjectAttribute : System.Attribute
    { }
}
