using Customer.Project.Domain.Entities;
using Customer.Project.Domain.ValueObjects;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace Customer.Project.DataAccess.NHibernateMappings
{
   
    public class PersonMap : IAutoMappingOverride<Person>
    {
         [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        public void Override(AutoMapping<Person> mapping)
        {
            // Value object mapping example
            mapping.Component<Address>(p => p.Address, m =>
                        {
                            m.Map(x => x.City);
                            m.Map(x => x.Line1);
                            m.Map(x => x.Line2);
                            m.Map(x => x.PoBox);
                            m.Map(x => x.State);

                            m.Component(x => x.Country, m2 =>
                                                            {
                                                                m2.Map(y => y.Name,
                                                                        "CountryName");
                                                                m2.Map(y => y.Code);
                                                            });
                        });
        }
    }
    public class ProductMap : IAutoMappingOverride<Product>
    {
        public void Override(AutoMapping<Product> mapping)
        {
            // examples

            // Do not auto increment / generate id while inserting new items
            // mapping.Id(x => x.IdOfEntity).GeneratedBy.Assigned();

            // Ignore a property that should not be serialized to the database
            // mapping.IgnoreProperty(p => p.MyCalculatedProperty);

            // Change the length of a property
            // mapping.Map(p => p.MyProperty).Length(512);

            // HasMany, lazy load
            // mapping.IgnoreProperty(p => p.Parts);
            // mapping.HasMany(p => p._parts).Inverse()
            //   .Cascade.AllDeleteOrphan().LazyLoad();

            // Reference
            // mapping.References<Order>(p => p.Order)
            //   .Cascade.All().LazyLoad();

            // map picture
            // ImageData is of type byte[]
            // mapping.Map(x => x.ImageData).LazyLoad().Length(524288);
        }
    }


}
