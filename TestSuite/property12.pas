type
  t1 = class
    static function f1 := 1;
    static procedure f1(x: integer) := exit;
    property p1: integer read f1 write f1;
    
  end;
  
begin
  var a := new t1;
  assert(a.p1 = 1);
  a.p1 := 2;
  
end.