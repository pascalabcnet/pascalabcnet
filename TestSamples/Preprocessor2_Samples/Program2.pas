//uses my_unit;

{$define GO}

begin

  {$ifdef PI} 
    {$ifndef PI}
    write('a');
    {$else} 
    writeln('else');
    {$endif}
  {$endif}

end.