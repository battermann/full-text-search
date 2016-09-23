-- CREATE
CREATE TABLE Posts
(
	[Id] INT NOT NULL,
	[Title] VARBINARY(MAX) NOT NULL,
	[Content] VARBINARY(MAX) NOT NULL,
	[FileExtension]  AS ('.html'),
	CONSTRAINT Pk_Posts PRIMARY KEY (Id)
)

CREATE FULLTEXT CATALOG [posts_catalog]

CREATE FULLTEXT INDEX ON Posts
(
	[Title] TYPE COLUMN FileExtension, [Content] TYPE COLUMN FileExtension
)
	KEY INDEX Pk_Posts ON posts_catalog

--INSERT DATA
INSERT INTO Posts
VALUES 
	(1, CONVERT(VARBINARY(MAX), 'What are SOLID Principles?'), CONVERT(VARBINARY(MAX), '<p>SOLID is an acronym that stands for five basic principles in software design.</p><ul><li><strong>S</strong>ingle responsibility principle</li><li><strong>O</strong>pen/closed principle</li><li><strong>L</strong>iskov substitution principle</li><li><strong>I</strong>nterface segregation principle</li><li><strong>D</strong>ependency inversion principle</li></ul>')),
	(2, CONVERT(VARBINARY(MAX), '3 Facts About Cats And Mice'), CONVERT(VARBINARY(MAX), '<ol><li>Urna nunc in ut, vut magna. Adipiscing, in habitasse enim! Mus! Rhoncus placerat ultricies, amet arcu arcu placerat, tincidunt sociis.</li><li>Facilisis, phasellus tortor? Placerat mus augue nisi nisi tempor, velit dapibus aenean odio lorem a! Tristique augue cras pellentesque.</li><li>Ut urna lundium integer sed phasellus pid? Dignissim a. Est hac? Platea turpis enim amet arcu scelerisque vut auctor.</li></ol>')),
	(3, CONVERT(VARBINARY(MAX), 'Tortor ac elementum? Augue ut ac, sit.'), CONVERT(VARBINARY(MAX), '<p>Sed natoque pulvinar et mauris etiam auctor dolor in, et augue etiam habitasse sit amet augue dictumst nisi magna arcu pulvinar, cursus mid cursus placerat dapibus ut risus eu amet? Arcu? Urna, mid? Penatibus quis adipiscing ultrices lectus in, nisi.</p><p>Magnis? Ridiculus mattis, amet, vel lectus vut! Elementum elementum. Sociis urna. Porttitor integer, amet augue non, cursus, placerat elit, ultricies scelerisque scelerisque sed, velit cras rhoncus dolor scelerisque in mus a. Odio magna scelerisque porttitor sagittis odio turpis habitasse.</p><p>Aenean turpis dis, velit dictumst. Nec porttitor integer habitasse platea rhoncus, penatibus adipiscing scelerisque a porttitor tempor quis lectus aliquet cursus aenean cum platea, sagittis placerat! Ultrices, purus dis? Integer, odio mid aenean ridiculus pulvinar, nisi in scelerisque! Amet.</p>')),
	(4, CONVERT(VARBINARY(MAX), 'German translation of &quot;mice&quot;'), CONVERT(VARBINARY(MAX), '<table class=''table''><tr><th>English</th><th>German</th></tr><tr><td>mice</td><td>M&auml;use</td></tr></table>'))


-- REMOVE
DELETE FROM Posts
DROP TABLE Posts
DROP FULLTEXT CATALOG posts_catalog