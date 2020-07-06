FROM alpine:3.12

RUN apk add --no-cache openssl
RUN apk add --no-cache --repository http://dl-cdn.alpinelinux.org/alpine/edge/testing kubectl

COPY start.sh /
RUN chmod +x /start.sh

ENV HOST_NAME="kiamol.local" \
    HOST_IP="127.0.0.1" \
    SAN="DNS:hello.kiamol.local,DNS:vweb.kiamol.local,DNS:todo.kiamol.local,DNS:todo2.kiamol.local,DNS:pi.kiamol.local" \
    EXPIRY_DAYS=730

WORKDIR /certs
CMD /start.sh ${HOST_NAME} ${HOST_IP} ${SAN} ${EXPIRY_DAYS}
