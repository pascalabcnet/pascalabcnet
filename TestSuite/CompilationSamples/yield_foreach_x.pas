function GetWords(sq:sequence of string):sequence of string;
begin
  foreach var x in sq do
  begin
    var a := x.toWords; //в этой строке ошибка, если раскомментировать yield
    yield a[0];
  end;
end;
begin
  var sq := Seq('1 2 3', 'a b c d');
  GetWords(sq).Print;
end.