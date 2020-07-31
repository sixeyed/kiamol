#!/bin/sh

CONTROLLER='ingress-nginx-controller'
NS='kiamol-ingress-nginx'
IP=$(kubectl get svc $CONTROLLER -o jsonpath='{.status.loadBalancer.ingress[0].*}' -n $NS)
if [ "$IP" = "localhost" ]; then 
    IP='127.0.0.1'
fi

echo "\n$IP  todo.kiamol.local\n$IP  api.todo.kiamol.local" | sudo tee -a /etc/hosts