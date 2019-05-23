begin
  match (1, 2, 'string') with
    (1, _, 'str'): assert(false); 
    (1, _, 'strin'): assert(false);
    (_, _, 'string'): ;
  end;
end.