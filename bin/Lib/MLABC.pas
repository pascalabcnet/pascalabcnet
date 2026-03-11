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
uses MLExceptions;
uses InspectionML;
uses MLPipelineABC;
uses MLDatasets;
uses DataAdapters;

type 
  Vector = LinearAlgebraML.Vector;
  Matrix = LinearAlgebraML.Matrix;
  
  Validation = ValidationML.Validation;
  
  ConfusionMatrix = MetricsABC.ConfusionMatrix;
  Metrics = MetricsABC.Metrics;
  
  DataPipeline = MLPipelineABC.DataPipeline;
  
  DataFrame = DataFrameABC.DataFrame;
  Statistics = DataFrameABC.Statistics;
  CsvLoader = DataFrameABC.CsvLoader;
  
  IProbabilisticClassifier = MLCoreABC.IProbabilisticClassifier;

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
  
  function LabelsToInts(y: Vector): array of integer;
  function EncodeLabels(labels: array of string): array of integer;

  
implementation

function LabelsToInts(y: Vector): array of integer;
begin
  Result := DataAdapters.LabelsToInts(y);
end;

function EncodeLabels(labels: array of string): array of integer := DataAdapters.EncodeLabels(labels);
  
end.