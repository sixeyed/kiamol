FROM node:12.17 AS builder

WORKDIR /src
COPY package.json .
RUN npm install

FROM node:12.17-alpine3.11

CMD ["node", "/app/server.js"]
ENV PORT=80
EXPOSE 80

WORKDIR /app
COPY --from=builder /src/node_modules/ /app/node_modules/
COPY *.js ./