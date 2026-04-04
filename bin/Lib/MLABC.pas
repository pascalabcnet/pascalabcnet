/// Основной модуль библиотеки машинного обучения.
/// Объединяет модели, метрики, валидацию и вспомогательные компоненты.
unit MLABC;

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

type 
  Vector = LinearAlgebraML.Vector;
  Matrix = LinearAlgebraML.Matrix;
  
  Validation = ValidationML.Validation;
  
  Metrics = MetricsABC.Metrics;
  ClassificationMetrics = MetricsABC.ClassificationMetrics;
  RegressionMetrics = MetricsABC.RegressionMetrics;
  ClusteringMetrics = MetricsABC.ClusteringMetrics;
  ConfusionMatrix = MetricsABC.ConfusionMatrix;
  
  DataPipeline = MLPipelineABC.DataPipeline;
  
  DataFrame = DataFrameABC.DataFrame;
  DataFrameCursor = DataFrameABCCore.DataFrameCursor;
  
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
  Pipeline = MLModelsABC.Pipeline;
  
  LinearRegression = MLModelsABC.LinearRegression;
  LogisticRegression = MLModelsABC.LogisticRegression;
  RidgeRegression = MLModelsABC.RidgeRegression;
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
  LabelEncoder = PreprocessorABC.LabelEncoder;
  OneHotEncoder = PreprocessorABC.OneHotEncoder;
  ImputeStrategy = PreprocessorABC.ImputeStrategy;
  Imputer = PreprocessorABC.Imputer;
  
  Datasets = MLDatasets.Datasets;
  Dataset = MLDatasets.Dataset;
  
  IModel = MLCoreABC.IModel;
  ISupervisedModel = MLCoreABC.ISupervisedModel;
  IUnsupervisedModel = MLCoreABC.IUnsupervisedModel;
  UPipeline = MLModelsABC.UPipeline;
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

  function LabelsToInts(y: Vector): array of integer;
  function EncodeLabels(labels: array of string): array of integer;

  
implementation

function LabelsToInts(y: Vector): array of integer;
begin
  Result := DataAdapters.LabelsToInts(y);
end;

function EncodeLabels(labels: array of string): array of integer := DataAdapters.EncodeLabels(labels);
  
end.