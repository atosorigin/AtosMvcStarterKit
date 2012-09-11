[T4Scaffolding.Scaffolder(Description = "Adds ASP.NET MVC views for Create/Read/Update/Delete/Index scenarios")][CmdletBinding()]
param(        
	[parameter(Mandatory = $true, ValueFromPipelineByPropertyName = $true, Position = 0)][string]$Controller,
	[string]$ModelType,
	[string]$Area,
	[alias("MasterPage")]$Layout = "",	# If not set, we'll use the default layout
 	[alias("ContentPlaceholderIDs")][string[]]$SectionNames,
	[alias("PrimaryContentPlaceholderID")][string]$PrimarySectionName,
	[switch]$ReferenceScriptLibraries = $false,
    [string]$Project,
	[string]$CodeLanguage,
	[string[]]$TemplateFolders,
	[string]$ViewScaffolder = "RazorViewEx",
	[string]$DisplayTemplateScaffolder = "DisplayTemplate",
	[switch]$Force = $false
)

# Ensure we can find the model type
$foundModelType = Get-ProjectType $ModelType -Project $Project
if (!$foundModelType) { return }

@("Create", "Edit", "_CreateOrEdit", "Index") | %{
	$viewModelNamePart = $_

	if ( ($viewModelNamePart -eq "Create") -or ($viewModelNamePart -eq "Edit") -or ($viewModelNamePart -eq "_CreateOrEdit"))
	{ $viewModelNamePart = "CreateEdit" }
	else
	{ $viewModelNamePart = "List" }

	$viewModelType = $foundModelType.Name + $viewModelNamePart + "ViewModel"
	Scaffold $ViewScaffolder -Controller $Controller -ViewName $_ -ModelType $ModelType -ViewModelType $viewModelType		-Template $_ -Area $Area -Layout $Layout -SectionNames $SectionNames -PrimarySectionName $PrimarySectionName -ReferenceScriptLibraries:$ReferenceScriptLibraries -Project $Project -CodeLanguage $CodeLanguage -OverrideTemplateFolders $TemplateFolders -Force:$Force -BlockUi
}

@("Delete", "Details", "DetailsPart") | %{
	Scaffold $ViewScaffolder -Controller $Controller -ViewName $_ -ModelType $ModelType										-Template $_ -Area $Area -Layout $Layout -SectionNames $SectionNames -PrimarySectionName $PrimarySectionName -ReferenceScriptLibraries:$ReferenceScriptLibraries -Project $Project -CodeLanguage $CodeLanguage -OverrideTemplateFolders $TemplateFolders -Force:$Force -BlockUi
}

Scaffold $DisplayTemplateScaffolder -ModelType $ModelType -Area $Area -ReferenceScriptLibraries:$ReferenceScriptLibraries -Project $Project -CodeLanguage $CodeLanguage -OverrideTemplateFolders $TemplateFolders -Force:$Force -BlockUi