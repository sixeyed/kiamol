# Ch05 lab

Run the app:

```
kubectl apply -f lab/todo-list
```

Check the pods - the proxy fails. Logs tell you why:

```
kubectl get pods

kubectl logs -l app=todo-proxy-lab
```

The proxy pod needs the cache directory to exist before it starts. So there are two volumes to provide:

- in the proxy a volume to mount to `/data/nginx/cache` (from the logs)
- in the web app a mount to `/data` (from the env settings in the pod spec).

## Sample solution

I'm using PVCs with no storage class defined, so they will use the default provisioner. These YAML files contain the PVC spec and updated deployment specs:

- [proxy.yaml](solution/proxy.yaml)
- [web.yaml](solution/web.yaml)

Deploy the updates and check volumes:

```
kubectl apply -f lab/solution/

kubectl get pvc

kubectl get pv
```

Find the URL for the proxy:

```
kubectl get svc todo-proxy-lab -o jsonpath='http://{.status.loadBalancer.ingress[0].*}:8082'
```

Browse, add some items, delete the pods:

```
kubectl delete pod -l app=todo-proxy-lab

kubectl delete pod -l app=todo-web-lab
```

Refresh your browser, and you should see your original data.