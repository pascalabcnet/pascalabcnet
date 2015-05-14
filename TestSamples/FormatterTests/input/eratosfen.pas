const n=255;
Var primes: set of byte;
    x,i,c, J: integer;
begin
   FOR J:=1 TO 100 DO
   BEGIN
      primes:=[2..n];
      for i:=2 to round(sqrt(n)) do
      begin
         if not (i in primes) then continue;
         x:=2*i;
         While x<=n do
         begin
            Exclude (primes, x);
            x:=x+i;
         end;
      end;
      for i:=2 to 255 do
      if i in primes then
      Write (i,' ');
   END;
end.