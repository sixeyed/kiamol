cd /kiamol/ch04 

kubectl apply -f sleep/sleep-with-secret.yaml
kubectl exec deploy/sleep -- printenv KIAMOL_SECRET
