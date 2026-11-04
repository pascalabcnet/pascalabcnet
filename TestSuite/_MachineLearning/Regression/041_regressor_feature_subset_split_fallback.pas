uses MLABC;
uses TestHelpers in '..\TestHelpers.pas';

type
  SubsetRegressor = class(DecisionTreeRegressor)
  public
    constructor Create(seed: integer; minLeaf: integer := 2);
    begin
      inherited Create(maxDepth := 1, minSamplesSplit := 2 * minLeaf,
        minSamplesLeaf := minLeaf, seed := seed);
      fMaxFeatures := 1;
    end;
  end;

begin
  var y := Vector([0.0, 0.0, 10.0, 10.0]);

  // Единственный пригодный признак должен быть найден, даже если
  // случайно выбранный признак постоянен или нарушает minSamplesLeaf.
  for var seed := 0 to 31 do
    for var scenario := 0 to 1 do
    begin
      var X: Matrix := [[0.0, 0.0, 0.0], [0.0, 0.0, 0.0],
        [0.0, 0.0, 1.0], [0.0, 0.0, 1.0]];
      if scenario = 1 then
        X[3, 0] := 1.0;

      var model := new SubsetRegressor(seed);
      model.Fit(X, y);
      var predictions := model.Predict(X);
      for var i := 0 to y.Length - 1 do
        Check(Abs(predictions[i] - y[i]) < 1e-9,
          $'Split fallback failed: seed={seed}, scenario={scenario}, row={i}');
    end;

  // Если допустимых разбиений нет вообще, поиск завершается листом.
  var constantX := new Matrix(4, 3);
  var constantModel := new SubsetRegressor(42);
  constantModel.Fit(constantX, y);
  var constantPredictions := constantModel.Predict(constantX);
  for var i := 0 to y.Length - 1 do
    Check(Abs(constantPredictions[i] - 5.0) < 1e-9,
      'Constant features must produce the mean target');

  // При наличии разбиения в выбранном подмножестве нельзя искать
  // более выгодный признак за его пределами: maxFeatures сохраняет смысл.
  var subsetSeed := 0;
  while (new System.Random(subsetSeed)).Next(2) <> 1 do
    subsetSeed += 1;
  var subsetX: Matrix := [[0.0, 0.0], [0.0, 1.0],
    [1.0, 0.0], [1.0, 1.0]];
  var subsetY := Vector([0.0, 2.0, 10.0, 12.0]);
  var subsetModel := new SubsetRegressor(subsetSeed);
  subsetModel.Fit(subsetX, subsetY);
  var subsetPredictions := subsetModel.Predict(subsetX);
  var expected := Arr(5.0, 7.0, 5.0, 7.0);
  for var i := 0 to expected.Length - 1 do
    Check(Abs(subsetPredictions[i] - expected[i]) < 1e-9,
      'A valid selected feature must not trigger fallback');
end.
