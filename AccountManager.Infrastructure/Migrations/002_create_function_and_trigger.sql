CREATE FUNCTION function_soft_delete_user()
    RETURNS TRIGGER AS
$$
BEGIN
    UPDATE table_users
    SET is_deleted = TRUE
    WHERE id = OLD.id;
    
    RETURN NULL;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_soft_delete_user
    BEFORE DELETE
    ON table_users
    FOR EACH ROW
EXECUTE FUNCTION function_soft_delete_user();