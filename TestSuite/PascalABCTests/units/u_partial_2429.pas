unit u_partial_2429;

interface

type
  t1 = partial class
    function f1: integer;
  end;
  
implementation

type
  t0<T> = class
    function f0 := 5;
  end;
  //Ошибка: Неизвестное имя 't1`1'
  t1 = partial class(t0<byte>)
    
  end;

function t1.f1 := self.f0;
  
begin end.