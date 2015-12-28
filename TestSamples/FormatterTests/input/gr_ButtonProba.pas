uses GraphABC,ABCObjects,ABCButtons,Events;
 
var b: ButtonABC;

procedure Click;
begin
  b.Text:='Нажали!';
end;
 
begin
  b:=ButtonABC.Create(10,10,240,'Кнопка12345678901234567',clMoneyGreen);
  b := new ButtonABC(10,10,240,'Кнопка12345678901234567',clMoneyGreen);
  
  b.OnClick:=Click;
end.
