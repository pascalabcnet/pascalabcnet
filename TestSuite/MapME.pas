{procedure operator-=<Key,Value>(Self: IDictionary<Key,Value>; k: Key); extensionmethod;
begin
  Self.Remove(k);
end;}

begin
  var d := Dict(KV(1,2),KV(2,3));
  d -= 1; d -= 4;
  Assert(d.Count = 1);
end.