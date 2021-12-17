#!/bin/sh

# set up access to Kube API
kubectl config set-cluster default --server=https://kubernetes.default.svc.cluster.local --certificate-authority=/var/run/secrets/kubernetes.io/serviceaccount/ca.crt
kubectl config set-context default --cluster=default
kubectl config set-credentials user --token=$(cat /var/run/secrets/kubernetes.io/serviceaccount/token)
kubectl config set-context default --user=user
kubectl config use-context default

# promote registry details to env:
registry=$(cat ~/.docker/config.json | jq '.auths' | jq 'keys[0]' -r)
if [ "$registry" = "https://index.docker.io/v1/" ]; then export REGISTRY_SERVER='docker.io'; else export REGISTRY_SERVER=$registry; fi
export REGISTRY_USER=$(cat ~/.docker/config.json | jq '.auths[].username' -r)
echo "*** Using registry: $REGISTRY_SERVER, with user: $REGISTRY_USER ***"

# run Jenkins
java -Duser.home=${JENKINS_HOME} -Djenkins.install.runSetupWizard=false -jar /jenkins/jenkins.war
