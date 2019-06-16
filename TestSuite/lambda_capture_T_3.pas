type
  t1<T>=class
    function p1(t: byte := 4): integer;
  end;

function t1<T>.p1(t: byte): integer;
begin
  var p: procedure := () ->
  begin
    Assert(t=4);
  end;
  p;
end;

begin
  t1&<integer>.Create.p1
end.