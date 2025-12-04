type TFileArray = array[1..3] of Text;
     TDynFileArray = array of Text;
     
var arr : TFileArray;
    
    darr : TDynFileArray;
    
begin
  Assign(arr[1],'test.dat');
  SetLength(darr,3);
  Assign(darr[0],'test3.dat');
  darr := new Text[3];
  Assign(darr[1],'test.dat');
end.