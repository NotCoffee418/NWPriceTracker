BEGIN;
-- INFO:
-- New files will be run as new revision automatically after registering in Dockerfile
CREATE TABLE item
(
    id                  serial                          PRIMARY KEY,
    name                varchar(12)                     NOT NULL,
    alias               varchar(12)                     NOT NULL,
    type                varchar(12)                     NOT NULL,
    category            varchar(12)                     NOT NULL,
    description         varchar(12)                     NOT NULL,
    rarity              varchar(12)                     NOT NULL,
    weight              decimal(8,2),
    icon                varchar(12)                     NOT NULL
);

CREATE TYPE area AS ENUM (
    'Brightwood',
    'Cutlass Keys',
    'Ebonscale Reach',
    'Edengrove',
    'Everfall',
    'First Light',
    'Great Cleave',
    'Monarchs Bluffs',
    'Mourningdale',
    'Reekwater',
    'Restless Shore',
    'Shattered Mountain',
    'Weavers Fen'
);

CREATE TABLE priceentry
(
    id                  bigserial                       PRIMARY KEY,
    targetitemid        integer                         NOT NULL,
    targetarea          area                            NOT NULL,
    price               decimal(18,2)                   NOT NULL,
    updatedtime         timestamp                       NOT NULL,

	
    -- FK: item
    CONSTRAINT priceentry_item_fk FOREIGN KEY (targetitemid)
        REFERENCES item ("id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,

    -- Unique pair
    UNIQUE (targetitemid, targetarea)
);

COMMIT;