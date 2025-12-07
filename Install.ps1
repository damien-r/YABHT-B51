$scriptpath = $MyInvocation.MyCommand.Path
$dir = Split-Path $scriptpath
Push-Location "$dir"

cd .\src\YABHT-B51\YABHTService
# May require to run as administrator
dotnet publish -c Release --runtime win-x64 --framework net10.0 -o "C:\Program Files\B51\YABHT-B51\"
sc.exe create YABHT-B51 binPath= "C:\Program Files\B51\YABHT-B51\YABHTService.exe" start= auto
sc.exe description YABHT-B51 https://github.com/damien-r/YABHT-B51
