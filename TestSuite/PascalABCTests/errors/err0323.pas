type
  t1 = class
    constructor Create(i: integer);
    begin
      //Create();
    end;
    
    constructor Create(f: ()->byte);
    begin
    end;
    
    constructor Create();
    begin
      //var a := 1;
      Create(()->1);
    end;
  end;

begin
  {var p := new t1;
  p.Create(()->1);}
end.