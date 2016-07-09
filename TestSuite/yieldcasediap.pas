function f(n: integer): sequence of integer;
begin
  var m := n;
  case m of
  1: begin 
       yield 3;
       for var i:=1 to 5 do
         yield i;         
     end;
  2: for var i:=1 to 5 do
       case i of
      1,2,3: yield 4;
      4..20: yield -1
       end; 
  else yield 5;
  end;
end;

procedure AssertSeq<T>(q,correct: sequence of T);
begin
  Assert(q.SequenceEqual(correct),q.JoinIntoString);
end;

begin
  AssertSeq(f(1).Println,Seq(3,1,2,3,4,5));
  AssertSeq(f(2).Println,Seq(4,4,4,-1,-1));
  AssertSeq(f(3).Println,Seq(5));
end.