type
  t1<T>=class
    function p1(t3: byte := 2): integer;
  end;

function t1<T>.p1(t3: byte): integer;
begin
  var t: integer := 6;
  var p: procedure := () ->
  begin
    Assert(t=6);
  end;
  p;
end;

begin
  t1&<integer>.Create.p1
end.