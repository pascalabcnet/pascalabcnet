begin
  match (1, 2, 'string') with
    (1, _, 1): Println(1);  // Нет проверки типов каждого элемента tuple - надо ли?
    (1, _, 'strin'): print(2);
    (_,_,'string'): print(3);
  end;
end.