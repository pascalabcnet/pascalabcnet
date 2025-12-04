function f1<TArray>: TArray;
where TArray: System.Array;
begin
  
  Result := TArray(System.Array(
    new integer[10]
  ));
  
  Result.Length.Println; // до #1986 выводило 107, не зависимо от массива выше
  
end;

begin
  
  f1&<array of integer>;
  
//  readln;
end.