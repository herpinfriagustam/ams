CREATE TABLE CS_MEDICINE_TRANS
(
  TRANS_ID      NUMBER                          NOT NULL,
  MED_CD        VARCHAR2(10 BYTE)               NOT NULL,
  TRANS_TYPE    VARCHAR2(3 BYTE)                NOT NULL,
  TRANS_DATE    DATE,
  TRANS_QTY     NUMBER,
  BATCH_NO      VARCHAR2(30 BYTE),
  EXPIRE_DATE   DATE,
  TRANS_REMARK  VARCHAR2(30 BYTE),
  INS_DATE      DATE,
  INS_EMP       VARCHAR2(10 BYTE),
  UPD_DATE      DATE,
  UPD_EMP       VARCHAR2(10 BYTE),
  RECEIPT_ID    NUMBER,
  TRANS_CD      VARCHAR2(3 BYTE)
)
TABLESPACE TS_ITASSET_DATA
PCTUSED    40
PCTFREE    10
INITRANS   1
MAXTRANS   255
STORAGE    (
            INITIAL          64K
            NEXT             1M
            MINEXTENTS       1
            MAXEXTENTS       2147483645
            PCTINCREASE      0
            FREELISTS        1
            FREELIST GROUPS  1
            BUFFER_POOL      DEFAULT
           )
LOGGING 
NOCOMPRESS 
NOCACHE
NOPARALLEL
MONITORING;


CREATE INDEX CS_MEDICINE_TRANS_IDX_01 ON CS_MEDICINE_TRANS
(TRANS_ID, MED_CD, TRANS_TYPE, TRANS_DATE, TRANS_QTY, 
RECEIPT_ID)
LOGGING
TABLESPACE TS_ITASSET_DATA
PCTFREE    10
INITRANS   2
MAXTRANS   255
STORAGE    (
            INITIAL          64K
            NEXT             1M
            MINEXTENTS       1
            MAXEXTENTS       2147483645
            PCTINCREASE      0
            FREELISTS        1
            FREELIST GROUPS  1
            BUFFER_POOL      DEFAULT
           )
NOPARALLEL;


CREATE INDEX CS_MEDICINE_TRANS_IDX_02 ON CS_MEDICINE_TRANS
(EXPIRE_DATE, TRANS_CD)
LOGGING
TABLESPACE TS_ITASSET_DATA
PCTFREE    10
INITRANS   2
MAXTRANS   255
STORAGE    (
            INITIAL          64K
            NEXT             1M
            MINEXTENTS       1
            MAXEXTENTS       2147483645
            PCTINCREASE      0
            FREELISTS        1
            FREELIST GROUPS  1
            BUFFER_POOL      DEFAULT
           )
NOPARALLEL;