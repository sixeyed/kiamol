cd /kiamol/ch04 

kubectl exec deploy/todo-web -- sh -c 'ls -l /app/app*.json'
kubectl exec deploy/todo-web -- sh -c 'ls -l /app/config/*.json'
kubectl exec deploy/todo-web -- sh -c 'echo ch04 >> /app/config/config.json'

