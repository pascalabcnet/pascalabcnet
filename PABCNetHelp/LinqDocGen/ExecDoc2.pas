begin
  var r := new Regex('IEnumerable<(\w+)>');

  var section := '';
  var funcs := ReadLines('Документация.pas')
    .Select(s->begin 
                 if s.StartsWith('// ') then
                 begin
                   section := s.Remove(0,3);
                   writeln(section);  
                 end;  
                 Result := s 
               end)
    .Where(s->s.startswith('function IEnumerable<') or s.startswith('procedure IEnumerable<')).Print(NewLine)
    .Select(s->s.Replace('function IEnumerable<T>.','function '))
    .Select(s->s.Replace('procedure IEnumerable<T>.','procedure '))
    .Select(s->s.Replace('function IEnumerable<число>.','function '))
    .Select(s->r.Replace(s,'sequence of $1'))
    .Select(s->s.Replace('<','&lt;').Replace('>','&gt;'))
    .Select(s->s.Replace('sequence of ','<b>sequence of</b> '))
    .Select(s->s.Insert(9,'</b>').Insert(0,'<b>'))
    .Select(s->Rec(section,s));
  var comments := ReadLines('Документация.pas').Where(s->s.startswith('///')).Select(s->s.Remove(0,4));
  comments := comments.Select(s->begin s := Trim(s); if s[s.Length]<>'.' then s += '.'; Result := s; end);
  
  var z := funcs.Zip(comments,(f,c)->Rec(f.Item1,f.Item2,c));
  
  var g := z.GroupBy(x->x.Item1);
  
  var toc := Seq('');
  foreach var sect in g do
  begin
    var Key: string := sect.Key;
    //writeln(Key);
    var qq := Key.SkipWhile(c->not (c in ['a'..'z','A'..'Z']));
    Key := new string(qq.ToArray());
    var keys := key.ToWords(' ',',').Select(s->'<param name="Keyword" value="'+s+'">');
    Writeln(keys);
    var sq := sect.Select(s->'<tr><td>'#13#10'<code>'+s.Item2+'</code>'+NewLine+'<br>'+'&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;'+s.Item3+#13#10'</td></tr>');
    sq := '<H1>'+sect.Key+'</H1>'+'<h2>Описание методов</h2><p>Методы приведены для последовательности <code><b>sequence of</b> T</code>.</p>'+'<table>'+sq+'</table>';
    sq := sq + '<h2>Пример </h2><blockquote><code></code></blockquote>';
    sq := '<body>'+sq+'</body>';
    sq := '<html><head><object type="application/x-oleobject" classid="clsid:1e2a7bd0-dab9-11d0-b93a-00c04fc99f9e">' +
          keys +
          '</object><meta http-equiv="Content-Type" content="text/html; charset=windows-1251"><title></title><link rel="StyleSheet" href="../../../default.css"></head>' +
          sq;
    WriteLines('Files\'+Key+'.html',sq);
    var s1 := '<param name="Name" value="'+Key+'">';
    var s2 := '<param name="Local" value="LangGuide\FuncProgramming\Files\'+Key+'.html">';
    toc := toc + '<LI> <OBJECT type="text/sitemap">'+s1;
    toc := toc + s2;
    toc := toc + '</OBJECT>';
    //Println(sect.Key);
    //Println(sect.AsEnumerable);
  end;
  WriteLines('toc.html',toc);  
  
  //g.Print(NewLine+NewLine);
  
  {var z := funcs.Zip(comments,(f,c)->'<tr><td>'#13#10'<code>'+f+'</code>'+NewLine+'<br>'+'&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;'+c+#13#10'</td></tr>');
  z := '<table border=0>'+z+'</table>';
  WriteAllLines('a.html',z);}
end.