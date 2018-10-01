{function DQNToNullable<T>(v: T): T; where T: record; 
begin
  var res := new System.Nullable<T>(v);
  Result := res.Value;
end;}

begin
  var t := DQNToNullable(2);
  assert(t.Value = 2);
end.