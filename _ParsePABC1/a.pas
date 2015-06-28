label A,B,C;

var 
  i,j: integer;
  t,t1: real := 2+3;
begin
  write(1+i,(t+2)*3);
  i := 5;
  begin
    i := i + 1;
    goto A;
    j := 2;
  end;
  A: write(2);
end.