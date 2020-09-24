cd /kiamol/ch04 

kubectl apply -f todo-list/todo-web.yaml
kubectl wait --for=condition=Ready pod -l app=todo-web
kubectl get svc todo-web -o jsonpath='http://{.status.loadBalancer.ingress[0].*}:8080'
kubectl logs -l app=todo-web

