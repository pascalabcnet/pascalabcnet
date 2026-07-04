/// Основной модуль библиотеки машинного обучения.
/// Объединяет модели, метрики, валидацию и вспомогательные компоненты.
unit MLABC;

// =============================================================
// СТАТИСТИЧЕСКАЯ ПОЛИТИКА БИБЛИОТЕКИ ML PascalABC.NET
//
// В библиотеке используются следующие соглашения:
//
// 1. DataFrame (описательная статистика):
//    • дисперсия вычисляется с делением на (n - 1)
//
// 2. LinearAlgebra и ML:
//    • дисперсия вычисляется с делением на n
//
// 3. PCA:
//    • ковариационная матрица вычисляется с делением на (n - 1)
//
// Это соответствует распространённой практике:
//   • описательная статистика — выборочная дисперсия
//   • алгоритмы ML — дисперсия генеральной совокупности
// =============================================================

// =============================================================
// КОНВЕЙЕРЫ
//
// Табличные:
//   DataPipeline
//   ClassificationDataPipeline
//   RegressionDataPipeline
//   ClusteringDataPipeline
//
// Матричные:
//   MatrixPipeline
//   ClassificationMatrixPipeline
//   RegressionMatrixPipeline
//   ClusteringMatrixPipeline
//
// DataPipeline и MatrixPipeline используются как фасады
// для построения специализированных конвейеров.
// =============================================================

// =============================================================
// ПОЛИТИКА PREDICT
//
// 1. Классификация:
//    • Для матричных моделей Predict возвращает метки классов
//      в том виде, в каком они были поданы модели при обучении.
//
//    • ClassificationMatrixPipeline следует той же политике:
//      Predict возвращает метки классов в том виде,
//      в каком они были поданы вложенной модели при обучении.
//
//    • ClassificationDataPipeline работает с DataFrame, где целевой столбец
//      часто является строковым. Поэтому внутри pipeline целевой столбец
//      кодируется в целые числа, и Predict возвращает именно эти коды.
//
//    • PredictLabels возвращает строковые метки классов.
//
//    • Таким образом, различие естественно:
//      матричные модели и ClassificationMatrixPipeline возвращают метки
//      из пространства обучения модели, а ClassificationDataPipeline
//      возвращает коды, построенные при кодировании целевого столбца.
//
// 2. Регрессия:
//    • Predict возвращает вектор числовых предсказаний.
//
// 3. Кластеризация:
//    • Predict и FitPredict возвращают номера кластеров.
// =============================================================

interface 

uses LinearAlgebraML;
uses ValidationML;
uses MLCoreABC;
uses MLModelsABC;
uses MetricsABC;
uses PreprocessorABC;
uses DataFrameABC;
uses DataFrameABCCore;
uses MLExceptions;
uses InspectionML;
uses MLPipelineABC;
uses MLDatasets;
uses DataAdapters;
uses MLUtilsABC;

type 
  Vector = LinearAlgebraML.Vector;
  Matrix = LinearAlgebraML.Matrix;
  
  Validation = ValidationML.Validation;
  GridSearch = ValidationML.GridSearch;
  
  Metrics = MetricsABC.Metrics;
  ClassificationMetrics = MetricsABC.ClassificationMetrics;
  RegressionMetrics = MetricsABC.RegressionMetrics;
  ClusteringMetrics = MetricsABC.ClusteringMetrics;
  ConfusionMatrix = MetricsABC.ConfusionMatrix;
  
  DataPipeline = MLPipelineABC.DataPipeline;
  ClassificationDataPipeline = MLPipelineABC.ClassificationDataPipeline;
  RegressionDataPipeline = MLPipelineABC.RegressionDataPipeline;
  ClusteringDataPipeline = MLPipelineABC.ClusteringDataPipeline;
  
  DataFrame = DataFrameABC.DataFrame;
  DataFrameCursor = DataFrameABCCore.DataFrameCursor;
  DataValue = DataFrameABCCore.DataValue;
  ColumnType = DataFrameABCCore.ColumnType;
  Column = DataFrameABCCore.Column;
  DatePartKind = DataFrameABC.DatePartKind;
  
  Statistics = DataFrameABC.Statistics;
  CsvLoader = DataFrameABC.CsvLoader;
  CsvSaver = DataFrameABC.CsvSaver;
  JoinKind = DataFrameABC.JoinKind;
  GroupView = DataFrameABC.GroupView;
  EncodedTarget = DataAdapters.EncodedTarget;
  
  IProbabilisticClassifier = MLCoreABC.IProbabilisticClassifier;
  IClassifier = MLCoreABC.IClassifier;
  IRegressor = MLCoreABC.IRegressor;

  StandardScaler = MLModelsABC.StandardScaler;
  PCATransformer = MLModelsABC.PCATransformer;
  MinMaxScaler = MLModelsABC.MinMaxScaler;
  VarianceThreshold = MLModelsABC.VarianceThreshold;
  SelectKBest = MLModelsABC.SelectKBest;
  FeatureScore = MLModelsABC.FeatureScore;
  Normalizer = MLModelsABC.Normalizer;
  
  NormType = MLModelsABC.NormType;
  
  Activations = MLModelsABC.Activations;
  MatrixPipeline = MLModelsABC.MatrixPipeline;
  ClassificationMatrixPipeline = MLModelsABC.ClassificationMatrixPipeline;
  RegressionMatrixPipeline = MLModelsABC.RegressionMatrixPipeline;
  ClusteringMatrixPipeline = MLModelsABC.ClusteringMatrixPipeline;
  LinearDecisionBoundary = MLModelsABC.LinearDecisionBoundary;
  LinearRegressionLine = MLModelsABC.LinearRegressionLine;
  
  LinearRegression = MLModelsABC.LinearRegression;
  LogisticRegression = MLModelsABC.LogisticRegression;
  RidgeRegression = MLModelsABC.RidgeRegression;
  LassoRegression = MLModelsABC.LassoRegression;
  ElasticNet = MLModelsABC.ElasticNet;
  DecisionTreeClassifier = MLModelsABC.DecisionTreeClassifier;
  DecisionTreeRegressor = MLModelsABC.DecisionTreeRegressor;
  DecisionTreeView = MLModelsABC.DecisionTreeView;
  RandomForestRegressor = MLModelsABC.RandomForestRegressor;
  RandomForestClassifier = MLModelsABC.RandomForestClassifier;
  GradientBoostingRegressor = MLModelsABC.GradientBoostingRegressor;
  GradientBoostingClassifier = MLModelsABC.GradientBoostingClassifier;
  KNNClassifier = MLModelsABC.KNNClassifier;
  KNNRegressor = MLModelsABC.KNNRegressor;
  NeighborInfo = MLModelsABC.NeighborInfo;
  KMeans = MLModelsABC.KMeans;
  DBSCAN = MLModelsABC.DBSCAN;
  
  KNNWeighting = MLModelsABC.KNNWeighting;
  TGBLoss = MLModelsABC.TGBLoss;
  TMaxFeaturesMode = MLModelsABC.TMaxFeaturesMode;

  MLException = MLExceptions.MLException;
  MLNotFittedException = MLExceptions.MLNotFittedException;
  MLDimensionException = MLExceptions.MLDimensionException;
  
  Inspection = InspectionML.Inspection;
  
  IPreprocessor = PreprocessorABC.IPreprocessor;
  OrdinalEncoder = PreprocessorABC.OrdinalEncoder;
  OneHotEncoder = PreprocessorABC.OneHotEncoder;
  DateTimeComponentsEncoder = PreprocessorABC.DateTimeComponentsEncoder;
  DateTimeUnit = PreprocessorABC.DateTimeUnit;
  DateTimeOrdinalEncoder = PreprocessorABC.DateTimeOrdinalEncoder;
  DateTimeCyclicEncoder = PreprocessorABC.DateTimeCyclicEncoder;
  ImputeStrategy = PreprocessorABC.ImputeStrategy;
  Imputer = PreprocessorABC.Imputer;
  
  Datasets = MLDatasets.Datasets;
  Dataset = MLDatasets.Dataset;
  LabelEncoder = MLDatasets.LabelEncoder;
  TaskType = MLDatasets.TaskType;
  
  IModel = MLCoreABC.IModel;
  IUnsupervisedModel = MLCoreABC.IUnsupervisedModel;
  
  TaskKind = MLPipelineABC.TaskKind;
  
  AggregationKind = DataFrameABC.AggregationKind;
  
const
  ctInt = ColumnType.ctInt;
  ctFloat = ColumnType.ctFloat;
  ctStr = ColumnType.ctStr;
  ctBool = ColumnType.ctBool;
  ctDateTime = ColumnType.ctDateTime;

  akMean = AggregationKind.akMean;
  akMin = AggregationKind.akMin;
  akMax = AggregationKind.akMax;
  akCount = AggregationKind.akCount;
  akSum = AggregationKind.akSum;
  akStd = AggregationKind.akStd;
  
  /// Внутреннее соединение
  jkInner = JoinKind.jkInner;
  jkLeft = JoinKind.jkLeft;
  jkRight = JoinKind.jkRight;
  jkFull = JoinKind.jkFull;
  
  dpYear = DatePartKind.dpYear;
  dpMonth = DatePartKind.dpMonth;
  dpDay = DatePartKind.dpDay;
  dpHour = DatePartKind.dpHour;
  dpMinute = DatePartKind.dpMinute;
  dpSecond = DatePartKind.dpSecond;
  dpDayOfWeek = DatePartKind.dpDayOfWeek;
  dpTimeOfDay = DatePartKind.dpTimeOfDay;
  dpDate = DatePartKind.dpDate;
  
  dtuDays = DateTimeUnit.dtuDays;
  dtuHours = DateTimeUnit.dtuHours;
  dtuMinutes = DateTimeUnit.dtuMinutes;
  dtuSeconds = DateTimeUnit.dtuSeconds;

  /// Кодирует строковые метки классов в целочисленные индексы.
  /// Каждому уникальному значению присваивается номер 0,1,2,...
  /// Порядок кодирования соответствует порядку первого появления меток.
  /// Используется при обучении моделей и визуализации.
  /// Предполагается, что входные данные уже очищены от пропущенных значений.
  function EncodeLabels(labels: array of string): array of integer;

  
implementation

function EncodeLabels(labels: array of string): array of integer := MLUtilsABC.EncodeLabels(labels);
  
end.

