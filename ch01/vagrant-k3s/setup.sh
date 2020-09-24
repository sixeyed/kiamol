#!/bin/bash

echo '--------'
echo "setup.sh as: $(whoami)"
echo '--------'

hostname -I | awk '{print $NF}' > /tmp/ip.txt

# turn off swap - for the Kubelet
swapoff -a 
sed -ri '/\sswap\s/s/^#?/#/' /etc/fstab

# install Docker
curl -fsSL https://get.docker.com | sh

# use Docker without sudo
sudo usermod -aG docker vagrant

# install Docker Compose
sudo curl -L "https://github.com/docker/compose/releases/download/1.27.3/docker-compose-$(uname -s)-$(uname -m)" -o /usr/local/bin/docker-compose
sudo chmod +x /usr/local/bin/docker-compose

# install Helm
curl https://raw.githubusercontent.com/helm/helm/master/scripts/get-helm-3 | bash

# install K3s
curl -sfL https://get.k3s.io | sh -s - --docker --disable=traefik --write-kubeconfig-mode=644