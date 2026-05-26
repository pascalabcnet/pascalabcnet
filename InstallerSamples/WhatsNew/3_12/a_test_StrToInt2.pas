// Сравнение StrToInt с Int32.Parse

begin
  var tests := [
    // --- OK ---
    '0', '5', '123', '000123',
    '+123', '-123', '+0', '-0',
    '   123', '123   ', '   123   ', '   -42   ',
    '2147483647', '-2147483648',
    '2147483646', '-2147483647',
    '0000000000', '0000000001',
    
    // --- Overflow ---
    '2147483648', '2147483649',
    '-2147483649', '-9999999999', '9999999999',
    
    // --- Format ---
    '', '   ', '+', '-',
    'abc', '12a', 'a12',
    '++1', '--1', '+-1',
    '123abc', '123  abc',
    '1 23', '- 123', '  + 1'
  ];
  
  foreach var s in tests do
  begin
    var ok1 := true;
    var ok2 := true;
    
    var r1 := 0;
    var r2 := 0;
    
    var e1: Exception := nil;
    var e2: Exception := nil;
    
    // --- StrToInt ---
    try
      r1 := StrToInt(s);
    except
      on e: Exception do
      begin
        ok1 := false;
        e1 := e;
      end;
    end;
    
    // --- Int32.Parse ---
    try
      r2 := System.Int32.Parse(s);
    except
      on e: Exception do
      begin
        ok2 := false;
        e2 := e;
      end;
    end;
    
    // --- сравнение ---
    
    if ok1 and ok2 then
    begin
      if r1 <> r2 then
        raise new Exception($'FAIL value: "{s}" → {r1} vs {r2}');
    end
    else if (not ok1) and (not ok2) then
    begin
      if e1.GetType <> e2.GetType then
        raise new Exception($'FAIL exception: "{s}" → {e1.GetType} vs {e2.GetType}');
    end
    else
    begin
      if ok1 then
        raise new Exception($'FAIL mismatch: "{s}" → StrToInt={r1}, Parse throws {e2.GetType}')
      else
        raise new Exception($'FAIL mismatch: "{s}" → StrToInt throws {e1.GetType}, Parse={r2}');
    end;
  end;
  
  Println('All tests matched Int32.Parse');
end.