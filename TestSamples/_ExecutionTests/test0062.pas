type TFile = file of real;
     TFileArray = array[1..3] of file of integer;
     TDynFileArray = array of file of real;
     
var arr : TFileArray;
    parr : ^TFileArray;
    darr : TDynFileArray;
    
begin
  Assign(arr[1],'test.dat');
  New(parr);
  Assign(parr^[2],'test2.dat');
  SetLength(darr,3);
  Assign(darr[0],'test3.dat');
  darr := new TFile[3];
  Assign(darr[1],'test.dat');
end.