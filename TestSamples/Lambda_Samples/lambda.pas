type _class=class
    function f:integer;
    begin
      result:=integer(((x,y)=>integer(x)*integer(y))(3,7))+
              integer(((x,y)=>integer(x)*integer(y)+integer((x=>x)(8)))(3,7));
    end;
    procedure p;
    begin
      var a:integer;
      a:=integer(((x,y)=>integer(x)*integer(y))(6,4))+
              integer(((x,y)=>integer(x)*integer(y)+integer((x=>x)(3)))(6,4));
      writeln(a);        
    end;
end;
function f:integer;
procedure pp;
begin
  writeln('hello!');
end;
begin
  pp;
  result:=integer(((x,y)=>integer(x)*integer(y))(3,7))+
          integer(((x,y)=>integer(x)*integer(y)+integer((x=>x)(8)))(3,7));
end;
procedure p;
begin
  var a:integer;
  a:=integer(((x,y)=>integer(x)*integer(y))(6,4))+
          integer(((x,y)=>integer(x)*integer(y)+integer((x=>x)(3)))(6,4));
  writeln(a);        
end;
begin
  var c:_class;
  c:=new _class();
  writeln(c.f);
  c.p;
  writeln(f);
  p;
  var a:integer;
  a:=integer(((x,y)=>integer(x)*integer(y)+integer((x=>x)(8)))(5,4))+
     integer(((x,y)=>integer(x)*integer(y)+integer((x=>x)(8)))(5,4));
  writeln(a);
  var bb := (x,y)=>integer(x)*integer(y);
  writeln(bb(9,8));
  var cc := x=>x;
  writeln(cc(9));
end.