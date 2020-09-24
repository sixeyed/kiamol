cd /kiamol/ch04 

kubectl apply -f sleep/sleep.yaml
kubectl wait --for=condition=Ready pod -l app=sleep
kubectl exec deploy/sleep -- printenv HOSTNAME KIAMOL_CHAPTER
