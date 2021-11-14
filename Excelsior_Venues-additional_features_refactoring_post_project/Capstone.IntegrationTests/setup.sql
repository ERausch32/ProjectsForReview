-- Put steps here to set up your database in a default good state for testing

DELETE FROM reservation 
DELETE FROM category_venue
DELETE FROM category
DELETE FROM space
DELETE FROM venue
DELETE FROM city
DELETE FROM state



INSERT INTO state (abbreviation, name) VALUES ('MI', 'Michigan')

SET IDENTITY_INSERT city ON
INSERT INTO city (id, name, state_abbreviation) VALUES (1, 'Bona', 'MI');
SET IDENTITY_iNSERT city OFF

SET IDENTITY_INSERT category ON
INSERT INTO category (id, name) VALUES (1, 'Family Friendly');
INSERT INTO category (id, name) VALUES (2, 'Outdoors');
INSERT INTO category (id, name) VALUES (3, 'Historic');
INSERT INTO category (id, name) VALUES (4, 'Rustic');
INSERT INTO category (id, name) VALUES (5, 'Luxury');
INSERT INTO category (id, name) VALUES (6, 'Modern');
SET IDENTITY_INSERT category OFF

SET IDENTITY_INSERT venue ON
INSERT INTO venue (id, name, city_id, description) VALUES (1, 'The Bittersweet Elephant Tavern', 1, 'It''s like a zoo in here! This animal themed venue is a hoot. You can really go wild in this jungle.');
SET IDENTITY_INSERT venue OFF

SET IDENTITY_INSERT space ON
INSERT INTO space (id, venue_id, name, is_accessible, open_from, open_to, daily_rate, max_occupancy) VALUES (1, 1, 'Otter Offices', 0, NULL, NULL, '3800', 190);
INSERT INTO space (id, venue_id, name, is_accessible, open_from, open_to, daily_rate, max_occupancy) VALUES (2, 1, 'The Mousehole', 1, 4, 11, '5000', 250);
INSERT INTO space (id, venue_id, name, is_accessible, open_from, open_to, daily_rate, max_occupancy) VALUES (3, 1, 'Giraffe Gym', 1, NULL, NULL, '7500', 250);
INSERT INTO space (id, venue_id, name, is_accessible, open_from, open_to, daily_rate, max_occupancy) VALUES (4, 1, 'The Doghouse', 0, 9, 12, '1200', 40);
INSERT INTO space (id, venue_id, name, is_accessible, open_from, open_to, daily_rate, max_occupancy) VALUES (5, 1, 'Kitty Cat Conference Room', 0, NULL, NULL, '900', 60);
INSERT INTO space (id, venue_id, name, is_accessible, open_from, open_to, daily_rate, max_occupancy) VALUES (6, 1, 'Platypus Plotting Corner', 0, NULL, NULL, '1050', 1);
SET IDENTITY_INSERT space OFF


INSERT INTO category_venue (venue_id, category_id) VALUES (1, 1);
INSERT INTO category_venue (venue_id, category_id) VALUES (1, 2);


SET IDENTITY_INSERT reservation ON
INSERT INTO reservation (reservation_id, space_id, number_of_attendees, start_date, end_date, reserved_for) VALUES (1, 1, 125, '2021-10-03', '2021-10-15', 'Tricia Griffith');
INSERT INTO reservation (reservation_id, space_id, number_of_attendees, start_date, end_date, reserved_for) VALUES (2, 2, 200, '2021-11-05', '2021-11-12', 'Toby Cosgrove');
INSERT INTO reservation (reservation_id, space_id, number_of_attendees, start_date, end_date, reserved_for) VALUES (3, 3, 220, '2021-12-10', '2021-12-18', 'Akram Boutros');
SET IDENTITY_INSERT reservation OFF

SELECT * FROM space INNER JOIN venue ON space.venue_id = venue.id INNER JOIN reservation ON reservation.space_id = space.id


