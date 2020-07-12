Scripts for provisioning base Vagrant boxes.

## Build

Hyper-V:

```
packer build -force -only=hyperv-iso -var "hyperv_switch=Default Switch" .\windows\windows-2019-core.json
packer build -force -only=hyperv-iso -var "hyperv_switch=Default Switch" .\ubuntu\ubuntu-2004.json
```

## Export

Hyper-V:

```
vagrant box add --name kiamol-windows-2019 .\windows\windows-2019-core-hyperv.box
vagrant box add --name kiamol-ubuntu-20.04 .\ubuntu\ubuntu-2004-hyperv.box
```

## Publish

Hyper-V:

```
vagrant cloud auth login

vagrant cloud provider create kiamol/windows-2019 hyperv 0.0.1
vagrant cloud provider upload kiamol/windows-2019 hyperv 0.0.1 windows-2019-core-hyperv.box

vagrant cloud provider create kiamol/ubuntu-20.04 hyperv 0.0.1
vagrant cloud provider upload kiamol/ubuntu-20.04 hyperv 0.0.1 ubuntu-2004-hyperv.box
```

## Credits

Bento: https://github.com/chef/bento

Stefan Scherer: https://github.com/StefanScherer/packer-windows

Boxcutter: https://github.com/boxcutter/windows-ps