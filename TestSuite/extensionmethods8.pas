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
end.