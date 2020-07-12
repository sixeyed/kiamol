#!/bin/sh -eux

kiamol='
Welcome to Learn Kubernetes in a Month of Lunches.
This box is for Chapter 17. Find the source at https://kiamol.net'

if [ -d /etc/update-motd.d ]; then
    MOTD_CONFIG='/etc/update-motd.d/99-kiamol'

    cat >> "$MOTD_CONFIG" <<KIAMOL
#!/bin/sh

cat <<'EOF'
$kiamol
EOF
KIAMOL

    chmod 0755 "$MOTD_CONFIG"
else
    echo "$kiamol" >> /etc/motd
fi
