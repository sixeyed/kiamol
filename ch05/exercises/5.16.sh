cd /kiamol/ch05

kubectl apply -f storageClass/postgres-persistentVolumeClaim-storageClass.yaml
kubectl apply -f storageClass/todo-db.yaml
kubectl get pvc
kubectl get pv
kubectl get pods -l app=todo-db
