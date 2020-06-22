#!/bin/sh

buildctl --addr tcp://buildkitd:1234 \
  build \
  --frontend=gateway.v0 \
  --opt source=kiamol/buildkit-buildpacks \
  --local context=src \
  --output type=image,name=${REGISTRY_SERVER}/${REGISTRY_USER}/bulletin-board:${BUILD_NUMBER}-kiamol,push=true
