function Sq1: sequence of integer;
begin
  yield 1;
  yield 5;
end;

function Squares(n: integer): sequence of integer;
begin
  for var i:=1 to n do
    yield i*i;
  yield 666;
  begin
    yield 777;
  end;  
end;

function Sq2(n: integer): sequence of integer;
begin
  yield(555);
  for var i:=1 to n do
  begin
    yield i;
    yield i;
  end;
end;


begin
  Assert(Sq1.SequenceEqual(Seq(1,5)),'1');
  Assert(Squares(5).SequenceEqual(Seq(1,4,9,16,25,666,777)),'not Squares(5).SequenceEqual(Seq(1,4,9,16,25))');
  Assert(Sq2(5).SequenceEqual(Seq(555,1,1,2,2,3,3,4,4,5,5)),'3');
end.