begin
  var s := '1234';
  var fnc: char->boolean := char.IsDigit;
  assert(s.All(fnc) = true);
  assert(s.All&<char>(char.IsDigit) = true);
  assert(s.All&<char>(char.IsControl) = false);
end.