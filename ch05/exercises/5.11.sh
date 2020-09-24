cd /kiamol/ch05

kubectl apply -f sleep/sleep-with-hostPath.yaml
kubectl wait --for=condition=Ready pod -l app=sleep
kubectl exec deploy/sleep -- mkdir -p /node-root/volumes/pv01
