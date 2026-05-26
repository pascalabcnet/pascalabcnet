  // Исправлены ошибки в StrToInt
  
  procedure TestOK(s: string; expected: integer);
  begin
    var x := StrToInt(s);
    if x <> expected then
      raise new Exception($'FAIL OK: "{s}" → {x}, expected {expected}');
  end;
  
  procedure TestFormat(s: string);
  begin
    try
      var x := StrToInt(s);
      raise new Exception($'FAIL Format: "{s}" → {x}, expected FormatException');
    except
      on e: System.FormatException do ;
      on e: Exception do
        raise new Exception($'FAIL Format: "{s}" → wrong exception type: {e.GetType}');
    end;
  end;
  
  procedure TestOverflow(s: string);
  begin
    try
      var x := StrToInt(s);
      raise new Exception($'FAIL Overflow: "{s}" → {x}, expected OverflowException');
    except
      on e: System.OverflowException do ;
      on e: Exception do
        raise new Exception($'FAIL Overflow: "{s}" → wrong exception type: {e.GetType}');
    end;
  end;
  
begin
  // --- helpers ---
  
  // --- basic ---
  TestOK('0', 0);
  TestOK('5', 5);
  TestOK('123', 123);
  TestOK('000123', 123);
  
  // --- signs ---
  TestOK('+123', 123);
  TestOK('-123', -123);
  TestOK('+0', 0);
  TestOK('-0', 0);
  
  // --- whitespace ---
  TestOK('   123', 123);
  TestOK('123   ', 123);
  TestOK('   123   ', 123);
  TestOK('   -42   ', -42);
  
  // --- boundaries ---
  TestOK('2147483647', 2147483647);
  TestOK('-2147483648', -2147483648);
  
  // --- near boundaries ---
  TestOK('2147483646', 2147483646);
  TestOK('-2147483647', -2147483647);
  
  // --- overflow ---
  TestOverflow('2147483648');
  TestOverflow('2147483649');
  TestOverflow('-2147483649');
  TestOverflow('-9999999999');
  TestOverflow('9999999999');
  
  // --- format errors ---
  TestFormat('');
  TestFormat('   ');
  TestFormat('+');
  TestFormat('-');
  TestFormat('abc');
  TestFormat('12a');
  TestFormat('a12');
  TestFormat('++1');
  TestFormat('--1');
  TestFormat('+-1');
  
  // --- trailing garbage ---
  TestFormat('123abc');
  TestFormat('123  abc');
  
  // --- internal spaces ---
  TestFormat('1 23');
  TestFormat('- 123');
  
  // --- edge tricky ---
  TestOK('0000000000', 0);
  TestOK('0000000001', 1);
  TestFormat('  + 1');
  
  Println('All tests passed');
end.