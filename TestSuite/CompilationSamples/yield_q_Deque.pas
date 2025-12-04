function SeqGen: sequence of integer;
begin
  var q := new Queue<integer>;
  var d := q.Dequeue;
  yield 1;
end;


begin
end.