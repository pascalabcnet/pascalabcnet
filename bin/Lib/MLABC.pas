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
// PIPELINES
//
// DataFrame-based:
//   DataPipeline
//   UDataPipeline
//
// Matrix/Vector-based:
//   MatrixPipeline
//   UMatrixPipeline
//
// Оба варианта являются равноправными и используются
// в зависимости от представления данных.
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
  
  DataFrame = DataFrameABC.DataFrame;
  DataFrameCursor = DataFrameABCCore.DataFrameCursor;
  ColumnType = DataFrameABCCore.ColumnType;
  Column = DataFrameABCCore.Column;
  
  Statistics = DataFrameABC.Statistics;
  CsvLoader = DataFrameABC.CsvLoader;
  JoinKind = DataFrameABC.JoinKind;
  GroupView = DataFrameABC.GroupView;
  
  IProbabilisticClassifier = MLCoreABC.IProbabilisticClassifier;
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
  
  LinearRegression = MLModelsABC.LinearRegression;
  LogisticRegression = MLModelsABC.LogisticRegression;
  RidgeRegression = MLModelsABC.RidgeRegression;
  LassoRegression = MLModelsABC.LassoRegression;
  ElasticNet = MLModelsABC.ElasticNet;
  DecisionTreeClassifier = MLModelsABC.DecisionTreeClassifier;
  DecisionTreeRegressor = MLModelsABC.DecisionTreeRegressor;
  RandomForestRegressor = MLModelsABC.RandomForestRegressor;
  RandomForestClassifier = MLModelsABC.RandomForestClassifier;
  GradientBoostingRegressor = MLModelsABC.GradientBoostingRegressor;
  GradientBoostingClassifier = MLModelsABC.GradientBoostingClassifier;
  KNNClassifier = MLModelsABC.KNNClassifier;
  KNNRegressor = MLModelsABC.KNNRegressor;
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
  ImputeStrategy = PreprocessorABC.ImputeStrategy;
  Imputer = PreprocessorABC.Imputer;
  
  Datasets = MLDatasets.Datasets;
  Dataset = MLDatasets.Dataset;
  LabelEncoder = MLDatasets.LabelEncoder;
  
  IModel = MLCoreABC.IModel;
  ISupervisedModel = MLCoreABC.ISupervisedModel;
  IUnsupervisedModel = MLCoreABC.IUnsupervisedModel;
  
  UMatrixPipeline = MLModelsABC.UMatrixPipeline;
  UDataPipeline = MLPipelineABC.UDataPipeline;
  TaskKind = MLPipelineABC.TaskKind;
  
  AggregationKind = DataFrameABC.AggregationKind;
  
const
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

  /// Преобразует вектор меток классов в массив целых чисел.
  /// Значения округляются функцией Round, чтобы устранить
  ///   возможные небольшие численные ошибки 
  function LabelsToInts(y: Vector): array of integer;
  
  /// Кодирует строковые метки классов в целочисленные индексы.
  /// Каждому уникальному значению присваивается номер 0,1,2,...
  /// Порядок кодирования соответствует порядку первого появления меток.
  /// Используется при обучении моделей и визуализации.
  /// Предполагается, что входные данные уже очищены от пропущенных значений.
  function EncodeLabels(labels: array of string): array of integer;

  
implementation

function LabelsToInts(y: Vector): array of integer;
begin
  Result := MLUtilsABC.LabelsToInts(y);
end;

function EncodeLabels(labels: array of string): array of integer := MLUtilsABC.EncodeLabels(labels);
  
end.
