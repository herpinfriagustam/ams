CREATE OR REPLACE FUNCTION TTIT.CS_CNT_DIAG( P_DATE VARCHAR2, P_DIAG VARCHAR2, P_PLANT VARCHAR2) RETURN NUMBER 
IS v_return_value NUMBER;
BEGIN  
       
     BEGIN
     
     SELECT COUNT (DISTINCT a.empid) 
       INTO v_return_value
       FROM cs_visit a JOIN cs_patient b ON (a.empid = b.empid)
            JOIN cs_diagnosa c
            ON (    TRUNC (a.visit_date) = c.insp_date
               AND b.rm_no = c.rm_no
               AND a.que01 = c.visit_no
               )
            JOIN cs_employees d ON (a.empid = d.empid)
            JOIN view_eam100_s1@dl_ttergtotthcmif e ON (d.deptcd = e.deptcd)
      WHERE b.status = 'A'
        AND a.status = 'CLS'
        AND poli_cd IN ('POL0000', 'POL0001')
        AND c.item_cd NOT IN ('X11')
        AND type_diagnosa = 'P'
        AND c.item_cd = P_DIAG
        AND plant = P_PLANT
        AND TO_CHAR (visit_date, 'yyyymm') = P_DATE;

      EXCEPTION
        WHEN OTHERS THEN
                    v_return_value := 0;  
        END;
             
        RETURN v_return_value;
 
END CS_CNT_DIAG;
/