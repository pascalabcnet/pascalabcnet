unit GenDocUnit;

procedure Step1;
const fname = 'D:\PABC_Git\bin\Lib\PABCSystem.pas';
const fname1 = 'D:\PABC_Git\bin\Lib\PABCExtensions.pas';
begin
  var q := ReadLines(fname).Where(s->s.Trim.Length>0);
  
  var l := new List<string>;
  var skip := true;
  foreach var s in ReadLines(fname).ToArray + ReadLines(fname1).ToArray do
  begin
    if s.StartsWith('//{{{doc:') then 
    begin
      skip := false;
      continue;
    end;
    if s.StartsWith('//{{{--doc:') then 
    begin
      skip := true;
    end;
    if skip then 
      continue;
    l.Add(s.Replace('&',''));  
  end;
  
  WriteLines('PABC.pas',l);
end;

procedure Step2;
const fname = 'PABC.pas';
begin
  var l := new List<string>;
  var skipNext := False;
  var skipUntilEnd := False;
  foreach var s in ReadLines(fname) do
  begin
    if s.Trim.StartsWith('/// !! ')then
      continue;
    if s.Trim.StartsWith('// ')then
      continue;
    if s.Trim.StartsWith('//---')then
      continue;
    if s.Trim.StartsWith('const')then
      continue;
    if s.Trim.StartsWith('type')then
      continue;
    if s.Trim.Length=0 then
      continue;
    
    if s.StartsWith('///--') or s.StartsWith('///!!-') then
    begin
      skipNext := True;
      continue;
    end;
    if skipNext then
    begin
      skipNext := false;
      continue;
    end;
    if s.StartsWith('begin') then
    begin
      skipUntilEnd := True;
      continue;
    end;
    if s.StartsWith('end') then
    begin
      skipUntilEnd := False;
      continue;
    end;
    if skipUntilEnd then
    begin
      continue;
    end;
    l.Add(s.Trim);    
  end;
  
  WriteLines('PABC1.pas',l);
end;

procedure Step3;
const fname = 'PABC1.pas';
begin
  var l := new List<string>;
  var Prev: string := '';
  foreach var s in ReadLines(fname) do
  begin
    if Prev.StartsWith('/// ') and s.StartsWith('///') and not s.StartsWith('///-') then
    begin
      l[l.Count-1] := l[l.Count-1] + '' + s.Remove(0,3).Trim;
    end
    else l.Add(s);    
    Prev := s;
  end;
  
  WriteLines('PABC2.pas',l);
end;

procedure Step4;
const fname = 'PABC2.pas';
begin
  var l := new List<string>;
  var p1 := '';
  var p2 := '';
  foreach var s in ReadLines(fname) do
  begin
    if s.StartsWith('///-')then
    begin
      p1 := s;
      continue;
    end;
    if (p1<>'') and (p2='') then
    begin
      p2 := s;
      continue;
    end;
    if (p1<>'') and (p2<>'') then
    begin
      l.Add(p2); 
      l.Add(p1.Remove(0,4).Trim); 
      p1 := '';
      p2 := '';
      continue;
    end;
    {var s1 := s;
    var ind := s.IndexOf(' = ');
    if ind>=0 then
      s1 := s.Substring(0,ind);}
    l.Add(s);    
  end;
  
  WriteLines('PABCdoc.pas',l);
end;

function Around(Self: string; s: string): string; extensionmethod;
begin
  var opentag := '<'+s+'>';
  var closetag := '</'+s+'>';
  Result := opentag + Self + closetag;
end;

function DeleteFirst(Self: string; s: string): string; extensionmethod;
begin
  if Self.StartsWith(s) then
    Result := Self.Remove(0,s.Length)
  else Result := Self
end;

var toc := Seq('');
var hrefs := Seq('');

procedure GenerateHTMLFile(HelpFileName,Title: string; lst: List<string>);
begin
  var keywords := new SortedSet<string>;
  var s := '';
  var table := '';
  var lst1 := lst.Select(s->s.Replace('<','&lt;').Replace('>','&gt;'));
  var funcs := lst1.Slice(1,2).ToList;
  var defs := lst1.Slice(0,2).ToList;
  var names := funcs.Select(s->begin Result := ''; var ss := s.Split(' ','<','(','&',':'); if (ss[0]='procedure') or (ss[0]='function') then Result := ss[1] else Result := ss[0]; end).ToList;
  var ttt := funcs.ZipTuple(defs,names).OrderBy(t->t[2]);
  
  funcs := ttt.Select(t->t[0]).ToList;
  defs := ttt.Select(t->t[1]).ToList;
  
  if funcs.Count<>defs.Count then
  begin
    Println(HelpFileName,Title);
    writeln('Разное количество');
    halt;
  end;
  
  for var i:=0 to funcs.Count-1 do
  begin
    //funcs[i] := Regex.Replace(funcs[i],'\(\ *Self[^;\)]*\)','()');
    //funcs[i] := Regex.Replace(funcs[i],'\(\ *Self[^;]*;\ *','(');
    funcs[i] := Regex.Replace(funcs[i],'extensionmethod[^;]*;','');

    var dd := defs[i].Remove(0,3).Trim;
    var td := funcs[i].Around('code')
      .Replace('System.IComparable','IComparable')
      .Replace('function','<b>function</b>')
      .Replace('procedure','<b>procedure</b>')
      .Replace('sequence ','<b>sequence </b> ')
      .Replace('set ','<b>set </b> ')
      .Replace('array ','<b>array</b> ')
      .Replace('where ','<b>where</b> ')
      .Replace('file ',' <b>file</b> ')
      .Replace('of ',' <b>of</b> ')
      .Replace('var ','<b>var</b> ')
      .Replace('params ','<b>params</b> ')
      .Replace('const ','<b>const</b> ')
      .Replace('extensionmethod','<b>extensionmethod</b>')
      + NewLine + '<br>' + '&nbsp;'*8 + dd;
    var k := funcs[i].DeleteFirst('function ').DeleteFirst('procedure ');
    k := k.MatchValue('\w+');
    keywords += k;
    td := td.Around('td').Around('tr')+NewLine;
    table += td;
  end;
  var keys := keywords.Select(s->'<param name="Keyword" value="'+s+'">').JoinIntoString(NewLine);
  keys := '<object type="application/x-oleobject" classid="clsid:1e2a7bd0-dab9-11d0-b93a-00c04fc99f9e">'
    + NewLine +keys
    + '</object>'+ NewLine ;
  keys += '<meta http-equiv="Content-Type" content="text/html; charset=windows-1251"><link rel="StyleSheet" href="../../../default.css">'+NewLine;
    
  table := NewLine+'<table cellpadding=3>' + table + '</table>';
  s := keys.Around('HEAD')
    + (Title.Around('H1') + table).Around('body');
  //Println(HelpFileName);  
  WriteAllText('..\LangGuide\PABCSystemUnit\Files\'+HelpFileName+'.html',s.Around('HTML'));

  var s1 := '<param name="Name" value="' + Title + '">';
  var s2 := '<param name="Local" value="LangGuide\PABCSystemUnit\Files\' + HelpFileName + '.html">';
  toc := toc + '<LI> <OBJECT type="text/sitemap">' + s1;
  toc := toc + s2;
  toc := toc + '</OBJECT>';
  
  hrefs := hrefs + ('<li><a href="Files\'+HelpFileName+'.html">'+Title+'</a></li>');
end;

procedure FinalStep;
begin
  var lines := ReadLines('PABCdoc.pas').ToArray;
  var lst := new List<string>;
  var HelpFileName := '';
  var Title := '';
  var num := 0;
  for var i:=0 to lines.Count-1 do
  begin
    var s := lines[i];
    if s.StartsWith('//>>') then
    begin
      num += 1;
      //if num=2 then break;
      if num>1 then
        GenerateHTMLFile(HelpFileName,Title,lst);
        
      lst.Clear;
      HelpFileName := s.Remove(0,4).Trim;
      var ss := HelpFileName.Split('#');
      try
      HelpFileName := ss[1].Trim;
      except
        Print(ss[0])
      end;
      Title := ss[0].Trim;
      continue;
      //Println(Title,HelpFileName);
    end;
    lst.Add(s);
  end;
  GenerateHTMLFile(HelpFileName,Title,lst);
  WriteLines('toc.html',toc);
  WriteLines('hrefs.html',hrefs);
  
  Println(num);
end;


begin
end.