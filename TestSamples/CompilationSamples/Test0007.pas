// div и mod должны быть применимы ко всем целым и здесь возвращать тип integer 
// вообще правила для бинарных операций таковы: если оба операнда<=integer, то 
// возвращается integer, в противном случае возвращается наименьший объемлющий 
// тип для обоих операндов. Если хотя бы один операнд знаковый, то результат - знаковый. Например 
// longword div shortint = int64
// longword - integer = longint
// uint64 - uint64 = uint64 
// Исключение составляют uint64 и любой знаковый тип - над ними никаких бинарных операций делать нельзя

var 
  a: shortint;
  b: smallint;
  c: byte;
  d: word;
  lw : longword;
  li : longint;
  ui : uint64;
  i : integer;
  
begin
a := 3;
  writeln((a div a).GetType);
  b := a;
  writeln((b div b).GetType);
  c := b;
  writeln((c div c).GetType);
  d := c;
  writeln((d div d).GetType);
  lw := d;
  //writeln((lw div lw).GetType);
  li := lw;
  writeln((li div li).GetType);
  ui := li;
  writeln((ui div ui).GetType);
  writeln((ui + c).GetType);
end.