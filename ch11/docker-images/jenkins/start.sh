#!/bin/sh

# set up access to Kube API
kubectl config set-cluster default --server=https://kubernetes.default --certificate-authority=/var/run/secrets/kubernetes.io/serviceaccount/ca.crt
kubectl config set-context default --cluster=default
kubectl config set-credentials user --token=$(cat /var/run/secrets/kubernetes.io/serviceaccount/token)
kubectl config set-context default --user=user
kubectl config use-context default

# promote registry user to env:
export REGISTRY_USER=$(cat ~/.docker/config.json | jq '.auths[].username' -r)

# run Jenkins
java -Duser.home=${JENKINS_HOME} -Djenkins.install.runSetupWizard=false -jar /jenkins/jenkins.war