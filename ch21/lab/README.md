# Ch20 lab

## Setup

Deploy Knative with Contour:

```
kubectl apply -f lab/knative/
```

Deploy the to-do app:

```
kubectl apply -f lab/todo-list/
```

Get the ingress URL:

```
kubectl get svc -n contour-external envoy -o jsonpath='http://{.status.loadBalancer.ingress[0].*}:80'
```

> Map your `hosts` file to use the Contour IP for `todo.kiamol.local` and `api.todo.kiamol.local`


## Solution

I've used a [Knative Service custom resource](./solution/todo-api.yaml) which specifies the API container image.

```
kubectl apply -f lab/solution/todo-api.yaml
```

Check the Knative Service is running:

```
kubectl get ksvc -n todo
```

Invoke the function:

```
curl --data 'Write KIAMOL ch22' http://api.todo.kiamol.local/todos
```

Print the function logs - you should see the event being published:

```
kubectl -n todo logs -l serving.knative.dev/service=api -c user-container
```

Print the save handler logs - you should see the new item being saved:

```
kubectl -n todo logs -l component=save-handler
```

> Browse to http://todo.kiamol.local to see the new item