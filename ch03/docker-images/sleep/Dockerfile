FROM alpine:3.11

CMD exec /bin/sh -c "trap : TERM INT; (while true; do sleep 1000; done) & wait"