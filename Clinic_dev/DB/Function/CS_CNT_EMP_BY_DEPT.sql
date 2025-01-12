CREATE OR REPLACE FUNCTION TTIT.CS_CNT_EMP_BY_DEPT( P_DEPT VARCHAR2) RETURN NUMBER 
IS v_return_value NUMBER;
BEGIN  
       
     BEGIN
     
        SELECT COUNT (0)
          INTO v_return_value
          FROM cs_employees a JOIN view_eam100_s1@dl_ttergtotthcmif b
               ON (a.deptcd = b.deptcd)
         WHERE retire_dt IS NULL AND parent_dept = P_DEPT;

      EXCEPTION
        WHEN OTHERS THEN
                    v_return_value := 0;  
        END;
             
        RETURN v_return_value;
 
END CS_CNT_EMP_BY_DEPT;
/