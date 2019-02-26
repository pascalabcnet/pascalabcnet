type
  t1 = class
    
    constructor(p: procedure) := p;
    
  end;

begin
  new t1(()->
  begin
    var b{@var b: byte;@}: byte;
  end);
  
end.