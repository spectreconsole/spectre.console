#!/usr/local/bin/pwsh

##########################################################
# Script that generates progress spinners.
##########################################################

$Output = Join-Path $PSScriptRoot "Temp"
$Generator = Join-Path $PSScriptRoot "/../../src/Generator"
$Source = Join-Path $PSScriptRoot "/../../src/Spectre.Console"

if(!(Test-Path $Output -PathType Container)) {
    New-Item -ItemType Directory -Path $Output | Out-Null
}

# Generate the files
Push-Location $Generator
&dotnet run -- spinners "$Output"
if(!$?) {
    Pop-Location
    Throw "An error occured when generating code."
}
Pop-Location

# Copy the files to the correct location
Copy-Item  (Join-Path "$Output" "Spinner.Generated.cs") -Destination "$Source/Live/Progress/Spinner.Generated.cs"
