##########################################################
# Script that generates the emoji lookup table.
##########################################################

$Output = Join-Path $PSScriptRoot "Temp"
$Source = Join-Path $PSScriptRoot "/../../src/Spectre.Console"
$Docs = Join-Path $PSScriptRoot "/../../docs/src/Data"

if(!(Test-Path $Output -PathType Container)) {
    New-Item -ItemType Directory -Path $Output | Out-Null
}

# Generate the files
Push-Location Generator
&dotnet run -- emoji "$Output" --input $Output
if(!$?) {
    Pop-Location
    Throw "An error occured when generating code."
}
Pop-Location

# Copy the files to the correct location
Copy-Item  (Join-Path "$Output" "Emoji.Generated.cs") -Destination "$Source/Emoji.Generated.cs"
Copy-Item  (Join-Path "$Output" "emojis.json") -Destination "$Docs/emojis.json"