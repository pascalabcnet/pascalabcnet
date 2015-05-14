//var a : set of integer;
uses set_unit1;

procedure test(s : set of integer);
var t : set of char;
begin
t := ['a','b','c'];
end;

const h : set of char = ['a','k','l'];

var a : set of integer;
    b : set of integer;
    c : set of integer;
    d : set of integer;
    e : set of string;
    g : set of (red, green, blue, yellow);
    r : TRealSet;
    
begin
test2;
writeln(2 in a);
writeln('k' in h);
g := [green, yellow];
writeln(yellow in g);
test(a);
writeln(2 in a);
//r := [2.71,3.14];
Include(a,23);
Include(a,28);
Include(a,12);
//writeln(InSet(b,a));
Include(b,28);
Include(b,47);
//c := Union(a,b);
c := a + b;
d := a * b;
Include(e,'aaa');
Include(e,'bbb');
Include(e,'ccc');
Exclude(e,'bbb');

a := [21,45,34];
writeln(21 in a);
a := a - [21,45];
writeln(21 in a);
a := [1,2] + [2,4] +[12,13];

e := ['as','ddd','sss'];
end.