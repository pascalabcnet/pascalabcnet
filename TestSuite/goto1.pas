label c1;
begin
  var s := 0;
  foreach var i in range(0, 2) do
  begin
    goto c1;
    s := 2;
  end;  
  c1: assert(s=0);
end.