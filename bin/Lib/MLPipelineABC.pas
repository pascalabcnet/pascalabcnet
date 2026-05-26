unit MLPipelineABC;

interface

uses MLCoreABC;
uses PreprocessorABC;
uses DataFrameABC;
uses LinearAlgebraML;

type 
  TaskKind = (tkRegression, tkClassification);

type
  PipelineBase = abstract class
  protected
    fDataSteps: List<IPreprocessor>;
    fMatrixSteps: List<ITransformer>;
    fFeatures: array of string;
    fFinalFeatures: array of string;
    fFitted: boolean;

    procedure ValidateSchema(df: DataFrame); virtual; abstract;
    class procedure ValidateFeatureList(features: array of string);
    
    /// Вычисляет итоговый список признаков после всех DataFrame-преобразований.
    ///
    /// Для каждого исходного признака из fFeatures:
    ///   • пытается получить расширенные столбцы через IColumnExpander;
    ///   • если расширения нет, использует сам исходный столбец.
    ///
    /// Возвращает только те столбцы, которые реально присутствуют в current,
    /// без повторов и в итоговом порядке для построения матрицы признаков.
    ///
    /// Если после preprocessing не осталось ни одного признака,
    /// выбрасывается исключение
    function ResolveFinalFeatures(current: DataFrame): array of string;

    function PrepareMatrix(df: DataFrame; var current: DataFrame): Matrix;
    function TransformDataFrame(df: DataFrame): DataFrame;
    
    function FitTransformMatrix(var X: Matrix; y: Vector := nil): Matrix;
    
    function TransformMatrix(X: Matrix): Matrix;
  public
    constructor Create;
    
    /// Признак того, что был вызван Fit.
    property IsFitted: boolean read fFitted;
  end;
  
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
  /// В конвейере используются:
  ///   • features — признаки;
  ///   • target — целевая переменная.
  ///
  DataPipeline = class(PipelineBase, IModel)
  private
    fModel: ISupervisedModel;
    fTask: TaskKind;
    fTarget: string;

  protected
    procedure ValidateSchema(df: DataFrame); override;
  public
    /// Создаёт пустой конвейер
    constructor Create;

    /// Добавляет шаг в конец конвейера.
    /// Принимает:
    ///   • IPreprocessor (DataFrame-уровень)
    ///   • ITransformer (Matrix-уровень)
    ///   • ISupervisedModel (модель, должна быть последней)
    /// Запрещено добавлять шаги после вызова Fit.
    function Add(step: IPipelineStep): DataPipeline;
    
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

    /// Строит только preprocessing-часть supervised-конвейера без модели.
    /// Модель затем можно присоединить методом WithModel
    static function BuildPreprocessing(
      task: TaskKind;
      target: string;
      features: array of string;
      params steps: array of IPipelineStep
    ): DataPipeline;

    /// Возвращает копию текущего preprocessing-конвейера с добавленной моделью.
    function WithModel(model: ISupervisedModel): DataPipeline;
      
    /// Обучает конвейер на DataFrame.
    /// Семантика:
    ///   • выполняет Fit/Transform для всех DataFrame-шагов;
    ///   • если присутствует модель — преобразует DataFrame в (X, y) по features/target,
    ///     затем обучает матричные трансформеры и модель.
    function Fit(df: DataFrame): DataPipeline;
    
    /// Применяет только обученные DataFrame-шаги pipeline и возвращает новый DataFrame.
    ///
    /// Матричные шаги и модель при этом не используются.
    /// Метод предназначен в основном для просмотра и анализа промежуточного
    /// результата preprocessing.
    ///
    /// Полученный DataFrame обычно не следует повторно подавать в тот же pipeline,
    /// так как DataFrame-преобразования будут выполнены ещё раз.
    function Transform(df: DataFrame): DataFrame;
    
    /// Делает предсказание модели для объектов из DataFrame.
    /// Для задач классификации возвращает внутренние индексы классов (0..K-1).
    /// В отличие от обычных моделей-классификаторов, здесь Predict
    /// возвращает не исходные метки классов, а их внутренние индексы.
    /// Доступен после обучения конвейера (Fit).
    function Predict(df: DataFrame): Vector;
    
    /// Возвращает исходные строковые метки классов для объектов из DataFrame.
    /// Это удобная расшифровка результата Predict через GetClassLabels.
    /// Доступен только для задач классификации после Fit.
    function PredictLabels(df: DataFrame): array of string;
    
    /// Возвращает матрицу вероятностей (nSamples × nClasses).
    /// Доступен только если конечная модель поддерживает IProbabilisticClassifier.
    function PredictProba(df: DataFrame): Matrix;
    
    /// Возвращает исходные метки классов в порядке внутреннего кодирования (0..K-1),
    /// соответствующем индексам, возвращаемым Predict.
    /// Доступен только для задач классификации.
    function GetClassLabels: array of string;
    
    /// Возвращает внутренние индексы истинных меток классов для DataFrame
    /// в соответствии с кодированием, полученным при Fit.
    /// Используется для вычисления метрик после Predict.
    /// Доступен только для задач классификации после Fit.
    function GetEncodedLabels(df: DataFrame): Vector;
    
    function ToString: string; override;
    
    function Name: string := Self.GetType.Name;
    
    function Clone: IModel;
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
  UDataPipeline = class(PipelineBase, IModel)
  private
    fModel: IUnsupervisedModel;
  
    procedure ValidateNumericFeatures(df: DataFrame);
    function TransformToMatrix(df: DataFrame): Matrix;
  protected  
    procedure ValidateSchema(df: DataFrame); override;
  public
    /// Создаёт пустой конвейер
    constructor Create;
  
    /// Добавляет шаг в конец конвейера.
    /// Принимает:
    ///   • IPreprocessor (DataFrame-уровень)
    ///   • ITransformer (Matrix-уровень)
    ///   • IUnsupervisedModel (модель, должна быть последней)
    function Add(step: IPipelineStep): UDataPipeline;
  
    /// Строит unsupervised-конвейер из шагов обработки данных и модели.
    static function Build(features: array of string;
      params steps: array of IPipelineStep): UDataPipeline;

    /// Строит только preprocessing-часть unsupervised-конвейера без модели.
    static function BuildPreprocessing(features: array of string;
      params steps: array of IPipelineStep): UDataPipeline;

    /// Возвращает копию текущего preprocessing-конвейера с добавленной моделью.
    function WithModel(model: IUnsupervisedModel): UDataPipeline;
  
    /// Обучает конвейер на DataFrame.
    function Fit(df: DataFrame): UDataPipeline;
  
    /// Применяет только обученные DataFrame-шаги pipeline и возвращает новый DataFrame.
    ///
    /// Матричные шаги и модель при этом не используются.
    /// Метод предназначен в основном для просмотра и анализа промежуточного
    /// результата preprocessing.
    ///
    /// Полученный DataFrame обычно не следует повторно подавать в тот же pipeline,
    /// так как DataFrame-преобразования будут выполнены ещё раз.
    function Transform(df: DataFrame): DataFrame;
  
    /// Обучает конвейер и сразу возвращает метки кластеров.
    /// 
    /// Выполняет полный pipeline:
    ///   DataFrame-преобразования → построение матрицы признаков →
    ///   матричные трансформеры → FitPredict модели.
    /// 
    /// Используется для задач кластеризации, где нет разделения на train/test.
    /// 
    /// Доступен только если:
    ///   • конечная модель поддерживает кластеризацию (IClusterer).
    function FitPredict(df: DataFrame): array of integer;
  
    /// Делает предсказание модели для объектов из DataFrame.
    function Predict(df: DataFrame): Vector;
    
    /// Возвращает метки кластеров для объектов из DataFrame.
    /// 
    /// Выполняет последовательные преобразования данных (DataFrame и матричные шаги),
    /// затем применяет обученную модель кластеризации.
    /// 
    /// Доступен только если:
    ///   • конечная модель поддерживает кластеризацию (IClusterer);
    ///   • конвейер был обучен (Fit).
    function PredictLabels(df: DataFrame): array of integer;
  
    function ToString: string; override;
    
    function Clone: IModel;
    
    function Name: string := Self.GetType.Name;
  end;  
  
implementation

uses MLExceptions;
uses DataAdapters;
uses MLUtilsABC;

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
  ER_LABEL_INDEX_OUT_OF_RANGE =
    'Индекс метки {0} вне диапазона [0, {1})!!Label index {0} is out of range [0, {1})';
  ER_ORDINALENCODER_TARGET_NOT_ALLOWED =
    'OrdinalEncoder нельзя применять к целевой переменной — кодирование выполняется внутри модели!!OrdinalEncoder cannot be applied to target — encoding is handled internally by the model';
  ER_ENCODELABELS_NOT_CATEGORICAL =
    'Целевой столбец должен быть категориальным для задач классификации!!Target column must be categorical for classification tasks';
  ER_CLASSIFICATION_TARGET_MUST_BE_CATEGORICAL_STR_OR_INT =
    'Целевой столбец "{0}" должен быть категориальным строковым или целочисленным для задач классификации!!' +
    'Target column "{0}" must be a categorical string or integer column for classification tasks';
  ER_REGRESSION_TARGET_MUST_BE_NUMERIC =
    'Целевой столбец "{0}" должен быть числовым для задач регрессии!!Target column "{0}" must be numeric for regression tasks';
  ER_PREPROCESSOR_ROWCOUNT_CHANGED =
    'DataFrame-преобразователь не должен изменять число строк!!DataFrame preprocessor must preserve RowCount';
  ER_PIPELINE_TARGET_TRANSFORM_NOT_ALLOWED =
    'Преобразование целевой переменной "{0}" запрещено в DataPipeline!!' +
    'Transformation of target variable "{0}" is not allowed in DataPipeline';    
  ER_DATAPIPE_INVALID_TASK =
    'DataPipeline поддерживает только классификацию и регрессию!!' +
    'DataPipeline supports classification and regression only';  
  ER_PIPELINE_NO_STEPS =
    'Pipeline не содержит шагов!!' +
    'Pipeline must contain at least one step';  
  ER_PIPELINE_LAST_NOT_UNSUPERVISED_MODEL =
    'Последний шаг Pipeline должен быть unsupervised-моделью!!' +
    'Last Pipeline step must be an unsupervised model';  
  ER_PIPELINE_INVALID_STEP_ORDER =
    'Неверный порядок шагов в Pipeline: модель должна быть последней!!' +
    'Invalid pipeline step order: model must be the last step';
  ER_PIPELINE_LAST_NOT_SUPERVISED_MODEL =
    'Последний шаг Pipeline должен быть supervised-моделью!!' +
    'Last Pipeline step must be a supervised model';
  ER_MODEL_NOT_CLASSIFIER =
    'Модель не является классификатором!!' +
    'Model is not a classifier';
  ER_MODEL_CLONE_TYPE = 
    'Clone модели вернул неподдерживаемый тип!!Model Clone returned unsupported type';
  ER_INVALID_MODEL_TYPE =
    'Clone модели вернул неподдерживаемый тип (ожидается {0})!!Model Clone returned unsupported type (expected {0})';
//-----------------------------
//        DataPipeline
//-----------------------------

constructor PipelineBase.Create;
begin
  fDataSteps := new List<IPreprocessor>;
  fMatrixSteps := new List<ITransformer>;
  fFeatures := nil;
  fFitted := false;
end;

class procedure PipelineBase.ValidateFeatureList(features: array of string);
begin
  if (features = nil) or (features.Length = 0) then
    ArgumentError(ER_FEATURES_EMPTY);

  var seen := new HashSet<string>;

  foreach var f in features do
  begin
    if (f = nil) or (f = '') then
      ArgumentError(ER_FEATURE_EMPTY);

    if f in seen then
      ArgumentError(ER_DATAPIPE_DUPLICATE_FEATURE, f);

    seen.Add(f);
  end;
end;

function PipelineBase.ResolveFinalFeatures(current: DataFrame): array of string;
begin
  if current = nil then
    ArgumentNullError(ER_ARG_NULL, 'current');

  var feats := new List<string>;

  foreach var f in fFeatures do
  begin
    var expanded := false;

    // ищем expander-ы В ПРЯМОМ порядке pipeline
    for var i := 0 to fDataSteps.Count - 1 do
    begin
      var expander := fDataSteps[i] as IColumnExpander;
      if expander = nil then
        continue;

      var cols := expander.GetExpandedColumns(f);
      if (cols <> nil) and (cols.Length > 0) then
      begin
        foreach var c in cols do
          if current.HasColumn(c) then
            if c not in feats then
              feats.Add(c);

        expanded := true;
        break;
      end;
    end;

    if expanded then
      continue;

    // fallback: обычный столбец
    if current.HasColumn(f) then
      if f not in feats then
        feats.Add(f);
  end;

  if feats.Count = 0 then
    ArgumentError(ER_PIPELINE_NO_FEATURES);

  Result := feats.ToArray;
end;

function PipelineBase.PrepareMatrix(df: DataFrame; var current: DataFrame): Matrix;
begin
  if df = nil then
    ArgumentNullError(ER_ARG_NULL, 'df');

  current := df;

  // --- schema
  ValidateSchema(current);

  // --- DataFrame steps
  for var i := 0 to fDataSteps.Count - 1 do
  begin
    var prevRows := current.RowCount;
    fDataSteps[i] := fDataSteps[i].Fit(current);
    current := fDataSteps[i].Transform(current);
    if current.RowCount <> prevRows then
      Error(ER_PREPROCESSOR_ROWCOUNT_CHANGED);
  end;

  fFinalFeatures := ResolveFinalFeatures(current);

  Result := current.ToMatrix(fFinalFeatures);
end;

function PipelineBase.TransformDataFrame(df: DataFrame): DataFrame;
begin
  if df = nil then
    ArgumentNullError(ER_ARG_NULL, 'df');
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  var current := df;
  foreach var s in fDataSteps do
  begin
    var prevRows := current.RowCount;
    current := s.Transform(current);
    if current.RowCount <> prevRows then
      Error(ER_PREPROCESSOR_ROWCOUNT_CHANGED);
  end;

  Result := current;
end;

function PipelineBase.FitTransformMatrix(var X: Matrix; y: Vector): Matrix;
begin
  for var i := 0 to fMatrixSteps.Count - 1 do
  begin
    var t := fMatrixSteps[i];

    if y <> nil then
    begin
      if t is ISupervisedTransformer(var sup) then
        fMatrixSteps[i] := sup.Fit(X, y)
      else if t is IUnsupervisedTransformer(var unsup) then
        fMatrixSteps[i] := unsup.Fit(X)
      else
        ArgumentError(ER_MATRIXSTEP_NO_FIT, i);
    end
    else
    begin
      if t is IUnsupervisedTransformer(var unsup) then
        fMatrixSteps[i] := unsup.Fit(X)
      else
        ArgumentError(ER_MATRIXSTEP_NO_FIT, i);
    end;

    X := fMatrixSteps[i].Transform(X);
  end;

  Result := X;
end;

function PipelineBase.TransformMatrix(X: Matrix): Matrix;
begin
  foreach var t in fMatrixSteps do
    X := t.Transform(X);

  Result := X;
end;

//-----------------------------
//        DataPipeline
//-----------------------------

constructor DataPipeline.Create;
begin
  inherited Create;
  fModel := nil;
  fTarget := '';
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
  if step is ISupervisedModel then
  begin
    if fModel <> nil then
      ArgumentError(ER_PIPELINE_MULTIPLE_MODELS);

    fModel := step as ISupervisedModel;
    exit(Self);
  end;

  ArgumentError(ER_DATAPIPE_UNKNOWN_STEP_TYPE, step.ToString);
  Result := Self;
end;

class function DataPipeline.BuildPreprocessing(
  task: TaskKind;
  target: string;
  features: array of string;
  params steps: array of IPipelineStep
): DataPipeline;
begin
  if not (task in [TaskKind.tkClassification, TaskKind.tkRegression]) then
    ArgumentError(ER_DATAPIPE_INVALID_TASK);

  if (target = nil) or (target = '') then
    ArgumentError(ER_TARGET_EMPTY);

  ValidateFeatureList(features);

  foreach var f in features do
    if f = target then
      ArgumentError(ER_DATAPIPE_TARGET_IN_FEATURES, target);

  if (steps = nil) or (Length(steps) = 0) then
    ArgumentError(ER_PIPELINE_NO_STEPS);

  for var i := 0 to High(steps) do
  begin
    var step := steps[i];

    if step = nil then
      ArgumentError(ER_PIPELINE_STEP_NULL, i);

    if step is ISupervisedModel then
      ArgumentError(ER_PIPELINE_INVALID_STEP_ORDER);
  end;

  var p := new DataPipeline;
  p.fTarget := target;
  p.fFeatures := Copy(features);
  p.fTask := task;

  for var i := 0 to High(steps) do
    p.Add(steps[i]);

  Result := p;
end;

class function DataPipeline.Build(
  task: TaskKind;
  target: string;
  features: array of string; 
  params steps: array of IPipelineStep
): DataPipeline;
begin
  if (steps = nil) or (Length(steps) = 0) then
    ArgumentError(ER_PIPELINE_NO_STEPS);

  var last := steps[High(steps)];

  if last = nil then
    ArgumentError(ER_PIPELINE_STEP_NULL, High(steps));

  if not (last is ISupervisedModel) then
    ArgumentError(ER_PIPELINE_LAST_NOT_SUPERVISED_MODEL);
  
  var modelFound := false;

  for var i := 0 to High(steps) do
  begin
    if steps[i] is ISupervisedModel then
    begin
      if modelFound then
        ArgumentError(ER_PIPELINE_MULTIPLE_MODELS);
  
      modelFound := true;
    end;
  end;

  for var i := 0 to High(steps) - 1 do
  begin
    var step := steps[i];

    if step = nil then
      ArgumentError(ER_PIPELINE_STEP_NULL, i);

    if step is ISupervisedModel then
      ArgumentError(ER_PIPELINE_INVALID_STEP_ORDER);
  end;

  var prepSteps := new IPipelineStep[steps.Length - 1];
  for var i := 0 to High(prepSteps) do
    prepSteps[i] := steps[i];

  Result :=
    BuildPreprocessing(task, target, features, prepSteps)
      .WithModel(last as ISupervisedModel);
end;

function DataPipeline.WithModel(model: ISupervisedModel): DataPipeline;
begin
  if model = nil then
    ArgumentError(ER_MODEL_NULL);

  var p := Clone as DataPipeline;

  if p = nil then
    Error(ER_MODEL_CLONE_TYPE);

  if p.fFitted then
    Error(ER_PIPELINE_MODIFY_AFTER_FIT);

  if p.fModel <> nil then
    ArgumentError(ER_PIPELINE_MULTIPLE_MODELS);

  p.Add(model as IPipelineStep);
  Result := p;
end;

function DataPipeline.Fit(df: DataFrame): DataPipeline;
begin
  if df = nil then
    ArgumentNullError(ER_ARG_NULL, 'df');
  if fModel = nil then
    ArgumentError(ER_MODEL_NULL);

  var current: DataFrame;
  var X := PrepareMatrix(df, current);

  if not current.HasColumn(fTarget) then
    ArgumentError(ER_PIPELINE_TARGET_REMOVED, fTarget);

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

  X := FitTransformMatrix(X, y);

  fModel := fModel.Fit(X, y);

  if fTask = tkClassification then
  begin
    if fModel is IClassifierInternal(var cls) then
      cls.SetClassLabels(classes)
    else
      Error(ER_MODEL_NOT_CLASSIFIER);
  end;

  fFitted := true;
  Result := Self;
end;

function DataPipeline.Transform(df: DataFrame): DataFrame := TransformDataFrame(df);


function DataPipeline.Predict(df: DataFrame): Vector;
begin
  if df = nil then
    ArgumentNullError(ER_ARG_NULL, 'df');
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if fModel = nil then
    ArgumentError(ER_MODEL_NULL);

  var current := Transform(df);

  var X := current.ToMatrix(fFinalFeatures);
  X := TransformMatrix(X);

  if not (fModel is IPredictiveModel) then
    Error(ER_PREDICT_NOT_SUPPORTED);

  Result := (fModel as IPredictiveModel).Predict(X);
end;

function DataPipeline.PredictLabels(df: DataFrame): array of string;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if fTask <> tkClassification then
    ArgumentError(ER_NOT_CLASSIFICATION);

  var encoded := LabelsToInts(Predict(df));
  var classes := GetClassLabels;
  Result := new string[encoded.Length];
  
  for var i := 0 to encoded.Length - 1 do
  begin
    var idx := encoded[i];
    if (idx < 0) or (idx >= classes.Length) then
      Error(ER_LABEL_INDEX_OUT_OF_RANGE, idx, classes.Length);
    
    Result[i] := classes[idx];
  end;
end;

function DataPipeline.PredictProba(df: DataFrame): Matrix;
begin
  if df = nil then
    ArgumentNullError(ER_ARG_NULL, 'df');
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if fTask <> tkClassification then
    ArgumentError(ER_NOT_CLASSIFICATION);
  
  if not (fModel is IProbabilisticClassifier) then
    ArgumentError(ER_PROBA_NOT_SUPPORTED);

  var current := Transform(df);

  var X := current.ToMatrix(fFinalFeatures);
  X := TransformMatrix(X);

  Result := (fModel as IProbabilisticClassifier).PredictProba(X);
end;

function DataPipeline.GetEncodedLabels(df: DataFrame): Vector;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if fTask <> tkClassification then
    ArgumentError(ER_NOT_CLASSIFICATION);

  if df = nil then
    ArgumentNullError(ER_ARG_NULL, 'df');

  if not df.HasColumn(fTarget) then
    ArgumentError(ER_COLUMN_NOT_FOUND, fTarget);

  var classes := GetClassLabels;
  var labels := df.TransformLabels(fTarget, classes);
  Result := new Vector(labels);
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

  ValidateFeatureList(fFeatures);

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
  
  case fTask of
    tkClassification:
      begin
        if not df.IsCategorical(fTarget) then
          ArgumentError(ER_ENCODELABELS_NOT_CATEGORICAL, fTarget);
        var ct := df.GetColumnType(fTarget);
        if not (ct in [ColumnType.ctStr, ColumnType.ctInt]) then
          ArgumentError(ER_CLASSIFICATION_TARGET_MUST_BE_CATEGORICAL_STR_OR_INT, fTarget);
      end;

    tkRegression:
      begin
        var ct := df.GetColumnType(fTarget);
        if not (ct in [ColumnType.ctInt, ColumnType.ctFloat]) then
          ArgumentError(ER_REGRESSION_TARGET_MUST_BE_NUMERIC, fTarget);
      end;
  end;
  
  for var i := 0 to High(fFeatures) do
  begin
    var f := fFeatures[i];

    if not df.Schema.HasColumn(f) then
    begin
      var cols := df.Schema.ColumnNames.JoinToString(', ');
      ArgumentError(ER_DATAPIPE_FEATURE_NOT_FOUND, f, cols);
    end;
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

function DataPipeline.Clone: IModel;
begin
  var p := new DataPipeline;

  // --- конфигурация
  p.fFeatures := Copy(fFeatures);
  p.fTarget := fTarget;
  p.fTask := fTask;

  // --- шаги (глубокая копия)
  p.fDataSteps := fDataSteps.Select(s -> s.Clone).ToList;
  p.fMatrixSteps := fMatrixSteps.Select(s -> s.Clone).ToList;

  // --- модель (глубокая копия конфигурации)
  if fModel <> nil then
  begin  
    var m := fModel.Clone;
    if not (m is ISupervisedModel) then
      Error(ER_MODEL_CLONE_TYPE);
    
    p.fModel := m as ISupervisedModel;   
  end;  

  // --- состояние НЕ копируем
  p.fFinalFeatures := nil;
  p.fFitted := false;

  Result := p;
end;

//-----------------------------
//        UDataPipeline 
//-----------------------------

constructor UDataPipeline.Create;
begin
  inherited Create;
  fModel := nil;
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
  if step is IUnsupervisedModel then
  begin
    if fModel <> nil then
      ArgumentError(ER_PIPELINE_MULTIPLE_MODELS);

    fModel := step as IUnsupervisedModel;
    exit(Self);
  end;

  ArgumentError(ER_DATAPIPE_UNKNOWN_STEP_TYPE, step.ToString);
  Result := Self;
end;

class function UDataPipeline.BuildPreprocessing(features: array of string;
  params steps: array of IPipelineStep): UDataPipeline;
begin
  ValidateFeatureList(features);

  if (steps = nil) or (Length(steps) = 0) then
    ArgumentError(ER_PIPELINE_NO_STEPS);

  for var i := 0 to High(steps) do
  begin
    var step := steps[i];

    if step = nil then
      ArgumentError(ER_PIPELINE_STEP_NULL, i);

    if step is IUnsupervisedModel then
      ArgumentError(ER_PIPELINE_INVALID_STEP_ORDER);
  end;

  var p := new UDataPipeline;
  p.fFeatures := Copy(features);

  for var i := 0 to High(steps) do
    p.Add(steps[i]);

  Result := p;
end;

class function UDataPipeline.Build(features: array of string;
  params steps: array of IPipelineStep): UDataPipeline;
begin
  if (steps = nil) or (Length(steps) = 0) then
    ArgumentError(ER_PIPELINE_NO_STEPS);

  var last := steps[High(steps)];

  if last = nil then
    ArgumentError(ER_PIPELINE_STEP_NULL, High(steps));

  if not (last is IUnsupervisedModel) then
    ArgumentError(ER_PIPELINE_LAST_NOT_UNSUPERVISED_MODEL);

  for var i := 0 to High(steps) - 1 do
  begin
    var step := steps[i];

    if step = nil then
      ArgumentError(ER_PIPELINE_STEP_NULL, i);

    if step is IUnsupervisedModel then
      ArgumentError(ER_PIPELINE_INVALID_STEP_ORDER);
  end;

  var prepSteps := new IPipelineStep[steps.Length - 1];
  for var i := 0 to High(prepSteps) do
    prepSteps[i] := steps[i];

  Result :=
    BuildPreprocessing(features, prepSteps)
      .WithModel(last as IUnsupervisedModel);
end;

function UDataPipeline.WithModel(model: IUnsupervisedModel): UDataPipeline;
begin
  if model = nil then
    ArgumentError(ER_MODEL_NULL);

  var p := Clone as UDataPipeline;

  if p = nil then
    Error(ER_INVALID_MODEL_TYPE, 'UDataPipeline');

  if p.fFitted then
    Error(ER_PIPELINE_MODIFY_AFTER_FIT);

  if p.fModel <> nil then
    ArgumentError(ER_PIPELINE_MULTIPLE_MODELS);

  p.Add(model as IPipelineStep);
  Result := p;
end;

function UDataPipeline.Fit(df: DataFrame): UDataPipeline;
begin
  if df = nil then
    ArgumentNullError(ER_ARG_NULL, 'df');
  if fModel = nil then
    ArgumentError(ER_MODEL_NULL);

  var current: DataFrame;
  var X := PrepareMatrix(df, current);

  X := FitTransformMatrix(X);

  fModel := fModel.Fit(X);

  fFitted := true;
  Result := Self;
end;

function UDataPipeline.Transform(df: DataFrame): DataFrame := TransformDataFrame(df);

function UDataPipeline.FitPredict(df: DataFrame): array of integer;
begin
  if df = nil then
    ArgumentNullError(ER_ARG_NULL, 'df');

  if fModel = nil then
    ArgumentError(ER_MODEL_NULL);

  if not (fModel is IClusterer) then
    ArgumentError(ER_MODEL_NOT_CLUSTERER, fModel.GetType.Name);

  var current := df;

  // --- 0) Проверка входной схемы
  ValidateSchema(current);

  // --- 1) DataFrame шаги
  for var i := 0 to fDataSteps.Count - 1 do
  begin
    var prevRows := current.RowCount;
    fDataSteps[i] := fDataSteps[i].Fit(current);
    current := fDataSteps[i].Transform(current);
    if current.RowCount <> prevRows then
      Error(ER_PREPROCESSOR_ROWCOUNT_CHANGED);
  end;

  // --- 2) вычислить финальные признаки
  fFinalFeatures := ResolveFinalFeatures(current);

  var X := current.ToMatrix(fFinalFeatures);

  // --- 3) Matrix transformers
  X := FitTransformMatrix(X);

  // --- 4) модель
  var cl := fModel as IClusterer;
  Result := cl.FitPredict(X);

  fFitted := true;
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

  if fFinalFeatures = nil then
    Error(ER_PIPELINE_FINALFEATURES);
  
  for var i := 0 to High(fFinalFeatures) do
    if not current.HasColumn(fFinalFeatures[i]) then
      ArgumentError(ER_PIPELINE_FEATURE_NOT_FOUND, fFinalFeatures[i]);
  
  var X := current.ToMatrix(fFinalFeatures);

  X := TransformMatrix(X);

  Result := X;
end;

function UDataPipeline.Predict(df: DataFrame): Vector;
begin
  if df = nil then
    ArgumentNullError(ER_ARG_NULL, 'df');

  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if fModel = nil then
    ArgumentError(ER_MODEL_NULL);

  var X := TransformToMatrix(df);

  if not (fModel is IPredictiveModel) then
    Error(ER_PREDICT_NOT_SUPPORTED);

  Result := (fModel as IPredictiveModel).Predict(X);
end;

function UDataPipeline.PredictLabels(df: DataFrame): array of integer;
begin
  if df = nil then
    ArgumentNullError(ER_ARG_NULL, 'df');

  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if fModel = nil then
    ArgumentError(ER_MODEL_NULL);

  if not (fModel is IPredictiveClusterer) then
    Error(ER_PREDICT_NOT_SUPPORTED);

  var cl := fModel as IPredictiveClusterer;

  var X := TransformToMatrix(df);

  Result := cl.PredictLabels(X);
end;

procedure UDataPipeline.ValidateSchema(df: DataFrame);
begin
  ValidateFeatureList(fFeatures);

  for var i := 0 to High(fFeatures) do
  begin
    var f := fFeatures[i];

    if not df.Schema.HasColumn(f) then
    begin
      var cols := df.Schema.ColumnNames.JoinToString(', ');
      ArgumentError(ER_DATAPIPE_FEATURE_NOT_FOUND, f, cols);
    end;
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

function UDataPipeline.Clone: IModel;
begin
  var p := new UDataPipeline;

  // --- конфигурация
  p.fFeatures := Copy(fFeatures);

  // --- шаги (глубокая копия)
  p.fDataSteps := fDataSteps.Select(s -> s.Clone).ToList;
  p.fMatrixSteps := fMatrixSteps.Select(s -> s.Clone).ToList;

  // --- модель
  if fModel <> nil then
  begin
    var m := fModel.Clone;

    if not (m is IUnsupervisedModel) then
      Error(ER_INVALID_MODEL_TYPE, 'IUnsupervisedModel');

    p.fModel := m as IUnsupervisedModel;
  end;

  // --- состояние НЕ копируем
  p.fFinalFeatures := nil;
  p.fFitted := false;

  Result := p;
end;

end.
