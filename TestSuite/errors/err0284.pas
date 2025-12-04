type
  I1 = interface
    function f1: Action0;
  end;
  t1 = class(I1)
    public function f1: procedure := nil;
  end;

begin end.