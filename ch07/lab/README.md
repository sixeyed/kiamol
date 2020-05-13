# Ch07 lab

Run the app:

```
kubectl apply -f lab/pi/
```

Check the app and see it's broken:

```
kubectl describe pod -l app=pi-web
```

> The startup command for the app container uses a script which doesn't exist.

## Sample Solution

My updated [Deployment](solution/web.yaml) for the web app uses multiple containers.

Init containers:

- init container `init-1` writes the startup script file in an EmptyDir volume
- `init-2` makes the startup script executable
- `init-3` writes a text file with a fake app version.

App container:

- mounts the EmptyDir volume and runs the script at startup; serves the app on port 80.

Sidecar:

- runs a simple NCat HTTP server, serving the version number text file on port 8080.


Run the update and browse to your Service on port 8070 for Pi and 8071 for the version:

```
kubectl apply -f lab/solution/
```

> This is not the most efficient way to do this! It's just an example which makes use of multi-container Pods.


## Teardown

Delete the lab resources by their labels:

```
kubectl get all -l kiamol=ch07-lab

kubectl delete all -l kiamol=ch07-lab
```
