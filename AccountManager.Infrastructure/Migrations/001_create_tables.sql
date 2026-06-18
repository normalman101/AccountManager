CREATE TABLE table_accounts
(
    email      TEXT PRIMARY KEY NOT NULL,
    is_deleted BOOLEAN          NOT NULL DEFAULT FALSE,
    role       INTEGER          NOT NULL
);

CREATE TABLE table_passwords
(
    password      TEXT NOT NULL,
    account_email TEXT NOT NULL,
    CONSTRAINT unique_account_email UNIQUE (account_email),
    CONSTRAINT foreign_key_account_email FOREIGN KEY (account_email) REFERENCES table_accounts (email)
);

CREATE VIEW view_accounts AS
SELECT table_accounts.email, table_accounts.role, table_passwords.password
FROM table_accounts
         INNER JOIN table_passwords ON table_accounts.email = table_passwords.account_email
WHERE table_accounts.is_deleted = FALSE;