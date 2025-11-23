type
  t1 = class
    
    public constructor(a: object);
    begin
    end;
    
    public function f1: sequence of byte;
    begin
      yield 0;
    end;
  
  end;

begin 
  Assert(t1.Create(nil).f1.First = 0);
end.