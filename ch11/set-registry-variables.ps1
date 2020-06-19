$registry = Read-Host -Prompt 'Registry server (blank for Docker Hub)'
if ($registry.Length -eq 0) {
    $REGISTRY_SERVER='https://index.docker.io/v1/'
}
else {
    $REGISTRY_SERVER=$registry
}

$REGISTRY_USER = Read-Host -Prompt 'Username'

$password = Read-Host -Prompt 'Password'-AsSecureString
$REGISTRY_PASSWORD = [System.Net.NetworkCredential]::new("", $password).Password