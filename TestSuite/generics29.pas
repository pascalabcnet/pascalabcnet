function DQNToNullable<T>(v: T): System.Nullable<T>; where T: record; 
begin
  Result := new System.Nullable<T>(v);
end;

type TClass = class
  function DQNToNullable<T>(v: T): System.Nullable<T>; where T: record; 
  begin
    Result := new System.Nullable<T>(v);
  end;
end;

type TClass2 = class
  static function DQNToNullable<T>(v: T): System.Nullable<T>; where T: record; 
  begin
    Result := new System.Nullable<T>(v);
  end;
end;

begin
  var x := DQNToNullable(2);
  assert(x.Value = 2);
  assert(DQNToNullable(2).Value = 2);
  var o := new TClass;;
  assert(o.DQNToNullable(3).Value = 3);
  assert(TClass2.DQNToNullable(3).Value = 3);
end.