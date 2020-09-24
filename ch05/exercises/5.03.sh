cd /kiamol/ch05

kubectl apply -f pi/v1/ 
kubectl wait --for=condition=Ready pod -l app=pi-web
kubectl get svc pi-proxy -o jsonpath='http://{.status.loadBalancer.ingress[0].*}:8080/?dp=30000'
kubectl exec deploy/pi-proxy -- ls -l /data/nginx/cache
