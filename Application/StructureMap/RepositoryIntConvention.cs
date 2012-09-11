using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace Customer.Project.Application.StructureMap
{
    public class RepositoryIntConvention : IRegistrationConvention
{
    public void Process(Type type, Registry registry)
    {
        // only interested in non abstract concrete types that have a matching named interface if the 
        // name is surrounded with "I" and "Int" 
        if (type != null && (type.IsAbstract || !type.IsClass))
            return;

        string interfaceName = string.Format(CultureInfo.InvariantCulture, "I{0}Int", type.Name);
        // test for generic
        int tilde = interfaceName.IndexOf("`1", StringComparison.OrdinalIgnoreCase);
        if (-1 != tilde)
        {
            // move the `1 to the end of the string
            interfaceName = interfaceName.Remove(tilde, 2);
            interfaceName += "`1";

            // also test without Int
            int intIndex = interfaceName.IndexOf("Int`1", StringComparison.OrdinalIgnoreCase);
            ProcessInterfaceName(type, registry, interfaceName.Remove(intIndex, 3));
        }
        ProcessInterfaceName(type, registry, interfaceName);
    }

    private static void ProcessInterfaceName(Type type, Registry registry, string interfaceName)
    {
        //Trace.WriteLine(string.Format("RepositoryIntConvention: testing type: {0}, formatted: {1}"
        //        , type.Name, interfaceName));

        // Get interface and register (can use AddType overload method to create named types
        Type interfaceType = type.GetInterface(interfaceName);

        if (null != interfaceType)
        {
            Trace.WriteLine("RepositoryIntConvention: adding type: " + type.Name);
            registry.AddType(interfaceType, type);
        }
    }
}
}