Atos MVC Starter Kit
=================

The Atos MVC Starter Kit is a project template for turbo boosting the creation of new Microsoft.NET applications. 

The Starter Kit is designed for creating new Web applications and SOA service applications. It offers a broad set 
of reusable functionality. This lowers the chance that for each new project wheels have to be reinvented over and 
over again. In addition, the Start Kit features code generation of Repositories, Controllers and Views to optimize
productivity.

The Atos MVC Starter Kit is an important component of the Samoa software factory, the software factory of Atos.
See the Samoa wiki for details of the other components of Samoa: https://wiki.myatos.net/display/SAMOA/Samoa

Solution
- Pre defined solution with layers for UI, Business, Resource access
- Projects and project dependencies are preconfigured
- All application projects use one global assembly configuration file 

Web development
- Starter Kit targets ASP.NET MVC 4
- Supports latest versions of jQuery, jQuery-ui, knockout and other libraries as of September 1 1021
- Drop down menu with support for xml sitemap and role authorisations
- Initial set of meta tags, web configuration, configuration transformations, bundles, auto build of mvc views
- Editor Templates for HTML5 types
- ActionResultMessage for displaying simple messages to the user
- Logging of all unhandled exceptions for all web requests and Web Api requests
- PreWarmCache support
- Web setup project is included

Instrumentation
- Formatted Logging facade
- Logging attributes for logging web requests and web api requests
- Error handling with Elmah
- Error reporting, extended error page
- Inversion of Control container initialization (StructureMap)
- Configuration of IoC container for resolving MVC and MVC Web API components
- Design by contract with the AtosComponents Check class
- RoleAuthorization attribute for authorization on actions based on role membership
- Preconfigured FxCop ruleset with Atos standards

Database
- Prepared for Entity Framework and NHibernate Data Access Layer

Code generation
- Generation of Entity Framework repositories for CRUD operations
- Generation of Controller with support for Index, Details, Delete, Create actions
- Generation of ViewModels
- Generation of List, Details, Delete, Create views
- List views support paging

Alexander van Trijffel
