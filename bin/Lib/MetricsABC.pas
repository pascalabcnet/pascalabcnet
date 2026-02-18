unit MetricsABC;

interface

uses LinearAlgebraML;

type
  /// Метрики оценки качества моделей машинного обучения
  /// для задач регрессии и классификации.
  Metrics = static class
  public
    /// Среднеквадратичная ошибка (MSE).
    /// MSE = (1/n) * Σ (yTrue - yPred)^2.
    /// Чувствительна к большим отклонениям.
    static function MSE(yTrue, yPred: Vector): real;
    
    /// Средняя абсолютная ошибка (MAE).
    /// MAE = (1/n) * Σ |yTrue - yPred|.
    /// Менее чувствительна к выбросам, чем MSE.
    static function MAE(yTrue, yPred: Vector): real;
    
    /// Коэффициент детерминации (R²).
    /// R2 = 1 - SS_res / SS_tot.
    /// Показывает долю объяснённой дисперсии.
    /// R2 = 1 — идеальная модель,
    /// R2 = 0 — не лучше среднего,
    /// R2 < 0 — хуже константной модели.
    static function R2(yTrue, yPred: Vector): real;
    
// ---- Вероятностные метрики классификации ----

    /// Логарифмическая функция потерь (LogLoss).
    /// Оценивает качество вероятностных предсказаний.
    /// Сильно штрафует уверенные, но ошибочные прогнозы.
    /// Используется для моделей, возвращающих вероятности
    /// (например, LogisticRegression).
    static function LogLoss(yTrue, yProb: Vector): real;
  
    // ---- ROC / AUC ----
  
    /// Строит точки ROC-кривой по вероятностным предсказаниям.
    /// Возвращает кортеж из двух векторов:
    /// FPR (доля ложных положительных)
    /// и TPR (доля истинно положительных).
    static function ROC(yTrue, yProb: Vector): (Vector, Vector);
  
    /// Площадь под ROC-кривой (AUC).
    /// Значение от 0 до 1.
    /// 0.5 — случайная модель,
    /// 1 — идеальная модель.
    static function AUC(yTrue, yProb: Vector): real;
    
    /// Доля правильных предсказаний.
    /// Показывает, какая часть объектов классифицирована верно.
    /// Удобна, когда классы примерно сбалансированы.
    /// Пример: Accuracy = 0.9 означает 90% правильных ответов.
    static function Accuracy(yTrue, yPred: Vector): real;

    /// Точность (Precision) для положительного класса (1).
    /// Среди всех объектов, которые модель предсказала как 1,
    /// показывает долю действительно принадлежащих этому классу.
    /// Важна, когда критичны ложные положительные результаты («ложные тревоги»).
    static function Precision(yTrue, yPred: Vector): real;

    /// Полнота (Recall) для положительного класса (1).
    /// Среди всех объектов, которые в действительности равны 1,
    /// показывает долю правильно найденных моделью.
    /// Важна, когда нежелательны пропуски положительных случаев (например, заболеваний).
    static function Recall(yTrue, yPred: Vector): real;

    /// F1-мера.
    /// Объединённая метрика, учитывающая и Precision, и Recall одновременно.
    /// Высока только тогда, когда и Precision, и Recall высоки.
    /// Используется, когда классы сильно различаются по количеству элементов
    static function F1(yTrue, yPred: Vector): real;
  end;
  
type  
/// Матрица ошибок (Confusion Matrix) для бинарной классификации.
/// Хранит количество истинно положительных (TP),
/// истинно отрицательных (TN),
/// ложно положительных (FP)
/// и ложно отрицательных (FN) предсказаний.
/// Используется для анализа качества классификатора
/// и вычисления метрик Accuracy, Precision, Recall и F1
  ConfusionMatrix = class
  private
    fTP, fTN, fFP, fFN: integer;
  public
/// Создаёт матрицу ошибок по истинным меткам yTrue
/// и предсказаниям модели yPred.
/// Ожидается, что векторы содержат значения 0 и 1
/// и имеют одинаковую длину.
/// Подсчитывает TP, TN, FP и FN.
    constructor Create(yTrue, yPred: Vector);

    /// Истинно положительные (True Positives).
    /// Количество объектов класса 1,
    /// которые модель правильно предсказала как 1.
    property TP: integer read fTP;
    
    /// Истинно отрицательные (True Negatives).
    /// Количество объектов класса 0,
    /// которые модель правильно предсказала как 0.
    property TN: integer read fTN;
    
    /// Ложно положительные (False Positives).
    /// Количество объектов класса 0,
    /// которые модель ошибочно предсказала как 1.
    property FP: integer read fFP;
    
    /// Ложно отрицательные (False Negatives).
    /// Количество объектов класса 1,
    /// которые модель ошибочно предсказала как 0.
    property FN: integer read fFN;
    
    /// Доля правильных предсказаний.
    /// Показывает, какая часть объектов классифицирована верно.
    /// Удобна, когда классы примерно сбалансированы.
    /// Пример: Accuracy = 0.9 означает 90% правильных ответов.
    function Accuracy: real;
    /// Точность (Precision) для положительного класса (1).
    /// Среди всех объектов, которые модель предсказала как 1,
    /// показывает долю действительно принадлежащих этому классу.
    /// Важна, когда критичны ложные положительные результаты («ложные тревоги»).
    function Precision: real;
    /// Полнота (Recall) для положительного класса (1).
    /// Среди всех объектов, которые в действительности равны 1,
    /// показывает долю правильно найденных моделью.
    /// Важна, когда нежелательны пропуски положительных случаев (например, заболеваний).
    function Recall: real;
    /// F1-мера.
    /// Объединённая метрика, учитывающая и Precision, и Recall одновременно.
    /// Высока только тогда, когда и Precision, и Recall высоки.
    /// Используется, когда классы сильно различаются по количеству элементов
    function F1: real;
  end;
  
implementation  

uses MLExceptions;

//-----------------------------
//           Metrics
//-----------------------------

static function Metrics.MSE(yTrue, yPred: Vector): real;
begin
  if yTrue.Length <> yPred.Length then
    DimensionError(ER_DIM_MISMATCH, yTrue.Length, yPred.Length);

  var n := yTrue.Length;
  var s := 0.0;

  for var i := 0 to n - 1 do
  begin
    var d := yTrue[i] - yPred[i];
    s += d * d;
  end;

  Result := s / n;
end;

static function Metrics.MAE(yTrue, yPred: Vector): real;
begin
  if yTrue.Length <> yPred.Length then
    DimensionError(ER_DIM_MISMATCH, yTrue.Length, yPred.Length);

  var n := yTrue.Length;
  var s := 0.0;

  for var i := 0 to n - 1 do
    s += Abs(yTrue[i] - yPred[i]);

  Result := s / n;
end;

static function Metrics.R2(yTrue, yPred: Vector): real;
begin
  if yTrue.Length <> yPred.Length then
    DimensionError(ER_DIM_MISMATCH, yTrue.Length, yPred.Length);

  var n := yTrue.Length;
  var meanY := yTrue.Mean;

  var ssRes := 0.0;
  var ssTot := 0.0;

  for var i := 0 to n - 1 do
  begin
    var d1 := yTrue[i] - yPred[i];
    ssRes += d1 * d1;

    var d2 := yTrue[i] - meanY;
    ssTot += d2 * d2;
  end;

  if ssTot = 0 then
    exit(0.0);

  Result := 1 - ssRes / ssTot;
end;

static function Metrics.LogLoss(yTrue, yProb: Vector): real;
begin
  if yTrue.Length <> yProb.Length then
    DimensionError(ER_DIM_MISMATCH, yTrue.Length, yProb.Length);

  var n := yTrue.Length;
  var eps := 1e-15;
  var s := 0.0;

  for var i := 0 to n - 1 do
  begin
    var p := yProb[i];

    if p < eps then p := eps;
    if p > 1 - eps then p := 1 - eps;

    s += yTrue[i] * Ln(p) + (1 - yTrue[i]) * Ln(1 - p);
  end;

  Result := -s / n;
end;

static function Metrics.ROC(yTrue, yProb: Vector): (Vector, Vector);
begin
  if yTrue.Length <> yProb.Length then
    DimensionError(ER_DIM_MISMATCH, yTrue.Length, yProb.Length);

  var n := yTrue.Length;

  // Индексы, отсортированные по убыванию вероятностей
  var idx :=
  (0..n-1)
    .OrderByDescending(i -> yProb[i])
    .ToArray;

  var tp := 0;
  var fp := 0;

  var totalPos := 0;
  var totalNeg := 0;

  for var i := 0 to n-1 do
    if yTrue[i] = 1 then
      totalPos += 1
    else
      totalNeg += 1;

  var fprList := new List<real>;
  var tprList := new List<real>;

  for var k := 0 to n-1 do
  begin
    var i := idx[k];

    if yTrue[i] = 1 then
      tp += 1
    else
      fp += 1;

    var tpr := 0.0;
    var fpr := 0.0;

    if totalPos > 0 then
      tpr := tp / totalPos;

    if totalNeg > 0 then
      fpr := fp / totalNeg;

    tprList.Add(tpr);
    fprList.Add(fpr);
  end;

  Result := (new Vector(fprList.ToArray),
             new Vector(tprList.ToArray));
end;

static function Metrics.AUC(yTrue, yProb: Vector): real;
begin
  var (fpr, tpr) := ROC(yTrue, yProb);

  var area := 0.0;

  for var i := 1 to fpr.Length - 1 do
  begin
    var dx := fpr[i] - fpr[i - 1];
    var avgY := (tpr[i] + tpr[i - 1]) / 2;
    area += dx * avgY;
  end;

  Result := area;
end;

static function Metrics.Accuracy(yTrue, yPred: Vector): real;
begin
  Result := ConfusionMatrix.Create(yTrue, yPred).Accuracy;
end;

static function Metrics.Precision(yTrue, yPred: Vector): real;
begin
  Result := ConfusionMatrix.Create(yTrue, yPred).Precision;
end;

static function Metrics.Recall(yTrue, yPred: Vector): real;
begin
  Result := ConfusionMatrix.Create(yTrue, yPred).Recall;
end;

static function Metrics.F1(yTrue, yPred: Vector): real;
begin
  Result := ConfusionMatrix.Create(yTrue, yPred).F1;
end;


//-----------------------------
//        ConfusionMatrix
//-----------------------------

constructor ConfusionMatrix.Create(yTrue, yPred: Vector);
begin
  if yTrue.Length <> yPred.Length then
    DimensionError(ER_DIM_MISMATCH, yTrue.Length, yPred.Length);

  for var i := 0 to yTrue.Length - 1 do
  begin
    if yTrue[i] = 1 then
      if yPred[i] = 1 then
        fTP += 1
      else
        fFN += 1
    else
      if yPred[i] = 1 then
        fFP += 1
      else
        fTN += 1;
  end;
end;

function ConfusionMatrix.Accuracy: real;
begin
  var total := fTP + fTN + fFP + fFN;
  if total = 0 then
    exit(0.0);
  Result := (fTP + fTN) / total;
end;

function ConfusionMatrix.Precision: real;
begin
  if fTP + fFP = 0 then
    exit(0.0);
  Result := fTP / (fTP + fFP);
end;

function ConfusionMatrix.Recall: real;
begin
  if fTP + fFN = 0 then
    exit(0.0);
  Result := fTP / (fTP + fFN);
end;

function ConfusionMatrix.F1: real;
begin
  var p := Precision;
  var r := Recall;

  if p + r = 0 then
    exit(0.0);

  Result := 2 * p * r / (p + r);
end;

  
  
end.