cd /kiamol/ch04 

kubectl apply -f todo-list/secrets/todo-db-secret-test.yaml
kubectl get secret todo-db-secret-test -o jsonpath='{.data.POSTGRES_PASSWORD}'
kubectl get secret todo-db-secret-test -o jsonpath='{.metadata.annotations}'
