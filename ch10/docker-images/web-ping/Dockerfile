FROM node:12.17 AS builder

WORKDIR /src
COPY src/package.json .
RUN npm install

FROM node:12.17-alpine3.11

CMD ["node", "/app/app.js"]

ENV TARGET="blog.sixeyed.com" \
    METHOD="HEAD" \
    INTERVAL="3000"

WORKDIR /app
COPY --from=builder /src/node_modules/ /app/node_modules/
COPY src/ .