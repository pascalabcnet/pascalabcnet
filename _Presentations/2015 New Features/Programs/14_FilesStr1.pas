var 
  f: text;
  s,kind: string;
  num,p: integer;
  freq: real;
  ch: char;

begin
  assign(f,'freqs.txt');  
  reset(f);
  while not eof(f) do
  begin
    read(f,num,freq);
    read(f,ch);
    readln(f,s);
    p := Pos(' ',s);
    kind := Copy(s,p+1,100);
    s := Copy(s,1,p-1);
    if kind = 'verb' then
      writeln(s);
  end;
  close(f);
end.