Scripts for provisioning base Vagrant boxes.

## Build

Hyper-V:

```
packer build -force -only=hyperv-iso -var "hyperv_switch=Default Switch" .\windows\windows-2019-core.json
```

## Export

Hyper-V:

```
vagrant box add --name kiamol-windows-2019 .\windows\windows-2019-core-hyperv.box
```

## Publish

Hyper-V:

```
vagrant cloud auth login

vagrant cloud provider create kiamol/windows-2019 hyperv 0.0.1
vagrant cloud provider upload kiamol/windows-2019 hyperv 0.0.1 windows-2019-core-hyperv.box
```

## Credits

Bento: https://github.com/chef/bento

Stefan Scherer: https://github.com/StefanScherer/packer-windows

Boxcutter: https://github.com/boxcutter/windows-ps