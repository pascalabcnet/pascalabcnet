begin
  match (1, 2, 'string') with
    (1, _, var x): Println(x);  
    (1, _, 'strin'): print(2);
    (_, _, 'string'): print(3);
  end;
end.