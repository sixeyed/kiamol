FROM golang:1.15-alpine  AS builder
ENV CGO_ENABLED=0

WORKDIR /src
COPY go.mod .
RUN go mod download

COPY main.go .
RUN go build -o /server

# app
FROM alpine:3.15

ENV IMAGE_API_URL="http://apod-api/image" \
    ACCESS_API_URL="http://apod-log/access-log"

CMD ["/web/server"]

WORKDIR /web
COPY index.html .
COPY --from=builder /server .