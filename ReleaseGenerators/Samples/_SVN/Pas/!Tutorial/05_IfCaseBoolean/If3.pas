// Óñëîâíûé îïåğàòîğ. Ëîãè÷åñêèå óñëîâèÿ ñ or è and
var x: integer;

begin
  writeln('Ââåäèòå x (îò 1 äî 99): ');
  readln(x);
  if (x>=1) and (x<=9) then 
    writeln('Îäíîçíà÷íîå ÷èñëî');
  // Odd - ôóíêöèÿ, âîçâğàùàşùàÿ True òîëüêî åñëè x - íå÷åòíî
  if Odd(x) and (x>=10) and (x<=99) then 
    writeln('Íå÷åòíîå äâóçíà÷íîå ÷èñëî');
  if (x=3) or (x=9) or (x=27) or (x=81) then 
    writeln('Ñòåïåíü òğîéêè');
end.