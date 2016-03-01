//uses StrToIntUnit;
  
procedure TestWrong(const s: string);
begin
  var i: integer;
  var code := 0;
  try
    i := StrToInt(s);
  except
    code := 1;
  end;
  if code<>1 then
  begin
    writeln;
    writeln(s+' : Должно было произойти исключение')
  end  
  else write('OK  ');  
end;  

procedure Test(const s: string);
begin
  var code := 0;
  try
    var i := StrToInt(s);
    var i1 := integer.Parse(s);
    if i<>i1 then
      code := 2;
  except
    code := 1;
  end;
  if code=1 then
  begin
    writeln;
    writeln(s+' : Ошибка при переводе строки в целое')
  end  
  else if code=2 then
  begin
    writeln;
    writeln(s+' : Ошибка - неправильно работает')
  end
  else write('OK  ');  
end;
  
begin
  TestWrong('');
  TestWrong('   ');
  TestWrong('+');
  TestWrong('  -  ');
  TestWrong(' a1');
  TestWrong('  1a   ');
  TestWrong(' 12+3 ');
  TestWrong(' ++1 ');
  Test(integer.MaxValue.ToString);
  Test(integer.MinValue.ToString);
  var s1 := '2147483648'; // integer.MaxValue + 1
  TestWrong(s1);
  s1 := '-2147483649'; // integer.MinValue - 1
  TestWrong(s1);
  Test(' 1 ');
  Test(' -1 ');
  Test(' -1 ');
end.