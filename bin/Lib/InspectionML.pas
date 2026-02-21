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
      scoreFunc: (Vector, Vector) -> real): Vector;
  end;  

implementation

static function Inspection.PermutationImportance(model: IModel; X: Matrix; y: Vector;
  scoreFunc: (Vector, Vector) -> real): Vector;
begin
  if X.Rows <> y.Length then
    raise new Exception('PermutationImportance: dimension mismatch');

  var baselinePred := model.Predict(X);
  var baselineScore := scoreFunc(y, baselinePred);

  var n := X.Rows;
  var p := X.Cols;

  var resultVec := new Vector(p);

  for var j := 0 to p-1 do
  begin
    var Xperm := X.Clone;

    // Fisher–Yates shuffle столбца j
    for var i := n-1 downto 1 do
    begin
      var k := Random(i+1);
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