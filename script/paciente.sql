CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240917011223_PacienteInicial') THEN
    CREATE TABLE "Pacientes" (
        "Id" uuid NOT NULL,
        "Nome" varchar(100) NOT NULL,
        "Cpf" varchar(11) NOT NULL,
        "Email" varchar(100) NOT NULL,
        "Senha" varchar(20) NOT NULL,
        CONSTRAINT "PK_Pacientes" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240917011223_PacienteInicial') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20240917011223_PacienteInicial', '8.0.0');
    END IF;
END $EF$;
COMMIT;

