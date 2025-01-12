CREATE OR REPLACE FUNCTION TTIT.CS_PREG_STATUS2 ( P_DATE VARCHAR2) RETURN NUMBER 
IS v_return_value NUMBER;
BEGIN  
       
     BEGIN
        SELECT COUNT (0) INTO v_return_value
          FROM (SELECT CASE
                          WHEN TO_DATE (P_DATE, 'yyyy-mm-dd')
                                 BETWEEN TO_DATE (info05, 'yyyy-mm-dd')
                                     AND TO_DATE (info09, 'yyyy-mm-dd')
                             THEN 'A'
                          ELSE 'I'
                       END as aktif
                  FROM cs_patient a
                 WHERE group_patient = 'PREG')
         WHERE aktif = 'A';

      EXCEPTION
        WHEN OTHERS THEN
                    v_return_value := 0;  
        END;
             
        RETURN v_return_value;
 
END CS_PREG_STATUS2;
/