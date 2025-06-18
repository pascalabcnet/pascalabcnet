unit u_fileofset1;
type TByteSet = set of byte;
     TEnumSet = set of (one, two, three, four,five,six,seven,eight,nine,ten);
     TDiapSet = set of 0..99;
     TDiapSet2 = set of two..seven;
     
var {f : file of TByteSet;
    f1 : file of TEnumSet;
    f2 : file of TDiapSet;
    f3 : file of TDiapSet2;}
    set1 : TByteSet;
    set2 : TEnumSet;
    set3 : TDiapSet;
    set4 : TDiapSet2;
    
begin
 {Assign(f,'test2.dat');
 Rewrite(f);
 set1 := [1,2,5,9,15];
 Write(f,set1);
 Close(f);
 Reset(f);
 set1 := [];
 Read(f,set1);
 assert(set1=[1,2,5,9,15]);
 Close(f);
 
 Assign(f1,'test2.dat');
 Rewrite(f1);
 set2 := [four,seven,nine];
 Write(f1,set2);
 Close(f1);
 Reset(f1);
 set2 := [];
 Read(f1,set2);
 //assert(set2=[four,seven,nine]);
 Close(f1);
 
 Assign(f2,'test2.dat');
 Rewrite(f2);
 set3 := [2..13,23..56,71..84];
 Write(f2,set3);
 Close(f2);
 Reset(f2);
 set3 := [];
 Read(f2,set3);
 assert(set3=[2..13,23..56,71..84]);
 Close(f2);
 
 Assign(f3,'test2.dat');
 Rewrite(f3);
 set4 := [four,seven];
 Write(f3,set4);
 Close(f3);
 Reset(f3);
 set4 := [];
 Read(f3,set4);
 //assert(set4=[four,seven]);
 Close(f3);}
end.