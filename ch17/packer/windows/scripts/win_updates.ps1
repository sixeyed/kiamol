Get-PackageProvider -name nuget -force
Install-Module PSWindowsUpdate -confirm:$false -force
Get-WindowsUpdate -Install -acceptall -IgnoreReboot