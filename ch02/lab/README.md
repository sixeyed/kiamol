kubectl apply -f solution/deployment.yaml

kubectl port-forward deploy/whoami 8080:80

curl http://localhost:8080

> "I'm whoami-687976f48b-tkxp9 running on Linux 4.19.76-linuxkit #1 SMP Thu Oct 17 19:31:58 UTC 2019"

kubectl get pods -o custom-columns=NAME:metadata.name

> whoami-68bf776fd-s6sr9

kubectl exec deploy/whoami -- sh -c 'hostname'

> whoami-687976f48b-tkxp9
