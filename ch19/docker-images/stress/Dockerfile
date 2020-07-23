FROM debian:buster-slim

RUN apt-get update \
 && apt-get install -y --no-install-recommends \
    bc \
    stress \
 && rm -rf /var/lib/apt/lists/*

 ENV MEMORY_STRESS_MB="50" \
     MEMORY_STRESS_FACTOR="" \
     STRESS_HANG="3540" \
     STRESS_TIMEOUT="3600"

COPY ./stress-memory.sh .

CMD chmod +x ./stress-memory.sh && ./stress-memory.sh
