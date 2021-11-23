CREATE TABLE "bludo" (
  "id" INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  "name" TEXT NOT NULL
);

CREATE TABLE "howto" (
  "id" INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  "id_bludo" integer NOT NULL,
  "text" TEXT NOT NULL,
  CONSTRAINT "fk_howto_bludo_1" FOREIGN KEY ("id_bludo") REFERENCES "bludo" ("id") ON DELETE CASCADE
);

CREATE TABLE "products" (
  "id" INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  "name" TEXT NOT NULL,
  "kkl" integer NOT NULL
);

CREATE TABLE "usedproducts" (
  "id" INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  "id_bludo" integer NOT NULL,
  "id_product" integer NOT NULL,
  "gramm" integer NOT NULL,
  CONSTRAINT "fk_usedproducts_products_1" FOREIGN KEY ("id_product") REFERENCES "products" ("id") ON DELETE RESTRICT,
  CONSTRAINT "fk_usedproducts_bludo_1" FOREIGN KEY ("id_bludo") REFERENCES "bludo" ("id") ON DELETE CASCADE
);

