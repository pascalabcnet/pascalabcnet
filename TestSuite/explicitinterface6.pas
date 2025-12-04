var a: integer;

type
  I1 = interface
    
    procedure p1(i: integer);
    procedure p1(s: string);
    
  end;
  
  t1 = class(I1)
    
    public procedure I1.p1(i: integer);
    begin
      a := 1;
    end;
    public procedure I1.p1(s: string);
    begin
      a := 2;
    end;
    
  end;
  
begin 
  var o: I1 := new t1;
  o.p1(2);
  assert(a = 1);
  o.p1('');
  assert(a = 2);
end.