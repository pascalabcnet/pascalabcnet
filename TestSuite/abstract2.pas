type
  I1 = interface
    
    function f1: byte;
  
  end;
  AC1 = abstract class(I1)
    
    public function f1: byte; abstract;
  
  end;
  C1 = class(AC1)
    
    public function f1: byte; override := 1;
  
  end;

begin
  var c: I1 := new C1;
  assert(c.f1=1);
end.