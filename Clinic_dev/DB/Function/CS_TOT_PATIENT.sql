CREATE OR REPLACE FUNCTION TTIT.CS_TOT_PATIENT ( P_GROUP VARCHAR2) RETURN NUMBER 
IS v_return_value NUMBER;
 BEGIN  
       BEGIN
       
        SELECT COUNT (0) into v_return_value
               FROM cs_patient a
              WHERE 1 = 1 AND status = 'A' 
                AND group_patient = P_GROUP;
        
        EXCEPTION
        WHEN OTHERS THEN
                    v_return_value := 0;  
        END;
          
        RETURN v_return_value;
 
END CS_TOT_PATIENT;
/