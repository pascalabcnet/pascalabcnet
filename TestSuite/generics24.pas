type
  at1 = abstract class
    public constructor := exit;
  end;

procedure p1<T1>(o1: T1);
where T1: at1;
begin end;

begin
  var o: at1;
  p1(o);
end.