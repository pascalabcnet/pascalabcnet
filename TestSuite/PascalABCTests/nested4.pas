type TreeNode = class
a: integer;
end;

procedure Test(var t: TreeNode);
procedure Nested;
procedure Nested2;
begin
  t.a += 1;
  if t.a < 10 then
    Nested;   
end;
begin
  Nested2;
end;
begin
  Nested;
end;
begin
var t := new TreeNode;
t.a := 2;
Test(t);
assert(t.a = 10);
end.