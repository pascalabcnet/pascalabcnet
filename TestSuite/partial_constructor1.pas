type
  t1 = partial class end;
  
  t1 = partial class
    constructor(b: integer);
    begin
      assert(b = 1);
    end;
  end;
  
begin
  new t1(1);
end.