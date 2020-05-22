#!/bin/bash

URL=$(kubectl get svc vweb -o jsonpath='http://{.status.loadBalancer.ingress[0].*}:8090/v.txt')

for i in {1..10}
do
   curl -s "$URL"
done