// CrossCast должен работать
type 
  IA = interface
    procedure p;
  end;
  IB = interface
    procedure q;
  end;

  AB = class(IA,IB)
  public
    procedure p; virtual; 
    begin 
      writeln('A.p'); 
    end;
    procedure q;
    begin 
      writeln('A.q'); 
    end;
  end;

var 
  a1: AB;
  ia1: IA;
  ib1: IB;
  
begin
  a1 := new AB;
  ia1 := a1;
  ib1 := IB(ia1);
end.