function myfirst<T>(self:array of T): T; extensionmethod;
begin
  Result := self[0];
end;

function abs(self: integer): integer; extensionmethod;
begin
  Result := PABCSystem.Abs(self);
end;

function test(self: List<integer>): integer; extensionmethod;
begin
  Result := self[1];
end;
function test(self: List<real>): real; extensionmethod;
begin
  Result := self[0];
end;
procedure myproc(self: integer); extensionmethod;
begin
  assert(self = 2);
end;
function glue(self: array of char): string; extensionmethod;
begin
  var sb: StringBuilder := new StringBuilder();
  foreach var c in self do
    sb.Append(c);
  Result := sb.ToString();
end;
procedure MyAdd<T>(self: List<T>; params arr: array of T); extensionmethod;
begin
  self.AddRange(arr);
end;
procedure Add(self: List<integer>; params arr: array of integer); extensionmethod;
begin
  self.AddRange(arr);
end;
function BinarySearch<T>(self: array of T; item: T): integer; extensionmethod;
begin
  Result := System.Array.BinarySearch(self,item);
end;
procedure Test1<T>(self: array[,] of T); extensionmethod;
begin
assert(self[0,0].ToString() = '3');
var e := self[0,0];
assert(e.ToString='3');
end;

begin
  var arr := Arr(1,2,3);
  assert(arr.myfirst=1);
  var i := -2;
  assert(i.abs = 2);
  var lst: List<integer> := new System.Collections.Generic.List<integer>();
  lst.Add(20);lst.Add(15);
  assert(lst.test() = 15);
  var lst2: List<real> := new System.Collections.Generic.List<real>();
  lst2.Add(20);lst2.Add(15);
  assert(lst2.test() = 20);
  var j:= 2;
  j.myproc;
  assert(PABCSystem.Arr('a','b','c').glue='abc');
  var lst3 := new List<integer>;
  lst3.MyAdd(2,3,4,5);
  assert(lst3[3]=5);
  var lst4 := new List<integer>;
  lst4.Add(2,3,4,5);
  assert(lst4[3]=5);
  assert(arr.BinarySearch(3)=2);
  var arr3 := new integer[2,2];
  arr3[0,0] := 3;
  arr3.Test1;

end.