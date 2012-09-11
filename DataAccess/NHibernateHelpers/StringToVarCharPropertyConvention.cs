using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Customer.Project.DataAccess.NHibernateHelpers
{
    /// <summary>
    /// Ensures that all of our strings are stored as varchar instead of nvarchar.
    /// Map to varchar to substantially improve the performance of queries.
    /// </summary>
    public class StringToVarCharPropertyConvention : IPropertyConvention
    {
        public void Apply(IPropertyInstance instance)
        {
            if (instance.Property.PropertyType == typeof(string))
                instance.CustomType("AnsiString");
        }
    }
}
