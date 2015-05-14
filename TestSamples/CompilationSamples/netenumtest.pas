var i:integer;
    e:System.TypeCode;
begin
  e:=System.TypeCode.Int64;
  i:=integer(System.TypeCode.Int64);
  writeln(i);
  writeln(integer(System.TypeCode.Int64));
  writeln(integer(e));
  e:=System.TypeCode(i);
  writeln(e);
  writeln(System.TypeCode(11));
  writeln(System.TypeCode(i));
  //case integer(e) of
  //  System.TypeCode.Int64:;
  //end;
  readln;
end.