unit u_fileofdiap1;
type TDiap = 1..5;
     TDigits = (one,two,three,four,five);
     TDigDiap = one..five;
     
var f : file of TDiap;
    a : TDiap;
    f1 : file of TDigDiap;
    a1 : TDigDiap;
    
begin
 Assign(f,'test2.dat');
 Rewrite(f);
 Write(f,3);
 a := 2;
 Write(f,a);
 Close(f);
 Reset(f);
 Read(f,a); assert(a=3);
 Read(f,a); assert(a=2);
 Close(f);
 
 Assign(f1,'test2.dat');
 Rewrite(f1);
 Write(f1,four);
 a1 := three;
 Write(f1,a1);
 Close(f1);
 Reset(f1);
 Read(f1,a1); assert(a1=four);
 Read(f1,a1); assert(a1=three);
 Close(f1);
end.