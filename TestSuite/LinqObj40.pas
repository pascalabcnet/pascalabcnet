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

begin
  Task('LinqObj40');
  var res := ReadAllLines(ReadString)
    .Select(s ->
      begin
        var ss := s.Split();
        result := new class
          (
            C := ss[0],
            M := integer.Parse(ss[2]),
            P := integer.Parse(ss[3]),
            S := ss[1]
          );
      end)
    .GroupBy(s -> s.S, (k, ss) -> new class
      (
         S := k,
         C := Range(0,2).Select(i -> ss.Count(s -> s.M = 92+i*3))
      ))
    .OrderBy(e -> e.S)
    .Select(e -> e.S + e.C.Aggregate('', (seed, s) -> seed + ' ' + s));
  WriteAllLines(ReadString, res);
end.


