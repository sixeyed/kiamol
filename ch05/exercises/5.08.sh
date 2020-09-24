cd /kiamol/ch05

kubectl label node $(kubectl get nodes -o jsonpath='{.items[0].metadata.name}') kiamol=ch05
kubectl get nodes -l kiamol=ch05
kubectl apply -f todo-list/persistentVolume.yaml
kubectl get pv
