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
  writeln('sizeof(integer) = ':20, sizeof(integer));
  writeln('sizeof(shortint) = ':20,sizeof(shortint));
  writeln('sizeof(smallint) = ':20,sizeof(smallint));
  writeln('sizeof(longint) = ':20, sizeof(longint));
  writeln('sizeof(int64) = ':20,   sizeof(int64));
  writeln('sizeof(byte) = ':20,    sizeof(byte));
  writeln('sizeof(word) = ':20,    sizeof(word));
  writeln('sizeof(longword) = ':20,sizeof(longword));
  writeln('sizeof(cardinal) = ':20,sizeof(cardinal));
  writeln('sizeof(uint64) = ':20,  sizeof(uint64));
  writeln('sizeof(real) = ':20,    sizeof(real));
  writeln('sizeof(double) = ':20,  sizeof(double));
  writeln('sizeof(single) = ':20,  sizeof(single));
  writeln('sizeof(char) = ':20,    sizeof(char));
end.