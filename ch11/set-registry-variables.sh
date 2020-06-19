#! /bin/sh

echo 'Registry server (blank for Docker Hub):'
read registry

if [ -n "$registry" ]; then
    REGISTRY_SERVER=$registry
else
    REGISTRY_SERVER='https://index.docker.io/v1/'
fi

echo 'Username:'
read REGISTRY_USER

echo 'Password:'
read -s REGISTRY_PASSWORD