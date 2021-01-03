uses u_where2;

var i: integer;
type
  
  t1 = class(t0<t1>)
    
    procedure p1(a: t1); override;
    begin
      i := 1;  
    end;
    
  end;
  
begin 
var obj := new t1;
obj.p1(obj);
assert(i = 1);
end.