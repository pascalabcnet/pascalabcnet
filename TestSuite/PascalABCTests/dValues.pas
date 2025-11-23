// #2256
begin
  var d := new Dictionary<byte, word>;
  d.Values.GetEnumerator(); 
  Assert(1=1);
end.