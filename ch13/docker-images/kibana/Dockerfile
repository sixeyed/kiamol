FROM alpine:3.12 as download
# 6.8 makes a 360MB image; 7.8 is 690MB; official 7.8 is 1.3GB
ARG KIBANA_VERSION="6.8.10"

RUN wget "https://artifacts.elastic.co/downloads/kibana/kibana-oss-${KIBANA_VERSION}-linux-x86_64.tar.gz"
RUN tar -xzf "kibana-oss-${KIBANA_VERSION}-linux-x86_64.tar.gz"

WORKDIR /kibana-oss-${KIBANA_VERSION}-linux-x86_64
RUN rm -rf node

FROM node:10.19.0-slim
ARG KIBANA_VERSION="6.8.10"

EXPOSE 5601
ENV KIBANA_HOME="/usr/share/kibana" 

WORKDIR /usr/share/kibana
COPY --from=download /kibana-${KIBANA_VERSION}-linux-x86_64/ .
COPY kibana bin/
COPY kibana.yml config/

RUN chmod +x bin/kibana
CMD ["/usr/share/kibana/bin/kibana"]