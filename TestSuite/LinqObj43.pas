uses PT4, PT4Linq;

begin
  Task('LinqObj43');
  var m := ReadInteger;
  var res := ReadAllLines(ReadString)
    .Select(s ->
      begin
        var ss := s.Split();
        result := new class
          (
            C := ss[3],
            M := integer.Parse(ss[1]),
            P := integer.Parse(ss[0]),
            S := ss[2]
          )
      end)
    .GroupBy(s1 -> s1.C, (k, ss) -> new class
      (
        Comp := k,
        C := ss.Where(s2 -> s2.M = m)
      ))
end.


