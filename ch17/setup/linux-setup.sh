#!/bin/bash

DOCKER_VERSION="5:19.03.12~3-0~ubuntu-focal"
KUBERNETES_VERSION="1.18.5-00"
hostname -I | awk '{print $NF}' > /tmp/ip.txt

# turn off swap - for the Kubelet
swapoff -a 
sed -ri '/\sswap\s/s/^#?/#/' /etc/fstab

# install Docker (https://docs.docker.com/install/linux/docker-ce/ubuntu/)
apt-get update
apt-get install -y \
    apt-transport-https \
    ca-certificates \
    curl \
    gnupg-agent \
    software-properties-common

curl -fsSL https://download.docker.com/linux/ubuntu/gpg | apt-key add -

add-apt-repository \
   "deb [arch=amd64] https://download.docker.com/linux/ubuntu \
   $(lsb_release -cs) \
   stable"

apt-get update
apt-get install -y \
    docker-ce=$DOCKER_VERSION \
    docker-ce-cli=$DOCKER_VERSION \
    containerd.io

# install Kubeadm etc.
curl -s https://packages.cloud.google.com/apt/doc/apt-key.gpg | sudo apt-key add -
echo 'deb https://apt.kubernetes.io/ kubernetes-xenial main' > /etc/apt/sources.list.d/kubernetes.list

apt-get update
apt-get install -y \
    kubelet=$KUBERNETES_VERSION \
    kubeadm=$KUBERNETES_VERSION \
    kubectl=$KUBERNETES_VERSION

# set iptables for Flannel
sysctl net.bridge.bridge-nf-call-iptables=1