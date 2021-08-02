FROM alpine:3.12 AS installer

ARG JENKINS_VERSION="2.289.3"
WORKDIR /downloads/jenkins
RUN wget http://mirrors.jenkins.io/war-stable/$JENKINS_VERSION/jenkins.war

ARG BUILDKIT_VERSION="v0.9.0"
WORKDIR /downloads/buildkit
RUN wget https://github.com/moby/buildkit/releases/download/$BUILDKIT_VERSION/buildkit-$BUILDKIT_VERSION.linux-amd64.tar.gz
RUN tar xvf buildkit-$BUILDKIT_VERSION.linux-amd64.tar.gz

ARG HELM_VERSION="v3.6.2"
WORKDIR /downloads/helm
RUN wget https://get.helm.sh/helm-$HELM_VERSION-linux-amd64.tar.gz
RUN tar xvf helm-$HELM_VERSION-linux-amd64.tar.gz

RUN apk add --repository http://dl-cdn.alpinelinux.org/alpine/edge/testing kubectl

# Jenkins
FROM openjdk:8-jre-alpine

# jenkins deps
RUN apk add --no-cache \
    bash \
    coreutils \
    git \
    openssh-client \
    ttf-dejavu \
    unzip 

# additional
RUN apk add --no-cache \
    jq
    
ARG JENKINS_VERSION="2.2.289.3"
ENV JENKINS_VERSION=${JENKINS_VERSION} \
    JENKINS_HOME="/data"

VOLUME ${JENKINS_HOME}

EXPOSE 8080
ENTRYPOINT /start.sh

COPY --from=installer /downloads/buildkit/bin/buildctl /usr/bin/buildctl
COPY --from=installer /downloads/helm/linux-amd64/helm /usr/bin/helm
COPY --from=installer /usr/bin/kubectl /usr/bin/kubectl
COPY --from=installer /downloads/jenkins/jenkins.war /jenkins/jenkins.war

COPY ./jenkins.install.UpgradeWizard.state ${JENKINS_HOME}/
COPY ./scripts/ ${JENKINS_HOME}/init.groovy.d/
COPY start.sh /

RUN chmod +x /start.sh