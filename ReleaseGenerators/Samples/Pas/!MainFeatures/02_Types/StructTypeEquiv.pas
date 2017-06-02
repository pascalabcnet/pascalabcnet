// Иллюстрация структурной эквивалентности для некоторых типов. В Delphi - именная эквивалентность
var 
  a: array of integer;
  a1: array of integer;
  s: set of real;
  s1: set of real;
  p: procedure (i: integer);
  p1: procedure (i: integer);
  r: ^integer;
  r1: ^integer;

procedure proc(aa: array of integer; ss: set of real; pp: procedure (i: integer); rr: ^integer);
begin
  
end;

begin
  a := a1;
  s := s1;
  p := p1;
  r := r1; // В Delphi ни одно из этих присваиваний не сработает
  proc(a,s,p,r); // Этот вызов - тоже
end.