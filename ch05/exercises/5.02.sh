cd /kiamol/ch05

kubectl apply -f sleep/sleep-with-emptyDir.yaml
kubectl exec deploy/sleep -- ls /data
kubectl exec deploy/sleep -- sh -c 'echo ch05 > /data/file.txt; ls /data'
kubectl get pod -l app=sleep -o jsonpath='{.items[0].status.containerStatuses[0].containerID}'
kubectl exec deploy/sleep -- killall5
kubectl get pod -l app=sleep -o jsonpath='{.items[0].status.containerStatuses[0].containerID}'
kubectl exec deploy/sleep -- cat /data/file.txt
