cd /kiamol/ch04 

kubectl apply -f todo-list/todo-web-dev-no-logging.yaml
kubectl exec deploy/todo-web -- sh -c 'ls /app/config'
kubectl logs -l app=todo-web
kubectl get pods -l app=todo-web

