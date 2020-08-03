
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
///Справка данного модуля находится в папке примеров
///   По-умолчанию, её можно найти в "C:\PABCWork.NET\Samples\OpenGL и OpenCL"
///
unit OpenCLABC;

interface

uses System;

uses OpenCL;
uses OpenCLABCBase in 'Internal\OpenCLABCBase';

{$region Re:definition's}

type
  
  ///Тип устройства, поддерживающего OpenCL
  DeviceType              = OpenCL.DeviceType;
  ///Уровень кэша, используемый в Device.SplitByAffinityDomain
  DeviceAffinityDomain    = OpenCL.DeviceAffinityDomain;
  
  ///Представляет платформу OpenCL, объединяющую одно или несколько устройств
  Platform                = OpenCLABCBase.Platform;
  ///Представляет устройство, поддерживающее OpenCL
  Device                  = OpenCLABCBase.Device;
  ///Представляет виртуальное устройство, использующее часть ядер другого устройства
  ///Объекты данного типа обычно создаются методами "Device.Split*"
  SubDevice               = OpenCLABCBase.SubDevice;
  ///Представляет контекст для хранения данных и выполнения команд на GPU
  Context                 = OpenCLABCBase.Context;
  
  ///Представляет область памяти устройства OpenCL
  Buffer                  = OpenCLABCBase.Buffer;
  ///Представляет область памяти внутри другого буфера
  SubBuffer               = OpenCLABCBase.SubBuffer;
  ///Представляет подпрограмму, выполняемую на GPU
  Kernel                  = OpenCLABCBase.Kernel;
  ///Представляет контейнер с откомпилированным кодом для GPU, содержащим подпрограммы-kernel'ы
  ProgramCode             = OpenCLABCBase.ProgramCode;
  
  ///Представляет задачу выполнения очереди, создаваемую методом Context.BeginInvoke
  CLTaskBase              = OpenCLABCBase.CLTaskBase;
  ///Представляет задачу выполнения очереди, создаваемую методом Context.BeginInvoke
  CLTask<T>               = OpenCLABCBase.CLTask<T>;
  
  ///Представляет очередь, состоящую в основном из команд, выполняемых на GPU
  CommandQueueBase        = OpenCLABCBase.CommandQueueBase;
  ///Представляет очередь, состоящую в основном из команд, выполняемых на GPU
  CommandQueue<T>         = OpenCLABCBase.CommandQueue<T>;
  
  ///Интерфейс, который реализован только классом ConstQueue<T>
  ///Позволяет получить значение, из которого была создана константая очередь, не зная его типа
  IConstQueue             = OpenCLABCBase.IConstQueue;
  ///Представляет константную очередь
  ///Константные очереди ничего не выполняют и возвращает заданное при создании значение
  ConstQueue<T>           = OpenCLABCBase.ConstQueue<T>;
  
  ///Представляет очередь-контейнер для команд GPU, применяемых к объекту типа Buffer
  BufferCommandQueue      = OpenCLABCBase.BufferCommandQueue;
  ///Представляет очередь-контейнер для команд GPU, применяемых к объекту типа Kernel
  KernelCommandQueue      = OpenCLABCBase.KernelCommandQueue;
  
  ///Представляет аргумент, передаваемый в вызов kernel-а
  KernelArg               = OpenCLABCBase.KernelArg;
  
{$endregion Re:definition's}

{$region HFQ/HPQ}

///Создаёт очередь, выполняющую указанную функцию на CPU
///И возвращающую результат этой функци
function HFQ<T>(f: ()->T): CommandQueue<T>;
///Создаёт очередь, выполняющую указанную функцию на CPU
///И возвращающую результат этой функци
function HFQ<T>(f: Context->T): CommandQueue<T>;

///Создаёт очередь, выполняющую указанную процедуру на CPU
///И возвращающую object(nil)
function HPQ(p: ()->()): CommandQueueBase;
///Создаёт очередь, выполняющую указанную процедуру на CPU
///И возвращающую object(nil)
function HPQ(p: Context->()): CommandQueueBase;

{$endregion HFQ/HPQ}

{$region WaitFor}

///Создаёт очередь, ожидающую сигнала выполненности от каждой из указанных очередей
function WaitForAll(params qs: array of CommandQueueBase): CommandQueueBase;
///Создаёт очередь, ожидающую сигнала выполненности от каждой из указанных очередей
function WaitForAll(qs: sequence of CommandQueueBase): CommandQueueBase;

///Создаёт очередь, ожидающую первого сигнала выполненности от любой из указанных очередей
function WaitForAny(params qs: array of CommandQueueBase): CommandQueueBase;
///Создаёт очередь, ожидающую первого сигнала выполненности от любой из указанных очередей
function WaitForAny(qs: sequence of CommandQueueBase): CommandQueueBase;

///Создаёт очередь, ожидающую сигнала выполненности от указанной очереди
function WaitFor(q: CommandQueueBase): CommandQueueBase;

{$endregion WaitFor}

{$region CombineQueue's}

{$region Sync}

{$region NonConv}

///Создаёт очередь, выполняющую указанные очереди одну за другой
///И возвращающую результат последней очереди
function CombineSyncQueueBase(params qs: array of CommandQueueBase): CommandQueueBase;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///И возвращающую результат последней очереди
function CombineSyncQueueBase(qs: sequence of CommandQueueBase): CommandQueueBase;

///Создаёт очередь, выполняющую указанные очереди одну за другой
///И возвращающую результат последней очереди
function CombineSyncQueue<T>(qs: sequence of CommandQueueBase; last: CommandQueue<T>): CommandQueue<T>;

///Создаёт очередь, выполняющую указанные очереди одну за другой
///И возвращающую результат последней очереди
function CombineSyncQueue<T>(params qs: array of CommandQueue<T>): CommandQueue<T>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///И возвращающую результат последней очереди
function CombineSyncQueue<T>(qs: sequence of CommandQueue<T>): CommandQueue<T>;

{$endregion NonConv}

{$region Conv}

{$region NonContext}

///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineSyncQueue<TRes>(conv: Func<array of object, TRes>; params qs: array of CommandQueueBase): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineSyncQueue<TRes>(conv: Func<array of object, TRes>; qs: sequence of CommandQueueBase): CommandQueue<TRes>;

///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineSyncQueue<TInp, TRes>(conv: Func<array of TInp, TRes>; params qs: array of CommandQueue<TInp>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineSyncQueue<TInp, TRes>(conv: Func<array of TInp, TRes>; qs: sequence of CommandQueue<TInp>): CommandQueue<TRes>;

///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineSyncQueue2<TInp1, TInp2, TRes>(conv: Func<TInp1, TInp2, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineSyncQueue3<TInp1, TInp2, TInp3, TRes>(conv: Func<TInp1, TInp2, TInp3, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineSyncQueue4<TInp1, TInp2, TInp3, TInp4, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineSyncQueue5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineSyncQueue6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineSyncQueue7<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>; q7: CommandQueue<TInp7>): CommandQueue<TRes>;

{$endregion NonContext}

{$region Context}

///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineSyncQueue<TRes>(conv: Func<array of object, Context, TRes>; params qs: array of CommandQueueBase): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineSyncQueue<TRes>(conv: Func<array of object, Context, TRes>; qs: sequence of CommandQueueBase): CommandQueue<TRes>;

///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineSyncQueue<TInp, TRes>(conv: Func<array of TInp, Context, TRes>; params qs: array of CommandQueue<TInp>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineSyncQueue<TInp, TRes>(conv: Func<array of TInp, Context, TRes>; qs: sequence of CommandQueue<TInp>): CommandQueue<TRes>;

///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineSyncQueue2<TInp1, TInp2, TRes>(conv: Func<TInp1, TInp2, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineSyncQueue3<TInp1, TInp2, TInp3, TRes>(conv: Func<TInp1, TInp2, TInp3, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineSyncQueue4<TInp1, TInp2, TInp3, TInp4, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineSyncQueue5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineSyncQueue6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одну за другой
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineSyncQueue7<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>; q7: CommandQueue<TInp7>): CommandQueue<TRes>;

{$endregion Context}

{$endregion Conv}

{$endregion Sync}

{$region Async}

{$region NonConv}

///Создаёт очередь, выполняющую указанные очереди одновременно
///И возвращающую результат последней очереди
function CombineAsyncQueueBase(params qs: array of CommandQueueBase): CommandQueueBase;
///Создаёт очередь, выполняющую указанные очереди одновременно
///И возвращающую результат последней очереди
function CombineAsyncQueueBase(qs: sequence of CommandQueueBase): CommandQueueBase;

///Создаёт очередь, выполняющую указанные очереди одновременно
///И возвращающую результат последней очереди
function CombineAsyncQueue<T>(qs: sequence of CommandQueueBase; last: CommandQueue<T>): CommandQueue<T>;

///Создаёт очередь, выполняющую указанные очереди одновременно
///И возвращающую результат последней очереди
function CombineAsyncQueue<T>(params qs: array of CommandQueue<T>): CommandQueue<T>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///И возвращающую результат последней очереди
function CombineAsyncQueue<T>(qs: sequence of CommandQueue<T>): CommandQueue<T>;

{$endregion NonConv}

{$region Conv}

{$region NonContext}

///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineAsyncQueue<TRes>(conv: Func<array of object, TRes>; params qs: array of CommandQueueBase): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineAsyncQueue<TRes>(conv: Func<array of object, TRes>; qs: sequence of CommandQueueBase): CommandQueue<TRes>;

///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineAsyncQueue<TInp, TRes>(conv: Func<array of TInp, TRes>; params qs: array of CommandQueue<TInp>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineAsyncQueue<TInp, TRes>(conv: Func<array of TInp, TRes>; qs: sequence of CommandQueue<TInp>): CommandQueue<TRes>;

///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineAsyncQueue2<TInp1, TInp2, TRes>(conv: Func<TInp1, TInp2, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineAsyncQueue3<TInp1, TInp2, TInp3, TRes>(conv: Func<TInp1, TInp2, TInp3, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineAsyncQueue4<TInp1, TInp2, TInp3, TInp4, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineAsyncQueue5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineAsyncQueue6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineAsyncQueue7<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>; q7: CommandQueue<TInp7>): CommandQueue<TRes>;

{$endregion NonContext}

{$region Context}

///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineAsyncQueue<TRes>(conv: Func<array of object, Context, TRes>; params qs: array of CommandQueueBase): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineAsyncQueue<TRes>(conv: Func<array of object, Context, TRes>; qs: sequence of CommandQueueBase): CommandQueue<TRes>;

///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineAsyncQueue<TInp, TRes>(conv: Func<array of TInp, Context, TRes>; params qs: array of CommandQueue<TInp>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineAsyncQueue<TInp, TRes>(conv: Func<array of TInp, Context, TRes>; qs: sequence of CommandQueue<TInp>): CommandQueue<TRes>;

///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineAsyncQueue2<TInp1, TInp2, TRes>(conv: Func<TInp1, TInp2, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineAsyncQueue3<TInp1, TInp2, TInp3, TRes>(conv: Func<TInp1, TInp2, TInp3, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineAsyncQueue4<TInp1, TInp2, TInp3, TInp4, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineAsyncQueue5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineAsyncQueue6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>): CommandQueue<TRes>;
///Создаёт очередь, выполняющую указанные очереди одновременно
///Затем выполняющую указанную первым параметров функцию на CPU, передавая результаты всех очередей
///И возвращающую результат этой функции
function CombineAsyncQueue7<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>; q7: CommandQueue<TInp7>): CommandQueue<TRes>;

{$endregion Context}

{$endregion Conv}

{$endregion Async}

{$endregion CombineQueue's}

implementation

{$region HFQ/HPQ}

type
  CommandQueueHostQueueBase<T,TFunc> = abstract class(HostQueue<object,T>)
  where TFunc: Delegate;
    
    private f: TFunc;
    public constructor(f: TFunc) := self.f := f;
    private constructor := raise new InvalidOperationException($'Был вызван не_применимый конструктор без параметров... Обратитесь к разработчику OpenCLABC');
    
    protected function InvokeSubQs(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; var cq: cl_command_queue; prev_ev: EventList): QueueRes<object>; override :=
    new QueueResConst<Object>(nil, prev_ev ?? new EventList);
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override := exit;
    
  end;
  
  CommandQueueHostFunc<T> = sealed class(CommandQueueHostQueueBase<T, Context->T>)
    
    protected function ExecFunc(o: object; c: Context): T; override := f(c);
    
  end;
  CommandQueueHostProc = sealed class(CommandQueueHostQueueBase<object, Context->()>)
    
    protected function ExecFunc(o: object; c: Context): object; override;
    begin
      f(c);
      Result := nil;
    end;
    
  end;
  
function HFQ<T>(f: ()->T) :=
new CommandQueueHostFunc<T>(c->f());
function HFQ<T>(f: Context->T) :=
new CommandQueueHostFunc<T>(f);

function HPQ(p: ()->()) :=
new CommandQueueHostProc(c->p());
function HPQ(p: Context->()) :=
new CommandQueueHostProc(p);

{$endregion HFQ/HPQ}

{$region WaitFor}

type
  CommandQueueWaitFor = sealed class(CommandQueue<object>)
    public waiter: WCQWaiter;
    
    public constructor(waiter: WCQWaiter) :=
    self.waiter := waiter;
    
    protected function InvokeImpl(tsk: CLTaskBase; c: Context; main_dvc: cl_device_id; need_ptr_qr: boolean; var cq: cl_command_queue; prev_ev: EventList): QueueRes<object>; override;
    begin
      if need_ptr_qr then new System.InvalidOperationException;
      var wait_ev := waiter.GetWaitEv(tsk, c);
      Result := new QueueResConst<object>(nil, prev_ev=nil ? wait_ev : prev_ev+wait_ev);
    end;
    
    protected procedure RegisterWaitables(tsk: CLTaskBase; prev_hubs: HashSet<MultiusableCommandQueueHubBase>); override :=
    waiter.RegisterWaitables(tsk);
    
  end;
  
function WaitForAll(params qs: array of CommandQueueBase) := WaitForAll(qs.AsEnumerable);
function WaitForAll(qs: sequence of CommandQueueBase) := new CommandQueueWaitFor(new WCQWaiterAll(qs.ToArray));

function WaitForAny(params qs: array of CommandQueueBase) := WaitForAny(qs.AsEnumerable);
function WaitForAny(qs: sequence of CommandQueueBase) := new CommandQueueWaitFor(new WCQWaiterAny(qs.ToArray));

function WaitFor(q: CommandQueueBase) := WaitForAll(q);

{$endregion WaitFor}

{$region CombineQueue's}

{$region Sync}

{$region NonConv}

function CombineSyncQueueBase(params qs: array of CommandQueueBase) := new SimpleSyncQueueArray<object>(QueueArrayUtils.FlattenSyncQueueArray(qs));
function CombineSyncQueueBase(qs: sequence of CommandQueueBase) := new SimpleSyncQueueArray<object>(QueueArrayUtils.FlattenSyncQueueArray(qs));

function CombineSyncQueue<T>(qs: sequence of CommandQueueBase; last: CommandQueue<T>) := new SimpleSyncQueueArray<T>(QueueArrayUtils.FlattenSyncQueueArray(qs.Append(last as CommandQueueBase)));

function CombineSyncQueue<T>(params qs: array of CommandQueue<T>) := new SimpleSyncQueueArray<T>(QueueArrayUtils.FlattenSyncQueueArray(qs.Cast&<CommandQueueBase>));
function CombineSyncQueue<T>(qs: sequence of CommandQueue<T>) := new SimpleSyncQueueArray<T>(QueueArrayUtils.FlattenSyncQueueArray(qs.Cast&<CommandQueueBase>));

{$endregion NonConv}

{$region Conv}

{$region NonContext}

function CombineSyncQueue<TRes>(conv: Func<array of object, TRes>; params qs: array of CommandQueueBase) := new ConvSyncQueueArray<object, TRes>(qs.Select(q->q.Cast&<object>).ToArray, (a,c)->conv(a));
function CombineSyncQueue<TRes>(conv: Func<array of object, TRes>; qs: sequence of CommandQueueBase) := new ConvSyncQueueArray<object, TRes>(qs.Select(q->q.Cast&<object>).ToArray, (a,c)->conv(a));

function CombineSyncQueue<TInp, TRes>(conv: Func<array of TInp, TRes>; params qs: array of CommandQueue<TInp>) := new ConvSyncQueueArray<TInp, TRes>(qs.ToArray, (a,c)->conv(a));
function CombineSyncQueue<TInp, TRes>(conv: Func<array of TInp, TRes>; qs: sequence of CommandQueue<TInp>) := new ConvSyncQueueArray<TInp, TRes>(qs.ToArray, (a,c)->conv(a));

function CombineSyncQueue2<TInp1, TInp2, TRes>(conv: Func<TInp1, TInp2, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>) := new ConvSyncQueueArray2<TInp1, TInp2, TRes>(q1, q2, (o1, o2, c)->conv(o1, o2));
function CombineSyncQueue3<TInp1, TInp2, TInp3, TRes>(conv: Func<TInp1, TInp2, TInp3, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>) := new ConvSyncQueueArray3<TInp1, TInp2, TInp3, TRes>(q1, q2, q3, (o1, o2, o3, c)->conv(o1, o2, o3));
function CombineSyncQueue4<TInp1, TInp2, TInp3, TInp4, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>) := new ConvSyncQueueArray4<TInp1, TInp2, TInp3, TInp4, TRes>(q1, q2, q3, q4, (o1, o2, o3, o4, c)->conv(o1, o2, o3, o4));
function CombineSyncQueue5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>) := new ConvSyncQueueArray5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>(q1, q2, q3, q4, q5, (o1, o2, o3, o4, o5, c)->conv(o1, o2, o3, o4, o5));
function CombineSyncQueue6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>) := new ConvSyncQueueArray6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>(q1, q2, q3, q4, q5, q6, (o1, o2, o3, o4, o5, o6, c)->conv(o1, o2, o3, o4, o5, o6));
function CombineSyncQueue7<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>; q7: CommandQueue<TInp7>) := new ConvSyncQueueArray7<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>(q1, q2, q3, q4, q5, q6, q7, (o1, o2, o3, o4, o5, o6, o7, c)->conv(o1, o2, o3, o4, o5, o6, o7));

{$endregion NonContext}

{$region Context}

function CombineSyncQueue<TRes>(conv: Func<array of object, Context, TRes>; params qs: array of CommandQueueBase) := new ConvSyncQueueArray<object, TRes>(qs.Select(q->q.Cast&<object>).ToArray, conv);
function CombineSyncQueue<TRes>(conv: Func<array of object, Context, TRes>; qs: sequence of CommandQueueBase) := new ConvSyncQueueArray<object, TRes>(qs.Select(q->q.Cast&<object>).ToArray, conv);

function CombineSyncQueue<TInp, TRes>(conv: Func<array of TInp, Context, TRes>; params qs: array of CommandQueue<TInp>) := new ConvSyncQueueArray<TInp, TRes>(qs.ToArray, conv);
function CombineSyncQueue<TInp, TRes>(conv: Func<array of TInp, Context, TRes>; qs: sequence of CommandQueue<TInp>) := new ConvSyncQueueArray<TInp, TRes>(qs.ToArray, conv);

function CombineSyncQueue2<TInp1, TInp2, TRes>(conv: Func<TInp1, TInp2, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>) := new ConvSyncQueueArray2<TInp1, TInp2, TRes>(q1, q2, conv);
function CombineSyncQueue3<TInp1, TInp2, TInp3, TRes>(conv: Func<TInp1, TInp2, TInp3, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>) := new ConvSyncQueueArray3<TInp1, TInp2, TInp3, TRes>(q1, q2, q3, conv);
function CombineSyncQueue4<TInp1, TInp2, TInp3, TInp4, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>) := new ConvSyncQueueArray4<TInp1, TInp2, TInp3, TInp4, TRes>(q1, q2, q3, q4, conv);
function CombineSyncQueue5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>) := new ConvSyncQueueArray5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>(q1, q2, q3, q4, q5, conv);
function CombineSyncQueue6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>) := new ConvSyncQueueArray6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>(q1, q2, q3, q4, q5, q6, conv);
function CombineSyncQueue7<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>; q7: CommandQueue<TInp7>) := new ConvSyncQueueArray7<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>(q1, q2, q3, q4, q5, q6, q7, conv);

{$endregion Context}

{$endregion Conv}

{$endregion Sync}

{$region Async}

{$region NonConv}

function CombineAsyncQueueBase(params qs: array of CommandQueueBase) := new SimpleAsyncQueueArray<object>(QueueArrayUtils.FlattenAsyncQueueArray(qs));
function CombineAsyncQueueBase(qs: sequence of CommandQueueBase) := new SimpleAsyncQueueArray<object>(QueueArrayUtils.FlattenAsyncQueueArray(qs));

function CombineAsyncQueue<T>(qs: sequence of CommandQueueBase; last: CommandQueue<T>) := new SimpleAsyncQueueArray<T>(QueueArrayUtils.FlattenAsyncQueueArray(qs.Append(last as CommandQueueBase)));

function CombineAsyncQueue<T>(params qs: array of CommandQueue<T>) := new SimpleAsyncQueueArray<T>(QueueArrayUtils.FlattenAsyncQueueArray(qs.Cast&<CommandQueueBase>));
function CombineAsyncQueue<T>(qs: sequence of CommandQueue<T>) := new SimpleAsyncQueueArray<T>(QueueArrayUtils.FlattenAsyncQueueArray(qs.Cast&<CommandQueueBase>));

{$endregion NonConv}

{$region Conv}

{$region NonContext}

function CombineAsyncQueue<TRes>(conv: Func<array of object, TRes>; params qs: array of CommandQueueBase) := new ConvAsyncQueueArray<object, TRes>(qs.Select(q->q.Cast&<object>).ToArray, (a,c)->conv(a));
function CombineAsyncQueue<TRes>(conv: Func<array of object, TRes>; qs: sequence of CommandQueueBase) := new ConvAsyncQueueArray<object, TRes>(qs.Select(q->q.Cast&<object>).ToArray, (a,c)->conv(a));

function CombineAsyncQueue<TInp, TRes>(conv: Func<array of TInp, TRes>; params qs: array of CommandQueue<TInp>) := new ConvAsyncQueueArray<TInp, TRes>(qs.ToArray, (a,c)->conv(a));
function CombineAsyncQueue<TInp, TRes>(conv: Func<array of TInp, TRes>; qs: sequence of CommandQueue<TInp>) := new ConvAsyncQueueArray<TInp, TRes>(qs.ToArray, (a,c)->conv(a));

function CombineAsyncQueue2<TInp1, TInp2, TRes>(conv: Func<TInp1, TInp2, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>) := new ConvAsyncQueueArray2<TInp1, TInp2, TRes>(q1, q2, (o1, o2, c)->conv(o1, o2));
function CombineAsyncQueue3<TInp1, TInp2, TInp3, TRes>(conv: Func<TInp1, TInp2, TInp3, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>) := new ConvAsyncQueueArray3<TInp1, TInp2, TInp3, TRes>(q1, q2, q3, (o1, o2, o3, c)->conv(o1, o2, o3));
function CombineAsyncQueue4<TInp1, TInp2, TInp3, TInp4, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>) := new ConvAsyncQueueArray4<TInp1, TInp2, TInp3, TInp4, TRes>(q1, q2, q3, q4, (o1, o2, o3, o4, c)->conv(o1, o2, o3, o4));
function CombineAsyncQueue5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>) := new ConvAsyncQueueArray5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>(q1, q2, q3, q4, q5, (o1, o2, o3, o4, o5, c)->conv(o1, o2, o3, o4, o5));
function CombineAsyncQueue6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>) := new ConvAsyncQueueArray6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>(q1, q2, q3, q4, q5, q6, (o1, o2, o3, o4, o5, o6, c)->conv(o1, o2, o3, o4, o5, o6));
function CombineAsyncQueue7<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>; q7: CommandQueue<TInp7>) := new ConvAsyncQueueArray7<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>(q1, q2, q3, q4, q5, q6, q7, (o1, o2, o3, o4, o5, o6, o7, c)->conv(o1, o2, o3, o4, o5, o6, o7));

{$endregion NonContext}

{$region Context}

function CombineAsyncQueue<TRes>(conv: Func<array of object, Context, TRes>; params qs: array of CommandQueueBase) := new ConvAsyncQueueArray<object, TRes>(qs.Select(q->q.Cast&<object>).ToArray, conv);
function CombineAsyncQueue<TRes>(conv: Func<array of object, Context, TRes>; qs: sequence of CommandQueueBase) := new ConvAsyncQueueArray<object, TRes>(qs.Select(q->q.Cast&<object>).ToArray, conv);

function CombineAsyncQueue<TInp, TRes>(conv: Func<array of TInp, Context, TRes>; params qs: array of CommandQueue<TInp>) := new ConvAsyncQueueArray<TInp, TRes>(qs.ToArray, conv);
function CombineAsyncQueue<TInp, TRes>(conv: Func<array of TInp, Context, TRes>; qs: sequence of CommandQueue<TInp>) := new ConvAsyncQueueArray<TInp, TRes>(qs.ToArray, conv);

function CombineAsyncQueue2<TInp1, TInp2, TRes>(conv: Func<TInp1, TInp2, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>) := new ConvAsyncQueueArray2<TInp1, TInp2, TRes>(q1, q2, conv);
function CombineAsyncQueue3<TInp1, TInp2, TInp3, TRes>(conv: Func<TInp1, TInp2, TInp3, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>) := new ConvAsyncQueueArray3<TInp1, TInp2, TInp3, TRes>(q1, q2, q3, conv);
function CombineAsyncQueue4<TInp1, TInp2, TInp3, TInp4, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>) := new ConvAsyncQueueArray4<TInp1, TInp2, TInp3, TInp4, TRes>(q1, q2, q3, q4, conv);
function CombineAsyncQueue5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>) := new ConvAsyncQueueArray5<TInp1, TInp2, TInp3, TInp4, TInp5, TRes>(q1, q2, q3, q4, q5, conv);
function CombineAsyncQueue6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>) := new ConvAsyncQueueArray6<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TRes>(q1, q2, q3, q4, q5, q6, conv);
function CombineAsyncQueue7<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>(conv: Func<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, Context, TRes>; q1: CommandQueue<TInp1>; q2: CommandQueue<TInp2>; q3: CommandQueue<TInp3>; q4: CommandQueue<TInp4>; q5: CommandQueue<TInp5>; q6: CommandQueue<TInp6>; q7: CommandQueue<TInp7>) := new ConvAsyncQueueArray7<TInp1, TInp2, TInp3, TInp4, TInp5, TInp6, TInp7, TRes>(q1, q2, q3, q4, q5, q6, q7, conv);

{$endregion Context}

{$endregion Conv}

{$endregion Async}

{$endregion CombineQueue's}

end.