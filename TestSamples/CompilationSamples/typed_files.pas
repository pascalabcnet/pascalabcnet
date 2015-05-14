uses typed_files_unit;

type 
  rec1 = record 
    x,y:integer; 
  end;
  //c=class
  //  f:file of real;
  //end;
  
procedure test;
var f1:file of byte;
begin
  writeln('in procedure:');
  writeln(f1.ElementType);
end;


procedure writeln_tf(f:TypedFile);
begin
  writeln(f.ElementType);
end;

var f1:file of integer;
f5 : file of real;
    f2:file of rec1;
    f3:file of record 
                 x,y,z:integer; 
               end;
    f4:file of array[1..10] of record arr:array[1..10] of integer end;
 //   cc:c;
begin
  //sin(f1);
  writeln('global:'); 
  writeln(f1.ElementType);
  writeln(f2.ElementType);
  writeln(f3.ElementType);
  writeln('in unit:'); 
  writeln(ff.ElementType);
//  write(ff,ff); неправильно€ ошибка при общей компил€ции! перектрытие имен!
  test;
  //cc := new c;
  //writeln('in class:'); 
  //writeln(cc.f.ElementType);
  writeln_tf(f1);
  readln;  
end.