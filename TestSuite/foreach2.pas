uses System.Collections.Generic;
type TRec = record
a: integer;
end;

type TClass<T> = class
lst: List<T>;
constructor(params arr: array of T);
begin
  lst := new List<T>;
  foreach item: T in arr do
    lst.Add(item);
end;
procedure Check(params arr: array of T);
begin
  var i := 0;
  foreach item: T in lst do
  begin
    assert(item = arr[i]);
    i += 1;
  end;  
end;
end;

type TClass2<T,U> = class where T: IEnumerable<U>;
lst: T;
procedure Check(params arr: array of U);
begin
  var i := 0;
  foreach item: U in lst do
  begin
    assert(item = arr[i]);
    i += 1;
  end;
end;
end;

begin
var lst: List<TRec> := new List<TRec>;
var rec: TRec;
rec.a := 23;
lst.Add(rec);
lst.Add(rec);
foreach r: TRec in lst do
  assert(r.a = 23);
var lst2: List<integer> := new List<integer>();
lst2.Add(35);
lst2.Add(35);
foreach i: integer in lst2 do
  assert(i = 35);
foreach i: integer in new integer[3](23,23,23) do
  assert(i = 23);
var obj: TClass<integer> := new TClass<integer>(1,2,3,4,5);
obj.Check(1,2,3,4,5);
var obj2: TClass<integer> := new TClass<integer>(1,2,3,4,5);
obj2.Check(1,2,3,4,5);
var obj3: TClass<TClass<integer>> := new TClass<TClass<integer>>(obj,obj2);
obj3.Check(obj, obj2);
var arlst := new System.Collections.ArrayList();
arlst.Add(23);
arlst.Add(23);
foreach i: integer in arlst do
  assert(i = 23);
var obj4: TClass2<List<integer>,integer> := new TClass2<List<integer>,integer>;
obj4.lst := lst2;
obj4.Check(35,35);
end.