type TRec = record
a : integer;
b : array of integer;
c : array[1..1,1..1] of integer;
d : array of array of integer;
end;

type IntArr = array of integer;

const carr : array[1..1] of integer = (1);
    carr2 : array of integer = (3);
    carr3 : array[1..2,1..1] of integer = ((1),(2));
    carr4 : array[1..1,1..1] of integer = ((3));
    cnst : integer = (2);
    
var arr : array[1..1] of integer := (1);
    arr2 : array of integer := (3);
    arr3 : array[1..2,1..1] of integer := ((1),(2));
    arr4 : array[1..1,1..1] of integer := ((3));
    arr5 : array[,] of integer := ((1),(2));
    arr6 : array of integer := ((1),(3));
    arr7 : array[1..2] of integer := (((1)),(3));
    arr8 : array of array of integer := ((1),(2));
    arr9 : array[,] of integer := (((1)),((2)));
    rec : TRec := (a:(3);b:(2);c:((5));d:((2),(4)));
    
begin
 assert(arr[1]=1);
 assert(arr2[0]=3);
 assert(arr3[2,1]=2);
 assert(arr4[1,1]=3);
 assert(arr5[1,0]=2);
 assert(carr[1]=1);
 assert(carr2[0]=3);
 assert(carr3[2,1]=2);
 assert(carr4[1,1]=3);
 assert(cnst=2);
 assert(arr6[0]=1);
 assert(arr7[1]=1);
 assert(arr8[1][0]=2);
 assert(arr9[0,0]=1);
 assert(rec.a=3);
 assert(rec.b[0]=2);
 assert(rec.c[1,1]=5);
 assert(rec.d[1][0]=4);
 arr2 := new integer[3]((2),(3),(4));
 assert(arr2[0]=2);
 arr5 := new integer[2,1]((2),(3));
 assert(arr5[1,0]=3);
 //arr8 := new IntArr[2]((1),(2));
end. 