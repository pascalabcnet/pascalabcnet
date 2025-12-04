begin
  var a:='asd';
  if (a is string(var s)) and (s is string(var s1)) then
    if (s1 is string(var s2)) and (s2 is string(var s3)) and (s3 = 'asd') then
      Assert(s3 = 'asd')
    else
      Assert(false)
  else
    Assert(false);
end.