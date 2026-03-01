unit MLPipelineABC;

interface

uses MLCoreABC;
uses PreprocessorABC;
uses DataFrameABC;
uses LinearAlgebraML;

type
/// DataPipeline — конвейер подготовки данных и обучения модели на DataFrame.
  /// Поддерживает два уровня шагов:
  ///   • DataFrame-уровень: IPreprocessor (Fit/Transform над DataFrame)
  ///   • Matrix-уровень: ITransformer + IModel (после преобразования DataFrame → Matrix/Vector)
  ///
  /// Правила порядка шагов:
  ///   • Сначала идут только DataFrame-шаги (IPreprocessor).
  ///   • Затем — матричные шаги (ITransformer).
  ///   • Модель (IModel) добавляется последней и может быть только одна.
  ///
  /// Если модель не добавлена, DataPipeline работает как чистый DF-конвейер (Fit/Transform/FitTransform)  
  DataPipeline = class
  private
    fDataSteps: List<IPreprocessor>;
    fMatrixSteps: List<ITransformer>;
    fModel: IModel;

    fTarget: string;
    fFeatures: array of string;

    fFitted: boolean;

    procedure ValidateSchema(df: DataFrame);
  public
    /// Создаёт пустой конвейер
    constructor Create;

    /// Добавляет шаг в конец конвейера.
    /// Принимает:
    ///   • IPreprocessor (DataFrame-уровень)
    ///   • ITransformer (Matrix-уровень)
    ///   • IModel (модель, должна быть последней)
    /// Запрещено добавлять шаги после вызова Fit/FitTransform.
    function Add(step: IPipelineStep): DataPipeline;
    
    /// Строит конвейер из последовательности шагов.
    /// target и features используются только в режиме с моделью (когда в шагах присутствует IModel).
    /// Шаги должны удовлетворять порядку: DataFrame* → Matrix* → Model.
    class function Build(target: string; features: array of string;
      params steps: array of IPipelineStep): DataPipeline;

    /// Обучает конвейер на DataFrame.
    /// Семантика:
    ///   • выполняет Fit/Transform для всех DataFrame-шагов;
    ///   • если присутствует модель — преобразует DataFrame в (X, y) по features/target,
    ///     затем обучает матричные трансформеры и модель.
    function Fit(df: DataFrame): DataPipeline;
    
    /// Применяет обученные DataFrame-шаги к DataFrame и возвращает новый DataFrame.
    /// В ML-режиме Transform не выполняет матричные шаги и не вызывает модель.
    function Transform(df: DataFrame): DataFrame;
    
    /// Выполняет Fit и Transform за один проход для DataFrame-режима.
    /// Недоступен, если конвейер содержит модель (IModel).
    function FitTransform(df: DataFrame): DataFrame;
    
    /// Делает предсказание модели для объектов из DataFrame.
    /// Доступен только если конвейер содержит модель (IModel) и был обучен (Fit).
    function Predict(df: DataFrame): Vector;
    
    /// Возвращает матрицу вероятностей (nSamples × nClasses).
    /// Доступен только если конечная модель поддерживает IProbabilisticClassifier.
    function PredictProba(df: DataFrame): Matrix;
    
    /// Возвращает список классов в порядке столбцов PredictProba.
    /// Доступен только если конечная модель поддерживает IProbabilisticClassifier.
    function GetClasses: array of real;
    
    /// Признак того, что был вызван Fit или FitTransform.
    property IsFitted: boolean read fFitted;
  end;
  
implementation

uses MLExceptions;

const
  ER_PIPELINE_MODIFY_AFTER_FIT =
    'Нельзя добавлять шаг после вызова Fit()!!Cannot add step after Fit';
  ER_PIPELINE_STEP_NULL =
    'Шаг конвейера не может быть nil!!Pipeline step cannot be nil';
  ER_DATAPIPE_DF_AFTER_MATRIX =
    'Шаг DataFrame не может идти после матричных шагов!!DataFrame step cannot appear after matrix steps';
  ER_PIPELINE_MULTIPLE_MODELS =
    'В конвейере разрешена только одна модель!!Only one model is allowed in the pipeline';  
  ER_DATAPIPE_MATRIX_AFTER_MODEL =
    'Матричный шаг не может идти после модели!!Matrix step cannot appear after the model';
  ER_DATAPIPE_UNKNOWN_STEP =
    'Неизвестный тип шага конвейера!!Unknown pipeline step type';
  ER_TO_MATRIX_NO_COLUMNS =
    'ToMatrix: не указаны столбцы!!ToMatrix: no columns specified';
  ER_TO_VECTOR_NON_NUMERIC =
    'ToVector: столбец "{0}" содержит нечисловые или NA значения!!' +
    'ToVector: column "{0}" contains non-numeric or NA values';  
  ER_FITTRANSFORM_WITH_MODEL =
    'FitTransform недоступен, если конвейер содержит модель!!FitTransform is not available when the pipeline contains a model';
  ER_PROBA_NOT_SUPPORTED =
    'Модель не поддерживает предсказание вероятностей!!Model does not support probability prediction';
  ER_TARGET_EMPTY =
    'Имя целевой переменной не задано!!Target column name is not specified';
  ER_FEATURES_EMPTY =
    'Список признаков пуст!!Feature list must not be empty';
  ER_DATAPIPE_TARGET_IN_FEATURES =
    'Целевая переменная не должна входить в список признаков!!Target column must not be included in feature list';    
  ER_TO_MATRIX_NON_NUMERIC =
    'Столбец "{0}" содержит нечисловые или NA значения!!Column "{0}" contains non-numeric or NA values';
  ER_DATAPIPE_TARGET_NOT_FOUND =
    'Целевой столбец "{0}" не найден!!Target column "{0}" not found';
  ER_DATAPIPE_FEATURE_NOT_FOUND =
    'Столбец признака "{0}" не найден!!Feature column "{0}" not found';
    
function ToMatrix(Self: DataFrame; colNames: array of string): Matrix; extensionmethod;
begin
  var df := Self;
  var n := df.RowCount;
  var p := colNames.Length;

  if p = 0 then
    ArgumentError(ER_TO_MATRIX_NO_COLUMNS);

  Result := new Matrix(n, p);

  for var j := 0 to p - 1 do
  begin
    var col := df[colNames[j]];

    for var i := 0 to n - 1 do
    begin
      var value: real;

      if not col.TryGetNumericValue(i, value) then
        ArgumentError(ER_TO_MATRIX_NON_NUMERIC, colNames[j]);

      Result[i,j] := value;
    end;
  end;
end;

function ToVector(Self: DataFrame; colName: string): Vector; extensionmethod;
begin
  var df := Self;
  var n := df.RowCount;
  Result := new Vector(n);

  var col := df[colName];

  for var i := 0 to n - 1 do
  begin
    var value: real;

    if not col.TryGetNumericValue(i, value) then
      ArgumentError(ER_TO_VECTOR_NON_NUMERIC, colName);

    Result[i] := value;
  end;
end;

//-----------------------------
//        DataPipeline
//-----------------------------

constructor DataPipeline.Create;
begin
  fDataSteps := new List<IPreprocessor>;
  fMatrixSteps := new List<ITransformer>;
  fModel := nil;
  fTarget := '';
  fFeatures := nil;
  fFitted := false;
end;

function DataPipeline.Add(step: IPipelineStep): DataPipeline;
begin
  if step = nil then
    ArgumentError(ER_PIPELINE_STEP_NULL);

  if fFitted then
    Error(ER_PIPELINE_MODIFY_AFTER_FIT);

  if step is IPreprocessor then // Data Step
  begin
    if (fMatrixSteps.Count > 0) or (fModel <> nil) then
      ArgumentError(ER_DATAPIPE_DF_AFTER_MATRIX);
  
    fDataSteps.Add(step as IPreprocessor);
    exit(Self);
  end;

  if step is ITransformer then // Matrix Step
  begin
    if step is IModel then
    begin
      if fModel <> nil then
        ArgumentError(ER_PIPELINE_MULTIPLE_MODELS);

      fModel := step as IModel;
      exit(Self);
    end;

    if fModel <> nil then
      ArgumentError(ER_DATAPIPE_MATRIX_AFTER_MODEL);

    fMatrixSteps.Add(step as ITransformer);
    exit(Self);
  end;

  ArgumentError(ER_DATAPIPE_UNKNOWN_STEP);
  Result := Self;
end;  

class function DataPipeline.Build(target: string;
  features: array of string; params steps: array of IPipelineStep): DataPipeline;
begin
  var p := new DataPipeline;
  p.fTarget := target;
  p.fFeatures := features;

  for var i := 0 to High(steps) do
    p.Add(steps[i]);

  Result := p;
end;

function DataPipeline.Fit(df: DataFrame): DataPipeline;
begin
  var current := df;

  // 1) DataFrame слой (с сохранением fitted-объектов)
  for var i := 0 to fDataSteps.Count - 1 do
  begin
    fDataSteps[i] := fDataSteps[i].Fit(current);
    current := fDataSteps[i].Transform(current);
  end;

  // DF-only режим
  if fModel = nil then
  begin
    fFitted := true;
    exit(Self);
  end;

  // 2) Проверка схемы
  ValidateSchema(current);

  // 3) Split
  var X := current.ToMatrix(fFeatures);
  var y := current.ToVector(fTarget);

  // 4) Matrix слой (с сохранением fitted-объектов)
  for var i := 0 to fMatrixSteps.Count - 1 do
  begin
    var t := fMatrixSteps[i];
  
    if t is ISupervisedTransformer (var sup) then
      fMatrixSteps[i] := sup.Fit(X, y)
    else
      fMatrixSteps[i] := t.Fit(X);
  
    X := fMatrixSteps[i].Transform(X);
  end;

  // 5) Модель
  fModel := fModel.Fit(X, y);

  fFitted := true;
  Result := Self;
end;

function DataPipeline.Transform(df: DataFrame): DataFrame;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  var current := df;
  foreach var s in fDataSteps do
    current := s.Transform(current);

  Result := current;
end;

function DataPipeline.FitTransform(df: DataFrame): DataFrame;
begin
  if fModel <> nil then
    ArgumentError(ER_FITTRANSFORM_WITH_MODEL);

  var current := df;

  for var i := 0 to fDataSteps.Count - 1 do
  begin
    fDataSteps[i] := fDataSteps[i].Fit(current);
    current := fDataSteps[i].Transform(current);
  end;

  fFitted := true;
  Result := current;
end;

function DataPipeline.Predict(df: DataFrame): Vector;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if fModel = nil then
    ArgumentError(ER_MODEL_NULL);

  var current := Transform(df);
  var X := current.ToMatrix(fFeatures);

  foreach var t in fMatrixSteps do
    X := t.Transform(X);

  Result := fModel.Predict(X);
end;

function DataPipeline.PredictProba(df: DataFrame): Matrix;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if not (fModel is IProbabilisticClassifier) then
    Error(ER_PROBA_NOT_SUPPORTED);

  var current := Transform(df);
  var X := current.ToMatrix(fFeatures);

  foreach var t in fMatrixSteps do
    X := t.Transform(X);

  Result := (fModel as IProbabilisticClassifier).PredictProba(X);
end;

function DataPipeline.GetClasses: array of real;
begin
  if not (fModel is IProbabilisticClassifier) then
    ArgumentError(ER_PROBA_NOT_SUPPORTED);

  Result := (fModel as IProbabilisticClassifier).GetClasses;
end;

procedure DataPipeline.ValidateSchema(df: DataFrame);
begin
  if fTarget = '' then
    ArgumentError(ER_TARGET_EMPTY);

  if (fFeatures = nil) or (Length(fFeatures) = 0) then
    ArgumentError(ER_FEATURES_EMPTY);

  for var i := 0 to High(fFeatures) do
    if fFeatures[i] = fTarget then
      ArgumentError(ER_DATAPIPE_TARGET_IN_FEATURES);

  if not df.Schema.HasColumn(fTarget) then
    ArgumentError(ER_DATAPIPE_TARGET_NOT_FOUND, fTarget);

  for var i := 0 to High(fFeatures) do
    if not df.Schema.HasColumn(fFeatures[i]) then
      ArgumentError(ER_DATAPIPE_FEATURE_NOT_FOUND, fFeatures[i]);
end;



end.