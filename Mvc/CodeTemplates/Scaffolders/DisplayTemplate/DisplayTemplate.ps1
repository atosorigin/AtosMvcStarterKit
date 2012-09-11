[T4Scaffolding.ViewScaffolder("Razor", Description = "Adds an ASP.NET MVC view using the Razor view engine", IsRazorType = $true, LayoutPageFilter = "*.cshtml,*.vbhtml|*.cshtml,*.vbhtml")][CmdletBinding()]
param(        
	[string]$ModelType,
	[string]$Template = "DisplayTemplateForModel",
	[string]$Area,
	[switch]$ReferenceScriptLibraries = $false,
    [string]$Project,
	[string]$CodeLanguage,
	[string[]]$TemplateFolders,
	[switch]$Force = $false
)

# Ensure we have a model type if specified
if ($ModelType) {
	$foundModelType = Get-ProjectType $ModelType -Project $Project
	if (!$foundModelType) { return }
	$primaryKeyName = [string](Get-PrimaryKey $foundModelType.FullName -Project $Project)
	$viewDataTypePluralName = [string](Get-PluralizedWord $foundModelType.Name) 
}

# Decide where to put the output
$outputFolderName = Join-Path Views "Shared\DisplayTemplates"

if ($Area) {
	# We don't create areas here, so just ensure that if you specify one, it already exists
	$areaPath = Join-Path Areas $Area
	if (-not (Get-ProjectItem $areaPath -Project $Project)) {
		Write-Error "Cannot find area '$Area'. Make sure it exists already."
		return
	}
	$outputFolderName = Join-Path $areaPath $outputFolderName
}

if ($foundModelType) { $relatedEntities = [Array](Get-RelatedEntities $foundModelType.FullName -Project $project) }
if (!$relatedEntities) { $relatedEntities = @() }

$modelTypeName = $foundModelType.Name + "ListItem"
$outputPath = Join-Path $outputFolderName $modelTypeName

# Render the T4 template, adding the output to the Visual Studio project
Add-ProjectItemViaTemplate $outputPath -Template $Template -Model @{
	ReferenceScriptLibraries = $ReferenceScriptLibraries.ToBool();
	PrimaryKeyName = $primaryKeyName;
	ViewDataType = [MarshalByRefObject]$foundModelType;
	ViewModelType = [MarshalByRefObject]$foundViewModelType; 
	ViewDataTypeName = $foundModelType.Name;
	RelatedEntities = $relatedEntities;
	ViewDataTypePluralName = $viewDataTypePluralName;
} -SuccessMessage "Added $modelTypeName DisplayTemplate at '{0}'" -TemplateFolders $TemplateFolders -Project $Project -CodeLanguage $CodeLanguage -Force:$Force
