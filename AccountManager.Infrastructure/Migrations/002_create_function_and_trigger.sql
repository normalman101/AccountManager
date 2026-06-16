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