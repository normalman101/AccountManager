CREATE TYPE UserRole AS ENUM ('normal', 'admin');

CREATE TABLE table_users
(
    id         UUID PRIMARY KEY  DEFAULT uuidv7(),
    email      TEXT     NOT NULL,
    role       UserRole NOT NULL,
    is_deleted BOOLEAN  NOT NULL DEFAULT FALSE,
    CONSTRAINT check_email CHECK ( length(email) > 0 )
);

CREATE UNIQUE INDEX unique_user_email
ON table_users (email)
WHERE (is_deleted = FALSE);

CREATE TABLE table_passwords
(
    password TEXT NOT NULL,
    user_id  UUID NOT NULL,
    CONSTRAINT check_password CHECK ( length(password) > 0 ),
    CONSTRAINT unique_user_id UNIQUE (user_id),
    CONSTRAINT foreign_key_user_id FOREIGN KEY (user_id) REFERENCES table_users (id)
);