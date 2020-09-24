cd /kiamol/ch04 

kubectl apply -f todo-list/configMaps/todo-web-config-test.yaml
kubectl apply -f todo-list/secrets/todo-web-secret-test.yaml
kubectl apply -f todo-list/todo-web-test.yaml
kubectl exec deploy/todo-web-test -- cat /app/secrets/secrets.json
