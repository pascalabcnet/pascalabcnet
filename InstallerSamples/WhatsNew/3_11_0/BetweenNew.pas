// 3.11. Внешняя Beetween
begin
  Between(6,2,6).Print;
  Between(6,2,6,inclusive := False).Println;

// Универсальная Between. Должен поддерживаться интерфейс IComparable<T>
  var dt1 := DateTime.Create(2025,04,16);
  var dt3 := DateTime.Now;
  Between(DateTime.Now,dt1,dt3).Print; 
  Between(DateTime.Now,dt1,dt3,inclusive := False).Println; 
end.