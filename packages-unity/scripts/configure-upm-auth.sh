#!/usr/bin/env bash

# Script to configure Unity Package Manager (UPM) authentication for TrickleCharge Registry
# Usage: chmod +x configure-upm-auth.sh && ./configure-upm-auth.sh

# 1. Prompt the user for their token securely
printf "Enter your TrickleCharge Registry Token: "
read -r REGISTRY_TOKEN

# 2. Safety Check: Exit early if the token is empty
if [[ -z "$REGISTRY_TOKEN" ]]; then
    echo "Error: Token cannot be empty. Authentication setup aborted." >&2
    exit 1
fi

CONFIG_PATH="$HOME/.upmconfig.toml"

# 3. Append to the config file
# The physical empty line before the header is completely preserved inside this block
cat << EOF >> "$CONFIG_PATH"

[npmAuth."https://npm.tricklecharge.dev/"]
_authToken = "$REGISTRY_TOKEN"
alwaysAuth = true
EOF

echo -e "\nSetup complete! Configured $CONFIG_PATH"