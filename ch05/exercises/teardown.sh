cd /kiamol/ch05

kubectl delete -f pi/v1 -f sleep/ -f storageClass/ -f todo-list/web -f todo-list/postgres -f todo-list/
kubectl delete sc kiamol
