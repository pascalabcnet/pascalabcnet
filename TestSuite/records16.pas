type
  t0=class
    static i := 0;
    
    constructor :=
    i += 1;
  end;
  r1=record
    ni := t0.i;
  end;
  
  t1=class(t0)
    
    constructor;
    begin
      var a: r1;
      assert(a.ni = 1);
    end;
    
  end;

begin
  new t1;
end.