DELETE FROM transfers;
DELETE FROM accounts;
DELETE FROM users;
DELETE FROM transfer_types;
DELETE FROM transfer_statuses;

SET IDENTITY_INSERT transfer_statuses ON

INSERT INTO transfer_statuses (transfer_status_id, transfer_status_desc) VALUES (2000, 'Pending');
INSERT INTO transfer_statuses (transfer_status_id, transfer_status_desc) VALUES (2001, 'Approved');
INSERT INTO transfer_statuses (transfer_status_id, transfer_status_desc) VALUES (2002, 'Rejected');

SET IDENTITY_INSERT transfer_statuses OFF

SET IDENTITY_INSERT transfer_types ON

INSERT INTO transfer_types (transfer_type_id, transfer_type_desc) VALUES (1000, 'Request');
INSERT INTO transfer_types (transfer_type_id, transfer_type_desc) VALUES (1001, 'Send');

SET IDENTITY_INSERT transfer_types OFF

SET IDENTITY_INSERT users ON 

INSERT INTO users (user_id, username, password_hash, salt) VALUES (3000, 'Alice', 'x3f5DSXUim3xbxUMdNZZrK1wLZE=', '4sYRXdlbFwg=');
INSERT INTO users (user_id, username, password_hash, salt) VALUES (3001, 'Bob', 'j9de7Uq1Kj03eybDQDBv9UrlahA=', 'p8r5Vr6fY+E=');
INSERT INTO users (user_id, username, password_hash, salt) VALUES (3002, 'Carol', 'PtFGCqh1+Sela43/gllUtganEZQ=', '8ZicVCJCjrc=')

SET IDENTITY_INSERT users OFF

SET IDENTITY_INSERT accounts ON

INSERT INTO accounts (account_id, user_id, balance) VALUES (4000, 3000, 1000.00);
INSERT INTO accounts (account_id, user_id, balance) VALUES (4001, 3001, 1000.00);
INSERT INTO accounts (account_id, user_id, balance) VALUES (4002, 3002, 1000.00);

SET IDENTITY_INSERT accounts OFF

SET IDENTITY_INSERT transfers ON

INSERT INTO transfers (transfer_id, transfer_type_id, transfer_status_id, account_from, account_to, amount) VALUES (1, 1001, 2001, 4000, 4001, 100.00);
INSERT INTO transfers (transfer_id, transfer_type_id, transfer_status_id, account_from, account_to, amount) VALUES (2, 1000, 2000, 4000, 4001, 100.00);
INSERT INTO transfers (transfer_id, transfer_type_id, transfer_status_id, account_from, account_to, amount) VALUES (3, 1000, 2002, 4000, 4001, 100.00);
INSERT INTO transfers (transfer_id, transfer_type_id, transfer_status_id, account_from, account_to, amount) VALUES (4, 1000, 2002, 4001, 4002, 100.00);

SET IDENTITY_INSERT transfers OFF