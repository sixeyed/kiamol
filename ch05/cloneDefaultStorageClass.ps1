$PSDefaultParameterValues['Out-File:Encoding'] = 'utf8'
$cmd = Get-Content -Raw cloneDefaultStorageClass.sh
Invoke-Expression $cmd