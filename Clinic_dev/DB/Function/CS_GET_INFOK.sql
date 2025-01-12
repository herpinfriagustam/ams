CREATE OR REPLACE FUNCTION TTIT.CS_GET_INFOK ( P_RM VARCHAR2, P_K VARCHAR2) RETURN DATE 
IS v_return_value DATE;
 BEGIN  
       BEGIN
       
        SELECT MAX (insp_date) ddate 
          INTO v_return_value
          FROM cs_anamnesa
         WHERE rm_no = P_RM AND info_k = P_K;
        
        EXCEPTION
        WHEN OTHERS THEN
                    v_return_value := null;  
        END;
          
        RETURN v_return_value;
 
END CS_GET_INFOK;
/