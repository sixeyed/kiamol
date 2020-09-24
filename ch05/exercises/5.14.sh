cd /kiamol/ch05

kubectl apply -f todo-list/postgres-persistentVolumeClaim-dynamic.yaml
kubectl get pvc
kubectl get pv
kubectl delete pvc postgres-pvc-dynamic
kubectl get pv
