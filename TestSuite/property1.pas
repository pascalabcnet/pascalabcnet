type TClass = class
a : integer;
b : integer;
a3 : string;
function getProp2(i : integer):integer;
begin
Result := b;
end;

procedure setProp2(i,item : integer);
begin
  b := item;
end;

property Prop : integer read a write a;
property Prop2[i : integer] : integer read getProp2 write setProp2;default;
end;

var t : TClass := new TClass;
    s : string := 'aaa';
    sb : System.Text.StringBuilder := new System.Text.StringBuilder();
    arr : array[1..3] of integer;
    
begin
sb.Append('abcde');
t.Prop += 2;
assert(t.Prop =2);
t.Prop *= 3;
assert(t.Prop = 6);
t.Prop -= 4;
assert(t.Prop = 2);
t.Prop2[1] += 3;
assert(t.Prop2[1]=3);
end.
