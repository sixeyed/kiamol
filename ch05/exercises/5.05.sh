cd /kiamol/ch05

kubectl apply -f pi/nginx-with-hostPath.yaml
kubectl exec deploy/pi-proxy -- ls -l /data/nginx/cache

#
curl http://localhost:8080/?dp=30000

kubectl delete pod -l app=pi-proxy
kubectl exec deploy/pi-proxy -- ls -l /data/nginx/cache
