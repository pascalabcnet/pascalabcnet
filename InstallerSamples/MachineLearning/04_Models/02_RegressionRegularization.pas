// Пример влияния регуляризации (RidgeRegression) на линейную модель
// в задаче с большим числом признаков и относительно малым числом объектов.
//
// Датасет:
// • 20 признаков, из которых информативны только 3, остальные — шум
// • добавлен существенный гауссов шум (noise = 2.0)
// • зависимость остаётся линейной, но сигнал частично скрыт шумом
//
// Такая постановка приводит к переобучению:
// модель может подстраиваться под шум и случайные колебания признаков.
//
// Наблюдения:
//
// • LinearRegression:
//   – достигает меньшей ошибки на обучающей выборке (лучше подгонка)
//   – но заметно хуже работает на тесте (переобучение)
//
// • RidgeRegression:
//   – даёт большую ошибку на обучении (штраф за коэффициенты)
//   – но существенно лучше обобщает и снижает ошибку на тесте
//
// Интерпретация:
//
// • обычная линейная регрессия переобучается на шуме и лишних признаках
// • Ridge вводит L2-регуляризацию и "сжимает" коэффициенты модели
// • регуляризация подавляет влияние неинформативных признаков
// • в результате модель становится более устойчивой к новым данным
//
// Этот пример демонстрирует:
// • разницу между train и test ошибками как признак переобучения
// • влияние лишних признаков на качество модели
// • практическую пользу регуляризации в задачах с шумом и мультиколлинеарностью
uses MLABC;

function Normal(rnd: System.Random): real;
begin
  var u1 := rnd.NextDouble;
  var u2 := rnd.NextDouble;
  if u1 < 1e-12 then
    u1 := 1e-12;
  Result := Sqrt(-2 * Ln(u1)) * Cos(2 * Pi * u2);
end;

begin
  var (X, y) := Datasets.MakeRegression(
    n := 100,
    nFeatures := 20,
    nInformative := 3,
    noise := 2.0,
    coefScale := 1.0,
    bias := 0.0,
    nonlinearStrength := 0.0,
    shuffle := True,
    seed := 42
  );

  // --- добавляем корреляцию
  var rnd := new System.Random(1);
  for var i := 0 to X.RowCount - 1 do
  begin
    X[i, 1] := X[i, 0] + 0.01 * Normal(rnd);
    X[i, 2] := X[i, 0] - 0.01 * Normal(rnd);
  end;

  var (XTrain, XTest, yTrain, yTest) := Validation.TrainTestSplit(
    X, y,
    testRatio := 0.3,
    seed := 42
  );

  var lr := new LinearRegression;
  lr.Fit(XTrain, yTrain);

  var ridge := new RidgeRegression(50.0);
  ridge.Fit(XTrain, yTrain);

  var yTrainLR := lr.Predict(XTrain);
  var yTestLR  := lr.Predict(XTest);

  var yTrainRidge := ridge.Predict(XTrain);
  var yTestRidge  := ridge.Predict(XTest);

  Println('--- Train ---');
  Println($'Linear MSE: {Metrics.MSE(yTrain, yTrainLR),0:F4}');
  Println($'Ridge  MSE: {Metrics.MSE(yTrain, yTrainRidge),0:F4}');

  Println;
  Println('--- Test ---');
  Println($'Linear MSE: {Metrics.MSE(yTest, yTestLR),0:F4}');
  Println($'Ridge  MSE: {Metrics.MSE(yTest, yTestRidge),0:F4}');
end.