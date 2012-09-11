using System;
using System.Diagnostics;
using AtosOrigin.NetLibrary.Components.Core;
using Customer.Project.Application.StructureMap;
using Customer.Project.DataAccess.RepositoryInterfaces;
using Customer.Project.Utilities;
using StructureMap;

namespace Customer.Project.Application
{
    /// <summary>
    /// The AppContext class offers the AppLogger service and all Repositories through its public properties.
    /// The properties refer to Repository Interfaces and can be used to Get and Find Business Entities. 
    /// The interfaces of the services must be registered in Unity in either web.config or code.
    /// </summary>
    public static class ServiceLocator
    {
        public static IContainer Container { get { return ObjectFactory.Container; } }

        public static void InitializeContainer(Action<IInitializationExpression> initializationExpressions)
        {
            ObjectFactory.Initialize(container =>
            {
                container.Scan(cfg =>
                {   
                    // scan the following assemblies for interfaces and interface implementations. 
                    // If exactly one class is found that implements a certain interface, the 
                    // class/interface combination is added to the StructureMap IoC container.
                    cfg.TheCallingAssembly();
#warning validate assembly name of DataAccess for StructureMap configuration
                    cfg.Assembly("Customer.Project.DataAccess");
                    cfg.Assembly("Customer.Project.DataAccessEF");
                    cfg.AssemblyContainingType(typeof(IBaseRepository<>));
                    cfg.AssemblyContainingType<RepositoryIntConvention>();
                    cfg.AssemblyContainingType<IFormatLogger>();
                    cfg.WithDefaultConventions();
                    cfg.Convention<RepositoryIntConvention>();
                });

                if (initializationExpressions != null)
                    initializationExpressions(container);

                container.SetAllProperties(x => x.OfType<IFormatLogger>());
            });

            // display all registered interfaces and implementation mappings
            Trace.WriteLine(ObjectFactory.WhatDoIHave());
        }

        /// <summary>
        /// Retrieves the specified type from the StructureMap IOC container
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Get<T>() where T : class
        {
            return ObjectFactory.GetInstance<T>();
        }

        /// <summary>
        /// Retrieves the specified type from the StructureMap IOC container. Throws an exception if it is null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetExisting<T>() where T : class
        {
            var result = ObjectFactory.GetInstance<T>();
            Check.RequireNotNull<InvalidOperationException>(result, 
                                 "Inversion of Control container cannot resolve an instance of type "
                                 + typeof (T).ToString());
            return result;
        }
    }
}
