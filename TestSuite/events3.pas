type
  T1<T> = class
    event E1: procedure;
    
    procedure TryE1();
    begin
      if E1 <> nil then E1();
    end;
  end;

var i: integer;  
procedure p;
begin
  i := 1;
end;  

begin
  var Obj := new T1<integer>();
  Obj.E1 += p;
  Obj.TryE1();
  assert(i = 1);
end.