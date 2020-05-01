#!/bin/sh

# ARGH!
kubectl get sc $(kubectl get storageclass -o jsonpath="{range .items[?(@.metadata.annotations.storageclass\.kubernetes\.io/is-default-class == 'true')]}{.metadata.name}") $(kubectl get storageclass -o jsonpath="{range.items[?(@.metadata.annotations.storageclass\.beta\.kubernetes\.io/is-default-class == 'true')]}{.metadata.name}") -o json > defaultStorageClass.json

kubectl apply -f  storageClass/clone-storageClass-script.yaml 

kubectl wait --for=condition=Ready pod/clone-sc

kubectl cp defaultStorageClass.json clone-sc:/defaultStorageClass.json

kubectl exec clone-sc /scripts/duplicate-default-storage-class.sh > kiamolStorageClass.json

kubectl apply -f kiamolStorageClass.json

kubectl delete -f  storageClass/clone-storageClass-script.yaml