// Ëîãè÷åñêèé òèï. Ëîãè÷åñêèå âûğàæåíèÿ ñ and, or è not
var 
  b: boolean;
  x: integer;
begin
  write('Ââåäèòå x (îò 1 äî 9): ');
  readln(x);
  b := x=5;
  writeln('x=5? ',b);
  b := (x>=3) and (x<=5);
  writeln('x=3,4 èëè 5? ',b);
  b := (x=3) or (x=4) or (x=5);
  writeln('x=3,4 èëè 5? ',b);
  b := not Odd(x);
  writeln('x - ÷åòíîå? ',b);
end.