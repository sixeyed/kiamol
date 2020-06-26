# Ch10 lab

Run the app using plain manifests (from this `lab` folder):

```
kubectl apply -f ./todo-list/
```

Get the URL and browse:

```
kubectl get svc todo-web -o jsonpath='http://{.status.loadBalancer.ingress[0].*}:8080'
```


## Sample Solution

My Helm chart templates the Kubernetes manifests for all the resources:

- [templates/todo-web-configMap.yaml](./ch10-lab-solution/templates/todo-web-configMap.yaml)
- [templates/todo-web-deployment.yaml](./ch10-lab-solution/templates/todo-web-deployment.yaml)
- [templates/todo-web-service.yaml](./ch10-lab-solution/templates/todo-web-service.yaml)

The [default values](./ch10-lab-solution/values.yaml) specify the Test environment and a Service port of 8080.

Validate the chart:

```
helm lint ./ch10-lab-solution
```

Install the test setup:

```
helm install lab-test ./ch10-lab-solution
```

> You can browse on port 8080, and the /config path returns a 404

Install the dev setup:

```
helm install -f dev-values.yaml lab-dev ./ch10-lab-solution
```

> You can browse on port 8088, and the /config path is working

## Teardown

Uninstall the Helm releases:

```
helm uninstall lab-test lab-dev
```

Delete other lab resources by their labels:

```
kubectl delete all -l kiamol=ch10-lab
```