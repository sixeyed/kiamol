cd /kiamol/ch05

kubectl apply -f sleep/sleep-with-hostPath.yaml
kubectl exec deploy/sleep -- ls -l /var/log
kubectl exec deploy/sleep -- ls -l /node-root/var/log
kubectl exec deploy/sleep -- whoami
