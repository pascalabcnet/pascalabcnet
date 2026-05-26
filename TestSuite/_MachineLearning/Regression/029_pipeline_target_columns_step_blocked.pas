uses MLABC, PreprocessorABC, MLCoreABC;
uses TestHelpers in '..\TestHelpers.pas';

type
  BadTargetColumnsStep = class(IPreprocessor, IColumnsBoundStep)
  private
    fColumns: array of string;
  public
    constructor Create(columns: array of string);
    function Fit(df: DataFrame): IPreprocessor;
    function Transform(df: DataFrame): DataFrame;
    function FitTransform(df: DataFrame): DataFrame;
    function Clone: IPreprocessor;
    property Columns: array of string read fColumns;
  end;

constructor BadTargetColumnsStep.Create(columns: array of string);
begin
  fColumns := Copy(columns);
end;

function BadTargetColumnsStep.Fit(df: DataFrame): IPreprocessor;
begin
  Result := Self;
end;

function BadTargetColumnsStep.Transform(df: DataFrame): DataFrame;
begin
  Result := df;
end;

function BadTargetColumnsStep.FitTransform(df: DataFrame): DataFrame;
begin
  Result := df;
end;

function BadTargetColumnsStep.Clone: IPreprocessor;
begin
  Result := new BadTargetColumnsStep(fColumns);
end;

begin
  CheckRaises(procedure -> begin
    var pipe := DataPipeline.Build(
      TaskKind.tkClassification,
      'Target',
      Arr($'X'),
      new BadTargetColumnsStep(Arr($'X', $'Target')),
      new LogisticRegression
    );
  end,
  'DataPipeline must reject a multi-column bound preprocessor that includes the target column');
end.
