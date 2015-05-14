type 
  c=class
    const z = 'z';
    private const x:real = 10;
    public const y = 10+x;
    procedure p;
    begin
      writeln(x);
      writeln(y);
      writeln(z);
    end;
  end;

var cc:c;
  
begin
  writeln(c.x);
  cc := new c;
  cc.p;  
  readln;
end.