begin
  try
    writeln(1);
    raise new Exception('xxx');
  except  
  on e:Exception do
    writeln('Исключение: ',e);
  end;
  readln;
end.
