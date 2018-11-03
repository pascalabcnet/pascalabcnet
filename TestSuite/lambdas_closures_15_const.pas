procedure p1;
const
  c1 = ' ';
begin
  var p: procedure := ()->
  begin
    var s := c1;
    Assert(s=' ');
  end;
  p;
end;

begin 
  p1;
end.