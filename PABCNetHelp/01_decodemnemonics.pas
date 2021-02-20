uses System.Collections.Generic;

var 
  mnem,f,f1: text;
  d := new Dictionary<string,char>;
  c: char;
  s: string;
  
function Convert(s: string): string;
begin
  foreach x: KeyValuePair<string,char> in d do
  begin
    var p := Pos(x.Key,s);
    while p>0 do
    begin
      Delete(s,p,Length(x.Key));
      Insert(x.Value,s,p);
      p := Pos(x.Key,s);
    end;  
  end;
  Result := s;
end;

begin
  assign(mnem,'mnemonicsrus.txt');
  reset(mnem);
  for c:='а' to 'я' do
  begin
    readln(mnem,s);
    d.Add(Trim(s),c);   
  end;  
  for c:='А' to 'Я' do
  begin
    readln(mnem,s);
    d.Add(Trim(s),c);   
  end;
  close(mnem);
  assign(f,'Table of Contents.hhc');
  assign(f1,'Table of Contents New.hhc');
  reset(f);
  rewrite(f1);
  while not eof(f) do
  begin
    readln(f,s);
    s := Convert(s);
    writeln(f1,s);
  end;
  close(f);
  close(f1);
end.