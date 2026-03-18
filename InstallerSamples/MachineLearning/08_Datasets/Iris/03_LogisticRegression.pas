uses MLABC;

begin
  var ds := Datasets.Iris;
  var df := ds.Data;

  var X := df.ToMatrix(ds.Features);
  var y := df.EncodeLabels(ds.Target); 
  
  var model := new LogisticRegression;
  
  model.Fit(X, y); 

  var pred := model.Predict(X);

  Println('Accuracy:', Metrics.Accuracy(y, pred):0:3);
end.