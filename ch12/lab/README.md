# Ch12 lab

## Setup

Deploy the namespaces & Services:

```
kubectl apply -f lab/namespaces.yaml -f lab/services.yaml
```

Find out how many cores your node has available:

```
kubectl get nodes -o jsonpath='{.items[].status.allocatable.cpu}'
```

> Quotas should allocate 50% for UAT, 25% for test and 25% for dev

## Sample Solution

My node has 8 CPU cores, so my [quotas.yaml](./solution/quotas.yaml) is set with 4 cores for UAT and 2 each for dev and test.

Set the quotas:

```
kubectl apply -f lab/solution/quotas.yaml
```

My Pi app is set in [web.yaml](./solution/web.yaml) to use 0.5 cores, so I can run at least four replicas in every environment.

Deploy to each namespace:

```
kubectl apply -f lab/solution/web.yaml -n pi-dev

kubectl apply -f lab/solution/web.yaml -n pi-test

kubectl apply -f lab/solution/web.yaml -n pi-uat
```

You should find all ReplicaSets are running at desired capacity:

```
kubectl get rs -l app=pi-web --all-namespaces
```

UAT has enough quota to run 8 replicas on my node:

```
kubectl scale deploy/pi-web --replicas 8 -n pi-uat
```

... except that it doesn't:

```
kubectl get rs -l app=pi-web -n pi-uat\

# returns 6 Pods running out of 8 desired
```

> You can't use 100% of the node's CPU because Kubernetes system components allocate CPU themselves.

## Teardown

Remove the namespaces and that removes everything:

```
kubectl delete ns -l kiamol=ch12-lab
```