#!/bin/bash
#!/usr/bin/env bash

# This script reproduces what the kubelet does
# to calculate memory.available relative to root cgroup.

# current memory usage
memory_capacity_in_kb=$(cat /proc/meminfo | grep MemTotal | awk '{print $2}')
memory_capacity_in_bytes=$((memory_capacity_in_kb * 1024))
memory_usage_in_bytes=$(cat /sys/fs/cgroup/memory/memory.usage_in_bytes)
memory_total_inactive_file=$(cat /sys/fs/cgroup/memory/memory.stat | grep total_inactive_file | awk '{print $2}')

memory_working_set=${memory_usage_in_bytes}
if [ "$memory_working_set" -lt "$memory_total_inactive_file" ];
then
    memory_working_set=0
else
    memory_working_set=$((memory_usage_in_bytes - memory_total_inactive_file))
fi

memory_available_in_bytes=$((memory_capacity_in_bytes - memory_working_set))
memory_available_in_kb=$((memory_available_in_bytes / 1024))
memory_available_in_mb=$((memory_available_in_kb / 1024))

if [ -n "$MEMORY_STRESS_FACTOR" ]; then
    stress_mb=$(echo "$memory_available_in_mb * $MEMORY_STRESS_FACTOR" | bc -l)    
else
    stress_mb=$MEMORY_STRESS_MB
fi
stress_mb_int="$(printf '%d' $stress_mb 2>/dev/null)"

echo '----------------'
echo "Memory available: ${memory_available_in_mb}M"
echo "Stress factor: ${MEMORY_STRESS_FACTOR}"
echo "Stressing memory: ${stress_mb_int}M"
echo '----------------'

exec /bin/sh -c "trap : TERM INT; (stress -q --vm 1 --vm-bytes ${stress_mb_int}M --vm-hang $STRESS_HANG -t $STRESS_TIMEOUT) & wait"