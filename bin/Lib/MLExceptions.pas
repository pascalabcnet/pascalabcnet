unit MLExceptions;

interface

const
  ER_DIM_MISMATCH =
    'Размерности не совпадают: {0} и {1}!!Dimension mismatch: {0} and {1}';  
  ER_TO_VECTOR_NON_NUMERIC =
      'ToVector: столбец "{0}" содержит нечисловые или NA значения!!' +
      'ToVector: column "{0}" contains non-numeric or NA values';
  ER_LABELS_NOT_INTEGER =
      'Метки классов должны быть целыми числами!!Class labels must be integers';
  ER_PARAM_VALUES_EMPTY =
      'Список paramValues пуст!!paramValues is empty';
  ER_EMPTY_DATASET =
    'Набор данных пуст!!Dataset is empty';
  ER_LAMBDA_NEGATIVE =
    'Параметр lambda должен быть >= 0!!Lambda parameter must be >= 0';  
  ER_FIT_NOT_CALLED =
    'Метод Fit() не вызван!!Fit() must be called first';
  ER_MODEL_NULL =
    'Модель не может быть nil!!Model cannot be nil';
  ER_SELECTKBEST_K_INVALID =
    'Параметр k в SelectKBest должен быть >= 1. Передано: {0}.!!SelectKBest parameter k must be >= 1. Given: {0}.';  
  ER_ARG_NULL =
    'Аргумент {0} равен nil!!Argument {0} is null';
  ER_EMPTY_DATA =
    'Пустой набор данных для {0}!!Empty dataset for {0}';
  ER_COLUMN_NOT_FOUND =
    'В DataFrame отсутствует столбец "{0}"!!DataFrame does not contain column "{0}"';  
  ER_NOT_IMPLEMENTED = 
    'Метод {0} не реализован!!Method {0} is not implemented';
  ER_X_NULL =
    'X не может быть nil!!X cannot be nil';
  ER_Y_NULL =
    'y не может быть nil!!y cannot be nil';
  ER_XY_SIZE_MISMATCH =
    'Размеры X и y не совпадают: X={0}, y={1}!!X and y size mismatch: X={0}, y={1}';
  ER_FEATURE_COUNT_MISMATCH =
    'Число признаков не совпадает: {0} и {1}!!Feature count mismatch: {0}, {1}';
  ER_NAN_IN_X =  
    'X содержит NaN (пропуски)!!X contains NaN';  
  ER_NAN_IN_Y =
    'y содержит NaN (пропуски)!!y contains NaN';  
  ER_TRAINING_DATA_CONTAINS_NAN =
    'Обучающие данные содержат NaN. Обработайте пропуски до вызова Fit.!!' +
    'Training data contains NaN. Please handle missing values before calling Fit.';
  ER_TRAINING_DATA_CONTAINS_INF =
    'Обучающие данные содержат Infinity. Проверьте данные на выбросы.!!' +
    'Training data contains Infinity. Please check for extreme values.';
  ER_PREDICTION_DATA_CONTAINS_NAN =
    'Данные для предсказания содержат NaN. Обработайте пропуски до вызова Predict.!!' +
    'Prediction data contains NaN. Please handle missing values before calling Predict.';
  ER_PREDICTION_DATA_CONTAINS_INF =
    'Данные для предсказания содержат Infinity. Проверьте данные на выбросы.!!' +
    'Prediction data contains Infinity. Please check for extreme values.';  
  ER_MODEL_NOT_FITTED =
    'Модель "{0}" не обучена. Сначала вызовите Fit()!!Model "{0}" is not fitted. Call Fit() first';
  ER_TEST_RATIO_INVALID =
    'Параметр testRatio должен быть в интервале (0,1), получено {0}!!' +
    'Parameter testRatio must be in (0,1), got {0}';
  ER_PREDICT_NOT_SUPPORTED =
    'Модель не поддерживает операцию Predict!!' +
    'Model does not support Predict operation';
  
type
  /// Базовое исключение ML-библиотеки
  MLException = class(Exception);

  /// Попытка использования необученной модели/пайплайна
  MLNotFittedException = class(MLException);

  /// Несоответствие размерностей
  MLDimensionException = class(MLException);
  
  MLNotImplementedException = class(MLException);

procedure Error(msg: string; params args: array of object);
procedure ArgumentError(msg: string; params args: array of object);
procedure NotFittedError(msg: string; params args: array of object);
procedure DimensionError(msg: string; params args: array of object);
procedure ArgumentNullError(msg: string; params args: array of object);
procedure ArgumentOutOfRangeError(msg: string; params args: array of object);
procedure NotImplementedError(msg: string; params args: array of object);

implementation

function FormatSafe(msg: string; args: array of object): string;
begin
  var text := GetTranslation(msg);
  try
    Result := Format(text, args);
  except
    Result := text; // если не совпали {0},{1} — оставляем как есть
  end;
end;

procedure Error(msg: string; params args: array of object);
begin
  raise new MLException(FormatSafe(msg, args));
end;

procedure ArgumentError(msg: string; params args: array of object);
begin
  raise new System.ArgumentException(FormatSafe(msg, args));
end;

procedure NotFittedError(msg: string; params args: array of object);
begin
  raise new MLNotFittedException(FormatSafe(msg, args));
end;

procedure DimensionError(msg: string; params args: array of object);
begin
  raise new MLDimensionException(FormatSafe(msg, args));
end;

procedure ArgumentNullError(msg: string; params args: array of object);
begin
  raise new System.ArgumentNullException(FormatSafe(msg, args));
end;

procedure ArgumentOutOfRangeError(msg: string; params args: array of object);
begin
  raise new System.ArgumentOutOfRangeException(FormatSafe(msg, args));
end;

procedure NotImplementedError(msg: string; params args: array of object);
begin
  raise new MLNotImplementedException(FormatSafe(msg, args));
end;
  

end.
