
-- Adicionar colunas à tabela u_logs
ALTER TABLE u_logs ADD LogLevel VARCHAR(20) NULL; -- Info, Warning, Error, Debug, Critical
ALTER TABLE u_logs ADD SourceBank VARCHAR(100) NULL; -- Banco (BCI, BIM, FCB, etc)
ALTER TABLE u_logs ADD HttpMethod VARCHAR(10) NULL;
ALTER TABLE u_logs ADD HttpStatusCode INT NULL;
ALTER TABLE u_logs ADD ip VARCHAR(50) NULL;
ALTER TABLE u_logs ADD DurationMs INT NULL; -- Duração em milissegundos
ALTER TABLE u_logs ADD EndpointUrl VARCHAR(500) NULL;
ALTER TABLE u_logs ADD ProcessingStep VARCHAR(100) NULL; -- Ex: "LoadPayment", "CheckPayment", "UpdateStatus"


-- Adicionar colunas à tabela u_2b_payments_hs
ALTER TABLE u_2b_payments_hs ADD Canal INT NULL; -- 106=BCI, 107=BIM, etc
ALTER TABLE u_2b_payments_hs ADD CanalNome VARCHAR(50) NULL; -- BCI, BIM, FCB, MOZA, NEDBANK
ALTER TABLE u_2b_payments_hs ADD Tabela VARCHAR(10) NULL; -- PO, PD, OW, TB
