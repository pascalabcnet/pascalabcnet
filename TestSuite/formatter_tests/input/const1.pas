const const1 = 2;
      const2 = const1*3+5;
      const3 = 17 div 4;
      const4 = 1.3+2.4+2;
      const5 = 17 mod 4;
      const6 = 1 shl 3;
      const7 = 8 shr 2;
      const8 = sin(2);
      const9 = ln(3);
      tconst1 : shortint = 1;
      tconst2 : smallint = 1;
      tconst3 : byte = 1;
      tconst4 : word = 1;
      tconst5 : integer = 1;
      tconst6 : longword = 1;
      tconst7 : int64 = 1;
      tconst8 : uint64 = 1;
      tconst9 = tconst1+tconst2+tconst3+tconst4+tconst5+tconst6+tconst7+tconst8;
      
begin
assert(const1=2);
assert(const2=11);
assert(const3=4);
assert(const4=5.7);
assert(const5=1);
assert(const6=8);
assert(const7=2);
assert(const8=sin(2));
assert(const9=ln(3));
assert(tconst9=8);
end.