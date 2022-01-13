ARG ALPINE_VERSION="3.15"    
FROM alpine:$ALPINE_VERSION

RUN apk add --no-cache curl git jq
RUN apk add --no-cache --repository http://dl-cdn.alpinelinux.org/alpine/edge/testing kubectl

RUN set -x; cd "$(mktemp -d)" && \
    OS="$(uname | tr '[:upper:]' '[:lower:]')" && \
    ARCH="$(uname -m | sed -e 's/x86_64/amd64/' -e 's/\(arm\)\(64\)\?.*/\1\2/' -e 's/aarch64$/arm64/')" && \
    KREW="krew-${OS}_${ARCH}" && \
    curl -fsSLO "https://github.com/kubernetes-sigs/krew/releases/latest/download/${KREW}.tar.gz" && \
    tar zxvf "${KREW}.tar.gz" && \
    ./"${KREW}" install krew

ENV PATH="${PATH}:/root/.krew/bin"

CMD exec /bin/sh -c "trap : TERM INT; (while true; do sleep 1000; done) & wait"