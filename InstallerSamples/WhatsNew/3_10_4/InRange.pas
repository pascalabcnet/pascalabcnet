begin
  // Поведение изменено!! Теперь диапазон [6,2] считается пустым
  5.InRange(6,2).Println; 

  // Универсальная InRange. Должен поддерживаться интерфейс IEnumerable<T>
  InRange(5,2,6).Println;

  var dt1 := DateTime.Create(2025,04,16);
  var dt2 := DateTime.Create(2025,04,29);
  var dt3 := DateTime.Now;
  InRange(dt2,dt1,dt3).Println; 
end.