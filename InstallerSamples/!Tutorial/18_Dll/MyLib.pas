// Откомпилируйте библиотеку, нажав Ctrl-F9. В папке должен появиться файл MyLib.dll

library MyLib;

const MyPi = 3.14;

function add(a,b: integer): integer;
begin
  Result := a + b;
end;

type 
  Frac = record
    num,denom: integer;
    constructor (n,d: integer);
    begin
      num := n;
      denom := d;
    end;
    procedure Print;
    begin
      writeln(num,'/',denom);
    end;
  end;

end.