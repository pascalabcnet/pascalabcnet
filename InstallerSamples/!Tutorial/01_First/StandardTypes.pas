/// Стандартные размерные типы данных и их размер
var 
  i: integer;
  j: shortint;
  k: smallint;
  l: longint; // синоним integer
  i64: int64;
  b: byte;
  w: word;
  lw: longword;
  car: cardinal; // синоним longword
  ui64: uint64;
  r: real;
  d: double; // синоним real
  sn: single;
  c: char;
  
begin
  Writeln('sizeof(integer) = ':20, sizeof(integer));
  Writeln('sizeof(shortint) = ':20,sizeof(shortint));
  Writeln('sizeof(smallint) = ':20,sizeof(smallint));
  Writeln('sizeof(longint) = ':20, sizeof(longint));
  Writeln('sizeof(int64) = ':20,   sizeof(int64));
  Writeln('sizeof(byte) = ':20,    sizeof(byte));
  Writeln('sizeof(word) = ':20,    sizeof(word));
  Writeln('sizeof(longword) = ':20,sizeof(longword));
  Writeln('sizeof(cardinal) = ':20,sizeof(cardinal));
  Writeln('sizeof(uint64) = ':20,  sizeof(uint64));
  Writeln('sizeof(real) = ':20,    sizeof(real));
  Writeln('sizeof(double) = ':20,  sizeof(double));
  Writeln('sizeof(single) = ':20,  sizeof(single));
  Writeln('sizeof(char) = ':20,    sizeof(char));
end.