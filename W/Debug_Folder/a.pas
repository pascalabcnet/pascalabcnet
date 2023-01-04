{$reference Microsoft.ML.Core.dll}
{$reference Microsoft.ML.Data.dll}
{$reference Microsoft.ML.KMeansClustering.dll}

uses Microsoft.ML;
uses Microsoft.ML.Data;
uses Microsoft.ML.Trainers;
uses Microsoft.ML.Transforms;
 
begin
  var Context := new MLContext();
  {var loader := Context.Data.CreateTextLoader(
    Arr(
        new TextLoader.Column('SentimentText', DataKind.String, 1),
        new TextLoader.Column('Label', DataKind.Boolean, 0)
    ));}
  var fullData := Context.Data.LoadFromTextFile('iris-full.txt',
    Arr(
      new TextLoader.Column('Label', DataKind.Single, 0),
      new TextLoader.Column('SepalLength', DataKind.Single, 1),
      new TextLoader.Column('SepalWidth', DataKind.Single, 2),
      new TextLoader.Column('PetalLength', DataKind.Single, 3),
      new TextLoader.Column('PetalWidth', DataKind.Single, 4)
    ),
    #9, True
  );
  var trainTestData := Context.Data.TrainTestSplit(fullData, 0.2);
  var trainingDataView := trainTestData.TrainSet;
  var testingDataView := trainTestData.TestSet;
  var dataProcessPipeline := Context.Transforms.Concatenate('Features', 
    'SepalLength', 'SepalWidth', 'PetalLength', 'PetalWidth');
  
  var trainer: IEstimator<ClusteringPredictionTransformer<KMeansModelParameters>>;
  trainer := Context.Clustering.Trainers.KMeans('Features', nil, 3);
  //trainer := Context.Clustering.Trainers.KMeans('Features', nil, 3);
  var trainingPipeline := dataProcessPipeline.Append(trainer);
  //var trainedModel := trainingPipeline.Fit(trainingDataView);
end.