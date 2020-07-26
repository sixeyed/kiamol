#!/bin/bash

echo '---------'
echo "kiamol.sh as: $(whoami)"
echo '---------'

# create the cluster
mkdir -p ~/.kube
sudo kind create cluster --image kindest/node:v1.16.9 --name kiamol --kubeconfig /home/vagrant/.kube/config
sudo chown vagrant ~/.kube/config

# set MOTD
sudo sh -c 'echo "\n** Learn Kubernetes in a Month of Lunches **\n**  https://kiamol.net  **\nSource is in /kiamol\nIf Kubectl does not respond, start the Kind container with:\n  docker start kiamol-control-plane\n" > /etc/motd'