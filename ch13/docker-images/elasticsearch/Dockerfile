FROM alpine:3.12 as download
# 6.8 makes a 310MB image; 7.8 is 670MB; official 7.8 is 810MB
ARG ELASTIC_VERSION="6.8.10"

RUN wget "https://artifacts.elastic.co/downloads/elasticsearch/elasticsearch-oss-${ELASTIC_VERSION}.tar.gz"
RUN tar -xzf "elasticsearch-oss-${ELASTIC_VERSION}.tar.gz"

# elasticsearch
FROM openjdk:11-jre-slim
ARG ELASTIC_VERSION="6.8.10"

EXPOSE 9200 9300
ENV ES_HOME="/usr/share/elasticsearch" \
    ES_JAVA_OPTS="-Xms1024m -Xmx1024m"

RUN groupadd -g 1000 elasticsearch && \
    adduser --uid 1000 --gid 1000 --home /usr/share/elasticsearch elasticsearch

WORKDIR /usr/share/elasticsearch
COPY --from=download --chown=1000:0 /elasticsearch-${ELASTIC_VERSION}/ .
COPY elasticsearch.yml log4j2.properties config/

USER elasticsearch
CMD ["./bin/elasticsearch"]