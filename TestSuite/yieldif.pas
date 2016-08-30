function Sq(n: integer): sequence of integer;
begin
  if n mod 2 = 0 then
    yield 555
  else yield 444;  
end;

function Where1<T>(s: sequence of T; p: T->boolean): sequence of T;
begin
  foreach var x in s do
    //if p(x) then
      yield x;
end;

begin
  Assert(Sq(2).SequenceEqual(Seq(555)),Sq(2).JoinIntoString);
  Assert(Sq(1).SequenceEqual(Seq(444)),Sq(2).JoinIntoString);
  var q := Where1(Arr(1,2,3,4),x->x mod 2 <>0);
  q.Println;
  //q.Println;
  //Assert(q.SequenceEqual(Seq(444)),q.JoinIntoString);
end.