begin
  try
    writeln(1);
    raise new Exception('xxx');
  except  
  //on e:exception do
    //writeln('Исключение: ',e);
    writeln('Исключение!');
  end;
  readln;
end.
