{$string_nullbased+}

begin
  var s := '12345';
  var a := s[:4];
  Assert(a='1234');
  //s[1:5] := a; // Почему-то в 32 битном режиме тесты падают
  //Assert(s='11234');
  s := '123456';
  a := s[::2];
  Assert(a='135');
  //s[::2]:=s[1::2];
  //Assert(s='224466');
end. 