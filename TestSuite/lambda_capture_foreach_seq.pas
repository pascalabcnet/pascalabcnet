type
  t0 = class
    public s1: sequence of integer;
  end;
  
  t1 = class
    procedure p1;
    begin
      var a2: t0;
      var a1: t0;
      foreach var x in a1.s1 do
      begin
        var p: procedure := ()->
        begin
          writeln(x);
          a1 := a1;
        end;
      end;
    end;
  end;

begin
  Assert(1=1);
end.