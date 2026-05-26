// В этом примере подбирается число кластеров для KMeans.
//
// Используются данные MakeBlobs с тремя естественными группами.
// Для каждого значения k выводится inertia:
// чем она меньше, тем ближе объекты к центрам своих кластеров.
//
// Идея метода:
// при увеличении k inertia всегда уменьшается,
// но после "правильного" числа кластеров выигрыш обычно становится небольшим.

uses MLABC;

begin
  var (X, yTrue) := Datasets.MakeBlobs(
    n := 300,
    centers := 3,
    nFeatures := 2,
    clusterStd := 0.8,
    centerBox := 6.0,
    shuffle := True,
    seed := 42
  );

  Println('Подбор числа кластеров для KMeans');
  Println;
  Println('k    inertia');
  Println('-' * 18);

  for var k := 2 to 6 do
  begin
    var model := new KMeans(k, seed := 42);
    model.Fit(X);
    Println($'{k,-4} {model.Inertia,10:F3}');
  end;

  Println;
  Println('Интерпретация результата:');
  Println('- Inertia всегда уменьшается при росте k.');
  Println('- Ищут момент, после которого уменьшение становится не таким заметным.');
  Println('- В этом примере естественно ожидать около 3 кластеров.');
end.
