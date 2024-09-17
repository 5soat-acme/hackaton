CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240917013409_MedicoInicial') THEN
    CREATE TABLE "Medicos" (
        "Id" uuid NOT NULL,
        "Nome" varchar(100) NOT NULL,
        "Cpf" varchar(11) NOT NULL,
        "NroCrm" varchar(15) NOT NULL,
        "Email" varchar(100) NOT NULL,
        "Senha" varchar(20) NOT NULL,
        CONSTRAINT "PK_Medicos" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240917013409_MedicoInicial') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20240917013409_MedicoInicial', '8.0.0');
    END IF;
END $EF$;
COMMIT;

