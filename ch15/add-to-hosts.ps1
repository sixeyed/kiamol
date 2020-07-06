param (
	[string] $domain,
	[string] $ingress
)

$controller="$ingress-controller"
$ns="kiamol-$ingress"
$ip=$(kubectl get svc $controller -o jsonpath='{.status.loadBalancer.ingress[0].*}' -n $ns)
if ($ip -eq 'localhost') {
	$ip='127.0.0.1'
}

Add-Content -Value "$ip  $domain" -Path C:/windows/system32/drivers/etc/hosts