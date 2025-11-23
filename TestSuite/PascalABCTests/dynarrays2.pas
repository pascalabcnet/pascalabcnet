type TClass<T> = class
a : array[,] of T;
constructor(a,b : integer);
begin
  self.a := new T[a,b];
end;

procedure SetElem(i,j : integer; it : T);
begin
  a[i,j] := it;
end;
function GetElem(i,j : integer):T ;
begin
Result := a[i,j];
end;
end;

type TRec = record
a : integer;
b : set of char;
end;

type TRec2 = record
arr : array[,] of real;
arr2 : array[1..3] of integer;
end;

type TSet = set of integer;
     TArr = array[,,] of string;
     
procedure Test3(var a : integer);
begin
a := 11;
end;

procedure Test4(var arr : TArr);
begin
  arr := new string[2,2,2];
  arr[0,1,0] := 'ttt';
end;

procedure Test;
var arr3 : array[,] of array of integer := (((1,2),(1,2)),((1,1),(1,1)));
    arr : array[,] of integer:=((1,2),(2,3));
    arr2 : array[,,] of TRec;
    arr4 : array[,] of object;
    arr5 : array[,] of string;
    arr6 : array[,,] of TSet;
    rec : TRec2 := (arr:((1.2,2.5),(3.2,2.3));arr2:(2,4,6));
    
begin
  assert(rec.arr[1,1] = 2.3);
  assert(arr3[0,0][1]=2);
  assert(arr[0,0]=1);
  assert(arr[1,1]=3);
  arr := new integer[2,2]((1,3),(2,4));
  assert(arr[1,0]=2);
  assert(arr[1,1]=4);
  arr[0,1] := 6;
  assert(arr[0,1]=6);
  arr2 := new TRec[2,2,1];
  arr2[0,1,0].a := 13;
  assert(arr2[0,1,0].a=13);
  arr4 := new object[3,3];
  arr4[1,1] := 7;
  assert(integer(arr4[1,1])=7);
  arr5 := new string[6,6];
  assert(arr5[1,2]='');
  arr6 := new TSet[3,2,2];
  arr6[0,1,1] := [2,3];
  assert(arr6[0,1,1]=[2,3]);
end;

procedure Test2;
var arr3 : array[,] of array of integer := (((1,2),(1,2)),((1,1),(1,1)));
    arr : array[,] of integer:=((1,2),(2,3));
    arr2 : array[,,] of TRec;
    arr4 : array[,] of object;
    arr5 : array[,] of string;
    arr6 : array[,,] of TSet;
    rec : TRec2 := (arr:((1.2,2.5),(3.2,2.3));arr2:(2,4,6));
    
procedure Nested;
begin
  assert(rec.arr[1,1] = 2.3);
  assert(arr3[0,0][1]=2);
  arr := new integer[2,3]((2,3,4),(5,3,7));
  assert(arr[1,2]=7);
end;
    
begin
  assert(rec.arr[1,1] = 2.3);
  assert(arr3[0,0][1]=2);
  assert(arr[0,0]=1);
  assert(arr[1,1]=3);
  arr := new integer[2,2]((1,3),(2,4));
  assert(arr[1,0]=2);
  assert(arr[1,1]=4);
  arr[0,1] := 6;
  assert(arr[0,1]=6);
  arr2 := new TRec[2,2,1];
  arr2[0,1,0].a := 13;
  assert(arr2[0,1,0].a=13);
  arr4 := new object[3,3];
  arr4[1,1] := 7;
  assert(integer(arr4[1,1])=7);
  arr5 := new string[6,6];
  assert(arr5[1,2]='');
  arr6 := new TSet[3,2,2];
  arr6[0,1,1] := [2,3];
  assert(arr6[0,1,1]=[2,3]);
  Nested;
end;

var arr3 : array[,] of array of integer := (((1,2),(1,2)),((1,1),(1,1)));
    arr : array[,] of integer:=((1,2),(2,3));
    _arr : array[,] of integer;
    arr2 : array[,,] of TRec;
    arr4 : array[,] of object;
    arr5 : array[,] of string;
    arr6 : array[,,] of TSet;
    r : TRec;
    arr7 : array[,] of object := ((2,r),(r,3));
    arr8 : array[1..2] of array[,] of real := (((1,2),(2,3)),((2.2,2.3),(2,3.3)));
    arr9 : array of array[,] of real := (((1,2),(2,3)),((2.2,2.3),(2,3.3)));
    rec : TRec2 := (arr:((1.2,2.5),(3.2,2.3));arr2:(2,4,6));
    cls : TClass<char>;
    p : ^integer;
    
begin
  assert(Length(arr7,0)=2);
  assert(Length(arr7,1)=2);
  assert(rec.arr[1,1] = 2.3);
  assert(arr3[0,0][1]=2);
  assert(arr[0,0]=1);
  assert(arr[1,1]=3);
  p := @arr[1,1];
  assert(p^=arr[1,1]);
  arr := new integer[2,2]((1,3),(2,4));
  assert(arr[1,0]=2);
  assert(arr[1,1]=4);
  arr[0,1] := 6;
  assert(arr[0,1]=6);
  arr2 := new TRec[2,2,1];
  arr2[0,1,0].a := 13;
  assert(arr2[0,1,0].a=13);
  arr4 := new object[3,3];
  arr4[1,1] := 7;
  assert(integer(arr4[1,1])=7);
  arr5 := new string[6,6];
  assert(arr5[1,2]='');
  arr6 := new TSet[3,2,2];
  arr6[0,1,1] := [2,3];
  assert(arr6[0,1,1]=[2,3]);
  assert(TRec(arr7[0,1])=r);
  assert(integer(arr7[0,0])=2);
  assert(arr8[1][1,0]=2);
  assert(arr9[1][1,0]=2);
  Test;
  Test2;
  Test3(arr[1,1]);
  assert(arr[1,1]=11);
  var arr10: TArr;
  Test4(arr10);
  assert(arr10[0,1,0]='ttt');
  cls := new TClass<char>(3,4);
  cls.SetElem(2,2,'m');
  assert(cls.GetElem(2,2)='m');
end.