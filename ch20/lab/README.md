# Ch20 lab

## Setup

Deploy the custom controller:

```
kubectl apply -f lab/timecheck-controller/
```

Try to create custom resource:

```
kubectl apply -f lab/timecheck-test.yaml
```

Check the controller logs:

```
kubectl logs -l app=timecheck-controller --tail 100
```

> This controller doesn't handle the situation where the CRD doesn't exist, so the container exits and the Pod will keep restarting

## Sample Solution

The [CRD](./solution/timecheck-crd.yaml) specifies the structure of the custom timecheck resource.

```
kubectl apply -f lab/solution/timecheck-crd.yaml
```

Restart the controller - it will be in CrashLoopBackOff status by now:

```
kubectl delete pods -l app=timecheck-controller
```

Now you can create the custom resource:

```
kubectl apply -f lab/timecheck-test.yaml
```

Check the controller has created a Deployment:

```
kubectl logs -l app=timecheck-controller --tail 4
```

List the timecheck resources:

```
kubectl get tc
```

And check the logs from the timecheck app:

```
kubectl logs -l app=timecheck -c logger
```

## Teardown

Delete the CRD:

```
kubectl delete crd timechecks.ch20.kiamol.net
```

And the controller:

```
kubectl delete -f lab/timecheck-controller/
```