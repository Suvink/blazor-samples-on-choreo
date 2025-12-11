#!/bin/bash
set -e

echo "=========================================="
echo "Starting BookStore Single Component"
echo "=========================================="

# Run database migrations
echo "Running database migrations..."
cd /app/migrator
dotnet Acme.BookStore.DbMigrator.dll

if [ $? -eq 0 ]; then
    echo "✅ Database migrations completed successfully"
else
    echo "❌ Database migrations failed"
    exit 1
fi

echo "=========================================="
echo "Starting services with supervisor..."
echo "=========================================="

# Start supervisor to manage nginx, API, and Blazor
exec /usr/bin/supervisord -c /etc/supervisor/conf.d/supervisord.conf
