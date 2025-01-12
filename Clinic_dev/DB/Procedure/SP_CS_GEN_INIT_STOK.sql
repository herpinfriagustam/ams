CREATE OR REPLACE PROCEDURE TTIT.SP_CS_GEN_INIT_STOK
AS

BEGIN

    delete from cs_medicine_stok_tmp;    

    DELETE FROM cs_medicine_stok where period = to_char(LAST_DAY(ADD_MONTHS(trunc(sysdate), -1)),'yyyymm');

    insert into cs_medicine_stok_tmp
    select to_char(LAST_DAY(ADD_MONTHS(trunc(sysdate), -1)),'yyyymm') period, 
    med_cd, stok, null expire_date, sysdate ins_date, 'SYSTEM' ins_emp,
    null upd_date, null upd_emp from (  
    select med_cd, 
    TTIT.FN_CS_INIT_STOCK(LAST_DAY(ADD_MONTHS(trunc(sysdate), -1)),med_cd) + 
    TTIT.FN_CS_TRX_IN(LAST_DAY(ADD_MONTHS(trunc(sysdate), -1)),med_cd) -  
    TTIT.FN_CS_TRX_OUT(LAST_DAY(ADD_MONTHS(trunc(sysdate), -1)),med_cd) -  
    TTIT.FN_CS_REQ_STOCK(LAST_DAY(ADD_MONTHS(trunc(sysdate), -1)),med_cd) as stok
    from cs_medicine ) a where 1=1 
    order by med_cd;
    
    insert into cs_medicine_stok 
    select * from cs_medicine_stok_tmp;
      
    COMMIT;
EXCEPTION
   WHEN NO_DATA_FOUND THEN ROLLBACK;
   
END;
/