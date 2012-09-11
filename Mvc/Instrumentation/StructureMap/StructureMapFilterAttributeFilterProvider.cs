using System.Web.Mvc;
using StructureMap;

public class StructureMapFilterAttributeFilterProvider : FilterAttributeFilterProvider
{
    private readonly IContainer _container;
    public StructureMapFilterAttributeFilterProvider(IContainer container)
    {
        _container = container;
    }

    protected override System.Collections.Generic.IEnumerable<FilterAttribute> GetControllerAttributes(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
    {
        var attributes = base.GetControllerAttributes(controllerContext, actionDescriptor);

        foreach (var attribute in attributes)
        {
            _container.BuildUp(attributes);
        }
        return attributes;
    }

    protected override System.Collections.Generic.IEnumerable<FilterAttribute> GetActionAttributes(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
    {
        var attributes = base.GetActionAttributes(controllerContext, actionDescriptor);

        foreach (var attribute in attributes)
        {
            _container.BuildUp(attribute);
        }
        return attributes;
    }

}