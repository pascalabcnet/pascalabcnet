uses System.Collections, System.Collections.Generic, foreach_unit;

type TEnum = (red, green, blue, black, white);


var lst : ArrayList;
    lst2 : object;
    ss : set of real;
    arr : array of integer;
    arr2 : array[1..4] of integer;
    s:string;
    k,i:integer;
    en:TEnum;
    
begin
Test;
SetLength(arr,5);
arr[0] := 102; arr[1] := 103; arr[2] := 104;
 lst := new ArrayList();
 //lst.Add('Hello');
 //lst.Add('World');
 foreach s  in ['ass','abbb','adfs','rfds','naaa'] do
 writeln(s);
 
 foreach en in [red, green, blue] do
   writeln(en);
 
 foreach k in arr do
   writeln(k);
 lst.Add(12);
 lst.Add(34);
 foreach i in lst do
 writeln(i);
end.