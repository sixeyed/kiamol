#!/bin/sh

helm upgrade --install \
  --atomic \
  --set registryServer=${REGISTRY_SERVER},registryUser=${REGISTRY_USER},imageBuildNumber=${BUILD_NUMBER} \
  --namespace kiamol-ch11-test \
  bulletin-board \
  helm/bulletin-board