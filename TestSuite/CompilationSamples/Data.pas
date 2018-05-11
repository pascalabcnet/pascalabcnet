unit Data;

//обязательно params
procedure p2(params o: array of Exception) := exit;//не обязательно короткая процедура

procedure p1 :=//не обязательно короткая процедура
try
  if true then
    p2(nil);
except
  //обязательно on do и использование переменной исключения в процедуре с params
  on e: Exception do 
  
    p2(e);
  
end;

end.