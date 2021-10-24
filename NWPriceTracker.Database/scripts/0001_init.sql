BEGIN;
-- INFO:
-- New files will be run as new revision automatically after registering in Dockerfile

-- item
CREATE TABLE item
(
    id                  integer                         PRIMARY KEY,
    name                varchar(128)                    NOT NULL,
    alias               varchar(128),
    type                varchar(32)                     NOT NULL,
    category            varchar(32),
    description         varchar(1024),
    rarity              varchar(32)                     NOT NULL,
    tier                integer,
    icon                varchar(256)
);
CREATE INDEX idx_item_name ON item(name);

-- priceentry
CREATE TABLE priceentry
(
    id                  bigserial                       PRIMARY KEY,
    targetitemid        integer                         NOT NULL,
    targetarea          integer                         NOT NULL,
    price               numeric(18,2)                   NOT NULL,
    updatedtime         timestamp                       NOT NULL,

	
    -- FK: item
    CONSTRAINT priceentry_item_fk FOREIGN KEY (targetitemid)
        REFERENCES item ("id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,

    -- Unique pair
    UNIQUE (targetitemid, targetarea)
);
CREATE INDEX idx_priceentry_priceentry ON priceentry(targetitemid);
CREATE INDEX idx_priceentry_targetarea ON priceentry(targetarea);


-- Procedures
CREATE PROCEDURE insert_update_item(
    _id integer,
    _name varchar(32),
    _alias varchar(32),
    _type varchar(32),
    _category varchar(32),
    _description varchar(1024),
    _rarity varchar(32),
    _tier integer,
    _icon varchar(64))
LANGUAGE SQL
AS $$
    INSERT INTO item (id, name, alias, type, category, description, rarity, tier, icon)
    VALUES (_id, _name, _alias, _type, _category, _description, _rarity, _tier, _icon)
    ON CONFLICT (id) DO 
    UPDATE SET
        name = _name,
        alias = _alias,
        type = _type,
        category = _category,
        description = _description,
        rarity = _rarity,
        tier = _tier,
        icon = _icon
$$;

COMMIT;