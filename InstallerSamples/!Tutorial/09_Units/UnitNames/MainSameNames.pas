// Алгоритм поиска имен в модулях
uses MyA,MyB; // Имена вначале ищутся в основной программе, а затем в модулях в порядке справа налево

begin
  p; // Вызывается MyB.p
  MyA.p; // Если надо вызвать MyA.p, следует использовать имя p, уточненное именем модуля
end.
