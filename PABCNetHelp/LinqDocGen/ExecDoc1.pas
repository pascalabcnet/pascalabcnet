begin
  var funcs := ReadLines('Документация.pas').Where(s->s.startswith('function')).Select(s->s.Replace('<','&lt;').Replace('>','&gt;'));
  funcs := funcs.Select(s->s.Replace('<','&lt;').Replace('>','&gt;')).Select(s->s.Insert(8,'</b>').Insert(0,'<b>'));
  var comments := ReadLines('Документация.pas').Where(s->s.startswith('///')).Select(s->s.Remove(0,4));
  comments := comments.Select(s->begin s := Trim(s); if s[s.Length]<>'.' then s += '.'; Result := s; end);
  var z := funcs.Zip(comments,(f,c)->'<tr><td>'#13#10'<code>'+f+'</code>'+NewLine+'<br>'+'&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;'+c+#13#10'</td></tr>');
  z := '<table border=0>'+z+'</table>';
  WriteAllLines('a.html',z);
end.