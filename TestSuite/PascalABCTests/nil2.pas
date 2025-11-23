begin
  var s1 := Seq('abc','def');
  var s2 := s1 + string(nil); //Вызывает это: function operator+<T>(a, b: sequence of T): sequence of T; extensionmethod;
  assert(s2.ToArray[2] = nil);
  var i := 0;
  try
    var s3 := s1 + nil;
  except
    i := 1;
  end;
  assert(i = 1);
end.