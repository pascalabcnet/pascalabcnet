begin
  var pred := char.IsDigit;
  pred :=false ? char.IsLetter : char.IsDigit;
  assert(pred('2'));
end.