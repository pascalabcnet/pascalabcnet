type
  i1 = interface
    procedure proc;
  end;
  
  i2 = interface(i1)
  end;
  
  c = class(i2)
  public
    procedure proc;
    begin
      writeln('ok');
    end;
  end;
  
var
  obj: c;
  i: i2;
  
begin
  obj := new c;
  i := obj;
  i.proc;
  readln;
end.