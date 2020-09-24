cd /kiamol/ch04 

kubectl logs -l app=todo-web
kubectl apply -f todo-list/configMaps/todo-web-config-dev-with-logging.yaml
sleep 120
kubectl exec deploy/todo-web -- sh -c 'ls -l /app/config/*.json'
kubectl logs -l app=todo-web
