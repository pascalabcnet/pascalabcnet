var work,work1: array ['à'..'ÿ'] of integer;

begin
  work['à']:=1;
  work1:=work;
  writeln(work1['à']);
  readln;
end.

