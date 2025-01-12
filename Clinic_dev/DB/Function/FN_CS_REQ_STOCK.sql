CREATE OR REPLACE FUNCTION TTIT.FN_CS_REQ_STOCK ( P_DATE DATE
                                                , P_MED_CD VARCHAR2) RETURN NUMBER 
IS v_return_value NUMBER;
 BEGIN  
       BEGIN
       

        select nvl(SUM(med_qty),0) into v_return_value
        from cs_receipt
        where insp_date= P_DATE
        and confirm='N'
        and med_cd=P_MED_CD;
        
        EXCEPTION
        WHEN OTHERS THEN
                    v_return_value := 0;  
        END;
          
        RETURN v_return_value;
 
END FN_CS_REQ_STOCK;
/