DELIMITER //

CREATE TRIGGER trg_update_positions_after_quote_update
AFTER INSERT ON Quotes
FOR EACH ROW
BEGIN
    UPDATE Positions
    SET p_and_l = (NEW.price - average_price) * quantity
    WHERE asset_id = NEW.asset_id;
END //

DELIMITER ;