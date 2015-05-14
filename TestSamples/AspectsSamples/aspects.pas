begin
  {$include asp_inc.pas}
  writeln('1');
{$asp test}
  writeln('2');
{$endasp test}
{$asp test1}
  writeln();//THIS IS ASPECT111
  {$aspdata test1 orlov 1.1}
  var J: integer;
  for J:=10 to 20 do
    writeln(J);
    {$endasp test1}
  writeln('Hello');  
  writeln('world!');
{$asp write}
  writeln('this is aspect write');
{$endasp write}
{$asp newaspect}
  {$aspdata newaspect orlov 1.2}
  writeln('newaspect');
{$endasp newaspect}
end.