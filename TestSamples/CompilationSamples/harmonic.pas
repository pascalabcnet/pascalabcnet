
function harm_member(i, j : integer) : real;
begin
 harm_member := 1.0/(i*j);
end;

var i, j : integer;
    s : real;

begin
 s := 0;
 i := 1; j := 1;
 while i < 30000 do
 begin
  j := 1;
  while j < 30000 do
  begin
   s := s + harm_member(i,j);
   j := j+1;
  end;
  i := i + 1;
 end;
 writeln(s); 
end.