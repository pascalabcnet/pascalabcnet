uses System, System.IO, System.Text;
begin
  var sb: StringBuilder := new StringBuilder();
  var tw: TextWriter := new StringWriter(sb);
  var stout := Console.Out;
  Console.SetOut(tw);
  writeln(23);
  assert(sb.ToString()='23'+Environment.NewLine);
  Console.Out.Flush();
  sb.Clear();
  write(23);
  assert(sb.ToString()='23');
  Console.Out.Flush();
  sb.Clear();
  write(2.3:3:2);
  assert(sb.ToString()='2.30');
  Console.Out.Flush();
  sb.Clear();
  write(4:3);
  assert(sb.ToString()='  4');
  Console.Out.Flush();
  sb.Clear();
  write(45,23);
  assert(sb.ToString()='4523');
  Console.Out.Flush();
  sb.Clear();
  writeln('abc');
  assert(sb.ToString()='abc'+Environment.NewLine);
  writeln('def');
  assert(sb.ToString()='abc'+Environment.NewLine+'def'+Environment.NewLine);
  Console.Out.Flush();
  sb.Clear();
  
end.