// Методы .Sum, .Average и .Product для последовательности BigInteger
begin
  var s := SeqGen(10,i->BigInteger(i)**i);
  Print(s.Sum,s.Product,s.Average);
end.
