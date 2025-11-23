//type TArr = array[1..4] of real;
//     TMatr = array[1..3,1..3] of integer;

type TRec = record
  b : real;
  arr : array[1..2] of string[5];
  c : char;
end;
     
type TArr = array[1..2] of string[4];
     TArr2 = array[1..2] of TRec;
     
var f : file of TArr;
    f2 : file of TArr2;
    arr : TArr;
    arr2 : TArr2;
    
begin
  Assign(f,'test6.dat');
  Rewrite(f);
  arr[1] := 'ggg';
  arr[2] := 'bcd';
  Write(f,arr);
  arr[1] := 'lll';
  arr[2] := 'kk';
  Write(f,arr);
  Close(f);
  Reset(f);
  Seek(f,1);
  Read(f,arr);
  assert(arr[1]='lll');
  assert(arr[2]='kk');
  Seek(f,0);
  Read(f,arr);
  assert(arr[1]='ggg');
  assert(arr[2]='bcd');
  Close(f);
end.