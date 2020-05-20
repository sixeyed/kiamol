# Ch08 lab

Run the app:

```
kubectl apply -f lab/nginx/
```

Get the URL and browse:

```
kubectl get svc nginx -o jsonpath='http://{.status.loadBalancer.ingress[0].*}:8088'
```

> It works, but its just a single Pod writing logs to an EmptyDir.

## Sample Solution

My [StatefulSet](solution/nginx-statefulset.yaml) runs three Pods, with volume claim templates for storage.

```
kubectl apply -f lab/solution/nginx-statefulset.yaml
```

> Make lots of calls to the web app

I used this in Powershell:

```
for($i = 0; $i -lt 100; $i++) { curl http://localhost:8088 | Out-Null }
```

Then the [Job](solution/disk-calc-job.yaml) is configured to mount all of the PVCs used in the StatefulSet Pods.

```
kubectl apply -f lab/solution/disk-calc-job.yaml
```

When I check the logs I see this:

```
PS>kubectl logs -l job-name=disk-calc
32.0K   /nginx0/access.log
24.0K   /nginx1/access.log
40.0K   /nginx2/access.log
```

## Teardown

Delete the lab resources by their labels:

```
kubectl delete all -l kiamol=ch08-lab

kubectl delete pvc -l kiamol=ch08-lab
```
