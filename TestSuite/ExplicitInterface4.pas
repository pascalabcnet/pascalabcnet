var s: integer;

type
  IA = interface
    procedure P();
  end;
  
  A = class(IA)
  public
    {procedure P(); 
    begin
    end;}
    procedure IA.P();
    begin
      s := 1;
    end;
  end;
  
  B = class(A, IA)
  public
    procedure IA.P(); 
    begin
      s := 2;
    end;
  end;

begin
  var b1 := new B;
  IA(b1).P;
  Assert(s=2)
end.