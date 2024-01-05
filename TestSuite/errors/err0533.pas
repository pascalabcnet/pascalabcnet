//exclude
unit err0533;
{$savepcu false}
interface

type
  t1 = class
    static function f1: integer;
  end;
  
implementation

static function t1.f1 := 1;
end.