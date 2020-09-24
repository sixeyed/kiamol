cd /kiamol/ch05

kubectl delete pod -l app=pi-proxy
kubectl exec deploy/pi-proxy -- ls -l /data/nginx/cache
