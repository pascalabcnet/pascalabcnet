begin
  var arr1 := Arr(1,2,3);
  var arr2 := Arr(4,5,6);
  var arr3: array of integer := arr1+arr2;
  assert(arr3[3]=4);
  var arr4 := Arr('aaa','bbb','ccc');
  var arr5 := Arr('ddd','eee','fff');
  var arr6: array of string;
  arr6 := arr4 + arr5;
  assert(arr6[5]='fff');
  
end.