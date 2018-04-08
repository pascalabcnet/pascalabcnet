var i: integer;

type
  I1 = interface
    
    procedure p1;
  
  end;
  R1 = record(I1)
    
    public procedure p1;
    begin
      Inc(i);
    end;
    
    public constructor;
    begin
    end;
  
  end;
  R2 = record(I1)
    
    public next1: I1;
    public next2: I1;
    
    public procedure p1;
    begin
      next1.p1;
      next2.p1;
    end;
    
    public constructor(b:boolean);
    begin
      next1 := b? I1(new R2(false)) : I1(new R1);
      next2 := b? I1(new R2(false)) : I1(new R1);
    end;
  
  end;

begin
  var a := new R2(true);
  a.p1;
  assert(i = 4);
end.