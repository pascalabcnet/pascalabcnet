uses
  PT4, PT4Linq;

begin
  Task('LinqObj36');
  System.Threading.Thread.CurrentThread.CurrentCulture := 
    new System.Globalization.CultureInfo('en-US');
  var res := ReadAllLines(ReadString)
    .Select(s ->
      begin
        var ss := s.Split();
        result := new class
          (
             N := integer.Parse(ss[0]),
             D := double.Parse(ss[2]),
             F := ss[1]
          );
      end)
    .GroupBy(s -> (s.N - 1) mod 36 div 4 + 1, (k, ss) -> new class
      (
        P := k,
        Info := ss.Where(s -> s.D <= ss.Average(s1 -> s1.D))
      ))
 // так не компилируется 
    .SelectMany(s -> s.Info.Select(s1 -> new class
      (
        P := s.P, 
        F := s1.F, 
        N := s1.N, 
        D := s1.D
      )))
    .OrderBy(s -> s.P).ThenBy(s -> s.D)
    .Select(e -> e.P + ' ' + e.D.ToString('F2') + ' ' + e.F + ' ' + e.N);
  WriteAllLines(ReadString, res);

(* // так тоже не компилируется 
    .SelectMany(e -> e.Info)
    .OrderBy(e -> (e.N - 1) mod 36 div 4 + 1).ThenBy(e -> e.D)
    .Select(e -> ((e.N - 1) mod 36 div 4 + 1) + ' ' + e.D.ToString('F2') + ' ' + e.F + ' ' + e.N);
  WriteAllLines(ReadString, res);
*)
  
end.