FROM node:10.21-alpine3.11 AS builder

WORKDIR /src
COPY src/package.json .
RUN npm install

# app
FROM node:10.21-alpine3.11 

ENV PORT=8080
EXPOSE 8080
CMD ["node", "/app/server.js"]

WORKDIR /app
COPY --from=builder /src/node_modules/ ./node_modules/
COPY src/ .