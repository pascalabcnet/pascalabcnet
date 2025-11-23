function DQNToNullable2<T> (ff: System.Nullable<T>; v: T): System.Nullable<T>; where T: record;
begin
  Result := new System.Nullable<T>(v);
end;

begin
  var v := DQNToNullable2(2,2);
  assert(v.Value = 2);
end.