type
  rec=record
    x:integer;
    y:real;
    a:array[1..10] of integer;
    //вопрос: а как передать масив в неуправляемый код? такой масив не передаш!
  end;  


begin
  writeln('integer types');
  writeln(sizeof(boolean));
  writeln(sizeof(byte));
  writeln(sizeof(shortint));
  writeln(sizeof(smallint));
  writeln(sizeof(char));
  writeln(sizeof(word));
  writeln(sizeof(integer));//longint
  writeln(sizeof(longword));//cardinal
  writeln(sizeof(longint));//int64
  writeln(sizeof(uint64));

  writeln('float types');
  writeln(sizeof(single));
  writeln(sizeof(real));
  
  writeln('value types');
  writeln(sizeof(rec)*10/10);
  
  readln;
end.