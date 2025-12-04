type TColor = (red, green, blue);
     TArr = array[1..4] of integer;
     
var f : file of TColor;
    col : TColor;
    f1 : file of TArr;
    arr,arr2 : TArr;
    
begin
Assign(f,'test2.dat');
Rewrite(f);
Write(f,blue);
col := green;
Write(f,col);
Close(f);
Reset(f);
Read(f,col);
assert(col=blue);
Read(f,col);
assert(col=green);
Close(f);
Assign(f1,'test2.dat');
Rewrite(f1);
arr[1] := 4;
Write(f1,arr);
Close(f1);
Reset(f1);
Read(f1,arr2);
assert(arr2[1]=4);
Close(f1);
end.