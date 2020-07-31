#using node to match base image for serverless cli
FROM node:12  

ENV KUBELESS_VERSION="v1.0.7" \
    KUBERNETES_VERSION="1.18.5" 

RUN curl -LO https://github.com/kubeless/kubeless/releases/download/${KUBELESS_VERSION}/kubeless_linux-amd64.zip && \
    unzip kubeless_linux-amd64.zip && \
    rm -f kubeless_linux-amd64.zip && \
    chmod +x bundles/kubeless_linux-amd64/kubeless && \
    mv bundles/kubeless_linux-amd64/kubeless /usr/local/bin/

RUN curl -LO https://storage.googleapis.com/kubernetes-release/release/v${KUBERNETES_VERSION}/bin/linux/amd64/kubectl && \
    chmod +x kubectl && \
    mv kubectl /usr/local/bin/

COPY start.sh /
RUN chmod +x /start.sh
CMD /start.sh
WORKDIR /kiamol