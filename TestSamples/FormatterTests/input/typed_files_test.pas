type rec = record
             x: integer;
             y: real;
             z: array [1..10] of integer;
             procedure Init(_x: integer; _y:real);
             var i: integer;
             begin
               x := _x;
               y := _y;
               for i:=1 to 10 do 
                 z[i] := i;
             end;
             function ToString:string; override;
             begin
               result := string.Format('({0},{1})',x,y);
             end;
           end;

var f: file of rec;
    r1,r2,r3,r4: rec;
    
begin
  r1.Init(1,2);
  r2.Init(3,4);
  
  AssignFile(f,'typed_files_test.dat');
  Rewrite(f);
  Write(f,r1,r2);
  CloseFile(f);
  
  AssignFile(f,'typed_files_test.dat');
  Reset(f);
  Read(f,r3,r4);
  CloseFile(f);
  
  Writeln(r3,r4);
  Readln;
end.
