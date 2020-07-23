# ch18 lab

## Setup

Connect to the control plane node:

```
vagrant ssh kiamol-control
```

## Sample Solution

You take a node out of service by draining it, which reschedules the Pods - you need the DaemonSet flag so the system components are ignored:

```
kubectl drain kiamol-node --ignore-daemonsets
```

There is also the `kubectl cordon` command which marks the node so it won't have any new Pods scheduled, but that doesn't remove the existing Pods.

When you're done working on the node you can bring it back into service by uncordoning it:

```
kubectl uncordon kiamol-node
```

That marks the node as available for work, but Kubernetes doesn't automatically reschedule existing workloads so the node won't start any application Pods. 

You can rebalance the API Pods by restarting the rollout:

```
kubectl rollout restart deploy apod-api
```

## Teardown

You can delete all of the Vagrant VMs with:

```
vagrant destroy
```