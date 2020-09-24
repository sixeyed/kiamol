cd /kiamol/ch04 

kubectl create secret generic sleep-secret-literal --from-literal=secret=shh...
kubectl describe secret sleep-secret-literal
kubectl get secret sleep-secret-literal -o jsonpath='{.data.secret}'
kubectl get secret sleep-secret-literal -o jsonpath='{.data.secret}' | base64 -d
