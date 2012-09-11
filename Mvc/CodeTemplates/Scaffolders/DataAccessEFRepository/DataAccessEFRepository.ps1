[T4Scaffolding.Scaffolder(Description = "Creates a repository in the DataAccess.EF project for Entity Framework")][CmdletBinding()]
param(
	[parameter(Position = 0, Mandatory = $true, ValueFromPipelineByPropertyName = $true)][string]$ModelType,
    [string]$DbContextType,
	[string]$Area,
    [string]$Project,
    [string]$CodeLanguage,
	[switch]$NoChildItems = $false,
	[string[]]$TemplateFolders,
	[switch]$Force = $false,
	[string]$OutputProjectName
)

# Ensure you've referenced System.Data.Entity
(Get-Project $OutputProjectName).Object.References.Add("System.Data.Entity") | Out-Null

$foundModelType = Get-ProjectType $ModelType -Project $Project -BlockUi
if (!$foundModelType) { return }

$primaryKey = Get-PrimaryKey $foundModelType.FullName -Project $Project -ErrorIfNotFound
if (!$primaryKey) { return }

if(!$DbContextType) { $DbContextType = [System.Text.RegularExpressions.Regex]::Replace((Get-Project $Project).Name, "[^a-zA-Z0-9]", "") + "Context" }

$outputPath = "Repositories\Generated\" + $foundModelType.Name + "Repository"  
$outputPathInterface = "RepositoryInterfaces\Generated\I" + $foundModelType.Name + "Repository"

if (!$NoChildItems) {
	$dbContextScaffolderResult = Scaffold DataAccessDbContext -ModelType $ModelType -DbContextType $DbContextType -Area $Area -Project $Project -OutputProjectName $OutputProjectName -CodeLanguage $CodeLanguage -BlockUi
	$foundDbContextType = $dbContextScaffolderResult.DbContextType
	if (!$foundDbContextType) { return }
}
if (!$foundDbContextType) { $foundDbContextType = Get-ProjectType $DbContextType -Project $Project }
if (!$foundDbContextType) { return }

$modelTypePluralized = Get-PluralizedWord $foundModelType.Name
$defaultNamespace = (Get-Project $OutputProjectName).Properties.Item("DefaultNamespace").Value
$repositoryNamespace = [T4Scaffolding.Namespaces]::Normalize($defaultNamespace + "." + [System.IO.Path]::GetDirectoryName($outputPath).Replace([System.IO.Path]::DirectorySeparatorChar, ".").Replace(".Generated", ""))
$modelTypeNamespace = [T4Scaffolding.Namespaces]::GetNamespace($foundModelType.FullName)
$repositoryInterfaceNamespace = [T4Scaffolding.Namespaces]::Normalize($defaultNamespace + "." + [System.IO.Path]::GetDirectoryName($outputPathInterface).Replace([System.IO.Path]::DirectorySeparatorChar, ".").Replace(".Generated", ""))

# do we have a base repository?
$baseRepositoryPath = "Repository"
$baseRepositoryPath = Join-Path Repositories $baseRepositoryPath

Write-Host "Ensuring that a base repository is present in $OutputProjectName"
$baseRepositoryPathWithExt = $baseRepositoryPath + ".cs"
if (-not (Get-ProjectItem $baseRepositoryPathWithExt -Project $OutputProjectName)) {
	Write-Host "Scaffolding base repository..."

	Add-ProjectItemViaTemplate $baseRepositoryPath -Template RepositoryBase -Model @{
			RepositoryNamespace = $repositoryNamespace; 	
			DbContextType = [MarshalByRefObject]$foundDbContextType;
		} -SuccessMessage "Added repository '{0}'" -TemplateFolders $TemplateFolders -Project $OutputProjectName -CodeLanguage $CodeLanguage -Force:$Force
}



# generate the repository

Add-ProjectItemViaTemplate $outputPath -Template Repository -Model @{
	ModelType = [MarshalByRefObject]$foundModelType; 
	PrimaryKey = [string]$primaryKey; 
	DefaultNamespace = $defaultNamespace; 
	RepositoryNamespace = $repositoryNamespace; 
	RepositoryInterfaceNamespace = $repositoryInterfaceNamespace;
	ModelTypeNamespace = $modelTypeNamespace; 
	ModelTypePluralized = [string]$modelTypePluralized; 
	DbContextNamespace = $foundDbContextType.Namespace.FullName;
	DbContextType = [MarshalByRefObject]$foundDbContextType;
} -SuccessMessage "Added repository '{0}'" -TemplateFolders $TemplateFolders -Project $OutputProjectName -CodeLanguage $CodeLanguage -Force:$Force

# generate the interface

Add-ProjectItemViaTemplate $outputPathInterface -Template RepositoryInterface -Model @{
	ModelType = [MarshalByRefObject]$foundModelType; 
	PrimaryKey = [string]$primaryKey; 
	DefaultNamespace = $defaultNamespace; 
	RepositoryInterfaceNamespace = $repositoryInterfaceNamespace; 
	ModelTypeNamespace = $modelTypeNamespace; 
	ModelTypePluralized = [string]$modelTypePluralized; 
	DbContextNamespace = $foundDbContextType.Namespace.FullName;
	DbContextType = [MarshalByRefObject]$foundDbContextType;
} -SuccessMessage "Added repository interface 'I{0}'" -TemplateFolders $TemplateFolders -Project $OutputProjectName -CodeLanguage $CodeLanguage -Force:$Force