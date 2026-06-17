CREATE TYPE Role AS ENUM ('normal', 'admin');

CREATE TABLE table_accounts
(
    email      TEXT PRIMARY KEY NOT NULL,
    role       Role             NOT NULL,
    is_deleted BOOLEAN          NOT NULL DEFAULT FALSE
);

CREATE TABLE table_passwords
(
    password   TEXT NOT NULL,
    account_email TEXT NOT NULL,
    CONSTRAINT relate_to_account UNIQUE (account_email),
    CONSTRAINT foreign_key_account_email FOREIGN KEY (account_email) REFERENCES table_accounts (email)
);

CREATE VIEW view_accounts AS
SELECT table_accounts.email, table_accounts.role, table_passwords.password
FROM table_accounts
JOIN table_passwords ON table_accounts.email = table_passwords.account_email
WHERE table_accounts.is_deleted = FALSE;