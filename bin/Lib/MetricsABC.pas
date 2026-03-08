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
    
    /// Вычисляет среднеквадратичную ошибку (Root Mean Squared Error, RMSE)
    /// между истинными и предсказанными значениями.
    /// RMSE равна квадратному корню из MSE и измеряется
    /// в тех же единицах, что и целевая переменная.
    static function RMSE(yTrue, yPred: Vector): real;
    
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
    /// Объединённая метрика, учитывающая одновременно Precision и Recall.
    /// Высока только тогда, когда и Precision, и Recall имеют высокие значения.
    /// Особенно полезна при несбалансированных классах.
    static function F1(yTrue, yPred: Vector): real;
    
    /// Specificity (True Negative Rate).
    /// Доля правильно предсказанных отрицательных объектов:
    /// TN / (TN + FP).
    /// Показывает способность модели корректно распознавать отрицательный класс.
    static function Specificity(yTrue, yPred: Vector): real;
    
    /// Balanced Accuracy.
    /// Среднее арифметическое Recall (чувствительности)
    /// и Specificity (доли истинно отрицательных).
    /// Устойчива к дисбалансу классов.
    static function BalancedAccuracy(yTrue, yPred: Vector): real;
    
    /// Вычисляет силуэт (Silhouette) для каждой точки.
    /// Для каждой строки матрицы X возвращает значение s(i) в диапазоне [-1, 1].
    /// Требует как минимум 2 различных кластера.
    /// Использует евклидово расстояние (квадратный корень не извлекается).
    /// Сложность O(n²) - для умеренных размеров данных.
    static function SilhouetteSamples(X: Matrix; labels: Vector): Vector;
  
    /// Вычисляет средний силуэт (Silhouette Score).
    /// Возвращает среднее значение Silhouette по всем точкам.
    /// Чем ближе к 1 — тем лучше разделены кластеры.
    /// Значение около 0 означает пересечение кластеров.
    /// Отрицательное значение указывает на возможную ошибку кластеризации
    static function SilhouetteScore(X: Matrix; labels: Vector): real;
    
    /// Вычисляет индекс Калински–Харабаса (Calinski–Harabasz Index).
    /// Возвращает отношение межкластерной дисперсии к внутрикластерной дисперсии.
    /// Чем больше значение — тем лучше разделены кластеры.
    /// Требует как минимум 2 кластера и n > k.
    /// Сложность O(n·p).
    static function CalinskiHarabaszScore(X: Matrix; labels: Vector): real;
    
    /// Вычисляет индекс Дэвиса–Булдина (Davies–Bouldin Index).
    /// Чем меньше значение — тем лучше разделены кластеры.
    /// Требует минимум 2 кластера.
    /// Сложность O(n·p + k²·p).
    static function DaviesBouldinScore(X: Matrix; labels: Vector): real;
    
    /// Adjusted Rand Index (ARI).
    /// Внешняя метрика качества кластеризации.
    /// Сравнивает предсказанные кластеры с истинными метками.
    ///
    /// Основана на анализе всех пар объектов:
    ///   проверяет, совпадает ли решение алгоритма и истинная разметка
    ///   в вопросе "находятся ли две точки в одном кластере".
    ///
    /// Корректирует обычный Rand Index на случайные совпадения.
    ///
    /// Диапазон значений:
    /// 1   — идеальное совпадение кластеризации и истинных меток
    /// 0   — случайное разбиение
    /// <0  — хуже случайного
    static function AdjustedRandIndex(yTrue, yPred: Vector): real;
  end;
  
type  
/// Матрица ошибок (Confusion Matrix) для бинарной классификации.
/// Хранит количество истинно положительных (TP),
///   истинно отрицательных (TN),
///   ложно положительных (FP)
///   и ложно отрицательных (FN) предсказаний.
/// Используется для анализа качества классификатора
///   и вычисления метрик Accuracy, Precision, Recall и F1
  ConfusionMatrix = class
  private
    fTP, fTN, fFP, fFN: integer;
  public
/// Создаёт матрицу ошибок по истинным меткам yTrue и предсказаниям модели yPred.
/// Ожидается, что векторы содержат значения 0 и 1 и имеют одинаковую длину.
/// Подсчитывает TP, TN, FP и FN
    constructor Create(yTrue, yPred: Vector);

    /// Истинно положительные (True Positives).
    /// Количество объектов класса 1, которые модель правильно предсказала как 1.
    property TP: integer read fTP;
    
    /// Истинно отрицательные (True Negatives).
    /// Количество объектов класса 0, которые модель правильно предсказала как 0.
    property TN: integer read fTN;
    
    /// Ложно положительные (False Positives).
    /// Количество объектов класса 0, которые модель ошибочно предсказала как 1.
    property FP: integer read fFP;
    
    /// Ложно отрицательные (False Negatives).
    /// Количество объектов класса 1, которые модель ошибочно предсказала как 0.
    property FN: integer read fFN;
    
    /// Доля правильных предсказаний.
    /// Показывает, какая часть объектов классифицирована верно.
    /// Удобна, когда классы примерно сбалансированы.
    /// Пример: Accuracy = 0.9 означает 90% правильных ответов.
    function Accuracy: real;
    /// Точность (Precision) для положительного класса (1).
    /// Среди всех объектов, которые модель предсказала как 1,
    ///   показывает долю действительно принадлежащих этому классу.
    /// Важна, когда критичны ложные положительные результаты («ложные тревоги»).
    function Precision: real;
    /// Полнота (Recall) для положительного класса (1).
    /// Среди всех объектов, которые в действительности равны 1,
    ///   показывает долю правильно найденных моделью.
    /// Важна, когда нежелательны пропуски положительных случаев (например, заболеваний).
    function Recall: real;
    /// F1-мера.
    /// Объединённая метрика, учитывающая и Precision, и Recall одновременно.
    /// Высока только тогда, когда и Precision, и Recall высоки.
    /// Используется, когда классы сильно различаются по количеству элементов
    function F1: real;
    
    function Specificity: real;
    function BalancedAccuracy: real;
  end;
  
implementation  

uses MLExceptions;

const
  ER_INVALID_VALUE =
    'Некорректное значение {0} на позиции {1}!!Invalid value in {0} at index {1}';
  ER_DIV_BY_ZERO =
    'Деление на ноль в {0}!!Division by zero in {0}';
  ER_INVALID_CLASS_LABEL =
    'Некорректная метка класса: {0}!!Invalid class label: {0}';
  ER_STRATIFIED_LABELS_INVALID =
    'Метки кластеров должны быть целыми числами!!Cluster labels must be integers';
  ER_SILHOUETTE_NEEDS_2_CLUSTERS =
    'Для вычисления Silhouette требуется минимум 2 кластера!!Silhouette requires at least 2 clusters';
  ER_INVALID_VALUE_AT =
    'Некорректное значение в {0} на позиции {1}!!Invalid value in {0} at index {1}';
  ER_CH_NEEDS_2_CLUSTERS =
    'Для Calinski–Harabasz требуется минимум 2 кластера!!Calinski–Harabasz requires at least 2 clusters';
  ER_CH_NEEDS_N_GT_K =
    'Для Calinski–Harabasz требуется n > k!!Calinski–Harabasz requires n > k';
  ER_LABELS_MUST_BE_INTEGERS =
    'Метки кластеров должны быть целыми числами!!Cluster labels must be integers';    
//-----------------------------
//           Metrics
//-----------------------------

static function Metrics.MSE(yTrue, yPred: Vector): real;
begin
  if yTrue = nil then
    ArgumentNullError(ER_ARG_NULL, 'yTrue');

  if yPred = nil then
    ArgumentNullError(ER_ARG_NULL, 'yPred');

  var n := yTrue.Length;

  if n <> yPred.Length then
    DimensionError(ER_DIM_MISMATCH, yTrue.Length, yPred.Length);

  if n = 0 then
    ArgumentError(ER_EMPTY_DATA, 'MSE');

  var sum := 0.0;

  for var i := 0 to n - 1 do
  begin
    var yt := yTrue[i];
    var yp := yPred[i];

    if double.IsNaN(yt) or double.IsInfinity(yt) then
      ArgumentError(ER_INVALID_VALUE, 'yTrue', i);

    if double.IsNaN(yp) or double.IsInfinity(yp) then
      ArgumentError(ER_INVALID_VALUE, 'yPred', i);

    var d := yt - yp;
    sum += d * d;
  end;

  Result := sum / n;
end;

static function Metrics.RMSE(yTrue, yPred: Vector): real;
begin
  Result := Sqrt(MSE(yTrue, yPred));
end;

static function Metrics.MAE(yTrue, yPred: Vector): real;
begin
  if yTrue = nil then
    ArgumentNullError(ER_ARG_NULL, 'yTrue');

  if yPred = nil then
    ArgumentNullError(ER_ARG_NULL, 'yPred');

  var n := yTrue.Length;

  if n <> yPred.Length then
    DimensionError(ER_DIM_MISMATCH, yTrue.Length, yPred.Length);

  if n = 0 then
    ArgumentError(ER_EMPTY_DATA, 'MAE');

  var s := 0.0;

  for var i := 0 to n - 1 do
  begin
    var yt := yTrue[i];
    var yp := yPred[i];

    if double.IsNaN(yt) or double.IsInfinity(yt) then
      ArgumentError(ER_INVALID_VALUE, 'yTrue', i);

    if double.IsNaN(yp) or double.IsInfinity(yp) then
      ArgumentError(ER_INVALID_VALUE, 'yPred', i);

    s += Abs(yt - yp);
  end;

  Result := s / n;
end;

static function Metrics.R2(yTrue, yPred: Vector): real;
begin
  if yTrue = nil then
    ArgumentNullError(ER_ARG_NULL, 'yTrue');

  if yPred = nil then
    ArgumentNullError(ER_ARG_NULL, 'yPred');

  var n := yTrue.Length;

  if n <> yPred.Length then
    DimensionError(ER_DIM_MISMATCH, yTrue.Length, yPred.Length);

  if n = 0 then
    ArgumentError(ER_EMPTY_DATA, 'R2');

  var sumY := 0.0;

  for var i := 0 to n - 1 do
  begin
    var yt := yTrue[i];
    if double.IsNaN(yt) or double.IsInfinity(yt) then
      ArgumentError(ER_INVALID_VALUE, 'yTrue', i);

    sumY += yt;
  end;

  var meanY := sumY / n;

  var ssRes := 0.0;
  var ssTot := 0.0;

  for var i := 0 to n - 1 do
  begin
    var yt := yTrue[i];
    var yp := yPred[i];

    if double.IsNaN(yp) or double.IsInfinity(yp) then
      ArgumentError(ER_INVALID_VALUE, 'yPred', i);

    var d1 := yt - yp;
    ssRes += d1 * d1;

    var d2 := yt - meanY;
    ssTot += d2 * d2;
  end;

  if ssTot = 0.0 then
  begin
    Result := 0.0;
    exit;
  end;

  Result := 1 - ssRes / ssTot;
end;

static function Metrics.LogLoss(yTrue, yProb: Vector): real;
begin
  if yTrue = nil then
    ArgumentNullError(ER_ARG_NULL, 'yTrue');

  if yProb = nil then
    ArgumentNullError(ER_ARG_NULL, 'yProb');

  var n := yTrue.Length;

  if n <> yProb.Length then
    DimensionError(ER_DIM_MISMATCH, yTrue.Length, yProb.Length);

  if n = 0 then
    ArgumentError(ER_EMPTY_DATA, 'LogLoss');

  var eps := 1e-15;
  var s := 0.0;

  for var i := 0 to n - 1 do
  begin
    var yt := yTrue[i];
    var p := yProb[i];

    if double.IsNaN(yt) or double.IsInfinity(yt) then
      ArgumentError(ER_INVALID_VALUE, 'yTrue', i);

    if double.IsNaN(p) or double.IsInfinity(p) then
      ArgumentError(ER_INVALID_VALUE, 'yProb', i);

    if (yt <> 0.0) and (yt <> 1.0) then
      ArgumentError(ER_INVALID_CLASS_LABEL, yt);

    if p < eps then p := eps;
    if p > 1.0 - eps then p := 1.0 - eps;

    s += yt * Ln(p) + (1.0 - yt) * Ln(1.0 - p);
  end;

  Result := -s / n;
end;

static function Metrics.ROC(yTrue, yProb: Vector): (Vector, Vector);
begin
  if yTrue = nil then
    ArgumentNullError(ER_ARG_NULL, 'yTrue');

  if yProb = nil then
    ArgumentNullError(ER_ARG_NULL, 'yProb');

  var n := yTrue.Length;

  if n <> yProb.Length then
    DimensionError(ER_DIM_MISMATCH, yTrue.Length, yProb.Length);

  if n = 0 then
    ArgumentError(ER_EMPTY_DATA, 'ROC');

  var totalPos := 0;
  var totalNeg := 0;

  for var i := 0 to n - 1 do
  begin
    var yt := yTrue[i];
    var p := yProb[i];

    if double.IsNaN(yt) or double.IsInfinity(yt) then
      ArgumentError(ER_INVALID_VALUE, 'yTrue', i);

    if double.IsNaN(p) or double.IsInfinity(p) then
      ArgumentError(ER_INVALID_VALUE, 'yProb', i);

    if (yt <> 0.0) and (yt <> 1.0) then
      ArgumentError(ER_INVALID_CLASS_LABEL, yt);

    if yt = 1.0 then
      totalPos += 1
    else
      totalNeg += 1;
  end;

  if (totalPos = 0) or (totalNeg = 0) then
    ArgumentError(ER_DIV_BY_ZERO, 'ROC');

  var idx :=
    (0..n-1)
      .OrderByDescending(i -> yProb[i])
      .ToArray;

  var tp := 0;
  var fp := 0;

  var fprList := new List<real>;
  var tprList := new List<real>;

  for var k := 0 to n - 1 do
  begin
    var i := idx[k];

    if yTrue[i] = 1.0 then
      tp += 1
    else
      fp += 1;

    var tpr := tp / totalPos;
    var fpr := fp / totalNeg;

    tprList.Add(tpr);
    fprList.Add(fpr);
  end;

  Result := (new Vector(fprList.ToArray),
             new Vector(tprList.ToArray));
end;

static function Metrics.AUC(yTrue, yProb: Vector): real;
begin
  if yTrue = nil then
    ArgumentNullError(ER_ARG_NULL, 'yTrue');

  if yProb = nil then
    ArgumentNullError(ER_ARG_NULL, 'yProb');

  var n := yTrue.Length;

  if n <> yProb.Length then
    DimensionError(ER_DIM_MISMATCH, yTrue.Length, yProb.Length);

  if n = 0 then
    ArgumentError(ER_EMPTY_DATA, 'AUC');

  var (fpr, tpr) := ROC(yTrue, yProb);

  if fpr.Length <> tpr.Length then
    DimensionError(ER_DIM_MISMATCH, fpr.Length, tpr.Length);

  if fpr.Length < 2 then
    ArgumentError(ER_EMPTY_DATA, 'AUC');

  var area := 0.0;

  for var i := 1 to fpr.Length - 1 do
  begin
    var dx := fpr[i] - fpr[i - 1];

    if double.IsNaN(dx) or double.IsInfinity(dx) then
      ArgumentError(ER_INVALID_VALUE, 'fpr', i);

    var avgY := (tpr[i] + tpr[i - 1]) / 2;

    if double.IsNaN(avgY) or double.IsInfinity(avgY) then
      ArgumentError(ER_INVALID_VALUE, 'tpr', i);

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

static function Metrics.Specificity(yTrue, yPred: Vector): real;
begin
  Result := ConfusionMatrix.Create(yTrue, yPred).Specificity;
end;

static function Metrics.BalancedAccuracy(yTrue, yPred: Vector): real;
begin
  Result := ConfusionMatrix.Create(yTrue, yPred).BalancedAccuracy;
end;

static function Metrics.SilhouetteSamples(X: Matrix; labels: Vector): Vector;
begin
  if X = nil then
    ArgumentNullError(ER_ARG_NULL, 'X');
  if labels = nil then
    ArgumentNullError(ER_ARG_NULL, 'labels');

  var n := X.RowCount;
  var p := X.ColCount;

  if n = 0 then
    ArgumentError(ER_EMPTY_DATA, 'SilhouetteSamples');

  if labels.Length <> n then
    DimensionError(ER_DIM_MISMATCH, labels.Length, n);

  // --- собираем размеры кластеров и маппинг label -> 0..c-1
  var map := new Dictionary<integer, integer>();
  var counts := new List<integer>();
  var clsId := new integer[n];

  for var i := 0 to n - 1 do
  begin
    var v := labels[i];
    var lab := integer(v);

    if Abs(v - lab) > 1e-12 then
      ArgumentError(ER_LABELS_MUST_BE_INTEGERS);

    var id: integer;
    if map.TryGetValue(lab, id) then
    begin
      clsId[i] := id;
      counts[id] += 1;
    end
    else
    begin
      id := map.Count;
      map.Add(lab, id);
      clsId[i] := id;
      counts.Add(1);
    end;
  end;

  var cCount := counts.Count;
  if cCount < 2 then
    ArgumentError(ER_SILHOUETTE_NEEDS_2_CLUSTERS);

  // --- список уникальных кластеров, чтобы не обращаться к Dictionary в hot-path
  var sizes := counts.ToArray;

  // --- результат
  Result := new Vector(n);

  // --- рабочие массивы (переиспользуем, без аллокаций в цикле по i)
  var sumToCluster := new real[cCount];

  for var i := 0 to n - 1 do
  begin
    // обнуляем суммы
    for var c := 0 to cCount - 1 do
      sumToCluster[c] := 0.0;

    // суммируем расстояния от i до всех остальных по кластерам
    for var t := 0 to n - 1 do
      if t <> i then
      begin
        var dist := 0.0;

        for var j := 0 to p - 1 do
        begin
          var a := X[i,j];
          if double.IsNaN(a) or double.IsInfinity(a) then
            ArgumentError(ER_INVALID_VALUE_AT, 'X', i);

          var b := X[t,j];
          if double.IsNaN(b) or double.IsInfinity(b) then
            ArgumentError(ER_INVALID_VALUE_AT, 'X', t);

          var d := a - b;
          dist += d * d;
        end;

        sumToCluster[clsId[t]] += Sqrt(dist);
      end;

    var my := clsId[i];

    // a(i): среднее расстояние до своего кластера (без самой точки)
    var aMean := 0.0;
    if sizes[my] > 1 then
      aMean := sumToCluster[my] / (sizes[my] - 1)
    else
      aMean := 0.0;

    // b(i): минимальное среднее расстояние до другого кластера
    var bMean := double.MaxValue;

    for var c := 0 to cCount - 1 do
      if c <> my then
      begin
        if sizes[c] = 0 then
          continue;

        var mean := sumToCluster[c] / sizes[c];
        if mean < bMean then
          bMean := mean;
      end;

    if double.IsInfinity(bMean) then
      bMean := 0.0;

    var denom := Max(aMean, bMean);
    if denom = 0 then
      Result[i] := 0.0
    else
      Result[i] := (bMean - aMean) / denom;
  end;
end;

static function Metrics.SilhouetteScore(X: Matrix; labels: Vector): real;
begin
  var s := SilhouetteSamples(X, labels);

  var n := s.Length;
  var sum := 0.0;

  for var i := 0 to n - 1 do
    sum += s[i];

  if n = 0 then
    exit(0.0);

  Result := sum / n;
end;

/// Вычисляет индекс Калински–Харабаса (Calinski–Harabasz Index).
/// Возвращает отношение межкластерной дисперсии к внутрикластерной дисперсии.
/// Чем больше значение — тем лучше разделены кластеры.
/// При нулевой внутрикластерной дисперсии возвращает +∞.
/// Требует как минимум 2 кластера и n > k.
/// Сложность O(n·p).
static function Metrics.CalinskiHarabaszScore(X: Matrix; labels: Vector): real;
begin
  if X = nil then
    ArgumentNullError(ER_ARG_NULL, 'X');

  if labels = nil then
    ArgumentNullError(ER_ARG_NULL, 'labels');

  var n := X.RowCount;
  var p := X.ColCount;

  if n = 0 then
    ArgumentError(ER_EMPTY_DATA, 'CalinskiHarabaszScore');

  if labels.Length <> n then
    DimensionError(ER_DIM_MISMATCH, labels.Length, n);

  // --- label -> 0..k-1 + sizes
  var map := new Dictionary<integer, integer>();
  var counts := new List<integer>();
  var clsId := new integer[n];

  for var i := 0 to n - 1 do
  begin
    var v := labels[i];
    var lab := integer(v);

    if Abs(v - lab) > 1e-12 then
      ArgumentError(ER_LABELS_MUST_BE_INTEGERS);

    var id: integer;
    if map.TryGetValue(lab, id) then
    begin
      clsId[i] := id;
      counts[id] += 1;
    end
    else
    begin
      id := map.Count;
      map.Add(lab, id);
      clsId[i] := id;
      counts.Add(1);
    end;
  end;

  var k := counts.Count;

  if k < 2 then
    ArgumentError(ER_CH_NEEDS_2_CLUSTERS);

  if k >= n then
    ArgumentError(ER_CH_NEEDS_N_GT_K);

  var sizes := counts.ToArray;

  // --- глобальное среднее
  var globalMean := new real[p];

  for var i := 0 to n - 1 do
    for var j := 0 to p - 1 do
    begin
      var xij := X[i,j];
      if double.IsNaN(xij) or double.IsInfinity(xij) then
        ArgumentError(ER_INVALID_VALUE_AT, 'X', i);
      globalMean[j] += xij;
    end;

  for var j := 0 to p - 1 do
    globalMean[j] /= n;

  // --- центры кластеров
  var centers := new Matrix(k, p);

  for var i := 0 to n - 1 do
  begin
    var c := clsId[i];
    for var j := 0 to p - 1 do
      centers[c,j] += X[i,j];
  end;

  for var c := 0 to k - 1 do
    for var j := 0 to p - 1 do
      centers[c,j] /= sizes[c];

  // --- Within (W): сумма квадратов расстояний до центров кластеров
  var W := 0.0;

  for var i := 0 to n - 1 do
  begin
    var c := clsId[i];
    var dist := 0.0;

    for var j := 0 to p - 1 do
    begin
      var d := X[i,j] - centers[c,j];
      dist += d * d;
    end;

    W += dist;
  end;

  // --- Between (B): сумма n_c * ||mu_c - mu||^2
  var B := 0.0;

  for var c := 0 to k - 1 do
  begin
    var dist := 0.0;

    for var j := 0 to p - 1 do
    begin
      var d := centers[c,j] - globalMean[j];
      dist += d * d;
    end;

    B += sizes[c] * dist;
  end;

  if W = 0 then
    exit(double.PositiveInfinity);

  var ch := (B / (k - 1)) / (W / (n - k));

  if double.IsNaN(ch) or double.IsInfinity(ch) then
    ArgumentError(ER_INVALID_VALUE, 'CalinskiHarabaszScore');

  Result := ch;
end;

static function Metrics.DaviesBouldinScore(X: Matrix; labels: Vector): real;
begin
  if X = nil then
    ArgumentNullError(ER_ARG_NULL, 'X');

  if labels = nil then
    ArgumentNullError(ER_ARG_NULL, 'labels');

  var n := X.RowCount;
  var p := X.ColCount;

  if labels.Length <> n then
    DimensionError(ER_DIM_MISMATCH, labels.Length, n);

  // --- label -> 0..k-1
  var map := new Dictionary<integer, integer>();
  var counts := new List<integer>();
  var clsId := new integer[n];

  for var i := 0 to n - 1 do
  begin
    var v := labels[i];
    var lab := integer(v);

    if Abs(v - lab) > 1e-12 then
      ArgumentError(ER_LABELS_MUST_BE_INTEGERS);

    var id: integer;
    if map.TryGetValue(lab, id) then
    begin
      clsId[i] := id;
      counts[id] += 1;
    end
    else
    begin
      id := map.Count;
      map.Add(lab, id);
      clsId[i] := id;
      counts.Add(1);
    end;
  end;

  var k := counts.Count;
  if k < 2 then
    ArgumentError(ER_CH_NEEDS_2_CLUSTERS);

  var sizes := counts.ToArray;

  // --- центры
  var centers := new Matrix(k, p);

  for var i := 0 to n - 1 do
  begin
    var c := clsId[i];
    for var j := 0 to p - 1 do
      centers[c,j] += X[i,j];
  end;

  for var c := 0 to k - 1 do
    for var j := 0 to p - 1 do
      centers[c,j] /= sizes[c];

  // --- Si: среднее расстояние точек до центра
  var S := new real[k];

  for var i := 0 to n - 1 do
  begin
    var c := clsId[i];
    var dist := 0.0;

    for var j := 0 to p - 1 do
    begin
      var d := X[i,j] - centers[c,j];
      dist += d * d;
    end;

    S[c] += Sqrt(dist);
  end;

  for var c := 0 to k - 1 do
    S[c] /= sizes[c];

  // --- считаем DB
  var total := 0.0;

  for var i := 0 to k - 1 do
  begin
    var maxR := 0.0;

    for var j := 0 to k - 1 do
      if i <> j then
      begin
        var dist := 0.0;

        for var d := 0 to p - 1 do
        begin
          var delta := centers[i,d] - centers[j,d];
          dist += delta * delta;
        end;

        dist := Sqrt(dist);

        if dist = 0 then
          continue;

        var Rij := (S[i] + S[j]) / dist;

        if Rij > maxR then
          maxR := Rij;
      end;

    total += maxR;
  end;

  Result := total / k;
end;

/// Adjusted Rand Index (ARI).
/// Внешняя метрика качества кластеризации.
/// Сравнивает предсказанные кластеры с истинными метками.
///
/// Основана на анализе всех пар объектов:
/// проверяет, совпадает ли решение алгоритма и истинная разметка
/// в вопросе "находятся ли две точки в одном кластере".
///
/// Корректирует обычный Rand Index на случайные совпадения.
///
/// Диапазон значений:
/// 1   — идеальное совпадение кластеризации и истинных меток
/// 0   — случайное разбиение
/// <0  — хуже случайного
static function Metrics.AdjustedRandIndex(yTrue, yPred: Vector): real;
begin
  if yTrue.Length <> yPred.Length then
    DimensionError(ER_DIM_MISMATCH, yTrue.Length, yPred.Length);

  var n := yTrue.Length;

  if n < 2 then
    Result := 1
  else
  begin
    var tp := 0;
    var tn := 0;
    var fp := 0;
    var fn := 0;

    for var i := 0 to n-2 do
      for var j := i+1 to n-1 do
      begin
        var sameTrue := yTrue[i] = yTrue[j];
        var samePred := yPred[i] = yPred[j];

        if sameTrue and samePred then
          tp += 1
        else if (not sameTrue) and (not samePred) then
          tn += 1
        else if (not sameTrue) and samePred then
          fp += 1
        else
          fn += 1;
      end;

    var total := tp + tn + fp + fn;

    var ri := (tp + tn) / total;

    var a := tp + fp;
    var b := tp + fn;
    var c := fn + tn;
    var d := fp + tn;

    var expected := ((a*b) + (c*d)) / (total*total);

    if 1 - expected = 0 then
      Result := 0
    else
      Result := (ri - expected) / (1 - expected);
  end;
end;

//-----------------------------
//        ConfusionMatrix
//-----------------------------

constructor ConfusionMatrix.Create(yTrue, yPred: Vector);
begin
  if yTrue = nil then
    ArgumentNullError(ER_ARG_NULL, 'yTrue');

  if yPred = nil then
    ArgumentNullError(ER_ARG_NULL, 'yPred');

  var n := yTrue.Length;

  if n <> yPred.Length then
    DimensionError(ER_DIM_MISMATCH, yTrue.Length, yPred.Length);

  if n = 0 then
    ArgumentError(ER_EMPTY_DATA, 'ConfusionMatrix');

  fTP := 0;
  fFP := 0;
  fTN := 0;
  fFN := 0;

  for var i := 0 to n - 1 do
  begin
    var yt := yTrue[i];
    var yp := yPred[i];

    if double.IsNaN(yt) or double.IsInfinity(yt) then
      ArgumentError(ER_INVALID_VALUE, 'yTrue', i);

    if double.IsNaN(yp) or double.IsInfinity(yp) then
      ArgumentError(ER_INVALID_VALUE, 'yPred', i);

    if (yt <> 0.0) and (yt <> 1.0) then
      ArgumentError(ER_INVALID_CLASS_LABEL, yt);

    if (yp <> 0.0) and (yp <> 1.0) then
      ArgumentError(ER_INVALID_CLASS_LABEL, yp);

    if yt = 1.0 then
      if yp = 1.0 then
        fTP += 1
      else
        fFN += 1
    else
      if yp = 1.0 then
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

function ConfusionMatrix.Specificity: real;
begin
  if fTN + fFP = 0 then
    exit(0.0);

  Result := fTN / (fTN + fFP);
end;

function ConfusionMatrix.BalancedAccuracy: real;
begin
  Result := (Recall + Specificity) / 2;
end;  
  
end.