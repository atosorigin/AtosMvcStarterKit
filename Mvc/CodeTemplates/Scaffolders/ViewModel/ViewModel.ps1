[T4Scaffolding.Scaffolder(Description = "Enter a description of ViewModel here")][CmdletBinding()]
param(        
	[parameter(Mandatory = $true, ValueFromPipelineByPropertyName = $true)][string]$ModelType,
	[string]$ViewModelPurpose,
	[string]$Area,
    [string]$Project,
	[string]$CodeLanguage,
	[string[]]$TemplateFolders,
	[switch]$Force = $false
)

# Ensure we can find the model type
$foundModelType = Get-ProjectType $ModelType -Project $Project
if (!$foundModelType) { return }

$viewModelName = $foundModelType.Name + $ViewModelPurpose + "ViewModel"

$outputPath = "Models\" + $viewModelName
$namespace = (Get-Project $Project).Properties.Item("DefaultNamespace").Value

# We don't create areas here, so just ensure that if you specify one, it already exists
if ($Area) {
	$areaPath = Join-Path Areas $Area
	if (-not (Get-ProjectItem $areaPath -Project $Project)) {
		Write-Error "Cannot find area '$Area'. Make sure it exists already."
		return
	}
	$outputPath = Join-Path $areaPath $outputPath

	$namespace = $namespace + ".Areas." + $Area
}

$namespace = $namespace + ".Models"
$modelTypeNamespace = [T4Scaffolding.Namespaces]::GetNamespace($foundModelType.FullName)
$relatedEntities = [Array](Get-RelatedEntities $foundModelType.FullName -Project $project)
if (!$relatedEntities) { $relatedEntities = @() }
$modelTypePluralized = Get-PluralizedWord $foundModelType.Name

$template = "ViewModelTemplate"
if ($ViewModelPurpose -eq "List")
{ $template = "List" } 

Add-ProjectItemViaTemplate $outputPath -Template $template `
	-Model @{	Namespace = $namespace; 
				ModelType = [MarshalByRefObject]$foundModelType; 
				ViewModelName = $viewModelName;
				ModelTypeNamespace = $modelTypeNamespace; 	 
				RelatedEntities = $relatedEntities;
				ModelTypePluralized = $modelTypePluralized;
	} -SuccessMessage "Added ViewModel output at {0}" -TemplateFolders $TemplateFolders -Project $Project -CodeLanguage $CodeLanguage -Force:$Force