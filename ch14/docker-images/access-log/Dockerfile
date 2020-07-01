FROM node:10.19.0-slim AS builder

WORKDIR /src
COPY src/package.json .
RUN npm install

# app
FROM node:10.19.0-slim

EXPOSE 80
ENV PORT=80
CMD ["node", "server.js"]

WORKDIR /app
COPY --from=builder /src/node_modules/ /app/node_modules/
COPY src/ .