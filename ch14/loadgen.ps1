
Remove-Item Alias:curl -ErrorAction Ignore

$url = $(kubectl get svc todo-proxy -o jsonpath='http://{.status.loadBalancer.ingress[0].*}:8015' -n kiamol-ch14-test)

for($i = 0; $i -lt 500; $i++) { 
    curl -s "$url/List"  | Out-Null
}
