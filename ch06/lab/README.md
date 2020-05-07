# Ch06 lab

Run the app:

```
kubectl apply -f lab/numbers/
```

Get the URL to browse to:

```
kubectl get svc numbers-web -o jsonpath='http://{.status.loadBalancer.ingress[0].*}:8086'
```

> Browse and try to get a random number, the app fails.

## Sample Solution

Add the RNG label to a node:

```
kubectl label node $(kubectl get nodes -o jsonpath='{.items[0].metadata.name}') rng=hw
```

Deploy the new resources - a [Deployment](solution/web-deployment.yaml) for the web app and a [DaemonSet](solution/api-daemonset.yaml) for the API:

```
kubectl apply -f lab/solution/
```

> Refresh the browser and confirm you can get a random number.

Delete the old resources by their labels:

```
kubectl get all -l kiamol=ch06-lab

kubectl delete all -l kiamol=ch06-lab
```
