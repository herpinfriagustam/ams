CREATE OR REPLACE VIEW CS_TRANS_MED_V
(TRANS_DATE, TRANS_TYPE, MED_NAME, TRANS_QTY, TRANS_CD, 
 TRANS_REMARK, BATCH_NO, EXPIRE_DATE, RECEIPT_ID, RM_NO)
AS 
select trans_date, trans_type, initcap(med_name) med_name,
trans_qty, trans_cd, trans_remark, batch_no, expire_date, b.receipt_id, c.rm_no
from cs_medicine a
join cs_medicine_trans b on (a.med_cd=b.med_cd)
left join cs_receipt c on(b.receipt_id=c.receipt_id)
where a.status='A'
/

