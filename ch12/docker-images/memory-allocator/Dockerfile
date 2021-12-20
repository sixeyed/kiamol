FROM golang:1.15-alpine AS builder
ENV CGO_ENABLED=0

WORKDIR /src
COPY ./src/go.mod .
RUN go mod download

COPY ./src/main.go .
RUN go build -o /server

# app
FROM alpine:3.15

EXPOSE 80
CMD ["/app/server"]

WORKDIR /app
COPY ./src/config.toml .
COPY --from=builder /server .