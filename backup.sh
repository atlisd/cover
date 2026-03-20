#!/usr/bin/env bash
set -euo pipefail

# Load credentials from .env
if [ -f .env ]; then
  # shellcheck disable=SC2046
  export $(grep -v '^#' .env | xargs)
fi

POSTGRES_DB="${POSTGRES_DB:-splitclaude}"
POSTGRES_USER="${POSTGRES_USER:-postgres}"
BACKUP_FILE="${1:-scrooge_$(date +%Y%m%d-%H%M).bck}"

echo "Backing up database '$POSTGRES_DB' to '$BACKUP_FILE'..."
docker compose exec -T db pg_dump -U "$POSTGRES_USER" "$POSTGRES_DB" > "$BACKUP_FILE"
echo "Done. Backup saved to: $BACKUP_FILE"
