function Sk(s:string): string;
begin
  var q := s.SkipWhile(c->not (c in ['a'..'z','A'..'Z']));
  Result := new string(q.ToArray())
end;

begin
  var files := ReadLines('Документация.pas').Where(s->s.startswith('// ')).Select(s->Rec(s.Remove(0,3),sk(s)));
  var z := files.Select(p->'<li><a href="Files\'+p.Item2+'.html">'+p.Item1+'</a></li>');
  //files.Print(NewLine);
  WriteAllLines('d.html',z);
  
  
  //g.Print(NewLine+NewLine);
  
  {var z := funcs.Zip(comments,(f,c)->'<tr><td>'#13#10'<code>'+f+'</code>'+NewLine+'<br>'+'&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;'+c+#13#10'</td></tr>');
  z := '<table border=0>'+z+'</table>';}
end.