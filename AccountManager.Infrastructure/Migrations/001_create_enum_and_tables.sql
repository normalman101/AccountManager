CREATE TYPE Role AS ENUM ('normal', 'admin');

CREATE TABLE table_accounts
(
    email      TEXT PRIMARY KEY NOT NULL,
    role       Role             NOT NULL,
    is_deleted BOOLEAN          NOT NULL DEFAULT FALSE
);

CREATE UNIQUE INDEX unique_account_email
    ON table_accounts (email)
    WHERE (is_deleted = FALSE);

CREATE TABLE table_passwords
(
    password   TEXT NOT NULL,
    account_email TEXT NOT NULL,
    CONSTRAINT relate_to_account UNIQUE (account_email),
    CONSTRAINT foreign_key_account_email FOREIGN KEY (account_email) REFERENCES table_accounts (email)
);