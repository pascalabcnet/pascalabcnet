uses System.Collections.Generic, System.Linq;

procedure CheckChars(actual: sequence of char; expected: string);
begin
  assert(actual.SequenceEqual(expected));
end;

begin
  var chars := Arr('A', 'B', 'C');
  var text := 'DE';
  CheckChars(chars + text, 'ABCDE');
  CheckChars(text + chars, 'DEABC');
  CheckChars(chars + 'D', 'ABCD');
  CheckChars('D' + chars, 'DABC');

  var list := new List<char>(chars);
  CheckChars(list + text, 'ABCDE');
  CheckChars(text + list, 'DEABC');

  var numbers := new List<integer>(Arr(1, 2));
  var tail: sequence of integer := Arr(3, 4);
  assert((numbers + tail).SequenceEqual(Arr(1, 2, 3, 4)));
  assert((tail + numbers).SequenceEqual(Arr(3, 4, 1, 2)));

  // Ordinary extension methods found through the same interface still work.
  assert(list.Where(c -> c <> 'B').SequenceEqual(Arr('A', 'C')));
end.
