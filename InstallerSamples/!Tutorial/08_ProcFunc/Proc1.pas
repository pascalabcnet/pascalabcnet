// Параметры процедур

procedure Operations(a,b: integer);
begin
  Println(a,'+',b,'=',a+b);
  Println(a,'-',b,'=',a-b);
  Println(a,'*',b,'=',a*b);
  Println(a,'/',b,'=',a/b);
  Println(a,'div',b,'=',a div b);
  Println(a,'mod',b,'=',a mod b);
end;

begin
  Operations(5,3);
  Println;
  Operations(7,4);
end.  