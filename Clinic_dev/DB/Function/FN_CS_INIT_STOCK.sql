CREATE OR REPLACE FUNCTION TTIT.FN_CS_INIT_STOCK ( P_DATE DATE
                                                  , P_MED_CD VARCHAR2) RETURN NUMBER 
IS 
v_return_value NUMBER;
v_check VARCHAR2(1);

 BEGIN  
       BEGIN
       
        BEGIN
            select stock into v_return_value
            from cs_medicine_stok
            where period=to_char(add_months(P_DATE,-1),'yyyymm')
            and med_cd=P_MED_CD;
        EXCEPTION
            WHEN NO_DATA_FOUND THEN
                 v_check := 'N';
            WHEN OTHERS THEN
                 v_check := 'N';
        END;
        
        if (v_check = 'N') then
        
            select nvl(sum(trans_qty),0) trans_qty  into v_return_value
            from cs_medicine_trans
            where trans_type='IN'
            and med_cd=P_MED_CD
            and to_char(trans_date,'yyyymm')=to_char(add_months(P_DATE,-1),'yyyymm');
        
        end if;
        
        EXCEPTION
                WHEN OTHERS THEN
                    v_return_value := 0;  
        END;
          
        RETURN v_return_value;
 
END FN_CS_INIT_STOCK;
/