type
  t1=class end;

begin
  var o:object;
  
  if o is t1(var o_t1) then
    writeln(o_t1{@var o_t1: t1;@});
  if o is t1(var o_t1) then
  begin
    writeln(o_t1{@var o_t1: t1;@});
  end;
  writeln(o_t1{@@});
end.