CREATE OR REPLACE PROCEDURE TTIT.sp_cs_gen_patient
AS
BEGIN
   INSERT INTO cs_patient
      SELECT    'C'
             || REPLACE (empid, 'T', '')
             || TO_CHAR (SYSDATE, 'yymmdd') rm_no,
             empid, 'COMM', '', '', '', '', '', '', '', '', '', '', '', '',
             '', '', '', 'A' status, SYSDATE ins_date, 'TT17100003' ins_emp,
             '', ''
        FROM cs_employees a
       WHERE retire_dt IS NULL AND empid NOT IN (SELECT empid
                                                   FROM cs_patient);

   COMMIT;
EXCEPTION
   WHEN NO_DATA_FOUND
   THEN
      ROLLBACK;
END;
/