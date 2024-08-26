DO
$$
BEGIN
  IF NOT EXISTS (SELECT FROM pg_database WHERE datname = 'testdb') THEN
    CREATE DATABASE testdb;
  END IF;
END
$$;

\connect testdb;

CREATE TABLE IF NOT EXISTS messages (
    id SERIAL PRIMARY KEY,
    content VARCHAR(128) NOT NULL,
    timestamp TIMESTAMPTZ DEFAULT CURRENT_TIMESTAMP
);