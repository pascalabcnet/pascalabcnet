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
  
/// Преобразует вектор меток классов в массив целых чисел.
/// Используется при визуализации и других задачах,
///   где метки должны быть представлены как 0,1,2,...
/// Значения округляются функцией Round, чтобы устранить
///   возможные небольшие численные ошибки 
function LabelsToInts(y: Vector): array of integer;
  
implementation
  
function LabelsToInts(y: Vector): array of integer;
begin
  if y = nil then
    ArgumentNullError(ER_ARG_NULL, 'y');

  Result := ArrGen(y.Length, i -> Round(y[i]));
end;
    
end.