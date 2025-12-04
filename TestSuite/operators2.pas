type
  t1 = class
    
    static function operator=(a,b: t1) := true;
    
  end;
  t2 = class(t1) end;
  
begin
  var a := new t2;
  var b := new t2;
  assert(a = b);
end.