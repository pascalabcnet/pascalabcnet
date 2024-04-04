unit u_records7;

type
  intr = interface
    procedure p;
  end;
  
  r = record(intr)
    // Обязательно использовать тот же конструктор, что и в p1
    procedure p := new r;
  end;
  
// Обязательно принимать интерфейс
procedure p0(o: intr) := exit;

// Обязательно передать экземпляр r, не присвоенный ни одной переменной
// (можно использовать как простой обход)
procedure p1 := p0(new r);

end.