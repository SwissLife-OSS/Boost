Remove-Item  .\src\UI\boost-ui\dist\ -Recurse -Force -ErrorAction SilentlyContinue
Remove-Item .\src\Tool\src\Boost\UI -Recurse -ErrorAction SilentlyContinue
yarn --cwd .\src\UI\boost-ui build