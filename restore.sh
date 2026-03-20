#!/usr/bin/env bash
set -euo pipefail

BACKUP_FILE="${1:?Usage: ./restore.sh <backup-file>}"

if [ ! -f "$BACKUP_FILE" ]; then
  echo "Error: file '$BACKUP_FILE' not found"
  exit 1
fi

# Load credentials from .env
if [ -f .env ]; then
  # shellcheck disable=SC2046
  export $(grep -v '^#' .env | xargs)
fi

POSTGRES_DB="${POSTGRES_DB:-splitclaude}"
POSTGRES_USER="${POSTGRES_USER:-postgres}"

echo "Restoring '$BACKUP_FILE' into database '$POSTGRES_DB'..."
echo "WARNING: This will overwrite all existing data."
read -r -p "Continue? [y/N] " confirm
[[ "$confirm" =~ ^[Yy]$ ]] || { echo "Aborted."; exit 1; }

# Drop and recreate public schema to clear existing data
docker compose exec -T db psql -U "$POSTGRES_USER" -d "$POSTGRES_DB" \
  -c "DROP SCHEMA public CASCADE; CREATE SCHEMA public;"

# Restore from backup
docker compose exec -T db psql -U "$POSTGRES_USER" "$POSTGRES_DB" < "$BACKUP_FILE"
echo "Restore complete. Restart the stack if it is currently running."
