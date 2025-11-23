function Sq(n: integer): sequence of integer;
begin
  for var i:=1 to n do
    case i mod 3 of
  0: yield 22;  
  1: yield 33;  
  2: yield 44;  
    end;
  yield 55;  
end;

procedure AssertSeq<T>(q,correct: sequence of T);
begin
  Assert(q.SequenceEqual(correct),q.JoinIntoString);
end;


begin
  AssertSeq(Sq(5).Println,Seq(33,44,22,33,44,55));
end.