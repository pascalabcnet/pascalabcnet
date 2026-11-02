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
  ER_IMAGE_WIDTH_GT_ZERO =
    'Image: ширина должна быть > 0!!Image: width must be > 0';
  ER_IMAGE_HEIGHT_GT_ZERO =
    'Image: высота должна быть > 0!!Image: height must be > 0';
  ER_IMAGE_VALUES_LENGTH =
    'Image: длина values должна быть {0}!!Image: values length must be {0}';
  ER_IMAGEGRID_STARTROW_NON_NEGATIVE =
    'ImageGrid: startRow должен быть >= 0!!ImageGrid: startRow must be >= 0';
  ER_IMAGEGRID_COUNT_GT_ZERO =
    'ImageGrid: count должен быть > 0!!ImageGrid: count must be > 0';
  ER_IMAGEGRID_COLS_GT_ZERO =
    'ImageGrid: cols должен быть > 0!!ImageGrid: cols must be > 0';
  ER_IMAGEGRID_SPACING_NON_NEGATIVE =
    'ImageGrid: spacing должен быть >= 0!!ImageGrid: spacing must be >= 0';
  ER_IMAGEGRID_ROW_RANGE =
    'ImageGrid: startRow + count должно быть <= X.RowCount!!' +
    'ImageGrid: startRow + count must be <= X.RowCount';
  ER_IMAGEGRID_COLCOUNT =
    'ImageGrid: число столбцов X должно быть равно width * height!!' +
    'ImageGrid: X.ColCount must be width * height';
  ER_IMAGE_VALUES_FINITE =
    '{0}: значения изображения должны быть конечными!!{0}: image values must be finite';
  ER_IMAGE_GRAYSCALE_ONLY =
    'Image: пока поддерживаются только grayscale-изображения!!' +
    'Image: only grayscale images are supported';
  ER_IMAGE_VALUE_GE_ZERO =
    'Image: значение должно быть >= 0!!Image: value must be >= 0';
  ER_IMAGE_VALUE_LE_255 =
    'Image: значение должно быть <= 255!!Image: value must be <= 255';
  ER_IMAGE_DATASET_FEATURE_COUNT =
    'Image: число ds.Features должно быть равно {0}, получено {1}!!' +
    'Image: ds.Features count must be {0}, got {1}';
  ER_IMAGE_DATASET_NUMERIC_FEATURE =
    'Image: признак "{0}" должен быть числовым!!' +
    'Image: feature "{0}" must be numeric';
  ER_IMAGE_DATASET_MISSING_VALUE =
    'Image: пропуск в строке {0}, признак "{1}"!!' +
    'Image: missing value at row {0}, feature "{1}"';
  ER_IMAGE_DATASET_ROW_RANGE =
    'Image: row должен быть в диапазоне 0..{0}!!' +
    'Image: row must be in range 0..{0}';
  ER_PLOT_POINTS_NUMERIC_COLUMN =
    'Points: столбец "{0}" должен быть числовым!!' +
    'Points: column "{0}" must be numeric';
  ER_PLOT_POINTS_CLASS_COLUMN =
    'Points: столбец классов "{0}" должен быть строковым или целочисленным!!' +
    'Points: class column "{0}" must be string or integer';
  ER_PLOT_POINTS_NO_VALID_ROWS =
    'Points: после исключения строк с пропусками не осталось точек!!' +
    'Points: no points remain after rows with missing values are excluded';
  ER_PLOT_POINTS_COLUMN_LENGTH_MISMATCH =
    'Points: длины столбцов должны совпадать!!' +
    'Points: column lengths must match';
  ER_PLOT_POINTS_TOOLTIPS_ROWCOUNT =
    'Points: число подсказок должно совпадать с числом исходных строк!!' +
    'Points: tooltip count must match source row count';
  ER_PLOT_HIST_NUMERIC_COLUMN =
    'Hist: столбец "{0}" должен быть числовым!!' +
    'Hist: column "{0}" must be numeric';
  ER_PLOT_HIST_NO_VALID_ROWS =
    'Hist: после исключения пропусков не осталось значений!!' +
    'Hist: no values remain after missing values are excluded';
  ER_PLOT_HIST_BINS_OR_EDGES =
    'Hist: задайте либо bins, либо edges, но не оба параметра одновременно!!' +
    'Hist: specify either bins or edges, not both';
  ER_PLOT_LINEGRAPH_NUMERIC_COLUMN =
    'LineGraph: столбец "{0}" должен быть числовым!!' +
    'LineGraph: column "{0}" must be numeric';
  ER_PLOT_LINEGRAPH_COLUMN_LENGTH_MISMATCH =
    'LineGraph: длины столбцов должны совпадать!!' +
    'LineGraph: column lengths must match';
  ER_PLOT_LINEGRAPH_NO_VALID_ROWS =
    'LineGraph: после исключения строк с пропусками не осталось точек!!' +
    'LineGraph: no points remain after rows with missing values are excluded';
  ER_PLOT_HORIZONTALBAR_LABEL_COLUMN =
    'HorizontalBar: столбец подписей "{0}" должен быть строковым или целочисленным!!' +
    'HorizontalBar: label column "{0}" must be string or integer';
  ER_PLOT_HORIZONTALBAR_NUMERIC_COLUMN =
    'HorizontalBar: столбец значений "{0}" должен быть числовым!!' +
    'HorizontalBar: value column "{0}" must be numeric';
  ER_PLOT_HORIZONTALBAR_COLUMN_LENGTH_MISMATCH =
    'HorizontalBar: длины столбцов должны совпадать!!' +
    'HorizontalBar: column lengths must match';
  ER_PLOT_HORIZONTALBAR_NO_VALID_ROWS =
    'HorizontalBar: после исключения строк с пропусками не осталось значений!!' +
    'HorizontalBar: no values remain after rows with missing values are excluded';
  ER_PLOT_BAR_NUMERIC_COLUMN =
    'Bar: столбец "{0}" должен быть числовым!!' +
    'Bar: column "{0}" must be numeric';
  ER_PLOT_BAR_COLUMN_LENGTH_MISMATCH =
    'Bar: длины столбцов должны совпадать!!' +
    'Bar: column lengths must match';
  ER_PLOT_BAR_NO_VALID_ROWS =
    'Bar: после исключения строк с пропусками не осталось значений!!' +
    'Bar: no values remain after rows with missing values are excluded';
  ER_PLOT_PAIRPLOT_COLUMNS_EMPTY =
    'PairPlot: список столбцов признаков не должен быть пустым!!' +
    'PairPlot: feature column list must not be empty';
  ER_PLOT_PAIRPLOT_NUMERIC_COLUMN =
    'PairPlot: столбец признака "{0}" должен быть числовым!!' +
    'PairPlot: feature column "{0}" must be numeric';
  ER_PLOT_PAIRPLOT_COLOR_COLUMN =
    'PairPlot: столбец окраски "{0}" должен быть числовым, строковым или логическим!!' +
    'PairPlot: color column "{0}" must be numeric, string, or boolean';
  ER_PLOT_PAIRPLOT_NO_VALID_ROWS =
    'PairPlot: после исключения строк с пропусками не осталось данных!!' +
    'PairPlot: no data remains after rows with missing values are excluded';
  ER_PLOT_HISTMANY_COLUMNS_EMPTY =
    'HistMany: список столбцов не должен быть пустым!!' +
    'HistMany: column list must not be empty';
  ER_PLOT_HISTMANY_NUMERIC_COLUMN =
    'HistMany: столбец "{0}" должен быть числовым!!' +
    'HistMany: column "{0}" must be numeric';
  ER_PLOT_HISTMANY_NO_VALID_VALUES =
    'HistMany: после исключения пропусков не осталось значений!!' +
    'HistMany: no values remain after missing values are excluded';
  ER_PLOT_HEATMAP_COLUMNS_EMPTY =
    'Heatmap: список столбцов не должен быть пустым!!' +
    'Heatmap: column list must not be empty';
  ER_PLOT_HEATMAP_NUMERIC_COLUMN =
    'Heatmap: столбец "{0}" должен быть числовым!!' +
    'Heatmap: column "{0}" must be numeric';
  ER_PLOT_HEATMAP_NO_VALID_ROWS =
    'Heatmap: после исключения строк с пропусками не осталось данных!!' +
    'Heatmap: no data remains after rows with missing values are excluded';
  ER_PLOT_DATASET_NO_NUMERIC_FEATURES =
    'Plot: в ds.Features нет числовых не категориальных признаков!!' +
    'Plot: ds.Features contains no numeric non-categorical features';
  ER_PLOT_PAIRPLOT_TWO_FEATURES =
    'PairPlot: требуется не менее двух числовых признаков!!' +
    'PairPlot: at least two numeric features are required';
  
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
