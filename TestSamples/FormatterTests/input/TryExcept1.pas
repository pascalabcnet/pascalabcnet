begin
  try
    writeln(1);
    raise new Exception('xxx');
  except  
    writeln('Исключение!');
  end;
  readln;
end.
