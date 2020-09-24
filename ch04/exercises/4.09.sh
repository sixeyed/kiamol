cd /kiamol/ch04 

kubectl apply -f todo-list/todo-web-dev-broken.yaml
kubectl logs -l app=todo-web
kubectl get pods -l app=todo-web
