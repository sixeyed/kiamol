ARG ALPINE_VERSION="3.15"    
FROM alpine:$ALPINE_VERSION

RUN apk add --no-cache --repository http://dl-cdn.alpinelinux.org/alpine/edge/testing kubectl

COPY start.sh /
COPY config.json /config-in/config.json
RUN chmod +x /start.sh

CMD /start.sh