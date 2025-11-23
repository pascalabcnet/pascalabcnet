unit unit2;

interface 

implementation

type 
  c1 = class
    f1: string;
  end;

  procedure pr1;
  begin
    var i1:= new c1;
    i1.f1{@var c1.f1: string;@} := '';
  end;


end.