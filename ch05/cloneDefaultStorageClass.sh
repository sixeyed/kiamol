#!/bin/sh

# Kubernetes uses an annotation to mark the default storage class rather than a label,
# so that needs a JSONPath query - plus there are different annotation keys for 
# different Kubernetes versions. This finds the configured default and writes the spec as JSON:
kubectl get sc $(kubectl get storageclass -o jsonpath="{range .items[?(@.metadata.annotations.storageclass\.kubernetes\.io/is-default-class == 'true')]}{.metadata.name}") $(kubectl get storageclass -o jsonpath="{range.items[?(@.metadata.annotations.storageclass\.beta\.kubernetes\.io/is-default-class == 'true')]}{.metadata.name}") -o json > defaultStorageClass.json

# create a pod which has some tools and a script we can use to clone the sc definition:
kubectl apply -f  storageClass/clone-storageClass-script.yaml 

# wait for the pod:
kubectl wait --for=condition=Ready pod/clone-sc

# copy the default sc spec into the cloning pod:
kubectl cp defaultStorageClass.json clone-sc:/defaultStorageClass.json

# run the clone script, which generates a custom sc defintion:
kubectl exec clone-sc -- /scripts/duplicate-default-storage-class.sh > kiamolStorageClass.json

# apply the new sc:
kubectl apply -f kiamolStorageClass.json

# and remove the cloning pod:
kubectl delete -f  storageClass/clone-storageClass-script.yaml