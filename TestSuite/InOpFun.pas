begin
  var s := 'abc';
  var hs := HSet(s);
  
  Assert(s.ToLower() in hs);
  Assert(s.ToLower in hs);
end.