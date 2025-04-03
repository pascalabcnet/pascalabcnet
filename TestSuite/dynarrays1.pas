type shortstr = string[2];
     tset = set of 1..3;
     
procedure Test;
var arr : array of integer := new integer[3](1,2,3);
    arr4 : array of shortstr := new shortstr[2]('abc','def');
    arr5 : array of tset := new tset[2]([1,2,7],[]);
    
begin
assert(arr[0]=1); assert(arr[1]=2); assert(arr[2]=3);
arr := new integer[2](11,12);
assert(arr[0]=11);
var arr2 : array of char := new char[2]('k','l');
assert(arr2[1]='l');
var arr3 :array of real := new real[3](min(2,3),max(1,3),3.2);
assert(arr3[0]=2); assert(arr3[1]=3); assert(arr3[2]=3.2);
assert(arr4[0]='ab');
assert(arr5[0]=[1,2,7]);
end;

procedure Test2;
var arr : array of integer := new integer[3](1,2,3);
    arr4 : array of shortstr := new shortstr[2]('abc','def');
    arr5 : array of tset := new tset[2]([1,2,7],[]);

procedure Nested;
begin
arr := new integer[3](1,2,3);
assert(arr[0]=1); assert(arr[1]=2); assert(arr[2]=3);
arr := new integer[2](11,12);
assert(arr[0]=11);
var arr2 : array of char := new char[2]('k','l');
assert(arr2[1]='l');
assert(arr4[0]='ab');
assert(arr5[0]=[1,2,7]);
var arr3 :array of real := new real[3](min(2,3),max(1,3),3.2);
assert(arr3[0]=2); assert(arr3[1]=3); assert(arr3[2]=3.2);      
end;

begin
assert(arr[0]=1); assert(arr[1]=2); assert(arr[2]=3);
arr := new integer[2](11,12);
assert(arr[0]=11);
var arr2 : array of char := new char[2]('k','l');
assert(arr2[1]='l');
assert(arr4[0]='ab');
assert(arr5[0]=[1,2,7]);
var arr3 :array of real := new real[3](min(2,3),max(1,3),3.2);
assert(arr3[0]=2); assert(arr3[1]=3); assert(arr3[2]=3.2);
Nested;  
end;
  
procedure Test3(a : array of char);
begin
  assert(a.Length = 3);
  assert(a[0]='a');
end;

var arr : array of integer := new integer[3](1,2,3);
    arr4 : array of shortstr := new shortstr[2]('abc','def');
    arr5 : array of tset := new tset[2]([1,2,7],[]);
    arr6 : array of object := (1,2,3);
    arr7 : array of array of set of 1..3 := (([2,3],[3,5]),([1,4],[2,3]));
begin
assert(Length(arr)=3);
assert(arr[0]=1); assert(arr[1]=2); assert(arr[2]=3);
arr := new integer[2](11,12);
assert(arr[0]=11);
var arr2 : array of char := new char[2]('k','l');
assert(arr2[1]='l');
var arr3 :array of real := new real[3](min(2,3),max(1,3),3.2);
assert(arr3[0]=2); assert(arr3[1]=3); assert(arr3[2]=3.2);
assert(arr4[0]='ab');
assert(arr5[0]=[1,2,7]);
assert(integer(arr6[1])=2);
arr6 := new object[3](1,2,3);
assert(integer(arr6[1])=2);
Test;
Test2;
Test3(new char[3]('a','b','c'));
assert(arr7[0,1]=[3,5]);
end.