unit GenDocUnit;

var fname := '';
var fname1 := '';

var RootOutputDirectory := '';

procedure Step1;
begin
  var q := ReadLines(fname).Where(s->s.Trim.Length>0);
  
  var l := new List<string>;
  var skip := true;
  
  var ll := ReadLines(fname);
  if fname1<>'' then
    ll := ll + ReadLines(fname1);
  
  foreach var s in ll do
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
  
  WriteLines('__PABC.pas',l);
end;

procedure Step2;
const fname = '__PABC.pas';
begin
  var l := new List<string>;
  var skipNext := False;
  var skipUntilEnd := False;
  foreach var s in ReadLines(fname) do
  begin
    if '[Serializable]' in s then
      continue;
    if s.Trim.StartsWith('/// !! ')then
      continue;
    if s.Trim.StartsWith('// ') then
      continue;
    if s.Trim.StartsWith('//---')then
      continue;
      
    if s.Trim.StartsWith('///!#') then
      continue;
    
    if s.Trim.StartsWith('const ')then
      continue;
    if s.Trim.StartsWith('type ')then
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
  
  WriteLines('__PABC1.pas',l);
end;

function ClearParam(s: string): string;
begin
  var ind := Pos('<param',s);
  var ind2 := Pos('</param>',s,ind + 1);
  if (ind > 0) and (ind2 > 0) then
    s := '///';
  Result := s;
end;

procedure Step3;
const fname = '__PABC1.pas';
begin
  var l := new List<string>;
  var Prev: string := '';
  foreach var s in ReadLines(fname) do
  begin
    // Если есть summary, то взять только внутренность
    var s1 := s.Replace('<summary>','').Replace('</summary>','');
    // Если есть <param, то выбросить всю внутренность 
    s1 := ClearParam(s1);
    
    if Prev.StartsWith('/// ') and s1.StartsWith('///') and not s1.StartsWith('///-') then
    begin
      l[l.Count-1] := l[l.Count-1] + '' + s1.Remove(0,3).Trim;
    end
    else l.Add(s1);    
    Prev := s1;
  end;
  l := l.Where(s -> s <> '///').ToList;
  
  WriteLines('__PABC1-1.pas',l);
end;

procedure Step3_1(); // пропускать подряд идущие строки без /// или //>> в начале
const fname = '__PABC1-1.pas';
begin
  var l := new List<string>;
  var Prev: string := '';
  foreach var s in ReadLines(fname) do
  begin
    if Prev.StartsWith('///') or Prev.StartsWith('//>>') or s.StartsWith('///') or s.StartsWith('//>>') then
      l.Add(s);    
    Prev := s;
  end;

  WriteLines('__PABC2.pas',l);
end;

procedure Step4;
const fname = '__PABC2.pas';
begin
  var l := new List<string>;
  var p1 := '';
  var p2 := '';
  foreach var s in ReadLines(fname) do
  begin
    if s.StartsWith('///-') then
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
  
  WriteLines('__PABCdoc.pas',l);
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

type 
  FD = auto class
    fun,def: string;
  end;
  BFD = auto class
    basename: string;
    fds: List<FD>;
  end;
 
var dictClasses := new Dictionary<string,BFD>;

procedure ConvertFunDef(var fun: string);
begin
  fun := fun.Trim;
  
  if (fun.Length>0) and (fun.Last<>';') then
    fun += ';';

  fun := Regex.Replace(fun,'extensionmethod[^;]*;','');
  fun := Regex.Replace(fun,' read.*;','');
  fun := Regex.Replace(fun,' write.*;','');
  fun := Regex.Replace(fun,'\)\s*:=.*',')');
  fun := fun.DeleteFirst('var ');
  
  fun := fun.Trim;
  
  // Попробуем убрать := реализация
  // Это первое := на внешнем уровне (вне скобок)
  
  var p := fun.IndexOf(':=');
  while p>=0 do
  begin
    if fun[:p].Count(c->c='(')=fun[:p].Count(c->c=')') then 
      break;
    p := fun.IndexOf(':=',p+1);
  end;
  if p>=0 then
    fun := fun[:p];
  
  if (fun.Length>0) and (fun.Last<>';') then
    fun += ';';
end;

function CreateTableData(fds: List<FD>; Keywords: SortedSet<string>): string;
begin
  Result := '';
  for var i:=0 to fds.Count-1 do
  begin
    //funcs[i] := Regex.Replace(funcs[i],'\(\ *Self[^;\)]*\)','()');
    //funcs[i] := Regex.Replace(funcs[i],'\(\ *Self[^;]*;\ *','(');
    ConvertFunDef(fds[i].fun);
    var dd: string;
try
  //if fds[i].def.Length >= 3 then
    dd := fds[i].def.Remove(0,3).Trim;
  //else dd := fds[i].def; 
except
  Println(fds[i-3].fun,'!!!',fds[i-3].def);
  Println(fds[i-2].fun,'!!!',fds[i-2].def);
  Println(fds[i-1].fun,'!!!',fds[i-1].def);
  Println(fds[i-0].fun,'!!!',fds[i-0].def);
  Println('i=',i);
  halt;
end;    
    var td := fds[i].fun.Around('code')
      .Replace('System.IComparable','IComparable')
      .Replace('function','<b>function</b>')
      .Replace('procedure','<b>procedure</b>')
      .Replace('constructor','<b>constructor</b>')
      .Replace('class','<b>class</b>')
      .Replace('property','<b>property</b>')
      .Replace(' read',' <b>read</b>')
      .Replace(' write',' <b>write</b>')
      .Replace('virtual','<b>virtual</b>')
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
    if keywords<>nil then // =nil если мы добавляем имена предка и в Keywords их не надо добавлять 
    if not fds[i].fun.StartsWith('constructor') then
    begin
      var k := fds[i].fun.DeleteFirst('function ').DeleteFirst('procedure ').DeleteFirst('property ');
      k := k.MatchValue('\w+');
      keywords += k;
    end;
    td := td.Around('td').Around('tr')+NewLine;
    Result += td;
  end;
end;

function CreateSectionInClassDef(title: string; fds: sequence of System.Linq.IGrouping<string,FD>; Keywords: SortedSet<string>): string;
begin
  Result := '';
  if fds.Count=0 then
    exit;
  title := '<font style="font-size: 11pt">'+title+'</font>';
  title := '<td class="secttitle">'+title+'</td>';
  title := '<tr><td></td></tr>' + title.Around('tr');
  Result += title + NewLine;
  foreach var group in fds do
    Result += NewLine + CreateTableData(group.ToList,keywords).Around('tr');
end;

procedure GenerateHTMLFile(HelpFileName,Title: string; lst: List<string>);
begin
  var keywords := new SortedSet<string>;
  var s := '';
  var lst1 := lst.Select(s->s.Replace('<','&lt;').Replace('>','&gt;'));
  var funcs := lst1.Slice(1,2).ToList;
  var defs := lst1.Slice(0,2).ToList;
  
  // Вырезать из funcs строку где встречается class, его имя и предка

  var classdef,classname,classbase: string;  
  var ind := funcs.FindIndex(s ->s.Contains(' class') and not s.Trim.StartsWith('class'));
  if ind<>-1 then
  begin
    Println(funcs[ind]);
    var str := funcs[ind];

  //try
    //if defs[ind].ToString.Length>=3 then
      classdef := defs[ind].ToString.Remove(0,3).Trim;
    //else classdef := defs[ind].ToString;  
  {except
    //Println(defs[ind-0],'!!!');
    Println('ind=',ind,defs.Count);
    halt;
  end;    }
    
    funcs.RemoveAt(ind);
    defs.RemoveAt(ind);
    
    var ss := str.ToWords(' =()');
    classname := ss[0];
    if ss.Length>2 then
      classbase := ss[2];
    var q := classbase.ToWords(',');
    if q.Count>0 then
      classbase := q[0];      
    
    classdef := classdef + '.';
    if classbase<>'' then
      classdef += ' Базовый класс - '+ classbase.Around('code') + '.';
    s += classdef;
  end;
  
  var names := funcs.Select(s->begin 
    Result := ''; 
    var ss := s.Split(' ','<','(','&',':'); 
    if (ss[0]='class') then  // class function
      Result := ss[2] 
    else if (ss[0]='procedure') or (ss[0]='function') or (ss[0]='property') then 
      Result := ss[1] 
    else Result := ss[0]; 
  end).ToList;
  var ttt := funcs.ZipTuple(defs,names).OrderBy(t->t[2]);
  
  funcs := ttt.Select(t->t[0]).ToList;
  defs := ttt.Select(t->t[1]).ToList;
  
  if funcs.Count<>defs.Count then
  begin
    Println(HelpFileName,Title);
    writeln('Разное количество');
    halt;
  end;
  
  // Слить funcs defs в одну структуру FD
  var fds := funcs.Zip(defs,(f,d)->new FD(f,d)).ToList;

  var table := '';
  
  if classname<>'' then
  begin
    // пополнение словарика классов. Чтобы потом добавлять свойства и методы предков    
    dictClasses[classname] := new BFD(classbase,fds);
    Keywords += classname;
    
  // funcs надо отклассифицировать: конструкторы, свойства, методы, события
  // Каждую группу предварить заголовком
    var gr := fds.GroupBy(fd->fd.fun.ToWords[0].Trim);
    
    var cons := gr.Where(g->g.Key='constructor');
    table += CreateSectionInClassDef('Конструкторы класса '+classname,cons,Keywords);
    
    var prop := gr.Where(g->g.Key='property');
    table += CreateSectionInClassDef('Свойства класса '+classname,prop,Keywords);

    var pf := gr.Where(g->(g.Key='procedure') or (g.Key='function'));
    table += CreateSectionInClassDef('Методы класса '+classname,pf,Keywords);

    var cpf := gr.Where(g->(g.Key='class'));
    table += CreateSectionInClassDef('Статические методы класса '+classname,cpf,Keywords);
    
    while (classbase<>'') and dictClasses.ContainsKey(classbase) do
    begin
      gr := dictClasses[classbase].fds.GroupBy(fd->fd.fun.ToWords[0].Trim);
    // Добавить свойства предка
      prop := gr.Where(g->g.Key='property');
      table += CreateSectionInClassDef('Свойства базового класса '+classbase,prop,nil);
    // Добавить методы предка
      pf := gr.Where(g->(g.Key='procedure') or (g.Key='function'));
      table += CreateSectionInClassDef('Методы базового класса '+classbase,pf,nil);
      classbase := dictClasses[classbase].basename;
    end;
  end
  else
  begin
    table := CreateTableData(fds.ToList,keywords);
  end;
  table := NewLine+'<table border=0 cellpadding=5>' + table + '</table>'+NewLine;
  s += table;
  
  //AddToTable(fds,table,keywords);

  var keys := keywords.Select(s->'<param name="Keyword" value="'+s+'">').JoinIntoString(NewLine);
  keys := '<object type="application/x-oleobject" classid="clsid:1e2a7bd0-dab9-11d0-b93a-00c04fc99f9e">'
    + NewLine +keys
    + '</object>'+ NewLine ;
  keys += '<meta http-equiv="Content-Type" content="text/html; charset=windows-1251"><link rel="StyleSheet" href="../../../default.css">'+NewLine;
    
  s := keys.Around('HEAD')
    + (Title.Around('H1') + s).Around('body');
  //Println('..\'+RootOutputDirectory+'Files\'+HelpFileName+'.html');
  WriteAllText('..\'+RootOutputDirectory+'Files\'+HelpFileName+'.html',s.Around('HTML'));

  var s1 := '<param name="Name" value="' + Title + '">';
  var s2 := '<param name="Local" value="'+RootOutputDirectory+'Files\' + HelpFileName + '.html">';
  toc := toc + '<LI> <OBJECT type="text/sitemap">' + s1;
  toc := toc + s2;
  toc := toc + '</OBJECT>';
  
  hrefs := hrefs + ('<li><a href="Files\'+HelpFileName+'.html">'+Title+'</a></li>');
end;

procedure FinalStep;
begin
  var lines := ReadLines('__PABCdoc.pas').ToArray;
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