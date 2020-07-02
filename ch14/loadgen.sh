#!/bin/bash

URL=$(kubectl get svc todo-proxy -o jsonpath='http://{.status.loadBalancer.ingress[0].*}:8015' -n kiamol-ch14-test)

for i in {1..500}
do
   curl -s "$URL/List" > /dev/null
done