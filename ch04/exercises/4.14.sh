cd /kiamol/ch04 

kubectl apply -f todo-list/todo-db-test.yaml
kubectl logs -l app=todo-db --tail 1
kubectl exec deploy/todo-db -- sh -c 'ls -l $(readlink -f /secrets/postgres_password)'
