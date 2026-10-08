-- Thêm menu "Thời gian chờ & thực hiện khám" cho DB ĐÃ có sẵn (không cần chạy lại gen_app_dab.sql)
-- Đặt cùng cấp với "Trung bình khám" (con của /khambenh) và cấp quyền cho các role đang có quyền "Trung bình khám".
DECLARE
  APP_ID number;
BEGIN
  select id into APP_ID from acs_application where application_code='DAB';

  insert into ACS_MODULE(MODULE_NAME,MODULE_LINK,ICON_LINK,IS_ANONYMOUS,MODULE_URL,APPLICATION_ID,IS_VISIBLE,NUM_ORDER,PARENT_ID)
  VALUES ('Thời gian chờ & thực hiện khám','/thoigiankham','fa fa-clock-o',NULL,'bg-green',APP_ID,1,1,
         (select max(id) from acs_module where module_link='/khambenh' and application_id=APP_ID));

  insert into acs_module_role (role_id,module_id)
  select distinct mr.role_id, (select max(id) from acs_module where module_link='/thoigiankham' and application_id=APP_ID)
  from acs_module_role mr
  where mr.module_id in (select id from acs_module where module_link='/trungbinhkham' and application_id=APP_ID);

  commit;
END;
/
