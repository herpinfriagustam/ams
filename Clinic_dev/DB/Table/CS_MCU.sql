CREATE TABLE CS_MCU
(
  MCU_NO     VARCHAR2(20 BYTE)                  NOT NULL,
  EMPID      VARCHAR2(10 BYTE)                  NOT NULL,
  PERIODE    VARCHAR2(4 BYTE),
  MCU_DATE   DATE,
  EMP_STAT   VARCHAR2(1 BYTE),
  PAKET      VARCHAR2(50 BYTE),
  KESIMP     VARCHAR2(500 BYTE),
  STATUS     VARCHAR2(200 BYTE),
  RIWAYAT    VARCHAR2(400 BYTE),
  TB         VARCHAR2(10 BYTE),
  BB         VARCHAR2(10 BYTE),
  TENSI      VARCHAR2(10 BYTE),
  VISUSKN    VARCHAR2(150 BYTE),
  VISUSKR    VARCHAR2(150 BYTE),
  BUTAWARNA  VARCHAR2(20 BYTE),
  KSMFISIK   VARCHAR2(400 BYTE),
  LABSMUA    VARCHAR2(400 BYTE),
  LABHEMA    VARCHAR2(400 BYTE),
  LABKIMIA   VARCHAR2(400 BYTE),
  LABURINE   VARCHAR2(400 BYTE),
  RONTGEN    VARCHAR2(400 BYTE),
  JANTUNG    VARCHAR2(400 BYTE),
  AUDIO      VARCHAR2(400 BYTE),
  SPIRO      VARCHAR2(400 BYTE),
  INS_DATE   DATE,
  INS_EMP    VARCHAR2(10 BYTE),
  UPD_DATE   DATE,
  UPD_EMP    VARCHAR2(10 BYTE),
  BMI        VARCHAR2(10 BYTE)
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


CREATE INDEX CS_MCU_IDX_01 ON CS_MCU
(MCU_NO, EMPID, PERIODE, MCU_DATE, EMP_STAT)
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