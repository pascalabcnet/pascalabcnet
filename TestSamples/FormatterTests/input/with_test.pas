type rec=record
        x:string:='rex.x';
        y:=10;
     end;
cl=class
        xc:string:='cl.xc';
        yc:=11;
     end;
var i:=1;
    j:=2;
    s:string;
    r:rec;
begin
  with i,j do
    writeln(ToString);
  with r,new cl,j do
    with i do 
      writeln(x,y,' ',xc,yc,' ',tostring);
end.