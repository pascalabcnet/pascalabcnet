type
  t1<T>=class
    procedure p1();
    var t2: integer := 2;
    begin
      var p: procedure := ()->
      begin
        Assert(t2=2);
      end;
      p;
    end;
  
  end;

begin
  T1&<integer>.Create.p1;
end.