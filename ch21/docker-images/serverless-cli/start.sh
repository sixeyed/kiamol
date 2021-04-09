#!/bin/sh

# set up access to Kube API
kubectl config set-cluster default --server=https://kubernetes.default.svc.cluster.local --certificate-authority=/var/run/secrets/kubernetes.io/serviceaccount/ca.crt
kubectl config set-context default --cluster=default
kubectl config set-credentials user --token=$(cat /var/run/secrets/kubernetes.io/serviceaccount/token)
kubectl config set-context default --user=user
kubectl config use-context default

cd /
git clone https://github.com/sixeyed/kiamol

while true; do sleep 1000; done