unit my_unit1;

interface

{$asp Aspect1}
  write('real');
  //Тело фрагмента аспекта
  //
{$endasp Aspect1}


implementation
{$asp Aspect3}
  //Тело фрагмента аспекта 
  //
{$endasp Aspect3}

end.
