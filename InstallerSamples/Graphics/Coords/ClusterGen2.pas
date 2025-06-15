{$reference Mathnet.Numerics.dll}
uses Mathnet.Numerics;
uses Coords;

function GenerateCluster(X, Y, spread: real; count: integer): array of Point;
begin
  var xx := Generate.Normal(count, X, spread);
  var yy := Generate.Normal(count, Y, spread);
  Result := ArrGen(count, i -> Pnt(xx[i],yy[i]));
end;

begin
  Window.Title := 'Генерация кластеров';
  var cluster1 := GenerateCluster(5, 3, 0.5, 90);
  var cluster2 := GenerateCluster(7, -6, 1.5, 105);
  var cluster3 := GenerateCluster(-7, 2, 1, 44);
  
  DrawPoints(cluster1,3);
  DrawPoints(cluster2,3);
  DrawPoints(cluster3,PointRadius := 3);
end.