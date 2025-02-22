CREATE TABLE CS_DIAGNOSA
(
  DIAGNOSA_ID    NUMBER                         NOT NULL,
  RM_NO          VARCHAR2(20 BYTE)              NOT NULL,
  INSP_DATE      DATE                           NOT NULL,
  ITEM_CD        VARCHAR2(10 BYTE),
  TYPE_DIAGNOSA  VARCHAR2(10 BYTE),
  REMARK         VARCHAR2(100 BYTE),
  INS_DATE       DATE,
  INS_EMP        VARCHAR2(10 BYTE),
  VISIT_NO       VARCHAR2(10 BYTE),
  UPD_DATE       DATE,
  UPD_EMP        VARCHAR2(10 BYTE),
  NOTED          VARCHAR2(100 BYTE)
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


CREATE INDEX CS_DIAGNOSA_IDX_01 ON CS_DIAGNOSA
(DIAGNOSA_ID, RM_NO, INSP_DATE, ITEM_CD, TYPE_DIAGNOSA, 
VISIT_NO)
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