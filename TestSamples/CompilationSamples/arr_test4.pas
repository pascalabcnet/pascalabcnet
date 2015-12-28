var work,work1: array ['а'..'я'] of integer;

begin
  work['а']:=1;
  work1:=work;
  writeln(work1['а']);
  readln;
end.

