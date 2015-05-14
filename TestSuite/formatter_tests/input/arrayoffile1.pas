type TFile = file of real;
     TFileArray = array[1..3] of file of integer;
     TDynFileArray = array of file of real;
     
var arr : TFileArray;
    darr : TDynFileArray;
    
begin
  Assign(arr[1],'test.dat');
  SetLength(darr,3);
  Assign(darr[0],'test3.dat');
  darr := new TFile[3];
  Assign(darr[1],'test.dat');
end.