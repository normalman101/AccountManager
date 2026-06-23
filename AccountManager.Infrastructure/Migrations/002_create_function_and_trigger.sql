CREATE FUNCTION function_add_account(
    p_email TEXT,
    p_password TEXT,
    p_role INTEGER
)
    RETURNS BOOLEAN AS
$$
BEGIN
    INSERT INTO table_accounts(email, role)
    VALUES (p_email, p_role);

    INSERT INTO table_passwords(password, account_email)
    VALUES (p_password, p_email);

    RETURN FOUND;
END ;
$$ LANGUAGE plpgsql;

CREATE PROCEDURE procedure_update_account(
    p_account_old_email TEXT,
    p_new_account_email TEXT,
    p_new_account_password TEXT,
    p_new_account_role INTEGER
)
AS
$$
BEGIN
    DELETE
    FROM table_passwords
    WHERE account_email = p_account_old_email;

    UPDATE table_accounts
    SET email = p_new_account_email,
        role  = p_new_account_role
    WHERE email = p_account_old_email;

    INSERT INTO table_passwords(password, account_email)
    VALUES (p_new_account_password, p_new_account_email);
END;
$$ LANGUAGE plpgsql;

CREATE FUNCTION function_recover_account(
    p_email TEXT,
    p_new_password TEXT
)
    RETURNS BOOLEAN AS
$$
BEGIN
    UPDATE table_passwords
    SET password = p_new_password
    WHERE account_email = p_email;
    
    RETURN FOUND;
END;
$$ LANGUAGE plpgsql;

CREATE FUNCTION function_soft_delete_account()
    RETURNS TRIGGER AS
$$
BEGIN
    UPDATE table_accounts
    SET is_deleted = TRUE
    WHERE email = OLD.email;

    RETURN NULL;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_soft_delete_account
    BEFORE DELETE
    ON table_accounts
    FOR EACH ROW
EXECUTE FUNCTION function_soft_delete_account();

