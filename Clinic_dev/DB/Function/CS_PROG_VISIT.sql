CREATE OR REPLACE FUNCTION TTIT.CS_PROG_VISIT ( P_DATE VARCHAR2, P_PUR VARCHAR2) RETURN NUMBER 
IS v_return_value NUMBER;
BEGIN  

  IF P_PUR = '' OR P_PUR is null THEN
  
     BEGIN
       
        SELECT COUNT (0) 
          INTO v_return_value
          FROM cs_visit
         WHERE 1 = 1 
           AND to_char(visit_date,'yyyy-mm-dd')=P_DATE
           and status in ('PRE','RSV','INS','MED','OBS');
        
        EXCEPTION
        WHEN OTHERS THEN
                    v_return_value := 0;  
     END;
  
  ELSE
  
     BEGIN
       
        SELECT COUNT (0) 
          INTO v_return_value
          FROM cs_visit 
         WHERE 1 = 1 
           AND to_char(visit_date,'yyyy-mm-dd')=P_DATE
           and status in ('PRE','RSV','INS','MED','OBS')
           AND PURPOSE = P_PUR;
        
        EXCEPTION
        WHEN OTHERS THEN
                    v_return_value := 0;  
     END;
  
  END IF;
  
  RETURN v_return_value;
 
END CS_PROG_VISIT;
/