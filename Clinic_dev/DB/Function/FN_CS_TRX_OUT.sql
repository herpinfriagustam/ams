CREATE OR REPLACE FUNCTION TTIT.FN_CS_TRX_OUT ( P_DATE DATE
                                                  , P_MED_CD VARCHAR2) RETURN NUMBER 
IS v_return_value NUMBER;
 BEGIN  
       BEGIN
       

        select nvl(sum(trans_qty),0) into v_return_value
        from cs_medicine_trans
        where trans_type='OUT'
        and med_cd=P_MED_CD
        and to_char(trans_date,'yyyymm')=to_char(P_DATE,'yyyymm');
        
        EXCEPTION
        WHEN OTHERS THEN
                    v_return_value := 0;  
        END;
          
        RETURN v_return_value;
 
END FN_CS_TRX_OUT;
/