 
uses arr_use;
  		
type
arrtype=array[1..10] of array[3..5] of real;
diap1=1..10;
diap2=3..5;
my_real=real;
arrt=array[diap2] of my_real;
arrt2=array[diap1] of arrt;

procedure Some(a : arrtype);
begin
 a[3][4] := 1000;
end;

var
arr:array[1..10] of array[3..5] of real;
a1:arrtype;
a2:arrt2;
arr3 : array[1..4,1..7] of integer;

akp:array[1..10] of integer;

i:integer;

begin
 arr[3][4]:=3;
 writeln(arr[3][4]);
 //Some(arr);
 writeln(arr[3][4]);
 //a1[2][3]:=5;
 arr3[4][7] := 1000;
 writeln(arr3[4][7]);
 writeln('a'.tostring); 

 for i:=1 to 10 do
 begin
  writeln('OK');
  akp[i]:=1 shl i;
 end;

 //pr(akp);

 readln;
end.
