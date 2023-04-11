uses u_deconstruct1; 

begin 
  var i := 0;
  if new t1 is t1(var a) then
  begin
    i := 1;
  end;
  assert(i = 1);
end.