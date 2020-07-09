FROM node:10.19.0-slim AS builder

WORKDIR /src
COPY src/package.json .
RUN npm install

# app
FROM node:10.19.0-slim

EXPOSE 8080
ENV PORT="8080" \
    USE_HTTPS="false"

CMD ["node", "server.js"]

WORKDIR /app
COPY --from=builder /src/node_modules/ /app/node_modules/
COPY src/ .