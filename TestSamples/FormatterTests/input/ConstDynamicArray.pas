uses ConstDynamicArray_unit;
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
  procedure test2;
  const an: array of real = (1,2,3); 
  var vn: array of real := (3,4,5);
  begin
    print('nested pocedure const',an);
    print('nested pocedure var',vn);
  end;    
begin
  print('pocedure const',a);
  print('pocedure var',v);
  test2;
end;
  
begin
  var vb: array of real := (7,8,9);
  print('blok var',vb);
  print('global const',a);
  print('global var',v);
  test;
  print('class field',(new c).v);
end.