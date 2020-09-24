#!/bin/bash

echo '---------'
echo "kiamol.sh as: $(whoami)"
echo '---------'

# set MOTD
sudo sh -c 'echo "\n** Learn Kubernetes in a Month of Lunches **\n**  https://kiamol.net  **\nSource is in /kiamol\n" > /etc/motd'

# add aliases:
echo "alias k='kubectl'" >> ~/.bashrc
echo "alias d='docker'" >> ~/.bashrc
echo "alias cls='clear'" >> ~/.bashrc