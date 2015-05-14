
type student=class
public
name:string;
height:integer;

constructor create(name:string; height:integer);
begin
 self.name:=name;
 self.height:=height;
end;

procedure print;
begin
 writeln('Student: ',name,' height: ',height);
end;

class function operator<(left,right:student):boolean;
begin
 if (left.height<right.height) then
 begin
  result:=true;
 end
 else
 begin
  result:=false;
 end;
end;

function ToString:string;override;
begin
  //Это почему не можеш?
  result:=string.Format('{0}({1})',name,height);
end;

end;

var
s1,s2:student;

begin
 s1:=student.create('Stepa Morkovkin',188);
 s2:=student.create('Petya Pomidorov',180);
if (s1<s2) then
writeln(s1,'<',s2)
else
writeln(s1,'>=',s2);
end.