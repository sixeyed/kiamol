# Ch09 lab

Run v1 of the app:

```
kubectl apply -f lab/v1/
```

Get the URL and browse:

```
kubectl get svc vweb -o jsonpath='http://{.status.loadBalancer.ingress[0].*}:8090'
```

> v1 is the blue deployment

## Sample Solution

My [v2 Deployment](solution/vweb-v2.yaml) runs four Pods from the v2 image. It's a new Deployment object, not an update to the existing v1 Deployment.

My [Service update](solution/vweb-service-v2.yaml) changes the label selector in the existing service to point to the Pods in the v2 Deployment.

```
kubectl apply -f lab/solution/
```

> v2 is the green deployment

You can flip between blue and green by updating just the Service:

```
# for v1
kubectl apply -f lab/v1/vweb-service-v1.yaml
```

```
# for v2
kubectl apply -f lab/solution/vweb-service-v2.yaml
```

> Your browser may cache the response, so be sure to do a full refresh (usually Ctrl-F5 on Windows and Cmd+Shift+R on Mac)

## Teardown

Delete the lab resources by their labels:

```
kubectl delete all -l kiamol=ch09-lab
```
