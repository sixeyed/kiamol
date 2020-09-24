cd /kiamol/ch05

kubectl apply -f todo-list/postgres/
sleep 30
kubectl logs -l app=todo-db --tail 1
kubectl exec deploy/sleep -- sh -c 'ls -l /node-root/volumes/pv01 | grep wal'
