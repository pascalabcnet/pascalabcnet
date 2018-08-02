var dict := new Dictionary<string,string>;

procedure CollectInDir(path: string);
begin
  var ff := System.IO.Directory.EnumerateFiles(path,'*.html');
  foreach f: string in ff do
  begin
    var l := new List<string>;
    ReadLines(f).ForEach(
      procedure(s) -> 
      begin 
        if s.Contains('name="Keyword"') then
        begin
          var p := s.IndexOf('value="');
          var q := s.IndexOf('"',p+8);
          l.Add(Copy(s,p+8,q-p-7));          
        end;
      end);
    if l.Count>0 then
    begin
      dict[f.Substring(2)] := l.JoinIntoString('`\')+'`\';
    end;
  end;
  var dd := System.IO.Directory.EnumerateDirectories(path);
  foreach d: string in dd do
    CollectInDir(d);  
end;

begin
  CollectInDir('.');
  //dict.Println(NewLine);
  
  var f := ReadLines('WinChmWebHelp\Table of Contents.wcp',Encoding.UTF8);
  
  var name := '';
  
  f := f.Select(s->begin 
    if s.StartsWith('TitleList.Url') then
    begin
      var p := s.IndexOf('=');
      name := s.Substring(p+1);
    end;
    if s.StartsWith('TitleList.Keywords') then
    begin
      var p := s.IndexOf('=');
      if dict.ContainsKey(name) then 
      begin
        s := s.Substring(0,p+1)+dict[name];
      end;
    end;
    Result := s;
  end);
  WriteLines('WinChmWebHelp\Table of Contents.wcp',f.ToArray,new System.Text.UTF8Encoding(false));
end.