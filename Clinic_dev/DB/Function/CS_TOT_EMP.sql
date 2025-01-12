CREATE OR REPLACE FUNCTION TTIT.CS_TOT_EMP ( P_GROUP VARCHAR2) RETURN NUMBER 
IS v_return_value NUMBER;
BEGIN  

  IF P_GROUP = 'ALL' THEN
  
       BEGIN
       
        SELECT COUNT (0) 
          INTO v_return_value
          FROM cs_employees a
         WHERE 1 = 1 
           AND retire_dt is null;
        
        EXCEPTION
        WHEN OTHERS THEN
                    v_return_value := 0;  
        END;
          
   ELSIF P_GROUP = 'LOCAL' THEN
   
       BEGIN
       
        SELECT COUNT (0) 
          INTO v_return_value
          FROM cs_employees a
         WHERE 1 = 1 
           AND retire_dt is null
           AND empid like 'TT%';
        
        EXCEPTION
        WHEN OTHERS THEN
                    v_return_value := 0;  
        END;
        
   ELSIF P_GROUP = 'EXP' THEN
   
       BEGIN
       
        SELECT COUNT (0) 
          INTO v_return_value
          FROM cs_employees a
         WHERE 1 = 1 
           AND retire_dt is null
           AND empid not like 'TT%';
        
        EXCEPTION
        WHEN OTHERS THEN
                    v_return_value := 0;  
        END;
        
    ELSE
    
    v_return_value := 0;
   
   END IF;
  
  
  RETURN v_return_value;
 
END CS_TOT_EMP;
/