type A = class
  x: A;
  o: object := 999;
  constructor Create;
  begin
    x := Self;
  end;
  procedure p;
  begin
  end;
end;

begin
  var q: A := new A;
  var ob := q?.x?.o;
  Assert(integer(ob) = 999);
end.