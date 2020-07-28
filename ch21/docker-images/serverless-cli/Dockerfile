FROM node:12

ENV SERVERLESS_VERSION="1.77.1" \
    KUBERNETES_VERSION="1.18.5" 

RUN npm install -g serverless@${SERVERLESS_VERSION}

RUN curl -LO https://storage.googleapis.com/kubernetes-release/release/v${KUBERNETES_VERSION}/bin/linux/amd64/kubectl && \
    chmod +x kubectl && \
    mv kubectl /usr/local/bin/

COPY start.sh /
RUN chmod +x /start.sh
CMD /start.sh
WORKDIR /kiamol