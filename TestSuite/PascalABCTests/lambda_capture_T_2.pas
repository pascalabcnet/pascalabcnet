type
  t1<T>=class
    procedure p1();
    begin
      var t: byte := 2;
      var p: procedure(t1: integer) := t1 ->
      begin
        Assert(t=2);
      end;
      p(3);
    end;
  end;

begin
  t1&<integer>.Create.p1
end.