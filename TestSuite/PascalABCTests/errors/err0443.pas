//!Использование exit, break и continue в блоке finally недопустимо
begin
  for var i := 1 to 10 do
  try
    
  finally
    continue; 
  end;
end.