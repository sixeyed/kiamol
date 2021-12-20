ARG ALPINE_VERSION="3.15"

FROM alpine:$ALPINE_VERSION AS download-base
WORKDIR /downloads
RUN echo "$(apk --print-arch)" > /arch.txt 

FROM download-base AS installer
ARG ES_VERSION="7.10.2"

# find the downloads for previous versions here - https://www.elastic.co/downloads/past-releases#elasticsearch-oss-no-jdk
# 7.10 is the latest version which is OSS, see - https://www.elastic.co/pricing/faq/licensing
# there's no no-jdk version for arm64, so we download the JDK and strip it out

#https://artifacts.elastic.co/downloads/elasticsearch/elasticsearch-oss-7.10.2-linux-x86_64.tar.gz
#https://artifacts.elastic.co/downloads/elasticsearch/elasticsearch-oss-7.10.2-linux-aarch64.tar.gz

RUN wget -O elasticsearch.tar.gz "https://artifacts.elastic.co/downloads/elasticsearch/elasticsearch-oss-${ES_VERSION}-linux-$(cat /arch.txt).tar.gz" 

RUN mkdir /elasticsearch && \
    tar -xzf elasticsearch.tar.gz --strip-components=1 -C /elasticsearch && \
    rm -rf /elasticsearch/jdk

# Elasticsearch requires a JVM - this image provides a minimal JRE installation
# see the product-JVM version matrix https://www.elastic.co/support/matrix#matrix_jvm
FROM openjdk:11.0.11-jre-slim

WORKDIR /usr/share/elasticsearch
COPY --from=installer /elasticsearch .

EXPOSE 9200 9300
ENV ES_HOME="/usr/share/elasticsearch" \
    ES_JAVA_OPTS="-Xms1024m -Xmx1024m"

COPY elasticsearch.yml log4j2.properties ./config/

RUN groupadd -g 1000 elasticsearch && \
    adduser -uid 1000 -gid 1000 --home ${ES_HOME} elasticsearch && \
    chmod 0775 ${ES_HOME} && \
    chown -R 1000:0 ${ES_HOME}

USER elasticsearch:root
CMD ["/usr/share/elasticsearch/bin/elasticsearch"]