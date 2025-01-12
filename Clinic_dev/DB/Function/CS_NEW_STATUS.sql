CREATE OR REPLACE FUNCTION TTIT.CS_NEW_STATUS ( P_TYPE VARCHAR2) RETURN NUMBER 
IS v_return_value NUMBER;
BEGIN  

    IF P_TYPE = 'PREG' THEN
    
       BEGIN
        SELECT COUNT (DISTINCT a.empid)
          INTO v_return_value
          FROM cs_visit a JOIN cs_patient b ON (a.empid = b.empid)
               JOIN cs_anamnesa c
               ON (    b.rm_no = c.rm_no
                   AND TRUNC (a.visit_date) = c.insp_date
                   AND a.que01 = c.visit_no
                  )
         WHERE TO_CHAR (visit_date, 'yyyy-mm') = TO_CHAR (SYSDATE, 'yyyy-mm')
           AND a.status = 'CLS'
           AND b.status = 'A'
           AND poli_cd = 'POL0002'
           AND group_patient = 'PREG'
           AND info_k = 'K1';

       EXCEPTION
            WHEN OTHERS THEN
                        v_return_value := 0;  
       END;

    ELSIF P_TYPE = 'FAMP' THEN
    
      BEGIN
       SELECT COUNT (DISTINCT a.empid)
         INTO v_return_value
         FROM cs_visit a JOIN cs_patient b ON (a.empid = b.empid)
        WHERE TO_CHAR (visit_date, 'yyyy-mm') = TO_CHAR (SYSDATE, 'yyyy-mm')
          AND a.status = 'CLS'
          AND b.status = 'A'
          AND poli_cd = 'POL0003'
          AND group_patient = 'FAMP';

      EXCEPTION
          WHEN OTHERS THEN
                      v_return_value := 0;  
      END;

    END IF;
   
    RETURN v_return_value;
 
END CS_NEW_STATUS;
/