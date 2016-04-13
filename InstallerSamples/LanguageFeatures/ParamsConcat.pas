// Создание функции Concat с переменным числом параметров

function Concat(params strs: array of string): string;
begin
  var sb := new StringBuilder;
  foreach var x in strs do
    sb.Append(x);
  Result := sb.ToString;
end;

begin
  Writeln(Concat('Pascal','ABC','.NET'));
end. 