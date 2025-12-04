type TClass = class
procedure Meth;
end;

procedure TClass.Meth;
type TRec = record
a : integer;
b : array[1..2] of byte;
end;

TNum = (one,two,three,four);
const 
  i = 12;
  d = 2.3;
  s = 'abcd';
  c = 'k';
  arr : array of integer = (2,3,4);
  arr2 : array[1..3] of integer = (1,2,3);
  rec : TRec = (a:2;b:(1,3));
  set1 : set of char = ['a','c','f'];
  num1 = three;
  set2 : set of two..four=[one,three,four];
  
procedure Nested;
begin
 assert(i=12);
 assert(d=2.3);
 assert(s='abcd');
 assert(c='k');
 assert(arr[0]=2);
 assert(arr2[1]=1);
 assert(rec.a = 2);
 assert(rec.b[1]=1);
 assert(set1=['a','c','f']);
 assert(num1=three);
 assert(set2=[one,three,four]);
end;
begin
 assert(i=12);
 assert(d=2.3);
 assert(s='abcd');
 assert(c='k');
 assert(arr[0]=2);
 assert(arr2[1]=1);
 assert(rec.a = 2);
 assert(rec.b[1]=1);
 assert(set1=['a','c','f']);
 assert(num1=three);
 assert(set2=[one,three,four]);
Nested;
end;

type TNum = (one,two,three,four);   
var t : TClass := new TClass;
begin
t.Meth;
end.