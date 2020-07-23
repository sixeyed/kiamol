# pi @ 100K do is hard on CPU:
$url = $(kubectl get svc pi-web -o jsonpath='http://{.status.loadBalancer.ingress[0].*}:8031/?dp=100000')

# two calls is enough to trigger HPA:
Start-Process -NoNewWindow curl $url
Start-Process -NoNewWindow curl $url