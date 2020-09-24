# Using Vagrant to run KinD

This configures a virtual machine which installs Docker and KinD.

## Pre-requisites

You need to install [Vagrant](https://www.vagrantup.com) and use one of the supported VM providers:

* Hyper-V on Windows
* VirtualBox on Linux, Windows or Mac

> Pull requests to add support for other providers is welcome :)

## Usage

From this directory run:

```
vagrant up
```

The first time you run this it will take a while to download the base VM, but subsequent runs will be fast.

Connect to the VM:

```
vagrant ssh
```

The VM mounts the `kiamol` folder on your host into the VM, so you can get to all the source for the book from here:

```
cd /kiamol
```

## Teardown

Use one of these options:

* `vagrant suspend` to suspend the VM which keeps your current state
* `vagrant halt` to stop the VM
* `vagrant destroy` to remove the VM altogether
