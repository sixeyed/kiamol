#!/bin/bash

# pi @ 100K do is hard on CPU:
URL=$(kubectl get svc pi-web -o jsonpath='http://{.status.loadBalancer.ingress[0].*}:8031/?dp=100000')

# two calls is enough to trigger HPA:
curl -s $URL > /dev/null & 
curl -s $URL > /dev/null &
