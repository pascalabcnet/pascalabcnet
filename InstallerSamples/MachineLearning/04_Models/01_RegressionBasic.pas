// Простейший пример линейной регрессии на синтетических данных.
//
// Датасет сгенерирован функцией MakeRegression:
// • признаки — случайные вещественные значения
// • зависимость — линейная комбинация признаков
// • добавлен небольшой гауссов шум
// • нелинейность отключена (nonlinearStrength = 0)
//
// Таким образом, модель полностью соответствует предположениям LinearRegression.
//
// Ожидаемый результат:
// • модель восстанавливает зависимость почти идеально
// • ошибка (MSE) близка к нулю
//
// Этот пример демонстрирует:
// • базовый pipeline регрессии (Fit → Predict → Metrics)
// • ситуацию "идеальной модели", когда данные линейны
// • нижнюю границу ошибки для линейных методов
uses MLABC;

begin
  var (X, y) := Datasets.MakeRegression(
    n := 300,
    nFeatures := 10,
    nInformative := 8,
    noise := 0.1,
    coefScale := 1.0,
    bias := 0.0,
    nonlinearStrength := 0.0,
    shuffle := True,
    seed := 42
  );

  var model := new LinearRegression;
  model.Fit(X, y);

  var yPred := model.Predict(X);
  
  Println('LinearRegression MSE:', Metrics.MSE(y, yPred):0:4);
end.