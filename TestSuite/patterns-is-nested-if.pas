begin
  var a:='asd';
  if (a is string(s)) and (s is string(s1)) then
    if (s1 is string(s2)) and (s2 is string(s3)) and (s3 = 'asd') then
      Assert(s3 = 'asd')
    else
      Assert(false)
  else
    Assert(false);
end.