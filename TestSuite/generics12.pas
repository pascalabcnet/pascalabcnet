function DQNToNullable<T>(v: T): T; where T: record; 
begin
  var res := new System.Nullable<T>(v);
  Result := res.Value;
end;

begin
  assert(DQNToNullable(2) = 2);
end.