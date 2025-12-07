$scriptpath = $MyInvocation.MyCommand.Path
$dir = Split-Path $scriptpath
Push-Location "$dir"

cd .\src\YABHT-B51\YABHTService
# May require to run as administrator
dotnet publish -c Release --runtime win-x64 --framework net10.0 -o "C:\Program Files\B51\YABHT-B51\"