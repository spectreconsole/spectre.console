##########################################################
# Script that generates known colors and lookup tables.
##########################################################

$Output = Join-Path $PSScriptRoot "Temp"
$Source = Join-Path $PSScriptRoot "/../../src/Spectre.Console"

if(!(Test-Path $Output -PathType Container)) {
    New-Item -ItemType Directory -Path $Output | Out-Null
}

# Generate the files
Push-Location Generator
&dotnet run -- colors "$Output"
if(!$?) { 
    Pop-Location
    Throw "An error occured when generating code."
}
Pop-Location

# Copy the files to the correct location
Copy-Item  (Join-Path "$Output" "Color.Generated.cs") -Destination "$Source/Color.Generated.cs"
Copy-Item  (Join-Path "$Output" "ColorPalette.Generated.cs") -Destination "$Source/Internal/Colors/ColorPalette.Generated.cs"
Copy-Item  (Join-Path "$Output" "ColorTable.Generated.cs") -Destination "$Source/Internal/Colors/ColorTable.Generated.cs"