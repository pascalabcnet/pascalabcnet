/// Метрики качества моделей машинного обучения.
///
/// Содержит метрики для:
/// • регрессии (MSE, RMSE, MAE, R2)
/// • классификации (Accuracy, Precision, Recall, F1 и др.)
/// • вероятностных моделей (LogLoss, ROC, AUC)
/// • кластеризации (Silhouette, Calinski–Harabasz, Davies–Bouldin, ARI)
///
/// Также включает класс ConfusionMatrix для анализа результатов классификации.
unit MetricsABC;

interface

uses LinearAlgebraML;

type
  Averaging = (avMacro, avMicro, avWeighted);
  
  /// Метрики оценки качества моделей машинного обучения
  /// для задач регрессии, классификации и кластеризации
  Metrics = static class
  public
    /// Среднеквадратичная ошибка (MSE).
    /// MSE = (1/n) * Σ (yTrue - yPred)^2.
    /// Чувствительна к большим отклонениям.
    static function MSE(yTrue, yPred: Vector): real;
    
    /// Вычисляет среднеквадратичную ошибку (Root Mean Squared Error, RMSE)
    /// между истинными и предсказанными значениями.
    /// RMSE равна квадратному корню из MSE и измеряется
    /// в тех же единицах, что и целевая переменная.
    static function RMSE(yTrue, yPred: Vector): real;
    
    /// Средняя абсолютная ошибка (MAE).
    /// MAE = (1/n) * Σ |yTrue - yPred|.
    /// Менее чувствительна к выбросам, чем MSE.
    static function MAE(yTrue, yPred: Vector): real;
    
    /// Средняя абсолютная процентная ошибка (MAPE).
    /// MAPE = (1/n) * Σ |(yTrue - yPred) / yTrue|.
    /// Значения yTrue = 0 игнорируются.
    /// Удобна для интерпретации в процентах.
    static function MAPE(yTrue, yPred: Vector): real;
    
    /// Медианная абсолютная ошибка (MedianAE).
    /// MedianAE = median(|yTrue - yPred|).
    /// Устойчива к выбросам.
    static function MedianAE(yTrue, yPred: Vector): real;
    
    /// Коэффициент детерминации (R²).
    /// R2 = 1 - SS_res / SS_tot.
    /// Показывает долю объяснённой дисперсии.
    /// R2 = 1 — идеальная модель,
    /// R2 = 0 — не лучше среднего,
    /// R2 < 0 — хуже константной модели.
    ///
    /// Если дисперсия целевой переменной равна нулю (все значения одинаковы),
    /// метрика не определена. В этом случае возвращается 0.0 по соглашению.
    static function R2(yTrue, yPred: Vector): real;
    
// ---- Вероятностные метрики классификации ----

    /// Логарифмическая функция потерь (LogLoss).
    /// Оценивает качество вероятностных предсказаний.
    /// Сильно штрафует уверенные, но ошибочные прогнозы.
    /// Используется для моделей, возвращающих вероятности
    /// (например, LogisticRegression).
    static function LogLoss(yTrue, yPred: Vector): real;
  
    // ---- ROC / AUC ----
  
    /// Строит точки ROC-кривой по вероятностным предсказаниям.
    /// Возвращает кортеж из двух векторов:
    /// FPR (доля ложных положительных)
    /// и TPR (доля истинно положительных).
    static function ROC(yTrue, yPred: Vector): (Vector, Vector);
  
    /// Площадь под ROC-кривой (AUC).
    /// Значение от 0 до 1.
    /// 0.5 — случайная модель,
    /// 1 — идеальная модель.
    static function AUC(yTrue, yPred: Vector): real;
    
    /// Доля правильных предсказаний.
    /// Показывает, какая часть объектов классифицирована верно.
    /// Удобна, когда классы примерно сбалансированы.
    /// Пример: Accuracy = 0.9 означает 90% правильных ответов.
    static function Accuracy(yTrue, yPred: Vector): real;
    
    /// Доля правильных предсказаний.
    /// Показывает, какая часть объектов классифицирована верно.
    /// Удобна, когда классы примерно сбалансированы.
    /// Пример: Accuracy = 0.9 означает 90% правильных ответов.
    static function Accuracy(yTrue, yPred: array of integer): real;
    
    /// Доля правильных предсказаний.
    /// Показывает, какая часть объектов классифицирована верно.
    /// Удобна, когда классы примерно сбалансированы.
    /// Пример: Accuracy = 0.9 означает 90% правильных ответов.
    static function Accuracy(yTrue: Vector; yPred: array of integer): real;

    /// Точность (Precision) для положительного класса (1).
    /// Precision = TP / (TP + FP)
    /// Среди всех объектов, которые модель предсказала как положительные (1),
    /// показывает долю действительно принадлежащих этому классу.
    /// Используется только для бинарной классификации.
    /// Важна, когда критичны ложные положительные результаты («ложные тревоги»).
    static function Precision(yTrue, yPred: Vector): real;
    
    /// Точность (Precision) с усреднением по классам.
    /// Вычисляется для каждого класса отдельно как:
    /// Precision(c) = TP(c) / (TP(c) + FP(c)),
    /// а затем результаты объединяются способом, заданным параметром avg:
    ///   avMacro    — среднее по всем классам (каждый класс равновесен);
    ///   avMicro    — глобальная метрика по суммарным TP и FP;
    ///   avWeighted — среднее с весами, пропорциональными размеру классов.
    /// Особенно полезна для многоклассовой классификации.
    static function Precision(yTrue, yPred: Vector; avg: Averaging): real;
    
    /// Полнота (Recall) для положительного класса (1).
    /// Recall = TP / (TP + FN)
    /// Среди всех объектов, которые в действительности принадлежат положительному классу (1),
    /// показывает долю правильно найденных моделью.
    /// Используется только для бинарной классификации.
    /// Важна, когда нежелательны пропуски положительных случаев (например, заболеваний).
    static function Recall(yTrue, yPred: Vector): real;
    
    /// Полнота (Recall) с усреднением по классам.
    /// Вычисляется для каждого класса отдельно как:
    /// Recall(c) = TP(c) / (TP(c) + FN(c)),
    /// а затем результаты объединяются способом, заданным параметром avg:
    ///   avMacro    — среднее по всем классам (каждый класс равновесен);
    ///   avMicro    — глобальная метрика по суммарным TP и FN;
    ///   avWeighted — среднее с весами, пропорциональными размеру классов.
    /// Особенно полезна для многоклассовой классификации.
    static function Recall(yTrue, yPred: Vector; avg: Averaging): real;
    
    /// F1-мера для бинарной классификации.
    /// F1 = 2 * (Precision * Recall) / (Precision + Recall)
    /// Гармоническое среднее точности (Precision) и полноты (Recall).
    /// Принимает высокие значения только тогда, когда оба показателя высоки.
    /// Используется только для бинарной классификации.
    /// Особенно полезна при несбалансированных классах.
    static function F1(yTrue, yPred: Vector): real;
    
    /// F1-мера с усреднением по классам.
    /// Вычисляется для каждого класса отдельно как:
    /// F1(c) = 2 * Precision(c) * Recall(c) / (Precision(c) + Recall(c)),
    /// а затем результаты объединяются способом, заданным параметром avg:
    ///   avMacro    — среднее по всем классам (каждый класс равновесен);
    ///   avMicro    — вычисляется по суммарным TP, FP и FN;
    ///   avWeighted — среднее с весами, пропорциональными размеру классов.
    /// Особенно полезна для многоклассовой классификации.
    static function F1(yTrue, yPred: Vector; avg: Averaging): real;

    /// Специфичность (Specificity, True Negative Rate) для бинарной классификации.
    /// Specificity = TN / (TN + FP)
    /// Доля объектов отрицательного класса (0), правильно распознанных моделью.
    /// Показывает способность модели корректно распознавать отрицательный класс.
    /// Используется только для бинарной классификации.
    static function Specificity(yTrue, yPred: Vector): real;
    
    /// Сбалансированная точность (Balanced Accuracy) для бинарной классификации.
    /// Balanced Accuracy = (Recall + Specificity) / 2
    /// Среднее арифметическое полноты (Recall, чувствительности)
    /// и специфичности (Specificity, доли истинно отрицательных).
    /// Устойчива к дисбалансу классов.
    /// Используется только для бинарной классификации
    static function BalancedAccuracy(yTrue, yPred: Vector): real;
    
    static function ClassificationReport(yTrue, yPred: Vector): string;
    
    /// Вычисляет силуэт (Silhouette) для каждой точки.
    /// Для каждой строки матрицы X возвращает значение s(i) в диапазоне [-1, 1].
    /// Требует как минимум 2 различных кластера.
    /// Использует евклидово расстояние (квадратный корень не извлекается).
    /// Сложность O(n²) - для умеренных размеров данных.
    static function SilhouetteSamples(X: Matrix; labels: Vector): Vector;
  
    /// Вычисляет средний силуэт (Silhouette Score).
    /// Возвращает среднее значение Silhouette по всем точкам.
    /// Чем ближе к 1 — тем лучше разделены кластеры.
    /// Значение около 0 означает пересечение кластеров.
    /// Отрицательное значение указывает на возможную ошибку кластеризации
    static function SilhouetteScore(X: Matrix; labels: Vector): real;
    
    /// Вычисляет индекс Калински–Харабаса (Calinski–Harabasz Index).
    /// Возвращает отношение межкластерной дисперсии к внутрикластерной дисперсии.
    /// Чем больше значение — тем лучше разделены кластеры.
    /// Требует как минимум 2 кластера и n > k.
    /// Сложность O(n·p).
    static function CalinskiHarabaszScore(X: Matrix; labels: Vector): real;
    
    /// Вычисляет индекс Дэвиса–Булдина (Davies–Bouldin Index).
    /// Чем меньше значение — тем лучше разделены кластеры.
    /// Требует минимум 2 кластера.
    /// Сложность O(n·p + k²·p).
    static function DaviesBouldinScore(X: Matrix; labels: Vector): real;
    
    /// Adjusted Rand Index (ARI).
    /// Внешняя метрика качества кластеризации.
    /// Сравнивает предсказанные кластеры с истинными метками.
    ///
    /// Основана на анализе всех пар объектов:
    ///   проверяет, совпадает ли решение алгоритма и истинная разметка
    ///   в вопросе "находятся ли две точки в одном кластере".
    ///
    /// Корректирует обычный Rand Index на случайные совпадения.
    ///
    /// Диапазон значений:
    /// 1   — идеальное совпадение кластеризации и истинных меток
    /// 0   — случайное разбиение
    /// <0  — хуже случайного
    static function AdjustedRandIndex(yTrue, yPred: Vector): real;
  end;
  
type  
/// Матрица ошибок (Confusion Matrix) для бинарной и многоклассовой классификации.
///
/// Показывает, сколько объектов каждого класса
/// модель распознала правильно и сколько перепутала.
///
/// На вход можно передавать любые целочисленные метки классов
/// (например, 0, 1, 2 или 2, 5, 8).
///
/// При построении матрицы все метки автоматически нумеруются
/// числами от 0 до K-1, где K — число различных классов.
///
/// Элемент матрицы [i, j] показывает, сколько объектов 
/// класса i модель предсказала как класс j.
///
/// Свойства Labels, GetLabel и GetIndex позволяют узнать,
/// как новые внутренние номера классов связаны с исходными метками.
///
/// Для бинарной классификации дополнительно доступны
/// показатели TP, TN, FP, FN, Precision, Recall, F1, Specificity и BalancedAccuracy
  ConfusionMatrix = class
  private
    fMatrix: array[,] of integer;
    fClassCount: integer;
    
    fLabels: array of integer;
    fLabelToIndex: Dictionary<integer, integer>;

    procedure EnsureBinary;

    function GetMatrix: array[,] of integer;

    function GetTPBinary: integer;
    function GetFPBinary: integer;
    function GetFNBinary: integer;
    function GetTNBinary: integer;
    
    procedure EnsureClassIndex(c: integer);
  public
    /// Создаёт матрицу ошибок по истинным меткам yTrue и предсказаниям модели yPred.
    ///
    /// Векторы должны иметь одинаковую длину и содержать целочисленные метки классов.
    ///
    /// Можно использовать любые целые метки классов. При построении матрицы они
    /// автоматически перенумеровываются числами от 0 до K-1, где K — число
    /// различных классов.
    ///
    /// Элемент [i, j] показывает, сколько объектов класса i модель предсказала
    /// как класс j.
    ///
    /// Для бинарной классификации дополнительно доступны значения TP, TN, FP и FN
    constructor Create(yTrue, yPred: Vector);
        
    /// Возвращает исходную метку класса по его внутреннему номеру.
    ///
    /// Внутренние номера классов используются внутри матрицы ошибок
    /// и находятся в диапазоне от 0 до ClassCount-1.
    ///
    /// Метод позволяет узнать, какой исходной метке соответствует
    /// данный внутренний номер.
    function GetLabel(c: integer): integer;
    
    /// Возвращает внутренний номер класса по его исходной метке.
    ///
    /// Результат находится в диапазоне от 0 до ClassCount-1
    /// и используется для обращения к элементам матрицы ошибок.
    ///
    /// Если такой метки нет, выбрасывается исключение.
    function GetIndex(labl: integer): integer;
    
    /// Исходные метки классов в порядке внутренних номеров.
    ///
    /// Labels[c] — это исходная метка класса,
    /// соответствующая внутреннему номеру c.
    property Labels: array of integer read fLabels;

    /// Число различных классов, встретившихся в yTrue или yPred.
    ///
    /// Используется для построения матрицы ошибок размера K × K, где K = ClassCount.
    property ClassCount: integer read fClassCount;  

    /// Матрица ошибок.
    ///
    /// Matrix[i, j] — количество объектов класса i, которые модель предсказала как класс j.
    property Matrix: array[,] of integer read GetMatrix;
    
    /// Число истинно положительных предсказаний для класса c.
    /// Здесь c — номер класса в матрице ошибок.
    /// Соответствует элементу матрицы [c, c].
    function TPForClass(c: integer): integer;
    
    /// Число ложно положительных предсказаний для класса c.
    /// Здесь c — номер класса в матрице ошибок.
    /// FP(c) — количество объектов, предсказанных как класс c,
    /// но в действительности принадлежащих другим классам.
    /// Вычисляется как сумма по столбцу c минус TP(c).
    function FPForClass(c: integer): integer;
    
    /// Число ложно отрицательных предсказаний для класса c.
    /// Здесь c — номер класса в матрице ошибок.
    /// FN(c) — количество объектов, принадлежащих классу c,
    /// но предсказанных как другие классы.
    /// Вычисляется как сумма по строке c минус TP(c).
    function FNForClass(c: integer): integer;
    
    /// Число истинно положительных предсказаний (TP) для бинарной классификации.
    /// Показывает, сколько объектов положительного класса
    /// модель правильно распознала как положительные.
    /// Доступно только при числе классов, равном 2.
    property TP: integer read GetTPBinary;
    
    /// Число ложно положительных предсказаний (FP) для бинарной классификации.
    /// Показывает, сколько объектов отрицательного класса
    /// модель ошибочно отнесла к положительному классу.
    /// Доступно только при числе классов, равном 2.
    property FP: integer read GetFPBinary;
    
    /// Число ложно отрицательных предсказаний (FN) для бинарной классификации.
    /// Показывает, сколько объектов положительного класса
    /// модель ошибочно отнесла к отрицательному классу.
    /// Доступно только при числе классов, равном 2.
    property FN: integer read GetFNBinary;
    
    /// Число истинно отрицательных предсказаний (TN) для бинарной классификации.
    /// Показывает, сколько объектов отрицательного класса
    /// модель правильно распознала как отрицательные.
    /// Доступно только при числе классов, равном 2.
    property TN: integer read GetTNBinary;
    
    /// Доля правильных предсказаний (Accuracy).
    /// Accuracy = (число правильных предсказаний) / (общее число объектов).
    /// Показывает, какая часть объектов классифицирована верно.
    /// Применима как для бинарной, так и для многоклассовой классификации.
    /// Удобна, когда классы примерно сбалансированы.
    function Accuracy: real;
    
    /// Точность (Precision) для положительного класса (1).
    /// Precision = TP / (TP + FP)
    /// Среди всех объектов, которые модель предсказала как положительные (1),
    /// показывает долю действительно принадлежащих этому классу.
    /// Используется только для бинарной классификации.
    /// Важна, когда критичны ложные положительные результаты («ложные тревоги»).
    function Precision: real;
    
    /// Полнота (Recall) для положительного класса (1).
    /// Recall = TP / (TP + FN)
    /// Среди всех объектов, которые в действительности принадлежат классу 1,
    /// показывает долю правильно найденных моделью.
    /// Используется только для бинарной классификации.
    /// Важна, когда нежелательны пропуски положительных случаев (например, заболеваний).
    function Recall: real;
    
    /// F1-мера для бинарной классификации.
    /// F1 = 2 * (Precision * Recall) / (Precision + Recall)
    /// Гармоническое среднее точности и полноты.
    /// Принимает высокие значения только тогда, когда оба показателя высоки.
    /// Используется только для бинарной классификации.
    /// Особенно полезна при несбалансированных классах.
    function F1: real;
    
    /// Специфичность (Specificity, True Negative Rate) для бинарной классификации.
    /// Specificity = TN / (TN + FP)
    /// Доля объектов отрицательного класса (0), правильно распознанных моделью.
    /// Показывает способность модели корректно распознавать отрицательный класс.
    /// Используется только для бинарной классификации.
    function Specificity: real;
    
    /// Сбалансированная точность (Balanced Accuracy) для бинарной классификации.
    /// Balanced Accuracy = (Recall + Specificity) / 2
    /// Среднее арифметическое полноты (Recall, чувствительности)
    /// и специфичности (Specificity).
    /// Устойчива к дисбалансу классов.
    /// Используется только для бинарной классификации.
    function BalancedAccuracy: real;
    
    /// Точность (Precision) для класса c.
    /// Precision(c) = TP(c) / (TP(c) + FP(c))
    /// Среди всех объектов, предсказанных как класс c,
    /// показывает долю действительно принадлежащих этому классу.
    /// Если модель ни разу не предсказала класс c (TP(c) + FP(c) = 0),
    /// значение Precision(c) считается равным 0.
    /// Применима для многоклассовой классификации.
    function PrecisionForClass(c: integer): real;
    
    /// Полнота (Recall) для класса c.
    /// Recall(c) = TP(c) / (TP(c) + FN(c))
    /// Среди всех объектов, принадлежащих классу c,
    /// показывает долю правильно найденных моделью.
    /// Применима для многоклассовой классификации.
    function RecallForClass(c: integer): real;
    
    /// F1-мера для класса c.
    /// F1(c) = 2 * Precision(c) * Recall(c) / (Precision(c) + Recall(c))
    /// Гармоническое среднее точности и полноты для класса c.
    /// Принимает высокие значения только тогда, когда оба показателя высоки.
    /// Применима для многоклассовой классификации.
    function F1ForClass(c: integer): real;

  end;
  
  /// Метрики для оценки качества классификационных моделей.
  /// Используются, когда целевая переменная представляет собой категорию (класс)
  /// (например, определение вида цветка, спам/не спам, диагноз пациента).
  ClassificationMetrics = static class
  public
    /// Доля правильных предсказаний (Accuracy).
    /// Accuracy = (число правильных предсказаний) / (общее число объектов).
    /// Показывает долю верно классифицированных объектов.
    /// Применима как для бинарной, так и для многоклассовой классификации.
    /// Удобна, когда классы примерно сбалансированы.
    static function Accuracy(yTrue, yPred: Vector): real := Metrics.Accuracy(yTrue, yPred);
    
    /// Доля правильных предсказаний (Accuracy).
    /// Accuracy = (число правильных предсказаний) / (общее число объектов).
    /// Показывает долю верно классифицированных объектов.
    /// Применима как для бинарной, так и для многоклассовой классификации.
    /// Удобна, когда классы примерно сбалансированы.
    static function Accuracy(yTrue, yPred: array of integer): real := Metrics.Accuracy(yTrue, yPred);
    
    /// Доля правильных предсказаний (Accuracy).
    /// Accuracy = (число правильных предсказаний) / (общее число объектов).
    /// Показывает долю верно классифицированных объектов.
    /// Применима как для бинарной, так и для многоклассовой классификации.
    /// Удобна, когда классы примерно сбалансированы.
    static function Accuracy(yTrue: Vector; yPred: array of integer): real := Metrics.Accuracy(yTrue, yPred);
    
    /// Точность (Precision) для положительного класса (1).
    /// Precision = TP / (TP + FP)
    /// Среди всех объектов, предсказанных как положительные (1),
    /// показывает долю действительно принадлежащих этому классу.
    /// Используется только для бинарной классификации.
    /// Важна, когда критичны ложные положительные результаты («ложные тревоги»).
    static function Precision(yTrue, yPred: Vector): real := Metrics.Precision(yTrue, yPred);
    
    /// Полнота (Recall) для положительного класса (1).
    /// Recall = TP / (TP + FN)
    /// Среди всех объектов, принадлежащих положительному классу (1),
    /// показывает долю правильно найденных моделью.
    /// Используется только для бинарной классификации.
    /// Важна, когда нежелательны пропуски положительных случаев (например, заболеваний).
    static function Recall(yTrue, yPred: Vector): real := Metrics.Recall(yTrue, yPred);
    
    /// F1-мера для бинарной классификации.
    /// F1 = 2 * (Precision * Recall) / (Precision + Recall)
    /// Гармоническое среднее точности и полноты.
    /// Принимает высокие значения только тогда, когда оба показателя высоки.
    /// Особенно полезна при несбалансированных классах.
    /// Используется только для бинарной классификации.
    static function F1(yTrue, yPred: Vector): real := Metrics.F1(yTrue, yPred);
    
    /// Специфичность (Specificity, True Negative Rate) для бинарной классификации.
    /// Specificity = TN / (TN + FP)
    /// Доля объектов отрицательного класса (0), правильно распознанных моделью.
    /// Показывает способность модели корректно распознавать отрицательный класс.
    /// Используется только для бинарной классификации.
    static function Specificity(yTrue, yPred: Vector): real := Metrics.Specificity(yTrue, yPred);
    
    /// Сбалансированная точность (Balanced Accuracy) для бинарной классификации.
    /// Balanced Accuracy = (Recall + Specificity) / 2
    /// Среднее арифметическое полноты (Recall, чувствительности)
    /// и специфичности (Specificity).
    /// Устойчива к дисбалансу классов.
    /// Используется только для бинарной классификации.
    static function BalancedAccuracy(yTrue, yPred: Vector): real := Metrics.BalancedAccuracy(yTrue, yPred);
    
    /// Логарифмическая функция потерь (LogLoss) для бинарной классификации.
    /// LogLoss = - (1/n) * Σ [yTrue * log(yProb) + (1 - yTrue) * log(1 - yProb)]
    /// Оценивает качество вероятностных предсказаний для класса 1.
    /// Сильно штрафует уверенные, но ошибочные прогнозы.
    /// Чем меньше значение, тем лучше модель.
    /// Используется только для бинарной классификации.
    static function LogLoss(yTrue, yPred: Vector): real := Metrics.LogLoss(yTrue, yPred);
    
    /// Построение ROC-кривой для бинарной классификации.
    /// Возвращает кортеж из двух векторов:
    ///   FPR (False Positive Rate, доля ложноположительных),
    ///   TPR (True Positive Rate, полнота / Recall).
    /// Вектор yPred должен содержать вероятности принадлежности к классу 1.
    /// Используется для анализа качества вероятностных предсказаний.
    /// Используется только для бинарной классификации.
    static function ROC(yTrue, yPred: Vector): (Vector, Vector) := Metrics.ROC(yTrue, yPred); 
    
    /// Площадь под ROC-кривой (AUC) для бинарной классификации.
    /// Значение от 0 до 1:
    ///   1.0 — идеальная модель,
    ///   0.5 — случайная модель,
    ///   <0.5 — модель хуже случайной.
    /// Вектор yPred должен содержать вероятности принадлежности к классу 1.
    /// Используется только для бинарной классификации.
    static function AUC(yTrue, yPred: Vector): real := Metrics.AUC(yTrue, yPred);
    
    /// Формирует текстовый отчёт по качеству классификации.
    /// Для каждого класса выводит Precision, Recall, F1 и число объектов этого класса.
    /// Также выводит усреднённые показатели по всем классам.
    static function ClassificationReport(yTrue, yPred: Vector): string := Metrics.ClassificationReport(yTrue, yPred);
  end;  
  
  /// Метрики для оценки качества регрессионных моделей
  /// Используются когда целевая переменная является непрерывным числом
  /// (например, предсказание цены дома, температуры, возраста)
  RegressionMetrics = static class
  public
    /// Среднеквадратичная ошибка (MSE)
    /// MSE = (1/n) * Σ (yTrue - yPred)^2
    /// Чувствительна к большим отклонениям и выбросам
    /// Чем меньше значение, тем лучше модель
    static function MSE(yTrue, yPred: Vector): real := Metrics.MSE(yTrue, yPred);  
    
    /// Корень из среднеквадратичной ошибки (RMSE)
    /// RMSE = √MSE
    /// Измеряется в тех же единицах, что и целевая переменная
    /// Чем меньше значение, тем лучше модель
    static function RMSE(yTrue, yPred: Vector): real := Metrics.RMSE(yTrue, yPred);  
    
    /// Средняя абсолютная ошибка (MAE)
    /// MAE = (1/n) * Σ |yTrue - yPred|
    /// Менее чувствительна к выбросам, чем MSE
    /// Чем меньше значение, тем лучше модель
    static function MAE(yTrue, yPred: Vector): real := Metrics.MAE(yTrue, yPred);   
    
    /// Средняя абсолютная процентная ошибка (MAPE).
    /// MAPE = (1/n) * Σ |(yTrue - yPred) / yTrue|.
    /// Значения yTrue = 0 игнорируются.
    /// Удобна для интерпретации в процентах.
    static function MAPE(yTrue, yPred: Vector): real := Metrics.MAPE(yTrue, yPred);
    
    /// Медианная абсолютная ошибка (MedianAE).
    /// MedianAE = median(|yTrue - yPred|).
    /// Устойчива к выбросам.
    static function MedianAE(yTrue, yPred: Vector): real := Metrics.MedianAE(yTrue, yPred);   
    
    /// Коэффициент детерминации (R²)
    /// R² = 1 - SS_res / SS_tot
    /// Показывает долю дисперсии, объяснённую моделью
    /// Значения:
    ///   1.0 — идеальная модель
    ///   0.0 — модель не лучше среднего
    ///   <0  — модель хуже простого среднего
    ///
    /// Если дисперсия целевой переменной равна нулю (все значения одинаковы),
    /// метрика не определена. В этом случае возвращается 0.0 по соглашению.
    static function R2(yTrue, yPred: Vector): real := Metrics.R2(yTrue, yPred);
  end;
  
  /// Метрики для оценки качества кластеризации
  /// Используются когда нужно оценить, насколько хорошо объекты сгруппированы в кластеры
  /// Делятся на:
  ///   - внутренние (без эталонных меток) — оценивают компактность и разделимость
  ///   - внешние (с эталонными метками) — сравнивают с правильным разбиением
  ClusteringMetrics = static class
  public
    // ========== ВНУТРЕННИЕ МЕТРИКИ (без эталонных меток) ==========
    
    /// Вычисляет силуэт (Silhouette) для каждой точки
    /// Для каждой строки матрицы X возвращает значение s(i) в диапазоне [-1, 1]
    ///   s(i) ≈ 1  — точка хорошо сгруппирована в своём кластере
    ///   s(i) ≈ 0  — точка находится на границе между кластерами
    ///   s(i) < 0  — точка, вероятно, попала не в свой кластер
    /// Требует как минимум 2 различных кластера
    /// Сложность O(n²) — для умеренных размеров данных
    static function SilhouetteSamples(X: Matrix; labels: Vector): Vector := Metrics.SilhouetteSamples(x, labels);  
    
    /// Вычисляет средний силуэт (Silhouette Score)
    /// Возвращает среднее значение Silhouette по всем точкам
    /// Чем ближе к 1 — тем лучше разделены кластеры
    /// Значение около 0 означает пересечение кластеров
    /// Отрицательное значение указывает на возможную ошибку кластеризации
    static function SilhouetteScore(X: Matrix; labels: Vector): real := Metrics.SilhouetteScore(x, labels);  
    
    /// Индекс Калински–Харабаса (Calinski–Harabasz Index)
    /// Возвращает отношение межкластерной дисперсии к внутрикластерной дисперсии
    /// Чем больше значение — тем лучше разделены кластеры
    /// Требует как минимум 2 кластера и n > k
    /// Сложность O(n·p)
    static function CalinskiHarabaszScore(X: Matrix; labels: Vector): real := Metrics.CalinskiHarabaszScore(x, labels);
    
    /// Индекс Дэвиса–Булдина (Davies–Bouldin Index)
    /// Чем меньше значение — тем лучше разделены кластеры
    /// Минимальное значение 0 (идеальное разделение)
    /// Требует минимум 2 кластера
    /// Сложность O(n·p + k²·p)
    static function DaviesBouldinScore(X: Matrix; labels: Vector): real := Metrics.DaviesBouldinScore(x, labels);
    
    // ========== ВНЕШНИЕ МЕТРИКИ (с эталонными метками) ==========
    
    /// Скорректированный индекс Рэнда (Adjusted Rand Index, ARI)
    /// Сравнивает предсказанные кластеры с истинными метками
    /// Основан на анализе всех пар объектов:
    ///   проверяет, совпадает ли решение алгоритма и истинная разметка
    ///   в вопросе "находятся ли две точки в одном кластере"
    /// Значения:
    ///   1.0 — идеальное совпадение кластеризации и истинных меток
    ///   0.0 — случайное разбиение
    ///   <0  — хуже случайного
    static function AdjustedRandIndex(yTrue, yPred: Vector): real := Metrics.AdjustedRandIndex(yTrue, yPred);
  end;

implementation  

uses MLExceptions;
uses MLUtilsABC;

const
  ER_INVALID_VALUE =
    'Некорректное значение {0} на позиции {1}!!Invalid value in {0} at index {1}';
  ER_DIV_BY_ZERO =
    'Деление на ноль в {0}!!Division by zero in {0}';
  ER_INVALID_CLASS_LABEL =
    'Некорректная метка класса: {0}!!Invalid class label: {0}';
  ER_STRATIFIED_LABELS_INVALID =
    'Метки кластеров должны быть целыми числами!!Cluster labels must be integers';
  ER_SILHOUETTE_NEEDS_2_CLUSTERS =
    'Для вычисления Silhouette требуется минимум 2 кластера!!Silhouette requires at least 2 clusters';
  ER_INVALID_VALUE_AT =
    'Некорректное значение в {0} на позиции {1}!!Invalid value in {0} at index {1}';
  ER_ARG_NAN_INF =
    'Аргумент {0} содержит NaN или Infinity!!Argument {0} contains NaN or Infinity';
  ER_CH_NEEDS_2_CLUSTERS =
    'Для Calinski–Harabasz требуется минимум 2 кластера!!Calinski–Harabasz requires at least 2 clusters';
  ER_CH_NEEDS_N_GT_K =
    'Для Calinski–Harabasz требуется n > k!!Calinski–Harabasz requires n > k';
  ER_LABELS_MUST_BE_INTEGERS =
    'Метки кластеров должны быть целыми числами!!Cluster labels must be integers';    
  ER_MAPE_ALL_ZERO_TARGET =
    'Все значения целевой переменной равны нулю — MAPE не определена!!All target values are zero — MAPE is undefined';
  ER_BINARY_ONLY =
    'Метрика доступна только для бинарной классификации!!This metric is available only for binary classification';
  ER_INVALID_PROBABILITY =
    'Вероятность вне диапазона [0,1]: {0}!!Probability out of range [0,1]: {0}';
  ER_ARG_OUT_OF_RANGE =
    'Аргумент {0} имеет недопустимое значение: {1}!!Argument {0} is out of range: {1}';  
  
//-----------------------------
//           Metrics
//-----------------------------

static function Metrics.MSE(yTrue, yPred: Vector): real;
begin
  if yTrue = nil then
    ArgumentNullError(ER_ARG_NULL, 'yTrue');

  if yPred = nil then
    ArgumentNullError(ER_ARG_NULL, 'yPred');

  var n := yTrue.Length;

  if n <> yPred.Length then
    DimensionError(ER_DIM_MISMATCH, yTrue.Length, yPred.Length);

  if n = 0 then
    ArgumentError(ER_EMPTY_DATA, 'MSE');

  var sum := 0.0;

  for var i := 0 to n - 1 do
  begin
    var yt := yTrue[i];
    var yp := yPred[i];

    if double.IsNaN(yt) or double.IsInfinity(yt) then
      ArgumentError(ER_INVALID_VALUE, 'yTrue', i);

    if double.IsNaN(yp) or double.IsInfinity(yp) then
      ArgumentError(ER_INVALID_VALUE, 'yPred', i);

    var d := yt - yp;
    sum += d * d;
  end;

  Result := sum / n;
end;

static function Metrics.RMSE(yTrue, yPred: Vector): real;
begin
  Result := Sqrt(MSE(yTrue, yPred));
end;

static function Metrics.MAE(yTrue, yPred: Vector): real;
begin
  if yTrue = nil then
    ArgumentNullError(ER_ARG_NULL, 'yTrue');

  if yPred = nil then
    ArgumentNullError(ER_ARG_NULL, 'yPred');

  var n := yTrue.Length;

  if n <> yPred.Length then
    DimensionError(ER_DIM_MISMATCH, yTrue.Length, yPred.Length);

  if n = 0 then
    ArgumentError(ER_EMPTY_DATA, 'MAE');

  var s := 0.0;

  for var i := 0 to n - 1 do
  begin
    var yt := yTrue[i];
    var yp := yPred[i];

    if double.IsNaN(yt) or double.IsInfinity(yt) then
      ArgumentError(ER_INVALID_VALUE, 'yTrue', i);

    if double.IsNaN(yp) or double.IsInfinity(yp) then
      ArgumentError(ER_INVALID_VALUE, 'yPred', i);

    s += Abs(yt - yp);
  end;

  Result := s / n;
end;

static function Metrics.MAPE(yTrue, yPred: Vector): real;
begin
  if yTrue = nil then
    ArgumentNullError(ER_ARG_NULL, 'yTrue');

  if yPred = nil then
    ArgumentNullError(ER_ARG_NULL, 'yPred');

  if yTrue.Length <> yPred.Length then
    DimensionError(ER_DIM_MISMATCH, yTrue.Length, yPred.Length);

  var sum := 0.0;
  var cnt := 0;

  for var i := 0 to yTrue.Length - 1 do
  begin
    var y := yTrue[i];
    var yp := yPred[i];

    if double.IsNaN(y) or double.IsInfinity(y) then
      ArgumentError(ER_ARG_NAN_INF, 'yTrue');

    if double.IsNaN(yp) or double.IsInfinity(yp) then
      ArgumentError(ER_ARG_NAN_INF, 'yPred');

    if y = 0.0 then continue;

    sum += Abs((y - yp) / y);
    cnt += 1;
  end;

  if cnt = 0 then
    Error(ER_MAPE_ALL_ZERO_TARGET);

  Result := sum / cnt;
end;

static function Metrics.MedianAE(yTrue, yPred: Vector): real;
begin
  if yTrue = nil then
    ArgumentNullError(ER_ARG_NULL, 'yTrue');

  if yPred = nil then
    ArgumentNullError(ER_ARG_NULL, 'yPred');

  if yTrue.Length <> yPred.Length then
    DimensionError(ER_DIM_MISMATCH, yTrue.Length, yPred.Length);

  var n := yTrue.Length;
  if n = 0 then
    ArgumentError(ER_EMPTY_DATA, 'MedianAbsoluteError');

  var errors := new real[n];

  for var i := 0 to n - 1 do
    errors[i] := Abs(yTrue[i] - yPred[i]);

  // сортировка
  &Array.Sort(errors);

  if n mod 2 = 1 then
    Result := errors[n div 2]
  else
    Result := (errors[n div 2 - 1] + errors[n div 2]) / 2.0;
end;

static function Metrics.R2(yTrue, yPred: Vector): real;
begin
  if yTrue = nil then
    ArgumentNullError(ER_ARG_NULL, 'yTrue');

  if yPred = nil then
    ArgumentNullError(ER_ARG_NULL, 'yPred');

  var n := yTrue.Length;

  if n <> yPred.Length then
    DimensionError(ER_DIM_MISMATCH, yTrue.Length, yPred.Length);

  if n = 0 then
    ArgumentError(ER_EMPTY_DATA, 'R2');

  var sumY := 0.0;

  for var i := 0 to n - 1 do
  begin
    var yt := yTrue[i];
    if double.IsNaN(yt) or double.IsInfinity(yt) then
      ArgumentError(ER_INVALID_VALUE, 'yTrue', i);

    sumY += yt;
  end;

  var meanY := sumY / n;

  var ssRes := 0.0;
  var ssTot := 0.0;

  for var i := 0 to n - 1 do
  begin
    var yt := yTrue[i];
    var yp := yPred[i];

    if double.IsNaN(yp) or double.IsInfinity(yp) then
      ArgumentError(ER_INVALID_VALUE, 'yPred', i);

    var d1 := yt - yp;
    ssRes += d1 * d1;

    var d2 := yt - meanY;
    ssTot += d2 * d2;
  end;

  if ssTot = 0.0 then
  begin
    Result := 0.0;
    exit;
  end;

  Result := 1 - ssRes / ssTot;
end;

static function Metrics.LogLoss(yTrue, yPred: Vector): real;
begin
  if yTrue = nil then
    ArgumentNullError(ER_ARG_NULL, 'yTrue');

  if yPred = nil then
    ArgumentNullError(ER_ARG_NULL, 'yPred');

  var n := yTrue.Length;

  if n <> yPred.Length then
    DimensionError(ER_DIM_MISMATCH, yTrue.Length, yPred.Length);

  if n = 0 then
    ArgumentError(ER_EMPTY_DATA, 'LogLoss');

  var eps := 1e-15;
  var s := 0.0;

  for var i := 0 to n - 1 do
  begin
    var yt := yTrue[i];
    var p := yPred[i];

    if double.IsNaN(yt) or double.IsInfinity(yt) then
      ArgumentError(ER_INVALID_VALUE, 'yTrue', i);

    if double.IsNaN(p) or double.IsInfinity(p) then
      ArgumentError(ER_INVALID_VALUE, 'yPred', i);

    if (yt <> 0.0) and (yt <> 1.0) then
      ArgumentError(ER_INVALID_CLASS_LABEL, yt);
    
    if (p < 0.0) or (p > 1.0) then
      ArgumentError(ER_INVALID_PROBABILITY, p);

    if p < eps then p := eps;
    if p > 1.0 - eps then p := 1.0 - eps;

    s += yt * Ln(p) + (1.0 - yt) * Ln(1.0 - p);
  end;

  Result := -s / n;
end;

static function Metrics.ROC(yTrue, yPred: Vector): (Vector, Vector);
begin
  if yTrue = nil then
    ArgumentNullError(ER_ARG_NULL, 'yTrue');

  if yPred = nil then
    ArgumentNullError(ER_ARG_NULL, 'yPred');

  var n := yTrue.Length;

  if n <> yPred.Length then
    DimensionError(ER_DIM_MISMATCH, yTrue.Length, yPred.Length);

  if n = 0 then
    ArgumentError(ER_EMPTY_DATA, 'ROC');

  var totalPos := 0;
  var totalNeg := 0;
  
  for var i := 0 to n - 1 do
  begin
    var yt := yTrue[i];
    var p := yPred[i];

    if double.IsNaN(yt) or double.IsInfinity(yt) then
      ArgumentError(ER_INVALID_VALUE, 'yTrue', i);

    if double.IsNaN(p) or double.IsInfinity(p) then
      ArgumentError(ER_INVALID_VALUE, 'yPred', i);

    if (yt <> 0.0) and (yt <> 1.0) then
      ArgumentError(ER_INVALID_CLASS_LABEL, yt);
    
    if (p < 0.0) or (p > 1.0) then
      ArgumentError(ER_INVALID_PROBABILITY, p);

    if yt = 1.0 then
      totalPos += 1
    else
      totalNeg += 1;
  end;

  if (totalPos = 0) or (totalNeg = 0) then
    ArgumentError(ER_DIV_BY_ZERO, 'ROC');

  var idx :=
    (0..n-1)
      .OrderByDescending(i -> yPred[i])
      .ToArray;

  var tp := 0;
  var fp := 0;

  var fprList := new List<real>;
  var tprList := new List<real>;
  
  fprList.Add(0.0);
  tprList.Add(0.0);

  for var k := 0 to n - 1 do
  begin
    var i := idx[k];

    if yTrue[i] = 1.0 then
      tp += 1
    else
      fp += 1;

    var tpr := tp / totalPos;
    var fpr := fp / totalNeg;

    tprList.Add(tpr);
    fprList.Add(fpr);
  end;
  
  Result := (new Vector(fprList.ToArray),
             new Vector(tprList.ToArray));
end;

static function Metrics.AUC(yTrue, yPred: Vector): real;
begin
  if yTrue = nil then
    ArgumentNullError(ER_ARG_NULL, 'yTrue');

  if yPred = nil then
    ArgumentNullError(ER_ARG_NULL, 'yPred');

  var n := yTrue.Length;

  if n <> yPred.Length then
    DimensionError(ER_DIM_MISMATCH, yTrue.Length, yPred.Length);

  if n = 0 then
    ArgumentError(ER_EMPTY_DATA, 'AUC');

  var (fpr, tpr) := ROC(yTrue, yPred);

  if fpr.Length <> tpr.Length then
    DimensionError(ER_DIM_MISMATCH, fpr.Length, tpr.Length);

  if fpr.Length < 2 then
    ArgumentError(ER_EMPTY_DATA, 'AUC');

  var area := 0.0;

  for var i := 1 to fpr.Length - 1 do
  begin
    var dx := fpr[i] - fpr[i - 1];

    if double.IsNaN(dx) or double.IsInfinity(dx) then
      ArgumentError(ER_INVALID_VALUE, 'fpr', i);

    var avgY := (tpr[i] + tpr[i - 1]) / 2;

    if double.IsNaN(avgY) or double.IsInfinity(avgY) then
      ArgumentError(ER_INVALID_VALUE, 'tpr', i);

    area += dx * avgY;
  end;

  Result := area;
end;

static function Metrics.Accuracy(yTrue, yPred: Vector): real;
begin
  if yTrue = nil then
    ArgumentNullError(ER_ARG_NULL, 'yTrue');

  if yPred = nil then
    ArgumentNullError(ER_ARG_NULL, 'yPred');

  var n := yTrue.Length;

  if n <> yPred.Length then
    DimensionError(ER_DIM_MISMATCH, yTrue.Length, yPred.Length);

  if n = 0 then
    ArgumentError(ER_EMPTY_DATA, 'Accuracy');

  var correct := 0;

  for var i := 0 to n - 1 do
  begin
    var yt := yTrue[i];
    var yp := yPred[i];
    var ytInt := Round(yt);
    var ypInt := Round(yp);
    
    if Abs(yt - ytInt) > 1e-12 then
      ArgumentError(ER_INVALID_CLASS_LABEL, yt);
    
    if Abs(yp - ypInt) > 1e-12 then
      ArgumentError(ER_INVALID_CLASS_LABEL, yp);
    
    if ytInt = ypInt then
      correct += 1;
  end;

  Result := correct / n;
end;

static function Metrics.Accuracy(yTrue, yPred: array of integer): real;
begin
  if yTrue = nil then
    ArgumentNullError(ER_ARG_NULL, 'yTrue');

  if yPred = nil then
    ArgumentNullError(ER_ARG_NULL, 'yPred');

  var n := yTrue.Length;

  if n <> yPred.Length then
    DimensionError(ER_DIM_MISMATCH, yTrue.Length, yPred.Length);

  if n = 0 then
    ArgumentError(ER_EMPTY_DATA, 'Accuracy');

  var correct := 0;

  for var i := 0 to n - 1 do
    if yTrue[i] = yPred[i] then
      correct += 1;

  Result := correct / n;
end;

static function Metrics.Accuracy(yTrue: Vector; yPred: array of integer): real;
begin
  if yTrue = nil then
    ArgumentNullError(ER_ARG_NULL, 'yTrue');

  var data := yTrue.Data;
  var n := data.Length;

  if n <> yPred.Length then
    DimensionError(ER_DIM_MISMATCH, n, yPred.Length);

  if n = 0 then
    ArgumentError(ER_EMPTY_DATA, 'Accuracy');

  var correct := 0;

  for var i := 0 to n - 1 do
  begin
    var yt := data[i];
    var ytInt := Round(yt);

    // проверка: метки должны быть целыми
    if Abs(yt - ytInt) > 1e-12 then
      ArgumentError(ER_INVALID_CLASS_LABEL, yt);

    if ytInt = yPred[i] then
      correct += 1;
  end;

  Result := correct / n;
end;

static function Metrics.Precision(yTrue, yPred: Vector): real;
begin
  Result := ConfusionMatrix.Create(yTrue, yPred).Precision;
end;

static function Metrics.Recall(yTrue, yPred: Vector): real;
begin
  Result := ConfusionMatrix.Create(yTrue, yPred).Recall;
end;

static function Metrics.F1(yTrue, yPred: Vector): real;
begin
  Result := ConfusionMatrix.Create(yTrue, yPred).F1;
end;

static function Metrics.Precision(yTrue, yPred: Vector; avg: Averaging): real;
begin
  var cm := new ConfusionMatrix(yTrue, yPred);
  var k := cm.ClassCount;

  // --- Macro
  if avg = avMacro then
  begin
    var sum := 0.0;

    for var c := 0 to k - 1 do
      sum += cm.PrecisionForClass(c);

    Result := sum / k;
    exit;
  end;

  // --- Micro
  if avg = avMicro then
  begin
    var tpSum := 0;
    var fpSum := 0;

    for var c := 0 to k - 1 do
    begin
      tpSum += cm.TPForClass(c);
      fpSum += cm.FPForClass(c);
    end;

    if tpSum + fpSum = 0 then
      exit(0.0);

    Result := tpSum / (tpSum + fpSum);
    exit;
  end;

  // --- Weighted
  if avg = avWeighted then
  begin
    var sum := 0.0;
    var total := 0;

    for var c := 0 to k - 1 do
    begin
      var support := cm.TPForClass(c) + cm.FNForClass(c);
      if support = 0 then continue;

      sum += cm.PrecisionForClass(c) * support;
      total += support;
    end;

    if total = 0 then
      exit(0.0);

    Result := sum / total;
    exit;
  end;

  Result := 0.0;
end;

static function Metrics.Recall(yTrue, yPred: Vector; avg: Averaging): real;
begin
  var cm := new ConfusionMatrix(yTrue, yPred);
  var k := cm.ClassCount;

  // --- Macro
  if avg = avMacro then
  begin
    var sum := 0.0;

    for var c := 0 to k - 1 do
      sum += cm.RecallForClass(c);

    Result := sum / k;
    exit;
  end;

  // --- Micro
  if avg = avMicro then
  begin
    var tpSum := 0;
    var fnSum := 0;

    for var c := 0 to k - 1 do
    begin
      tpSum += cm.TPForClass(c);
      fnSum += cm.FNForClass(c);
    end;

    if tpSum + fnSum = 0 then
      exit(0.0);

    Result := tpSum / (tpSum + fnSum);
    exit;
  end;

  // --- Weighted
  if avg = avWeighted then
  begin
    var sum := 0.0;
    var total := 0;

    for var c := 0 to k - 1 do
    begin
      var support := cm.TPForClass(c) + cm.FNForClass(c);
      if support = 0 then continue;

      sum += cm.RecallForClass(c) * support;
      total += support;
    end;

    if total = 0 then
      exit(0.0);

    Result := sum / total;
    exit;
  end;

  Result := 0.0;
end;

static function Metrics.F1(yTrue, yPred: Vector; avg: Averaging): real;
begin
  var cm := new ConfusionMatrix(yTrue, yPred);
  var k := cm.ClassCount;

  // --- Macro
  if avg = avMacro then
  begin
    var sum := 0.0;

    for var c := 0 to k - 1 do
      sum += cm.F1ForClass(c);

    Result := sum / k;
    exit;
  end;

  // --- Micro
  if avg = avMicro then
  begin
    var tpSum := 0;
    var fpSum := 0;
    var fnSum := 0;

    for var c := 0 to k - 1 do
    begin
      tpSum += cm.TPForClass(c);
      fpSum += cm.FPForClass(c);
      fnSum += cm.FNForClass(c);
    end;

    if tpSum = 0 then
      exit(0.0);

    Result := 2 * tpSum / (2 * tpSum + fpSum + fnSum);
    exit;
  end;

  // --- Weighted
  if avg = avWeighted then
  begin
    var sum := 0.0;
    var total := 0;

    for var c := 0 to k - 1 do
    begin
      var support := cm.TPForClass(c) + cm.FNForClass(c);
      if support = 0 then continue;

      sum += cm.F1ForClass(c) * support;
      total += support;
    end;

    if total = 0 then
      exit(0.0);

    Result := sum / total;
    exit;
  end;

  Result := 0.0;
end;

static function Metrics.Specificity(yTrue, yPred: Vector): real;
begin
  Result := ConfusionMatrix.Create(yTrue, yPred).Specificity;
end;

static function Metrics.BalancedAccuracy(yTrue, yPred: Vector): real;
begin
  Result := ConfusionMatrix.Create(yTrue, yPred).BalancedAccuracy;
end;

static function Metrics.SilhouetteSamples(X: Matrix; labels: Vector): Vector;
begin
  if X = nil then
    ArgumentNullError(ER_ARG_NULL, 'X');
  if labels = nil then
    ArgumentNullError(ER_ARG_NULL, 'labels');

  var n := X.RowCount;
  var p := X.ColCount;

  if n = 0 then
    ArgumentError(ER_EMPTY_DATA, 'SilhouetteSamples');

  if labels.Length <> n then
    DimensionError(ER_DIM_MISMATCH, labels.Length, n);

  // --- собираем размеры кластеров и маппинг label -> 0..c-1
  var map := new Dictionary<integer, integer>();
  var counts := new List<integer>();
  var clsId := new integer[n];

  for var i := 0 to n - 1 do
  begin
    var v := labels[i];
    var lab := integer(v);

    if Abs(v - lab) > 1e-12 then
      ArgumentError(ER_LABELS_MUST_BE_INTEGERS);

    var id: integer;
    if map.TryGetValue(lab, id) then
    begin
      clsId[i] := id;
      counts[id] += 1;
    end
    else
    begin
      id := map.Count;
      map.Add(lab, id);
      clsId[i] := id;
      counts.Add(1);
    end;
  end;

  var cCount := counts.Count;
  if cCount < 2 then
    ArgumentError(ER_SILHOUETTE_NEEDS_2_CLUSTERS);

  // --- список уникальных кластеров, чтобы не обращаться к Dictionary в hot-path
  var sizes := counts.ToArray;

  // --- результат
  Result := new Vector(n);

  // --- рабочие массивы (переиспользуем, без аллокаций в цикле по i)
  var sumToCluster := new real[cCount];

  for var i := 0 to n - 1 do
  begin
    // обнуляем суммы
    for var c := 0 to cCount - 1 do
      sumToCluster[c] := 0.0;

    // суммируем расстояния от i до всех остальных по кластерам
    for var t := 0 to n - 1 do
      if t <> i then
      begin
        var dist := 0.0;

        for var j := 0 to p - 1 do
        begin
          var a := X[i,j];
          if double.IsNaN(a) or double.IsInfinity(a) then
            ArgumentError(ER_INVALID_VALUE_AT, 'X', i);

          var b := X[t,j];
          if double.IsNaN(b) or double.IsInfinity(b) then
            ArgumentError(ER_INVALID_VALUE_AT, 'X', t);

          var d := a - b;
          dist += d * d;
        end;

        sumToCluster[clsId[t]] += Sqrt(dist);
      end;

    var my := clsId[i];

    // a(i): среднее расстояние до своего кластера (без самой точки)
    var aMean := 0.0;
    if sizes[my] > 1 then
      aMean := sumToCluster[my] / (sizes[my] - 1)
    else
      aMean := 0.0;

    // b(i): минимальное среднее расстояние до другого кластера
    var bMean := double.MaxValue;

    for var c := 0 to cCount - 1 do
      if c <> my then
      begin
        if sizes[c] = 0 then
          continue;

        var mean := sumToCluster[c] / sizes[c];
        if mean < bMean then
          bMean := mean;
      end;

    if double.IsInfinity(bMean) then
      bMean := 0.0;

    var denom := Max(aMean, bMean);
    if denom = 0 then
      Result[i] := 0.0
    else
      Result[i] := (bMean - aMean) / denom;
  end;
end;

static function Metrics.SilhouetteScore(X: Matrix; labels: Vector): real;
begin
  var s := SilhouetteSamples(X, labels);

  var n := s.Length;
  var sum := 0.0;

  for var i := 0 to n - 1 do
    sum += s[i];

  if n = 0 then
    exit(0.0);

  Result := sum / n;
end;

/// Вычисляет индекс Калински–Харабаса (Calinski–Harabasz Index).
/// Возвращает отношение межкластерной дисперсии к внутрикластерной дисперсии.
/// Чем больше значение — тем лучше разделены кластеры.
/// При нулевой внутрикластерной дисперсии возвращает +∞.
/// Требует как минимум 2 кластера и n > k.
/// Сложность O(n·p).
static function Metrics.CalinskiHarabaszScore(X: Matrix; labels: Vector): real;
begin
  if X = nil then
    ArgumentNullError(ER_ARG_NULL, 'X');

  if labels = nil then
    ArgumentNullError(ER_ARG_NULL, 'labels');

  var n := X.RowCount;
  var p := X.ColCount;

  if n = 0 then
    ArgumentError(ER_EMPTY_DATA, 'CalinskiHarabaszScore');

  if labels.Length <> n then
    DimensionError(ER_DIM_MISMATCH, labels.Length, n);

  // --- label -> 0..k-1 + sizes
  var map := new Dictionary<integer, integer>();
  var counts := new List<integer>();
  var clsId := new integer[n];

  for var i := 0 to n - 1 do
  begin
    var v := labels[i];
    var lab := integer(v);

    if Abs(v - lab) > 1e-12 then
      ArgumentError(ER_LABELS_MUST_BE_INTEGERS);

    var id: integer;
    if map.TryGetValue(lab, id) then
    begin
      clsId[i] := id;
      counts[id] += 1;
    end
    else
    begin
      id := map.Count;
      map.Add(lab, id);
      clsId[i] := id;
      counts.Add(1);
    end;
  end;

  var k := counts.Count;

  if k < 2 then
    ArgumentError(ER_CH_NEEDS_2_CLUSTERS);

  if k >= n then
    ArgumentError(ER_CH_NEEDS_N_GT_K);

  var sizes := counts.ToArray;

  // --- глобальное среднее
  var globalMean := new real[p];

  for var i := 0 to n - 1 do
    for var j := 0 to p - 1 do
    begin
      var xij := X[i,j];
      if double.IsNaN(xij) or double.IsInfinity(xij) then
        ArgumentError(ER_INVALID_VALUE_AT, 'X', i);
      globalMean[j] += xij;
    end;

  for var j := 0 to p - 1 do
    globalMean[j] /= n;

  // --- центры кластеров
  var centers := new Matrix(k, p);

  for var i := 0 to n - 1 do
  begin
    var c := clsId[i];
    for var j := 0 to p - 1 do
      centers[c,j] += X[i,j];
  end;

  for var c := 0 to k - 1 do
    for var j := 0 to p - 1 do
      centers[c,j] /= sizes[c];

  // --- Within (W): сумма квадратов расстояний до центров кластеров
  var W := 0.0;

  for var i := 0 to n - 1 do
  begin
    var c := clsId[i];
    var dist := 0.0;

    for var j := 0 to p - 1 do
    begin
      var d := X[i,j] - centers[c,j];
      dist += d * d;
    end;

    W += dist;
  end;

  // --- Between (B): сумма n_c * ||mu_c - mu||^2
  var B := 0.0;

  for var c := 0 to k - 1 do
  begin
    var dist := 0.0;

    for var j := 0 to p - 1 do
    begin
      var d := centers[c,j] - globalMean[j];
      dist += d * d;
    end;

    B += sizes[c] * dist;
  end;

  if W = 0 then
    exit(double.PositiveInfinity);

  var ch := (B / (k - 1)) / (W / (n - k));

  if double.IsNaN(ch) or double.IsInfinity(ch) then
    ArgumentError(ER_INVALID_VALUE, 'CalinskiHarabaszScore');

  Result := ch;
end;

static function Metrics.DaviesBouldinScore(X: Matrix; labels: Vector): real;
begin
  if X = nil then
    ArgumentNullError(ER_ARG_NULL, 'X');

  if labels = nil then
    ArgumentNullError(ER_ARG_NULL, 'labels');

  var n := X.RowCount;
  var p := X.ColCount;

  if labels.Length <> n then
    DimensionError(ER_DIM_MISMATCH, labels.Length, n);

  // --- label -> 0..k-1
  var map := new Dictionary<integer, integer>();
  var counts := new List<integer>();
  var clsId := new integer[n];

  for var i := 0 to n - 1 do
  begin
    var v := labels[i];
    var lab := integer(v);

    if Abs(v - lab) > 1e-12 then
      ArgumentError(ER_LABELS_MUST_BE_INTEGERS);

    var id: integer;
    if map.TryGetValue(lab, id) then
    begin
      clsId[i] := id;
      counts[id] += 1;
    end
    else
    begin
      id := map.Count;
      map.Add(lab, id);
      clsId[i] := id;
      counts.Add(1);
    end;
  end;

  var k := counts.Count;
  if k < 2 then
    ArgumentError(ER_CH_NEEDS_2_CLUSTERS);

  var sizes := counts.ToArray;

  // --- центры
  var centers := new Matrix(k, p);

  for var i := 0 to n - 1 do
  begin
    var c := clsId[i];
    for var j := 0 to p - 1 do
      centers[c,j] += X[i,j];
  end;

  for var c := 0 to k - 1 do
    for var j := 0 to p - 1 do
      centers[c,j] /= sizes[c];

  // --- Si: среднее расстояние точек до центра
  var S := new real[k];

  for var i := 0 to n - 1 do
  begin
    var c := clsId[i];
    var dist := 0.0;

    for var j := 0 to p - 1 do
    begin
      var d := X[i,j] - centers[c,j];
      dist += d * d;
    end;

    S[c] += Sqrt(dist);
  end;

  for var c := 0 to k - 1 do
    S[c] /= sizes[c];

  // --- считаем DB
  var total := 0.0;

  for var i := 0 to k - 1 do
  begin
    var maxR := 0.0;

    for var j := 0 to k - 1 do
      if i <> j then
      begin
        var dist := 0.0;

        for var d := 0 to p - 1 do
        begin
          var delta := centers[i,d] - centers[j,d];
          dist += delta * delta;
        end;

        dist := Sqrt(dist);

        if dist = 0 then
          continue;

        var Rij := (S[i] + S[j]) / dist;

        if Rij > maxR then
          maxR := Rij;
      end;

    total += maxR;
  end;

  Result := total / k;
end;

/// Adjusted Rand Index (ARI).
/// Внешняя метрика качества кластеризации.
/// Сравнивает предсказанные кластеры с истинными метками.
///
/// Основана на анализе всех пар объектов:
/// проверяет, совпадает ли решение алгоритма и истинная разметка
/// в вопросе "находятся ли две точки в одном кластере".
///
/// Корректирует обычный Rand Index на случайные совпадения.
///
/// Диапазон значений:
/// 1   — идеальное совпадение кластеризации и истинных меток
/// 0   — случайное разбиение
/// <0  — хуже случайного
/// Использует таблицу сопряжённости (contingency table).
/// Сложность: O(n + kT * kP),
/// где kT и kP — числа различных классов в yTrue и yPred.
static function Metrics.AdjustedRandIndex(yTrue, yPred: Vector): real;
begin
  if yTrue = nil then
    ArgumentNullError(ER_ARG_NULL, 'yTrue');

  if yPred = nil then
    ArgumentNullError(ER_ARG_NULL, 'yPred');

  var n := yTrue.Length;
  if n <> yPred.Length then
    DimensionError(ER_DIM_MISMATCH, yTrue.Length, yPred.Length);

  if n < 2 then
  begin
    Result := 1.0;
    exit;
  end;

  // --- Vector -> int[]
  var yTInt := LabelsToInts(yTrue);
  var yPInt := LabelsToInts(yPred);

  // --- encoding
  var classesTrue: array of integer;
  var classesPred: array of integer;

  var yT := EncodeLabelsInt(yTInt, classesTrue);
  var yP := EncodeLabelsInt(yPInt, classesPred);

  var kT := classesTrue.Length;
  var kP := classesPred.Length;

  // --- contingency
  var table := new integer[kT, kP];

  for var i := 0 to n - 1 do
    table[yT[i], yP[i]] += 1;

  // --- sums
  var sumNij := 0.0;
  var sumAi := 0.0;
  var sumBj := 0.0;

  // n_ij
  for var i := 0 to kT - 1 do
    for var j := 0 to kP - 1 do
    begin
      var x := table[i,j];
      if x >= 2 then
        sumNij += x * (x - 1) / 2.0;
    end;

  // a_i
  for var i := 0 to kT - 1 do
  begin
    var s := 0;
    for var j := 0 to kP - 1 do
      s += table[i,j];

    if s >= 2 then
      sumAi += s * (s - 1) / 2.0;
  end;

  // b_j
  for var j := 0 to kP - 1 do
  begin
    var s := 0;
    for var i := 0 to kT - 1 do
      s += table[i,j];

    if s >= 2 then
      sumBj += s * (s - 1) / 2.0;
  end;

  var total := n * (n - 1) / 2.0;

  var expected := (sumAi * sumBj) / total;
  var maxIndex := 0.5 * (sumAi + sumBj);

  var denom := maxIndex - expected;

  if denom = 0 then
  begin
    Result := 0.0;
    exit;
  end;

  Result := (sumNij - expected) / denom;
end;

function R(s: string; w: integer): string;
begin
  Result := s.PadLeft(w);
end;

static function Metrics.ClassificationReport(yTrue, yPred: Vector): string;
begin
  var cm := new ConfusionMatrix(yTrue, yPred);

  var rows := new List<(string, real, real, real, integer)>;
  var total := yTrue.Length;

  // --- собираем строки классов
  for var c := 0 to cm.ClassCount - 1 do
  begin
    var cls := cm.GetLabel(c).ToString;

    var p := cm.PrecisionForClass(c);
    var r := cm.RecallForClass(c);
    var f := cm.F1ForClass(c);
    var support := cm.TPForClass(c) + cm.FNForClass(c);

    rows.Add((cls, p, r, f, support));
  end;

  // --- averages
  var pMacro := Metrics.Precision(yTrue, yPred, avMacro);
  var rMacro := Metrics.Recall(yTrue, yPred, avMacro);
  var fMacro := Metrics.F1(yTrue, yPred, avMacro);

  var pW := Metrics.Precision(yTrue, yPred, avWeighted);
  var rW := Metrics.Recall(yTrue, yPred, avWeighted);
  var fW := Metrics.F1(yTrue, yPred, avWeighted);

  // --- helper форматирования
  var F: real -> string := x -> x.ToString('0.00');

  // --- считаем ширины колонок
  var wClass := 'class'.Length;
  var wP := 'precision'.Length;
  var wR := 'recall'.Length;
  var wF := 'f1-score'.Length;
  var wS := 'support'.Length;

  foreach var row in rows do
  begin
    wClass := Max(wClass, row.Item1.Length);
    wP := Max(wP, F(row.Item2).Length);
    wR := Max(wR, F(row.Item3).Length);
    wF := Max(wF, F(row.Item4).Length);
    wS := Max(wS, row.Item5.ToString.Length);
  end;

  wClass := Max(wClass, 'macro avg'.Length);
  wClass := Max(wClass, 'weighted avg'.Length);

  wP := Max(wP, F(pMacro).Length);
  wR := Max(wR, F(rMacro).Length);
  wF := Max(wF, F(fMacro).Length);

  wP := Max(wP, F(pW).Length);
  wR := Max(wR, F(rW).Length);
  wF := Max(wF, F(fW).Length);

  wS := Max(wS, total.ToString.Length);

  // --- сборка строки
  var sb := new System.Text.StringBuilder;

  // header
  sb.AppendLine(
    R('class', wClass) + '  ' +
    R('precision', wP) + '  ' +
    R('recall', wR) + '  ' +
    R('f1-score', wF) + '  ' +
    R('support', wS)
  );

  var totalWidth := wClass + 2 + wP + 2 + wR + 2 + wF + 2 + wS;
  sb.AppendLine(new string('-', totalWidth));

  // rows
  foreach var row in rows do
    sb.AppendLine(
      R(row.Item1, wClass) + '  ' +
      R(F(row.Item2), wP) + '  ' +
      R(F(row.Item3), wR) + '  ' +
      R(F(row.Item4), wF) + '  ' +
      R(row.Item5.ToString, wS)
    );

  sb.AppendLine;

  // macro avg
  sb.AppendLine(
    R('macro avg', wClass) + '  ' +
    R(F(pMacro), wP) + '  ' +
    R(F(rMacro), wR) + '  ' +
    R(F(fMacro), wF) + '  ' +
    R(total.ToString, wS)
  );
  
  sb.AppendLine(
    R('weighted avg', wClass) + '  ' +
    R(F(pW), wP) + '  ' +
    R(F(rW), wR) + '  ' +
    R(F(fW), wF) + '  ' +
    R(total.ToString, wS)
  );

  Result := sb.ToString;
end;

//-----------------------------
//        ConfusionMatrix
//-----------------------------

constructor ConfusionMatrix.Create(yTrue, yPred: Vector);
begin
  if yTrue = nil then
    ArgumentNullError(ER_ARG_NULL, 'yTrue');

  if yPred = nil then
    ArgumentNullError(ER_ARG_NULL, 'yPred');

  var n := yTrue.Length;

  if n <> yPred.Length then
    DimensionError(ER_DIM_MISMATCH, yTrue.Length, yPred.Length);

  if n = 0 then
    ArgumentError(ER_EMPTY_DATA, 'ConfusionMatrix');

  var yArr := yTrue.Data;
  var pArr := yPred.Data;

  // --- 1. собираем уникальные метки
  var setLabels := new HashSet<integer>;

  for var i := 0 to n - 1 do
  begin
    var yt := yArr[i];
    var yp := pArr[i];

    if double.IsNaN(yt) or double.IsInfinity(yt) then
      ArgumentError(ER_INVALID_VALUE, 'yTrue', i);

    if double.IsNaN(yp) or double.IsInfinity(yp) then
      ArgumentError(ER_INVALID_VALUE, 'yPred', i);

    var ytInt := Round(yt);
    var ypInt := Round(yp);

    if Abs(yt - ytInt) > 1e-12 then
      ArgumentError(ER_INVALID_CLASS_LABEL, yt);

    if Abs(yp - ypInt) > 1e-12 then
      ArgumentError(ER_INVALID_CLASS_LABEL, yp);

    setLabels.Add(ytInt);
    setLabels.Add(ypInt);
  end;

  // --- 2. сортируем метки
  var labels := setLabels.ToArray;
  &Array.Sort(labels);
  
  fLabels := labels;

  fClassCount := labels.Length;

  // --- 3. строим отображение label → index
  fLabelToIndex := new Dictionary<integer, integer>(fClassCount);

  for var i := 0 to fClassCount - 1 do
    fLabelToIndex.Add(labels[i], i);

  // --- 4. создаём матрицу
  fMatrix := new integer[fClassCount, fClassCount];

  // --- 5. заполняем
  for var i := 0 to n - 1 do
  begin
    var yt := integer(yArr[i]);
    var yp := integer(pArr[i]);

    var row := fLabelToIndex[yt];
    var col := fLabelToIndex[yp];

    fMatrix[row, col] += 1;
  end;
end;

function ConfusionMatrix.GetMatrix: array[,] of integer;
begin
  Result := new integer[fClassCount, fClassCount];

  for var i := 0 to fClassCount - 1 do
    for var j := 0 to fClassCount - 1 do
      Result[i, j] := fMatrix[i, j];
end;

function ConfusionMatrix.GetLabel(c: integer): integer;
begin
  if (c < 0) or (c >= fClassCount) then
    ArgumentOutOfRangeError(ER_ARG_OUT_OF_RANGE, 'c', c);

  Result := fLabels[c];
end;

function ConfusionMatrix.GetIndex(labl: integer): integer;
begin
  if labl not in fLabelToIndex then
    ArgumentError(ER_INVALID_CLASS_LABEL, labl);

  Result := fLabelToIndex[labl];
end;

function ConfusionMatrix.TPForClass(c: integer): integer;
begin
  EnsureClassIndex(c);
  Result := fMatrix[c,c];
end;

function ConfusionMatrix.FPForClass(c: integer): integer;
begin
  EnsureClassIndex(c);
  
  var s := 0;
  for var i := 0 to fClassCount - 1 do
    s += fMatrix[i,c];

  Result := s - fMatrix[c,c];
end;

function ConfusionMatrix.FNForClass(c: integer): integer;
begin
  EnsureClassIndex(c);
  
  var s := 0;
  for var j := 0 to fClassCount - 1 do
    s += fMatrix[c,j];

  Result := s - fMatrix[c,c];
end;

procedure ConfusionMatrix.EnsureBinary;
begin
  if fClassCount <> 2 then
    Error(ER_BINARY_ONLY);
end;

function ConfusionMatrix.GetTPBinary: integer;
begin
  EnsureBinary;
  Result := fMatrix[1,1];
end;

function ConfusionMatrix.GetFPBinary: integer;
begin
  EnsureBinary;
  Result := fMatrix[0,1];
end;

function ConfusionMatrix.GetFNBinary: integer;
begin
  EnsureBinary;
  Result := fMatrix[1,0];
end;

function ConfusionMatrix.GetTNBinary: integer;
begin
  EnsureBinary;
  Result := fMatrix[0,0];
end;

procedure ConfusionMatrix.EnsureClassIndex(c: integer);
begin
  if (c < 0) or (c >= fClassCount) then
    ArgumentOutOfRangeError(ER_ARG_OUT_OF_RANGE, 'c', c);
end;

function ConfusionMatrix.Accuracy: real;
begin
  var total := 0;
  var correct := 0;

  for var i := 0 to fClassCount - 1 do
    for var j := 0 to fClassCount - 1 do
    begin
      total += fMatrix[i,j];
      if i = j then
        correct += fMatrix[i,j];
    end;

  if total = 0 then
    exit(0.0);

  Result := correct / total;
end;

function ConfusionMatrix.Precision: real;
begin
  EnsureBinary;

  var tp := TP;
  var fp := FP;

  if tp + fp = 0 then
    exit(0.0);

  Result := tp / (tp + fp);
end;

function ConfusionMatrix.Recall: real;
begin
  EnsureBinary;

  var tp := TP;
  var fn := FN;

  if tp + fn = 0 then
    exit(0.0);

  Result := tp / (tp + fn);
end;

function ConfusionMatrix.F1: real;
begin
  EnsureBinary;

  var p := Precision;
  var r := Recall;

  if p + r = 0 then
    exit(0.0);

  Result := 2 * p * r / (p + r);
end;

function ConfusionMatrix.Specificity: real;
begin
  EnsureBinary;

  var tn := TN;
  var fp := FP;

  if tn + fp = 0 then
    exit(0.0);

  Result := tn / (tn + fp);
end;

function ConfusionMatrix.BalancedAccuracy: real;
begin
  EnsureBinary;
  Result := (Recall + Specificity) / 2;
end;

function ConfusionMatrix.PrecisionForClass(c: integer): real;
begin
  var tp := TPForClass(c);
  var fp := FPForClass(c);

  if tp + fp = 0 then
    exit(0.0);

  Result := tp / (tp + fp);
end;

function ConfusionMatrix.RecallForClass(c: integer): real;
begin
  var tp := TPForClass(c);
  var fn := FNForClass(c);

  if tp + fn = 0 then
    exit(0.0);

  Result := tp / (tp + fn);
end;

function ConfusionMatrix.F1ForClass(c: integer): real;
begin
  var p := PrecisionForClass(c);
  var r := RecallForClass(c);

  if p + r = 0 then
    exit(0.0);

  Result := 2 * p * r / (p + r);
end;

end.
