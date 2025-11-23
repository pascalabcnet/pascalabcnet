begin
  var o: object;
  if false then
    raise new Exception( (o?.GetType).ToString);
  Assert(1=1);  
end.