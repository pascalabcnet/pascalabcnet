var i: integer;

procedure p;
begin
  i := 1;
end;
type
  I1 = interface
    function f1: Action0;
  end;
  t1 = class(I1)
    public function f1: Action0 := p;
  end;

begin 
  var t := new t1;
  t.f1()();
  assert(i = 1);
end.