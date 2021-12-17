ARG ALPINE_VERSION="3.15"

FROM golang:1.14-alpine3.13 AS builder
ARG GOGS_VERSION="v0.12.3"

RUN apk --no-cache --no-progress add --virtual \
    build-deps \
    build-base \
    git \
    linux-pam-dev 

WORKDIR /go/src/github.com/gogs
RUN git clone https://github.com/gogs/gogs.git && \
    cd gogs && \
    git checkout $GOGS_VERSION

WORKDIR /go/src/github.com/gogs/gogs
RUN go build -tags "sqlite" -o /out/gogs 

FROM alpine:$ALPINE_VERSION AS download-base
WORKDIR /downloads
RUN echo "$(apk --print-arch)" > /arch.txt 
RUN ARCH2= && alpineArch="$(apk --print-arch)" \
    && case "${alpineArch##*-}" in \
    x86_64) ARCH2='amd64' ;; \
    aarch64) ARCH2='arm64' ;; \
    *) echo "unsupported architecture"; exit 1 ;; \
    esac && \
    echo $ARCH2 > /arch2.txt 

# Gogs - adapted from project Dockerfile at github.com/gogs/gogs
FROM download-base AS gogs

# Install system utils & Gogs runtime dependencies
RUN wget -O /usr/sbin/gosu "https://github.com/tianon/gosu/releases/download/1.14/gosu-$(cat /arch2.txt)" && \
    chmod +x /usr/sbin/gosu \
    && echo http://dl-2.alpinelinux.org/alpine/edge/community/ >> /etc/apk/repositories \
    && apk --no-cache --no-progress add \
    bash \
    ca-certificates \
    curl \
    git \
    linux-pam \
    openssh \
    s6 \
    shadow \
    socat \
    tzdata \
    rsync

ENV GOGS_CUSTOM /data/gogs

COPY --from=builder /go/src/github.com/gogs/gogs/docker/nsswitch.conf /etc/nsswitch.conf

WORKDIR /app/gogs
COPY --from=builder /go/src/github.com/gogs/gogs/docker ./docker
COPY --from=builder /go/src/github.com/gogs/gogs/templates ./templates
COPY --from=builder /go/src/github.com/gogs/gogs/public ./public
COPY --from=builder /out/gogs .

RUN ./docker/finalize.sh

VOLUME ["/data"]
EXPOSE 3000
ENTRYPOINT ["/app/gogs/docker/start.sh"]
CMD ["/bin/s6-svscan", "/app/gogs/docker/s6/"]

# Customized Gogs build
FROM gogs

RUN apk add --no-cache jq
ENV GOGS_CUSTOM=""

COPY app.ini ./custom/conf/app.ini
COPY gogs-install.txt .
COPY init.sh .

# this uses the original start script to prep the data folders:
RUN chmod o+w ./custom/conf/app.ini && \
    chmod +x init.sh && ./init.sh

# replace with custom start script:
COPY start.sh ./docker/start.sh
RUN chmod +x ./docker/start.sh