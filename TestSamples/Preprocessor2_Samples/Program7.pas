//uses my_unit;

{$define GO}

begin

  {$ifndef PI} 
    {$ifndef PI}
        {$ifdef PI}  
        write('1'); 
        {$else}
        write('2');
        {$endif} 
    write('3');
    {$else} 
    writeln('4');
    {$endif}
    {$else} 
  writeln('else');
  {$endif}
  //writeln('end');
end.
