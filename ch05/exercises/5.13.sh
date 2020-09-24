cd /kiamol/ch05

kubectl apply -f todo-list/web/
kubectl wait --for=condition=Ready pod -l app=todo-web
kubectl get svc todo-web -o jsonpath='http://{.status.loadBalancer.ingress[0].*}:8081/new'

#

kubectl delete pod -l app=todo-db
kubectl exec deploy/sleep -- ls -l /node-root/volumes/pv01/pg_wal
