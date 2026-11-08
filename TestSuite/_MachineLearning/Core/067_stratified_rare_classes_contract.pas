uses MLABC;

procedure Check(condition: boolean; message: string);
begin
  if not condition then
    raise new Exception(message);
end;

procedure CheckFolds(y: array of integer; k: integer);
begin
  var folds := Validation.StratifiedKFold(y, k, seed := 42).ToArray;
  var repeated := Validation.StratifiedKFold(y, k, seed := 42).ToArray;
  var vectorFolds := Validation.StratifiedKFold(new Vector(y), k, seed := 42).ToArray;
  Check(folds.Length = k, 'Wrong number of folds');
  var visits := new integer[y.Length];
  var minSize := y.Length;
  var maxSize := 0;
  foreach var (trainIdx, testIdx) in folds do
  begin
    Check(trainIdx.Length > 0, 'Empty training fold');
    Check(testIdx.Length > 0, 'Empty validation fold');
    minSize := Min(minSize, testIdx.Length);
    maxSize := Max(maxSize, testIdx.Length);
    var coverage := new integer[y.Length];
    foreach var i in trainIdx do
      coverage[i] += 1;
    foreach var i in testIdx do
    begin
      coverage[i] += 1;
      visits[i] += 1;
    end;
    foreach var count in coverage do
      Check(count = 1, 'Train/test overlap or omitted row');
  end;
  Check(maxSize - minSize <= 1, 'Fold sizes differ by more than one');
  foreach var count in visits do
    Check(count = 1, 'Each row must be validated exactly once');
  for var f := 0 to k - 1 do
  begin
    var (trainIdx, testIdx) := folds[f];
    var (repeatTrain, repeatTest) := repeated[f];
    var (vectorTrain, vectorTest) := vectorFolds[f];
    Check(testIdx.SequenceEqual(repeatTest), 'Seed must reproduce folds');
    Check(trainIdx.SequenceEqual(vectorTrain) and testIdx.SequenceEqual(vectorTest),
      'Vector and integer overloads must agree');
  end;
  foreach var classValue in y.Distinct do
  begin
    var minCount := y.Length;
    var maxCount := 0;
    foreach var (trainIdx, testIdx) in folds do
    begin
      var count := testIdx.Count(i -> y[i] = classValue);
      minCount := Min(minCount, count);
      maxCount := Max(maxCount, count);
    end;
    Check(maxCount - minCount <= 1, 'Class distribution is not stratified');
  end;
end;

begin
  CheckFolds([0,0,0,0,0,1,1], 5);
  CheckFolds([0,0,0,0,0,1], 5);
  CheckFolds([0,0,0,0,0,0,0,1,1,1,2,2,2], 5);
  CheckFolds([0,0,0,0,0,1,1,1,1,1], 5);

  var raised := false;
  try
    var invalid := Validation.StratifiedKFold([0,0,0,1,1,1,1], 5, 42).ToArray;
  except
    on e: System.ArgumentException do raised := true;
  end;
  Check(raised, 'Must reject k greater than every class size');

  var y := [0,0,0,0,0,1,1];
  var X := new Matrix(y.Length, 1);
  for var i := 0 to y.Length - 1 do X[i,0] := i;
  var score := Validation.StratifiedCrossValidate(
    new DecisionTreeClassifier(seed := 42), X, y, 5, Metrics.Accuracy, seed := 42);
  Check((score >= 0) and (score <= 1), 'CV must accept a rare class');
  Println('OK');
end.
