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
    /// Для каждого признака случайно перемешивает его столбец и измеряет
    /// падение выбранной метрики качества модели.
    /// Работает с любой реализацией IModel.
    static function PermutationImportance(model: IModel; X: Matrix; y: Vector;
      scoreFunc: (Vector, Vector) -> real; seed: integer := 0): Vector;
  end;  

implementation

uses MLExceptions;

const
  ER_SCORE_FUNC_NULL = 'scoreFunc не может быть nil!!scoreFunc cannot be nil';

static function Inspection.PermutationImportance(model: IModel; X: Matrix; y: Vector;
  scoreFunc: (Vector, Vector) -> real; seed: integer): Vector;
begin
  if model = nil then
    ArgumentNullError(ER_MODEL_NULL);

  if scoreFunc = nil then
    ArgumentNullError(ER_SCORE_FUNC_NULL);

  if X.Rows <> y.Length then
    DimensionError(ER_DIM_MISMATCH, X.Rows, y.Length);

  var baselinePred := model.Predict(X);
  var baselineScore := scoreFunc(y, baselinePred);

  var n := X.Rows;
  var p := X.Cols;

  var resultVec := new Vector(p);
  var rng := new System.Random(seed);

  for var j := 0 to p-1 do
  begin
    var Xperm := X.Clone;

    // Fisher–Yates shuffle столбца j (детерминированно через seed)
    for var i := n-1 downto 1 do
    begin
      var k := rng.Next(i+1);
      var tmp := Xperm[i,j];
      Xperm[i,j] := Xperm[k,j];
      Xperm[k,j] := tmp;
    end;

    var permPred := model.Predict(Xperm);
    var permScore := scoreFunc(y, permPred);

    resultVec[j] := baselineScore - permScore;
  end;

  Result := resultVec;
end;

end.