#!/bin/sh

CONTROLLER="$2-controller"
NS="kiamol-$2"
IP=$(kubectl get svc $CONTROLLER -o jsonpath='{.status.loadBalancer.ingress[0].*}' -n $NS)
if [ "$IP" = "localhost" ]; then 
    IP='127.0.0.1'
fi

echo "$IP  $1" | sudo tee -a /etc/hosts