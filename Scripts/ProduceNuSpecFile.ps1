param(
    [string] $Version,
    [string] $PackageId,
    [string] $MainProjectFolder
)

$nuSpecData = @"
<?xml version="1.0" encoding="UTF-8"?>
<package xmlns="http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd">
<metadata>
    <id>$PackageId</id>
    <version>$Version</version>
</metadata>
</package>
"@


$nuspecFilePath = "./$MainProjectFolder/Package.nuspec"
Write-Host "Writing nuspec file to: $nuspecFilePath with content: $nuSpecData"

$nuSpecData.ToString() > $nuspecFilePath
