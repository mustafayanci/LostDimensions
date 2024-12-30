#!/bin/bash

# =================================================================
# Auto Git Deploy Script
# =================================================================
#
# Purpose:
#   Automatically detect and deploy changes to GitHub repository
#   by watching for file changes, deletions, and additions.
#
# Features:
#   - Monitors repository every 3 seconds
#   - Detects file modifications, additions, and deletions
#   - Automatically commits with timestamped messages
#   - Pushes changes to GitHub main branch
#   - Efficient status checking to minimize system resource usage
#
# Usage:
#   1. Make script executable: chmod +x watch-and-deploy.sh
#   2. Run: ./watch-and-deploy.sh
#   or
#   Run: npm run watch (if using package.json script)
#
# Note: 
#   Script must be run from the root of your git repository
# =================================================================

# Variables to store last modified times and git status
LAST_CHECK=0
LAST_STATUS=""

# Function to check if files have changed
check_changes() {
    # Get current git status
    CURRENT_STATUS=$(git status --porcelain)
    
    # Check if status has changed (including deletions)
    if [ "$CURRENT_STATUS" != "$LAST_STATUS" ]; then
        LAST_STATUS="$CURRENT_STATUS"
        if [ -n "$CURRENT_STATUS" ]; then
            echo "Changes detected in files:"
            echo "$CURRENT_STATUS"
            return 0
        fi
    fi
    return 1
}

# Main loop
while true; do
    if check_changes; then
        echo "Changes detected. Deploying..."
        TIMESTAMP=$(date "+%Y-%m-%d_%H-%M-%S")
        git add .
        git add -u  # This will track deletions
        git commit -m "auto: ${TIMESTAMP} - updates detected and deployed"
        git push origin main
        echo "Changes deployed at $(date)"
    fi
    sleep 3
done