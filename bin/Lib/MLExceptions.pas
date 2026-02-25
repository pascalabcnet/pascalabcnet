unit MLExceptions;

interface

const
  ER_DIM_MISMATCH =
  'Несоответствие размерностей: {0} и {1}!!Dimension mismatch: {0} and {1}';  
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

type
  /// Базовое исключение ML-библиотеки
  MLException = class(Exception);

  /// Попытка использования необученной модели/пайплайна
  MLNotFittedException = class(MLException);

  /// Несоответствие размерностей
  MLDimensionException = class(MLException);

procedure Error(msg: string; params args: array of object);
procedure ArgumentError(msg: string; params args: array of object);
procedure NotFittedError(msg: string; params args: array of object);
procedure DimensionError(msg: string; params args: array of object);
procedure ArgumentNullError(msg: string; params args: array of object);
procedure ArgumentOutOfRangeError(msg: string; params args: array of object);

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

end.
