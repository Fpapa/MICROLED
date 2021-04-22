CREATE USER AD02 IDENTIFIED BY AD02ECOPORTO;
GRANT RESOURCE TO AD02;
GRANT CONNECT TO AD02;


CREATE TABLE TB_LOG_EVENTOS (
    id NUMBER(10) DEFAULT 0 NOT NULL,
    Evento NUMBER(3) DEFAULT 0 NOT NULL,
    Envio_JSON varchar2(3000) NULL,
    Envio_dth date NULL,
    Retorno_dth date NULL,
    Retorno_JSON varchar2 (1000) NULL,
    Retorno_MSG varchar2 (1000) NULL,
    Retorno_codigo NUMBER(5) DEFAULT 0 NOT NULL,
    Id_anterior NUMBER(10) DEFAULT 0 NOT NULL,
    Protocolo varchar2(500) NULL,
    Tentativas NUMBER(3) DEFAULT 0 NOT NULL
)

CREATE SEQUENCE SEQ_LOG_EVENTOS START WITH 1 INCREMENT BY +1 NOCACHE;

ALTER TABLE TB_LOG_EVENTOS ADD CONSTRAINT PK_LOG_EVENTOS PRIMARY KEY (ID);

GRANT SELECT, INSERT, DELETE, UPDATE ON TB_LOG_EVENTOS TO OPERADOR;
GRANT SELECT, INSERT, DELETE, UPDATE ON TB_LOG_EVENTOS TO SGIPA;
GRANT SELECT, INSERT, DELETE, UPDATE ON TB_LOG_EVENTOS TO REDEX;


GRANT SELECT ON SEQ_LOG_EVENTOS TO OPERADOR;
GRANT SELECT ON SEQ_LOG_EVENTOS TO SGIPA;
GRANT SELECT ON SEQ_LOG_EVENTOS TO REDEX;