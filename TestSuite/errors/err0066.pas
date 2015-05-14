procedure Test<U,V>(a : U; b : V);
begin
end;

procedure Test<U,V>(a : V; b : U);
begin
end;
    
begin
Test&<integer,integer>(2,3);
end.