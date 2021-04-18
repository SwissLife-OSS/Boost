Remove-Item ./.nuget -Force -Recurse -ErrorAction SilentlyContinue
dotnet pack .\src\Tool\ -o ./.nuget
