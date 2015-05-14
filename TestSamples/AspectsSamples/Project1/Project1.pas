program Project1;

uses System;

begin
{$asp Aspect1}
  {$aspdata  Aspect1 Petrov 2.11}
   write('Aspect1');
  //Тело аспекта
  //
{$endasp Aspect1}

{$asp Aspect2}
  {$aspdata  Aspect2 Petrov 1.23}  
  var i2: boolean;
  //Тело аспекта
{$endasp Aspect2}

{$asp Aspect1}
  //Тело фрагмента аспекта
  //
{$endasp Aspect1}


{$asp Aspect3}
  var i3: boolean;
  //Тело аспекта
  //
{$endasp Aspect3}

end.
