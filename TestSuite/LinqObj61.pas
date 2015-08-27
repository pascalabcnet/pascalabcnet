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
// LinqObj61: правильное решение (с.199-200)
begin
// === ОШИБКА КОМПИЛЯЦИИ ===
    Task('LinqObj61');
    var subjects : array of string := ( 'Алгебра', 'Геометрия', 'Информатика' );
    var culture := new System.Globalization.CultureInfo('en-US');
    var r := ReadAllLines(ReadString)
      .Select(e ->
      begin
          var s := e.Split(' ');
          result := new class
          (
              name := s[0] + ' ' + s[1],
              subj := s[3],
              mark := StrToInt(s[4])
          );
      end)
      .GroupBy(e -> e.name, (k, ee) -> new class
      (
          name := k,
          avrs := subjects
            .GroupJoin(ee, s -> s, e -> e.subj,
              (s1, ee1) -> ee1.Select(e1 -> e1.mark)
            .DefaultIfEmpty().Average())
      ))
      .OrderBy(e -> e.name)
      .Select(e -> e.name + e.avrs.Aggregate('',
         (a, d) -> a + ' ' + d.ToString('f2', culture)));
    WriteAllLines(ReadString, r);
end.


