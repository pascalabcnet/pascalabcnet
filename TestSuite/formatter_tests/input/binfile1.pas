type TArr = array [1..4] of integer;
var f : file;
    arr : TArr :=(1,2,3,4);
    arr2 : TArr;
    //s : set of byte;
    
begin
Assign(f,'test3.dat');
Rewrite(f);
Write(f,45);
Write(f,'Hello');
Write(f,2.71);
Write(f,'p');
Write(f,arr);
Close(f);
Reset(f);
var i : integer;
Read(f,i); assert(i=45);
var s : string;
Read(f,s); assert(s='Hello');
var r : real;
Read(f,r); assert(r=2.71);
var c : char;
Read(f,c); assert(c='p');
Read(f,arr2); assert(arr2[1]=1);
Close(f);
end.