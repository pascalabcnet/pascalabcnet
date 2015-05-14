// Иллюстрация finally - срабатывает независимо от того, произошло исключение или нет

begin
  try
    if Random(2)=1 then
      raise Exception.Create('Ку-ку!');
  finally
    write(1);
    readln;
    exit;
    write(2);
  end;
end.

