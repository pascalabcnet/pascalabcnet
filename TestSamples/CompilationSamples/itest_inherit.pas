
type
  i1 = interface
    procedure p;
  end;

  m1 = class(i1)
  public
    procedure p;
    begin
      write('Hi!');
    end;
  end;
  
  m2 = class(m1)
  end;

  m3 = class(m2)
  end;

  m4 = class
  end;
  
var
  obj2: m3;
  i: i1;
  
begin
  obj2 := new m3;
  i := obj2;
  i.p;
  readln;
end.