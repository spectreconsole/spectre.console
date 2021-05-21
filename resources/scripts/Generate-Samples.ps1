# first arg is either the name of a single sample you want to run or leave
# blank if you want to run them all. the samples aren't going to run at the same
# speed each time so if you run all of them you'll have everything as a change
# for your commit so use this sparingly.

# Generate the files
Push-Location Generator
dotnet run -- samples -o "../../../docs/input/assets/casts" $args[0]
if(!$?) {
    Pop-Location
    Throw "An error occured when generating code."
}
Pop-Location