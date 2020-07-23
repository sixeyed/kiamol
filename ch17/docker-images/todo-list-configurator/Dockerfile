FROM alpine:3.12

RUN apk add --no-cache --repository http://dl-cdn.alpinelinux.org/alpine/edge/testing kubectl

COPY start.sh /
COPY config.json /config-in/config.json
RUN chmod +x /start.sh

CMD /start.sh