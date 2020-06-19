#!/bin/sh

helm upgrade --install \
  --create-namespace \
  --atomic \
  --set registryServer=${REGISTRY_SERVER},registryUser=${REGISTRY_USER},imageBuildNumber=${BUILD_NUMBER} \
  --namespace kiamol-ch11 \
  bulletin-board \
  helm/bulletin-board