DROP TABLE IF EXISTS Positions;
DROP TABLE IF EXISTS Quotes;
DROP TABLE IF EXISTS Operations;
DROP TABLE IF EXISTS Assets;
DROP TABLE IF EXISTS Users;

CREATE TABLE Users (
	id INT AUTO_INCREMENT PRIMARY KEY,
	name VARCHAR(100) NOT NULL,
	email VARCHAR(100) NOT NULL UNIQUE,
	brokerage_fee DECIMAL(10, 2) NOT NULL
);

CREATE TABLE Assets (
	id INT AUTO_INCREMENT PRIMARY KEY,
	code VARCHAR(10) NOT NULL UNIQUE,
	name VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE Operations (
	id INT AUTO_INCREMENT PRIMARY KEY,
	user_id INTEGER NOT NULL,
	asset_id INTEGER NOT NULL,
	quantity DECIMAL(10, 2) NOT NULL,
	price DECIMAL(10, 2) NOT NULL,
	operation_type VARCHAR(10) NOT NULL,
	brokerage_fee DECIMAL(10, 2) NOT NULL,
	date_time TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
	FOREIGN KEY (user_id) REFERENCES Users(id),
	FOREIGN KEY (asset_id) REFERENCES Assets(id)
);

CREATE TABLE Quotes (
	id CHAR(36) AUTO_INCREMENT PRIMARY KEY,
	asset_id INTEGER NOT NULL,
	price DECIMAL(10, 2) NOT NULL,
	date_time TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
	FOREIGN KEY (asset_id) REFERENCES Assets(id)
);

CREATE TABLE Positions (
	id INT AUTO_INCREMENT PRIMARY KEY,
	user_id INTEGER NOT NULL,
	asset_id INTEGER NOT NULL,
	quantity DECIMAL(10, 2) NOT NULL,
	average_price DECIMAL(10, 2) NOT NULL,
	p_and_l DECIMAL(10, 2) NOT NULL,
	FOREIGN KEY (user_id) REFERENCES Users(id),
	FOREIGN KEY (asset_id) REFERENCES Assets(id)
);

CREATE INDEX idx_operations_user_asset_date ON Operations(user_id, asset_id, date_time DESC);

INSERT INTO Users (name, email, brokerage_fee) VALUES 
('Alice', 'alice@example.com', 5.00),
('Bob', 'bob@example.com', 4.50),
('Carlos', 'carlos@example.com', 6.00),
('Diana', 'diana@example.com', 4.75);

INSERT INTO Assets (code, name) VALUES 
('PETR4', 'Petrobras PN'),
('VALE3', 'Vale ON'),
('ITUB4', 'Itaú Unibanco PN'),
('BBDC4', 'Bradesco PN');

INSERT INTO Operations (user_id, asset_id, quantity, price, operation_type, brokerage_fee, date_time) VALUES 
(1, 1, 100, 28.50, 'BUY', 5.00, NOW()),
(1, 2, 50, 72.10, 'BUY', 5.00, NOW()),
(2, 1, 150, 27.80, 'BUY', 4.50, NOW()),
(3, 3, 200, 24.30, 'BUY', 6.00, NOW()),
(4, 4, 120, 17.50, 'BUY', 4.75, NOW()),
(1, 1, 100, 30.00, 'SELL', 5.00, NOW());

INSERT INTO Quotes (id, asset_id, price, date_time) VALUES 
(UUID(), 1, 29.30, NOW()),
(UUID(), 2, 73.00, NOW()),
(UUID(), 3, 25.10, NOW()),
(UUID(), 4, 18.20, NOW()),
(UUID(), 1, 30.15, NOW());

INSERT INTO Positions (user_id, asset_id, quantity, average_price, p_and_l) VALUES 
(1, 1, 100, 28.50, 80.00),
(1, 2, 50, 72.10, 45.00),
(2, 1, 150, 27.80, 225.00),
(3, 3, 200, 24.30, 160.00),
(4, 4, 120, 17.50, 84.00);