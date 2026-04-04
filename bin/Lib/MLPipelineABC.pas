unit MLPipelineABC;

interface

uses MLCoreABC;
uses PreprocessorABC;
uses DataFrameABC;
uses LinearAlgebraML;

type 
  TaskKind = (tkRegression, tkClassification);

type
  /// DataPipeline — конвейер подготовки данных и обучения модели с учителем на DataFrame.
  /// 
  /// Поддерживает два уровня шагов:
  ///   • DataFrame-уровень: IPreprocessor (Fit/Transform над DataFrame)
  ///   • Matrix-уровень: ITransformer / ISupervisedTransformer и IModel
  ///     (после преобразования DataFrame → Matrix/Vector)
  ///
  /// Правила порядка шагов:
  ///   • Сначала идут только DataFrame-шаги (IPreprocessor).
  ///   • Затем — матричные шаги (ITransformer / ISupervisedTransformer).
  ///   • Модель (IModel) добавляется последней и может быть только одна.
  ///
  /// В режиме с моделью используются:
  ///   • features — признаки;
  ///   • target — целевая переменная.
  ///
  /// Если модель не добавлена, DataPipeline работает как чистый DF-конвейер (Fit/Transform/FitTransform)  
  DataPipeline = class
  private
    fDataSteps: List<IPreprocessor>;
    fMatrixSteps: List<ITransformer>;
    fModel: IModel;
    fTask: TaskKind;

    fTarget: string;
    fFeatures: array of string;
    fFinalFeatures: array of string;

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
    
    { // Примеры использования препроцессоров и моделей в DataPipeline.
    // Препроцессоры работают на уровне DataFrame.
    // После них Pipeline автоматически преобразует данные в Matrix/Vector.
    // Далее выполняются матричные трансформеры и модель.
    
    // ------------------------------------------------------------
    // Пример 1. Классификация с категориальным целевым признаком
    var pipe :=
      DataPipeline.Build(
        'species',
        ['length','width'],
        new LabelEncoder('species'),   // DataFrame-препроцессор (строки → числа)
        new StandardScaler,            // матричный трансформер
        new LogisticRegression         // модель
      );
    
    // ------------------------------------------------------------
    // Пример 2. Регрессия с пропущенными значениями и категориальным признаком
    var pipe :=
      DataPipeline.Build(
        'price',
        ['area','floor','district'],
        new Imputer('area'),           // DataFrame-препроцессор (заполнение NA)
        new OneHotEncoder('district'), // DataFrame-препроцессор
        new RandomForestRegressor      // модель
      );
    
    // ------------------------------------------------------------
    // Пример 3. Сложный pipeline
    var pipe :=
      DataPipeline.Build(
        'target',
        ['f1','f2','f3','category'],
        new Imputer('f2'),             // DataFrame-препроцессор
        new OneHotEncoder('category'), // DataFrame-препроцессор
        new StandardScaler,            // матричный трансформер
        new PCATransformer(2),         // матричный трансформер
        new GradientBoostingRegressor  // модель
      );
      
    // ------------------------------------------------------------
    // Пример 4. Классификация без Pipeline
    var df := Datasets.Flowers;

    // --- Encode target (DataFrame уровень)
    df := df.SetCategorical(['species']);
    
    var labels := df.EncodeLabels('species');
    
    // --- X, y
    var X := df.ToMatrix(['length','width']);
    var y := new Vector(labels);
    
    // --- Matrix уровень
    var scaler := new StandardScaler;
    X := scaler.FitTransform(X);
    
    // --- Модель
    var model := new LogisticRegression;
    model.Fit(X, y);
    
    // Pipeline.Build используется, когда данные уже представлены
    // в виде числовой матрицы признаков X и вектора целевой переменной y.
    // В этом случае DataFrame и препроцессоры уровня таблицы не требуются.
    //
    // Типичные ситуации:
    //  • экспериментирование с ML-алгоритмами
    //  • сравнение моделей
    //  • кросс-валидация
    //  • подбор гиперпараметров
    //  • тестирование моделей
    
    // Пример 5. Pipeline на матричном уровне
    var pipe :=
      Pipeline.Build(
        new StandardScaler,
        new PCATransformer(2),
        new LogisticRegression
      );
    
      }
    
    /// Строит конвейер из шагов обработки данных и модели.
    /// 
    /// Используется в задачах с учителем (с target). 
    /// task задает тип модели - регрессия или классификация
    /// Шаги выполняются последовательно:
    /// DataFrame-преобразования → (при необходимости) матричные шаги → модель.
    static function Build(
      task: TaskKind;
      target: string; 
      features: array of string;
      params steps: array of IPipelineStep
    ): DataPipeline;
      
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
    
    /// Возвращает метки классов в порядке кодирования (0,1,2,...),
    /// используемом при EncodeLabels.
    /// Доступен только для задач классификации после Fit.
    function GetClassLabels: array of string;
    
    /// Признак того, что был вызван Fit или FitTransform.
    property IsFitted: boolean read fFitted;
    
    function ToString: string; override;
  end;
  
  /// UDataPipeline — конвейер подготовки данных и обучения модели без учителя на DataFrame.
  /// 
  /// Поддерживает два уровня шагов:
  ///   • DataFrame-уровень: IPreprocessor (Fit/Transform над DataFrame)
  ///   • Matrix-уровень: IUnsupervisedTransformer и IUnsupervisedModel
  ///     (после преобразования DataFrame → Matrix)
  ///
  /// Правила порядка шагов:
  ///   • Сначала идут только DataFrame-шаги (IPreprocessor).
  ///   • Затем — матричные шаги (IUnsupervisedTransformer).
  ///   • Модель (IUnsupervisedModel) добавляется последней и может быть только одна.
  ///
  /// В конвейере используются:
  ///   • features — признаки (целевая переменная отсутствует).
  ///
  /// Если модель не добавлена, UDataPipeline работает как чистый DF-конвейер
  /// (Fit/Transform/FitTransform)
  UDataPipeline = class
  private
    fDataSteps: List<IPreprocessor>;
    fMatrixSteps: List<ITransformer>;
    fModel: IModel;
  
    fFeatures: array of string;
    fFinalFeatures: array of string;
  
    fFitted: boolean;
  
    procedure ValidateSchema(df: DataFrame);
    procedure ValidateNumericFeatures(df: DataFrame);
    function TransformToMatrix(df: DataFrame): Matrix;
  public
    /// Создаёт пустой конвейер
    constructor Create;
  
    /// Добавляет шаг в конец конвейера.
    /// Принимает:
    ///   • IPreprocessor (DataFrame-уровень)
    ///   • ITransformer (Matrix-уровень)
    ///   • IModel (модель, должна быть последней)
    function Add(step: IPipelineStep): UDataPipeline;
  
    /// Строит unsupervised-конвейер из шагов обработки данных и модели.
    static function Build(features: array of string;
      params steps: array of IPipelineStep): UDataPipeline;
  
    /// Обучает конвейер на DataFrame.
    function Fit(df: DataFrame): UDataPipeline;
  
    /// Применяет обученные DataFrame-шаги к DataFrame и возвращает новый DataFrame.
    function Transform(df: DataFrame): DataFrame;
  
    /// Выполняет Fit и Transform за один проход для DataFrame-режима.
    /// Недоступен, если конвейер содержит модель (IModel).
    function FitTransform(df: DataFrame): DataFrame;
    
    /// Обучает конвейер и сразу возвращает метки кластеров.
    /// 
    /// Эквивалентно последовательному вызову:
    ///   Fit(df) → PredictLabels(df)
    /// 
    /// Используется для задач кластеризации, где нет разделения на train/test.
    /// 
    /// Доступен только если:
    ///   • конвейер содержит модель кластеризации (IClusterer).
    function FitPredict(df: DataFrame): array of integer;
  
    /// Делает предсказание модели для объектов из DataFrame.
    function Predict(df: DataFrame): Vector;
    
    /// Возвращает метки кластеров для объектов из DataFrame.
    /// 
    /// Выполняет последовательные преобразования данных (DataFrame и матричные шаги),
    /// затем применяет обученную модель кластеризации.
    /// 
    /// Доступен только если:
    ///   • конвейер содержит модель (IModel);
    ///   • модель поддерживает кластеризацию (IClusterer);
    ///   • конвейер был обучен (Fit).
    function PredictLabels(df: DataFrame): array of integer;
  
    /// Признак того, что был вызван Fit или FitTransform.
    property IsFitted: boolean read fFitted;
  
    function ToString: string; override;
  end;  
  
implementation

uses MLExceptions;
uses DataAdapters;

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
  ER_DATAPIPE_UNKNOWN_STEP_TYPE =
    'Неизвестный тип шага конвейера: {0}!!Unknown pipeline step type: {0}';
  
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
  ER_FEATURE_EMPTY =
    'Имя признака не может быть пустым!!Feature name cannot be empty';
  ER_DATAPIPE_TARGET_IN_FEATURES =
    'Целевая переменная "{0}" не должна входить в список признаков!!Target variable "{0}" must not appear in feature list';  
  ER_TO_MATRIX_NON_NUMERIC =
    'Столбец "{0}" содержит нечисловые или NA значения!!Column "{0}" contains non-numeric or NA values';
  ER_DATAPIPE_TARGET_NOT_FOUND =
    'Целевой столбец "{0}" не найден. Доступные столбцы: {1}!!' +
    'Target column "{0}" not found. Available columns: {1}';
  ER_DATAPIPE_FEATURE_NOT_FOUND =
    'Признак "{0}" не найден в DataFrame. Доступные столбцы: {1}!!'+
    'Feature "{0}" not found in DataFrame. Available columns: {1}';
  ER_DATAPIPE_DUPLICATE_FEATURE =
    'Повторяющийся признак: {0}!!Duplicate feature: {0}'; 
  ER_PIPELINE_FINALFEATURES =
    'Внутренняя ошибка pipeline: итоговый набор признаков не определён. Возможно, Fit не был выполнен корректно!!Pipeline internal error: final feature set is not defined. Fit may not have been executed correctly';    
  ER_PIPELINE_TARGET_REMOVED =
    'Целевая переменная "{0}" была удалена на этапе preprocessing pipeline!!Target column "{0}" was removed during pipeline preprocessing';  
  ER_PIPELINE_NO_FEATURES =
    'После preprocessing pipeline не осталось признаков для обучения модели!!No features remain after pipeline preprocessing';    
  ER_MATRIXSTEP_NO_FIT =
    'Шаг матричного конвейера #{0} не поддерживает Fit!!Matrix step #{0} does not support Fit'; 
  ER_Model_NoFit =
    'Модель (тип: {0}) не поддерживает Fit!!Model (type: {0}) does not support Fit';    
  ER_MODEL_NOT_SUPERVISED =
    'Модель (тип: {0}) не поддерживает Fit(X, y)!!' +
    'Model (type: {0}) does not support Fit(X, y)';
  ER_XY_SIZE_MISMATCH =
    'Несовпадение размеров X и y: X имеет {0} строк, y имеет {1} элементов!!' +
    'X and y size mismatch: X has {0} rows, y has {1} elements';
  ER_PIPELINE_FEATURE_NOT_FOUND =
    'Признак "{0}" отсутствует во входных данных!!' +
    'Feature "{0}" not found in input data';
  ER_MODEL_NOT_UNSUPERVISED =
    'Модель (тип: {0}) не является моделью без учителя!!' +
    'Model (type: {0}) is not an unsupervised model';
  ER_PIPELINE_FEATURE_NOT_NUMERIC =
    'Признак "{0}" имеет тип {1} и должен быть числовым!!Feature "{0}" has type {1} but must be numeric';    
  ER_MODEL_NOT_CLUSTERER =
    'Модель "{0}" не является алгоритмом кластеризации!!Model "{0}" is not a clustering algorithm';    
  ER_NOT_CLASSIFICATION = 
    'Операция доступна только для задач классификации!!Operation is only available for classification tasks';
  ER_CLASSES_NOT_AVAILABLE = 
    'Метки классов недоступны. Убедитесь, что конвейер обучен и задача — классификация!!Class labels are not available. Ensure the pipeline is fitted and the task is classification';  
  ER_LABELENCODER_TARGET_NOT_ALLOWED =
    'LabelEncoder нельзя применять к целевому столбцу. Используйте EncodeLabels!!LabelEncoder cannot be applied to the target column. Use EncodeLabels instead';
  ER_ENCODELABELS_NOT_CATEGORICAL =
    'Целевой столбец должен быть категориальным для задач классификации!!Target column must be categorical for classification tasks';
  ER_PIPELINE_TARGET_TRANSFORM_NOT_ALLOWED =
    'Преобразование целевой переменной "{0}" запрещено в DataPipeline!!' +
    'Transformation of target variable "{0}" is not allowed in DataPipeline';    
    
    
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
  // --- global invariants
  if step = nil then
    ArgumentError(ER_PIPELINE_STEP_NULL);

  if fFitted then
    Error(ER_PIPELINE_MODIFY_AFTER_FIT);

  // --- target protection
  // Любой шаг DataFrame, привязанный к одной или нескольким колонкам,
  // не должен затрагивать целевую переменную (target).
  // Проверка выполняется через интерфейсы IColumnBoundStep / IColumnsBoundStep
  // без привязки к конкретным классам.  
  if step is IColumnBoundStep(var cstep) then
    if cstep.ColumnName = fTarget then
      ArgumentError(ER_PIPELINE_TARGET_TRANSFORM_NOT_ALLOWED, fTarget);
  
  if step is IColumnsBoundStep(var mstep) then
    if fTarget in mstep.Columns then
      ArgumentError(ER_PIPELINE_TARGET_TRANSFORM_NOT_ALLOWED, fTarget);

  // --- DataFrame step
  if step is IPreprocessor then
  begin
    if (fMatrixSteps.Count > 0) or (fModel <> nil) then
      ArgumentError(ER_DATAPIPE_DF_AFTER_MATRIX);

    fDataSteps.Add(step as IPreprocessor);
    exit(Self);
  end;
  
  // --- Matrix transformer
  if step is ITransformer then
  begin
    if fModel <> nil then
      ArgumentError(ER_DATAPIPE_MATRIX_AFTER_MODEL);

    fMatrixSteps.Add(step as ITransformer);
    exit(Self);
  end;

  // --- Model (обязательно последний шаг)
  if step is IModel then
  begin
    if fModel <> nil then
      ArgumentError(ER_PIPELINE_MULTIPLE_MODELS);

    fModel := step as IModel;
    exit(Self);
  end;

  ArgumentError(ER_DATAPIPE_UNKNOWN_STEP_TYPE, step.ToString);
  Result := Self;
end;

class function DataPipeline.Build(
  task: TaskKind;
  target: string;
  features: array of string; 
  params steps: array of IPipelineStep
): DataPipeline;
begin
  if (target = nil) or (target = '') then
    ArgumentError(ER_TARGET_EMPTY);

  if (features = nil) or (features.Length = 0) then
    ArgumentError(ER_FEATURES_EMPTY);
  
  var seen := new HashSet<string>;

  foreach var f in features do
  begin
    if (f = nil) or (f = '') then
      ArgumentError(ER_FEATURE_EMPTY);

    if f = target then
      ArgumentError(ER_DATAPIPE_TARGET_IN_FEATURES, target);

    if seen.Contains(f) then
      ArgumentError(ER_DATAPIPE_DUPLICATE_FEATURE, f);

    seen.Add(f);
  end;

  var p := new DataPipeline;
  p.fTarget := target;
  p.fFeatures := Copy(features);
  p.fTask := task;

  if steps <> nil then
    for var i := 0 to High(steps) do
      p.Add(steps[i]);

  Result := p;
end;

function DataPipeline.Fit(df: DataFrame): DataPipeline;
begin
  if df = nil then
    ArgumentNullError(ER_ARG_NULL, 'df');
  if fModel = nil then
    ArgumentError(ER_MODEL_NULL);

  var current := df;

  // --- 0) Проверка входной схемы
  ValidateSchema(current);

  // --- 1) DataFrame шаги
  for var i := 0 to fDataSteps.Count - 1 do
  begin
    fDataSteps[i] := fDataSteps[i].Fit(current);
    current := fDataSteps[i].Transform(current);
  end;

  // --- target должен остаться
  if not current.HasColumn(fTarget) then
    ArgumentError(ER_PIPELINE_TARGET_REMOVED, fTarget);

  // --- 2) вычислить финальные признаки
  var feats := new List<string>;

  foreach var f in fFeatures do
  begin
    if current.HasColumn(f) then
    begin
      feats.Add(f);
      continue;
    end;

    // искать производные признаки (OneHotEncoder)
    foreach var c in current.Schema.ColumnNames do
      if c.StartsWith(f + '_') then
        if not feats.Contains(c) then
          feats.Add(c);
  end;

  if feats.Count = 0 then
    ArgumentError(ER_PIPELINE_NO_FEATURES);

  fFinalFeatures := feats.ToArray;

  var X := current.ToMatrix(fFinalFeatures);
  
  var classes: array of string;  
  var y: Vector;
  
  case fTask of
    tkRegression:
      y := current.ToVector(fTarget);
  
    tkClassification:
      begin
        var labels := current.EncodeLabels(fTarget, classes);
        y := new Vector(labels);
      end;
  end;
  
  if X.RowCount <> y.Length then
    DimensionError(ER_XY_SIZE_MISMATCH, X.RowCount, y.Length);

  // --- 3) Matrix transformers
  for var i := 0 to fMatrixSteps.Count - 1 do
  begin
    var t := fMatrixSteps[i];
  
    if t is ISupervisedTransformer(var sup) then
      fMatrixSteps[i] := sup.Fit(X, y)
    else if t is IUnsupervisedTransformer(var unsup) then
      fMatrixSteps[i] := unsup.Fit(X)
    else ArgumentError(ER_MATRIXSTEP_NO_FIT, i);
  
    X := fMatrixSteps[i].Transform(X);
  end;
  
  // --- 4) модель
  if fModel is ISupervisedModel(var supModel) then
    fModel := supModel.Fit(X, y)
  else ArgumentError(ER_MODEL_NOT_SUPERVISED, fModel.GetType.Name);
  
  if fTask = tkClassification then
    if fModel is IClassifier(var cls) then
      cls.SetClassLabels(classes);  

  fFitted := true;
  Result := Self;
end;

function DataPipeline.Transform(df: DataFrame): DataFrame;
begin
  if df = nil then
    ArgumentNullError(ER_ARG_NULL, 'df');
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  var current := df;
  foreach var s in fDataSteps do
    current := s.Transform(current);

  Result := current;
end;

function DataPipeline.FitTransform(df: DataFrame): DataFrame;
begin
  if df = nil then
    ArgumentNullError(ER_ARG_NULL, 'df');
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
  if df = nil then
    ArgumentNullError(ER_ARG_NULL, 'df');
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if fModel = nil then
    ArgumentError(ER_MODEL_NULL);

  var current := Transform(df);
  
  if fFinalFeatures = nil then
    Error(ER_PIPELINE_FINALFEATURES);
  
  for var i := 0 to High(fFinalFeatures) do
  if not current.HasColumn(fFinalFeatures[i]) then
    ArgumentError(ER_PIPELINE_FEATURE_NOT_FOUND, fFinalFeatures[i]);

  var X := current.ToMatrix(fFinalFeatures);

  foreach var t in fMatrixSteps do
    X := t.Transform(X);

  Result := fModel.Predict(X);
end;

function DataPipeline.PredictProba(df: DataFrame): Matrix;
begin
  if df = nil then
    ArgumentNullError(ER_ARG_NULL, 'df');
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if not (fModel is IProbabilisticClassifier) then
    ArgumentError(ER_PROBA_NOT_SUPPORTED);

  var current := Transform(df);
  
  if fFinalFeatures = nil then
    Error(ER_PIPELINE_FINALFEATURES);
  
  if fTask <> tkClassification then
    ArgumentError(ER_NOT_CLASSIFICATION);
  
  for var i := 0 to High(fFinalFeatures) do
    if not current.HasColumn(fFinalFeatures[i]) then
      ArgumentError(ER_PIPELINE_FEATURE_NOT_FOUND, fFinalFeatures[i]);
  
  var X := current.ToMatrix(fFinalFeatures);

  foreach var t in fMatrixSteps do
    X := t.Transform(X);

  Result := (fModel as IProbabilisticClassifier).PredictProba(X);
end;

function DataPipeline.GetClassLabels: array of string;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if fTask <> tkClassification then
    ArgumentError(ER_NOT_CLASSIFICATION);

  var cls := fModel as IClassifier;
  if cls = nil then
    ArgumentError(ER_CLASSES_NOT_AVAILABLE);

  Result := cls.GetClassLabels;
end;

procedure DataPipeline.ValidateSchema(df: DataFrame);
begin
  if fTarget = '' then
    ArgumentError(ER_TARGET_EMPTY);

  if (fFeatures = nil) or (Length(fFeatures) = 0) then
    ArgumentError(ER_FEATURES_EMPTY);

  // target не должен входить в features
  for var i := 0 to High(fFeatures) do
    if fFeatures[i] = fTarget then
      ArgumentError(ER_DATAPIPE_TARGET_IN_FEATURES, fTarget);

  // проверка существования target
  if not df.Schema.HasColumn(fTarget) then
  begin
    var cols := df.Schema.ColumnNames.JoinToString(', ');
    ArgumentError(ER_DATAPIPE_TARGET_NOT_FOUND, fTarget, cols);
  end;
  
  if fTask = tkClassification then
    if not df.IsCategorical(fTarget) then
      ArgumentError(ER_ENCODELABELS_NOT_CATEGORICAL, fTarget);
  
  var seen := new HashSet<string>;

  for var i := 0 to High(fFeatures) do
  begin
    var f := fFeatures[i];

    if f = '' then
      ArgumentError(ER_FEATURE_EMPTY);

    if not df.Schema.HasColumn(f) then
    begin
      var cols := df.Schema.ColumnNames.JoinToString(', ');
      ArgumentError(ER_DATAPIPE_FEATURE_NOT_FOUND, f, cols);
    end;

    if seen.Contains(f) then
      ArgumentError(ER_DATAPIPE_DUPLICATE_FEATURE, f);

    seen.Add(f);
  end;
end;

function DataPipeline.ToString: string;
begin
  var sb := 'DataPipeline (' +
            (if fFitted then 'trained' else 'not trained') + '):' + NewLine;

  sb += '  Target: ' + fTarget + NewLine;
  sb += '  Features: ' + fFeatures.JoinToString(', ') + NewLine;

  var idx := 1;

  foreach var s in fDataSteps do
  begin
    sb += '  [' + idx + '] ' + s.ToString + NewLine;
    idx += 1;
  end;

  foreach var t in fMatrixSteps do
  begin
    sb += '  [' + idx + '] ' + t.ToString + NewLine;
    idx += 1;
  end;

  if fModel <> nil then
    sb += '  [' + idx + '] ' + fModel.ToString;

  Result := sb;
end;

//-----------------------------
//        UDataPipeline 
//-----------------------------

constructor UDataPipeline.Create;
begin
  fDataSteps := new List<IPreprocessor>;
  fMatrixSteps := new List<ITransformer>;
  fModel := nil;
  fFeatures := nil;
  fFitted := false;
end;

function UDataPipeline.Add(step: IPipelineStep): UDataPipeline;
begin
  if step = nil then
    ArgumentError(ER_PIPELINE_STEP_NULL);

  if fFitted then
    Error(ER_PIPELINE_MODIFY_AFTER_FIT);

  // --- DataFrame step
  if step is IPreprocessor then
  begin
    if (fMatrixSteps.Count > 0) or (fModel <> nil) then
      ArgumentError(ER_DATAPIPE_DF_AFTER_MATRIX);

    fDataSteps.Add(step as IPreprocessor);
    exit(Self);
  end;

  // --- Matrix transformer
  if step is ITransformer then
  begin
    if fModel <> nil then
      ArgumentError(ER_DATAPIPE_MATRIX_AFTER_MODEL);

    fMatrixSteps.Add(step as ITransformer);
    exit(Self);
  end;

  // --- Model (обязательно последний шаг)
  if step is IModel then
  begin
    if fModel <> nil then
      ArgumentError(ER_PIPELINE_MULTIPLE_MODELS);

    fModel := step as IModel;
    exit(Self);
  end;

  ArgumentError(ER_DATAPIPE_UNKNOWN_STEP_TYPE, step.ToString);
  Result := Self;
end;

class function UDataPipeline.Build(features: array of string;
  params steps: array of IPipelineStep): UDataPipeline;
begin
  if (features = nil) or (features.Length = 0) then
    ArgumentError(ER_FEATURES_EMPTY);

  var seen := new HashSet<string>;

  foreach var f in features do
  begin
    if (f = nil) or (f = '') then
      ArgumentError(ER_FEATURE_EMPTY);

    if seen.Contains(f) then
      ArgumentError(ER_DATAPIPE_DUPLICATE_FEATURE, f);

    seen.Add(f);
  end;

  var p := new UDataPipeline;
  p.fFeatures := features;

  for var i := 0 to High(steps) do
    p.Add(steps[i]);

  Result := p;
end;

function UDataPipeline.Fit(df: DataFrame): UDataPipeline;
begin
  if df = nil then
    ArgumentNullError(ER_ARG_NULL, 'df');
  if fModel = nil then
    ArgumentError(ER_MODEL_NULL);

  var current := df;

  // --- 0) Проверка входной схемы
  ValidateSchema(current);

  // --- 1) DataFrame шаги
  for var i := 0 to fDataSteps.Count - 1 do
  begin
    fDataSteps[i] := fDataSteps[i].Fit(current);
    current := fDataSteps[i].Transform(current);
  end;

  // --- 2) вычислить финальные признаки
  var feats := new List<string>;

  foreach var f in fFeatures do
  begin
    if current.HasColumn(f) then
    begin
      feats.Add(f);
      continue;
    end;

    // искать производные признаки (OneHotEncoder)
    foreach var c in current.Schema.ColumnNames do
      if c.StartsWith(f + '_') then
        if not feats.Contains(c) then
          feats.Add(c);
  end;

  if feats.Count = 0 then
    ArgumentError(ER_PIPELINE_NO_FEATURES);

  fFinalFeatures := feats.ToArray;

  var X := current.ToMatrix(fFinalFeatures);

  // --- 3) Matrix transformers
  for var i := 0 to fMatrixSteps.Count - 1 do
  begin
    var t := fMatrixSteps[i];

    if t is IUnsupervisedTransformer(var unsup) then
      fMatrixSteps[i] := unsup.Fit(X)
    else
      ArgumentError(ER_MATRIXSTEP_NO_FIT, i, t.GetType.Name);

    X := fMatrixSteps[i].Transform(X);
  end;

  // --- 4) модель
  if fModel is IUnsupervisedModel(var unsupModel) then
    fModel := unsupModel.Fit(X)
  else
    ArgumentError(ER_MODEL_NOT_UNSUPERVISED, fModel.GetType.Name);

  fFitted := true;
  Result := Self;
end;

function UDataPipeline.Transform(df: DataFrame): DataFrame;
begin
  if df = nil then
    ArgumentNullError(ER_ARG_NULL, 'df');
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  var current := df;
  foreach var s in fDataSteps do
    current := s.Transform(current);

  Result := current;
end;

function UDataPipeline.FitTransform(df: DataFrame): DataFrame;
begin
  if df = nil then
    ArgumentNullError(ER_ARG_NULL, 'df');
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

function UDataPipeline.FitPredict(df: DataFrame): array of integer;
begin
  if df = nil then
    ArgumentNullError(ER_ARG_NULL, 'df');

  if fModel = nil then
    ArgumentError(ER_MODEL_NULL);

  if not (fModel is IClusterer) then
    ArgumentError(ER_MODEL_NOT_CLUSTERER, fModel.GetType.Name);

  Fit(df);
  Result := PredictLabels(df);
end;

procedure UDataPipeline.ValidateNumericFeatures(df: DataFrame);
begin
  if fFinalFeatures = nil then
    Error(ER_PIPELINE_FINALFEATURES);

  var schema := df.Schema;

  foreach var f in fFinalFeatures do
  begin
    var idx := schema.IndexOf(f);

    if idx < 0 then
      ArgumentError(ER_PIPELINE_FEATURE_NOT_FOUND, f);

    var t := schema.ColumnTypeAt(idx);

    if (t <> ColumnType.ctInt) and (t <> ColumnType.ctFloat) then
      ArgumentError(ER_PIPELINE_FEATURE_NOT_NUMERIC, f, t.ToString);
  end;
end;

function UDataPipeline.TransformToMatrix(df: DataFrame): Matrix;
begin
  if df = nil then
    ArgumentNullError(ER_ARG_NULL, 'df');
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  var current := Transform(df);

  ValidateNumericFeatures(current);

  var X := current.ToMatrix(fFinalFeatures);

  foreach var t in fMatrixSteps do
    X := t.Transform(X);

  Result := X;
end;

function UDataPipeline.Predict(df: DataFrame): Vector;
begin
  if fModel = nil then
    ArgumentError(ER_MODEL_NULL);

  var X := TransformToMatrix(df);

  Result := fModel.Predict(X);
end;

function UDataPipeline.PredictLabels(df: DataFrame): array of integer;
begin
  if fModel = nil then
    ArgumentError(ER_MODEL_NULL);

  if not (fModel is IClusterer) then
    ArgumentError(ER_MODEL_NOT_CLUSTERER, fModel.GetType.Name);

  var cl := fModel as IClusterer;

  var X := TransformToMatrix(df);

  Result := cl.PredictLabels(X);
end;

{function UDataPipeline.Predict(df: DataFrame): Vector;
begin
  if df = nil then
    ArgumentNullError(ER_ARG_NULL, 'df');
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if fModel = nil then
    ArgumentError(ER_MODEL_NULL);

  var current := Transform(df);

  if fFinalFeatures = nil then
    Error(ER_PIPELINE_FINALFEATURES);

  for var i := 0 to High(fFinalFeatures) do
    if not current.HasColumn(fFinalFeatures[i]) then
      ArgumentError(ER_PIPELINE_FEATURE_NOT_FOUND, fFinalFeatures[i]);

  var X := current.ToMatrix(fFinalFeatures);

  foreach var t in fMatrixSteps do
    X := t.Transform(X);

  Result := fModel.Predict(X);
end;}

procedure UDataPipeline.ValidateSchema(df: DataFrame);
begin
  if (fFeatures = nil) or (Length(fFeatures) = 0) then
    ArgumentError(ER_FEATURES_EMPTY);

  var seen := new HashSet<string>;

  for var i := 0 to High(fFeatures) do
  begin
    var f := fFeatures[i];

    if f = '' then
      ArgumentError(ER_FEATURE_EMPTY);

    if not df.Schema.HasColumn(f) then
    begin
      var cols := df.Schema.ColumnNames.JoinToString(', ');
      ArgumentError(ER_DATAPIPE_FEATURE_NOT_FOUND, f, cols);
    end;

    if seen.Contains(f) then
      ArgumentError(ER_DATAPIPE_DUPLICATE_FEATURE, f);

    seen.Add(f);
  end;
end;

function UDataPipeline.ToString: string;
begin
  var sb := 'UDataPipeline (' +
            (if fFitted then 'trained' else 'not trained') + '):' + NewLine;

  var idx := 1;

  foreach var s in fDataSteps do
  begin
    sb += '  [' + idx + '] ' + s.ToString + NewLine;
    idx += 1;
  end;

  foreach var t in fMatrixSteps do
  begin
    sb += '  [' + idx + '] ' + t.ToString + NewLine;
    idx += 1;
  end;

  if fModel <> nil then
    sb += '  [' + idx + '] ' + fModel.ToString;

  Result := sb;
end;

end.