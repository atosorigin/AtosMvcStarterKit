using System;
using System.Linq;
using Customer.Project.Domain.Entities;
using FluentNHibernate.Automapping;
using Customer.Project.Domain.ValueObjects;


namespace Customer.Project.DataAccess.NHibernateMappings
{
    /// <summary>
    ///
    /// </summary>
    public class CustomAutoMappingConfiguration : DefaultAutomappingConfiguration
    {
        /// <summary>
        /// ShouldMap method allows to specify a filter which instructs automapper
        /// for what all classes the automappings should be generated.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public override bool ShouldMap(Type type)
        {
            return type != null && type.IsClass && (type.GetInterface("IEntityId") != null
                                                    || IsValueObject(type));
        }

        /// <summary>
        /// Instruct the auto mapper how to identify components. This method will be called for each type 
        /// that has already been accepted by ShouldMap(Type), and 
        /// whichever types you return true for will be mapped as components.
        /// 
        /// The default column names for the component's properties are a combination of the name of 
        /// the property in the entity and the name of the property in the component type. 
        /// For example, if the Person class had a HomeAddress property, it would be mapped to the columns 
        /// HomeAddressStreet, HomeAddressPostCode, etc. 
        /// For more info refer http://wiki.fluentnhibernate.org/Auto_mapping
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public override bool IsComponent(System.Type type)
        {
            return IsValueObject(type);
        }

        /// <summary>
        /// Instructs automapper, which property of the business entity should be
        /// considered as Id field.
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public override bool IsId(FluentNHibernate.Member member)
        {
            return member != null && member.IsProperty && member.Name == "IdOfEntity";
        }

        private static bool IsValueObject(Type type)
        {
            var attr = type.GetCustomAttributes(false);
            return attr.OfType<ValueObjectAttribute>().Any();
        }
    }
}
