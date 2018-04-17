var a: integer;
type
  t1 = class
    
    public constructor(i: integer);
    begin
      var p:procedure := () ->begin
        a := i;
      end;
      p;
    end;
  
  end;

begin
  var t := new t1(2);
  assert(a = 2);
end.