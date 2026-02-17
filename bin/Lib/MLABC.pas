/// Основной модуль библиотеки машинного обучения.
/// Объединяет модели, метрики, валидацию и вспомогательные компоненты.
unit MLABC;

interface 

uses LinearAlgebraML;
uses ValidationML;
uses MLModelsABC;
uses MetricsABC;
uses PreprocessorABC;
USES DataFrameABC;

type 
  Vector = LinearAlgebraML.Vector;
  Matrix = LinearAlgebraML.Matrix;
  Validation = ValidationML.Validation;
  ConfusionMatrix = MetricsABC.ConfusionMatrix;
  Metrics = MetricsABC.Metrics;
  DataPipeline = PreprocessorABC.DataPipeline;
  DataStandardScaler = PreprocessorABC.DataStandardScaler;
  DataFrame = DataFrameABC.DataFrame;
  Statistics = DataFrameABC.Statistics;
  CsvLoader = DataFrameABC.CsvLoader;

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
  MulticlassLogisticRegression = MLModelsABC.MulticlassLogisticRegression;
  
implementation

function ToMatrix(Self: DataFrame; colNames: array of string): Matrix; extensionmethod;
begin
  var df := Self;
  var n := df.RowCount;
  var p := colNames.Length;

  if p = 0 then
    raise new Exception('ToMatrix: no columns specified');

  Result := new Matrix(n, p);

  for var j := 0 to p - 1 do
  begin
    var col := df[colNames[j]];

    for var i := 0 to n - 1 do
    begin
      var value: real;

      if not col.TryGetNumericValue(i, value) then
        raise new Exception(
          'ToMatrix: column "' + colNames[j] +
          '" contains non-numeric or NA values');

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
      raise new Exception(
        'ToVector: column "' + colName +
        '" contains non-numeric or NA values');

    Result[i] := value;
  end;
end;

    
end.