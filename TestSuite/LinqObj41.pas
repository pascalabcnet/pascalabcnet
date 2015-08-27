uses PT4, PT4Linq;

(*
  При решении задач группы LinqObj доступны следующие
  дополнительные методы, определенные в модуле PT4Linq:

  ReadAllLines(name) - функция, которая возвращает последовательность строк,
    содержащихся в текстовом файле с имененм name;
  WriteAllLines(name, seq) - процедура, которая записывает в текстовый файл
    с именем name последотвальеность строк seq;
  Show() и Show(cmt) (методы расширения) - отладочная печать
    последовательности, cmt - строковый комментарий;
  Show(e -> str) и Show(cmt, e -> str) (методы расширения) -
    отладочная печать строковых значений str, полученных
    из элементов e последовательности, cmt - строковый комментарий.
*)
// Абрамян М.Э. ТЕХНОЛОГИЯ LINQ НА ПРИМЕРАХ. М.: ДМК-Пресс, 2014.
// Глава 6. Примеры решения задач из группы LinqObj
//     6.2. Более сложные задания на обработку отдельных последовательностей
// LinqObj41: правильное решение (с.196)


begin
// === ОШИБКА КОМПИЛЯЦЫЫ ===
  Task('LinqObj41');
  var m := ReadInteger;
  var r := ReadAllLines(ReadString)
    .Select(e ->
      begin
        var s := e.Split(' ');
        result := new class (
          brand := StrToInt(s[0]),
          street := s[1],
          price := StrToInt(s[3])
        );
      end)
    .Where(e -> e.brand = m)
    .GroupBy(e -> e.street,
      (k, ee) -> new class ( street := k, max := ee.Max(e -> e.price) ))
    .OrderBy(e -> e.max).ThenBy(e -> e.street)
    .Select(e -> IntToStr(e.max) + ' ' + e.street)
    .DefaultIfEmpty('Нет');
  WriteAllLines(ReadString, r);
end.


