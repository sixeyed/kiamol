# Ch16 lab

## Setup

Deploy OPA Gatekeeper:

```
kubectl apply -f lab/gatekeeper.yaml
```

And the constraint template:

```
kubectl apply -f lab/restrictedPaths-template.yaml
```

## Sample Solution

The [constraint template](./restrictedPaths-template.yaml) uses a `paths` parameter to list restricted paths.

My [constraint](./solution/restrictedPaths-constraint.yaml) specifies paths and a label selector.

Deploy the constraint:

```
kubectl apply -f lab/solution/restrictedPaths-constraint.yaml
```

Try to deploy an app which uses restricted paths:

```
kubectl apply -f lab/sleep.yaml

kubectl get all -l app=sleep

kubectl describe rs -l app=sleep
```

> You should see the ReplicaSet has zero Pods, and the detail shows the error message from the constraint

You can fix it with an [updated sleep spec](./solution/sleep.yaml):

```
kubectl apply -f lab/solution/sleep.yaml

kubectl get all -l app=sleep

kubectl describe rs -l app=sleep
```

## Teardown

Delete all the resources:

```
kubectl delete -f lab/solution/sleep.yaml

kubectl delete RestrictedPaths,ConstraintTemplates --all

kubectl delete -f lab/gatekeeper.yaml
```