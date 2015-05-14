{$asp test}
     write('this is include aspect (fragment of test)');
{$endasp test}
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
{$asp write}
  write('this is aspect write');
{$endasp write}
{$asp newaspect}
  {$aspdata newaspect orlov 1.2}
  write('newaspect');
{$endasp newaspect}
