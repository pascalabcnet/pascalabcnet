type
  base = class
    function f1: Action0; abstract;
  end;
  t1 = class(base)
    public function f1: procedure; override := nil;
  end;

begin end.