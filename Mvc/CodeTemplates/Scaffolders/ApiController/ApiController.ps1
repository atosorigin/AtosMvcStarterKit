[T4Scaffolding.ControllerScaffolder("Controller with read/write action and views, using EF repositories of the ResourceAccessLayer", Description = "Adds an ASP.NET MVC controller with views and data access code", SupportsModelType = $true, SupportsDataContextType = $true, SupportsViewScaffolder = $true)][CmdletBinding()]
param(        
	[parameter(Mandatory = $true, ValueFromPipelineByPropertyName = $true)][string]$ControllerName,   
	[string]$ModelType,
    [string]$Project,
	[string]$DataAccessProject = "DataAccess.EF",
    [string]$CodeLanguage,
	[string]$DbContextType,
	[string]$Area,
	[string]$ViewScaffolder = "RazorViewEx",
	[alias("MasterPage")]$Layout,
 	[alias("ContentPlaceholderIDs")][string[]]$SectionNames,
	[alias("PrimaryContentPlaceholderID")][string]$PrimarySectionName,
	[switch]$ReferenceScriptLibraries = $false,
	[switch]$NoChildItems = $false,
	[string[]]$TemplateFolders,
	[switch]$Force = $false,
	[string]$ForceMode
)

# Interpret the "Force" and "ForceMode" options
$overwriteController = $Force -and ((!$ForceMode) -or ($ForceMode -eq "ControllerOnly"))
$overwriteFilesExceptController = $Force -and ((!$ForceMode) -or ($ForceMode -eq "PreserveController"))

# Ensure you've referenced System.Data.Entity
(Get-Project $Project).Object.References.Add("System.Data.Entity") | Out-Null

# If you haven't specified a model type, we'll guess from the controller name
if (!$ModelType) {
	if ($ControllerName.EndsWith("ApiController", [StringComparison]::OrdinalIgnoreCase)) {
		# If you've given "PeopleApiController" as the full controller name, we're looking for a model called People or Person
		$ModelType = [System.Text.RegularExpressions.Regex]::Replace($ControllerName, "ApiController$", "", [System.Text.RegularExpressions.RegexOptions]::IgnoreCase)
		$foundModelType = Get-ProjectType $ModelType -Project $Project -BlockUi -ErrorAction SilentlyContinue
		if (!$foundModelType) {
			$ModelType = [string](Get-SingularizedWord $ModelType)
			$foundModelType = Get-ProjectType $ModelType -Project $Project -BlockUi -ErrorAction SilentlyContinue
		}
	} else {
		# If you've given "people" as the controller name, we're looking for a model called People or Person, and the controller will be PeopleApiController
		$ModelType = $ControllerName
		$foundModelType = Get-ProjectType $ModelType -Project $Project -BlockUi -ErrorAction SilentlyContinue
		if (!$foundModelType) {
			$ModelType = [string](Get-SingularizedWord $ModelType)
			$foundModelType = Get-ProjectType $ModelType -Project $Project -BlockUi -ErrorAction SilentlyContinue
		}
		if ($foundModelType) {
			$ControllerName = [string](Get-PluralizedWord $foundModelType.Name) + "ApiController"
		}
	}
	if (!$foundModelType) { throw "Cannot find a model type corresponding to a controller called '$ControllerName'. Try supplying a -ModelType parameter value." }
} else {
	# If you have specified a model type
	$foundModelType = Get-ProjectType $ModelType -Project $Project -BlockUi
	if (!$foundModelType) { return }
	if (!$ControllerName.EndsWith("ApiController", [StringComparison]::OrdinalIgnoreCase)) {
		$ControllerName = $ControllerName + "ApiController"
	}
}

Write-Host "Scaffolding Data Layer..."

if(!$DbContextType) { $DbContextType = [System.Text.RegularExpressions.Regex]::Replace((Get-Project $Project).Name, "[^a-zA-Z0-9]", "") + "Context" }
if (!$NoChildItems) {
		Scaffold DataAccessEFRepository -ModelType $foundModelType.FullName -DbContextType $DbContextType -Area $Area -Project $Project -OutputProjectName $DataAccessProject -CodeLanguage $CodeLanguage -Force:$overwriteFilesExceptController -BlockUi
}
if (!$foundDbContextType) { $foundDbContextType = Get-ProjectType $DbContextType -Project $DataAccessProject }
if (!$foundDbContextType) { return }

$primaryKey = Get-PrimaryKey $foundModelType.FullName -Project $Project -ErrorIfNotFound
if (!$primaryKey) { return }

$outputPath = Join-Path Controllers Api
$outputPath = Join-Path $outputPath $ControllerName
# We don't create areas here, so just ensure that if you specify one, it already exists
if ($Area) {
	$areaPath = Join-Path Areas $Area
	if (-not (Get-ProjectItem $areaPath -Project $Project)) {
		Write-Error "Cannot find area '$Area'. Make sure it exists already."
		return
	}
	$outputPath = Join-Path $areaPath $outputPath
}

# Prepare all the parameter values to pass to the template, then invoke the template with those values
$repositoryName = $foundModelType.Name + "Repository"
$defaultNamespace = (Get-Project $Project).Properties.Item("DefaultNamespace").Value
$defaultDataAccessNamespace = (Get-Project $DataAccessProject).Properties.Item("DefaultNamespace").Value
$modelTypeNamespace = [T4Scaffolding.Namespaces]::GetNamespace($foundModelType.FullName)
$controllerNamespace = [T4Scaffolding.Namespaces]::Normalize($defaultNamespace + "." + [System.IO.Path]::GetDirectoryName($outputPath).Replace([System.IO.Path]::DirectorySeparatorChar, "."))
$areaNamespace = if ($Area) { [T4Scaffolding.Namespaces]::Normalize($defaultNamespace + ".Areas.$Area") } else { $defaultNamespace }
$dbContextNamespace = $foundDbContextType.Namespace.FullName
$repositoriesNamespace = $defaultDataAccessNamespace + ".Repositories"
$repositoryInterfacesNamespace = $defaultDataAccessNamespace + ".RepositoryInterfaces"
$modelTypePluralized = Get-PluralizedWord $foundModelType.Name
$relatedEntities = [Array](Get-RelatedEntities $foundModelType.FullName -Project $project)
if (!$relatedEntities) { $relatedEntities = @() }
$templateName = "ApiController"

# do we have a ControllerBase?
$baseControllerPath = Join-Path Controllers Api
$baseControllerPath = Join-Path $baseControllerPath ApiControllerBase.cs
if ($Area) {
	$areaPath = Join-Path Areas $Area
	$baseControllerPath = Join-Path $areaPath $baseControllerPath
}
Write-Host "Ensuring that a ControllerBase is present in $Project"
if (-not (Get-ProjectItem $baseControllerPath -Project $Project)) {
	
	Write-Host "Scaffolding ControllerBase..."

	Add-ProjectItemViaTemplate $baseControllerPath -Template ApiControllerBase -Model @{
		ControllerNamespace = $controllerNamespace; 
	} -SuccessMessage "Added ControllerBase {0}" -TemplateFolders $TemplateFolders -Project $Project -Force:$overwriteController
}

Write-Host "Scaffolding $ControllerName..."

Add-ProjectItemViaTemplate $outputPath -Template $templateName -Model @{
	ControllerName = $ControllerName;
	ModelType = [MarshalByRefObject]$foundModelType; 
	PrimaryKey = [string]$primaryKey; 
	DefaultNamespace = $defaultNamespace; 
	AreaNamespace = $areaNamespace; 
	DbContextNamespace = $dbContextNamespace;
	RepositoriesNamespace = $repositoriesNamespace;
	RepositoryInterfacesNamespace = $repositoryInterfacesNamespace;
	ModelTypeNamespace = $modelTypeNamespace; 
	ControllerNamespace = $controllerNamespace; 
	DbContextType = [MarshalByRefObject]$foundDbContextType;
	Repository = $repositoryName; 
	ModelTypePluralized = [string]$modelTypePluralized; 
	RelatedEntities = $relatedEntities;
} -SuccessMessage "Added controller {0}" -TemplateFolders $TemplateFolders -Project $Project -CodeLanguage $CodeLanguage -Force:$overwriteController