
//*****************************************************************************************************\\
// Copyright (©) Cergey Latchenko ( github.com/SunSerega | forum.mmcs.sfedu.ru/u/sun_serega )
// This code is distributed under the Unlicense
// For details see LICENSE file or this:
// https://github.com/SunSerega/POCGL/blob/master/LICENSE
//*****************************************************************************************************\\
// Copyright (©) Сергей Латченко ( github.com/SunSerega | forum.mmcs.sfedu.ru/u/sun_serega )
// Этот код распространяется с лицензией Unlicense
// Подробнее в файле LICENSE или тут:
// https://github.com/SunSerega/POCGL/blob/master/LICENSE
//*****************************************************************************************************\\

///
///Высокоуровневая оболочка модуля OpenCL
///   OpenCL и OpenCLABC можно использовать одновременно
///   Но контактировать они практически не будут
///
///Если не хватает типа/метода или найдена ошибка - писать сюда:
///   https://github.com/SunSerega/POCGL/issues
///
///Справка данного модуля находится в начале его исходника
///   Исходники можно открыть Ctrl-кликом на любом имени из модуля
///
unit OpenCLABC;

{$region Подробное описание OpenCLABC}

{$region 1. Основные принципы}

{$region 1.0. Разное}

// 1.0.1
// 
// Для большего удобства чтения справки рекомендуется включить сворачивание кода
// ( Сервис —> Настройки —> Редактор —> Разрешить сворачивание кода )
// Когда эта опция включена - регионы можно сворачивать нажав на [-] слева
// А в контекстном меню (по нажатии ПКМ) можно свернуть все регионы сразу
// 

// 1.0.2
// 
// В этой справке будут упоминания примеров
// Их можно найти в папке "C:\PABCWork.NET\Samples\OpenCL\OpenCLABC\Из справки"
// А также в соответствующей папке репозитория на гитхабе: "https://github.com/SunSerega/POCGL/tree/master/Samples/OpenCL/OpenCLABC/Из справки"
// 

// 1.0.3 —— Термины, которые часто путают новички
// 
// CPU — Центральное Процессорное Устройство (процессор)
// GPU — Графическое Процессорное Устройство (видеокарта)
//
// Команда — запрос на выполнение чего-либо. К примеру:
//   — Запрос на запуск программы на GPU
//   — Запрос на начало чтения данных из памяти GPU в оперативную память
//   
//   !! Называть процедуры и функции командами ошибочно !!
// 
// Подпрограмма — процедура или функция
// 
// Метод — особая подпрограмма, вызываемая через экземпляр
//   К примеру, метод Context.SyncInvoke выглядит в коде как "cont.SyncInvoke(...)", где cont — переменная типа Context
// 
// Статический метод - особая подпрограмма, вызываемая через тип
//   К примеру, статический метод Buffer.ValueQueue выглядит в коде как "Buffer.ValueQueue(...)"
// 
// Остальные непонятные термины ищите в справке PascalABC.NET (F1 —> Справка) или в интернете

{$endregion 1.0. Разное}

{$region 1.1 —— Что такое OpenCLABC}

// 
// OpenCLABC — высокоуровневая оболочка модуля OpenCL
// Это значит, что с OpenCLABC можно писать гораздо меньше кода в больших и сложных программах
// Однако, такой же уровень микроконтроля как с модулем OpenCL недоступен
// Например, напрямую управлять cl_event'ами в OpenCLABC невозможно
// Вместо этого надо использовать операции с очередями (как сложение и умножение очередей)
// 

{$endregion 1.1 —— Что такое OpenCLABC}

{$region 1.2 —— Контекст (Context)}

// 
// Для отправки команд в GPU необходим контекст (объект типа Context)
// Он содержит информацию о том, какое устройство будет использоваться при выполнении программ и хранении содержимого буферов
// 

{$endregion 1.2 —— Контекст (Context)}

{$region 1.3 —— Очередь [команд] (CommandQueue)}

// 
// Передавать команды для GPU по одной не эффективно
// Гораздо эффективнее передавать несколько команд сразу
// Для этого существуют очереди (типы, наследующие от CommandQueue<T>)
// Они хранят произвольное количество команд для GPU
// А при необходимости также и части кода, выполняемые на CPU
// 

{$endregion 1.3 —— Очередь [команд] (CommandQueue)}

{$region 1.4 —— Буфер (Buffer)}

// 
// Программы на GPU не могут использовать оперативную память (без определённых расширений)
// Из-за чего для передачи данных в программу и чтения результата нужно выделять память на самом GPU
// 
// Данные об области памяти, выделенной на GPU
// И все возможные операции с этой памятью
// Доступны через переменные типа Buffer
// 

{$endregion 1.4 —— Буфер (Buffer)}

{$region 1.5 —— Контейнер для кода (ProgramCode)}

// 
// Обычные программы невозможно запустить на GPU
// Специальные программы для GPU, запускаемые через OpenCL, обычно пишутся на особом языке "OpenCL C" (основанном на языке "C")
// Описание "OpenCL C" не входит в данную справку
// Одну из последних версий его спецификации можно найти тут:
// https://www.khronos.org/registry/OpenCL/specs/2.2/pdf/
// 
// Код на языке "OpenCL C" хранится в объектах типа ProgramCode
// Объекты этого типа используются только как контейнеры
// Один объект ProgramCode может содержать любое количествово подпрограмм-кёрнелов
// 

{$endregion 1.5 —— Контейнер для кода (ProgramCode)}

{$region 1.6 —— Кёрнел (Kernel)}

// 
// Объект типа Kernel представляет одну подпрограмму-кёрнел
// Он хранит код, который можно выполнить на GPU
// 

{$endregion 1.6 —— Кёрнел (Kernel)}

{$endregion 1. Основные принципы}

{$region 2. Контекст (Context)}

// 
// Создать контекст можно конструктором ("new Context")
// Контекст можно и не создавать, используя всюду свойство Context.Default
// Изначально этому свойству присваивается контекст, использующий один любой GPU, если таковой есть
// или один любой другой девайс, поддерживающий OpenCL, если GPU отсутствует
// 
// Context.Default можно перезаписывать
// Это удобно если во всей программе использовать общий контекст
// Операции, у которых невозможно указать контекст - всегда используют Context.Default
// 
// Для вызова команд в определённом контексте используется метод Context.BeginInvoke
// Он возвращает объект типа Task, через который можно наблюдать за выполнением очереди и ожидать его окончания
// Также есть метод Context.SyncInvoke, вызывающий .BeginInvoke и затем метод Task.Wait на полученом объекте
// Подробнее в разделе 3.2
// 

{$endregion 2. Контекст (Context)}

{$region 3. Очередь [команд] (CommandQueue)}

{$region 3.0 —— Возвращаемое значение очередей}

// 
// У каждого типа-очереди есть свой тип возвращаемого значение
// К примеру, так объявляется переменная в которую можно будет сохранить очередь, возвращающую integer:
// var Q1: CommandQueue<integer>;
// 
// Очереди, созданные из буфера или кёрнела возващают свой буфер/кёрнел соответственно, из которого были созданы
// Очереди, созданные с HFQ - значение, которое возвращала оригинальная функция
// Очереди, созданные с HPQ - значение типа object (и всегда nil)
// Подробнее в примере: "3 – Очередь\3.0\Примеры возвращаемого значения"
// 
// После выполнения очереди метод Context.SyncInvoke возвращает то, что вернула очередь
// Если использовать метод Context.BeginInvoke, то возвращаемое значение можно получить через свойство Task.Result
// 

{$endregion 3.0 —— Возвращаемое значение очередей}

{$region 3.1. —— Создание очередей}

// 3.1.1 —— Создание очередей с командами для GPU
// 
// Самый просто способ создать очередь —— выбрать объект (Kernel или Buffer)
// у которого есть что-то, что можно выполнять на GPU (выполнение кёрнела или запись/чтение содержимого буфера)
// и вызвать для него метод .NewQueue
// Подробнее в примере "3 – Очередь\3.1\Создание очереди из буфера.pas"
// 
// Полученная очередь будет иметь особый тип: KernelCommandQueue/BufferCommandQueue для кёрнела/буфера соответственно
// К такой очереди можно добавлять команды, вызывая её методы, имена которых начинаются с ".Add..."
// 

// 3.1.2 —— Создание очередей из подпрограммы, написанной для CPU
// 
// Иногда между командами для GPU надо вставить выполнение обычного кода на CPU
// И в большинстве таких случаев придётся разрывать очередь на две части, что плохо
// (Одна целая очередь всегда выполнится быстрее двух её частей)
// 
// Для таких случаев существуют глобальные подпрограммы HFQ и HPQ
// HFQ — Host Function Queue
// HPQ — Host Procedure Queue
// Они возвращают очередь, выполняющую код (функцию/процедуру соотвественно) на CPU
// 

// 3.1.3 —— Объединение очередей
// 
// Если сложить две очереди A и B ("var C := A+B") — получится очередь C, в которой сначала выполнится A, а затем B
// Очередь C будет считаться выполненной тогда, когда выполнится очередь B
// Очередь C будет возвращать то, что вернула очередь B
// 
// Если умножить две очереди A и B ("var C := A*B") — получится очередь C, в которой одновременно начнут выполняться A и B
// Очередь C будет считаться выполненной тогда, когда обе очереди (A и B) выполнятся
// Очередь C будет возвращать то, что вернула очередь B
// 
// Подробнее в примере "3 – Очередь\3.1\Сложение vs умножение очередей.pas"
// 
// Как и в математике, умножение имеет бОльший приоритет чем сложение
// 
// 
// Операторы += и *= также применимы к очередям
// И как и для чисел - "A += B" работает как "A := A+B" (и так же с *=)
// Это значит, что возвращаемые типы очередей A и B должны быть одинаковыми, чтобы к ним можно было применить +=/*=
// 
// 
// Если нужно сложить много очередей, лучше применять CombineSyncQueue
// Если нужно умножить много очередей — CombineAsyncQueue
// Эти подпрограммы работают немного быстрее чем сложение и умножение, если вы объединяете больше двух очередей
// 
// Кроме того, CombineSyncQueue и CombineAsyncQueue могут принимать ещё 1 параметр перед очередями
// Этот параметр позволяет указать функцию преобразования, которая использует результаты всех входных очередей
// Подробнее в примере "3 – Очередь\3.1\Использование результатов всех очередей.pas"
// 

// 3.1.4 —— CommandQueue.ThenConvert (Прикрепление очередей)
// 
// Результат очереди бывает необходимо преобразовать перед дальнейшим использованием
// Для этого используется метод .ThenConvert
// "q.ThenConvert(func)" работает так же как "q + HFQ(func)", с 1 поправкой:
// Функция, которую принимает .ThenConvert принимает результат предыдущей очереди в качестве параметра
// Подробнее в примере "3 – Очередь\3.1\Преобразование результата очереди.pas"
// 

// 3.1.5 —— CommandQueue.Cycle (Повторение очередей)
// 
// ToDo
// 

// 3.1.6 —— Неявное создание очередей (Передача по одной команде)
// 
// Передавать команды по одной, когда их несколько — ужасно медленно
// Но нередко бывает, что команда всего одна
// Или для отладки нужно как-то по простому одноразово вызвать одну команду
// 
// Для таких случаев можно создавать очереди неявно
// Это можно сделать вызвав метод переменной типа Buffer/Kernel
// 
// У каждого метода очереди, созданной с .NewQueue есть дублирующий метод в оригинальном объекте
// Этот метод создаёт новую очередь, добавляет одну соответствующую команду и выполняет полученную очередь методом Context.SyncInvoke
// Подробнее в примере "3 - Очередь\3.1\Код с очередью и без.pas"
// 
// Кроме того, у типа Buffer есть дополнительные методы "Buffer.Get..."
// Соответствующих методов у очередей — нет //ToDo возможно, в будущем появятся
// Методы ".Get..." создают новый объект типа записи, массива или выделяют область неуправляемой памяти,
// читают в полученный объект содержимое буфера и возвращают этот объект
// Они также используют неявную очередь (для чтения буфера)
// 

// 3.1.7 — Buffer.ValueQueue (Очередь из размерного значения, то есть записи)
// 
// Статический метод Buffer.ValueQueue создаёт новый буфер
// Затем создаёт из него новую очередь
// И добавляет в полученную очередь команду записи размерного значения в эту очередь
// 
// Это полезно, если вам, например, нужно передать одним из параметров в кёрнел размер передаваемого массива, так как массивы в C не хранят свой размер
// Но этим методом не стоит злоупотреблять, потому что каждый вызов создаёт новый буфер
// 
// Если вы запускаете кёрнел, в который нужно передавать число одним из параметров, лучше создайте буфер один раз
// и записывайте в него значение перед каждым вызовом кёрнела, используя .NewQueue.WriteValue
// 
// Если же вам нужна какая-то константа, то можно один раз вызвать Buffer.ValueQueue и далее использовать эту переменную (но не добавлять в неё команды)
// 

{$endregion 3.1. —— Создание очередей}

{$region 3.2 —— Выполнение очередей}

// 
// Самый простой способ выполнить очередь — через метод Context.SyncInvoke
// Он выполняет очередь и после того, как она завершилась, возвращает то, что вернула эта очередь
// 
// Но Context.SyncInvoke в свою очередь работает через метод Context.BeginInvoke
// метод Context.BeginInvoke начинает выполнение очереди и возвращает объект типа Task
// 

// У Task, так же как и у очереди — в <> указывается возвращаемое значение
// Через объект типа Task можно:
// — Следить за выполнением, через различные свойства типа Task
// — Ожидать пока выполнение не закончится методом Task.Wait
// — Получать возвращаемое значение после завершения выполнения, свойством Task.Result
// 

// Если при выполнении очереди возникает ошибка, о ней выведет не полную информацию
// Чтобы получить достаточно информации для того, чтобы понять что за ошибка возникла, используйте следующую конструкцию:
// <------------------------->
// try
//   
//   // ваш код, вызывающий ошибку
//   
// except
//   on e: Exception do writeln(e); // writeln выводит все внутренние исключения, поэтому в нём видно что произошло на самом деле
// end;
// <------------------------->
// Для данного кода есть стандартный снипет
// Чтобы активировать его - напишите "tryo" и нажмите Shift+Space
// 

{$endregion 3.2 —— Выполнение очередей}

{$region 3.3 —— Очереди как параметры}

// 
// Почти все параметры всех методов, создающих очереди (в том числе и неявные)
// Могут принимать вместо любого параметра очередь
// (анализатор кода об этом не говорит, чтобы не было слишком много перегрузок каждого метода)
// Но в таком случае, передаваемая очередь должна возвращать то, что принимает параметр
// Подробнее в примере "3 – Очередь\3.3\Использование очереди как парамметра.pas"
// 
// Очереди, переданные параметрами, выполняются в непредсказуемом порядке, но подчиняются следующим правилам:
// 1. Все очереди-параметры начинают выполняться прямо при вызове Context.BeginInvoke
// 2. Все очереди-параметры команды A выполнятся до того как начнёт выполняться сама команда A
// 
// Также, никто не запрещает передавать очередь-параметр в метод,
// создающий команду, которая сама является частью другой очереди-параметра
// В этом случае действуют все те же правила
// 

{$endregion 3.3 —— Очереди как параметры}

{$region 3.4. —— Множественное использование очереди}

// 3.4.0
// 
// Одну и ту же очередь можно использовать несколько раз:
// <------------------------->
// var Q1: CommandQueue<...>;
// ...
// Context.Default.SyncInvoke(Q1);
// Context.Default.SyncInvoke(Q1);
// <------------------------->
// 
// Однако, во время выполнения очередь хранит в себе данные о своём состоянии выполнения и результат, когда он уже вычислен
// Это значит, что 1 объект очереди нельзя выполнять в 2 местах параллельно
// Иначе данные о состоянии и результате перемешаются
// То есть, такой код:
// <------------------------->
// var Q1: CommandQueue<...>;
// ...
// Context.Default.SyncInvoke( Q1 * Q1 );
// <------------------------->
// Приведёт к неопределённом поведению
// То есть, результат выполнения может быть неправильный, но не при каждом запуске
// 
// Также, в некоторых случаях может быть вызвано исключение QueueDoubleInvokeException, помогающее узнать, где именно возник параллельный вызов
// Но не стоит полагаться на вызов этого исключения
// 
// Такую ошибку в коде может быть черезвычайно сложно распознать
// Поэтому необходимо внимательно проверять, чтобы одна очередь не выполнялась нескольких местах одновременно
// 
// Однако, если вам всё же надо использовать одну очередь в нескольких местах одновременно - есть 2 способа:

// 3.4.1 — Клонирование очередей (.Clone)
// 
// Методом CommandQueue.Clone можно создать полную копию очереди
// При этом, если исходная очередь проводила какие то вычисления,
// они будут произведены дважды, оригиналом и копией, при вызове обоих
// 
// Клон очереди является полностью независимым объектом
// Его можно вызывать не только параллельно с оригиналом
// Оригинал и копию можно вызывать даже в двух разных Context.BeginInvoke одновременно
// 

// 3.4.2 — Удлинители для очередей (.Multiusable)
// 
// Вариант выполнять очередь несколько раз, как в случае с .Clone, нередко не подходит,
// потому что это удваивает затраты производительности
// И некоторые очереди (например, выполнения кёрнелов) могут дать разные результаты, если выполнить их лишний раз
// 
// Если вам нужно использовать результат одной очереди многократно, лучше использовать метод CommandQueue.Multiusable
// !! Созданные таким образом очереди НЕ является независимыми объектами !!
// 
// .Multiusable работает словно провод-удлинитель,
// если сравнивать возвращаемое значение очереди с розеткой
// 
// Исходная очередь, для которой вызвали .Multiusable, подчиняется всё тем же правилам, что и очередь-параметр
// То есть, она начинает выполняться во время вызова Context.BeginInvoke
// И она всегда выполнится до того, как начнёт выполняться любая из очередей, которую вернул метод .Multiusable
// 
// Очереди, которые вернул .Multiusable могут быть использованы параллельно:
// <------------------------->
// var Q1: CommandQueue<...>;
// ...
// Qs1 := Q1.Multiusable(3); // создаёт массив из 3 очередей
// Context.Default.SyncInvoke( Qs1[0] * Qs1[1] * Qs1[2].ThenConvert(o->...) );
// <------------------------->
// Однако, все очереди, полученные из .Multiusable всё ещё связаны оригинальной очередью
// А так как Context.BeginInvoke управляет переключением состояния выполнения очереди
// Следующий код два раза запустит очередь Q1, и после этого два раза её завершит:
// <------------------------->
// var Q1: CommandQueue<...>;
// ...
// Qs1 := Q1.Multiusable(2);
// Context.Default.BeginInvoke( Qs1[0] );
// Context.Default.BeginInvoke( Qs1[1] );
// <------------------------->
// Из-за чего такой код тоже приведёт к неопределённом поведению
// Но вероятность получить QueueDoubleInvokeException в данном случае - ещё меньше
// Чтобы исправить такой код - надо объеденить вызовы метода Context.BeginInvoke
// 

{$endregion 3.4. —— Множественное использование очереди}

{$endregion 3. Очередь [команд] (CommandQueue)}

{$region 4 —— Буфер (Buffer)}

// 
// Буфер создаётся через конструктор ("new Buffer(...)")
// Однако если не передать в конструктор контекст,
// память на GPU выделяется только при вызове метода Buffer.Init
// Исходя из контекста, переданного Buffer.Init, выбирается на каком устройстве будет выделена память
// 
// При выделения памяти содержимое буфера НЕ очищается нулями, а значит содержит мусорные данные
// Повторный вызов Buffer.Init перевыделяет память
// 
// Если Buffer.Init НЕ был вызван до первой операции чтения/записи буфера, он будет вызван автоматически
// В таком случае в качестве контекста для выделения памяти выбирается тот, для которого был вызван метод Context.BeginInvoke
// 
// Буфер можно удалить методом Buffer.Dispose
// Но этот метод только освобождает память на GPU
// Если после .Dispose использовать буфер снова, память выделится заново
// .Dispose также вызовется автоматически, когда в программе не остаётся ссылок на буфер
// 

{$endregion 4 —— Буфер (Buffer)}

{$region 5. Контейнер для кода (ProgramCode)}

{$region 5.1. —— Создание ProgramCode}

// 5.1.1 — Создание из исходного кода
// 
// Конструктор ProgramCode("new ProgramCode(...)") принимает текст исходников программы на языке OpenCL-C
// 
// Так же как исходники паскаля хранятся в .pas файлах
// Исходники OpenCL-C кода обычно хранят в .cl файлах
// Однако это не принципиально, потому что код не обязательно должен быть в файле,
// Он может быть в любой текстовой форме (в том числе в строке в .pas программе)
// Тем не менее, хранение исходников OpenCL-C кода в .cl-файлах упрощает жизнь, потому что тогда их легко найти
// 

// 5.1.2 — Создание из бинарных файлов
// 
// После создания объекта типа ProgramCode из исходников
// Можно вызвать метод ProgramCode.SerializeTo, чтобы сохранить код в бинарном и прекомпилированном виде
// Это обычно делается отдельной программой (не той же самой, которая будет использовать этот бинарный код)
// 
// После этого основная программа может создать объект ProgramCode
// Используя статический метод ProgramCode.DeserializeFrom
// 
// Подробнее в примере "5 – ProgramCode\Прекомпиляция исходников OpenCL-C"
// 

{$endregion 5.1. —— Создание ProgramCode}

{$endregion 5. Контейнер для кода (ProgramCode)}

{$region 6 —— Кёрнел (Kernel)}

// 
// Кёрнел создаётся через индексное свойтсво ProgramCode:
// code['KernelName']
// Где code имеет тип ProgramCode, а KernelName — имя подпрограммы-кёрнела в исходном коде (регистр важен!)
// 
// Кёрнел вызывается методом KernelCommandQueue.Exec
// См. пример "1.6 - Кёрнел\Вызов кёрнела.pas"
// 

{$endregion 6 —— Кёрнел (Kernel)}

{$endregion Подробное описание OpenCLABC}

interface

uses OpenCL;
uses System;
uses System.Threading.Tasks;
uses System.Runtime.InteropServices;
uses System.Runtime.CompilerServices;

{$region ToDo}

//===================================
// Обязательно сделать до следующего пула:

//ToDo мультиюзбл хаб не должен использоваться для обычных Buffer.NewQueue

//ToDo разбить BufferCommandCopy на 2 типа +1 базовый, чтоб не использовать лишнюю очередь

//ToDo Написать в справке про implicit, типа способ создать очередь
//ToDo Написать в справке про CommandQueueBase
//ToDo Написать в справке про AddWait,AddProc,AddQueue вместе
//ToDo Написать в справке про WaitFor

//===================================
// Запланированное:

//ToDo DummyQueue.Multiusable может тупо возвращать себя несколько раз

//ToDo система создания описаний через отдельные файлы

//ToDo CommandQueue.Cycle(integer)
//ToDo CommandQueue.Cycle // бесконечность циклов
//ToDo CommandQueue.CycleWhile(***->boolean)
// - возможность передать свой обработчик ошибок как Exception->Exception
//
//Update:
// - бесконечный цикл будет больно делать
// - чтобы не накапливались Task-и - надо полностью перезапускать очередь
// - а значит надо что то вроде пре-запуска, чтобы не терять время между итерациями

//ToDo Read/Write для массивов - надо бы иметь возможность указывать отступ в массиве

//ToDo Типы Device и Platform
//ToDo А связь с OpenCL.pas сделать всему (и буферам и кёрнелам), но более человеческую

//ToDo Сделать методы BufferCommandQueue.AddGet
// - они особенные, потому что возвращают не BufferCommandQueue, а каждый свою очередь
// - полезно, потому что SyncInvoke такой очереди будет возвращать полученное значение

//ToDo Интегрировать профайлинг очередей

//===================================
// Сделать когда-нибуть:

//ToDo У всего, у чего есть Finalize - проверить чтобы было и .Dispose, если надо
// - и добавить в справку, про то что этот объект можно удалять

//ToDo Пройтись по всем функциям OpenCL, посмотреть функционал каких не доступен из OpenCLABC

//ToDo Тесты всех фич модуля

//===================================

//ToDo issue компилятора:
// - #1981
// - #2048
// - #2067
// - #2068
// - #2118
// - #2119
// - #2120

{$endregion ToDo}

type
  
  {$region misc class def}
  
  CommandQueue<T> = class;
  Context = class;
  Buffer = class;
  Kernel = class;
  ProgramCode = class;
  DeviceTypeFlags = OpenCL.DeviceTypeFlags;
  
  QueueDoubleInvokeException = class(Exception)
    
    public constructor :=
    inherited Create('Нельзя выполнять одну и ту же очередь в 2 местах одновременно. Используйте .Clone или .Multiusable');
    
  end;
  
  ///--
  __SafeNativQueue = sealed class
    q: cl_command_queue;
    
    constructor := raise new InvalidOperationException;
    
    constructor(c: cl_context; dvc: cl_device_id);
    begin
      var ec: ErrorCode;
      self.q := cl.CreateCommandQueue(c, dvc, CommandQueuePropertyFlags.NONE, ec);
      ec.RaiseIfError;
    end;
    
    protected procedure Finalize; override :=
    cl.ReleaseCommandQueue(self.q).RaiseIfError;
    
  end;
  
  {$endregion misc class def}
  
  {$region CommandQueue}
  
  CommandQueueBase = abstract class
    protected ev, mw_ev: cl_event;
    protected is_busy: boolean;
    
    {$region Queue converters}
    
    {$region DummyQueue}
    
    public static function operator implicit(o: object): CommandQueueBase;
    
    {$endregion DummyQueue}
    
    {$region Mutiusable}
    
    //ToDo #2120
//    ///Создаёт массив из n очередей, каждая из которых возвращает результат данной очереди
//    ///Каждую полученную очередь можно использовать одновременно с другими, но только в общей очереди
//    public function Multiusable(n: integer): array of CommandQueueBase;
//    
//    ///Создаёт функцию, создающую очередь, которая возвращает результат данной очереди
//    ///Каждую очередь, созданную полученной функцией, можно использовать одновременно с другими, но только в общей очереди
//    public function Multiusable: ()->CommandQueueBase;
    
    {$endregion Mutiusable}
    
    {$region ThenConvert}
    
    //ToDo #2118
//    ///Создаёт очередь, которая выполнит данную
//    ///А затем выполнит на CPU функцию f, используя результат данной очереди
//    public function ThenConvert<T>(f: object->T): CommandQueue<T> :=
//    self.ThenConvert((o,c)->f(o));
//    ///Создаёт очередь, которая выполнит данную
//    ///А затем выполнит на CPU функцию f, используя результат данной очереди и контекст на котором её выполнили
//    public function ThenConvert<T>(f: (object,Context)->T): CommandQueue<T>;
    
    {$endregion ThenConvert}
    
    {$region WaitFor}
    
    {$endregion WaitFor}
    
    {$region [A]SyncQueue}
    
    public static function operator+(q1, q2: CommandQueueBase): CommandQueueBase;
    public static function operator+<T>(q1: CommandQueueBase; q2: CommandQueue<T>): CommandQueue<T>;
    public static procedure operator+=(var q1: CommandQueueBase; q2: CommandQueueBase) := q1 := q1+q2;
    
    public static function operator*(q1, q2: CommandQueueBase): CommandQueueBase;
    public static function operator*<T>(q1: CommandQueueBase; q2: CommandQueue<T>): CommandQueue<T>;
    public static procedure operator*=(var q1: CommandQueueBase; q2: CommandQueueBase) := q1 := q1*q2;
    
    {$endregion [A]SyncQueue}
    
    {$endregion Queue converters}
    
    {$region def}
    
    protected function GetEstimateTaskCount(prev_hubs: HashSet<object>): integer; abstract;
    
    protected procedure Invoke(c: Context; var cq: __SafeNativQueue; prev_ev: cl_event; tasks: List<Task>); abstract;
    
    protected procedure UnInvoke; virtual;
    begin
      
      if self.is_busy then
        is_busy := false else
        raise new InvalidOperationException('Ошибка внутри модуля OpenCLABC: совершена попыта завершить не запущенную очередь. Сообщите, пожалуйста, разработчику OpenCLABC');
      
    end;
    
    protected function InternalClone(muhs: Dictionary<object, object>; cache: Dictionary<CommandQueueBase, CommandQueueBase>): CommandQueueBase; abstract;
    
    {$endregion def}
    
    {$region Utils}
    
    {$region Misc}
    
    protected procedure MakeBusy := lock self do
    if not self.is_busy then is_busy := true else
      raise new QueueDoubleInvokeException;
    
    protected function GetRes: object; abstract;
    
    {$endregion Misc}
    
    {$region Event's}
    
    protected static procedure WaitAndRelease(ev: cl_event);
    begin
      if ev=cl_event.Zero then exit;
      cl.WaitForEvents(1, @ev).RaiseIfError;
      cl.ReleaseEvent(ev).RaiseIfError;
    end;
    
    ///evs.Count<>0, не 1 из ивентов не должен быть Zero
    protected static procedure WaitAndRelease(evs: List<cl_event>);
    begin
      cl.WaitForEvents(evs.Count, evs.ToArray).RaiseIfError;
      foreach var ev in evs do cl.ReleaseEvent(ev).RaiseIfError;
    end;
    
    protected function GetMWEvent(c: cl_context): cl_event;
    begin
      lock self do
      begin
        if is_busy then
        begin
          Result := self.ev;
          cl.RetainEvent(Result).RaiseIfError;
          exit;
        end;
        
        if self.mw_ev=cl_event.Zero then
        begin
          var ec: ErrorCode;
          self.mw_ev := cl.CreateUserEvent(c, ec);
          ec.RaiseIfError;
        end else
          cl.RetainEvent(self.mw_ev).RaiseIfError;
        
        Result := self.mw_ev;
      end;
    end;
    
    protected procedure SignalMWEvent;
    begin
      if self.mw_ev=cl_event.Zero then exit;
      cl.SetUserEventStatus(self.mw_ev, CommandExecutionStatus.COMPLETE);
      self.mw_ev := cl_event.Zero;
    end;
    
    {$endregion Event's}
    
    {$region Invoke}
    
    protected procedure InvokeNewQ(c: Context; tasks: List<Task>);
    begin
      var cq: __SafeNativQueue := nil;
      Invoke(c, cq, cl_event.Zero, tasks);
    end;
    
    {$endregion Invoke}
    
    {$region Clone}
    
    protected function InternalCloneCached(muhs: Dictionary<object, object>; cache: Dictionary<CommandQueueBase, CommandQueueBase>): CommandQueueBase;
    begin
      if cache.TryGetValue(self, Result) then exit;
      Result := InternalClone(muhs, cache);
      cache.Add(self, Result);
    end;
    
    {$endregion Clone}
    
    {$endregion Utils}
    
  end;
  /// Базовый тип всех очередей команд в OpenCLABC
  CommandQueue<T> = abstract class(CommandQueueBase)
    protected res: T;
    
    {$region Misc}
    
    protected function GetRes: object; override := self.res;
    
    ///Создаёт полную копию данной очереди,
    ///Всех очередей из которых она состоит,
    ///А так же всех очередей-параметров, использованных в данной очереди
    public function Clone := self.InternalClone(new Dictionary<object,object>, new Dictionary<CommandQueueBase,CommandQueueBase>) as CommandQueue<T>;
    
    {$endregion Misc}
    
    {$region Queue converters}
    
    {$region DummyQueue}
    
    public static function operator implicit(o: T): CommandQueue<T>;
    
    {$endregion DummyQueue}
    
    {$region Mutiusable}
    
    ///Создаёт массив из n очередей, каждая из которых возвращает результат данной очереди
    ///Каждую полученную очередь можно использовать одновременно с другими, но только в общей очереди
    public function Multiusable(n: integer): array of CommandQueue<T>;
    
    ///Создаёт функцию, создающую очередь, которая возвращает результат данной очереди
    ///Каждую очередь, созданную полученной функцией, можно использовать одновременно с другими, но только в общей очереди
    public function Multiusable: ()->CommandQueue<T>;
    
    {$endregion Mutiusable}
    
    {$region ThenConvert}
    
    ///Создаёт очередь, которая выполнит данную
    ///А затем выполнит на CPU функцию f, используя результат данной очереди
    public function ThenConvert<T2>(f: T->T2): CommandQueue<T2> :=
    self.ThenConvert((o,c)->f(o));
    ///Создаёт очередь, которая выполнит данную
    ///А затем выполнит на CPU функцию f, используя результат данной очереди и контекст на котором её выполнили
    public function ThenConvert<T2>(f: (T,Context)->T2): CommandQueue<T2>;
    
    {$endregion ThenConvert}
    
    {$region WaitFor}
    
    public function WaitFor(q: CommandQueueBase; allow_q_cloning: boolean := true): CommandQueue<T>;
    
    {$endregion WaitFor}
    
    {$region [A]SyncQueue}
    
    public static function operator+<T2>(q1: CommandQueue<T>; q2: CommandQueue<T2>): CommandQueue<T2>;
    public static procedure operator+=(var q1: CommandQueue<T>; q2: CommandQueue<T>) := q1 := q1+q2;
    
    public static function operator*<T2>(q1: CommandQueue<T>; q2: CommandQueue<T2>): CommandQueue<T2>;
    public static procedure operator*=(var q1: CommandQueue<T>; q2: CommandQueue<T>) := q1 := q1*q2;
    
    {$endregion [A]SyncQueue}
    
    {$endregion Queue converters}
    
  end;
  
  {$endregion CommandQueue}
  
  {$region GPUCommand}
  
  ///--
  GPUCommand<T> = abstract class
    protected ev: cl_event;
    
    {$region Command def}
    
    protected function GetEstimateTaskCount(prev_hubs: HashSet<object>): integer; abstract;
    
    protected procedure Invoke(o_q: CommandQueue<T>; o: T; c: Context; var cq: __SafeNativQueue; prev_ev: cl_event; tasks: List<Task>); abstract;
    
    protected procedure UnInvoke; abstract;
    
    protected function Clone(muhs: Dictionary<object, object>; cache: Dictionary<CommandQueueBase, CommandQueueBase>): GPUCommand<T>; abstract;
    
    {$endregion Command def}
    
    {$region Utils}
    
    protected static procedure WaitAndRelease(ev: cl_event) :=
    CommandQueueBase.WaitAndRelease(ev);
    
    ///evs.Count<>0, не 1 из ивентов не должен быть Zero
    protected static procedure WaitAndRelease(evs: List<cl_event>) :=
    CommandQueueBase.WaitAndRelease(evs);
    
    {$endregion Utils}
    
  end;
  
  ///--
  GPUCommandContainer<T> = abstract class(CommandQueue<T>)
    protected res_q_hub: object;
    protected last_center_plug: CommandQueueBase;
    protected commands := new List<GPUCommand<T>>;
    
    {$region def}
    
    protected procedure OnEarlyInit(c: Context); virtual := exit;
    
    {$endregion def}
    
    {$region Common}
    
    protected constructor(o: T) := self.res := o;
    protected constructor(q: CommandQueue<T>);
    
    protected function GetNewResPlug: CommandQueue<T>;
    
    protected procedure InternalAddQueue(q: CommandQueueBase);
    protected procedure InternalAddProc(p: (T,Context)->());
    protected procedure InternalAddWait(q: CommandQueueBase; allow_q_cloning: boolean);
    
    {$endregion Common}
    
    {$region sub implementation}
    
    protected function GetEstimateTaskCount(prev_hubs: HashSet<object>): integer; override;
    
    private procedure CLSignalMWEvent(ev: cl_event; status: CommandExecutionStatus; data: pointer) := self.SignalMWEvent;
    
    protected procedure Invoke(c: Context; var cq: __SafeNativQueue; prev_ev: cl_event; tasks: List<Task>); override;
    
    protected procedure UnInvoke; override;
    begin
//      inherited; // не надо, команды уже удалили свои эвенты
      self.ev := cl_event.Zero;
      if last_center_plug<>nil then
      begin
        last_center_plug.UnInvoke;
        last_center_plug := nil;
      end;
      foreach var comm in commands do comm.UnInvoke;
    end;
    
    {$endregion sub implementation}
    
    {$region reintroduce методы}
    
    private function Equals(obj: object): boolean; reintroduce := false;
    
    private function ToString: string; reintroduce := nil;
    
    private function GetType: System.Type; reintroduce := nil;
    
    private function GetHashCode: integer; reintroduce := 0;
    
    {$endregion reintroduce методы}
    
  end;
  
  {$endregion GPUCommand}
  
  {$region Buffer}
  
  ///Особый тип очереди, всегда возвращающий Buffer
  ///Может быть создан из объекта Buffer или очереди, возвращающей Buffer
  ///Используется для хранения списка особых команд, применимых только к Buffer
  BufferCommandQueue = sealed class(GPUCommandContainer<Buffer>)
    
    {$region constructor's}
    
    ///Создаёт объект BufferCommandQueue, команды которого будут применятся к буферу b
    public constructor(b: Buffer) := inherited Create(b);
    ///Создаёт объект BufferCommandQueue, команды которого будут применятся к буферу, который будет результатом q
    public constructor(q: CommandQueue<Buffer>);
    
    {$endregion constructor's}
    
    {$region Utils}
    
    protected function AddCommand(comm: GPUCommand<Buffer>): BufferCommandQueue;
    begin
      self.commands += comm;
      Result := self;
    end;
    
    protected function GetSizeQ: CommandQueue<integer>;
    
    public function Clone: BufferCommandQueue := inherited Clone as BufferCommandQueue;
    
    {$endregion Utils}
    
    {$region Write}
    
    ///- function WriteData(ptr: IntPtr): BufferCommandQueue;
    ///Копирует область оперативной памяти, на которую ссылается ptr, в данный буфер
    ///Копируется нужное кол-во байт чтобы заполнить весь буфер
    public function AddWriteData(ptr: CommandQueue<IntPtr>): BufferCommandQueue := AddWriteData(ptr, 0,GetSizeQ);
    ///- function WriteData(ptr: IntPtr; offset, len: integer): BufferCommandQueue;
    ///Копирует область оперативной памяти, на которую ссылается ptr, в данный буфер
    ///offset это отступ в буфере, а len - кол-во копируемых байтов
    public function AddWriteData(ptr: CommandQueue<IntPtr>; offset, len: CommandQueue<integer>): BufferCommandQueue;
    
    ///- function WriteData(ptr: pointer): BufferCommandQueue;
    ///Копирует область оперативной памяти, на которую ссылается ptr, в данный буфер
    ///Копируется нужное кол-во байт чтобы заполнить весь буфер
    public function AddWriteData(ptr: pointer) := AddWriteData(IntPtr(ptr));
    ///- function WriteData(ptr: pointer; offset, len: integer): BufferCommandQueue;
    ///Копирует область оперативной памяти, на которую ссылается ptr, в данный буфер
    ///offset это отступ в буфере, а len - кол-во копируемых байтов
    public function AddWriteData(ptr: pointer; offset, len: CommandQueue<integer>) := AddWriteData(IntPtr(ptr), offset, len);
    
    
    ///- function WriteArray(a: Array): BufferCommandQueue;
    ///Копирует содержимое массива в данный буфер
    ///Копируется нужное кол-во байт чтобы заполнить весь буфер
    public function AddWriteArray(a: CommandQueue<&Array>): BufferCommandQueue := AddWriteArray(a, 0,GetSizeQ);
    ///- function WriteArray(a: Array; offset, len: integer): BufferCommandQueue;
    ///Копирует содержимое массива в данный буфер
    ///offset это отступ в буфере, а len - кол-во копируемых байтов
    public function AddWriteArray(a: CommandQueue<&Array>; offset, len: CommandQueue<integer>): BufferCommandQueue;
    
    ///- function WriteArray(a: Array): BufferCommandQueue;
    ///Копирует содержимое массива в данный буфер
    ///Копируется нужное кол-во байт чтобы заполнить весь буфер
    public function AddWriteArray(a: &Array) := AddWriteArray(CommandQueue&<&Array>(a));
    ///- function WriteArray(a: Array; offset, len: integer): BufferCommandQueue;
    ///Копирует содержимое массива в данный буфер
    ///offset это отступ в буфере, а len - кол-во копируемых байтов
    public function AddWriteArray(a: &Array; offset, len: CommandQueue<integer>) := AddWriteArray(CommandQueue&<&Array>(a), offset, len);
    
    
    ///- function WriteValue<TRecord>(val: TRecord; offset: integer := 0): BufferCommandQueue; where TRecord: record;
    ///Записывает значение любого размерного типа в данный буфер
    ///С отступом в offset байт в буфере
    public [MethodImpl(MethodImplOptions.AggressiveInlining)] function AddWriteValue<TRecord>(val: TRecord; offset: CommandQueue<integer> := 0): BufferCommandQueue; where TRecord: record;
    
    ///- function WriteValue<TRecord>(val: TRecord; offset: integer := 0): BufferCommandQueue; where TRecord: record;
    ///Записывает значение любого размерного типа в данный буфер
    ///С отступом в offset байт в буфере
    public function AddWriteValue<TRecord>(val: CommandQueue<TRecord>; offset: CommandQueue<integer> := 0): BufferCommandQueue; where TRecord: record;
    
    {$endregion Write}
    
    {$region Read}
    
    ///- function ReadData(ptr: IntPtr): BufferCommandQueue;
    ///Копирует всё содержимое буффера в область оперативной памяти, на которую указывает ptr
    public function AddReadData(ptr: CommandQueue<IntPtr>): BufferCommandQueue := AddReadData(ptr, 0,GetSizeQ);
    ///- function ReadData(ptr: IntPtr; offset, len: integer): BufferCommandQueue;
    ///Копирует len байт, начиная с байта №offset в буфере, в область оперативной памяти, на которую указывает ptr
    public function AddReadData(ptr: CommandQueue<IntPtr>; offset, len: CommandQueue<integer>): BufferCommandQueue;
    
    ///- function ReadData(ptr: pointer): BufferCommandQueue;
    ///Копирует всё содержимое буффера в область оперативной памяти, на которую указывает ptr
    public function AddReadData(ptr: pointer) := AddReadData(IntPtr(ptr));
    ///- function ReadData(ptr: pointer; offset, len: integer): BufferCommandQueue;
    ///Копирует len байт, начиная с байта №offset в буфере, в область оперативной памяти, на которую указывает ptr
    public function AddReadData(ptr: pointer; offset, len: CommandQueue<integer>) := AddReadData(IntPtr(ptr), offset, len);
    
    ///- function ReadArray(a: Array): BufferCommandQueue;
    ///Копирует всё содержимое буффера в содержимое массива
    public function AddReadArray(a: CommandQueue<&Array>): BufferCommandQueue := AddReadArray(a, 0,GetSizeQ);
    ///- function ReadArray(a: Array; offset, len: integer): BufferCommandQueue;
    ///Копирует len байт, начиная с байта №offset в буфере, в содержимое массива
    public function AddReadArray(a: CommandQueue<&Array>; offset, len: CommandQueue<integer>): BufferCommandQueue;
    
    ///- function ReadArray(a: Array): BufferCommandQueue;
    ///Копирует всё содержимое буффера в содержимое массива
    public function AddReadArray(a: &Array) := AddReadArray(CommandQueue&<&Array>(a));
    ///- function ReadArray(a: Array; offset, len: integer): BufferCommandQueue;
    ///Копирует len байт, начиная с байта №offset в буфере, в содержимое массива
    public function AddReadArray(a: &Array; offset, len: CommandQueue<integer>) := AddReadArray(CommandQueue&<&Array>(a), offset, len);
    
    ///- function ReadValue<TRecord>(var val: TRecord; offset: integer := 0): BufferCommandQueue; where TRecord: record;
    ///Читает значение любого размерного типа из данного буфера
    ///С отступом в offset байт в буфере
    public function AddReadValue<TRecord>(var val: TRecord; offset: CommandQueue<integer> := 0): BufferCommandQueue; where TRecord: record;
    begin
      Result := AddReadData(@val, offset, Marshal.SizeOf&<TRecord>);
    end;
    
    {$endregion Read}
    
    {$region Fill}
    
    ///- function PatternFill(ptr: IntPtr): BufferCommandQueue;
    ///Заполняет весь буфер копиями массива байт, длинной pattern_len,
    ///прочитанным из области оперативной памяти, на которую указывает ptr
    public function AddFillData(ptr: CommandQueue<IntPtr>; pattern_len: CommandQueue<integer>): BufferCommandQueue := AddFillData(ptr,pattern_len, 0,GetSizeQ);
    ///- function PatternFill(ptr: IntPtr; offset, len: integer): BufferCommandQueue;
    ///Заполняет часть буфера (начиная с байта №offset и длинной len) копиями массива байт, длинной pattern_len,
    ///прочитанным из области оперативной памяти, на которую указывает ptr
    public function AddFillData(ptr: CommandQueue<IntPtr>; pattern_len, offset, len: CommandQueue<integer>): BufferCommandQueue;
    
    ///- function PatternFill(ptr: pointer): BufferCommandQueue;
    ///Заполняет весь буфер копиями массива байт, длинной pattern_len,
    ///прочитанным из области оперативной памяти, на которую указывает ptr
    public function AddFillData(ptr: pointer; pattern_len: CommandQueue<integer>) := AddFillData(IntPtr(ptr), pattern_len);
    ///- function PatternFill(ptr: pointer; offset, len: integer): BufferCommandQueue;
    ///Заполняет часть буфера (начиная с байта №offset и длинной len) копиями массива байт, длинной pattern_len,
    ///прочитанным из области оперативной памяти, на которую указывает ptr
    public function AddFillData(ptr: pointer; pattern_len, offset, len: CommandQueue<integer>) := AddFillData(IntPtr(ptr), pattern_len, offset, len);
    
    ///- function PatternFill(a: Array): BufferCommandQueue;
    ///Заполняет весь буфер копиями содержимого массива
    public function AddFillArray(a: CommandQueue<&Array>): BufferCommandQueue := AddFillArray(a, 0,GetSizeQ);
    ///- function PatternFill(a: Array; offset, len: integer): BufferCommandQueue;
    ///Заполняет часть буфера (начиная с байта №offset и длинной len) копиями содержимого массива
    public function AddFillArray(a: CommandQueue<&Array>; offset, len: CommandQueue<integer>): BufferCommandQueue;
    
    ///- function PatternFill(a: Array): BufferCommandQueue;
    ///Заполняет весь буфер копиями содержимого массива
    public function AddFillArray(a: &Array) := AddFillArray(CommandQueue&<&Array>(a));
    ///- function PatternFill(a: Array; offset, len: integer): BufferCommandQueue;
    ///Заполняет часть буфера (начиная с байта №offset и длинной len) копиями содержимого массива
    public function AddFillArray(a: &Array; offset, len: CommandQueue<integer>) := AddFillArray(CommandQueue&<&Array>(a), offset, len);
    
    ///- function PatternFill<TRecord>(val: TRecord): BufferCommandQueue; where TRecord: record;
    ///Заполняет весь буфер копиями значения любого размерного типа
    public [MethodImpl(MethodImplOptions.AggressiveInlining)] function AddFillValue<TRecord>(val: TRecord): BufferCommandQueue; where TRecord: record;
    begin Result := AddFillValue(val, 0,GetSizeQ); end;
    ///- function PatternFill<TRecord>(val: TRecord; offset, len: integer): BufferCommandQueue; where TRecord: record;
    ///Заполняет часть буфера (начиная с байта №offset и длинной len) копиями значения любого размерного типа
    public [MethodImpl(MethodImplOptions.AggressiveInlining)] function AddFillValue<TRecord>(val: TRecord; offset, len: CommandQueue<integer>): BufferCommandQueue; where TRecord: record;
    
    ///- function PatternFill<TRecord>(val: TRecord): BufferCommandQueue; where TRecord: record;
    ///Заполняет весь буфер копиями значения любого размерного типа
    public function AddFillValue<TRecord>(val: CommandQueue<TRecord>): BufferCommandQueue; where TRecord: record;
    begin Result := AddFillValue(val, 0,GetSizeQ); end;
    ///- function PatternFill<TRecord>(val: TRecord; offset, len: integer): BufferCommandQueue; where TRecord: record;
    ///Заполняет часть буфера (начиная с байта №offset и длинной len) копиями значения любого размерного типа
    public function AddFillValue<TRecord>(val: CommandQueue<TRecord>; offset, len: CommandQueue<integer>): BufferCommandQueue; where TRecord: record;
    
    {$endregion Fill}
    
    {$region Copy}
    
    ///- function CopyFrom(b: Buffer; from, &to, len: integer): BufferCommandQueue;
    ///Копирует содержимое буфера b в данный буфер
    ///from - отступ в буффере b
    ///to   - отступ в данном буффере
    ///len  - кол-во копируемых байт
    public function AddCopyFrom(b: CommandQueue<Buffer>; from, &to, len: CommandQueue<integer>): BufferCommandQueue;
    ///- function CopyTo(b: Buffer; from, &to, len: integer): BufferCommandQueue;
    ///Копирует содержимое данного буфера в буфер b
    ///from - отступ в данном буффере
    ///to   - отступ в буффере b
    ///len  - кол-во копируемых байт
    public function AddCopyTo  (b: CommandQueue<Buffer>; from, &to, len: CommandQueue<integer>): BufferCommandQueue;
    
    ///- function CopyFrom(b: Buffer): BufferCommandQueue;
    ///Копирует всё содержимое буфера b в данный буфер
    public function AddCopyFrom(b: CommandQueue<Buffer>) := AddCopyFrom(b, 0,0, GetSizeQ);
    ///- function CopyTo(b: Buffer): BufferCommandQueue;
    ///Копирует всё содержимое данного буфера в буфер b
    public function AddCopyTo  (b: CommandQueue<Buffer>) := AddCopyTo  (b, 0,0, GetSizeQ);
    
    {$endregion Copy}
    
    {$region Non-command add's}
    
    public function AddQueue(q: CommandQueueBase): BufferCommandQueue;
    begin
      InternalAddQueue(q);
      Result := self;
    end;
    
    public function AddProc(p: (Buffer,Context)->()): BufferCommandQueue;
    begin
      InternalAddProc(p);
      Result := self;
    end;
    public function AddProc(p: Buffer->()) := AddProc((b,c)->p(b));
    
    public function AddWait(q: CommandQueueBase; allow_q_cloning: boolean := true): BufferCommandQueue;
    begin
      InternalAddWait(q, allow_q_cloning);
      Result := self;
    end;
    
    {$endregion Non-command add's}
    
    {$region override методы}
    
    protected procedure OnEarlyInit(c: Context); override;
    
    protected function InternalClone(muhs: Dictionary<object, object>; cache: Dictionary<CommandQueueBase, CommandQueueBase>): CommandQueueBase; override;
    
    {$endregion override методы}
    
  end;
  
  ///Буфер, хранящий своё содержимое в памяти GPU (обычно)
  ///Используется для передачи данных в Kernel-ы перед их выполнением
  Buffer = sealed class(IDisposable)
    private memobj: cl_mem;
    private sz: UIntPtr;
    private _parent: Buffer;
    
    {$region constructor's}
    
    private constructor := raise new System.NotSupportedException;
    
    ///Создаён не_инициализированный буфер с размером size байт
    public constructor(size: UIntPtr) := self.sz := size;
    ///Создаён не_инициализированный буфер с размером size байт
    public constructor(size: integer) := Create(new UIntPtr(size));
    ///Создаён не_инициализированный буфер с размером size байт
    public constructor(size: int64)   := Create(new UIntPtr(size));
    
    ///Создаён инициализированный в контексте "c" буфер с размером size байт
    public constructor(size: UIntPtr; c: Context);
    begin
      Create(size);
      Init(c);
    end;
    ///Создаён инициализированный в контексте "c" буфер с размером size байт
    public constructor(size: integer; c: Context) := Create(new UIntPtr(size), c);
    ///Создаён инициализированный в контексте "c" буфер с размером size байт
    public constructor(size: int64; c: Context)   := Create(new UIntPtr(size), c);
    
    ///Создаёт под-буфер размера size и с отступом в данном буфере offset
    ///Под буфер имеет общую память с оригинальным, но иммеет доступ только к её части
    public function SubBuff(offset, size: integer): Buffer; 
    
    ///Инициализирует буфер, выделяя память на девайсе - который связан с данным контекстом
    public procedure Init(c: Context);
    
    {$endregion constructor's}
    
    {$region property's}
    
    ///Возвращает размер буфера в байтах
    public property Size: UIntPtr read sz;
    ///Возвращает размер буфера в байтах
    public property Size32: UInt32 read sz.ToUInt32;
    ///Возвращает размер буфера в байтах
    public property Size64: UInt64 read sz.ToUInt64;
    
    ///Если данный буфер был создан функцией SubBuff - возвращает родительский буфер
    ///Иначе возвращает nil
    public property Parent: Buffer read _parent;
    
    {$endregion property's}
    
    {$region Queue's}
    
    ///Создаёт новую очередь-обёртку данного буфера
    ///Которая может хранить множество операций чтения/записи одновременно
    public function NewQueue :=
    new BufferCommandQueue(self);
    
    /// - static function ValueQueue<TRecord>(val: TRecord): BufferCommandQueue; where TRecord: record;
    ///Создаёт новый буфер того же размера что и val, оборачивает в очередь
    ///И вызывает у полученной очереди .WriteValue(val)
    public [MethodImpl(MethodImplOptions.AggressiveInlining)] static function ValueQueue<TRecord>(val: TRecord): BufferCommandQueue; where TRecord: record;
    begin
      Result := 
        Buffer.Create(Marshal.SizeOf&<TRecord>)
        .NewQueue.AddWriteValue(val);
    end;
    
    {$endregion Queue's}
    
    {$region Write}
    
    ///- function WriteData(ptr: IntPtr): BufferCommandQueue;
    ///Копирует область оперативной памяти, на которую ссылается ptr, в данный буфер
    ///Копируется нужное кол-во байт чтобы заполнить весь буфер
    public function WriteData(ptr: CommandQueue<IntPtr>): Buffer;
    ///- function WriteData(ptr: IntPtr; offset, len: integer): BufferCommandQueue;
    ///Копирует область оперативной памяти, на которую ссылается ptr, в данный буфер
    ///offset это отступ в буфере, а len - кол-во копируемых байтов
    public function WriteData(ptr: CommandQueue<IntPtr>; offset, len: CommandQueue<integer>): Buffer;
    
    ///- function WriteData(ptr: pointer): BufferCommandQueue;
    ///Копирует область оперативной памяти, на которую ссылается ptr, в данный буфер
    ///Копируется нужное кол-во байт чтобы заполнить весь буфер
    public function WriteData(ptr: pointer) := WriteData(IntPtr(ptr));
    ///- function WriteData(ptr: pointer; offset, len: integer): BufferCommandQueue;
    ///Копирует область оперативной памяти, на которую ссылается ptr, в данный буфер
    ///offset это отступ в буфере, а len - кол-во копируемых байтов
    public function WriteData(ptr: pointer; offset, len: CommandQueue<integer>) := WriteData(IntPtr(ptr), offset, len);
    
    
    ///- function WriteArray(a: Array): BufferCommandQueue;
    ///Копирует содержимое массива в данный буфер
    ///Копируется нужное кол-во байт чтобы заполнить весь буфер
    public function WriteArray(a: CommandQueue<&Array>): Buffer;
    ///- function WriteArray(a: Array; offset, len: integer): BufferCommandQueue;
    ///Копирует содержимое массива в данный буфер
    ///offset это отступ в буфере, а len - кол-во копируемых байтов
    public function WriteArray(a: CommandQueue<&Array>; offset, len: CommandQueue<integer>): Buffer;
    
    ///- function WriteArray(a: Array): BufferCommandQueue;
    ///Копирует содержимое массива в данный буфер
    ///Копируется нужное кол-во байт чтобы заполнить весь буфер
    public function WriteArray(a: &Array) := WriteArray(CommandQueue&<&Array>(a));
    ///- function WriteArray(a: Array; offset, len: integer): BufferCommandQueue;
    ///Копирует содержимое массива в данный буфер
    ///offset это отступ в буфере, а len - кол-во копируемых байтов
    public function WriteArray(a: &Array; offset, len: CommandQueue<integer>) := WriteArray(CommandQueue&<&Array>(a), offset, len);
    
    
    ///- function WriteValue<TRecord>(val: TRecord; offset: integer := 0): BufferCommandQueue; where TRecord: record;
    ///Записывает значение любого размерного типа в данный буфер
    ///С отступом в offset байт в буфере
    public [MethodImpl(MethodImplOptions.AggressiveInlining)] function WriteValue<TRecord>(val: TRecord; offset: CommandQueue<integer> := 0): Buffer; where TRecord: record;
    begin Result := WriteData(@val, offset, Marshal.SizeOf&<TRecord>); end;
    
    ///- function WriteValue<TRecord>(val: TRecord; offset: integer := 0): BufferCommandQueue; where TRecord: record;
    ///Записывает значение любого размерного типа в данный буфер
    ///С отступом в offset байт в буфере
    public function WriteValue<TRecord>(val: CommandQueue<TRecord>; offset: CommandQueue<integer> := 0): Buffer; where TRecord: record;
    
    {$endregion Write}
    
    {$region Read}
    
    ///- function ReadData(ptr: IntPtr): BufferCommandQueue;
    ///Копирует всё содержимое буффера в область оперативной памяти, на которую указывает ptr
    public function ReadData(ptr: CommandQueue<IntPtr>): Buffer;
    ///- function ReadData(ptr: IntPtr; offset, len: integer): BufferCommandQueue;
    ///Копирует len байт, начиная с байта №offset в буфере, в область оперативной памяти, на которую указывает ptr
    public function ReadData(ptr: CommandQueue<IntPtr>; offset, len: CommandQueue<integer>): Buffer;
    
    ///- function ReadData(ptr: pointer): BufferCommandQueue;
    ///Копирует всё содержимое буффера в область оперативной памяти, на которую указывает ptr
    public function ReadData(ptr: pointer) := ReadData(IntPtr(ptr));
    ///- function ReadData(ptr: pointer; offset, len: integer): BufferCommandQueue;
    ///Копирует len байт, начиная с байта №offset в буфере, в область оперативной памяти, на которую указывает ptr
    public function ReadData(ptr: pointer; offset, len: CommandQueue<integer>) := ReadData(IntPtr(ptr), offset, len);
    
    ///- function ReadArray(a: Array): BufferCommandQueue;
    ///Копирует всё содержимое буффера в содержимое массива
    public function ReadArray(a: CommandQueue<&Array>): Buffer;
    ///- function ReadArray(a: Array; offset, len: integer): BufferCommandQueue;
    ///Копирует len байт, начиная с байта №offset в буфере, в содержимое массива
    public function ReadArray(a: CommandQueue<&Array>; offset, len: CommandQueue<integer>): Buffer;
    
    ///- function ReadArray(a: Array): BufferCommandQueue;
    ///Копирует всё содержимое буффера в содержимое массива
    public function ReadArray(a: &Array) := ReadArray(CommandQueue&<&Array>(a));
    ///- function ReadArray(a: Array; offset, len: integer): BufferCommandQueue;
    ///Копирует len байт, начиная с байта №offset в буфере, в содержимое массива
    public function ReadArray(a: &Array; offset, len: CommandQueue<integer>) := ReadArray(CommandQueue&<&Array>(a), offset, len);
    
    ///- function ReadValue<TRecord>(var val: TRecord; offset: integer := 0): BufferCommandQueue; where TRecord: record;
    ///Читает значение любого размерного типа из данного буфера
    ///С отступом в offset байт в буфере
    public function ReadValue<TRecord>(var val: TRecord; offset: CommandQueue<integer> := 0): Buffer; where TRecord: record;
    begin
      Result := ReadData(@val, offset, Marshal.SizeOf&<TRecord>);
    end;
    
    {$endregion Read}
    
    {$region Fill}
    
    ///- function PatternFill(ptr: IntPtr): BufferCommandQueue;
    ///Заполняет весь буфер копиями массива байт, длинной pattern_len,
    ///прочитанным из области оперативной памяти, на которую указывает ptr
    public function FillData(ptr: CommandQueue<IntPtr>; pattern_len: CommandQueue<integer>): Buffer;
    ///- function PatternFill(ptr: IntPtr; offset, len: integer): BufferCommandQueue;
    ///Заполняет часть буфера (начиная с байта №offset и длинной len) копиями массива байт, длинной pattern_len,
    ///прочитанным из области оперативной памяти, на которую указывает ptr
    public function FillData(ptr: CommandQueue<IntPtr>; pattern_len, offset, len: CommandQueue<integer>): Buffer;
    
    ///- function PatternFill(ptr: pointer): BufferCommandQueue;
    ///Заполняет весь буфер копиями массива байт, длинной pattern_len,
    ///прочитанным из области оперативной памяти, на которую указывает ptr
    public function FillData(ptr: pointer; pattern_len: CommandQueue<integer>) := FillData(IntPtr(ptr), pattern_len);
    ///- function PatternFill(ptr: pointer; offset, len: integer): BufferCommandQueue;
    ///Заполняет часть буфера (начиная с байта №offset и длинной len) копиями массива байт, длинной pattern_len,
    ///прочитанным из области оперативной памяти, на которую указывает ptr
    public function FillData(ptr: pointer; pattern_len, offset, len: CommandQueue<integer>) := FillData(IntPtr(ptr), pattern_len, offset, len);
    
    ///- function PatternFill(a: Array): BufferCommandQueue;
    ///Заполняет весь буфер копиями содержимого массива
    public function FillArray(a: CommandQueue<&Array>): Buffer;
    ///- function PatternFill(a: Array; offset, len: integer): BufferCommandQueue;
    ///Заполняет часть буфера (начиная с байта №offset и длинной len) копиями содержимого массива
    public function FillArray(a: CommandQueue<&Array>; offset, len: CommandQueue<integer>): Buffer;
    
    ///- function PatternFill(a: Array): BufferCommandQueue;
    ///Заполняет весь буфер копиями содержимого массива
    public function FillArray(a: &Array) := FillArray(CommandQueue&<&Array>(a));
    ///- function PatternFill(a: Array; offset, len: integer): BufferCommandQueue;
    ///Заполняет часть буфера (начиная с байта №offset и длинной len) копиями содержимого массива
    public function FillArray(a: &Array; offset, len: CommandQueue<integer>) := FillArray(CommandQueue&<&Array>(a), offset, len);
    
    ///- function PatternFill<TRecord>(val: TRecord): BufferCommandQueue; where TRecord: record;
    ///Заполняет весь буфер копиями значения любого размерного типа
    public [MethodImpl(MethodImplOptions.AggressiveInlining)] function FillValue<TRecord>(val: TRecord): Buffer; where TRecord: record;
    ///- function PatternFill<TRecord>(val: TRecord; offset, len: integer): BufferCommandQueue; where TRecord: record;
    ///Заполняет часть буфера (начиная с байта №offset и длинной len) копиями значения любого размерного типа
    public [MethodImpl(MethodImplOptions.AggressiveInlining)] function FillValue<TRecord>(val: TRecord; offset, len: CommandQueue<integer>): Buffer; where TRecord: record;
    
    ///- function PatternFill<TRecord>(val: TRecord): BufferCommandQueue; where TRecord: record;
    ///Заполняет весь буфер копиями значения любого размерного типа
    public function FillValue<TRecord>(val: CommandQueue<TRecord>): Buffer; where TRecord: record;
    ///- function PatternFill<TRecord>(val: TRecord; offset, len: integer): BufferCommandQueue; where TRecord: record;
    ///Заполняет часть буфера (начиная с байта №offset и длинной len) копиями значения любого размерного типа
    public function FillValue<TRecord>(val: CommandQueue<TRecord>; offset, len: CommandQueue<integer>): Buffer; where TRecord: record;
    
    {$endregion Fill}
    
    {$region Copy}
    
    ///- function CopyFrom(b: Buffer; from, &to, len: integer): BufferCommandQueue;
    ///Копирует содержимое буфера b в данный буфер
    ///from - отступ в буффере b
    ///to   - отступ в данном буффере
    ///len  - кол-во копируемых байт
    public function CopyFrom(b: CommandQueue<Buffer>; from, &to, len: CommandQueue<integer>): Buffer;
    ///- function CopyTo(b: Buffer; from, &to, len: integer): BufferCommandQueue;
    ///Копирует содержимое данного буфера в буфер b
    ///from - отступ в данном буффере
    ///to   - отступ в буффере b
    ///len  - кол-во копируемых байт
    public function CopyTo  (b: CommandQueue<Buffer>; from, &to, len: CommandQueue<integer>): Buffer;
    
    ///- function CopyFrom(b: Buffer): BufferCommandQueue;
    ///Копирует всё содержимое буфера b в данный буфер
    public function CopyFrom(b: CommandQueue<Buffer>): Buffer;
    ///- function CopyTo(b: Buffer): BufferCommandQueue;
    ///Копирует всё содержимое данного буфера в буфер b
    public function CopyTo  (b: CommandQueue<Buffer>): Buffer;
    
    {$endregion Copy}
    
    {$region Get}
    
    ///- function GetData(offset, len: integer): IntPtr;
    ///Выделяет неуправляемую область в памяти
    ///И копирует в неё len байт из данного буфера, начиная с байта №offset
    ///Обязательно вызовите Marshal.FreeHGlobal на полученном дескрипторе, после использования
    public function GetData(offset, len: CommandQueue<integer>): IntPtr;
    ///- function GetData: IntPtr;
    ///Выделяет неуправляемую область в памяти, одинакового размера с данным буфером
    ///И копирует в неё всё содержимое данного буфера
    ///Обязательно вызовите Marshal.FreeHGlobal на полученном дескрипторе, после использования
    public function GetData := GetData(0,integer(self.Size32));
    
    
    
    ///- function GetArrayAt<TArray>(offset: integer; params szs: array of integer): TArray; where TArray: &Array;
    ///Создаёт новый массив с размерностями szs
    ///И копирует в него, начиная с байта offset, достаточно байт чтобы заполнить весь массив
    public function GetArrayAt<TArray>(offset: CommandQueue<integer>; szs: CommandQueue<array of integer>): TArray; where TArray: &Array;
    ///- function GetArray<TArray>(params szs: array of integer): TArray; where TArray: &Array;
    ///Создаёт новый массив с размерностями szs
    ///И копирует в него достаточно байт чтобы заполнить весь массив
    public function GetArray<TArray>(szs: CommandQueue<array of integer>): TArray; where TArray: &Array;
    begin Result := GetArrayAt&<TArray>(0, szs); end;
    
    ///- function GetArrayAt<TArray>(offset: integer; params szs: array of integer): TArray; where TArray: &Array;
    ///Создаёт новый массив с размерностями szs
    ///И копирует в него, начиная с байта offset, достаточно байт чтобы заполнить весь массив
    public function GetArrayAt<TArray>(offset: CommandQueue<integer>; params szs: array of CommandQueue<integer>): TArray; where TArray: &Array;
    ///- function GetArray<TArray>(params szs: array of integer): TArray; where TArray: &Array;
    ///Создаёт новый массив с размерностями szs
    ///И копирует в него достаточно байт чтобы заполнить весь массив
    public function GetArray<TArray>(params szs: array of integer): TArray; where TArray: &Array;
    begin Result := GetArrayAt&<TArray>(0, CommandQueue&<array of integer>(szs)); end;
    
    
    ///- function GetArray1At<TRecord>(offset: integer; length: integer): array of TRecord; where TRecord: record;
    ///Создаёт новый 1-мерный массив, с length элементами типа TRecord
    ///И копирует в него, начиная с байта offset, достаточно байт чтобы заполнить весь массив
    public function GetArray1At<TRecord>(offset, length: CommandQueue<integer>): array of TRecord; where TRecord: record;
    begin Result := GetArrayAt&<array of TRecord>(offset, length); end;
    ///- function GetArray1<TRecord>(length: integer): array of TRecord; where TRecord: record;
    ///Создаёт новый 1-мерный массив, с length элементами типа TRecord
    ///И копирует в него достаточно байт чтобы заполнить весь массив
    public function GetArray1<TRecord>(length: CommandQueue<integer>): array of TRecord; where TRecord: record;
    begin Result := GetArrayAt&<array of TRecord>(0,length); end;
    
    ///- function GetArray1<TRecord>: array of TRecord; where TRecord: record;
    ///Создаёт новый 1-мерный массив, с максимальным кол-вом элементов типа TRecord
    ///И копирует в него достаточно байт чтобы заполнить весь массив
    public function GetArray1<TRecord>: array of TRecord; where TRecord: record;
    begin Result := GetArrayAt&<array of TRecord>(0, integer(sz.ToUInt32) div Marshal.SizeOf&<TRecord>); end;
    
    
    ///- function GetArray2At<TRecord>(offset: integer; length: integer): array[,] of TRecord; where TRecord: record;
    ///Создаёт новый 2-мерный массив, с length элементами типа TRecord
    ///И копирует в него, начиная с байта offset, достаточно байт чтобы заполнить весь массив
    public function GetArray2At<TRecord>(offset, length1, length2: CommandQueue<integer>): array[,] of TRecord; where TRecord: record;
    begin Result := GetArrayAt&<array[,] of TRecord>(offset, length1, length2); end;
    ///- function GetArray2<TRecord>(length: integer): array of TRecord; where TRecord: record;
    ///Создаёт новый 2-мерный массив, с length элементами типа TRecord
    ///И копирует в него достаточно байт чтобы заполнить весь массив
    public function GetArray2<TRecord>(length1, length2: CommandQueue<integer>): array[,] of TRecord; where TRecord: record;
    begin Result := GetArrayAt&<array[,] of TRecord>(0, length1, length2); end;
    
    
    ///- function GetArray3At<TRecord>(offset: integer; length: integer): array[,,] of TRecord; where TRecord: record;
    ///Создаёт новый 3-мерный массив, с length элементами типа TRecord
    ///И копирует в него, начиная с байта offset, достаточно байт чтобы заполнить весь массив
    public function GetArray3At<TRecord>(offset, length1, length2, length3: CommandQueue<integer>): array[,,] of TRecord; where TRecord: record;
    begin Result := GetArrayAt&<array[,,] of TRecord>(offset, length1, length2, length3); end;
    ///- function GetArray3<TRecord>(length: integer): array[,,] of TRecord; where TRecord: record;
    ///Создаёт новый 3-мерный массив, с length элементами типа TRecord
    ///И копирует в него достаточно байт чтобы заполнить весь массив
    public function GetArray3<TRecord>(length1, length2, length3: CommandQueue<integer>): array[,,] of TRecord; where TRecord: record;
    begin Result := GetArrayAt&<array[,,] of TRecord>(0, length1, length2, length3); end;
    
    
    
    ///- function GetValueAt<TRecord>(offset: integer): TRecord; where TRecord: record;
    ///Читает значение любого размерного типа из данного буфера
    ///С отступом в offset байт в буфере
    public [MethodImpl(MethodImplOptions.AggressiveInlining)] function GetValueAt<TRecord>(offset: CommandQueue<integer>): TRecord; where TRecord: record;
    ///- function GetValue<TRecord>: TRecord; where TRecord: record;
    ///Читает значение любого размерного типа из начала данного буфера
    public [MethodImpl(MethodImplOptions.AggressiveInlining)] function GetValue<TRecord>: TRecord; where TRecord: record; begin Result := GetValueAt&<TRecord>(0); end;
    
    {$endregion Get}
    
    ///Высвобождает выделенную на GPU память
    ///Если такой нету - не делает ничего
    ///Память будет заново выделена, если снова использовать данный буфер
    public procedure Dispose :=
    if self.memobj<>cl_mem.Zero then
    begin
      cl.ReleaseMemObject(memobj).RaiseIfError;
      memobj := cl_mem.Zero;
    end;
    
    protected procedure Finalize; override :=
    self.Dispose;
    
  end;
  
  {$endregion Buffer}
  
  {$region Kernel}
  
  ///Особый тип очереди, всегда возвращающий Kernel
  ///Может быть создан из объекта Kernel или очереди, возвращающей Kernel
  ///Используется для хранения списка особых команд, применимых только к Kernel
  KernelCommandQueue = sealed class(GPUCommandContainer<Kernel>)
    
    {$region constructor's}
    
    ///Создаёт объект KernelCommandQueue, команды которого будут применятся к кёрнелу b
    public constructor(k: Kernel) := inherited Create(k);
    ///Создаёт объект KernelCommandQueue, команды которого будут применятся к кёрнелу, который будет результатом q
    public constructor(q: CommandQueue<Kernel>) := inherited Create(q);
    
    {$endregion constructor's}
    
    {$region Utils}
    
    protected function AddCommand(comm: GPUCommand<Kernel>): KernelCommandQueue;
    begin
      self.commands += comm;
      Result := self;
    end;
    
    public function Clone: KernelCommandQueue := inherited Clone as KernelCommandQueue;
    
    {$endregion Utils}
    
    {$region Exec}
    
    public function AddExec(work_szs: array of UIntPtr; params args: array of CommandQueue<Buffer>): KernelCommandQueue;
    public function AddExec(work_szs: array of integer; params args: array of CommandQueue<Buffer>) :=
    AddExec(work_szs.ConvertAll(sz->new UIntPtr(sz)), args);
    
    public function AddExec1(work_sz1: UIntPtr; params args: array of CommandQueue<Buffer>) := AddExec(new UIntPtr[](work_sz1), args);
    public function AddExec1(work_sz1: integer; params args: array of CommandQueue<Buffer>) := AddExec1(new UIntPtr(work_sz1), args);
    
    public function AddExec2(work_sz1, work_sz2: UIntPtr; params args: array of CommandQueue<Buffer>) := AddExec(new UIntPtr[](work_sz1, work_sz2), args);
    public function AddExec2(work_sz1, work_sz2: integer; params args: array of CommandQueue<Buffer>) := AddExec2(new UIntPtr(work_sz1), new UIntPtr(work_sz2), args);
    
    public function AddExec3(work_sz1, work_sz2, work_sz3: UIntPtr; params args: array of CommandQueue<Buffer>) := AddExec(new UIntPtr[](work_sz1, work_sz2, work_sz3), args);
    public function AddExec3(work_sz1, work_sz2, work_sz3: integer; params args: array of CommandQueue<Buffer>) := AddExec3(new UIntPtr(work_sz1), new UIntPtr(work_sz2), new UIntPtr(work_sz3), args);
    
    
    public function AddExec(work_szs: array of CommandQueue<UIntPtr>; params args: array of CommandQueue<Buffer>): KernelCommandQueue;
    public function AddExec(work_szs: array of CommandQueue<integer>; params args: array of CommandQueue<Buffer>) :=
    AddExec(work_szs.ConvertAll(sz_q->sz_q.ThenConvert(sz->new UIntPtr(sz))), args);
    
    public function AddExec1(work_sz1: CommandQueue<UIntPtr>; params args: array of CommandQueue<Buffer>) := AddExec(new CommandQueue<UIntPtr>[](work_sz1), args);
    public function AddExec1(work_sz1: CommandQueue<integer>; params args: array of CommandQueue<Buffer>) := AddExec1(work_sz1.ThenConvert(sz->new UIntPtr(sz)), args);
    
    public function AddExec2(work_sz1, work_sz2: CommandQueue<UIntPtr>; params args: array of CommandQueue<Buffer>) := AddExec(new CommandQueue<UIntPtr>[](work_sz1, work_sz2), args);
    public function AddExec2(work_sz1, work_sz2: CommandQueue<integer>; params args: array of CommandQueue<Buffer>) := AddExec2(work_sz1.ThenConvert(sz->new UIntPtr(sz)), work_sz2.ThenConvert(sz->new UIntPtr(sz)), args);
    
    public function AddExec3(work_sz1, work_sz2, work_sz3: CommandQueue<UIntPtr>; params args: array of CommandQueue<Buffer>) := AddExec(new CommandQueue<UIntPtr>[](work_sz1, work_sz2, work_sz3), args);
    public function AddExec3(work_sz1, work_sz2, work_sz3: CommandQueue<integer>; params args: array of CommandQueue<Buffer>) := AddExec3(work_sz1.ThenConvert(sz->new UIntPtr(sz)), work_sz2.ThenConvert(sz->new UIntPtr(sz)), work_sz3.ThenConvert(sz->new UIntPtr(sz)), args);
    
    
    public function AddExec(work_szs: CommandQueue<array of UIntPtr>; params args: array of CommandQueue<Buffer>): KernelCommandQueue;
    public function AddExec(work_szs: CommandQueue<array of integer>; params args: array of CommandQueue<Buffer>): KernelCommandQueue;
    
    {$endregion Exec}
    
    {$region Non-command add's}
    
    public function AddQueue(q: CommandQueueBase): KernelCommandQueue;
    begin
      InternalAddQueue(q);
      Result := self;
    end;
    
    public function AddProc(p: (Kernel,Context)->()): KernelCommandQueue;
    begin
      InternalAddProc(p);
      Result := self;
    end;
    public function AddProc(p: Kernel->()) := AddProc((k,c)->p(k));
    
    public function AddWait(q: CommandQueueBase; allow_q_cloning: boolean := true): KernelCommandQueue;
    begin
      InternalAddWait(q, allow_q_cloning);
      Result := self;
    end;
    
    {$endregion Non-command add's}
    
    {$region override методы}
    
    protected function InternalClone(muhs: Dictionary<object, object>; cache: Dictionary<CommandQueueBase, CommandQueueBase>): CommandQueueBase; override;
    
    {$endregion override методы}
    
  end;
  
  Kernel = sealed class
    private _kernel: cl_kernel;
    
    {$region constructor's}
    
    private constructor := raise new System.NotSupportedException;
    
    public constructor(prog: ProgramCode; name: string);
    
    {$endregion constructor's}
    
    {$region Queue's}
    
    public function NewQueue :=
    new KernelCommandQueue(self);
    
    {$endregion Queue's}
    
    {$region Exec}
    
    public function Exec(work_szs: array of UIntPtr; params args: array of CommandQueue<Buffer>): Kernel;
    public function Exec(work_szs: array of integer; params args: array of CommandQueue<Buffer>) :=
    Exec(work_szs.ConvertAll(sz->new UIntPtr(sz)), args);
    
    public function Exec1(work_sz1: UIntPtr; params args: array of CommandQueue<Buffer>) := Exec(new UIntPtr[](work_sz1), args);
    public function Exec1(work_sz1: integer; params args: array of CommandQueue<Buffer>) := Exec1(new UIntPtr(work_sz1), args);
    
    public function Exec2(work_sz1, work_sz2: UIntPtr; params args: array of CommandQueue<Buffer>) := Exec(new UIntPtr[](work_sz1, work_sz2), args);
    public function Exec2(work_sz1, work_sz2: integer; params args: array of CommandQueue<Buffer>) := Exec2(new UIntPtr(work_sz1), new UIntPtr(work_sz2), args);
    
    public function Exec3(work_sz1, work_sz2, work_sz3: UIntPtr; params args: array of CommandQueue<Buffer>) := Exec(new UIntPtr[](work_sz1, work_sz2, work_sz3), args);
    public function Exec3(work_sz1, work_sz2, work_sz3: integer; params args: array of CommandQueue<Buffer>) := Exec3(new UIntPtr(work_sz1), new UIntPtr(work_sz2), new UIntPtr(work_sz3), args);
    
    
    public function Exec(work_szs: array of CommandQueue<UIntPtr>; params args: array of CommandQueue<Buffer>): Kernel;
    public function Exec(work_szs: array of CommandQueue<integer>; params args: array of CommandQueue<Buffer>) :=
    Exec(work_szs.ConvertAll(sz_q->sz_q.ThenConvert(sz->new UIntPtr(sz))), args);
    
    public function Exec1(work_sz1: CommandQueue<UIntPtr>; params args: array of CommandQueue<Buffer>) := Exec(new CommandQueue<UIntPtr>[](work_sz1), args);
    public function Exec1(work_sz1: CommandQueue<integer>; params args: array of CommandQueue<Buffer>) := Exec1(work_sz1.ThenConvert(sz->new UIntPtr(sz)), args);
    
    public function Exec2(work_sz1, work_sz2: CommandQueue<UIntPtr>; params args: array of CommandQueue<Buffer>) := Exec(new CommandQueue<UIntPtr>[](work_sz1, work_sz2), args);
    public function Exec2(work_sz1, work_sz2: CommandQueue<integer>; params args: array of CommandQueue<Buffer>) := Exec2(work_sz1.ThenConvert(sz->new UIntPtr(sz)), work_sz2.ThenConvert(sz->new UIntPtr(sz)), args);
    
    public function Exec3(work_sz1, work_sz2, work_sz3: CommandQueue<UIntPtr>; params args: array of CommandQueue<Buffer>) := Exec(new CommandQueue<UIntPtr>[](work_sz1, work_sz2, work_sz3), args);
    public function Exec3(work_sz1, work_sz2, work_sz3: CommandQueue<integer>; params args: array of CommandQueue<Buffer>) := Exec3(work_sz1.ThenConvert(sz->new UIntPtr(sz)), work_sz2.ThenConvert(sz->new UIntPtr(sz)), work_sz3.ThenConvert(sz->new UIntPtr(sz)), args);
    
    
    public function Exec(work_szs: CommandQueue<array of UIntPtr>; params args: array of CommandQueue<Buffer>): Kernel;
    public function Exec(work_szs: CommandQueue<array of integer>; params args: array of CommandQueue<Buffer>): Kernel;
    
    {$endregion Exec}
    
    protected procedure Finalize; override :=
    cl.ReleaseKernel(self._kernel).RaiseIfError;
    
  end;
  
  {$endregion Kernel}
  
  {$region Context}
  
  Context = sealed class
    private static _platform: cl_platform_id;
    private static _def_cont: Context;
    
    private _device: cl_device_id;
    private _context: cl_context;
    private need_finnalize := false;
    
    public static property &Default: Context read _def_cont write _def_cont;
    
    static constructor :=
    try
      
      var ec := cl.GetPlatformIDs(1,@_platform,nil);
      ec.RaiseIfError;
      
      try
        _def_cont := new Context;
      except
        _def_cont := new Context(DeviceTypeFlags.All); // если нету GPU - попытаться хотя бы для чего то его инициализировать
      end;
      
    except
      on e: Exception do
      begin
        {$reference PresentationFramework.dll}
        System.Windows.MessageBox.Show(e.ToString, 'Не удалось инициализировать OpenCL');
        Halt;
      end;
    end;
    
    /// Инициализирует новый контекст c 1 девайсом типа GPU
    public constructor := Create(DeviceTypeFlags.GPU);
    
    /// Инициализирует новый контекст c 1 девайсом типа dt
    public constructor(dt: DeviceTypeFlags);
    begin
      var ec: ErrorCode;
      
      cl.GetDeviceIDs(_platform, dt, 1, @_device, nil).RaiseIfError;
      
      _context := cl.CreateContext(nil, 1, @_device, nil, nil, @ec);
      ec.RaiseIfError;
      
      need_finnalize := true;
    end;
    
    /// Создаёт обёртку для дескриптора контекста, полученного модулем OpenCL
    /// Девайс выбирается первый попавшейся из списка связанных
    /// Автоматическое удаление контекста не произойдёт при удалении всех ссылок на полученную обёртку
    /// В отличии от создания нового контекста - контекстом управляет модуль OpenCL а не OpenCLABC
    public constructor(context: cl_context);
    begin
      
      cl.GetContextInfo(context, ContextInfoType.CL_CONTEXT_DEVICES, new UIntPtr(IntPtr.Size), @_device, nil).RaiseIfError;
      
      _context := context;
    end;
    
    /// Создаёт обёртку для дескриптора контекста, полученного модулем OpenCL
    /// Девайс выбирается с указанным дескриптором, так же полученный из модуля OpenCL
    /// Автоматическое удаление контекста не произойдёт при удалении всех ссылок на полученную обёртку
    /// В отличии от создания нового контекста - контекстом управляет модуль OpenCL а не OpenCLABC
    public constructor(context: cl_context; device: cl_device_id);
    begin
      _device := device;
      _context := context;
    end;
    
    /// Инициализирует все команды в очереди и запускает первые
    /// Возвращает объект задачи, по которому можно следить за состоянием выполнения очереди
    public function BeginInvoke(q: CommandQueueBase): Task<object>;
    begin
      
      var tasks := new List<Task>( q.GetEstimateTaskCount(new HashSet<object>) );
      q.InvokeNewQ(self, tasks);
      
      Result := new Task<object>(()-> //ToDo #2048
      try
        while true do
        begin
          for var i := tasks.Count-1 downto 0 do
            if tasks[i].Status <> TaskStatus.Running then
            begin
              if tasks[i].Exception<>nil then raise tasks[i].Exception;
              tasks.RemoveAt(i);
            end;
          if tasks.Count=0 then break;
          Sleep(10);
        end;
        CommandQueueBase.WaitAndRelease(q.ev);
        
        Result := q.GetRes;
      finally
        q.UnInvoke;
      end);
      
      Result.Start;
    end;
    /// Инициализирует все команды в очереди и запускает первые
    /// Возвращает объект задачи, по которому можно следить за состоянием выполнения очереди
    public function BeginInvoke<T>(q: CommandQueue<T>): Task<T>;
    begin
      
      var tasks := new List<Task>( q.GetEstimateTaskCount(new HashSet<object>) );
      q.InvokeNewQ(self, tasks);
      
      Result := new Task<T>(()-> //ToDo #2048
      try
        while true do
        begin
          for var i := tasks.Count-1 downto 0 do
            if tasks[i].Status <> TaskStatus.Running then
            begin
              if tasks[i].Exception<>nil then raise tasks[i].Exception;
              tasks.RemoveAt(i);
            end;
          if tasks.Count=0 then break;
          Sleep(10);
        end;
        CommandQueueBase.WaitAndRelease(q.ev);
        
        Result := q.res;
      finally
        q.UnInvoke;
      end);
      
      Result.Start;
    end;
    
    /// Выполняет BeginInvoke и ожидает окончания выполнения возвращённой задачи
    /// Возвращает результат очереди
    public function SyncInvoke(q: CommandQueueBase): object;
    begin
      var tsk := BeginInvoke(q);
      tsk.Wait;
      Result := tsk.Result;
    end;
    /// Выполняет BeginInvoke и ожидает окончания выполнения возвращённой задачи
    /// Возвращает результат очереди
    public function SyncInvoke<T>(q: CommandQueue<T>): T;
    begin
      var tsk := BeginInvoke(q);
      tsk.Wait;
      Result := tsk.Result;
    end;
    
    protected procedure Finalize; override :=
    if need_finnalize then // если было исключение при инициализации или инициализация произошла из дескриптора
      cl.ReleaseContext(_context).RaiseIfError;
    
  end;
  
  {$endregion Context}
  
  {$region ProgramCode}
  
  ProgramCode = sealed class
    private _program: cl_program;
    private cntxt: Context;
    
    private constructor := exit;
    
    public constructor(c: Context; params files_texts: array of string);
    begin
      var ec: ErrorCode;
      self.cntxt := c;
      
      self._program := cl.CreateProgramWithSource(c._context, files_texts.Length, files_texts, files_texts.ConvertAll(s->new UIntPtr(s.Length)), ec);
      ec.RaiseIfError;
      
      cl.BuildProgram(self._program, 1, @c._device, nil,nil,nil).RaiseIfError;
      
    end;
    
    public constructor(params files_texts: array of string) :=
    Create(Context.Default, files_texts);
    
    public property KernelByName[kname: string]: Kernel read new Kernel(self, kname); default;
    
    public function GetAllKernels: Dictionary<string, Kernel>;
    begin
      
      var names_char_len: UIntPtr;
      cl.GetProgramInfo(_program, ProgramInfoType.NUM_KERNELS, new UIntPtr(UIntPtr.Size), @names_char_len, nil).RaiseIfError;
      
      var names_ptr := Marshal.AllocHGlobal(IntPtr(pointer(names_char_len))+1);
      cl.GetProgramInfo(_program, ProgramInfoType.KERNEL_NAMES, names_char_len, pointer(names_ptr), nil).RaiseIfError;
      
      var names := Marshal.PtrToStringAnsi(names_ptr).Split(';');
      Marshal.FreeHGlobal(names_ptr);
      
      Result := new Dictionary<string, Kernel>(names.Length);
      foreach var kname in names do
        Result[kname] := self[kname];
      
    end;
    
    public function Serialize: array of byte;
    begin
      var bytes_count: UIntPtr;
      cl.GetProgramInfo(_program, ProgramInfoType.BINARY_SIZES, new UIntPtr(UIntPtr.Size), @bytes_count, nil).RaiseIfError;
      
      var bytes_mem := Marshal.AllocHGlobal(IntPtr(pointer(bytes_count)));
      cl.GetProgramInfo(_program, ProgramInfoType.BINARIES, new UIntPtr(UIntPtr.Size), @bytes_mem, nil).RaiseIfError;
      
      Result := new byte[bytes_count.ToUInt64()];
      Marshal.Copy(bytes_mem,Result, 0,Result.Length);
      Marshal.FreeHGlobal(bytes_mem);
      
    end;
    
    public procedure SerializeTo(bw: System.IO.BinaryWriter);
    begin
      var bts := Serialize;
      bw.Write(bts.Length);
      bw.Write(bts);
    end;
    
    public procedure SerializeTo(str: System.IO.Stream) := SerializeTo(new System.IO.BinaryWriter(str));
    
    public static function Deserialize(c: Context; bin: array of byte): ProgramCode;
    begin
      var ec: ErrorCode;
      
      Result := new ProgramCode;
      Result.cntxt := c;
      
      var gchnd := GCHandle.Alloc(bin, GCHandleType.Pinned);
      var bin_mem: ^byte := pointer(gchnd.AddrOfPinnedObject);
      var bin_len := new UIntPtr(bin.Length);
      
      Result._program := cl.CreateProgramWithBinary(c._context,1,@c._device, @bin_len, @bin_mem, nil, @ec);
      ec.RaiseIfError;
      gchnd.Free;
      
    end;
    
    public static function DeserializeFrom(c: Context; br: System.IO.BinaryReader): ProgramCode;
    begin
      var bin_len := br.ReadInt32;
      var bin_arr := br.ReadBytes(bin_len);
      if bin_arr.Length<bin_len then raise new System.IO.EndOfStreamException;
      Result := Deserialize(c, bin_arr);
    end;
    
    public static function DeserializeFrom(c: Context; str: System.IO.Stream) :=
    DeserializeFrom(c, new System.IO.BinaryReader(str));
    
  end;
  
  {$endregion ProgramCode}
  
{$region Сахарные подпрограммы}

///Host Funcion Queue
///Создаёт новую очередь, выполняющую функцию на CPU
///И возвращающую результат этой функции
function HFQ<T>(f: ()->T): CommandQueue<T>;

///Host Procecure Queue
///Создаёт новую очередь, выполняющую процедуру на CPU
///И возвращающую object(nil)
function HPQ(p: ()->()): CommandQueue<object>;

///Складывает все очереди qs
///Возвращает очередь, по очереди выполняющую все очереди из qs
function CombineSyncQueue<T>(qs: List<CommandQueueBase>): CommandQueue<T>;
///Складывает все очереди qs
///Возвращает очередь, по очереди выполняющую все очереди из qs
function CombineSyncQueue<T>(qs: List<CommandQueue<T>>): CommandQueue<T>;
///Складывает все очереди qs
///Возвращает очередь, по очереди выполняющую все очереди из qs
function CombineSyncQueue<T>(params qs: array of CommandQueueBase): CommandQueue<T>;
///Складывает все очереди qs
///Возвращает очередь, по очереди выполняющую все очереди из qs
function CombineSyncQueue<T>(params qs: array of CommandQueue<T>): CommandQueue<T>;
///Складывает все очереди qs
///Возвращает очередь, по очереди выполняющую все очереди из qs
///И затем применяет преобразование conv чтобы получить из результатов очередей qs - свой результат
function CombineSyncQueue<T,TRes>(conv: Func<array of object, TRes>; qs: List<CommandQueueBase>): CommandQueue<TRes>;
///Складывает все очереди qs
///Возвращает очередь, по очереди выполняющую все очереди из qs
///И затем применяет преобразование conv чтобы получить из результатов очередей qs - свой результат
function CombineSyncQueue<T,TRes>(conv: Func<array of T, TRes>; qs: List<CommandQueue<T>>): CommandQueue<TRes>;
///Складывает все очереди qs
///Возвращает очередь, по очереди выполняющую все очереди из qs
///И затем применяет преобразование conv чтобы получить из результатов очередей qs - свой результат
function CombineSyncQueue<T,TRes>(conv: Func<array of object, TRes>; params qs: array of CommandQueueBase): CommandQueue<TRes>;
///Складывает все очереди qs
///Возвращает очередь, по очереди выполняющую все очереди из qs
///И затем применяет преобразование conv чтобы получить из результатов очередей qs - свой результат
function CombineSyncQueue<T,TRes>(conv: Func<array of T, TRes>; params qs: array of CommandQueue<T>): CommandQueue<TRes>;

///Умножает все очереди qs
///Возвращает очередь, параллельно выполняющую все очереди из qs
function CombineAsyncQueue<T>(qs: List<CommandQueueBase>): CommandQueue<T>;
///Умножает все очереди qs
///Возвращает очередь, параллельно выполняющую все очереди из qs
function CombineAsyncQueue<T>(qs: List<CommandQueue<T>>): CommandQueue<T>;
///Умножает все очереди qs
///Возвращает очередь, параллельно выполняющую все очереди из qs
function CombineAsyncQueue<T>(params qs: array of CommandQueueBase): CommandQueue<T>;
///Умножает все очереди qs
///Возвращает очередь, параллельно выполняющую все очереди из qs
function CombineAsyncQueue<T>(params qs: array of CommandQueue<T>): CommandQueue<T>;
///Умножает все очереди qs
///Возвращает очередь, параллельно выполняющую все очереди из qs
///И затем применяет преобразование conv чтобы получить из результатов очередей qs - свой результат
function CombineAsyncQueue<T,TRes>(conv: Func<array of object, TRes>; qs: List<CommandQueueBase>): CommandQueue<TRes>;
///Умножает все очереди qs
///Возвращает очередь, параллельно выполняющую все очереди из qs
///И затем применяет преобразование conv чтобы получить из результатов очередей qs - свой результат
function CombineAsyncQueue<T,TRes>(conv: Func<array of T, TRes>; qs: List<CommandQueue<T>>): CommandQueue<TRes>;
///Умножает все очереди qs
///Возвращает очередь, параллельно выполняющую все очереди из qs
///И затем применяет conv чтобы получить из результатов очередей qs - свой результат
///И затем применяет преобразование conv чтобы получить из результатов очередей qs - свой результат
function CombineAsyncQueue<T,TRes>(conv: Func<array of object, TRes>; params qs: array of CommandQueueBase): CommandQueue<TRes>;
///Умножает все очереди qs
///Возвращает очередь, параллельно выполняющую все очереди из qs
///И затем применяет преобразование conv чтобы получить из результатов очередей qs - свой результат
function CombineAsyncQueue<T,TRes>(conv: Func<array of T, TRes>; params qs: array of CommandQueue<T>): CommandQueue<TRes>;

{$endregion Сахарные подпрограммы}

implementation

{$region Utils}

type
  CLGCHandle = sealed class
    gchnd: GCHandle;
    
    constructor(o: object) :=
    gchnd := GCHandle.Alloc(o, GCHandleType.Pinned);
    
    property Ptr: IntPtr read gchnd.AddrOfPinnedObject;
    
    procedure CLFree(ev: cl_event; status: CommandExecutionStatus; data: pointer) := gchnd.Free;
    
  end;

{$endregion Utils}

{$region CommandQueue}

{$region Dummy}

type
  DummyCommandQueue<T> = sealed class(CommandQueue<T>)
    
    public constructor(o: T) :=
    self.res := o;
    
    protected function GetEstimateTaskCount(prev_hubs: HashSet<object>): integer; override := 0;
    
    private procedure CLSignalMWEvent(ev: cl_event; status: CommandExecutionStatus; data: pointer) := self.SignalMWEvent;
    
    protected procedure Invoke(c: Context; var cq: __SafeNativQueue; prev_ev: cl_event; tasks: List<Task>); override;
    begin
      MakeBusy;
      self.ev := prev_ev;
      if prev_ev=cl_event.Zero then
        SignalMWEvent else
        cl.SetEventCallback(prev_ev, CommandExecutionStatus.COMPLETE, CLSignalMWEvent, nil).RaiseIfError;
    end;
    
    protected function InternalClone(muhs: Dictionary<object, object>; cache: Dictionary<CommandQueueBase, CommandQueueBase>): CommandQueueBase; override :=
    new DummyCommandQueue<T>(self.res);
    
  end;
  
static function CommandQueueBase.operator implicit(o: object): CommandQueueBase :=
new DummyCommandQueue<object>(o);

static function CommandQueue<T>.operator implicit(o: T): CommandQueue<T> :=
new DummyCommandQueue<T>(o);

{$endregion Dummy}

{$region HostFunc}

type
  CommandQueueHostFunc<T> = sealed class(CommandQueue<T>)
    private f: ()->T;
    
    public constructor(f: ()->T) :=
    self.f := f;
    
    protected function GetEstimateTaskCount(prev_hubs: HashSet<object>): integer; override := 1;
    
    protected procedure Invoke(c: Context; var cq: __SafeNativQueue; prev_ev: cl_event; tasks: List<Task>); override;
    begin
      var ec: ErrorCode;
      MakeBusy;
      
      self.ev := cl.CreateUserEvent(c._context, ec);
      ec.RaiseIfError;
      
      tasks += Task.Run(()->
      begin
        WaitAndRelease(prev_ev);
        self.res := self.f();
        
        cl.SetUserEventStatus(self.ev, CommandExecutionStatus.COMPLETE).RaiseIfError;
        SignalMWEvent;
      end);
      
    end;
    
    protected function InternalClone(muhs: Dictionary<object, object>; cache: Dictionary<CommandQueueBase, CommandQueueBase>): CommandQueueBase; override :=
    new CommandQueueHostFunc<T>(self.f);
    
  end;
  
{$endregion HostFunc}

{$region Multiusable}

type
  MultiusableCommandQueueNode<T>=class;
  
  // invoke_status:
  // 0 - выполнение не начато
  // 1 - выполнение начинается
  // 3 - выполнение прекращается
  
  MultiusableCommandQueueHub<T> = class
    public q: CommandQueueBase;
    
    public invoke_status := 0;
    public invoked_count := 0;
    
    public constructor(q: CommandQueueBase) :=
    self.q := q;
    
    public procedure OnNodeInvoked(c: Context; cq: __SafeNativQueue; tasks: List<Task>);
    public procedure OnNodeUnInvoked;
    
  end;
  
  MultiusableCommandQueueNode<T> = sealed class(CommandQueue<T>)
    public hub: MultiusableCommandQueueHub<T>;
    
    public constructor(hub: MultiusableCommandQueueHub<T>) :=
    self.hub := hub;
    
    protected function GetEstimateTaskCount(prev_hubs: HashSet<object>): integer; override;
    begin
      Result := 1;
      if prev_hubs.Add(hub) then
        Result += hub.q.GetEstimateTaskCount(prev_hubs);
    end;
    
    protected procedure Invoke(c: Context; var cq: __SafeNativQueue; prev_ev: cl_event; tasks: List<Task>); override;
    begin
      var ec: ErrorCode;
      MakeBusy;
      
      hub.OnNodeInvoked(c, cq, tasks);
      
      self.ev := cl.CreateUserEvent(c._context, ec);
      ec.RaiseIfError;
      
      var ev_lst := new List<cl_event>(2);
      if prev_ev<>cl_event.Zero then ev_lst += prev_ev;
      if hub.q.ev<>cl_event.Zero then ev_lst += hub.q.ev;
      
      tasks += Task.Run(()->
      begin
        if ev_lst.Count<>0 then WaitAndRelease(ev_lst);
        self.res := T(hub.q.GetRes);
        cl.SetUserEventStatus(self.ev, CommandExecutionStatus.COMPLETE).RaiseIfError;
        SignalMWEvent;
      end);
      
    end;
    
    protected procedure UnInvoke; override;
    begin
      inherited;
      hub.OnNodeUnInvoked;
    end;
    
    protected function InternalClone(muhs: Dictionary<object, object>; cache: Dictionary<CommandQueueBase, CommandQueueBase>): CommandQueueBase; override;
    begin
      var res_hub_o: object;
      var res_hub: MultiusableCommandQueueHub<T>;
      
      if muhs.TryGetValue(self.hub, res_hub_o) then
        res_hub := MultiusableCommandQueueHub&<T>(res_hub_o) else
      begin
        res_hub := new MultiusableCommandQueueHub<T>(self.hub.q.InternalCloneCached(muhs, cache));
        muhs.Add(self.hub, res_hub);
      end;
      
      Result := new MultiusableCommandQueueNode<T>(res_hub);
    end;
    
  end;
  
procedure MultiusableCommandQueueHub<T>.OnNodeInvoked(c: Context; cq: __SafeNativQueue; tasks: List<Task>);
begin
  case invoke_status of
    0: invoke_status := 1;
    2: raise new QueueDoubleInvokeException;
  end;
  
  if invoked_count=0 then q.Invoke(c,cq, cl_event.Zero, tasks);
  
  if q.ev<>cl_event.Zero then cl.RetainEvent(q.ev).RaiseIfError;
  invoked_count += 1;
  
end;

procedure MultiusableCommandQueueHub<T>.OnNodeUnInvoked;
begin
  case invoke_status of
    //0: raise new InvalidOperationException('Ошибка внутри модуля OpenCLABC: совершена попыта завершить не запущенную очередь. Сообщите, пожалуйста, разработчику OpenCLABC');
    1: invoke_status := 2;
  end;
  
  invoked_count -= 1;
  
  if invoked_count=0 then
  begin
    if q.ev<>cl_event.Zero then cl.ReleaseEvent(q.ev).RaiseIfError;
    invoke_status := 0;
    q.UnInvoke;
  end;
  
end;

//function CommandQueueBase.Multiusable(n: integer): array of CommandQueueBase;
//begin
//  var hub := new MultiusableCommandQueueHub<object>(self);
//  Result := ArrGen(n, i-> new MultiusableCommandQueueNode<object>(hub) as CommandQueueBase );
//end;
//
//function CommandQueueBase.Multiusable: ()->CommandQueueBase;
//begin
//  var hub := new MultiusableCommandQueueHub<object>(self);
//  Result := ()-> new MultiusableCommandQueueNode<object>(hub);
//end;

function CommandQueue<T>.Multiusable(n: integer): array of CommandQueue<T>;
begin
  var hub := new MultiusableCommandQueueHub<T>(self);
  Result := ArrGen(n, i-> new MultiusableCommandQueueNode<T>(hub) as CommandQueue<T> );
end;

function CommandQueue<T>.Multiusable: ()->CommandQueue<T>;
begin
  var hub := new MultiusableCommandQueueHub<T>(self);
  Result := ()-> new MultiusableCommandQueueNode<T>(hub) as CommandQueue<T>;
end;

{$endregion Multiusable}

{$region ThenConvert}

type
  CommandQueueResConvertor<T1,T2> = sealed class(CommandQueue<T2>)
    q: CommandQueueBase;
    f: (T1,Context)->T2;
    
    constructor(q: CommandQueueBase; f: (T1,Context)->T2);
    begin
      self.q := q;
      self.f := f;
    end;
    
    protected function GetEstimateTaskCount(prev_hubs: HashSet<object>): integer; override := 1 + q.GetEstimateTaskCount(prev_hubs);
    
    protected procedure Invoke(c: Context; var cq: __SafeNativQueue; prev_ev: cl_event; tasks: List<Task>); override;
    begin
      var ec: ErrorCode;
      MakeBusy;
      
      q.Invoke(c, cq, prev_ev, tasks);
      
      self.ev := cl.CreateUserEvent(c._context, ec);
      ec.RaiseIfError;
      
      tasks += Task.Run(()->
      begin
        if q.ev<>cl_event.Zero then WaitAndRelease(q.ev);
        self.res := self.f(T1(q.GetRes), c);
        
        cl.SetUserEventStatus(self.ev, CommandExecutionStatus.COMPLETE).RaiseIfError;
        SignalMWEvent;
      end);
      
    end;
    
    protected procedure UnInvoke; override;
    begin
      inherited;
      q.UnInvoke;
    end;
    
    protected function InternalClone(muhs: Dictionary<object, object>; cache: Dictionary<CommandQueueBase, CommandQueueBase>): CommandQueueBase; override :=
    new CommandQueueResConvertor<T1,T2>(self.q.InternalCloneCached(muhs, cache), self.f);
    
  end;
  
//function CommandQueueBase.ThenConvert<T>(f: (object,Context)->T) :=
//new CommandQueueResConvertor<object,T>(self, f);

function CommandQueue<T>.ThenConvert<T2>(f: (T,Context)->T2) :=
new CommandQueueResConvertor<T,T2>(self, f);

{$endregion ThenConvert}

{$region WaitFor}

type
  CommandQueueWait<T> = sealed class(CommandQueue<T>)
    public q, wait_source: CommandQueueBase;
    public allow_source_cloning: boolean;
    
    public constructor(q, wait_source: CommandQueueBase; allow_source_cloning: boolean);
    begin
      self.q := q;
      self.wait_source := wait_source;
      self.allow_source_cloning := allow_source_cloning;
    end;
    
    protected function GetEstimateTaskCount(prev_hubs: HashSet<object>): integer; override := 2;
    
    protected procedure Invoke(c: Context; var cq: __SafeNativQueue; prev_ev: cl_event; tasks: List<Task>); override;
    begin
      var ec: ErrorCode;
      MakeBusy;
      
      var uev := cl.CreateUserEvent(c._context, ec);
      ec.RaiseIfError;
      
      var ev_lst := new List<cl_event>(2);
      if prev_ev<>cl_event.Zero then ev_lst += prev_ev;
      ev_lst += wait_source.GetMWEvent(c._context);
      
      tasks += Task.Run(()->
      begin
        cl.WaitForEvents(ev_lst.Count, ev_lst.ToArray).RaiseIfError;
        cl.SetUserEventStatus(uev, CommandExecutionStatus.COMPLETE).RaiseIfError;
      end);
      
      q.Invoke(c, cq, uev, tasks);
      self.ev := cl.CreateUserEvent(c._context, ec);
      ec.RaiseIfError;
      
      tasks += Task.Run(()->
      begin
        cl.WaitForEvents(1,@q.ev).RaiseIfError;
        self.res := T( q.GetRes() );
        cl.SetUserEventStatus(self.ev, CommandExecutionStatus.COMPLETE).RaiseIfError;
        SignalMWEvent;
      end);
      
    end;
    
    protected procedure UnInvoke; override;
    begin
      inherited;
      q.UnInvoke;
    end;
    
    protected function InternalClone(muhs: Dictionary<object, object>; cache: Dictionary<CommandQueueBase, CommandQueueBase>): CommandQueueBase; override :=
    new CommandQueueWait<T>(
      self.q.InternalCloneCached(muhs, cache),
      allow_source_cloning?
        self.wait_source.InternalCloneCached(muhs, cache) :
        self.wait_source,
      self.allow_source_cloning
    );
    
  end;
  
function CommandQueue<T>.WaitFor(q: CommandQueueBase; allow_q_cloning: boolean): CommandQueue<T> :=
new CommandQueueWait<T>(self, q, allow_q_cloning);

{$endregion WaitFor}

{$region SyncList}

//ToDo лучше всё же хранить массив а не список... И для Async тоже
//ToDo базовые не_шаблонные типы, чтоб можно было сделать красивее в CommandQueueBase.operator+

type
  CommandQueueSyncList<T> = sealed class(CommandQueue<T>)
    public lst: List<CommandQueueBase>;
    
    public constructor :=
    lst := new List<CommandQueueBase>;
    
    public constructor(qs: List<CommandQueueBase>) :=
    lst := qs;
    
    public constructor(qs: array of CommandQueueBase) :=
    lst := qs.ToList;
    
    protected function GetEstimateTaskCount(prev_hubs: HashSet<object>): integer; override := 1 + lst.Sum(sq->sq.GetEstimateTaskCount(prev_hubs));
    
    protected procedure Invoke(c: Context; var cq: __SafeNativQueue; prev_ev: cl_event; tasks: List<Task>); override;
    begin
      var ec: ErrorCode;
      MakeBusy;
      
      foreach var sq in lst do
      begin
        sq.Invoke(c, cq, prev_ev, tasks);
        prev_ev := sq.ev;
      end;
      
      if prev_ev<>cl_event.Zero then
      begin
        self.ev := cl.CreateUserEvent(c._context, ec);
        ec.RaiseIfError;
        
        tasks += Task.Run(()->
        begin
          WaitAndRelease(prev_ev);
          self.res := T(lst[lst.Count-1].GetRes);
          cl.SetUserEventStatus(self.ev, CommandExecutionStatus.COMPLETE).RaiseIfError;
          SignalMWEvent;
        end);
        
      end else
      begin
        self.res := T(lst[lst.Count-1].GetRes);
        self.ev := cl_event.Zero;
        SignalMWEvent;
      end;
      
    end;
    
    protected procedure UnInvoke; override;
    begin
      inherited;
      foreach var q in lst do q.UnInvoke;
    end;
    
    protected function InternalClone(muhs: Dictionary<object, object>; cache: Dictionary<CommandQueueBase, CommandQueueBase>): CommandQueueBase; override :=
    new CommandQueueSyncList<T>(self.lst.ConvertAll(q->q.InternalCloneCached(muhs, cache)));
    
  end;
  CommandQueueTSyncList<T> = sealed class(CommandQueue<T>)
    public lst: List<CommandQueue<T>>;
    
    public constructor :=
    lst := new List<CommandQueue<T>>;
    
    public constructor(qs: List<CommandQueue<T>>) :=
    lst := qs;
    
    public constructor(qs: array of CommandQueue<T>) :=
    lst := qs.ToList;
    
    protected function GetEstimateTaskCount(prev_hubs: HashSet<object>): integer; override := 1 + lst.Sum(sq->sq.GetEstimateTaskCount(prev_hubs));
    
    protected procedure Invoke(c: Context; var cq: __SafeNativQueue; prev_ev: cl_event; tasks: List<Task>); override;
    begin
      var ec: ErrorCode;
      MakeBusy;
      
      foreach var sq in lst do
      begin
        sq.Invoke(c, cq, prev_ev, tasks);
        prev_ev := sq.ev;
      end;
      
      if prev_ev<>cl_event.Zero then
      begin
        self.ev := cl.CreateUserEvent(c._context, ec);
        ec.RaiseIfError;
        
        tasks += Task.Run(()->
        begin
          WaitAndRelease(prev_ev);
          self.res := lst[lst.Count-1].res;
          cl.SetUserEventStatus(self.ev, CommandExecutionStatus.COMPLETE).RaiseIfError;
          SignalMWEvent;
        end);
      end else
      begin
        self.res := lst[lst.Count-1].res;
        self.ev := cl_event.Zero;
        SignalMWEvent;
      end;
      
    end;
    
    protected procedure UnInvoke; override;
    begin
      inherited;
      foreach var q in lst do q.UnInvoke;
    end;
    
    protected function InternalClone(muhs: Dictionary<object, object>; cache: Dictionary<CommandQueueBase, CommandQueueBase>): CommandQueueBase; override :=
    new CommandQueueTSyncList<T>(self.lst.ConvertAll(q->CommandQueue&<T>(q.InternalCloneCached(muhs, cache))));
    
  end;
  CommandQueueCSyncList<TRes> = sealed class(CommandQueue<TRes>)
    public lst: List<CommandQueueBase>;
    public conv: Func<array of object,TRes>;
    
    public constructor :=
    lst := new List<CommandQueueBase>;
    
    public constructor(qs: List<CommandQueueBase>; conv: Func<array of object,TRes>);
    begin
      self.lst := qs;
      self.conv := conv;
    end;
    
    public constructor(qs: array of CommandQueueBase; conv: Func<array of object,TRes>);
    begin
      self.lst := qs.ToList;
      self.conv := conv;
    end;
    
    protected function GetEstimateTaskCount(prev_hubs: HashSet<object>): integer; override := 1 + lst.Sum(sq->sq.GetEstimateTaskCount(prev_hubs));
    
    protected procedure Invoke(c: Context; var cq: __SafeNativQueue; prev_ev: cl_event; tasks: List<Task>); override;
    begin
      var ec: ErrorCode;
      MakeBusy;
      
      foreach var sq in lst do
      begin
        sq.Invoke(c, cq, prev_ev, tasks);
        prev_ev := sq.ev;
      end;
      
      if prev_ev<>cl_event.Zero then
      begin
        self.ev := cl.CreateUserEvent(c._context, ec);
        ec.RaiseIfError;
        
        tasks += Task.Run(()->
        begin
          WaitAndRelease(prev_ev);
          
          var a := new object[lst.Count];
          for var i := 0 to lst.Count-1 do a[i] := lst[i].GetRes;
          self.res := conv(a);
          
          cl.SetUserEventStatus(self.ev, CommandExecutionStatus.COMPLETE).RaiseIfError;
          SignalMWEvent;
        end);
      end else
      begin
        
        var a := new object[lst.Count];
        for var i := 0 to lst.Count-1 do a[i] := lst[i].GetRes;
        self.res := conv(a);
        
        self.ev := cl_event.Zero;
        SignalMWEvent;
      end;
      
    end;
    
    protected procedure UnInvoke; override;
    begin
      inherited;
      foreach var q in lst do q.UnInvoke;
    end;
    
    protected function InternalClone(muhs: Dictionary<object, object>; cache: Dictionary<CommandQueueBase, CommandQueueBase>): CommandQueueBase; override :=
    new CommandQueueCSyncList<TRes>(self.lst.ConvertAll(q->q.InternalCloneCached(muhs, cache)), conv);
    
  end;
  CommandQueueCTSyncList<T,TRes> = sealed class(CommandQueue<TRes>)
    public lst: List<CommandQueue<T>>;
    public conv: Func<array of T,TRes>;
    
    public constructor :=
    lst := new List<CommandQueue<T>>;
    
    public constructor(qs: List<CommandQueue<T>>; conv: Func<array of T,TRes>);
    begin
      self.lst := qs;
      self.conv := conv;
    end;
    
    public constructor(qs: array of CommandQueue<T>; conv: Func<array of T,TRes>);
    begin
      self.lst := qs.ToList;
      self.conv := conv;
    end;
    
    protected function GetEstimateTaskCount(prev_hubs: HashSet<object>): integer; override := 1 + lst.Sum(sq->sq.GetEstimateTaskCount(prev_hubs));
    
    protected procedure Invoke(c: Context; var cq: __SafeNativQueue; prev_ev: cl_event; tasks: List<Task>); override;
    begin
      var ec: ErrorCode;
      MakeBusy;
      
      foreach var sq in lst do
      begin
        sq.Invoke(c, cq, prev_ev, tasks);
        prev_ev := sq.ev;
      end;
      
      if prev_ev<>cl_event.Zero then
      begin
        self.ev := cl.CreateUserEvent(c._context, ec);
        ec.RaiseIfError;
        
        tasks += Task.Run(()->
        begin
          WaitAndRelease(prev_ev);
          
          var a := new T[lst.Count];
          for var i := 0 to lst.Count-1 do a[i] := lst[i].res;
          self.res := conv(a);
          
          cl.SetUserEventStatus(self.ev, CommandExecutionStatus.COMPLETE).RaiseIfError;
          SignalMWEvent;
        end);
      end else
      begin
        
        var a := new T[lst.Count];
        for var i := 0 to lst.Count-1 do a[i] := lst[i].res;
        self.res := conv(a);
        
        self.ev := cl_event.Zero;
        SignalMWEvent;
      end;
      
    end;
    
    protected procedure UnInvoke; override;
    begin
      inherited;
      foreach var q in lst do q.UnInvoke;
    end;
    
    protected function InternalClone(muhs: Dictionary<object, object>; cache: Dictionary<CommandQueueBase, CommandQueueBase>): CommandQueueBase; override :=
    new CommandQueueCTSyncList<T,TRes>(self.lst.ConvertAll(q->CommandQueue&<T>(q.InternalCloneCached(muhs, cache))), conv);
    
  end;
  
static function CommandQueue<T>.operator+<T2>(q1: CommandQueue<T>; q2: CommandQueue<T2>): CommandQueue<T2>;
begin
  var ql1 := q1 as CommandQueueSyncList<T>;
  var ql2 := q2 as CommandQueueSyncList<T2>;
  var qtl1 := q1 as CommandQueueTSyncList<T>;
  var qtl2 := q2 as CommandQueueTSyncList<T2>;
  
  if (typeof(T)=typeof(T2)) and (ql1=nil) and (ql2=nil) then
  begin
    var res := new CommandQueueTSyncList<T2>;
    
    if qtl1<>nil then
      res.lst.AddRange(qtl1.lst.Cast&<CommandQueue<T2>>) else
      res.lst += q1 as object as CommandQueue<T2>;
    
    if qtl2<>nil then
      res.lst.AddRange(qtl2.lst) else
      res.lst += q2;
    
    Result := res;
  end else
  begin
    var res := new CommandQueueSyncList<T2>;
    
    if ql1<>nil then res.lst.AddRange(ql1.lst) else
    if qtl1<>nil then res.lst.AddRange(qtl1.lst.Cast&<CommandQueueBase>) else
      res.lst += q1 as CommandQueueBase;
    
    if ql2<>nil then res.lst.AddRange(ql2.lst) else
    if qtl2<>nil then res.lst.AddRange(qtl2.lst.Cast&<CommandQueueBase>) else
      res.lst += q2 as CommandQueueBase;
    
    Result := res;
  end;
  
end;

static function CommandQueueBase.operator+(q1, q2: CommandQueueBase): CommandQueueBase :=
new CommandQueueSyncList<object>(new CommandQueueBase[](q1,q2));

static function CommandQueueBase.operator+<T>(q1: CommandQueueBase; q2: CommandQueue<T>): CommandQueue<T> :=
new CommandQueueSyncList<T>(new CommandQueueBase[](q1, q2 as object as CommandQueueBase)); //ToDo #2119

{$endregion SyncList}

{$region AsyncList}

type
  CommandQueueAsyncList<T> = sealed class(CommandQueue<T>)
    public lst: List<CommandQueueBase>;
    
    public constructor :=
    lst := new List<CommandQueueBase>;
    
    public constructor(qs: List<CommandQueueBase>) :=
    lst := qs;
    
    public constructor(qs: array of CommandQueueBase) :=
    lst := qs.ToList;
    
    protected function GetEstimateTaskCount(prev_hubs: HashSet<object>): integer; override := 1 + lst.Sum(sq->sq.GetEstimateTaskCount(prev_hubs));
    
    protected procedure Invoke(c: Context; var cq: __SafeNativQueue; prev_ev: cl_event; tasks: List<Task>); override;
    begin
      var ec: ErrorCode;
      MakeBusy;
      
      var evs := new List<cl_event>(lst.Count);
      foreach var sq in lst do
      begin
        var scq: __SafeNativQueue := nil;
        sq.Invoke(c, scq, prev_ev, tasks);
        if sq.ev<>cl_event.Zero then evs += sq.ev;
      end;
      
      if evs.Count<>0 then
      begin
        cq := nil;
        
        self.ev := cl.CreateUserEvent(c._context, ec);
        ec.RaiseIfError;
        
        tasks += Task.Run(()->
        begin
          WaitAndRelease(evs);
          self.res := T(lst[lst.Count-1].GetRes);
          cl.SetUserEventStatus(self.ev, CommandExecutionStatus.COMPLETE).RaiseIfError;
          SignalMWEvent;
        end);
        
      end else
      begin
        self.res := T(lst[lst.Count-1].GetRes);
        self.ev := cl_event.Zero;
        SignalMWEvent;
      end;
      
    end;
    
    protected procedure UnInvoke; override;
    begin
      inherited;
      foreach var q in lst do q.UnInvoke;
    end;
    
    protected function InternalClone(muhs: Dictionary<object, object>; cache: Dictionary<CommandQueueBase, CommandQueueBase>): CommandQueueBase; override :=
    new CommandQueueAsyncList<T>(self.lst.ConvertAll(q->q.InternalCloneCached(muhs, cache)));
    
  end;
  CommandQueueTAsyncList<T> = sealed class(CommandQueue<T>)
    public lst: List<CommandQueue<T>>;
    
    public constructor :=
    lst := new List<CommandQueue<T>>;
    
    public constructor(qs: List<CommandQueue<T>>) :=
    lst := qs;
    
    public constructor(qs: array of CommandQueue<T>) :=
    lst := qs.ToList;
    
    protected function GetEstimateTaskCount(prev_hubs: HashSet<object>): integer; override := 1 + lst.Sum(sq->sq.GetEstimateTaskCount(prev_hubs));
    
    protected procedure Invoke(c: Context; var cq: __SafeNativQueue; prev_ev: cl_event; tasks: List<Task>); override;
    begin
      var ec: ErrorCode;
      MakeBusy;
      
      var evs := new List<cl_event>(lst.Count);
      foreach var sq in lst do
      begin
        var scq: __SafeNativQueue := nil;
        sq.Invoke(c, scq, prev_ev, tasks);
        if sq.ev<>cl_event.Zero then evs += sq.ev;
      end;
      
      if evs.Count<>0 then
      begin
        cq := nil;
        
        self.ev := cl.CreateUserEvent(c._context, ec);
        ec.RaiseIfError;
        
        tasks += Task.Run(()->
        begin
          WaitAndRelease(evs);
          self.res := lst[lst.Count-1].res;
          cl.SetUserEventStatus(self.ev, CommandExecutionStatus.COMPLETE).RaiseIfError;
          SignalMWEvent;
        end);
        
      end else
      begin
        self.res := lst[lst.Count-1].res;
        self.ev := cl_event.Zero;
        SignalMWEvent;
      end;
      
    end;
    
    protected procedure UnInvoke; override;
    begin
      inherited;
      foreach var q in lst do q.UnInvoke;
    end;
    
    protected function InternalClone(muhs: Dictionary<object, object>; cache: Dictionary<CommandQueueBase, CommandQueueBase>): CommandQueueBase; override :=
    new CommandQueueTAsyncList<T>(self.lst.ConvertAll(q->CommandQueue&<T>(q.InternalCloneCached(muhs, cache))));
    
  end;
  CommandQueueCAsyncList<TRes> = sealed class(CommandQueue<TRes>)
    public lst: List<CommandQueueBase>;
    public conv: Func<array of object,TRes>;
    
    public constructor :=
    lst := new List<CommandQueueBase>;
    
    public constructor(qs: List<CommandQueueBase>; conv: Func<array of object,TRes>);
    begin
      self.lst := qs;
      self.conv := conv;
    end;
    
    public constructor(qs: array of CommandQueueBase; conv: Func<array of object,TRes>);
    begin
      self.lst := qs.ToList;
      self.conv := conv;
    end;
    
    protected function GetEstimateTaskCount(prev_hubs: HashSet<object>): integer; override := 1 + lst.Sum(sq->sq.GetEstimateTaskCount(prev_hubs));
    
    protected procedure Invoke(c: Context; var cq: __SafeNativQueue; prev_ev: cl_event; tasks: List<Task>); override;
    begin
      var ec: ErrorCode;
      MakeBusy;
      
      var evs := new List<cl_event>(lst.Count);
      foreach var sq in lst do
      begin
        var scq: __SafeNativQueue := nil;
        sq.Invoke(c, scq, prev_ev, tasks);
        if sq.ev<>cl_event.Zero then evs += sq.ev;
      end;
      
      if evs.Count<>0 then
      begin
        cq := nil;
        
        self.ev := cl.CreateUserEvent(c._context, ec);
        ec.RaiseIfError;
        
        tasks += Task.Run(()->
        begin
          WaitAndRelease(evs);
          
          var a := new object[lst.Count];
          for var i := 0 to lst.Count-1 do a[i] := lst[i].GetRes;
          self.res := conv(a);
          
          cl.SetUserEventStatus(self.ev, CommandExecutionStatus.COMPLETE).RaiseIfError;
          SignalMWEvent;
        end);
        
      end else
      begin
        
        var a := new object[lst.Count];
        for var i := 0 to lst.Count-1 do a[i] := lst[i].GetRes;
        self.res := conv(a);
        
        self.ev := cl_event.Zero;
        SignalMWEvent;
      end;
      
    end;
    
    protected procedure UnInvoke; override;
    begin
      inherited;
      foreach var q in lst do q.UnInvoke;
    end;
    
    protected function InternalClone(muhs: Dictionary<object, object>; cache: Dictionary<CommandQueueBase, CommandQueueBase>): CommandQueueBase; override :=
    new CommandQueueCAsyncList<TRes>(self.lst.ConvertAll(q->q.InternalCloneCached(muhs, cache)), conv);
    
  end;
  CommandQueueCTAsyncList<T,TRes> = sealed class(CommandQueue<TRes>)
    public lst: List<CommandQueue<T>>;
    public conv: Func<array of T,TRes>;
    
    public constructor :=
    lst := new List<CommandQueue<T>>;
    
    public constructor(qs: List<CommandQueue<T>>; conv: Func<array of T,TRes>);
    begin
      self.lst := qs;
      self.conv := conv;
    end;
    
    public constructor(qs: array of CommandQueue<T>; conv: Func<array of T,TRes>);
    begin
      self.lst := qs.ToList;
      self.conv := conv;
    end;
    
    protected function GetEstimateTaskCount(prev_hubs: HashSet<object>): integer; override := 1 + lst.Sum(sq->sq.GetEstimateTaskCount(prev_hubs));
    
    protected procedure Invoke(c: Context; var cq: __SafeNativQueue; prev_ev: cl_event; tasks: List<Task>); override;
    begin
      var ec: ErrorCode;
      MakeBusy;
      
      var evs := new List<cl_event>(lst.Count);
      foreach var sq in lst do
      begin
        var scq: __SafeNativQueue := nil;
        sq.Invoke(c, scq, prev_ev, tasks);
        if sq.ev<>cl_event.Zero then evs += sq.ev;
      end;
      
      if evs.Count<>0 then
      begin
        cq := nil;
        
        self.ev := cl.CreateUserEvent(c._context, ec);
        ec.RaiseIfError;
        
        tasks += Task.Run(()->
        begin
          WaitAndRelease(evs);
          
          var a := new T[lst.Count];
          for var i := 0 to lst.Count-1 do a[i] := lst[i].res;
          self.res := conv(a);
          
          cl.SetUserEventStatus(self.ev, CommandExecutionStatus.COMPLETE).RaiseIfError;
          SignalMWEvent;
        end);
        
      end else
      begin
        
        var a := new T[lst.Count];
        for var i := 0 to lst.Count-1 do a[i] := lst[i].res;
        self.res := conv(a);
        
        self.ev := cl_event.Zero;
        SignalMWEvent;
      end;
      
    end;
    
    protected procedure UnInvoke; override;
    begin
      inherited;
      foreach var q in lst do q.UnInvoke;
    end;
    
    protected function InternalClone(muhs: Dictionary<object, object>; cache: Dictionary<CommandQueueBase, CommandQueueBase>): CommandQueueBase; override :=
    new CommandQueueCTAsyncList<T,TRes>(self.lst.ConvertAll(q->CommandQueue&<T>(q.InternalCloneCached(muhs, cache))), conv);
    
  end;
  
static function CommandQueue<T>.operator*<T2>(q1: CommandQueue<T>; q2: CommandQueue<T2>): CommandQueue<T2>;
begin
  var ql1 := q1 as CommandQueueAsyncList<T>;
  var ql2 := q2 as CommandQueueAsyncList<T2>;
  var qtl1 := q1 as CommandQueueTAsyncList<T>;
  var qtl2 := q2 as CommandQueueTAsyncList<T2>;
  
  if (typeof(T)=typeof(T2)) and (ql1=nil) and (ql2=nil) then
  begin
    var res := new CommandQueueTAsyncList<T2>;
    
    if qtl1<>nil then
      res.lst.AddRange(qtl1.lst.Cast&<CommandQueue<T2>>) else
      res.lst += q1 as object as CommandQueue<T2>;
    
    if qtl2<>nil then
      res.lst.AddRange(qtl2.lst) else
      res.lst += q2;
    
    Result := res;
  end else
  begin
    var res := new CommandQueueAsyncList<T2>;
    
    if ql1<>nil then res.lst.AddRange(ql1.lst) else
    if qtl1<>nil then res.lst.AddRange(qtl1.lst.Cast&<CommandQueueBase>) else
      res.lst += q1 as CommandQueueBase;
    
    if ql2<>nil then res.lst.AddRange(ql2.lst) else
    if qtl2<>nil then res.lst.AddRange(qtl2.lst.Cast&<CommandQueueBase>) else
      res.lst += q2 as CommandQueueBase;
    
    Result := res;
  end;
  
end;

static function CommandQueueBase.operator*(q1, q2: CommandQueueBase): CommandQueueBase :=
new CommandQueueAsyncList<object>(new CommandQueueBase[](q1,q2));

static function CommandQueueBase.operator*<T>(q1: CommandQueueBase; q2: CommandQueue<T>): CommandQueue<T> :=
new CommandQueueAsyncList<T>(new CommandQueueBase[](q1,q2 as object as CommandQueueBase)); //ToDo #2119

{$endregion AsyncList}

{$region GPUCommand}

{$region GPUCommandContainer}

constructor GPUCommandContainer<T>.Create(q: CommandQueue<T>) :=
self.res_q_hub := new MultiusableCommandQueueHub<T>(q);

function GPUCommandContainer<T>.GetNewResPlug: CommandQueue<T> :=
new MultiusableCommandQueueNode<T>( MultiusableCommandQueueHub&<T>(res_q_hub) );

function GPUCommandContainer<T>.GetEstimateTaskCount(prev_hubs: HashSet<object>): integer;
begin
  Result := commands.Sum(comm->comm.GetEstimateTaskCount(prev_hubs));
  if res_q_hub=nil then exit;
  Result += commands.Count; // каждая команда вызывает выполнение 1 ноды
  if prev_hubs.Contains(self.res_q_hub) then exit; // если команда использовала GetSizeQ - хаб уже посчитало
  Result += MultiusableCommandQueueHub&<Buffer>(self.res_q_hub).q.GetEstimateTaskCount(prev_hubs);
end;

procedure GPUCommandContainer<T>.Invoke(c: Context; var cq: __SafeNativQueue; prev_ev: cl_event; tasks: List<Task>);
begin
  MakeBusy;
  
  var new_plug: ()->CommandQueue<T>;
  if res_q_hub=nil then
  begin
    new_plug := ()->nil;
    last_center_plug := nil;
    OnEarlyInit(c);
  end else
  begin
    var plug := GetNewResPlug;
    plug.Invoke(c, cq, prev_ev, tasks);
    prev_ev := plug.ev;
    last_center_plug := plug;
    new_plug := GetNewResPlug;
  end;
  
  foreach var comm in commands do
  begin
    comm.Invoke(new_plug, res, c, cq, prev_ev, tasks);
    prev_ev := comm.ev;
  end;
  
  self.ev := prev_ev;
  if prev_ev=cl_event.Zero then
    SignalMWEvent else
    cl.SetEventCallback(prev_ev, CommandExecutionStatus.COMPLETE, CLSignalMWEvent, nil).RaiseIfError;
  
end;

{$endregion GPUCommandContainer}

{$region QueueCommand}

type
  QueueCommand<T> = sealed class(GPUCommand<T>)
    public q: CommandQueueBase;
    
    public constructor(q: CommandQueueBase) :=
    self.q := q;
    
    protected function GetEstimateTaskCount(prev_hubs: HashSet<object>): integer; override := q.GetEstimateTaskCount(prev_hubs);
    
    protected procedure Invoke(o_q: CommandQueue<T>; o: T; c: Context; var cq: __SafeNativQueue; prev_ev: cl_event; tasks: List<Task>); override;
    begin
      q.Invoke(c, cq, prev_ev, tasks);
      self.ev := q.ev;
    end;
    
    protected procedure UnInvoke; override;
    begin
//      inherited; // не надо, q уже удалило свой эвент
      self.ev := cl_event.Zero;
      q.UnInvoke;
    end;
    
    protected function Clone(muhs: Dictionary<object, object>; cache: Dictionary<CommandQueueBase, CommandQueueBase>): GPUCommand<T>; override :=
    new QueueCommand<T>(self.q.InternalCloneCached(muhs, cache) as CommandQueue<T>);
    
  end;
  
procedure GPUCommandContainer<T>.InternalAddQueue(q: CommandQueueBase) :=
commands += new QueueCommand<T>(q) as GPUCommand<T>;

{$endregion QueueCommand}

{$region ProcCommand}

type
  ProcCommand<T> = sealed class(GPUCommand<T>)
    public p: (T,Context)->();
    public last_o_q: CommandQueue<T>;
    
    public constructor(p: (T,Context)->()) :=
    self.p := p;
    
    protected function GetEstimateTaskCount(prev_hubs: HashSet<object>): integer; override := 1;
    
    protected procedure Invoke(o_q: CommandQueue<T>; o: T; c: Context; var cq: __SafeNativQueue; prev_ev: cl_event; tasks: List<Task>); override;
    begin
      var ec: ErrorCode;
      
      self.last_o_q := o_q;
      if o_q<>nil then
      begin
        o_q.Invoke(c, cq, prev_ev, tasks);
        prev_ev := o_q.ev;
      end;
      
      self.ev := cl.CreateUserEvent(c._context, ec);
      ec.RaiseIfError;
      
      tasks += Task.Run(()->
      begin
        WaitAndRelease(prev_ev);
        self.p(
          (o_q=nil?o:o_q.res), c
        );
        cl.SetUserEventStatus(self.ev, CommandExecutionStatus.COMPLETE).RaiseIfError;
      end);
      
    end;
    
    protected procedure UnInvoke; override :=
    if last_o_q<>nil then
    begin
      last_o_q.UnInvoke;
      last_o_q := nil;
    end;
    
    protected function Clone(muhs: Dictionary<object, object>; cache: Dictionary<CommandQueueBase, CommandQueueBase>): GPUCommand<T>; override :=
    new ProcCommand<T>(self.p);
    
  end;
  
procedure GPUCommandContainer<T>.InternalAddProc(p: (T,Context)->()) :=
commands += new ProcCommand<T>(p) as GPUCommand<T>;

{$endregion ProcCommand}

{$region WaitCommand}

type
  WaitCommand<T> = sealed class(GPUCommand<T>)
    public wait_source: CommandQueueBase;
    public allow_q_cloning: boolean;
    
    public constructor(wait_source: CommandQueueBase; allow_q_cloning: boolean);
    begin
      self.wait_source := wait_source;
      self.allow_q_cloning := allow_q_cloning;
    end;
    
    protected function GetEstimateTaskCount(prev_hubs: HashSet<object>): integer; override := 1;
    
    protected procedure Invoke(o_q: CommandQueue<T>; o: T; c: Context; var cq: __SafeNativQueue; prev_ev: cl_event; tasks: List<Task>); override;
    begin
      var ec: ErrorCode;
      
      self.ev := cl.CreateUserEvent(c._context, ec);
      ec.RaiseIfError;
      
      var ev_lst := new List<cl_event>(2);
      if prev_ev<>cl_event.Zero then ev_lst += prev_ev;
      ev_lst += wait_source.GetMWEvent(c._context);
      
      tasks += Task.Run(()->
      begin
        cl.WaitForEvents(ev_lst.Count, ev_lst.ToArray).RaiseIfError;
        cl.SetUserEventStatus(self.ev, CommandExecutionStatus.COMPLETE).RaiseIfError;
      end);
      
    end;
    
    protected procedure UnInvoke; override := exit;
    
    protected function Clone(muhs: Dictionary<object, object>; cache: Dictionary<CommandQueueBase, CommandQueueBase>): GPUCommand<T>; override :=
    new WaitCommand<T>(
      allow_q_cloning?
        wait_source.InternalCloneCached(muhs, cache) :
        wait_source,
      allow_q_cloning
    );
    
  end;
  
procedure GPUCommandContainer<T>.InternalAddWait(q: CommandQueueBase; allow_q_cloning: boolean) :=
commands.Add( new WaitCommand<T>(q, allow_q_cloning) );

{$endregion WaitCommand}

{$region Special GPUCommand's}

type
  /// От этого наследуют все типы вызывающие cl.Enqueue* методы
  DirectGPUCommandBase<T> = abstract class(GPUCommand<T>)
    public last_o_q: CommandQueue<T>;
    
    {$region def}
    
    protected function GetSubQCount: integer; abstract;
    protected function EnmrSubQ: sequence of CommandQueueBase; abstract;
    
    protected procedure LowerEnqueueSelf(c: Context; cq: cl_command_queue; o: T; ev_c: integer; prev_ev, res_ev: ^cl_event); abstract;
    
    {$endregion def}
    
    {$region sub implementation}
    
    protected procedure Invoke(o_q: CommandQueue<T>; o: T; c: Context; var cq: __SafeNativQueue; prev_ev: cl_event; tasks: List<Task>); override;
    begin
      self.last_o_q := o_q;
      if o_q<>nil then
      begin
        o_q.Invoke(c, cq, prev_ev, tasks);
        prev_ev := cl_event.Zero;
      end;
      
      var ev_lst := new List<cl_event>(GetSubQCount);
      foreach var sq in EnmrSubQ do
      begin
        sq.InvokeNewQ(c, tasks);
        ev_lst += sq.ev;
      end;
      ev_lst.RemoveAll(ev->ev=cl_event.Zero);
      
      if cq=nil then cq := new __SafeNativQueue(c._context,c._device); // если предыдущий Invoke содержал асинхронный EnqueueSelf или вообще был в другой ветке умноженных очередей
      
      if (ev_lst.Count=0) and ((o_q=nil) or (o_q.ev=cl_event.Zero)) then
      begin
        var lo := o_q=nil ? o : o_q.res;
        
        if prev_ev=cl_event.Zero then
          LowerEnqueueSelf(c, cq.q, lo, 0,      nil, @self.ev) else
          LowerEnqueueSelf(c, cq.q, lo, 1, @prev_ev, @self.ev);
        
      end else
      begin
        var ec: ErrorCode;
        self.ev := cl.CreateUserEvent(c._context, ec);
        ec.RaiseIfError;
        
        var ncq := cq.q;
        tasks += Task.Run(()->
        begin
          var lo: T;
          
          if last_o_q=nil then lo := o else
          begin
            WaitAndRelease(last_o_q.ev);
            lo := last_o_q.res;
          end;
          
          if ev_lst.Count<>0 then WaitAndRelease(ev_lst);
          
          var buff_ev: cl_event;
          if prev_ev=cl_event.Zero then
            LowerEnqueueSelf(c, ncq, lo, 0,      nil, @buff_ev) else
            LowerEnqueueSelf(c, ncq, lo, 1, @prev_ev, @buff_ev);
          WaitAndRelease(buff_ev);
          
          cl.SetUserEventStatus(self.ev, CommandExecutionStatus.COMPLETE).RaiseIfError;
        end);
        
        cq := nil; // асинхронное EnqueueSelf, далее придётся создать новую очередь
      end;
      
    end;
    
    protected function GetEstimateTaskCount(prev_hubs: HashSet<object>): integer; override := EnmrSubQ.Sum(sq->sq.GetEstimateTaskCount(prev_hubs)) + 1;
    
    protected procedure UnInvoke; override;
    begin
      if last_o_q<>nil then
      begin
        last_o_q.UnInvoke;
        last_o_q := nil;
      end;
      foreach var sq in EnmrSubQ do sq.UnInvoke;
    end;
    
    {$endregion sub implementation}
    
  end;
  
{$endregion Special GPUCommand's}

{$endregion GPUCommand}

{$region Buffer}

{$region BufferCommandQueue}

//ToDo
function костыль_BufferCommandQueue_Create(b: Buffer; c: Context): Buffer;
begin
  if b.memobj=cl_mem.Zero then b.Init(c);
  Result := b;
end;

constructor BufferCommandQueue.Create(q: CommandQueue<Buffer>) :=
inherited Create(q.ThenConvert(костыль_BufferCommandQueue_Create));

function BufferCommandQueue.GetSizeQ: CommandQueue<integer> :=
self.res_q_hub=nil?integer(res.sz.ToUInt32) :
self.GetNewResPlug().ThenConvert(b->integer(b.sz.ToUInt32));

procedure BufferCommandQueue.OnEarlyInit(c: Context) :=
if res.memobj=cl_mem.Zero then res.Init(c);

function BufferCommandQueue.InternalClone(muhs: Dictionary<object, object>; cache: Dictionary<CommandQueueBase, CommandQueueBase>): CommandQueueBase;
begin
  var res := new BufferCommandQueue(self.res);
  
  if self.res_q_hub<>nil then
  begin
    var hub := MultiusableCommandQueueHub&<Buffer>(self.res_q_hub);
    
    res.res_q_hub := new MultiusableCommandQueueHub<Buffer>(CommandQueue&<Buffer>(
      hub.q.InternalCloneCached(muhs, cache)
    ));
    
    muhs.Add(self.res_q_hub, res.res_q_hub);
  end;
  
  res.commands.Capacity := self.commands.Capacity;
  foreach var comm in self.commands do res.commands += comm.Clone(muhs, cache);
  
  Result := res;
end;

{$endregion BufferCommandQueue}

{$region Misc}

type
  BufferCommandBase = abstract class(DirectGPUCommandBase<Buffer>)
    
    protected procedure EnqueueSelf(c: Context; cq: cl_command_queue; mem: cl_mem; ev_c: integer; prev_ev, res_ev: ^cl_event); abstract;
    
    protected procedure LowerEnqueueSelf(c: Context; cq: cl_command_queue; b: Buffer; ev_c: integer; prev_ev, res_ev: ^cl_event); override :=
    EnqueueSelf(c, cq, b.memobj, ev_c, prev_ev, res_ev);
    
  end;
  
{$endregion Misc}

{$region Write}

type
  BufferCommandWriteData = sealed class(BufferCommandBase)
    public ptr: CommandQueue<IntPtr>;
    public offset, len: CommandQueue<integer>;
    
    public constructor(ptr: CommandQueue<IntPtr>; offset, len: CommandQueue<integer>);
    begin
      self.ptr := ptr;
      self.offset := offset;
      self.len := len;
    end;
    
    protected function GetSubQCount: integer; override := 3;
    protected function EnmrSubQ: sequence of CommandQueueBase; override;
    begin
      yield ptr;
      yield offset;
      yield len;
    end;
    
    protected procedure EnqueueSelf(c: Context; cq: cl_command_queue; mem: cl_mem; ev_c: integer; prev_ev, res_ev: ^cl_event); override :=
    cl.EnqueueWriteBuffer(cq, mem, 0, new UIntPtr(offset.res), new UIntPtr(len.res), ptr.res, ev_c, prev_ev, res_ev).RaiseIfError;
    
    protected function Clone(muhs: Dictionary<object, object>; cache: Dictionary<CommandQueueBase, CommandQueueBase>): GPUCommand<Buffer>; override :=
    new BufferCommandWriteData(
      CommandQueue&<IntPtr> (self.ptr   .InternalCloneCached(muhs, cache)),
      CommandQueue&<integer>(self.offset.InternalCloneCached(muhs, cache)),
      CommandQueue&<integer>(self.len   .InternalCloneCached(muhs, cache))
    );
    
  end;
  BufferCommandWriteArray = sealed class(BufferCommandBase)
    public a: CommandQueue<&Array>;
    public offset, len: CommandQueue<integer>;
    
    public constructor(a: CommandQueue<&Array>; offset, len: CommandQueue<integer>);
    begin
      self.a := a;
      self.offset := offset;
      self.len := len;
    end;
    
    protected function GetSubQCount: integer; override := 3;
    protected function EnmrSubQ: sequence of CommandQueueBase; override;
    begin
      yield a;
      yield offset;
      yield len;
    end;
    
    protected procedure EnqueueSelf(c: Context; cq: cl_command_queue; mem: cl_mem; ev_c: integer; prev_ev, res_ev: ^cl_event); override;
    begin
      var hnd := new CLGCHandle(a.res);
      cl.EnqueueWriteBuffer(cq, mem, 0, new UIntPtr(offset.res), new UIntPtr(len.res), hnd.Ptr, ev_c, prev_ev, res_ev).RaiseIfError;
      cl.SetEventCallback(res_ev^, CommandExecutionStatus.COMPLETE, hnd.CLFree, nil).RaiseIfError;
    end;
    
    protected function Clone(muhs: Dictionary<object, object>; cache: Dictionary<CommandQueueBase, CommandQueueBase>): GPUCommand<Buffer>; override :=
    new BufferCommandWriteArray(
      CommandQueue&<&Array> (self.a     .InternalCloneCached(muhs, cache)),
      CommandQueue&<integer>(self.offset.InternalCloneCached(muhs, cache)),
      CommandQueue&<integer>(self.len   .InternalCloneCached(muhs, cache))
    );
    
  end;
  BufferCommandWriteValue = sealed class(BufferCommandBase)
    public ptr: CommandQueue<IntPtr>;
    public offset, len: CommandQueue<integer>;
    
    public constructor(ptr: CommandQueue<IntPtr>; offset, len: CommandQueue<integer>);
    begin
      self.ptr := ptr;
      self.offset := offset;
      self.len := len;
    end;
    
    protected function GetSubQCount: integer; override := 3;
    protected function EnmrSubQ: sequence of CommandQueueBase; override;
    begin
      yield ptr;
      yield offset;
      yield len;
    end;
    
    protected procedure CLFreeMem(ev: cl_event; status: CommandExecutionStatus; data: pointer) := Marshal.FreeHGlobal(ptr.res);
    
    protected procedure EnqueueSelf(c: Context; cq: cl_command_queue; mem: cl_mem; ev_c: integer; prev_ev, res_ev: ^cl_event); override;
    begin
      cl.EnqueueWriteBuffer(cq, mem, 0, new UIntPtr(offset.res), new UIntPtr(len.res), ptr.res, ev_c, prev_ev, res_ev).RaiseIfError;
      cl.SetEventCallback(res_ev^, CommandExecutionStatus.COMPLETE, CLFreeMem, nil).RaiseIfError;
    end;
    
    protected function Clone(muhs: Dictionary<object, object>; cache: Dictionary<CommandQueueBase, CommandQueueBase>): GPUCommand<Buffer>; override :=
    new BufferCommandWriteValue(
      CommandQueue&<IntPtr> (self.ptr   .InternalCloneCached(muhs, cache)),
      CommandQueue&<integer>(self.offset.InternalCloneCached(muhs, cache)),
      CommandQueue&<integer>(self.len   .InternalCloneCached(muhs, cache))
    );
    
  end;
  
  
function BufferCommandQueue.AddWriteData(ptr: CommandQueue<IntPtr>; offset, len: CommandQueue<integer>) :=
AddCommand(new BufferCommandWriteData(ptr, offset, len));

function BufferCommandQueue.AddWriteArray(a: CommandQueue<&Array>; offset, len: CommandQueue<integer>) :=
AddCommand(new BufferCommandWriteArray(a, offset, len));


function BufferCommandQueue.AddWriteValue<TRecord>(val: TRecord; offset: CommandQueue<integer>): BufferCommandQueue;
begin
  var sz := Marshal.SizeOf&<TRecord>;
  var ptr := Marshal.AllocHGlobal(sz);
  var typed_ptr: ^TRecord := pointer(ptr);
  typed_ptr^ := val;
  Result := AddCommand(new BufferCommandWriteValue(ptr, offset,Marshal.SizeOf&<TRecord>));
end;

function BufferCommandQueue.AddWriteValue<TRecord>(val: CommandQueue<TRecord>; offset: CommandQueue<integer>) :=
AddCommand(new BufferCommandWriteValue(
  val.ThenConvert&<IntPtr>(vval-> //ToDo #2067
  begin
    var sz := Marshal.SizeOf&<TRecord>;
    var ptr := Marshal.AllocHGlobal(sz);
    var typed_ptr: ^TRecord := pointer(ptr);
    var костыль_ptr: ^TRecord := pointer(@vval); //ToDo #2068
    typed_ptr^ := костыль_ptr^; // := vval
    Result := ptr;
  end),
  offset,
  Marshal.SizeOf&<TRecord>
));

{$endregion Write}

{$region Read}

type
  BufferCommandReadData = sealed class(BufferCommandBase)
    public ptr: CommandQueue<IntPtr>;
    public offset, len: CommandQueue<integer>;
    
    public constructor(ptr: CommandQueue<IntPtr>; offset, len: CommandQueue<integer>);
    begin
      self.ptr := ptr;
      self.offset := offset;
      self.len := len;
    end;
    
    protected function GetSubQCount: integer; override := 3;
    protected function EnmrSubQ: sequence of CommandQueueBase; override;
    begin
      yield ptr;
      yield offset;
      yield len;
    end;
    
    protected procedure EnqueueSelf(c: Context; cq: cl_command_queue; mem: cl_mem; ev_c: integer; prev_ev, res_ev: ^cl_event); override :=
    cl.EnqueueReadBuffer(cq, mem, 0, new UIntPtr(offset.res), new UIntPtr(len.res), ptr.res, ev_c, prev_ev, res_ev).RaiseIfError;
    
    protected function Clone(muhs: Dictionary<object, object>; cache: Dictionary<CommandQueueBase, CommandQueueBase>): GPUCommand<Buffer>; override :=
    new BufferCommandReadData(
      CommandQueue&<IntPtr> (self.ptr   .InternalCloneCached(muhs, cache)),
      CommandQueue&<integer>(self.offset.InternalCloneCached(muhs, cache)),
      CommandQueue&<integer>(self.len   .InternalCloneCached(muhs, cache))
    );
    
  end;
  BufferCommandReadArray = sealed class(BufferCommandBase)
    public a: CommandQueue<&Array>;
    public offset, len: CommandQueue<integer>;
    
    public constructor(a: CommandQueue<&Array>; offset, len: CommandQueue<integer>);
    begin
      self.a := a;
      self.offset := offset;
      self.len := len;
    end;
    
    protected function GetSubQCount: integer; override := 3;
    protected function EnmrSubQ: sequence of CommandQueueBase; override;
    begin
      yield a;
      yield offset;
      yield len;
    end;
    
    protected procedure EnqueueSelf(c: Context; cq: cl_command_queue; mem: cl_mem; ev_c: integer; prev_ev, res_ev: ^cl_event); override;
    begin
      var hnd := new CLGCHandle(a.res);
      cl.EnqueueReadBuffer(cq, mem, 0, new UIntPtr(offset.res), new UIntPtr(len.res), hnd.Ptr, ev_c, prev_ev, res_ev).RaiseIfError;
      cl.SetEventCallback(res_ev^, CommandExecutionStatus.COMPLETE, hnd.CLFree, nil).RaiseIfError;
    end;
    
    protected function Clone(muhs: Dictionary<object, object>; cache: Dictionary<CommandQueueBase, CommandQueueBase>): GPUCommand<Buffer>; override :=
    new BufferCommandReadArray(
      CommandQueue&<&Array> (self.a     .InternalCloneCached(muhs, cache)),
      CommandQueue&<integer>(self.offset.InternalCloneCached(muhs, cache)),
      CommandQueue&<integer>(self.len   .InternalCloneCached(muhs, cache))
    );
    
  end;
  
  
function BufferCommandQueue.AddReadData(ptr: CommandQueue<IntPtr>; offset, len: CommandQueue<integer>) :=
AddCommand(new BufferCommandReadData(ptr, offset, len));

function BufferCommandQueue.AddReadArray(a: CommandQueue<&Array>; offset, len: CommandQueue<integer>) :=
AddCommand(new BufferCommandReadArray(a, offset, len));

{$endregion Read}

{$region Fill}

type
  BufferCommandDataFill = sealed class(BufferCommandBase)
    public ptr: CommandQueue<IntPtr>;
    public pattern_len, offset, len: CommandQueue<integer>;
    public b_q: CommandQueue<Buffer>;
    
    public constructor(ptr: CommandQueue<IntPtr>; pattern_len, offset, len: CommandQueue<integer>);
    begin
      self.ptr := ptr;
      self.pattern_len := pattern_len;
      self.offset := offset;
      self.len := len;
    end;
    
    protected function GetSubQCount: integer; override := 4;
    protected function EnmrSubQ: sequence of CommandQueueBase; override;
    begin
      yield ptr;
      yield pattern_len;
      yield offset;
      yield len;
    end;
    
    protected procedure EnqueueSelf(c: Context; cq: cl_command_queue; mem: cl_mem; ev_c: integer; prev_ev, res_ev: ^cl_event); override :=
    cl.EnqueueFillBuffer(cq, mem, ptr.res, new UIntPtr(pattern_len.res), new UIntPtr(offset.res), new UIntPtr(len.res), ev_c, prev_ev, res_ev).RaiseIfError;
    
    protected function Clone(muhs: Dictionary<object, object>; cache: Dictionary<CommandQueueBase, CommandQueueBase>): GPUCommand<Buffer>; override :=
    new BufferCommandDataFill(
      CommandQueue&<IntPtr> (self.ptr         .InternalCloneCached(muhs, cache)),
      CommandQueue&<integer>(self.pattern_len .InternalCloneCached(muhs, cache)),
      CommandQueue&<integer>(self.offset      .InternalCloneCached(muhs, cache)),
      CommandQueue&<integer>(self.len         .InternalCloneCached(muhs, cache))
    );
    
  end;
  BufferCommandArrayFill = sealed class(BufferCommandBase)
    public a: CommandQueue<&Array>;
    public offset, len: CommandQueue<integer>;
    public b_q: CommandQueue<Buffer>;
    
    public constructor(a: CommandQueue<&Array>; offset, len: CommandQueue<integer>);
    begin
      self.a := a;
      self.offset := offset;
      self.len := len;
    end;
    
    protected function GetSubQCount: integer; override := 3;
    protected function EnmrSubQ: sequence of CommandQueueBase; override;
    begin
      yield a;
      yield offset;
      yield len;
    end;
    
    protected procedure EnqueueSelf(c: Context; cq: cl_command_queue; mem: cl_mem; ev_c: integer; prev_ev, res_ev: ^cl_event); override;
    begin
      var hnd := new CLGCHandle(a.res);
      var pattern_sz := Marshal.SizeOf(a.res.GetType.GetElementType) * a.res.Length;
      cl.EnqueueFillBuffer(cq, mem, hnd.Ptr, new UIntPtr(pattern_sz), new UIntPtr(offset.res), new UIntPtr(len.res), ev_c, prev_ev, res_ev).RaiseIfError;
      cl.SetEventCallback(res_ev^, CommandExecutionStatus.COMPLETE, hnd.CLFree, nil).RaiseIfError;
    end;
    
    protected function Clone(muhs: Dictionary<object, object>; cache: Dictionary<CommandQueueBase, CommandQueueBase>): GPUCommand<Buffer>; override :=
    new BufferCommandArrayFill(
      CommandQueue&<&Array> (self.a           .InternalCloneCached(muhs, cache)),
      CommandQueue&<integer>(self.offset      .InternalCloneCached(muhs, cache)),
      CommandQueue&<integer>(self.len         .InternalCloneCached(muhs, cache))
    );
    
  end;
  BufferCommandValueFill = sealed class(BufferCommandBase)
    public ptr: CommandQueue<IntPtr>;
    public pattern_len, offset, len: CommandQueue<integer>;
    
    public constructor(ptr: CommandQueue<IntPtr>; pattern_len, offset, len: CommandQueue<integer>);
    begin
      self.ptr := ptr;
      self.pattern_len := pattern_len;
      self.offset := offset;
      self.len := len;
    end;
    
    protected function GetSubQCount: integer; override := 4;
    protected function EnmrSubQ: sequence of CommandQueueBase; override;
    begin
      yield ptr;
      yield pattern_len;
      yield offset;
      yield len;
    end;
    
    protected procedure CLFreeMem(ev: cl_event; status: CommandExecutionStatus; data: pointer) := Marshal.FreeHGlobal(ptr.res);
    
    protected procedure EnqueueSelf(c: Context; cq: cl_command_queue; mem: cl_mem; ev_c: integer; prev_ev, res_ev: ^cl_event); override;
    begin
      cl.EnqueueFillBuffer(cq, mem, ptr.res, new UIntPtr(pattern_len.res), new UIntPtr(offset.res), new UIntPtr(len.res), ev_c, prev_ev, res_ev).RaiseIfError;
      cl.SetEventCallback(res_ev^, CommandExecutionStatus.COMPLETE, CLFreeMem, nil).RaiseIfError;
    end;
    
    protected function Clone(muhs: Dictionary<object, object>; cache: Dictionary<CommandQueueBase, CommandQueueBase>): GPUCommand<Buffer>; override :=
    new BufferCommandValueFill(
      CommandQueue&<IntPtr> (self.ptr         .InternalCloneCached(muhs, cache)),
      CommandQueue&<integer>(self.pattern_len .InternalCloneCached(muhs, cache)),
      CommandQueue&<integer>(self.offset      .InternalCloneCached(muhs, cache)),
      CommandQueue&<integer>(self.len         .InternalCloneCached(muhs, cache))
    );
    
  end;
  
  
function BufferCommandQueue.AddFillData(ptr: CommandQueue<IntPtr>; pattern_len, offset, len: CommandQueue<integer>) :=
AddCommand(new BufferCommandDataFill(ptr,pattern_len, offset,len));

function BufferCommandQueue.AddFillArray(a: CommandQueue<&Array>; offset, len: CommandQueue<integer>) :=
AddCommand(new BufferCommandArrayFill(a, offset,len));


function BufferCommandQueue.AddFillValue<TRecord>(val: TRecord; offset, len: CommandQueue<integer>): BufferCommandQueue;
begin
  var sz := Marshal.SizeOf&<TRecord>;
  var ptr := Marshal.AllocHGlobal(sz);
  var typed_ptr: ^TRecord := pointer(ptr);
  typed_ptr^ := val;
  Result := AddCommand(new BufferCommandValueFill(ptr,Marshal.SizeOf&<TRecord>, offset,len));
end;

function BufferCommandQueue.AddFillValue<TRecord>(val: CommandQueue<TRecord>; offset, len: CommandQueue<integer>) :=
AddCommand(new BufferCommandValueFill(
  val.ThenConvert&<IntPtr>(vval-> //ToDo #2067
  begin
    var sz := Marshal.SizeOf&<TRecord>;
    var ptr := Marshal.AllocHGlobal(sz);
    var typed_ptr: ^TRecord := pointer(ptr);
    var костыль_ptr: ^TRecord := pointer(@vval); //ToDo #2068
    typed_ptr^ := костыль_ptr^;
    Result := ptr;
  end),
  Marshal.SizeOf&<TRecord>,
  offset, len
));

{$endregion Fill}

{$region Copy}

type
  BufferCommandCopy = sealed class(BufferCommandBase)
    public f_buf, t_buf: CommandQueue<Buffer>;
    public f_pos, t_pos, len: CommandQueue<integer>;
    
    public constructor(f_buf, t_buf: CommandQueue<Buffer>; f_pos, t_pos, len: CommandQueue<integer>);
    begin
      self.f_buf := f_buf;
      self.t_buf := t_buf;
      self.f_pos := f_pos;
      self.t_pos := t_pos;
      self.len := len;
    end;
    
    protected function GetSubQCount: integer; override := 5;
    protected function EnmrSubQ: sequence of CommandQueueBase; override;
    begin
      yield f_buf;
      yield t_buf;
      yield f_pos;
      yield t_pos;
      yield len;
    end;
    
    protected procedure EnqueueSelf(c: Context; cq: cl_command_queue; mem: cl_mem; ev_c: integer; prev_ev, res_ev: ^cl_event); override;
    begin
      if f_buf.res.memobj=cl_mem.Zero then f_buf.res.Init(c);
      if t_buf.res.memobj=cl_mem.Zero then t_buf.res.Init(c);
      cl.EnqueueCopyBuffer(cq, f_buf.res.memobj, t_buf.res.memobj, new UIntPtr(f_pos.res), new UIntPtr(t_pos.res), new UIntPtr(len.res), ev_c, prev_ev, res_ev).RaiseIfError;
    end;
    
    protected function Clone(muhs: Dictionary<object, object>; cache: Dictionary<CommandQueueBase, CommandQueueBase>): GPUCommand<Buffer>; override :=
    new BufferCommandCopy(
      CommandQueue&<Buffer> (self.f_buf .InternalCloneCached(muhs, cache)),
      CommandQueue&<Buffer> (self.t_buf .InternalCloneCached(muhs, cache)),
      CommandQueue&<integer>(self.f_pos .InternalCloneCached(muhs, cache)),
      CommandQueue&<integer>(self.t_pos .InternalCloneCached(muhs, cache)),
      CommandQueue&<integer>(self.len   .InternalCloneCached(muhs, cache))
    );
    
  end;

function BufferCommandQueue.AddCopyFrom(b: CommandQueue<Buffer>; from, &to, len: CommandQueue<integer>) :=
AddCommand(new BufferCommandCopy(b,res_q_hub=nil?res:self.GetNewResPlug, from,&to, len));

function BufferCommandQueue.AddCopyTo(b: CommandQueue<Buffer>; from, &to, len: CommandQueue<integer>) :=
AddCommand(new BufferCommandCopy(res_q_hub=nil?res:self.GetNewResPlug,b, &to,from, len));

{$endregion Copy}

{$endregion Buffer}

{$region Kernel}

{$region KernelCommandQueue}

function KernelCommandQueue.InternalClone(muhs: Dictionary<object, object>; cache: Dictionary<CommandQueueBase, CommandQueueBase>): CommandQueueBase;
begin
  var res := new KernelCommandQueue(self.res);
  
  if self.res_q_hub<>nil then
  begin
    var hub := MultiusableCommandQueueHub&<Kernel>(self.res_q_hub);
    
    res.res_q_hub := new MultiusableCommandQueueHub<Kernel>(CommandQueue&<Kernel>(
      hub.q.InternalCloneCached(muhs, cache)
    ));
    
    muhs.Add(self.res_q_hub, res.res_q_hub);
  end;
  
  res.commands.Capacity := self.commands.Capacity;
  foreach var comm in self.commands do res.commands += comm.Clone(muhs, cache);
  
  Result := res;
end;

{$endregion KernelCommandQueue}

{$region Misc}

type
  KernelCommandBase = abstract class(DirectGPUCommandBase<Kernel>)
    
    protected procedure EnqueueSelf(c: Context; cq: cl_command_queue; k: cl_kernel; ev_c: integer; prev_ev, res_ev: ^cl_event); abstract;
    
    protected procedure LowerEnqueueSelf(c: Context; cq: cl_command_queue; k: Kernel; ev_c: integer; prev_ev, res_ev: ^cl_event); override :=
    EnqueueSelf(c, cq, k._kernel, ev_c, prev_ev, res_ev);
    
  end;
  
{$endregion Misc}

{$region Exec}

type
  KernelCommandExec = sealed class(KernelCommandBase)
    public work_szs_q: CommandQueue<array of UIntPtr>;
    public args_q: array of CommandQueue<Buffer>;
    
    public constructor(work_szs_q: CommandQueue<array of UIntPtr>; args: array of CommandQueue<Buffer>);
    begin
      self.work_szs_q := work_szs_q;
      self.args_q := args;
    end;
    
    protected function GetSubQCount: integer; override := 1 + args_q.Length;
    protected function EnmrSubQ: sequence of CommandQueueBase; override;
    begin
      yield work_szs_q;
      yield sequence args_q;
    end;
    
    protected procedure EnqueueSelf(c: Context; cq: cl_command_queue; k: cl_kernel; ev_c: integer; prev_ev, res_ev: ^cl_event); override;
    begin
      
      for var i := 0 to args_q.Length-1 do
      begin
        if args_q[i].res.memobj=cl_mem.Zero then args_q[i].res.Init(c);
        cl.SetKernelArg(k, i, new UIntPtr(UIntPtr.Size), args_q[i].res.memobj).RaiseIfError;
      end;
      
      cl.EnqueueNDRangeKernel(cq,k, work_szs_q.res.Length, nil,work_szs_q.res,nil, ev_c,prev_ev,res_ev).RaiseIfError;
    end;
    
    protected function Clone(muhs: Dictionary<object, object>; cache: Dictionary<CommandQueueBase, CommandQueueBase>): GPUCommand<Kernel>; override :=
    new KernelCommandExec(
      CommandQueue&<array of UIntPtr>(self.work_szs_q.InternalCloneCached(muhs, cache)),
      self.args_q.ConvertAll(q->CommandQueue&<Buffer>(q.InternalCloneCached(muhs, cache)))
    );
    
  end;
  
function KernelCommandQueue.AddExec(work_szs: array of UIntPtr; params args: array of CommandQueue<Buffer>) :=
AddCommand(new KernelCommandExec(work_szs, args));

function KernelCommandQueue.AddExec(work_szs: array of CommandQueue<UIntPtr>; params args: array of CommandQueue<Buffer>) :=
AddCommand(new KernelCommandExec(
  CombineAsyncQueue(a->a,work_szs),
  args
));

function KernelCommandQueue.AddExec(work_szs: CommandQueue<array of UIntPtr>; params args: array of CommandQueue<Buffer>) :=
AddCommand(new KernelCommandExec(
  work_szs,
  args
));

function KernelCommandQueue.AddExec(work_szs: CommandQueue<array of integer>; params args: array of CommandQueue<Buffer>) :=
AddCommand(new KernelCommandExec(
  work_szs.ThenConvert(a->a.ConvertAll(sz->new UIntPtr(sz))),
  args
));

{$endregion Exec}

{$endregion Kernel}

{$endregion CommandQueue}

{$region Неявные CommandQueue}

{$region Buffer}

{$region constructor's}

procedure Buffer.Init(c: Context) :=
lock self do
begin
  var ec: ErrorCode;
  if self.memobj<>cl_mem.Zero then cl.ReleaseMemObject(self.memobj).RaiseIfError;
  self.memobj := cl.CreateBuffer(c._context, MemoryFlags.READ_WRITE, self.sz, IntPtr.Zero, ec);
  ec.RaiseIfError;
end;

function Buffer.SubBuff(offset, size: integer): Buffer;
begin
  if self.memobj=cl_mem.Zero then Init(Context.Default);
  
  Result := new Buffer(size);
  Result._parent := self;
  
  var ec: ErrorCode;
  var reg := new cl_buffer_region(
    new UIntPtr( offset ),
    new UIntPtr( size )
  );
  Result.memobj := cl.CreateSubBuffer(self.memobj, MemoryFlags.READ_WRITE, BufferCreateType.REGION, pointer(@reg), ec);
  ec.RaiseIfError;
  
end;

{$endregion constructor's}

{$region Write}

function Buffer.WriteData(ptr: CommandQueue<IntPtr>) :=
Context.Default.SyncInvoke(NewQueue.AddWriteData(ptr) as CommandQueue<Buffer>);
function Buffer.WriteData(ptr: CommandQueue<IntPtr>; offset, len: CommandQueue<integer>) :=
Context.Default.SyncInvoke(NewQueue.AddWriteData(ptr, offset, len) as CommandQueue<Buffer>);

function Buffer.WriteArray(a: CommandQueue<&Array>) :=
Context.Default.SyncInvoke(NewQueue.AddWriteArray(a) as CommandQueue<Buffer>);
function Buffer.WriteArray(a: CommandQueue<&Array>; offset, len: CommandQueue<integer>) :=
Context.Default.SyncInvoke(NewQueue.AddWriteArray(a, offset, len) as CommandQueue<Buffer>);

function Buffer.WriteValue<TRecord>(val: CommandQueue<TRecord>; offset: CommandQueue<integer>) :=
Context.Default.SyncInvoke(NewQueue.AddWriteValue(val, offset) as CommandQueue<Buffer>);

{$endregion Write}

{$region Read}

function Buffer.ReadData(ptr: CommandQueue<IntPtr>) :=
Context.Default.SyncInvoke(NewQueue.AddReadData(ptr) as CommandQueue<Buffer>);
function Buffer.ReadData(ptr: CommandQueue<IntPtr>; offset, len: CommandQueue<integer>) :=
Context.Default.SyncInvoke(NewQueue.AddReadData(ptr, offset, len) as CommandQueue<Buffer>);

function Buffer.ReadArray(a: CommandQueue<&Array>) :=
Context.Default.SyncInvoke(NewQueue.AddReadArray(a) as CommandQueue<Buffer>);
function Buffer.ReadArray(a: CommandQueue<&Array>; offset, len: CommandQueue<integer>) :=
Context.Default.SyncInvoke(NewQueue.AddReadArray(a, offset, len) as CommandQueue<Buffer>);

{$endregion Read}

{$region PatternFill}

function Buffer.FillData(ptr: CommandQueue<IntPtr>; pattern_len: CommandQueue<integer>) :=
Context.Default.SyncInvoke(NewQueue.AddFillData(ptr, pattern_len) as CommandQueue<Buffer>);
function Buffer.FillData(ptr: CommandQueue<IntPtr>; pattern_len, offset, len: CommandQueue<integer>) :=
Context.Default.SyncInvoke(NewQueue.AddFillData(ptr, pattern_len, offset, len) as CommandQueue<Buffer>);

function Buffer.FillArray(a: CommandQueue<&Array>) :=
Context.Default.SyncInvoke(NewQueue.AddFillArray(a) as CommandQueue<Buffer>);
function Buffer.FillArray(a: CommandQueue<&Array>; offset, len: CommandQueue<integer>) :=
Context.Default.SyncInvoke(NewQueue.AddFillArray(a, offset, len) as CommandQueue<Buffer>);

function Buffer.FillValue<TRecord>(val: TRecord) :=
Context.Default.SyncInvoke(NewQueue.AddFillValue(val) as CommandQueue<Buffer>);
function Buffer.FillValue<TRecord>(val: TRecord; offset, len: CommandQueue<integer>) :=
Context.Default.SyncInvoke(NewQueue.AddFillValue(val, offset, len) as CommandQueue<Buffer>);
function Buffer.FillValue<TRecord>(val: CommandQueue<TRecord>) :=
Context.Default.SyncInvoke(NewQueue.AddFillValue(val) as CommandQueue<Buffer>);
function Buffer.FillValue<TRecord>(val: CommandQueue<TRecord>; offset, len: CommandQueue<integer>) :=
Context.Default.SyncInvoke(NewQueue.AddFillValue(val, offset, len) as CommandQueue<Buffer>);

{$endregion PatternFill}

{$region Copy}

function Buffer.CopyFrom(b: CommandQueue<Buffer>; from, &to, len: CommandQueue<integer>) :=
Context.Default.SyncInvoke(NewQueue.AddCopyFrom(b, from, &to, len) as CommandQueue<Buffer>);
function Buffer.CopyFrom(b: CommandQueue<Buffer>) :=
Context.Default.SyncInvoke(NewQueue.AddCopyFrom(b) as CommandQueue<Buffer>);

function Buffer.CopyTo(b: CommandQueue<Buffer>; from, &to, len: CommandQueue<integer>) :=
Context.Default.SyncInvoke(NewQueue.AddCopyTo(b, from, &to, len) as CommandQueue<Buffer>);
function Buffer.CopyTo(b: CommandQueue<Buffer>) :=
Context.Default.SyncInvoke(NewQueue.AddCopyTo(b) as CommandQueue<Buffer>);

{$endregion Copy}

{$region Get}

function Buffer.GetData(offset, len: CommandQueue<integer>): IntPtr;
begin
  var res: IntPtr;
  
  var Qs_len := len.Multiusable(2);
  
  var Q_res := Qs_len[0].ThenConvert(len_val->
  begin
    Result := Marshal.AllocHGlobal(len_val);
    res := Result;
  end);
  
  Context.Default.SyncInvoke(
    self.NewQueue.AddReadData(Q_res, offset,Qs_len[1]) as CommandQueue<Buffer>
  );
  
  Result := res;
end;

function Buffer.GetArrayAt<TArray>(offset: CommandQueue<integer>; szs: CommandQueue<array of integer>): TArray;
begin
  var el_t := typeof(TArray).GetElementType;
  var res: &Array;
  
  var Qs_szs := szs.Multiusable(2);
  
  var Q_res := Qs_szs[0].ThenConvert(szs_val->
  begin
    Result := System.Array.CreateInstance(
      el_t,
      szs_val
    );
    res := Result;
  end);
  var Q_res_len := Qs_szs[1].ThenConvert( szs_val -> Marshal.SizeOf(el_t)*szs_val.Aggregate((i1,i2)->i1*i2) );
  
  Context.Default.SyncInvoke(
    self.NewQueue
    .AddReadArray(Q_res, offset, Q_res_len) as CommandQueue<Buffer>
  );
  
  Result := TArray(res);
end;

function Buffer.GetArrayAt<TArray>(offset: CommandQueue<integer>; params szs: array of CommandQueue<integer>) :=
GetArrayAt&<TArray>(offset, CombineAsyncQueue(a->a, szs));

function Buffer.GetValueAt<TRecord>(offset: CommandQueue<integer>): TRecord;
begin
  Context.Default.SyncInvoke(
    self.NewQueue
    .AddReadValue(Result, offset) as CommandQueue<Buffer>
  );
end;

{$endregion Get}

{$endregion Buffer}

{$region Kernel}

{$region constructor's}

constructor Kernel.Create(prog: ProgramCode; name: string);
begin
  var ec: ErrorCode;
  
  self._kernel := cl.CreateKernel(prog._program, name, ec);
  ec.RaiseIfError;
  
end;

{$endregion constructor's}

{$region Exec}

function Kernel.Exec(work_szs: array of UIntPtr; params args: array of CommandQueue<Buffer>) :=
Context.Default.SyncInvoke(NewQueue.AddExec(work_szs, args) as CommandQueue<Kernel>);
function Kernel.Exec(work_szs: array of CommandQueue<UIntPtr>; params args: array of CommandQueue<Buffer>) :=
Context.Default.SyncInvoke(NewQueue.AddExec(work_szs, args) as CommandQueue<Kernel>);
function Kernel.Exec(work_szs: CommandQueue<array of UIntPtr>; params args: array of CommandQueue<Buffer>) :=
Context.Default.SyncInvoke(NewQueue.AddExec(work_szs, args) as CommandQueue<Kernel>);
function Kernel.Exec(work_szs: CommandQueue<array of integer>; params args: array of CommandQueue<Buffer>) :=
Context.Default.SyncInvoke(NewQueue.AddExec(work_szs, args) as CommandQueue<Kernel>);

{$endregion Exec}

{$endregion Kernel}

{$endregion Неявные CommandQueue}

{$region Сахарные подпрограммы}

function HFQ<T>(f: ()->T) :=
new CommandQueueHostFunc<T>(f);

function HPQ(p: ()->()) :=
HFQ&<object>(()->
begin
  p();
  Result := nil;
end);

function CombineSyncQueue<T>(qs: List<CommandQueueBase>) :=
new CommandQueueSyncList<T>(qs);
function CombineSyncQueue<T>(qs: List<CommandQueue<T>>) :=
new CommandQueueTSyncList<T>(qs);
function CombineSyncQueue<T>(params qs: array of CommandQueueBase) :=
new CommandQueueSyncList<T>(qs);
function CombineSyncQueue<T>(params qs: array of CommandQueue<T>) :=
new CommandQueueTSyncList<T>(qs);

function CombineSyncQueue<T, TRes>(conv: Func<array of object, TRes>; qs: List<CommandQueueBase>) :=
new CommandQueueCSyncList<TRes>(qs, conv);
function CombineSyncQueue<T, TRes>(conv: Func<array of T, TRes>; qs: List<CommandQueue<T>>) :=
new CommandQueueCTSyncList<T, TRes>(qs, conv);
function CombineSyncQueue<T, TRes>(conv: Func<array of object, TRes>; params qs: array of CommandQueueBase) :=
new CommandQueueCSyncList<TRes>(qs, conv);
function CombineSyncQueue<T, TRes>(conv: Func<array of T, TRes>; params qs: array of CommandQueue<T>) :=
new CommandQueueCTSyncList<T, TRes>(qs, conv);

function CombineAsyncQueue<T>(qs: List<CommandQueueBase>): CommandQueue<T> :=
new CommandQueueSyncList<T>(qs);
function CombineAsyncQueue<T>(qs: List<CommandQueue<T>>): CommandQueue<T> :=
new CommandQueueTSyncList<T>(qs);
function CombineAsyncQueue<T>(params qs: array of CommandQueueBase): CommandQueue<T> :=
new CommandQueueSyncList<T>(qs);
function CombineAsyncQueue<T>(params qs: array of CommandQueue<T>) :=
new CommandQueueTAsyncList<T>(qs);

function CombineAsyncQueue<T, TRes>(conv: Func<array of object, TRes>; qs: List<CommandQueueBase>) :=
new CommandQueueCAsyncList<TRes>(qs, conv);
function CombineAsyncQueue<T, TRes>(conv: Func<array of T, TRes>; qs: List<CommandQueue<T>>) :=
new CommandQueueCTAsyncList<T, TRes>(qs, conv);
function CombineAsyncQueue<T, TRes>(conv: Func<array of object, TRes>; params qs: array of CommandQueueBase) :=
new CommandQueueCAsyncList<TRes>(qs, conv);
function CombineAsyncQueue<T, TRes>(conv: Func<array of T, TRes>; params qs: array of CommandQueue<T>) :=
new CommandQueueCTAsyncList<T, TRes>(qs, conv);

{$endregion Сахарные подпрограммы}

end.