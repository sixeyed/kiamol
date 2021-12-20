ARG NODE_TAG=10.23.1-alpine3.11
ARG ALPINE_VERSION="3.15"

FROM alpine:$ALPINE_VERSION AS download-base
WORKDIR /downloads
RUN echo "$(apk --print-arch)" > /arch.txt 

FROM download-base AS installer
ARG KIBANA_VERSION="7.10.2" 

# find the downloads for previous versions here - https://www.elastic.co/downloads/past-releases#kibana-oss
# 7.10 is the latest version which is OSS, see - https://www.elastic.co/pricing/faq/licensing

RUN wget -O kibana.tar.gz https://artifacts.elastic.co/downloads/kibana/kibana-oss-${KIBANA_VERSION}-linux-$(cat /arch.txt).tar.gz

RUN mkdir /kibana && \
    tar -xzf kibana.tar.gz --strip-components=1 -C /kibana && \
    rm -rf /kibana/node

# Kibana requires Node.js - this image is the official Node distribution
# see the Node.js versions in https://www.elastic.co/guide/en/kibana/master/upgrading-nodejs.html
FROM node:$NODE_TAG

EXPOSE 5601
ENV KIBANA_HOME="/usr/share/kibana" 

WORKDIR /usr/share/kibana
COPY --from=installer /kibana .
COPY ./kibana bin/
COPY ./kibana.yml config/

RUN chmod +x bin/kibana
CMD ["/usr/share/kibana/bin/kibana", "--allow-root"]