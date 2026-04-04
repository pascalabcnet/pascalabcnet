/// InspectionML — инструменты анализа поведения обученных моделей.
///
/// Модуль предназначен для исследования и интерпретации уже обученных
/// моделей машинного обучения.
///
/// Содержит алгоритмы:
///   • оценки важности признаков
///   • анализа чувствительности модели
///   • построения частичных зависимостей 
///   • диагностических процедур
///
/// Область ответственности:
///   • работает только с обученными моделями (IModel)
///   • не участвует в обучении
///   • не вычисляет метрики напрямую
///   • не изменяет состояние модели
///
/// Архитектурный принцип:
///   • модуль зависит от абстракции IModel
///   • модели не зависят от данного модуля
unit InspectionML;

interface

uses MLCoreABC, LinearAlgebraML;

type
  Inspection = static class
  public
  /// PermutationImportance — оценка важности признаков методом перестановок.
  /// 
  /// Для каждого признака случайно перемешивает соответствующий столбец
  /// и измеряет снижение качества модели по заданной функции scoreFunc.
  /// 
  /// Требует модель, поддерживающую предсказания (IPredictiveModel).
  /// 
  /// Параметры:
  ///   • model — обученная модель, реализующая IPredictiveModel;
  ///   • X — матрица признаков (nSamples × nFeatures);
  ///   • y — вектор истинных значений;
  ///   • scoreFunc — функция оценки качества (например, MSE, Accuracy);
  ///   • seed — начальное значение генератора случайных чисел.
  /// 
  /// Возвращает:
  ///   Вектор важностей признаков длины nFeatures.
  ///   Чем больше значение, тем сильнее признак влияет на качество модели
    static function PermutationImportance(model: IPredictiveModel; X: Matrix; y: Vector;
      scoreFunc: (Vector, Vector) -> real; nRepeats: integer := 5; seed: integer := 0): Vector;
  end;  

implementation

uses MLExceptions;

const
  ER_SCORE_FUNC_NULL = 
    'scoreFunc не может быть nil!!scoreFunc cannot be nil';
  ER_ARG_OUT_OF_RANGE =
    'Аргумент {0} имеет недопустимое значение {1}!!Argument {0} has invalid value {1}';


static function Inspection.PermutationImportance(
  model: IPredictiveModel; 
  X: Matrix; 
  y: Vector;
  scoreFunc: (Vector, Vector) -> real; 
  nRepeats: integer;
  seed: integer  
): Vector;
begin
  if model = nil then
    ArgumentNullError(ER_MODEL_NULL);

  if scoreFunc = nil then
    ArgumentNullError(ER_SCORE_FUNC_NULL);

  if X.RowCount <> y.Length then
    DimensionError(ER_DIM_MISMATCH, X.RowCount, y.Length);

  if nRepeats < 1 then
    ArgumentOutOfRangeError(ER_ARG_OUT_OF_RANGE, 'nRepeats', nRepeats);

  var baselinePred := model.Predict(X);
  var baselineScore := scoreFunc(y, baselinePred);

  var n := X.RowCount;
  var p := X.ColCount;

  var resultVec := new Vector(p);

  for var j := 0 to p - 1 do
  begin
    var acc := 0.0;

    for var r := 0 to nRepeats - 1 do
    begin
      var Xperm := X.Clone;

      // --- seed для каждого (признак, повтор)
      var runSeed :=
        if seed >= 0 then seed + j * 100000 + r
        else System.Environment.TickCount and integer.MaxValue;

      var rnd := new System.Random(runSeed);

      // --- shuffle столбца j
      for var i := n - 1 downto 1 do
      begin
        var k := rnd.Next(i + 1);
        var tmp := Xperm[i,j];
        Xperm[i,j] := Xperm[k,j];
        Xperm[k,j] := tmp;
      end;

      var permPred := model.Predict(Xperm);
      var permScore := scoreFunc(y, permPred);

      acc += (baselineScore - permScore);
    end;

    resultVec[j] := acc / nRepeats;
  end;

  Result := resultVec;
end;

end.