cd /kiamol/ch05

kubectl apply -f sleep/sleep-with-hostPath-subPath.yaml
kubectl exec deploy/sleep -- sh -c 'ls /pod-logs | grep _pi-'
kubectl exec deploy/sleep -- sh -c 'ls /container-logs | grep nginx'
