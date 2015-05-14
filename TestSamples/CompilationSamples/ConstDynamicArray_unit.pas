unit ConstDynamicArray_unit;
type c=class
        //const a: array of real = (1,2,3); TODO
        v: array of real := (3,4,5);
     end;


const a: array of real = (1,2,3);
var v: array of real := (3,4,5);
  
procedure print(s:string;aa: array of real);
begin
  write(s+':');
  foreach r:real in aa do
    write(r,' ');
  writeln;
end;


procedure test;
const a: array of real = (1,2,3);
var v: array of real := (3,4,5);
begin
  print('unit pocedure const',a);
  print('unit pocedure var',v);
end;

begin
  var vb: array of real := (7,8,9);
  print('blok var in unit',vb);
  print('unit global const',a);
  print('unit global var',v);
  test;
  print('unit class field',(new c).v);
end.