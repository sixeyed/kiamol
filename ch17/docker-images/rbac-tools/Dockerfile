FROM alpine:3.12

RUN apk add --no-cache curl git jq
RUN apk add --no-cache --repository http://dl-cdn.alpinelinux.org/alpine/edge/testing kubectl

RUN curl -fsSLO "https://github.com/kubernetes-sigs/krew/releases/latest/download/krew.{tar.gz,yaml}" && \
    tar zxvf krew.tar.gz && \
    KREW=./krew-"$(uname | tr '[:upper:]' '[:lower:]')_amd64" && \
    "$KREW" install --manifest=krew.yaml --archive=krew.tar.gz && \
    mv $KREW /usr/local/bin/krew && \
    rm -f ./krew*

ENV PATH="${PATH}:/root/.krew/bin"

CMD exec /bin/sh -c "trap : TERM INT; (while true; do sleep 1000; done) & wait"