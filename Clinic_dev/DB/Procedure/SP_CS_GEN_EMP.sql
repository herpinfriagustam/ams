CREATE OR REPLACE PROCEDURE TTIT.SP_CS_GEN_EMP
AS

BEGIN

    DELETE FROM cs_employees;

    INSERT INTO cs_employees
       SELECT empid, rfid, replace(name,'''','') NAME, dept, line, POSITION, manager, birth_place,
              birth_date, age, gender, address, blood_type, height, weight,
              retire_dt, sysdate ins_date, 'SYSTEM' ins_emp, deptcd
         FROM tthcm.view_cl_emp@dl_ttergtotthcmif;
      
    COMMIT;
EXCEPTION
   WHEN NO_DATA_FOUND THEN ROLLBACK;
   
END;
/