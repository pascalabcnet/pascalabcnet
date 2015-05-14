type
  cc=class
    {s:string;
    st1,st2='static strings!';static;
    i:integer=100;
    rl=1.1;
    f:file of integer;
    r:record x:integer end=(x:10);}
    class static_r:record x,y:integer end:=(x:9;y:11);
    class i:=99;
  end;
  

var c:cc;
    s:string;
begin
  //c:=new cc;
  {writeln(length(s));
  writeln(length(c.s));
  writeln(c.f);
  writeln(c.rl);
  writeln(c.r.x);
  writeln(c.i);
  writeln(cc.st1);
  writeln(cc.st2);}
  writeln(cc.static_r.x);//,' ',cc.static_r.y);
  writeln(cc.i);
  //нужен статический конструктор
  readln;
end.