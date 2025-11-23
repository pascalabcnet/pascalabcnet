var a: integer;
type
  t1 = class
    event p:procedure;
    
    procedure p1;
    begin
      self.p();
    end;
  end;

procedure test;
begin
  a := 1;
end;

begin 
  var o := new t1;
  o.p += test;
  o.p1;
  assert(a = 1);
end.