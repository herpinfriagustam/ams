CREATE OR REPLACE FUNCTION TTIT.CS_TOT_WA ( P_DATE VARCHAR2) RETURN NUMBER 
IS v_return_value NUMBER;
BEGIN  

     BEGIN
       
        SELECT COUNT (0) 
          INTO v_return_value
          FROM cs_visit 
         WHERE 1 = 1 
           AND to_char(visit_date,'yyyy-mm')=P_DATE
           AND work_accident='Y'
           AND status='CLS';
        
        EXCEPTION
        WHEN OTHERS THEN
                    v_return_value := 0;  
     END;
  
  RETURN v_return_value;
 
END CS_TOT_WA;
/