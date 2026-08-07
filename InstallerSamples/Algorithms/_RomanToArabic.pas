function CorrectRomans(Self: string): List<string>; extensionmethod;
begin
  var roman := '(?=[IVXLCDM])(M{0,3}(CM|CD|D?C{0,3})(XC|XL|L?X{0,3})(IX|IV|V?I{0,3}))';
  Result := Self.MatchValues(roman).ToList
end;

/// Преобразует римское число, записанное в виде строки, в арабское число
function ToArabic(s: string): integer; 
begin
  var k := [1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1];
  var v := ['M', 'CM', 'D', 'CD', 'C', 'XC', 'L', 'XL', 'X', 'IX', 'V', 'IV', 'I'];
  Result := 0;
  var i := 0;
  while s.Length > 0 do
  begin
    while s.StartsWith(v[i]) do
    begin
      Result += k[i];
      s := s?[v[i].Length + 1:]
    end;
    i += 1;
  end
end;

begin
  // Из строки выделяем корректно записанные рииские числа и переводим их в арабские
  var s := 'XLXLXVXCIVIXCVCXDCVDILDDVCVLXCCCLVXXDLXCLLVXCDLMLCMVDDCXMDLDIIDVIVDMMMMLCCDCXDVIC';
  s.CorrectRomans.Select(s -> (s, ToArabic(s))).Print;
end.