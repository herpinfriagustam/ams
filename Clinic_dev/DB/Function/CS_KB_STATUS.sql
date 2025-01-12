CREATE OR REPLACE FUNCTION TTIT.CS_KB_STATUS ( P_DATE VARCHAR2) RETURN NUMBER 
IS v_return_value NUMBER;
BEGIN  
       
     BEGIN
        SELECT COUNT (0) INTO v_return_value
          FROM cs_patient a
         WHERE 1 = 1
           AND group_patient = 'FAMP'
           AND TRUNC (ins_date) <= LAST_DAY (TO_DATE (P_DATE, 'yyyy-mm'));

      EXCEPTION
        WHEN OTHERS THEN
                    v_return_value := 0;  
        END;
             
        RETURN v_return_value;
 
END CS_KB_STATUS;
/