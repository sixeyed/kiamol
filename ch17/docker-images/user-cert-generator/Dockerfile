FROM alpine:3.12

RUN apk add --no-cache jq openssl
RUN apk add --no-cache --repository http://dl-cdn.alpinelinux.org/alpine/edge/testing kubectl

COPY start.sh /
RUN chmod +x /start.sh

ENV USER_NAME="reader@kiamol.net" \
    GROUP="kiamol" \
    SET_CONTEXT="" \
    PRINT_CERTS=""

WORKDIR /certs
CMD /start.sh

COPY csr.yaml .
