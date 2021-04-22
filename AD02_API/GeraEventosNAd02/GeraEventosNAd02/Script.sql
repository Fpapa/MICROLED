/*
ALTER TABLE operador.tb_hist_shifting ADD SEQ_EVE_AD02_NEW NUMBER(12);
 
CREATE SEQUENCE operador.SEQ_EVE_AD02_NEW START WITH 1 INCREMENT BY +1 NOCACHE;


CREATE OR REPLACE VIEW OPERADOR.VW_AD02N_EVE11                (DATA,
                                                    yard,
                                                    origem,
                                                    autonumcntr,
                                                    id_conteiner,
                                                    viagem,
                                                    patio,
                                                    sistema,
                                                    ID,
                                                    valida,
                                                    autonum,
                                                    recinto,
                                                    isocode,
                                                    peso,
                                                    flag_historico,
                                                    tamanho,
                                                    flag_desova,
                                                    cnpjRecinto,
                                                    codeRecinto,
                                                    Avaria,
                                                    ConferenciaFisica,
                                                    Contingencia,
                                                    ListaCamera,
                                                    CPF
                                                   )
AS
  SELECT to_char(H.DATA,'YYYY-MM-DD') || 'T' || TO_CHAR(H.DATA,'HH24:MI:SS') || '.000Z-0300', UPPER (h.destino), h.origem, c.autonum AS autonumcntr,
         c.id_conteiner, c.viagem, TO_NUMBER (c.patio), h.tipo,  h.ROWID AS ID, NVL (y.valida, 0),
         h.autonum, r.recinto, vw.iso,
         ROUND (vw.peso_entrada - vw.peso_saida, 0), c.flag_historico,
         c.tamanho, c.flag_desovado, R2.CGC,r2.codeterminal,0,0,0,'[]',NVL(U.CPF,'00000000000')
    FROM operador.tb_hist_shifting h INNER JOIN sgipa.tb_cntr_bl c
         ON h.cntr = c.autonum
         LEFT JOIN operador.tb_yard y ON h.destino = y.yard
                                    AND c.patio = y.patio
         INNER JOIN
         (SELECT   MAX (h.autonum) AS autonum, h.tipo, h.cntr, h.destino
              FROM operador.tb_hist_shifting h INNER JOIN sgipa.tb_cntr_bl cc
                   ON h.cntr = cc.autonum
             WHERE h.DATA > SYSDATE - 150 AND h.tipo = 'I' AND cc.imo1 > '0'
          GROUP BY h.tipo, h.cntr, h.destino) h2 ON h.autonum = h2.autonum
         INNER JOIN sgipa.dte_tb_recinto_patio r ON c.patio = r.patio
         INNER JOIN operador.vw_dados_ei vw ON c.autonum = vw.cntr_ipa
         INNER JOIN OPERADOR.DTE_TB_RECINTOS R2 ON R.RECINTO=R2.CODE
         LEFT JOIN OPERADOR.TB_CAD_USUARIOS U ON H.USUARIO=U.AUTONUM
   WHERE (   (c.flag_historico = 0 AND h.destino <> 'SAIDA' AND h.destino <> 'NAVIO')
          OR (c.flag_historico = 1 AND h.destino = 'SAIDA' AND h.destino <> 'NAVIO')
          OR c.flag_desovado = 1
         )
     AND h.destino <> 'GATEIN'
     AND h.tipo = 'I'
     AND h.DATA > SYSDATE - 3
     AND NVL(SEQ_EVE_AD02_NEW,0)=0
  UNION ALL
  SELECT to_char(H.DATA,'YYYY-MM-DD') || 'T' || TO_CHAR(H.DATA,'HH24:MI:SS') || '.000Z-0300', UPPER (h.destino), h.origem, c.autonum AS autonumcntr,
         c.id_conteiner, v.viagem, TO_NUMBER (c.patio), h.tipo, h.ROWID AS ID, NVL (y.valida, 0),
         h.autonum, r.recinto,
         CASE NVL (vw.iso, 'X')
           WHEN 'X'
             THEN c.iso
           ELSE vw.iso
         END,
         CASE NVL (vw.peso_entrada, 0)
           WHEN 0
             THEN c.bruto
           ELSE ROUND (vw.peso_entrada - vw.peso_saida, 0)
         END,
         c.historico, c.tamanho, 0, R2.CGC,r2.codeterminal,0,0,0,'[]',NVL(U.CPF,'00000000000')
    FROM operador.tb_hist_shifting h INNER JOIN operador.tb_patio c
         ON h.cntr = c.autonum
         INNER JOIN operador.tb_viagens v ON c.autonumviagem = v.autonum
         LEFT JOIN operador.vw_dados_eo vw ON c.autonum = vw.patio_op
         LEFT JOIN operador.tb_yard y ON h.destino = y.yard
                                    AND c.patio = y.patio
         INNER JOIN
         (SELECT   MAX (h.autonum) AS autonum, tipo, cntr, destino
              FROM operador.tb_hist_shifting h INNER JOIN operador.tb_patio cc
                   ON h.cntr = cc.autonum
             WHERE h.DATA > SYSDATE - 3 AND h.tipo = 'O' 
          GROUP BY h.tipo, h.cntr, h.destino) h2 ON h.autonum = h2.autonum
         INNER JOIN sgipa.dte_tb_recinto_patio r ON c.patio = r.patio
         INNER JOIN OPERADOR.DTE_TB_RECINTOS R2 ON R.RECINTO=R2.CODE
         LEFT JOIN OPERADOR.TB_CAD_USUARIOS U ON H.USUARIO=U.AUTONUM
   WHERE (   (c.historico = 0 AND h.destino NOT IN ('SAIDA', 'NAVIO'))
          OR (c.historico = 1 AND h.destino IN ('SAIDA', 'NAVIO'))
         )
     AND h.destino NOT IN ('GATEIN')
     AND h.tipo = 'O'
     AND NVL(SEQ_EVE_AD02_NEW,0)=0
  UNION
  SELECT to_char(H.DATA,'YYYY-MM-DD') || 'T' || TO_CHAR(H.DATA,'HH24:MI:SS') || '.000Z-0300', UPPER (h.destino), h.origem, c.autonum_patio AS autonumcntr,
         c.id_conteiner, '', TO_NUMBER (c.patio), h.tipo, h.ROWID AS ID,
         NVL (y.valida, 0), h.autonum, r.recinto, gd.iso,
         CASE c.bruto
           WHEN 0
             THEN gd.tara
           ELSE c.bruto
         END, c.flag_historico, c.tamanho, 0, R2.CGC,r2.CODETERMINAL,0,0,0,'[]',NVL(U.CPF,'00000000000')
    FROM operador.tb_hist_shifting h INNER JOIN redex.tb_patio c
         ON h.cntr = c.autonum_patio
         LEFT JOIN operador.tb_yard y ON h.destino = y.yard
                                    AND c.patio = y.patio
         INNER JOIN
         (SELECT   MAX (h.autonum) AS autonum, tipo, cntr, destino
              FROM operador.tb_hist_shifting h
             WHERE h.DATA > SYSDATE - 3 AND tipo = 'R'
          GROUP BY tipo, cntr, destino) h2 ON h.autonum = h2.autonum
         INNER JOIN sgipa.dte_tb_recinto_patio r ON c.patio = r.patio
         INNER JOIN redex.vw_cntr_imo i ON c.autonum_patio = i.autonum_patio
         LEFT JOIN operador.tb_cad_onu d1 ON i.onu = d1.code
         INNER JOIN redex.tb_amr_gate ag ON c.autonum_patio = ag.cntr_rdx
         INNER JOIN redex.tb_gate_dados gd ON ag.autonum = gd.amr
         INNER JOIN OPERADOR.DTE_TB_RECINTOS R2 ON R.RECINTO=R2.CODE
         LEFT JOIN OPERADOR.TB_CAD_USUARIOS U ON H.USUARIO=U.AUTONUM
   WHERE (   (c.flag_historico = 0 AND h.destino <> 'SAIDA' AND h.destino <> 'NAVIO')
          OR (c.flag_historico = 1 AND h.destino = 'SAIDA')
         )
     AND h.destino NOT IN ('GATEIN')
     AND h.tipo = 'R'
     AND NVL(SEQ_EVE_AD02_NEW,0)=0

