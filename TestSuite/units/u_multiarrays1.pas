unit u_multiarrays1;
var arr : array[,] of integer;
    arr2 : array[,,] of integer;
    arr3 : array[,,,] of integer;
    arr4 : array[,,,,] of integer;
    arr5 : array[,] of pointer;
    
begin
  
  SetLength(arr,3,4);
  arr[2,1] := 17;
  SetLength(arr,4,6);
  assert(arr[2,1]=17);
  arr[1,1] := 23;
  SetLength(arr,2,2);
  assert(arr[1,1]=23);
  arr[0,0] := 17;
  SetLength(arr,1,1);
  assert(arr[0,0]=17);
  SetLength(arr5,2,3);
  arr5[1,1] := @arr;
 
  SetLength(arr2,3,4,3);
  arr2[1,2,1] := 50;
  SetLength(arr2,4,4,4);
  assert(arr2[1,2,1]=50);
  SetLength(arr2,3,4,4);
  assert(arr2[1,2,1]=50);
  SetLength(arr2,3,6,4);
  assert(arr2[1,2,1]=50);
  SetLength(arr2,4,4,4);
  assert(arr2[1,2,1]=50);
  SetLength(arr2,4,6,4);
  assert(arr2[1,2,1]=50);
  SetLength(arr2,3,4,4);
  assert(arr2[1,2,1]=50);
  SetLength(arr2,3,4,2);
  assert(arr2[1,2,1]=50);
  SetLength(arr2,4,3,5);
  assert(arr2[1,2,1]=50);
  SetLength(arr2,2,3,2);
  assert(arr2[1,2,1]=50);
  SetLength(arr2,4,6,7);
  assert(arr2[1,2,1]=50);
  arr2[1,0,1] := 10;
  SetLength(arr2,7,1,5);
  assert(arr2[1,0,1]=10);
end.