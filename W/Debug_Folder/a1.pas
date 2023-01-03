{$reference Microsoft.ML.Core.dll}
{$reference Microsoft.ML.Data.dll}
{$reference Microsoft.ML.KMeansClustering.dll}

uses Microsoft.ML;
uses Microsoft.ML.Data;
uses Microsoft.ML.Trainers;
 
begin
  var trainer: IEstimator<ClusteringPredictionTransformer<KMeansModelParameters>>;
end.