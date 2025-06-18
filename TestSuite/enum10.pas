type 
  Days = (mon,Tue,Wed,Thi,Fri,Sat,Sun);

procedure MyPrint(obj: object);
begin
  assert(obj.ToString() = 'Fri');
end;  
begin
  var en: System.Enum;
  en := fri;
  MyPrint(en);
end.