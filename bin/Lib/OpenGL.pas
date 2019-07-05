
//*****************************************************************************************************\\
// Copyright (©) Cergey Latchenko ( github.com/SunSerega | forum.mmcs.sfedu.ru/u/sun_serega )
// This code is distributed under the Unlicense
// For details see LICENSE file or this:
// https://github.com/SunSerega/POCGL/blob/master/LICENSE
//*****************************************************************************************************\\
// Copyright (©) Сергей Латченко ( github.com/SunSerega | forum.mmcs.sfedu.ru/u/sun_serega )
// Этот код распространяется под Unlicense
// Для деталей смотрите в файл LICENSE или это:
// https://github.com/SunSerega/POCGL/blob/master/LICENSE
//*****************************************************************************************************\\

///
/// Код переведён отсюда:
/// https://github.com/KhronosGroup/OpenGL-Registry
///
/// Спецификация (что то типа справки):
/// https://www.khronos.org/registry/OpenGL/specs/gl/glspec46.core.pdf
///
/// Если чего то не хватает - писать сюда:
/// https://github.com/SunSerega/POCGL/issues
///
unit OpenGL;

//ToDo матрицы в шейдерах передаются массивом столбцов
// - это может бить по производительности, потому что тут матрицы хранятся строками
// - разобраться - может там снова эта путаница и на самом деле в шейдере можно выбирать как хранится матрица
// - возможно придётся вывернуть все матрицы наизнанку... или добавить ещё типов матриц

//ToDo в самом конце - сделать прогу чтоб посмотреть какие константы по 2 раза, а каких вообще нет

//ToDo Возможно *Name -ам сделать отдельные типы?

//ToDo ^T -> pointer
// - и сразу проверить где можно nil передать
// - и в OpenCL тоже...

//ToDo проверить получение указателя на строчку матрицы

//ToDo проверить передачу external функции вместо лямбды

//ToDo issue компилятора:
// - #2029

uses System;
uses System.Runtime.InteropServices;

{$region Основные типы}

type
  
  GLsync                        = IntPtr;
  GLeglImageOES                 = IntPtr;
  
  QueryName                     = UInt32;
  BufferName                    = UInt32;
  ShaderName                    = UInt32;
  ProgramName                   = UInt32;
  ProgramPipelineName           = UInt32;
  TextureName                   = UInt32;
  SamplerName                   = UInt32;
  FramebufferName               = UInt32;
  RenderbufferName              = UInt32;
  VertexArrayName               = UInt32;
  TransformFeedbackName         = UInt32;
  
  ShaderBinaryFormat            = UInt32;
  ProgramResourceIndex          = UInt32;
  ProgramBinaryFormat           = UInt32;
  
  HGLRC                         = UInt32; //ToDo вроде это что то для связки с GDI... если в конце окажется не нужно - удалить
  
  // типы для совместимости с OpenCL
  ///--
  cl_context                    = IntPtr;
  ///--
  cl_event                      = IntPtr;
  
type
  OpenGLException = class(Exception)
    
    constructor(text: string) :=
    inherited Create($'Ошибка OpenGL: "{text}"');
    
  end;
  
{$endregion Основные типы}

{$region Энумы} type
  
  {$region case Result of}
  
  //R
  ErrorCode = record
    public val: UInt32;
    
    public const NO_ERROR =                                $0000;
    public const FRAMEBUFFER_COMPLETE =                    $8CD5;
    
    public const INVALID_ENUM =                            $0500;
    public const INVALID_VALUE =                           $0501;
    public const INVALID_OPERATION =                       $0502;
    public const STACK_OVERFLOW =                          $0503;
    public const STACK_UNDERFLOW =                         $0504;
    public const OUT_OF_MEMORY =                           $0505;
    public const INVALID_FRAMEBUFFER_OPERATION =           $0506;
    
    public function ToString: string; override;
    begin
      var res := typeof(ErrorCode).GetFields.Where(fi->fi.IsLiteral).FirstOrDefault(prop->integer(prop.GetValue(nil)) = self.val);
      Result := res=nil?
        $'ErrorCode[${self.val:X}]':
        res.Name.ToWords('_').Select(w->w[1].ToUpper+w.Substring(1).ToLower).JoinIntoString;
    end;
    
    public procedure RaiseIfError :=
    case val of
      NO_ERROR, FRAMEBUFFER_COMPLETE: ;
      else raise new OpenGLException(self.ToString);
    end;
    
  end;
  
  {$endregion case Result of}
  
  {$region 1 значение}
  
  {$region ...Mode}
  
  //S
  BeginMode = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property POINTS:          BeginMode read new BeginMode($0000);
    public static property LINES:           BeginMode read new BeginMode($0001);
    public static property LINE_LOOP:       BeginMode read new BeginMode($0002);
    public static property LINE_STRIP:      BeginMode read new BeginMode($0003);
    public static property TRIANGLES:       BeginMode read new BeginMode($0004);
    public static property TRIANGLE_STRIP:  BeginMode read new BeginMode($0005);
    public static property TRIANGLE_FAN:    BeginMode read new BeginMode($0006);
    
  end;
  
  //S
  ReservedTimeoutMode = record
    public val: uint64;
    public constructor(val: uint64) := self.val := val;
    
    public static property GL_TIMEOUT_IGNORED:  ReservedTimeoutMode read new ReservedTimeoutMode(uint64.MaxValue);
    
  end;
  
  {$endregion ...Mode}
  
  {$region ...InfoType}
  
  //S
  RenderbufferInfoType = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property WIDTH:           RenderbufferInfoType read new RenderbufferInfoType($8D42);
    public static property HEIGHT:          RenderbufferInfoType read new RenderbufferInfoType($8D43);
    public static property INTERNAL_FORMAT: RenderbufferInfoType read new RenderbufferInfoType($8D44);
    public static property SAMPLES:         RenderbufferInfoType read new RenderbufferInfoType($8CAB);
    public static property RED_SIZE:        RenderbufferInfoType read new RenderbufferInfoType($8D50);
    public static property GREEN_SIZE:      RenderbufferInfoType read new RenderbufferInfoType($8D51);
    public static property BLUE_SIZE:       RenderbufferInfoType read new RenderbufferInfoType($8D52);
    public static property ALPHA_SIZE:      RenderbufferInfoType read new RenderbufferInfoType($8D53);
    public static property DEPTH_SIZE:      RenderbufferInfoType read new RenderbufferInfoType($8D54);
    public static property STENCIL_SIZE:    RenderbufferInfoType read new RenderbufferInfoType($8D55);
    
  end;
  
  //S
  ActiveSubroutineInfoType = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property UNIFORMS:              ActiveSubroutineInfoType read new ActiveSubroutineInfoType($8DE6);
    public static property UNIFORM_LOCATIONS:     ActiveSubroutineInfoType read new ActiveSubroutineInfoType($8E47);
    public static property GL_ACTIVE_SUBROUTINES: ActiveSubroutineInfoType read new ActiveSubroutineInfoType($8DE5);
    public static property UNIFORM_MAX_LENGTH:    ActiveSubroutineInfoType read new ActiveSubroutineInfoType($8E49);
    public static property MAX_LENGTH:            ActiveSubroutineInfoType read new ActiveSubroutineInfoType($8E48);
    
  end;
  
  //S
  ProgramPipelineInfoType = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property ACTIVE_PROGRAM:  ProgramPipelineInfoType read new ProgramPipelineInfoType($8259);
    public static property VALIDATE_STATUS: ProgramPipelineInfoType read new ProgramPipelineInfoType($8B83);
    public static property INFO_LOG_LENGTH: ProgramPipelineInfoType read new ProgramPipelineInfoType($8B84);
    
  end;
  
  //S
  ProgramInfoType = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property DELETE_STATUS:                         ProgramInfoType read new ProgramInfoType($8B80);
    public static property LINK_STATUS:                           ProgramInfoType read new ProgramInfoType($8B82);
    public static property VALIDATE_STATUS:                       ProgramInfoType read new ProgramInfoType($8B83);
    public static property INFO_LOG_LENGTH:                       ProgramInfoType read new ProgramInfoType($8B84);
    public static property ATTACHED_SHADERS:                      ProgramInfoType read new ProgramInfoType($8B85);
    public static property ACTIVE_ATTRIBUTES:                     ProgramInfoType read new ProgramInfoType($8B89);
    public static property ACTIVE_ATTRIBUTE_MAX_LENGTH:           ProgramInfoType read new ProgramInfoType($8B8A);
    public static property ACTIVE_UNIFORMS:                       ProgramInfoType read new ProgramInfoType($8B86);
    public static property ACTIVE_UNIFORM_MAX_LENGTH:             ProgramInfoType read new ProgramInfoType($8B87);
    public static property TRANSFORM_FEEDBACK_BUFFER_MODE:        ProgramInfoType read new ProgramInfoType($8C7F);
    public static property TRANSFORM_FEEDBACK_VARYINGS:           ProgramInfoType read new ProgramInfoType($8C83);
    public static property TRANSFORM_FEEDBACK_VARYING_MAX_LENGTH: ProgramInfoType read new ProgramInfoType($8C76);
    public static property ACTIVE_UNIFORM_BLOCKS:                 ProgramInfoType read new ProgramInfoType($8A36);
    public static property ACTIVE_UNIFORM_BLOCK_MAX_NAME_LENGTH:  ProgramInfoType read new ProgramInfoType($8A35);
    public static property GEOMETRY_VERTICES_OUT:                 ProgramInfoType read new ProgramInfoType($8916);
    public static property GEOMETRY_INPUT_TYPE:                   ProgramInfoType read new ProgramInfoType($8917);
    public static property GEOMETRY_OUTPUT_TYPE:                  ProgramInfoType read new ProgramInfoType($8918);
    public static property GEOMETRY_SHADER_INVOCATIONS:           ProgramInfoType read new ProgramInfoType($887F);
    public static property TESS_CONTROL_OUTPUT_VERTICES:          ProgramInfoType read new ProgramInfoType($8E75);
    public static property TESS_GEN_MODE:                         ProgramInfoType read new ProgramInfoType($8E76);
    public static property TESS_GEN_SPACING:                      ProgramInfoType read new ProgramInfoType($8E77);
    public static property TESS_GEN_VERTEX_ORDER:                 ProgramInfoType read new ProgramInfoType($8E78);
    public static property TESS_GEN_POINT_MODE:                   ProgramInfoType read new ProgramInfoType($8E79);
    public static property COMPUTE_WORK_GROUP_SIZE:               ProgramInfoType read new ProgramInfoType($8267);
    public static property PROGRAM_SEPARABLE:                     ProgramInfoType read new ProgramInfoType($8258);
    public static property PROGRAM_BINARY_RETRIEVABLE_HINT:       ProgramInfoType read new ProgramInfoType($8257);
    public static property ACTIVE_ATOMIC_COUNTER_BUFFERS:         ProgramInfoType read new ProgramInfoType($92D9);
    
  end;
  
  //S
  ShaderInfoType = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property SHADER_TYPE:           ShaderInfoType read new ShaderInfoType($8B4F);
    public static property DELETE_STATUS:         ShaderInfoType read new ShaderInfoType($8B80);
    public static property COMPILE_STATUS:        ShaderInfoType read new ShaderInfoType($8B81);
    public static property INFO_LOG_LENGTH:       ShaderInfoType read new ShaderInfoType($8B84);
    public static property SHADER_SOURCE_LENGTH:  ShaderInfoType read new ShaderInfoType($8B88);
    public static property SPIR_V_BINARY:         ShaderInfoType read new ShaderInfoType($9552);
    
  end;
  
  //S
  SyncObjInfoType = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property OBJECT_TYPE:     SyncObjInfoType read new SyncObjInfoType($9112);
    public static property SYNC_CONDITION:  SyncObjInfoType read new SyncObjInfoType($9113);
    public static property SYNC_STATUS:     SyncObjInfoType read new SyncObjInfoType($9114);
    public static property SYNC_FLAGS:      SyncObjInfoType read new SyncObjInfoType($9115);
    
  end;
  
  //S
  QueryInfoType = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property SAMPLES_PASSED:                        QueryInfoType read new QueryInfoType($8914);
    public static property ANY_SAMPLES_PASSED:                    QueryInfoType read new QueryInfoType($8C2F);
    public static property ANY_SAMPLES_PASSED_CONSERVATIVE:       QueryInfoType read new QueryInfoType($8D6A);
    public static property PRIMITIVES_GENERATED:                  QueryInfoType read new QueryInfoType($8C87);
    public static property TRANSFORM_FEEDBACK_PRIMITIVES_WRITTEN: QueryInfoType read new QueryInfoType($8C88);
    public static property TIME_ELAPSED:                          QueryInfoType read new QueryInfoType($88BF);
    public static property TIMESTAMP:                             QueryInfoType read new QueryInfoType($8E28);
    
  end;
  
  //S
  GetQueryInfoName = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property QUERY_COUNTER_BITS:  GetQueryInfoName read new GetQueryInfoName($8864);
    public static property CURRENT_QUERY:       GetQueryInfoName read new GetQueryInfoName($8865);
    
  end;
  
  //S
  GetQueryObjectInfoName = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property RESULT:            GetQueryObjectInfoName read new GetQueryObjectInfoName($8866);
    public static property RESULT_AVAILABLE:  GetQueryObjectInfoName read new GetQueryObjectInfoName($8867);
    
  end;
  
  //S
  BufferInfoType = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property SIZE:              BufferInfoType read new BufferInfoType($8764);
    public static property USAGE:             BufferInfoType read new BufferInfoType($8765);
    public static property ACCESS:            BufferInfoType read new BufferInfoType($88BB);
    public static property ACCESS_FLAGS:      BufferInfoType read new BufferInfoType($911F);
    public static property IMMUTABLE_STORAGE: BufferInfoType read new BufferInfoType($821F);
    public static property MAPPED:            BufferInfoType read new BufferInfoType($88BC);
    public static property MAP_LENGTH:        BufferInfoType read new BufferInfoType($9120);
    public static property MAP_OFFSET:        BufferInfoType read new BufferInfoType($9121);
    public static property STORAGE_FLAGS:     BufferInfoType read new BufferInfoType($8220);
    public static property MAP_POINTER:       BufferInfoType read new BufferInfoType($88BD);
    
  end;
  
  {$endregion ...InfoType}
  
  //S
  InternalFormatInfoType = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property INTERNALFORMAT_SUPPORTED:                InternalFormatInfoType read new InternalFormatInfoType($826F);
    public static property INTERNALFORMAT_PREFERRED:                InternalFormatInfoType read new InternalFormatInfoType($8270);
    public static property INTERNALFORMAT_RED_SIZE:                 InternalFormatInfoType read new InternalFormatInfoType($8271);
    public static property INTERNALFORMAT_RED_TYPE:                 InternalFormatInfoType read new InternalFormatInfoType($8278);
    public static property MAX_WIDTH:                               InternalFormatInfoType read new InternalFormatInfoType($827E);
    public static property MAX_HEIGHT:                              InternalFormatInfoType read new InternalFormatInfoType($827F);
    public static property MAX_DEPTH:                               InternalFormatInfoType read new InternalFormatInfoType($8280);
    public static property MAX_LAYERS:                              InternalFormatInfoType read new InternalFormatInfoType($8281);
    public static property COLOR_COMPONENTS:                        InternalFormatInfoType read new InternalFormatInfoType($8283);
    public static property COLOR_RENDERABLE:                        InternalFormatInfoType read new InternalFormatInfoType($8286);
    public static property DEPTH_RENDERABLE:                        InternalFormatInfoType read new InternalFormatInfoType($8287);
    public static property STENCIL_RENDERABLE:                      InternalFormatInfoType read new InternalFormatInfoType($8288);
    public static property FRAMEBUFFER_RENDERABLE:                  InternalFormatInfoType read new InternalFormatInfoType($8289);
    public static property FRAMEBUFFER_RENDERABLE_LAYERED:          InternalFormatInfoType read new InternalFormatInfoType($828A);
    public static property FRAMEBUFFER_BLEND:                       InternalFormatInfoType read new InternalFormatInfoType($828B);
    public static property READ_PIXELS:                             InternalFormatInfoType read new InternalFormatInfoType($828C);
    public static property READ_PIXELS_FORMAT:                      InternalFormatInfoType read new InternalFormatInfoType($828D);
    public static property READ_PIXELS_TYPE:                        InternalFormatInfoType read new InternalFormatInfoType($828E);
    public static property TEXTURE_IMAGE_FORMAT:                    InternalFormatInfoType read new InternalFormatInfoType($828F);
    public static property TEXTURE_IMAGE_TYPE:                      InternalFormatInfoType read new InternalFormatInfoType($8290);
    public static property GET__TEXTURE_IMAGE_FORMAT:               InternalFormatInfoType read new InternalFormatInfoType($8291);
    public static property GET__TEXTURE_IMAGE_TYPE:                 InternalFormatInfoType read new InternalFormatInfoType($8292);
    public static property MIPMAP:                                  InternalFormatInfoType read new InternalFormatInfoType($8293);
    public static property GENERATE_MIPMAP:                         InternalFormatInfoType read new InternalFormatInfoType($8191);
    public static property AUTO_GENERATE_MIPMAP:                    InternalFormatInfoType read new InternalFormatInfoType($8295);
    public static property COLOR_ENCODING:                          InternalFormatInfoType read new InternalFormatInfoType($8296);
    public static property SRGB_READ:                               InternalFormatInfoType read new InternalFormatInfoType($8297);
    public static property SRGB_WRITE:                              InternalFormatInfoType read new InternalFormatInfoType($8298);
    public static property FILTER:                                  InternalFormatInfoType read new InternalFormatInfoType($829A);
    public static property VERTEX_TEXTURE:                          InternalFormatInfoType read new InternalFormatInfoType($829B);
    public static property TESS_CONTROL_TEXTURE:                    InternalFormatInfoType read new InternalFormatInfoType($829C);
    public static property TESS_EVALUATION_TEXTURE:                 InternalFormatInfoType read new InternalFormatInfoType($829D);
    public static property GEOMETRY_TEXTURE:                        InternalFormatInfoType read new InternalFormatInfoType($829E);
    public static property FRAGMENT_TEXTURE:                        InternalFormatInfoType read new InternalFormatInfoType($829F);
    public static property COMPUTE_TEXTURE:                         InternalFormatInfoType read new InternalFormatInfoType($82A0);
    public static property TEXTURE_SHADOW:                          InternalFormatInfoType read new InternalFormatInfoType($82A1);
    public static property TEXTURE_GATHER:                          InternalFormatInfoType read new InternalFormatInfoType($82A2);
    public static property TEXTURE_GATHER_SHADOW:                   InternalFormatInfoType read new InternalFormatInfoType($82A3);
    public static property SHADER_IMAGE_LOAD:                       InternalFormatInfoType read new InternalFormatInfoType($82A4);
    public static property SHADER_IMAGE_STORE:                      InternalFormatInfoType read new InternalFormatInfoType($82A5);
    public static property SHADER_IMAGE_ATOMIC:                     InternalFormatInfoType read new InternalFormatInfoType($82A6);
    public static property IMAGE_TEXEL_SIZE:                        InternalFormatInfoType read new InternalFormatInfoType($82A7);
    public static property IMAGE_COMPATIBILITY_CLASS:               InternalFormatInfoType read new InternalFormatInfoType($82A8);
    public static property IMAGE_PIXEL_FORMAT:                      InternalFormatInfoType read new InternalFormatInfoType($82A9);
    public static property IMAGE_PIXEL_TYPE:                        InternalFormatInfoType read new InternalFormatInfoType($82AA);
    public static property IMAGE_FORMAT_COMPATIBILITY_TYPE:         InternalFormatInfoType read new InternalFormatInfoType($90C7);
    public static property SIMULTANEOUS_TEXTURE_AND_DEPTH_TEST:     InternalFormatInfoType read new InternalFormatInfoType($82AC);
    public static property SIMULTANEOUS_TEXTURE_AND_STENCIL_TEST:   InternalFormatInfoType read new InternalFormatInfoType($82AD);
    public static property SIMULTANEOUS_TEXTURE_AND_DEPTH_WRITE:    InternalFormatInfoType read new InternalFormatInfoType($82AE);
    public static property SIMULTANEOUS_TEXTURE_AND_STENCIL_WRITE:  InternalFormatInfoType read new InternalFormatInfoType($82AF);
    public static property TEXTURE_COMPRESSED:                      InternalFormatInfoType read new InternalFormatInfoType($86A1);
    public static property TEXTURE_COMPRESSED_BLOCK_WIDTH:          InternalFormatInfoType read new InternalFormatInfoType($82B1);
    public static property TEXTURE_COMPRESSED_BLOCK_HEIGHT:         InternalFormatInfoType read new InternalFormatInfoType($82B2);
    public static property TEXTURE_COMPRESSED_BLOCK_SIZE:           InternalFormatInfoType read new InternalFormatInfoType($82B3);
    public static property CLEAR_BUFFER:                            InternalFormatInfoType read new InternalFormatInfoType($82B4);
    public static property TEXTURE_VIEW:                            InternalFormatInfoType read new InternalFormatInfoType($82B5);
    public static property VIEW_COMPATIBILITY_CLASS:                InternalFormatInfoType read new InternalFormatInfoType($82B6);
    public static property CLEAR_TEXTURE:                           InternalFormatInfoType read new InternalFormatInfoType($9365);
    
  end;
  
  //S
  TransformFeedbackInfoType = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property BUFFER_BINDING:  TransformFeedbackInfoType read new TransformFeedbackInfoType($8C8F);
    public static property BUFFER_START:    TransformFeedbackInfoType read new TransformFeedbackInfoType($8C84);
    public static property BUFFER_SIZE:     TransformFeedbackInfoType read new TransformFeedbackInfoType($8C85);
    public static property PAUSED:          TransformFeedbackInfoType read new TransformFeedbackInfoType($8E23);
    public static property ACTIVE:          TransformFeedbackInfoType read new TransformFeedbackInfoType($8E24);
    
  end;
  
  //S
  HintValue = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property FASTEST:   HintValue read new HintValue($1101);
    public static property NICEST:    HintValue read new HintValue($1102);
    public static property DONT_CARE: HintValue read new HintValue($1100);
    
  end;
  
  //S
  HintType = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property LINE_SMOOTH_HINT:                HintType read new HintType($0C52);
    public static property POLYGON_SMOOTH_HINT:             HintType read new HintType($0C53);
    public static property TEXTURE_COMPRESSION_HINT:        HintType read new HintType($84EF);
    public static property FRAGMENT_SHADER_DERIVATIVE_HINT: HintType read new HintType($8B8B);
    
  end;
  
  //S
  ObjectType = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property BUFFER:              ObjectType read new ObjectType($82E0);
    public static property FRAMEBUFFER:         ObjectType read new ObjectType($8D40);
    public static property PROGRAM_PIPELINE:    ObjectType read new ObjectType($82E4);
    public static property &PROGRAM:            ObjectType read new ObjectType($82E2);
    public static property QUERY:               ObjectType read new ObjectType($82E3);
    public static property RENDERBUFFER:        ObjectType read new ObjectType($8D41);
    public static property SAMPLER:             ObjectType read new ObjectType($82E6);
    public static property SHADER:              ObjectType read new ObjectType($82E1);
    public static property TEXTURE:             ObjectType read new ObjectType($1702);
    public static property TRANSFORM_FEEDBACK:  ObjectType read new ObjectType($8E22);
    public static property VERTEX_ARRAY:        ObjectType read new ObjectType($8074);
    
  end;
  
  //S
  PixelFilterMode = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property NEAREST: PixelFilterMode read new PixelFilterMode($2600);
    public static property LINEAR:  PixelFilterMode read new PixelFilterMode($2601);
    
  end;
  
  //S
  ColorClampTarget = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property GL_CLAMP_READ_COLOR: ColorClampTarget read new ColorClampTarget($891C);
    
  end;
  
  //S
  FrameBufferPart = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property NONE:            FrameBufferPart read new FrameBufferPart(0);
    public static property FRONT_LEFT:      FrameBufferPart read new FrameBufferPart($0400);
    public static property FRONT_RIGHT:     FrameBufferPart read new FrameBufferPart($0401);
    public static property BACK_LEFT:       FrameBufferPart read new FrameBufferPart($0402);
    public static property BACK_RIGHT:      FrameBufferPart read new FrameBufferPart($0403);
    public static property FRONT:           FrameBufferPart read new FrameBufferPart($0404);
    public static property BACK:            FrameBufferPart read new FrameBufferPart($0405);
    public static property LEFT:            FrameBufferPart read new FrameBufferPart($0406);
    public static property RIGHT:           FrameBufferPart read new FrameBufferPart($0407);
    public static property FRONT_AND_BACK:  FrameBufferPart read new FrameBufferPart($0408);
    
  end;
  
  //S
  LogicOpCode = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property CLEAR:         LogicOpCode read new LogicOpCode($1500);
    public static property &AND:          LogicOpCode read new LogicOpCode($1501);
    public static property AND_REVERSE:   LogicOpCode read new LogicOpCode($1502);
    public static property COPY:          LogicOpCode read new LogicOpCode($1503);
    public static property AND_INVERTED:  LogicOpCode read new LogicOpCode($1504);
    public static property NOOP:          LogicOpCode read new LogicOpCode($1505);
    public static property &XOR:          LogicOpCode read new LogicOpCode($1506);
    public static property &OR:           LogicOpCode read new LogicOpCode($1507);
    public static property NOR:           LogicOpCode read new LogicOpCode($1508);
    public static property EQUIV:         LogicOpCode read new LogicOpCode($1509);
    public static property INVERT:        LogicOpCode read new LogicOpCode($150A);
    public static property OR_REVERSE:    LogicOpCode read new LogicOpCode($150B);
    public static property COPY_INVERTED: LogicOpCode read new LogicOpCode($150C);
    public static property OR_INVERTED:   LogicOpCode read new LogicOpCode($150D);
    public static property NAND:          LogicOpCode read new LogicOpCode($150E);
    public static property &SET:          LogicOpCode read new LogicOpCode($150F);
    
  end;
  
  //S
  BlendFuncMode = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property ZERO:                      BlendFuncMode read new BlendFuncMode(0);
    public static property ONE:                       BlendFuncMode read new BlendFuncMode(1);
    public static property SRC_COLOR:                 BlendFuncMode read new BlendFuncMode($0300);
    public static property ONE_MINUS_SRC_COLOR:       BlendFuncMode read new BlendFuncMode($0301);
    public static property DST_COLOR:                 BlendFuncMode read new BlendFuncMode($0306);
    public static property ONE_MINUS_DST_COLOR:       BlendFuncMode read new BlendFuncMode($0307);
    public static property SRC_ALPHA:                 BlendFuncMode read new BlendFuncMode($0302);
    public static property ONE_MINUS_SRC_ALPHA:       BlendFuncMode read new BlendFuncMode($0303);
    public static property DST_ALPHA:                 BlendFuncMode read new BlendFuncMode($0304);
    public static property ONE_MINUS_DST_ALPHA:       BlendFuncMode read new BlendFuncMode($0305);
    public static property CONSTANT_COLOR:            BlendFuncMode read new BlendFuncMode($8001);
    public static property ONE_MINUS_CONSTANT_COLOR:  BlendFuncMode read new BlendFuncMode($8002);
    public static property CONSTANT_ALPHA:            BlendFuncMode read new BlendFuncMode($8003);
    public static property ONE_MINUS_CONSTANT_ALPHA:  BlendFuncMode read new BlendFuncMode($8004);
    public static property SRC_ALPHA_SATURATE:        BlendFuncMode read new BlendFuncMode($0308);
    public static property SRC1_COLOR:                BlendFuncMode read new BlendFuncMode($88F9);
    public static property ONE_MINUS_SRC1_COLOR:      BlendFuncMode read new BlendFuncMode($88FA);
    public static property SRC1_ALPHA:                BlendFuncMode read new BlendFuncMode($8589);
    public static property ONE_MINUS_SRC1_ALPHA:      BlendFuncMode read new BlendFuncMode($88FB);
    
  end;
  
  //S
  BlendEquationMode = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property FUNC_ADD:              BlendEquationMode read new BlendEquationMode($8006);
    public static property FUNC_SUBTRACT:         BlendEquationMode read new BlendEquationMode($800A);
    public static property FUNC_REVERSE_SUBTRACT: BlendEquationMode read new BlendEquationMode($800B);
    public static property MIN:                   BlendEquationMode read new BlendEquationMode($8007);
    public static property MAX:                   BlendEquationMode read new BlendEquationMode($8008);
    
  end;
  
  //S
  StencilOpFailMode = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property KEEP:      StencilOpFailMode read new StencilOpFailMode($1E00);
    public static property ZERO:      StencilOpFailMode read new StencilOpFailMode(0);
    public static property REPLACE:   StencilOpFailMode read new StencilOpFailMode($1E01);
    public static property INCR:      StencilOpFailMode read new StencilOpFailMode($1E02);
    public static property INCR_WRAP: StencilOpFailMode read new StencilOpFailMode($8507);
    public static property DECR:      StencilOpFailMode read new StencilOpFailMode($1E03);
    public static property DECR_WRAP: StencilOpFailMode read new StencilOpFailMode($8508);
    public static property INVERT:    StencilOpFailMode read new StencilOpFailMode($150A);
    
  end;
  
  //S
  FuncActivationMode = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property NEVER:     FuncActivationMode read new FuncActivationMode($0200);
    public static property LESS:      FuncActivationMode read new FuncActivationMode($0201);
    public static property LEQUAL:    FuncActivationMode read new FuncActivationMode($0203);
    public static property GREATER:   FuncActivationMode read new FuncActivationMode($0204);
    public static property GEQUAL:    FuncActivationMode read new FuncActivationMode($0206);
    public static property EQUAL:     FuncActivationMode read new FuncActivationMode($0202);
    public static property NOTEQUAL:  FuncActivationMode read new FuncActivationMode($0205);
    public static property ALWAYS:    FuncActivationMode read new FuncActivationMode($0207);
    
  end;
  
  //S
  PolygonRasterizationMode = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property POINT: PolygonRasterizationMode read new PolygonRasterizationMode($1B00);
    public static property LINE:  PolygonRasterizationMode read new PolygonRasterizationMode($1B01);
    public static property FILL:  PolygonRasterizationMode read new PolygonRasterizationMode($1B02);
    
  end;
  
  //S
  PolygonFace = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property FRONT:           PolygonFace read new PolygonFace($0404);
    public static property BACK:            PolygonFace read new PolygonFace($0405);
    public static property FRONT_AND_BACK:  PolygonFace read new PolygonFace($0408);
    
  end;
  
  //S
  FrontFaceDirection = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property СW:  FrontFaceDirection read new FrontFaceDirection($0900);
    public static property СCW: FrontFaceDirection read new FrontFaceDirection($0901);
    
  end;
  
  //S
  PointInfoType = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property FADE_THRESHOLD_SIZE: PointInfoType read new PointInfoType($8128);
    public static property SPRITE_COORD_ORIGIN: PointInfoType read new PointInfoType($8CA0);
    
  end;
  
  //S
  MultisampleInfoType = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property GL_SAMPLE_POSITION:  MultisampleInfoType read new MultisampleInfoType($8E50);
    
  end;
  
  //S
  ClipDepthMode = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property NEGATIVE_ONE_TO_ONE: ClipDepthMode read new ClipDepthMode($935E);
    public static property ZERO_TO_ONE:         ClipDepthMode read new ClipDepthMode($935F);
    
  end;
  
  //S
  ClipOriginMode = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property LOWER_LEFT:  ClipOriginMode read new ClipOriginMode($8CA1);
    public static property UPPER_LEFT:  ClipOriginMode read new ClipOriginMode($8CA2);
    
  end;
  
  //S
  TransformFeedbackBindTarget = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property GL_TRANSFORM_FEEDBACK: TransformFeedbackBindTarget read new TransformFeedbackBindTarget($8E22);
    
  end;
  
  //S
  TransformFeedbackBufferMode = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property INTERLEAVED_ATTRIBS: TransformFeedbackBufferMode read new TransformFeedbackBufferMode($8C8C);
    public static property SEPARATE_ATTRIBS:    TransformFeedbackBufferMode read new TransformFeedbackBufferMode($8C8D);
    
  end;
  
  //S
  ConditionalRenderingMode = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property WAIT:                        ConditionalRenderingMode read new ConditionalRenderingMode($8E13);
    public static property NO_WAIT:                     ConditionalRenderingMode read new ConditionalRenderingMode($8E14);
    public static property BY_REGION_WAIT:              ConditionalRenderingMode read new ConditionalRenderingMode($8E15);
    public static property BY_REGION_NO_WAIT:           ConditionalRenderingMode read new ConditionalRenderingMode($8E16);
    public static property WAIT_INVERTED:               ConditionalRenderingMode read new ConditionalRenderingMode($8E17);
    public static property NO_WAIT_INVERTED:            ConditionalRenderingMode read new ConditionalRenderingMode($8E18);
    public static property BY_REGION_WAIT_INVERTED:     ConditionalRenderingMode read new ConditionalRenderingMode($8E19);
    public static property BY_REGION_NO_WAIT_INVERTED:  ConditionalRenderingMode read new ConditionalRenderingMode($8E1A);
    
  end;
  
  //S
  VertexAttribInfoType = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property ELEMENT_ARRAY_BUFFER_BINDING:        VertexAttribInfoType read new VertexAttribInfoType($8895);
    public static property VERTEX_ATTRIB_ARRAY_ENABLED:         VertexAttribInfoType read new VertexAttribInfoType($8622);
    public static property VERTEX_ATTRIB_ARRAY_SIZE:            VertexAttribInfoType read new VertexAttribInfoType($8623);
    public static property VERTEX_ATTRIB_ARRAY_STRIDE:          VertexAttribInfoType read new VertexAttribInfoType($8624);
    public static property VERTEX_ATTRIB_ARRAY_TYPE:            VertexAttribInfoType read new VertexAttribInfoType($8625);
    public static property VERTEX_ATTRIB_ARRAY_NORMALIZED:      VertexAttribInfoType read new VertexAttribInfoType($886A);
    public static property VERTEX_ATTRIB_ARRAY_INTEGER:         VertexAttribInfoType read new VertexAttribInfoType($88FD);
    public static property VERTEX_ATTRIB_ARRAY_LONG:            VertexAttribInfoType read new VertexAttribInfoType($874E);
    public static property VERTEX_ATTRIB_ARRAY_DIVISOR:         VertexAttribInfoType read new VertexAttribInfoType($88FE);
    public static property VERTEX_ATTRIB_RELATIVE_OFFSET:       VertexAttribInfoType read new VertexAttribInfoType($82D5);
    public static property VERTEX_BINDING_OFFSET:               VertexAttribInfoType read new VertexAttribInfoType($82D7);
    public static property VERTEX_ATTRIB_ARRAY_BUFFER_BINDING:  VertexAttribInfoType read new VertexAttribInfoType($889F);
    public static property VERTEX_ATTRIB_BINDING:               VertexAttribInfoType read new VertexAttribInfoType($82D4);
    public static property CURRENT_VERTEX_ATTRIB:               VertexAttribInfoType read new VertexAttribInfoType($8626);
    
  end;
  
  //S
  PatchMode = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property VERTICES:            PatchMode read new PatchMode($8E72);
    public static property DEFAULT_INNER_LEVEL: PatchMode read new PatchMode($8E73);
    public static property DEFAULT_OUTER_LEVEL: PatchMode read new PatchMode($8E74);
    
  end;
  
  //S
  PrimitiveType = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property POINTS:                    PrimitiveType read new PrimitiveType($0000);
    
    public static property LINES:                     PrimitiveType read new PrimitiveType($0001);
    public static property LINE_LOOP:                 PrimitiveType read new PrimitiveType($0002);
    public static property LINE_STRIP:                PrimitiveType read new PrimitiveType($0003);
    public static property LINES_ADJACENCY:           PrimitiveType read new PrimitiveType($000A);
    public static property LINE_STRIP_ADJACENCY:      PrimitiveType read new PrimitiveType($000B);
    
    public static property TRIANGLES:                 PrimitiveType read new PrimitiveType($0004);
    public static property TRIANGLE_STRIP:            PrimitiveType read new PrimitiveType($0005);
    public static property TRIANGLE_FAN:              PrimitiveType read new PrimitiveType($0006);
    public static property TRIANGLES_ADJACENCY:       PrimitiveType read new PrimitiveType($000C);
    public static property TRIANGLE_STRIP_ADJACENCY:  PrimitiveType read new PrimitiveType($000D);
    
    public static property PATCHES:                   PrimitiveType read new PrimitiveType($000E);
    
  end;
  
  //S
  FramebufferAttachmentInfoType = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property OBJECT_TYPE:           FramebufferAttachmentInfoType read new FramebufferAttachmentInfoType($8CD0); // FramebufferAttachmentObjectType
    
    // non- NONE
    public static property OBJECT_NAME:           FramebufferAttachmentInfoType read new FramebufferAttachmentInfoType($8CD1); // UInt32
    public static property RED_SIZE:              FramebufferAttachmentInfoType read new FramebufferAttachmentInfoType($8212); // Int32
    public static property GREEN_SIZE:            FramebufferAttachmentInfoType read new FramebufferAttachmentInfoType($8213); // Int32
    public static property BLUE_SIZE:             FramebufferAttachmentInfoType read new FramebufferAttachmentInfoType($8214); // Int32
    public static property ALPHA_SIZE:            FramebufferAttachmentInfoType read new FramebufferAttachmentInfoType($8215); // Int32
    public static property DEPTH_SIZE:            FramebufferAttachmentInfoType read new FramebufferAttachmentInfoType($8216); // Int32
    public static property STENCIL_SIZE:          FramebufferAttachmentInfoType read new FramebufferAttachmentInfoType($8217); // Int32
    public static property COMPONENT_TYPE:        FramebufferAttachmentInfoType read new FramebufferAttachmentInfoType($8211); // Int32
    public static property COLOR_ENCODING:        FramebufferAttachmentInfoType read new FramebufferAttachmentInfoType($8210); // Int32
    
    // TEXTURE
    public static property TEXTURE_LEVEL:         FramebufferAttachmentInfoType read new FramebufferAttachmentInfoType($8CD2); // Int32
    public static property TEXTURE_CUBE_MAP_FACE: FramebufferAttachmentInfoType read new FramebufferAttachmentInfoType($8CD3); // Int32
    public static property LAYERED:               FramebufferAttachmentInfoType read new FramebufferAttachmentInfoType($8DA7); // 32-битное boolean (Int32, 0=False, остальное=True)
    public static property TEXTURE_LAYER:         FramebufferAttachmentInfoType read new FramebufferAttachmentInfoType($8CD4); // Int32
    
  end;
  
  //S
  FramebufferInfoType = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property WIDTH:                   FramebufferInfoType read new FramebufferInfoType($9310);
    public static property HEIGHT:                  FramebufferInfoType read new FramebufferInfoType($9311);
    public static property LAYERS:                  FramebufferInfoType read new FramebufferInfoType($9312);
    public static property SAMPLES:                 FramebufferInfoType read new FramebufferInfoType($9313);
    public static property FIXED_SAMPLE_LOCATIONS:  FramebufferInfoType read new FramebufferInfoType($9314);
    
  end;
  
  //S
  FramebufferBindTarget = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property READ_FRAMEBUFFER:  FramebufferBindTarget read new FramebufferBindTarget($8CA8);
    public static property DRAW_FRAMEBUFFER:  FramebufferBindTarget read new FramebufferBindTarget($8CA9);
    public static property FRAMEBUFFER:       FramebufferBindTarget read new FramebufferBindTarget($8D40);
    
  end;
  
  //S
  TextureCubeSide = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property POSITIVE_X:  TextureCubeSide read new TextureCubeSide($8515);
    public static property NEGATIVE_X:  TextureCubeSide read new TextureCubeSide($8516);
    public static property POSITIVE_Y:  TextureCubeSide read new TextureCubeSide($8517);
    public static property NEGATIVE_Y:  TextureCubeSide read new TextureCubeSide($8518);
    public static property POSITIVE_Z:  TextureCubeSide read new TextureCubeSide($8519);
    public static property NEGATIVE_Z:  TextureCubeSide read new TextureCubeSide($851A);
    
  end;
  
  //S
  RenderbufferBindTarget = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property RENDERBUFFER: RenderbufferBindTarget read new RenderbufferBindTarget($8D41);
    
  end;
  
  //S
  AccessType = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property READ_ONLY:   AccessType read new AccessType($88B8);
    public static property WRITE_ONLY:  AccessType read new AccessType($88B9);
    public static property READ_WRITE:  AccessType read new AccessType($88BA);
    
  end;
  
  //S
  TextureInfoType = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property DEPTH_STENCIL_TEXTURE_MODE:  TextureInfoType read new TextureInfoType($90EA);
    public static property BASE_LEVEL:                  TextureInfoType read new TextureInfoType($813C);
    public static property BORDER_COLOR:                TextureInfoType read new TextureInfoType($1004);
    public static property COMPARE_MODE:                TextureInfoType read new TextureInfoType($884C);
    public static property COMPARE_FUNC:                TextureInfoType read new TextureInfoType($884D);
    public static property LOD_BIAS:                    TextureInfoType read new TextureInfoType($8501);
    public static property MAG_FILTER:                  TextureInfoType read new TextureInfoType($2800);
    public static property MAX_ANISOTROPY:              TextureInfoType read new TextureInfoType($84FE);
    public static property MAX_LEVEL:                   TextureInfoType read new TextureInfoType($813D);
    public static property MAX_LOD:                     TextureInfoType read new TextureInfoType($813B);
    public static property MIN_FILTER:                  TextureInfoType read new TextureInfoType($2801);
    public static property MIN_LOD:                     TextureInfoType read new TextureInfoType($813A);
    public static property SWIZZLE_R:                   TextureInfoType read new TextureInfoType($8E42);
    public static property SWIZZLE_G:                   TextureInfoType read new TextureInfoType($8E43);
    public static property SWIZZLE_B:                   TextureInfoType read new TextureInfoType($8E44);
    public static property SWIZZLE_A:                   TextureInfoType read new TextureInfoType($8E45);
    public static property SWIZZLE_RGBA:                TextureInfoType read new TextureInfoType($8E46);
    public static property WRAP_S:                      TextureInfoType read new TextureInfoType($2802);
    public static property WRAP_T:                      TextureInfoType read new TextureInfoType($2803);
    public static property WRAP_R:                      TextureInfoType read new TextureInfoType($8072);
    
  end;
  
  //S
  PixelInfoType = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property UNPACK_SWAP_BYTES:               PixelInfoType read new PixelInfoType($0CF0);
    public static property UNPACK_LSB_FIRST:                PixelInfoType read new PixelInfoType($0CF1);
    public static property UNPACK_ROW_LENGTH:               PixelInfoType read new PixelInfoType($0CF2);
    public static property UNPACK_SKIP_ROWS:                PixelInfoType read new PixelInfoType($0CF3);
    public static property UNPACK_SKIP_PIXELS:              PixelInfoType read new PixelInfoType($0CF4);
    public static property UNPACK_ALIGNMENT:                PixelInfoType read new PixelInfoType($0CF5);
    public static property UNPACK_IMAGE_HEIGHT:             PixelInfoType read new PixelInfoType($806E);
    public static property UNPACK_SKIP_IMAGES:              PixelInfoType read new PixelInfoType($806D);
    public static property UNPACK_COMPRESSED_BLOCK_WIDTH:   PixelInfoType read new PixelInfoType($9127);
    public static property UNPACK_COMPRESSED_BLOCK_HEIGHT:  PixelInfoType read new PixelInfoType($9128);
    public static property UNPACK_COMPRESSED_BLOCK_DEPTH:   PixelInfoType read new PixelInfoType($9129);
    public static property UNPACK_COMPRESSED_BLOCK_SIZE:    PixelInfoType read new PixelInfoType($912A);
    public static property PACK_SWAP_BYTES:                 PixelInfoType read new PixelInfoType($0D00);
    public static property PACK_LSB_FIRST:                  PixelInfoType read new PixelInfoType($0D01);
    public static property PACK_ROW_LENGTH:                 PixelInfoType read new PixelInfoType($0D02);
    public static property PACK_SKIP_ROWS:                  PixelInfoType read new PixelInfoType($0D03);
    public static property PACK_SKIP_PIXELS:                PixelInfoType read new PixelInfoType($0D04);
    public static property PACK_ALIGNMENT:                  PixelInfoType read new PixelInfoType($0D05);
    public static property PACK_IMAGE_HEIGHT:               PixelInfoType read new PixelInfoType($806C);
    public static property PACK_SKIP_IMAGES:                PixelInfoType read new PixelInfoType($806B);
    public static property PACK_COMPRESSED_BLOCK_WIDTH:     PixelInfoType read new PixelInfoType($912B);
    public static property PACK_COMPRESSED_BLOCK_HEIGHT:    PixelInfoType read new PixelInfoType($912C);
    public static property PACK_COMPRESSED_BLOCK_DEPTH:     PixelInfoType read new PixelInfoType($912D);
    public static property PACK_COMPRESSED_BLOCK_SIZE:      PixelInfoType read new PixelInfoType($912E);
    
  end;
  
  //S
  TextureUnitId = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property Texture[i: integer]: TextureUnitId read new TextureUnitId($84C0+i);
    
  end;
  
  //S
  FramebufferAttachmentPoint = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    // custom framebuffer
    public static property COLOR_ATTACHMENT[i: integer]:  FramebufferAttachmentPoint read new FramebufferAttachmentPoint($8CE0+i);
    public static property DEPTH_ATTACHMENT:              FramebufferAttachmentPoint read new FramebufferAttachmentPoint($8D00);
    public static property STENCIL_ATTACHMENT:            FramebufferAttachmentPoint read new FramebufferAttachmentPoint($8D20);
    public static property DEPTH_STENCIL_ATTACHMENT:      FramebufferAttachmentPoint read new FramebufferAttachmentPoint($821A);
    
    // default framebuffer
    public static property FRONT:                         FramebufferAttachmentPoint read new FramebufferAttachmentPoint($0404);
    public static property FRONT_LEFT:                    FramebufferAttachmentPoint read new FramebufferAttachmentPoint($0400);
    public static property FRONT_RIGHT:                   FramebufferAttachmentPoint read new FramebufferAttachmentPoint($0401);
    public static property BACK:                          FramebufferAttachmentPoint read new FramebufferAttachmentPoint($0405);
    public static property BACK_LEFT:                     FramebufferAttachmentPoint read new FramebufferAttachmentPoint($0402);
    public static property BACK_RIGHT:                    FramebufferAttachmentPoint read new FramebufferAttachmentPoint($0403);
    public static property DEPTH:                         FramebufferAttachmentPoint read new FramebufferAttachmentPoint($1801);
    public static property STENCIL:                       FramebufferAttachmentPoint read new FramebufferAttachmentPoint($1802);
    
  end;
  
  //S
  TextureBindTarget = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property T_1D:                          TextureBindTarget read new TextureBindTarget($0DE0);
    public static property T_2D:                          TextureBindTarget read new TextureBindTarget($0DE1);
    public static property T_3D:                          TextureBindTarget read new TextureBindTarget($806F);
    public static property T_1D_ARRAY:                    TextureBindTarget read new TextureBindTarget($8C18);
    public static property T_2D_ARRAY:                    TextureBindTarget read new TextureBindTarget($8C1A);
    public static property T_RECTANGLE:                   TextureBindTarget read new TextureBindTarget($84F5);
    public static property T_BUFFER:                      TextureBindTarget read new TextureBindTarget($8C2A);
    public static property T_CUBE_MAP:                    TextureBindTarget read new TextureBindTarget($8513);
    public static property T_CUBE_MAP_ARRAY:              TextureBindTarget read new TextureBindTarget($9009);
    public static property T_2D_MULTISAMPLE:              TextureBindTarget read new TextureBindTarget($9100);
    public static property PROXY_T_2D_MULTISAMPLE:        TextureBindTarget read new TextureBindTarget($9101);
    public static property T_2D_MULTISAMPLE_ARRAY:        TextureBindTarget read new TextureBindTarget($9102);
    public static property PROXY_T_2D_MULTISAMPLE_ARRAY:  TextureBindTarget read new TextureBindTarget($9103);
    
  end;
  
  //S
  ShaderPrecisionFormatType = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property LOW_FLOAT:     ShaderPrecisionFormatType read new ShaderPrecisionFormatType($8DF0);
    public static property MEDIUM_FLOAT:  ShaderPrecisionFormatType read new ShaderPrecisionFormatType($8DF1);
    public static property HIGH_FLOAT:    ShaderPrecisionFormatType read new ShaderPrecisionFormatType($8DF2);
    public static property LOW_INT:       ShaderPrecisionFormatType read new ShaderPrecisionFormatType($8DF3);
    public static property MEDIUM_INT:    ShaderPrecisionFormatType read new ShaderPrecisionFormatType($8DF4);
    public static property HIGH_INT:      ShaderPrecisionFormatType read new ShaderPrecisionFormatType($8DF5);
    
  end;
  
  //S
  ProgramInterfaceProperty = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property ACTIVE_VARIABLES:                      ProgramInterfaceProperty read new ProgramInterfaceProperty($9305);
    public static property BUFFER_BINDING:                        ProgramInterfaceProperty read new ProgramInterfaceProperty($9302);
    public static property NUM_ACTIVE_VARIABLES:                  ProgramInterfaceProperty read new ProgramInterfaceProperty($9304);
    public static property ARRAY_SIZE:                            ProgramInterfaceProperty read new ProgramInterfaceProperty($92FB);
    public static property ARRAY_STRIDE:                          ProgramInterfaceProperty read new ProgramInterfaceProperty($92FE);
    public static property BLOCK_INDEX:                           ProgramInterfaceProperty read new ProgramInterfaceProperty($92FD);
    public static property IS_ROW_MAJOR:                          ProgramInterfaceProperty read new ProgramInterfaceProperty($9300);
    public static property MATRIX_STRIDE:                         ProgramInterfaceProperty read new ProgramInterfaceProperty($92FF);
    public static property ATOMIC_COUNTER_BUFFER_INDEX:           ProgramInterfaceProperty read new ProgramInterfaceProperty($9301);
    public static property BUFFER_DATA_SIZE:                      ProgramInterfaceProperty read new ProgramInterfaceProperty($9303);
    public static property NUM_COMPATIBLE_SUBROUTINES:            ProgramInterfaceProperty read new ProgramInterfaceProperty($8E4A);
    public static property COMPATIBLE_SUBROUTINES:                ProgramInterfaceProperty read new ProgramInterfaceProperty($8E4B);
    public static property IS_PER_PATCH:                          ProgramInterfaceProperty read new ProgramInterfaceProperty($92E7);
    public static property LOCATION:                              ProgramInterfaceProperty read new ProgramInterfaceProperty($930E);
    public static property LOCATION_COMPONENT:                    ProgramInterfaceProperty read new ProgramInterfaceProperty($934A);
    public static property LOCATION_INDEX:                        ProgramInterfaceProperty read new ProgramInterfaceProperty($930F);
    public static property NAME_LENGTH:                           ProgramInterfaceProperty read new ProgramInterfaceProperty($92F9);
    public static property OFFSET:                                ProgramInterfaceProperty read new ProgramInterfaceProperty($92FC);
    public static property REFERENCED_BY_VERTEX_SHADER:           ProgramInterfaceProperty read new ProgramInterfaceProperty($9306);
    public static property REFERENCED_BY_TESS_CONTROL_SHADER:     ProgramInterfaceProperty read new ProgramInterfaceProperty($9307);
    public static property REFERENCED_BY_TESS_EVALUATION_SHADER:  ProgramInterfaceProperty read new ProgramInterfaceProperty($9308);
    public static property REFERENCED_BY_GEOMETRY_SHADER:         ProgramInterfaceProperty read new ProgramInterfaceProperty($9309);
    public static property REFERENCED_BY_FRAGMENT_SHADER:         ProgramInterfaceProperty read new ProgramInterfaceProperty($930A);
    public static property REFERENCED_BY_COMPUTE_SHADER:          ProgramInterfaceProperty read new ProgramInterfaceProperty($930B);
    public static property TRANSFORM_FEEDBACK_BUFFER_INDEX:       ProgramInterfaceProperty read new ProgramInterfaceProperty($934B);
    public static property TRANSFORM_FEEDBACK_BUFFER_STRIDE:      ProgramInterfaceProperty read new ProgramInterfaceProperty($934C);
    public static property TOP_LEVEL_ARRAY_SIZE:                  ProgramInterfaceProperty read new ProgramInterfaceProperty($930C);
    public static property TOP_LEVEL_ARRAY_STRIDE:                ProgramInterfaceProperty read new ProgramInterfaceProperty($930D);
    public static property &TYPE:                                 ProgramInterfaceProperty read new ProgramInterfaceProperty($92FA);
    
//    public static property TEXTURE_BUFFER:                         read new ProgramInterfaceProperty($8C2A); // типа существует, но это не точно
    
  end;
  
  //S
  ProgramInterfaceInfoType = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property ACTIVE_RESOURCES:                ProgramInterfaceInfoType read new ProgramInterfaceInfoType($92F5);
    public static property MAX_NAME_LENGTH:                 ProgramInterfaceInfoType read new ProgramInterfaceInfoType($92F6);
    public static property MAX_NUM_ACTIVE_VARIABLES:        ProgramInterfaceInfoType read new ProgramInterfaceInfoType($92F7);
    public static property MAX_NUM_COMPATIBLE_SUBROUTINES:  ProgramInterfaceInfoType read new ProgramInterfaceInfoType($92F8);
    
  end;
  
  //S
  ProgramInterfaceType = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property UNIFORM:                             ProgramInterfaceType read new ProgramInterfaceType($92E1);
    public static property UNIFORM_BLOCK:                       ProgramInterfaceType read new ProgramInterfaceType($92E2);
    public static property ATOMIC_COUNTER_BUFFER:               ProgramInterfaceType read new ProgramInterfaceType($92C0);
    public static property PROGRAM_INPUT:                       ProgramInterfaceType read new ProgramInterfaceType($92E3);
    public static property PROGRAM_OUTPUT:                      ProgramInterfaceType read new ProgramInterfaceType($92E4);
    public static property VERTEX_SUBROUTINE:                   ProgramInterfaceType read new ProgramInterfaceType($92E8);
    public static property TESS_CONTROL_SUBROUTINE:             ProgramInterfaceType read new ProgramInterfaceType($92E9);
    public static property TESS_EVALUATION_SUBROUTINE:          ProgramInterfaceType read new ProgramInterfaceType($92EA);
    public static property GEOMETRY_SUBROUTINE:                 ProgramInterfaceType read new ProgramInterfaceType($92EB);
    public static property FRAGMENT_SUBROUTINE:                 ProgramInterfaceType read new ProgramInterfaceType($92EC);
    public static property COMPUTE_SUBROUTINE:                  ProgramInterfaceType read new ProgramInterfaceType($92ED);
    public static property VERTEX_SUBROUTINE_UNIFORM:           ProgramInterfaceType read new ProgramInterfaceType($92EE);
    public static property TESS_CONTROL_SUBROUTINE_UNIFORM:     ProgramInterfaceType read new ProgramInterfaceType($92EF);
    public static property TESS_EVALUATION_SUBROUTINE_UNIFORM:  ProgramInterfaceType read new ProgramInterfaceType($92F0);
    public static property GEOMETRY_SUBROUTINE_UNIFORM:         ProgramInterfaceType read new ProgramInterfaceType($92F1);
    public static property FRAGMENT_SUBROUTINE_UNIFORM:         ProgramInterfaceType read new ProgramInterfaceType($92F2);
    public static property COMPUTE_SUBROUTINE_UNIFORM:          ProgramInterfaceType read new ProgramInterfaceType($92F3);
    public static property TRANSFORM_FEEDBACK_VARYING:          ProgramInterfaceType read new ProgramInterfaceType($92F4);
    public static property BUFFER_VARIABLE:                     ProgramInterfaceType read new ProgramInterfaceType($92E5);
    public static property SHADER_STORAGE_BLOCK:                ProgramInterfaceType read new ProgramInterfaceType($92E6);
    public static property TRANSFORM_FEEDBACK_BUFFER:           ProgramInterfaceType read new ProgramInterfaceType($8C8E);
    
  end;
  
  //S
  ProgramParameterType = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property SEPARABLE:               ProgramParameterType read new ProgramParameterType($8258);
    public static property BINARY_RETRIEVABLE_HINT: ProgramParameterType read new ProgramParameterType($8257);
    
  end;
  
  //S
  ShaderType = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property VERTEX_SHADER:           ShaderType read new ShaderType($8B31);
    public static property TESS_CONTROL_SHADER:     ShaderType read new ShaderType($8E88);
    public static property TESS_EVALUATION_SHADER:  ShaderType read new ShaderType($8E87);
    public static property GEOMETRY_SHADER:         ShaderType read new ShaderType($8DD9);
    public static property FRAGMENT_SHADER:         ShaderType read new ShaderType($8B30);
    public static property COMPUTE_SHADER:          ShaderType read new ShaderType($91B9);
    
    public static function operator implicit(st: ShaderType): ProgramPipelineInfoType := new ProgramPipelineInfoType(st.val);
    
  end;
  
  //S
  FenceCondition = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property GL_SYNC_GPU_COMMANDS_COMPLETE: FenceCondition read new FenceCondition($9117);
    
  end;
  
  //S
  BufferBindType = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property ARRAY_BUFFER:              BufferBindType read new BufferBindType($8892);
    public static property ATOMIC_COUNTER_BUFFER:     BufferBindType read new BufferBindType($92C0);
    public static property COPY_READ_BUFFER:          BufferBindType read new BufferBindType($8F36);
    public static property COPY_WRITE_BUFFER:         BufferBindType read new BufferBindType($8F37);
    public static property DISPATCH_INDIRECT_BUFFER:  BufferBindType read new BufferBindType($90EE);
    public static property DRAW_INDIRECT_BUFFER:      BufferBindType read new BufferBindType($8F3F);
    public static property ELEMENT_ARRAY_BUFFER:      BufferBindType read new BufferBindType($8893);
    public static property PIXEL_PACK_BUFFER:         BufferBindType read new BufferBindType($88EB);
    public static property PIXEL_UNPACK_BUFFER:       BufferBindType read new BufferBindType($88EC);
    public static property QUERY_BUFFER:              BufferBindType read new BufferBindType($9192);
    public static property SHADER_STORAGE_BUFFER:     BufferBindType read new BufferBindType($90D2);
    public static property TEXTURE_BUFFER:            BufferBindType read new BufferBindType($8C2A);
    public static property TRANSFORM_FEEDBACK_BUFFER: BufferBindType read new BufferBindType($8C8E);
    public static property UNIFORM_BUFFER:            BufferBindType read new BufferBindType($8A11);
    
  end;
  
  //S
  CopyableImageType = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property GL_RENDERBUFFER: CopyableImageType read new CopyableImageType($8D41);
    
    public static function operator implicit(v: BufferBindType): CopyableImageType := new CopyableImageType(v.val);
    
  end;
  
  //S
  BufferDataUsage = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property STREAM_DRAW:   BufferDataUsage read new BufferDataUsage($88E0);
    public static property STREAM_READ:   BufferDataUsage read new BufferDataUsage($88E1);
    public static property STREAM_COPY:   BufferDataUsage read new BufferDataUsage($88E2);
    public static property STATIC_DRAW:   BufferDataUsage read new BufferDataUsage($88E4);
    public static property STATIC_READ:   BufferDataUsage read new BufferDataUsage($88E5);
    public static property STATIC_COPY:   BufferDataUsage read new BufferDataUsage($88E6);
    public static property DYNAMIC_DRAW:  BufferDataUsage read new BufferDataUsage($88E8);
    public static property DYNAMIC_READ:  BufferDataUsage read new BufferDataUsage($88E9);
    public static property DYNAMIC_COPY:  BufferDataUsage read new BufferDataUsage($88EA);
    
  end;
  
  //S
  InternalDataFormat = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property R8:        InternalDataFormat read new InternalDataFormat($8229);
    public static property R8I:       InternalDataFormat read new InternalDataFormat($8231);
    public static property R8UI:      InternalDataFormat read new InternalDataFormat($8232);
    public static property R16:       InternalDataFormat read new InternalDataFormat($822A);
    public static property R16I:      InternalDataFormat read new InternalDataFormat($8233);
    public static property R16UI:     InternalDataFormat read new InternalDataFormat($8234);
    public static property R16F:      InternalDataFormat read new InternalDataFormat($822D);
    public static property R32I:      InternalDataFormat read new InternalDataFormat($8235);
    public static property R32UI:     InternalDataFormat read new InternalDataFormat($8236);
    public static property R32F:      InternalDataFormat read new InternalDataFormat($822E);
    
    public static property RG8:       InternalDataFormat read new InternalDataFormat($822B);
    public static property RG8I:      InternalDataFormat read new InternalDataFormat($8237);
    public static property RG8UI:     InternalDataFormat read new InternalDataFormat($8238);
    public static property RG16:      InternalDataFormat read new InternalDataFormat($822C);
    public static property RG16I:     InternalDataFormat read new InternalDataFormat($8239);
    public static property RG16UI:    InternalDataFormat read new InternalDataFormat($823A);
    public static property RG16F:     InternalDataFormat read new InternalDataFormat($822F);
    public static property RG32I:     InternalDataFormat read new InternalDataFormat($823B);
    public static property RG32UI:    InternalDataFormat read new InternalDataFormat($823C);
    public static property RG32F:     InternalDataFormat read new InternalDataFormat($8230);
    
    public static property RGB8:      InternalDataFormat read new InternalDataFormat($8051);
    public static property RGB8I:     InternalDataFormat read new InternalDataFormat($8D8F);
    public static property RGB8UI:    InternalDataFormat read new InternalDataFormat($8D7D);
    public static property RGB16:     InternalDataFormat read new InternalDataFormat($8054);
    public static property RGB16I:    InternalDataFormat read new InternalDataFormat($8D89);
    public static property RGB16UI:   InternalDataFormat read new InternalDataFormat($8D77);
    public static property RGB16F:    InternalDataFormat read new InternalDataFormat($881B);
    public static property RGB32I:    InternalDataFormat read new InternalDataFormat($8D83);
    public static property RGB32UI:   InternalDataFormat read new InternalDataFormat($8D71);
    public static property RGB32F:    InternalDataFormat read new InternalDataFormat($8815);
    
    public static property RGBA8:     InternalDataFormat read new InternalDataFormat($8058);
    public static property RGBA16:    InternalDataFormat read new InternalDataFormat($805B);
    public static property RGBA16F:   InternalDataFormat read new InternalDataFormat($881A);
    public static property RGBA32F:   InternalDataFormat read new InternalDataFormat($8814);
    public static property RGBA8I:    InternalDataFormat read new InternalDataFormat($8D8E);
    public static property RGBA16I:   InternalDataFormat read new InternalDataFormat($8D88);
    public static property RGBA32I:   InternalDataFormat read new InternalDataFormat($8D82);
    public static property RGBA8UI:   InternalDataFormat read new InternalDataFormat($8D7C);
    public static property RGBA16UI:  InternalDataFormat read new InternalDataFormat($8D76);
    public static property RGBA32UI:  InternalDataFormat read new InternalDataFormat($8D70);
    
    public static property RGB4:      InternalDataFormat read new InternalDataFormat($804F);
    public static property RGB5:      InternalDataFormat read new InternalDataFormat($8050);
    public static property RGB10:     InternalDataFormat read new InternalDataFormat($8052);
    public static property RGB12:     InternalDataFormat read new InternalDataFormat($8053);
    public static property RGB5_A1:   InternalDataFormat read new InternalDataFormat($8057);
    public static property RGB10_A2:  InternalDataFormat read new InternalDataFormat($8059);
    
    public static property RGBA2:     InternalDataFormat read new InternalDataFormat($8055);
    public static property RGBA4:     InternalDataFormat read new InternalDataFormat($8056);
    public static property RGBA12:    InternalDataFormat read new InternalDataFormat($805A);
    
  end;
  
  //S
  DataFormat = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property RED:             DataFormat read new DataFormat($1903);
    public static property GREEN:           DataFormat read new DataFormat($1904);
    public static property BLUE:            DataFormat read new DataFormat($1905);
    public static property RG:              DataFormat read new DataFormat($8227);
    public static property RGB:             DataFormat read new DataFormat($1907);
    public static property BGR:             DataFormat read new DataFormat($80E0);
    public static property RGBA:            DataFormat read new DataFormat($1908);
    public static property BGRA:            DataFormat read new DataFormat($80E1);
    public static property RED_INTEGER:     DataFormat read new DataFormat($8D94);
    public static property GREEN_INTEGER:   DataFormat read new DataFormat($8D95);
    public static property BLUE_INTEGER:    DataFormat read new DataFormat($8D96);
    public static property RGB_INTEGER:     DataFormat read new DataFormat($8D98);
    public static property RGBA_INTEGER:    DataFormat read new DataFormat($8D99);
    public static property BGR_INTEGER:     DataFormat read new DataFormat($8D9A);
    public static property BGRA_INTEGER:    DataFormat read new DataFormat($8D9B);
    public static property RG_INTEGER:      DataFormat read new DataFormat($8228);
    public static property STENCIL_INDEX:   DataFormat read new DataFormat($1901);
    public static property DEPTH_COMPONENT: DataFormat read new DataFormat($1902);
    public static property DEPTH_STENCIL:   DataFormat read new DataFormat($84F9);
    
  end;
  
  //S
  GLGetQueries = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property ACTIVE_TEXTURE:                            GLGetQueries read new GLGetQueries($84E0);
    public static property ALIASED_LINE_WIDTH_RANGE:                  GLGetQueries read new GLGetQueries($846E);
    public static property ARRAY_BUFFER_BINDING:                      GLGetQueries read new GLGetQueries($8894);
    public static property BLEND_COLOR:                               GLGetQueries read new GLGetQueries($8005);
    public static property BLEND_DST_ALPHA:                           GLGetQueries read new GLGetQueries($80CA);
    public static property BLEND_DST_RGB:                             GLGetQueries read new GLGetQueries($80C8);
    public static property BLEND_EQUATION_RGB:                        GLGetQueries read new GLGetQueries($8009);
    public static property BLEND_EQUATION_ALPHA:                      GLGetQueries read new GLGetQueries($883D);
    public static property BLEND_SRC_ALPHA:                           GLGetQueries read new GLGetQueries($80CB);
    public static property BLEND_SRC_RGB:                             GLGetQueries read new GLGetQueries($80C9);
    public static property COLOR_CLEAR_VALUE:                         GLGetQueries read new GLGetQueries($0C22);
    public static property COLOR_WRITEMASK:                           GLGetQueries read new GLGetQueries($0C23);
    public static property COMPRESSED_TEXTURE_FORMATS:                GLGetQueries read new GLGetQueries($86A3);
    public static property MAX_COMPUTE_SHADER_STORAGE_BLOCKS:         GLGetQueries read new GLGetQueries($90DB);
    public static property MAX_COMBINED_SHADER_STORAGE_BLOCKS:        GLGetQueries read new GLGetQueries($90DC);
    public static property MAX_COMPUTE_UNIFORM_BLOCKS:                GLGetQueries read new GLGetQueries($91BB);
    public static property MAX_COMPUTE_TEXTURE_IMAGE_UNITS:           GLGetQueries read new GLGetQueries($91BC);
    public static property MAX_COMPUTE_UNIFORM_COMPONENTS:            GLGetQueries read new GLGetQueries($8263);
    public static property MAX_COMPUTE_ATOMIC_COUNTERS:               GLGetQueries read new GLGetQueries($8265);
    public static property MAX_COMPUTE_ATOMIC_COUNTER_BUFFERS:        GLGetQueries read new GLGetQueries($8264);
    public static property MAX_COMBINED_COMPUTE_UNIFORM_COMPONENTS:   GLGetQueries read new GLGetQueries($8266);
    public static property MAX_COMPUTE_WORK_GROUP_INVOCATIONS:        GLGetQueries read new GLGetQueries($90EB);
    public static property MAX_COMPUTE_WORK_GROUP_COUNT:              GLGetQueries read new GLGetQueries($91BE);
    public static property MAX_COMPUTE_WORK_GROUP_SIZE:               GLGetQueries read new GLGetQueries($91BF);
    public static property DISPATCH_INDIRECT_BUFFER_BINDING:          GLGetQueries read new GLGetQueries($90EF);
    public static property MAX_DEBUG_GROUP_STACK_DEPTH:               GLGetQueries read new GLGetQueries($826C);
    public static property DEBUG_GROUP_STACK_DEPTH:                   GLGetQueries read new GLGetQueries($826D);
    public static property CONTEXT_FLAGS:                             GLGetQueries read new GLGetQueries($821E);
    public static property CULL_FACE_MODE:                            GLGetQueries read new GLGetQueries($0B45);
    public static property CURRENT_PROGRAM:                           GLGetQueries read new GLGetQueries($8B8D);
    public static property DEPTH_CLEAR_VALUE:                         GLGetQueries read new GLGetQueries($0B73);
    public static property DEPTH_FUNC:                                GLGetQueries read new GLGetQueries($0B74);
    public static property DEPTH_RANGE:                               GLGetQueries read new GLGetQueries($0B70);
    public static property DEPTH_WRITEMASK:                           GLGetQueries read new GLGetQueries($0B72);
    public static property DOUBLEBUFFER:                              GLGetQueries read new GLGetQueries($0C32);
    public static property DRAW_BUFFER:                               GLGetQueries read new GLGetQueries($0C01);
    public static property DRAW_FRAMEBUFFER_BINDING:                  GLGetQueries read new GLGetQueries($8CA6);
    public static property READ_FRAMEBUFFER_BINDING:                  GLGetQueries read new GLGetQueries($8CAA);
    public static property ELEMENT_ARRAY_BUFFER_BINDING:              GLGetQueries read new GLGetQueries($8895);
    public static property FRAGMENT_SHADER_DERIVATIVE_HINT:           GLGetQueries read new GLGetQueries($8B8B);
    public static property IMPLEMENTATION_COLOR_READ_FORMAT:          GLGetQueries read new GLGetQueries($8B9B);
    public static property IMPLEMENTATION_COLOR_READ_TYPE:            GLGetQueries read new GLGetQueries($8B9A);
    public static property LINE_SMOOTH_HINT:                          GLGetQueries read new GLGetQueries($0C52);
    public static property LINE_WIDTH:                                GLGetQueries read new GLGetQueries($0B21);
    public static property LAYER_PROVOKING_VERTEX:                    GLGetQueries read new GLGetQueries($825E);
    public static property LOGIC_OP_MODE:                             GLGetQueries read new GLGetQueries($0BF0);
    public static property MAJOR_VERSION:                             GLGetQueries read new GLGetQueries($821B);
    public static property MAX_3D_TEXTURE_SIZE:                       GLGetQueries read new GLGetQueries($8073);
    public static property MAX_ARRAY_TEXTURE_LAYERS:                  GLGetQueries read new GLGetQueries($88FF);
    public static property MAX_CLIP_DISTANCES:                        GLGetQueries read new GLGetQueries($0D32);
    public static property MAX_COLOR_TEXTURE_SAMPLES:                 GLGetQueries read new GLGetQueries($910E);
    public static property MAX_COMBINED_ATOMIC_COUNTERS:              GLGetQueries read new GLGetQueries($92D7);
    public static property MAX_COMBINED_FRAGMENT_UNIFORM_COMPONENTS:  GLGetQueries read new GLGetQueries($8A33);
    public static property MAX_COMBINED_GEOMETRY_UNIFORM_COMPONENTS:  GLGetQueries read new GLGetQueries($8A32);
    public static property MAX_COMBINED_TEXTURE_IMAGE_UNITS:          GLGetQueries read new GLGetQueries($8B4D);
    public static property MAX_COMBINED_UNIFORM_BLOCKS:               GLGetQueries read new GLGetQueries($8A2E);
    public static property MAX_COMBINED_VERTEX_UNIFORM_COMPONENTS:    GLGetQueries read new GLGetQueries($8A31);
    public static property MAX_CUBE_MAP_TEXTURE_SIZE:                 GLGetQueries read new GLGetQueries($851C);
    public static property MAX_DEPTH_TEXTURE_SAMPLES:                 GLGetQueries read new GLGetQueries($910F);
    public static property MAX_DRAW_BUFFERS:                          GLGetQueries read new GLGetQueries($8824);
    public static property MAX_DUAL_SOURCE_DRAW_BUFFERS:              GLGetQueries read new GLGetQueries($88FC);
    public static property MAX_ELEMENTS_INDICES:                      GLGetQueries read new GLGetQueries($80E9);
    public static property MAX_ELEMENTS_VERTICES:                     GLGetQueries read new GLGetQueries($80E8);
    public static property MAX_FRAGMENT_ATOMIC_COUNTERS:              GLGetQueries read new GLGetQueries($92D6);
    public static property MAX_FRAGMENT_SHADER_STORAGE_BLOCKS:        GLGetQueries read new GLGetQueries($90DA);
    public static property MAX_FRAGMENT_INPUT_COMPONENTS:             GLGetQueries read new GLGetQueries($9125);
    public static property MAX_FRAGMENT_UNIFORM_COMPONENTS:           GLGetQueries read new GLGetQueries($8B49);
    public static property MAX_FRAGMENT_UNIFORM_VECTORS:              GLGetQueries read new GLGetQueries($8DFD);
    public static property MAX_FRAGMENT_UNIFORM_BLOCKS:               GLGetQueries read new GLGetQueries($8A2D);
    public static property MAX_FRAMEBUFFER_WIDTH:                     GLGetQueries read new GLGetQueries($9315);
    public static property MAX_FRAMEBUFFER_HEIGHT:                    GLGetQueries read new GLGetQueries($9316);
    public static property MAX_FRAMEBUFFER_LAYERS:                    GLGetQueries read new GLGetQueries($9317);
    public static property MAX_FRAMEBUFFER_SAMPLES:                   GLGetQueries read new GLGetQueries($9318);
    public static property MAX_GEOMETRY_ATOMIC_COUNTERS:              GLGetQueries read new GLGetQueries($92D5);
    public static property MAX_GEOMETRY_SHADER_STORAGE_BLOCKS:        GLGetQueries read new GLGetQueries($90D7);
    public static property MAX_GEOMETRY_INPUT_COMPONENTS:             GLGetQueries read new GLGetQueries($9123);
    public static property MAX_GEOMETRY_OUTPUT_COMPONENTS:            GLGetQueries read new GLGetQueries($9124);
    public static property MAX_GEOMETRY_TEXTURE_IMAGE_UNITS:          GLGetQueries read new GLGetQueries($8C29);
    public static property MAX_GEOMETRY_UNIFORM_BLOCKS:               GLGetQueries read new GLGetQueries($8A2C);
    public static property MAX_GEOMETRY_UNIFORM_COMPONENTS:           GLGetQueries read new GLGetQueries($8DDF);
    public static property MAX_INTEGER_SAMPLES:                       GLGetQueries read new GLGetQueries($9110);
    public static property MIN_MAP_BUFFER_ALIGNMENT:                  GLGetQueries read new GLGetQueries($90BC);
    public static property MAX_LABEL_LENGTH:                          GLGetQueries read new GLGetQueries($82E8);
    public static property MAX_PROGRAM_TEXEL_OFFSET:                  GLGetQueries read new GLGetQueries($8905);
    public static property MIN_PROGRAM_TEXEL_OFFSET:                  GLGetQueries read new GLGetQueries($8904);
    public static property MAX_RECTANGLE_TEXTURE_SIZE:                GLGetQueries read new GLGetQueries($84F8);
    public static property MAX_RENDERBUFFER_SIZE:                     GLGetQueries read new GLGetQueries($84E8);
    public static property MAX_SAMPLE_MASK_WORDS:                     GLGetQueries read new GLGetQueries($8E59);
    public static property MAX_SERVER_WAIT_TIMEOUT:                   GLGetQueries read new GLGetQueries($9111);
    public static property MAX_SHADER_STORAGE_BUFFER_BINDINGS:        GLGetQueries read new GLGetQueries($90DD);
    public static property MAX_TESS_CONTROL_ATOMIC_COUNTERS:          GLGetQueries read new GLGetQueries($92D3);
    public static property MAX_TESS_EVALUATION_ATOMIC_COUNTERS:       GLGetQueries read new GLGetQueries($92D4);
    public static property MAX_TESS_CONTROL_SHADER_STORAGE_BLOCKS:    GLGetQueries read new GLGetQueries($90D8);
    public static property MAX_TESS_EVALUATION_SHADER_STORAGE_BLOCKS: GLGetQueries read new GLGetQueries($90D9);
    public static property MAX_TEXTURE_BUFFER_SIZE:                   GLGetQueries read new GLGetQueries($8C2B);
    public static property MAX_TEXTURE_IMAGE_UNITS:                   GLGetQueries read new GLGetQueries($8872);
    public static property MAX_TEXTURE_LOD_BIAS:                      GLGetQueries read new GLGetQueries($84FD);
    public static property MAX_TEXTURE_SIZE:                          GLGetQueries read new GLGetQueries($0D33);
    public static property MAX_UNIFORM_BUFFER_BINDINGS:               GLGetQueries read new GLGetQueries($8A2F);
    public static property MAX_UNIFORM_BLOCK_SIZE:                    GLGetQueries read new GLGetQueries($8A30);
    public static property MAX_UNIFORM_LOCATIONS:                     GLGetQueries read new GLGetQueries($826E);
    public static property MAX_VARYING_COMPONENTS:                    GLGetQueries read new GLGetQueries($8B4B);
    public static property MAX_VARYING_VECTORS:                       GLGetQueries read new GLGetQueries($8DFC);
    public static property MAX_VARYING_FLOATS:                        GLGetQueries read new GLGetQueries($8B4B);
    public static property MAX_VERTEX_ATOMIC_COUNTERS:                GLGetQueries read new GLGetQueries($92D2);
    public static property MAX_VERTEX_ATTRIBS:                        GLGetQueries read new GLGetQueries($8869);
    public static property MAX_VERTEX_SHADER_STORAGE_BLOCKS:          GLGetQueries read new GLGetQueries($90D6);
    public static property MAX_VERTEX_TEXTURE_IMAGE_UNITS:            GLGetQueries read new GLGetQueries($8B4C);
    public static property MAX_VERTEX_UNIFORM_COMPONENTS:             GLGetQueries read new GLGetQueries($8B4A);
    public static property MAX_VERTEX_UNIFORM_VECTORS:                GLGetQueries read new GLGetQueries($8DFB);
    public static property MAX_VERTEX_OUTPUT_COMPONENTS:              GLGetQueries read new GLGetQueries($9122);
    public static property MAX_VERTEX_UNIFORM_BLOCKS:                 GLGetQueries read new GLGetQueries($8A2B);
    public static property MAX_VIEWPORT_DIMS:                         GLGetQueries read new GLGetQueries($0D3A);
    public static property MAX_VIEWPORTS:                             GLGetQueries read new GLGetQueries($825B);
    public static property MINOR_VERSION:                             GLGetQueries read new GLGetQueries($821C);
    public static property NUM_COMPRESSED_TEXTURE_FORMATS:            GLGetQueries read new GLGetQueries($86A2);
    public static property NUM_EXTENSIONS:                            GLGetQueries read new GLGetQueries($821D);
    public static property NUM_PROGRAM_BINARY_FORMATS:                GLGetQueries read new GLGetQueries($87FE);
    public static property NUM_SHADER_BINARY_FORMATS:                 GLGetQueries read new GLGetQueries($8DF9);
    public static property PACK_ALIGNMENT:                            GLGetQueries read new GLGetQueries($0D05);
    public static property PACK_IMAGE_HEIGHT:                         GLGetQueries read new GLGetQueries($806C);
    public static property PACK_LSB_FIRST:                            GLGetQueries read new GLGetQueries($0D01);
    public static property PACK_ROW_LENGTH:                           GLGetQueries read new GLGetQueries($0D02);
    public static property PACK_SKIP_IMAGES:                          GLGetQueries read new GLGetQueries($806B);
    public static property PACK_SKIP_PIXELS:                          GLGetQueries read new GLGetQueries($0D04);
    public static property PACK_SKIP_ROWS:                            GLGetQueries read new GLGetQueries($0D03);
    public static property PACK_SWAP_BYTES:                           GLGetQueries read new GLGetQueries($0D00);
    public static property PIXEL_PACK_BUFFER_BINDING:                 GLGetQueries read new GLGetQueries($88ED);
    public static property PIXEL_UNPACK_BUFFER_BINDING:               GLGetQueries read new GLGetQueries($88EF);
    public static property POINT_FADE_THRESHOLD_SIZE:                 GLGetQueries read new GLGetQueries($8128);
    public static property PRIMITIVE_RESTART_INDEX:                   GLGetQueries read new GLGetQueries($8F9E);
    public static property PROGRAM_BINARY_FORMATS:                    GLGetQueries read new GLGetQueries($87FF);
    public static property PROGRAM_PIPELINE_BINDING:                  GLGetQueries read new GLGetQueries($825A);
    public static property PROGRAM_POINT_SIZE:                        GLGetQueries read new GLGetQueries($8642);
    public static property PROVOKING_VERTEX:                          GLGetQueries read new GLGetQueries($8E4F);
    public static property POINT_SIZE:                                GLGetQueries read new GLGetQueries($0B11);
    public static property POINT_SIZE_GRANULARITY:                    GLGetQueries read new GLGetQueries($0B13);
    public static property POINT_SIZE_RANGE:                          GLGetQueries read new GLGetQueries($0B12);
    public static property POLYGON_OFFSET_FACTOR:                     GLGetQueries read new GLGetQueries($8038);
    public static property POLYGON_OFFSET_UNITS:                      GLGetQueries read new GLGetQueries($2A00);
    public static property POLYGON_SMOOTH_HINT:                       GLGetQueries read new GLGetQueries($0C53);
    public static property READ_BUFFER:                               GLGetQueries read new GLGetQueries($0C02);
    public static property RENDERBUFFER_BINDING:                      GLGetQueries read new GLGetQueries($8CA7);
    public static property SAMPLE_BUFFERS:                            GLGetQueries read new GLGetQueries($80A8);
    public static property SAMPLE_COVERAGE_VALUE:                     GLGetQueries read new GLGetQueries($80AA);
    public static property SAMPLE_COVERAGE_INVERT:                    GLGetQueries read new GLGetQueries($80AB);
    public static property SAMPLER_BINDING:                           GLGetQueries read new GLGetQueries($8919);
    public static property SAMPLES:                                   GLGetQueries read new GLGetQueries($80A9);
    public static property SCISSOR_BOX:                               GLGetQueries read new GLGetQueries($0C10);
    public static property SHADER_COMPILER:                           GLGetQueries read new GLGetQueries($8DFA);
    public static property SHADER_STORAGE_BUFFER_BINDING:             GLGetQueries read new GLGetQueries($90D3);
    public static property SHADER_STORAGE_BUFFER_OFFSET_ALIGNMENT:    GLGetQueries read new GLGetQueries($90DF);
    public static property SHADER_STORAGE_BUFFER_START:               GLGetQueries read new GLGetQueries($90D4);
    public static property SHADER_STORAGE_BUFFER_SIZE:                GLGetQueries read new GLGetQueries($90D5);
    public static property SMOOTH_LINE_WIDTH_RANGE:                   GLGetQueries read new GLGetQueries($0B22);
    public static property SMOOTH_LINE_WIDTH_GRANULARITY:             GLGetQueries read new GLGetQueries($0B23);
    public static property STENCIL_BACK_FAIL:                         GLGetQueries read new GLGetQueries($8801);
    public static property STENCIL_BACK_FUNC:                         GLGetQueries read new GLGetQueries($8800);
    public static property STENCIL_BACK_PASS_DEPTH_FAIL:              GLGetQueries read new GLGetQueries($8802);
    public static property STENCIL_BACK_PASS_DEPTH_PASS:              GLGetQueries read new GLGetQueries($8803);
    public static property STENCIL_BACK_REF:                          GLGetQueries read new GLGetQueries($8CA3);
    public static property STENCIL_BACK_VALUE_MASK:                   GLGetQueries read new GLGetQueries($8CA4);
    public static property STENCIL_BACK_WRITEMASK:                    GLGetQueries read new GLGetQueries($8CA5);
    public static property STENCIL_CLEAR_VALUE:                       GLGetQueries read new GLGetQueries($0B91);
    public static property STENCIL_FAIL:                              GLGetQueries read new GLGetQueries($0B94);
    public static property STENCIL_FUNC:                              GLGetQueries read new GLGetQueries($0B92);
    public static property STENCIL_PASS_DEPTH_FAIL:                   GLGetQueries read new GLGetQueries($0B95);
    public static property STENCIL_PASS_DEPTH_PASS:                   GLGetQueries read new GLGetQueries($0B96);
    public static property STENCIL_REF:                               GLGetQueries read new GLGetQueries($0B97);
    public static property STENCIL_VALUE_MASK:                        GLGetQueries read new GLGetQueries($0B93);
    public static property STENCIL_WRITEMASK:                         GLGetQueries read new GLGetQueries($0B98);
    public static property STEREO:                                    GLGetQueries read new GLGetQueries($0C33);
    public static property SUBPIXEL_BITS:                             GLGetQueries read new GLGetQueries($0D50);
    public static property TEXTURE_BINDING_1D:                        GLGetQueries read new GLGetQueries($8068);
    public static property TEXTURE_BINDING_1D_ARRAY:                  GLGetQueries read new GLGetQueries($8C1C);
    public static property TEXTURE_BINDING_2D:                        GLGetQueries read new GLGetQueries($8069);
    public static property TEXTURE_BINDING_2D_ARRAY:                  GLGetQueries read new GLGetQueries($8C1D);
    public static property TEXTURE_BINDING_2D_MULTISAMPLE:            GLGetQueries read new GLGetQueries($9104);
    public static property TEXTURE_BINDING_2D_MULTISAMPLE_ARRAY:      GLGetQueries read new GLGetQueries($9105);
    public static property TEXTURE_BINDING_3D:                        GLGetQueries read new GLGetQueries($806A);
    public static property TEXTURE_BINDING_BUFFER:                    GLGetQueries read new GLGetQueries($8C2C);
    public static property TEXTURE_BINDING_CUBE_MAP:                  GLGetQueries read new GLGetQueries($8514);
    public static property TEXTURE_BINDING_RECTANGLE:                 GLGetQueries read new GLGetQueries($84F6);
    public static property TEXTURE_COMPRESSION_HINT:                  GLGetQueries read new GLGetQueries($84EF);
    public static property TEXTURE_BUFFER_OFFSET_ALIGNMENT:           GLGetQueries read new GLGetQueries($919F);
    public static property TIMESTAMP:                                 GLGetQueries read new GLGetQueries($8E28);
    public static property TRANSFORM_FEEDBACK_BUFFER_BINDING:         GLGetQueries read new GLGetQueries($8C8F);
    public static property TRANSFORM_FEEDBACK_BUFFER_START:           GLGetQueries read new GLGetQueries($8C84);
    public static property TRANSFORM_FEEDBACK_BUFFER_SIZE:            GLGetQueries read new GLGetQueries($8C85);
    public static property UNIFORM_BUFFER_BINDING:                    GLGetQueries read new GLGetQueries($8A28);
    public static property UNIFORM_BUFFER_OFFSET_ALIGNMENT:           GLGetQueries read new GLGetQueries($8A34);
    public static property UNIFORM_BUFFER_SIZE:                       GLGetQueries read new GLGetQueries($8A2A);
    public static property UNIFORM_BUFFER_START:                      GLGetQueries read new GLGetQueries($8A29);
    public static property UNPACK_ALIGNMENT:                          GLGetQueries read new GLGetQueries($0CF5);
    public static property UNPACK_IMAGE_HEIGHT:                       GLGetQueries read new GLGetQueries($806E);
    public static property UNPACK_LSB_FIRST:                          GLGetQueries read new GLGetQueries($0CF1);
    public static property UNPACK_ROW_LENGTH:                         GLGetQueries read new GLGetQueries($0CF2);
    public static property UNPACK_SKIP_IMAGES:                        GLGetQueries read new GLGetQueries($806D);
    public static property UNPACK_SKIP_PIXELS:                        GLGetQueries read new GLGetQueries($0CF4);
    public static property UNPACK_SKIP_ROWS:                          GLGetQueries read new GLGetQueries($0CF3);
    public static property UNPACK_SWAP_BYTES:                         GLGetQueries read new GLGetQueries($0CF0);
    public static property VERTEX_ARRAY_BINDING:                      GLGetQueries read new GLGetQueries($85B5);
    public static property VERTEX_BINDING_DIVISOR:                    GLGetQueries read new GLGetQueries($82D6);
    public static property VERTEX_BINDING_OFFSET:                     GLGetQueries read new GLGetQueries($82D7);
    public static property VERTEX_BINDING_STRIDE:                     GLGetQueries read new GLGetQueries($82D8);
    public static property MAX_VERTEX_ATTRIB_RELATIVE_OFFSET:         GLGetQueries read new GLGetQueries($82D9);
    public static property MAX_VERTEX_ATTRIB_BINDINGS:                GLGetQueries read new GLGetQueries($82DA);
    public static property VIEWPORT:                                  GLGetQueries read new GLGetQueries($0BA2);
    public static property VIEWPORT_BOUNDS_RANGE:                     GLGetQueries read new GLGetQueries($825D);
    public static property VIEWPORT_INDEX_PROVOKING_VERTEX:           GLGetQueries read new GLGetQueries($825F);
    public static property VIEWPORT_SUBPIXEL_BITS:                    GLGetQueries read new GLGetQueries($825C);
    public static property MAX_ELEMENT_INDEX:                         GLGetQueries read new GLGetQueries($8D6B);
    
  end;
  
  //S
  EnablableName = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property BLEND:                         EnablableName read new EnablableName($0BE2);
    public static property CLIP_DISTANCE0:                EnablableName read new EnablableName($3000);
    public static property CLIP_DISTANCE1:                EnablableName read new EnablableName($3001);
    public static property CLIP_DISTANCE2:                EnablableName read new EnablableName($3002);
    public static property CLIP_DISTANCE3:                EnablableName read new EnablableName($3003);
    public static property CLIP_DISTANCE4:                EnablableName read new EnablableName($3004);
    public static property CLIP_DISTANCE5:                EnablableName read new EnablableName($3005);
    public static property CLIP_DISTANCE6:                EnablableName read new EnablableName($3006);
    public static property CLIP_DISTANCE7:                EnablableName read new EnablableName($3007);
    public static property COLOR_LOGIC_OP:                EnablableName read new EnablableName($0BF2);
    public static property CULL_FACE:                     EnablableName read new EnablableName($0B44);
    public static property DEBUG_OUTPUT:                  EnablableName read new EnablableName($92E0);
    public static property DEBUG_OUTPUT_SYNCHRONOUS:      EnablableName read new EnablableName($8242);
    public static property DEPTH_CLAMP:                   EnablableName read new EnablableName($864F);
    public static property DEPTH_TEST:                    EnablableName read new EnablableName($0B71);
    public static property DITHER:                        EnablableName read new EnablableName($0BD0);
    public static property FRAMEBUFFER_SRGB:              EnablableName read new EnablableName($8DB9);
    public static property LINE_SMOOTH:                   EnablableName read new EnablableName($0B20);
    public static property MULTISAMPLE:                   EnablableName read new EnablableName($809D);
    public static property POLYGON_OFFSET_FILL:           EnablableName read new EnablableName($8037);
    public static property POLYGON_OFFSET_LINE:           EnablableName read new EnablableName($2A02);
    public static property POLYGON_OFFSET_POINT:          EnablableName read new EnablableName($2A01);
    public static property POLYGON_SMOOTH:                EnablableName read new EnablableName($0B41);
    public static property PRIMITIVE_RESTART:             EnablableName read new EnablableName($8F9D);
    public static property PRIMITIVE_RESTART_FIXED_INDEX: EnablableName read new EnablableName($8D69);
    public static property RASTERIZER_DISCARD:            EnablableName read new EnablableName($8C89);
    public static property SAMPLE_ALPHA_TO_COVERAGE:      EnablableName read new EnablableName($809E);
    public static property SAMPLE_ALPHA_TO_ONE:           EnablableName read new EnablableName($809F);
    public static property SAMPLE_COVERAGE:               EnablableName read new EnablableName($80A0);
    public static property SAMPLE_SHADING:                EnablableName read new EnablableName($8C36);
    public static property SAMPLE_MASK:                   EnablableName read new EnablableName($8E51);
    public static property SCISSOR_TEST:                  EnablableName read new EnablableName($0C11);
    public static property STENCIL_TEST:                  EnablableName read new EnablableName($0B90);
    public static property TEXTURE_CUBE_MAP_SEAMLESS:     EnablableName read new EnablableName($884F);
    public static property PROGRAM_POINT_SIZE:            EnablableName read new EnablableName($8642);
    
    public static function operator implicit(v: EnablableName): GLGetQueries := new GLGetQueries(v.val);
    
  end;
  
  //S
  GLGetStringQueries = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property VENDOR:                    GLGetStringQueries read new GLGetStringQueries($1F00);
    public static property RENDERER:                  GLGetStringQueries read new GLGetStringQueries($1F01);
    public static property VERSION:                   GLGetStringQueries read new GLGetStringQueries($1F02);
    public static property EXTENSIONS:                GLGetStringQueries read new GLGetStringQueries($1F03);
    public static property SHADING_LANGUAGE_VERSION:  GLGetStringQueries read new GLGetStringQueries($8B8C);
    
  end;
  
  //SR
  ColorEncodingMode = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property LINEAR:  ColorEncodingMode read new ColorEncodingMode($2601);
    public static property SRGB:    ColorEncodingMode read new ColorEncodingMode($8C40);
    
    public property IS_LINEAR:  boolean read self = ColorEncodingMode.LINEAR;
    public property IS_SRGB:    boolean read self = ColorEncodingMode.SRGB;
    
    public function ToString: string; override;
    begin
      var res := typeof(ColorEncodingMode).GetProperties.Where(prop->prop.PropertyType=typeof(boolean)).Select(prop->(prop.Name,boolean(prop.GetValue(self)))).FirstOrDefault(t->t[1]);
      Result := res=nil?
        $'ColorEncodingMode[{self.val}]':
        res[0].Substring(3);
    end;
    
  end;
  
  //SR
  DataType = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property BYTE:                        DataType read new DataType($1400);
    public static property UNSIGNED_BYTE:               DataType read new DataType($1401);
    public static property SHORT:                       DataType read new DataType($1402);
    public static property UNSIGNED_SHORT:              DataType read new DataType($1403);
    public static property INT:                         DataType read new DataType($1404);
    public static property UNSIGNED_INT:                DataType read new DataType($1405);
    public static property FLOAT:                       DataType read new DataType($1406);
    public static property HALF_FLOAT:                  DataType read new DataType($140B);
    public static property UNSIGNED_BYTE_3_3_2:         DataType read new DataType($8032);
    public static property UNSIGNED_SHORT_5_6_5:        DataType read new DataType($8363);
    public static property UNSIGNED_SHORT_4_4_4_4:      DataType read new DataType($8033);
    public static property UNSIGNED_SHORT_5_5_5_1:      DataType read new DataType($8034);
    public static property UNSIGNED_INT_8_8_8_8:        DataType read new DataType($8035);
    public static property UNSIGNED_INT_10_10_10_2:     DataType read new DataType($8036);
    public static property UNSIGNED_BYTE_2_3_3_REV:     DataType read new DataType($8362);
    public static property UNSIGNED_SHORT_5_6_5_REV:    DataType read new DataType($8364);
    public static property UNSIGNED_SHORT_4_4_4_4_REV:  DataType read new DataType($8365);
    public static property UNSIGNED_SHORT_1_5_5_5_REV:  DataType read new DataType($8366);
    public static property UNSIGNED_INT_8_8_8_8_REV:    DataType read new DataType($8367);
    public static property UNSIGNED_INT_2_10_10_10_REV: DataType read new DataType($8368);
    public static property SIGNED_NORMALIZED:           DataType read new DataType($8F9C);
    public static property UNSIGNED_NORMALIZED:         DataType read new DataType($8C17);
    
    public property IS_NONE:                        boolean read self.val = 0;
    public property IS_BYTE:                        boolean read self.val = BYTE.val;
    public property IS_UNSIGNED_BYTE:               boolean read self.val = UNSIGNED_BYTE.val;
    public property IS_SHORT:                       boolean read self.val = SHORT.val;
    public property IS_UNSIGNED_SHORT:              boolean read self.val = UNSIGNED_SHORT.val;
    public property IS_INT:                         boolean read self.val = INT.val;
    public property IS_UNSIGNED_INT:                boolean read self.val = UNSIGNED_INT.val;
    public property IS_FLOAT:                       boolean read self.val = FLOAT.val;
    public property IS_HALF_FLOAT:                  boolean read self.val = HALF_FLOAT.val;
    public property IS_UNSIGNED_BYTE_3_3_2:         boolean read self.val = UNSIGNED_BYTE_3_3_2.val;
    public property IS_UNSIGNED_SHORT_5_6_5:        boolean read self.val = UNSIGNED_SHORT_5_6_5.val;
    public property IS_UNSIGNED_SHORT_4_4_4_4:      boolean read self.val = UNSIGNED_SHORT_4_4_4_4.val;
    public property IS_UNSIGNED_SHORT_5_5_5_1:      boolean read self.val = UNSIGNED_SHORT_5_5_5_1.val;
    public property IS_UNSIGNED_INT_8_8_8_8:        boolean read self.val = UNSIGNED_INT_8_8_8_8.val;
    public property IS_UNSIGNED_INT_10_10_10_2:     boolean read self.val = UNSIGNED_INT_10_10_10_2.val;
    public property IS_UNSIGNED_BYTE_2_3_3_REV:     boolean read self.val = UNSIGNED_BYTE_2_3_3_REV.val;
    public property IS_UNSIGNED_SHORT_5_6_5_REV:    boolean read self.val = UNSIGNED_SHORT_5_6_5_REV.val;
    public property IS_UNSIGNED_SHORT_4_4_4_4_REV:  boolean read self.val = UNSIGNED_SHORT_4_4_4_4_REV.val;
    public property IS_UNSIGNED_SHORT_1_5_5_5_REV:  boolean read self.val = UNSIGNED_SHORT_1_5_5_5_REV.val;
    public property IS_UNSIGNED_INT_8_8_8_8_REV:    boolean read self.val = UNSIGNED_INT_8_8_8_8_REV.val;
    public property IS_UNSIGNED_INT_2_10_10_10_REV: boolean read self.val = UNSIGNED_INT_2_10_10_10_REV.val;
    public property IS_SIGNED_NORMALIZED:           boolean read self.val = SIGNED_NORMALIZED.val;
    public property IS_UNSIGNED_NORMALIZED:         boolean read self.val = UNSIGNED_NORMALIZED.val;
    
    public function ToString: string; override;
    begin
      var res := typeof(DataType).GetProperties.Where(prop->prop.PropertyType=typeof(boolean)).Select(prop->(prop.Name,boolean(prop.GetValue(self)))).FirstOrDefault(t->t[1]);
      Result := res=nil?
        $'DataType[{self.val}]':
        res[0].Substring(3);
    end;
    
  end;
  
  //R
  ProgramVarType = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public property FLOAT:             boolean read self.val = $1406;
    public property FLOAT_VEC2:        boolean read self.val = $8B50;
    public property FLOAT_VEC3:        boolean read self.val = $8B51;
    public property FLOAT_VEC4:        boolean read self.val = $8B52;
    public property FLOAT_MAT2:        boolean read self.val = $8B5A;
    public property FLOAT_MAT3:        boolean read self.val = $8B5B;
    public property FLOAT_MAT4:        boolean read self.val = $8B5C;
    public property FLOAT_MAT2x3:      boolean read self.val = $8B65;
    public property FLOAT_MAT3x2:      boolean read self.val = $8B67;
    public property FLOAT_MAT2x4:      boolean read self.val = $8B66;
    public property FLOAT_MAT4x2:      boolean read self.val = $8B69;
    public property FLOAT_MAT3x4:      boolean read self.val = $8B68;
    public property FLOAT_MAT4x3:      boolean read self.val = $8B6A;
    public property INT:               boolean read self.val = $1404;
    public property INT_VEC2:          boolean read self.val = $8B53;
    public property INT_VEC3:          boolean read self.val = $8B54;
    public property INT_VEC4:          boolean read self.val = $8B55;
    public property UNSIGNED_INT:      boolean read self.val = $1405;
    public property UNSIGNED_INT_VEC2: boolean read self.val = $8DC6;
    public property UNSIGNED_INT_VEC3: boolean read self.val = $8DC7;
    public property UNSIGNED_INT_VEC4: boolean read self.val = $8DC8;
    public property DOUBLE:            boolean read self.val = $140A;
    public property DOUBLE_VEC2:       boolean read self.val = $8FFC;
    public property DOUBLE_VEC3:       boolean read self.val = $8FFD;
    public property DOUBLE_VEC4:       boolean read self.val = $8FFE;
    public property DOUBLE_MAT2:       boolean read self.val = $8F46;
    public property DOUBLE_MAT3:       boolean read self.val = $8F47;
    public property DOUBLE_MAT4:       boolean read self.val = $8F48;
    public property DOUBLE_MAT2x3:     boolean read self.val = $8F49;
    public property DOUBLE_MAT3x2:     boolean read self.val = $8F4B;
    public property DOUBLE_MAT2x4:     boolean read self.val = $8F4A;
    public property DOUBLE_MAT4x2:     boolean read self.val = $8F4D;
    public property DOUBLE_MAT3x4:     boolean read self.val = $8F4C;
    public property DOUBLE_MAT4x3:     boolean read self.val = $8F4E;
    
    public function ToString: string; override;
    begin
      var res := typeof(ProgramVarType).GetProperties.Where(prop->prop.PropertyType=typeof(boolean)).Select(prop->(prop.Name,boolean(prop.GetValue(self)))).FirstOrDefault(t->t[1]);
      Result := res=nil?
        $'ProgramVarType[{self.val}]':
        res[0];
    end;
    
  end;
  
  //R
  FramebufferAttachmentObjectType = record
    public val: UInt32;
    
    public property NONE:                boolean read self.val = $0000;
    public property FRAMEBUFFER_DEFAULT: boolean read self.val = $8218;
    public property TEXTURE:             boolean read self.val = $1702;
    public property RENDERBUFFER:        boolean read self.val = $8D41;
    
    public function ToString: string; override;
    begin
      var res := typeof(FramebufferAttachmentObjectType).GetProperties.Where(prop->prop.PropertyType=typeof(boolean)).Select(prop->(prop.Name,boolean(prop.GetValue(self)))).FirstOrDefault(t->t[1]);
      Result := res=nil?
        $'FramebufferAttachmentObjectType[{self.val}]':
        res[0];
    end;
    
  end;
  
  //R
  ClientWaitSyncResult = record
    public val: UInt32;
    
    public property ALREADY_SIGNALED:    boolean read self.val = $911A;
    public property TIMEOUT_EXPIRED:     boolean read self.val = $911B;
    public property CONDITION_SATISFIED: boolean read self.val = $911C;
    public property WAIT_FAILED:         boolean read self.val = $911D;
    
    public function ToString: string; override;
    begin
      var res := typeof(ClientWaitSyncResult).GetProperties.Select(prop->(prop.Name,boolean(prop.GetValue(self)))).FirstOrDefault(t->t[1]);
      Result := res=nil?
        $'ClientWaitSyncResult[{self.val}]':
        res[0];
    end;
    
  end;
  
  //R
  ShadingLanguageTypeToken = record
    public val: UInt32;
    
    public property FLOAT:                                     boolean read self.val = $1406;
    public property FLOAT_VEC2:                                boolean read self.val = $8B50;
    public property FLOAT_VEC3:                                boolean read self.val = $8B51;
    public property FLOAT_VEC4:                                boolean read self.val = $8B52;
    public property DOUBLE:                                    boolean read self.val = $140A;
    public property DOUBLE_VEC2:                               boolean read self.val = $8FFC;
    public property DOUBLE_VEC3:                               boolean read self.val = $8FFD;
    public property DOUBLE_VEC4:                               boolean read self.val = $8FFE;
    public property INT:                                       boolean read self.val = $1404;
    public property INT_VEC2:                                  boolean read self.val = $8B53;
    public property INT_VEC3:                                  boolean read self.val = $8B54;
    public property INT_VEC4:                                  boolean read self.val = $8B55;
    public property UNSIGNED_INT:                              boolean read self.val = $1405;
    public property UNSIGNED_INT_VEC2:                         boolean read self.val = $8DC6;
    public property UNSIGNED_INT_VEC3:                         boolean read self.val = $8DC7;
    public property UNSIGNED_INT_VEC4:                         boolean read self.val = $8DC8;
    public property BOOL:                                      boolean read self.val = $8B56;
    public property BOOL_VEC2:                                 boolean read self.val = $8B57;
    public property BOOL_VEC3:                                 boolean read self.val = $8B58;
    public property BOOL_VEC4:                                 boolean read self.val = $8B59;
    public property FLOAT_MAT2:                                boolean read self.val = $8B5A;
    public property FLOAT_MAT3:                                boolean read self.val = $8B5B;
    public property FLOAT_MAT4:                                boolean read self.val = $8B5C;
    public property FLOAT_MAT2x3:                              boolean read self.val = $8B65;
    public property FLOAT_MAT2x4:                              boolean read self.val = $8B66;
    public property FLOAT_MAT3x2:                              boolean read self.val = $8B67;
    public property FLOAT_MAT3x4:                              boolean read self.val = $8B68;
    public property FLOAT_MAT4x2:                              boolean read self.val = $8B69;
    public property FLOAT_MAT4x3:                              boolean read self.val = $8B6A;
    public property DOUBLE_MAT2:                               boolean read self.val = $8F46;
    public property DOUBLE_MAT3:                               boolean read self.val = $8F47;
    public property DOUBLE_MAT4:                               boolean read self.val = $8F48;
    public property DOUBLE_MAT2x3:                             boolean read self.val = $8F49;
    public property DOUBLE_MAT2x4:                             boolean read self.val = $8F4A;
    public property DOUBLE_MAT3x2:                             boolean read self.val = $8F4B;
    public property DOUBLE_MAT3x4:                             boolean read self.val = $8F4C;
    public property DOUBLE_MAT4x2:                             boolean read self.val = $8F4D;
    public property DOUBLE_MAT4x3:                             boolean read self.val = $8F4E;
    public property SAMPLER_1D:                                boolean read self.val = $8B5D;
    public property SAMPLER_2D:                                boolean read self.val = $8B5E;
    public property SAMPLER_3D:                                boolean read self.val = $8B5F;
    public property SAMPLER_CUBE:                              boolean read self.val = $8B60;
    public property SAMPLER_1D_SHADOW:                         boolean read self.val = $8B61;
    public property SAMPLER_2D_SHADOW:                         boolean read self.val = $8B62;
    public property SAMPLER_1D_ARRAY:                          boolean read self.val = $8DC0;
    public property SAMPLER_2D_ARRAY:                          boolean read self.val = $8DC1;
    public property SAMPLER_CUBE_MAP_ARRAY:                    boolean read self.val = $900C;
    public property SAMPLER_1D_ARRAY_SHADOW:                   boolean read self.val = $8DC3;
    public property SAMPLER_2D_ARRAY_SHADOW:                   boolean read self.val = $8DC4;
    public property SAMPLER_2D_MULTISAMPLE:                    boolean read self.val = $9108;
    public property SAMPLER_2D_MULTISAMPLE_ARRAY:              boolean read self.val = $910B;
    public property SAMPLER_CUBE_SHADOW:                       boolean read self.val = $8DC5;
    public property SAMPLER_CUBE_MAP_ARRAY_SHADOW:             boolean read self.val = $900D;
    public property SAMPLER_BUFFER:                            boolean read self.val = $8DC2;
    public property SAMPLER_2D_RECT:                           boolean read self.val = $8B63;
    public property SAMPLER_2D_RECT_SHADOW:                    boolean read self.val = $8B64;
    public property INT_SAMPLER_1D:                            boolean read self.val = $8DC9;
    public property INT_SAMPLER_2D:                            boolean read self.val = $8DCA;
    public property INT_SAMPLER_3D:                            boolean read self.val = $8DCB;
    public property INT_SAMPLER_CUBE:                          boolean read self.val = $8DCC;
    public property INT_SAMPLER_1D_ARRAY:                      boolean read self.val = $8DCE;
    public property INT_SAMPLER_2D_ARRAY:                      boolean read self.val = $8DCF;
    public property INT_SAMPLER_CUBE_MAP_ARRAY:                boolean read self.val = $900E;
    public property INT_SAMPLER_2D_MULTISAMPLE:                boolean read self.val = $9109;
    public property INT_SAMPLER_2D_MULTISAMPLE_ARRAY:          boolean read self.val = $910C;
    public property INT_SAMPLER_BUFFER:                        boolean read self.val = $8DD0;
    public property INT_SAMPLER_2D_RECT:                       boolean read self.val = $8DCD;
    public property UNSIGNED_INT_SAMPLER_1D:                   boolean read self.val = $8DD1;
    public property UNSIGNED_INT_SAMPLER_2D:                   boolean read self.val = $8DD2;
    public property UNSIGNED_INT_SAMPLER_3D:                   boolean read self.val = $8DD3;
    public property UNSIGNED_INT_SAMPLER_CUBE:                 boolean read self.val = $8DD4;
    public property UNSIGNED_INT_SAMPLER_1D_ARRAY:             boolean read self.val = $8DD6;
    public property UNSIGNED_INT_SAMPLER_2D_ARRAY:             boolean read self.val = $8DD7;
    public property UNSIGNED_INT_SAMPLER_CUBE_MAP_ARRAY:       boolean read self.val = $900F;
    public property UNSIGNED_INT_SAMPLER_2D_MULTISAMPLE:       boolean read self.val = $910A;
    public property UNSIGNED_INT_SAMPLER_2D_MULTISAMPLE_ARRAY: boolean read self.val = $910D;
    public property UNSIGNED_INT_SAMPLER_BUFFER:               boolean read self.val = $8DD8;
    public property UNSIGNED_INT_SAMPLER_2D_RECT:              boolean read self.val = $8DD5;
    public property IMAGE_1D:                                  boolean read self.val = $904C;
    public property IMAGE_2D:                                  boolean read self.val = $904D;
    public property IMAGE_3D:                                  boolean read self.val = $904E;
    public property IMAGE_2D_RECT:                             boolean read self.val = $904F;
    public property IMAGE_CUBE:                                boolean read self.val = $9050;
    public property IMAGE_BUFFER:                              boolean read self.val = $9051;
    public property IMAGE_1D_ARRAY:                            boolean read self.val = $9052;
    public property IMAGE_2D_ARRAY:                            boolean read self.val = $9053;
    public property IMAGE_CUBE_MAP_ARRAY:                      boolean read self.val = $9054;
    public property IMAGE_2D_MULTISAMPLE:                      boolean read self.val = $9055;
    public property IMAGE_2D_MULTISAMPLE_ARRAY:                boolean read self.val = $9056;
    public property INT_IMAGE_1D:                              boolean read self.val = $9057;
    public property INT_IMAGE_2D:                              boolean read self.val = $9058;
    public property INT_IMAGE_3D:                              boolean read self.val = $9059;
    public property INT_IMAGE_2D_RECT:                         boolean read self.val = $905A;
    public property INT_IMAGE_CUBE:                            boolean read self.val = $905B;
    public property INT_IMAGE_BUFFER:                          boolean read self.val = $905C;
    public property INT_IMAGE_1D_ARRAY:                        boolean read self.val = $905D;
    public property INT_IMAGE_2D_ARRAY:                        boolean read self.val = $905E;
    public property INT_IMAGE_CUBE_MAP_ARRAY:                  boolean read self.val = $905F;
    public property INT_IMAGE_2D_MULTISAMPLE:                  boolean read self.val = $9060;
    public property INT_IMAGE_2D_MULTISAMPLE_ARRAY:            boolean read self.val = $9061;
    public property UNSIGNED_INT_IMAGE_1D:                     boolean read self.val = $9062;
    public property UNSIGNED_INT_IMAGE_2D:                     boolean read self.val = $9063;
    public property UNSIGNED_INT_IMAGE_3D:                     boolean read self.val = $9064;
    public property UNSIGNED_INT_IMAGE_2D_RECT:                boolean read self.val = $9065;
    public property UNSIGNED_INT_IMAGE_CUBE:                   boolean read self.val = $9066;
    public property UNSIGNED_INT_IMAGE_BUFFER:                 boolean read self.val = $9067;
    public property UNSIGNED_INT_IMAGE_1D_ARRAY:               boolean read self.val = $9068;
    public property UNSIGNED_INT_IMAGE_2D_ARRAY:               boolean read self.val = $9069;
    public property UNSIGNED_INT_IMAGE_CUBE_MAP_ARRAY:         boolean read self.val = $906A;
    public property UNSIGNED_INT_IMAGE_2D_MULTISAMPLE:         boolean read self.val = $906B;
    public property UNSIGNED_INT_IMAGE_2D_MULTISAMPLE_ARRAY:   boolean read self.val = $906C;
    public property UNSIGNED_INT_ATOMIC_COUNTER:               boolean read self.val = $92DB;
    
  end;
  
  //R
  DebugSourceType = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public property API:                 boolean read self.val = $8246;
    public property WINDOW_SYSTEM:       boolean read self.val = $8247;
    public property SHADER_COMPILER:     boolean read self.val = $8248;
    public property THIRD_PARTY:         boolean read self.val = $8249;
    public property APPLICATION:         boolean read self.val = $824A;
    public property OTHER:               boolean read self.val = $824B;
    public property API_ARB:             boolean read self.val = $8246;
    public property WINDOW_SYSTEM_ARB:   boolean read self.val = $8247;
    public property SHADER_COMPILER_ARB: boolean read self.val = $8248;
    public property THIRD_PARTY_ARB:     boolean read self.val = $8249;
    public property APPLICATION_ARB:     boolean read self.val = $824A;
    public property OTHER_ARB:           boolean read self.val = $824B;
    
    public function ToString: string; override;
    begin
      var res := typeof(DebugSourceType).GetProperties.Where(prop->prop.PropertyType=typeof(boolean)).Select(prop->(prop.Name,boolean(prop.GetValue(self)))).FirstOrDefault(t->t[1]);
      Result := res=nil?
        $'DebugSourceType[{self.val}]':
        res[0];
    end;
    
  end;
  
  //R
  DebugMessageType = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public property ERROR:               boolean read self.val = $824C;
    public property DEPRECATED_BEHAVIOR: boolean read self.val = $824D;
    public property UNDEFINED_BEHAVIOR:  boolean read self.val = $824E;
    public property PORTABILITY:         boolean read self.val = $824F;
    public property PERFORMANCE:         boolean read self.val = $8250;
    public property OTHER:               boolean read self.val = $8251;
    public property MARKER:              boolean read self.val = $8268;
    public property PUSH_GROUP:          boolean read self.val = $8269;
    public property POP_GROUP:           boolean read self.val = $826A;
    
    public function ToString: string; override;
    begin
      var res := typeof(DebugMessageType).GetProperties.Where(prop->prop.PropertyType=typeof(boolean)).Select(prop->(prop.Name,boolean(prop.GetValue(self)))).FirstOrDefault(t->t[1]);
      Result := res=nil?
        $'DebugMessageType[{self.val}]':
        res[0];
    end;
    
  end;
  
  //R
  DebugSeverityLevel = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public property HIGH:          boolean read self.val = $9146;
    public property MEDIUM:        boolean read self.val = $9147;
    public property LOW:           boolean read self.val = $9148;
    public property NOTIFICATION:  boolean read self.val = $826B;
    
    public function ToString: string; override;
    begin
      var res := typeof(DebugSeverityLevel).GetProperties.Where(prop->prop.PropertyType=typeof(boolean)).Select(prop->(prop.Name,boolean(prop.GetValue(self)))).FirstOrDefault(t->t[1]);
      Result := res=nil?
        $'DebugSeverityLevel[{self.val}]':
        res[0];
    end;
    
  end;
  
  {$endregion 1 значение}
  
  {$region Флаги}
  
  //S
  BufferTypeFlags = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property COLOR_BUFFER_BIT:    BufferTypeFlags read new BufferTypeFlags($00004000);
    public static property DEPTH_BUFFER_BIT:    BufferTypeFlags read new BufferTypeFlags($00000100);
    public static property STENCIL_BUFFER_BIT:  BufferTypeFlags read new BufferTypeFlags($00000400);
    
    public static function operator or(f1,f2: BufferTypeFlags): BufferTypeFlags := new BufferTypeFlags(f1.val or f2.val);
    
  end;
  
  //S
  MemoryBarrierTypeFlags = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property VERTEX_ATTRIB_ARRAY_BARRIER_BIT: MemoryBarrierTypeFlags read new MemoryBarrierTypeFlags($00000001);
    public static property ELEMENT_ARRAY_BARRIER_BIT:       MemoryBarrierTypeFlags read new MemoryBarrierTypeFlags($00000002);
    public static property UNIFORM_BARRIER_BIT:             MemoryBarrierTypeFlags read new MemoryBarrierTypeFlags($00000004);
    public static property TEXTURE_FETCH_BARRIER_BIT:       MemoryBarrierTypeFlags read new MemoryBarrierTypeFlags($00000008);
    public static property SHADER_IMAGE_ACCESS_BARRIER_BIT: MemoryBarrierTypeFlags read new MemoryBarrierTypeFlags($00000020);
    public static property COMMAND_BARRIER_BIT:             MemoryBarrierTypeFlags read new MemoryBarrierTypeFlags($00000040);
    public static property PIXEL_BUFFER_BARRIER_BIT:        MemoryBarrierTypeFlags read new MemoryBarrierTypeFlags($00000080);
    public static property TEXTURE_UPDATE_BARRIER_BIT:      MemoryBarrierTypeFlags read new MemoryBarrierTypeFlags($00000100);
    public static property BUFFER_UPDATE_BARRIER_BIT:       MemoryBarrierTypeFlags read new MemoryBarrierTypeFlags($00000200);
    public static property FRAMEBUFFER_BARRIER_BIT:         MemoryBarrierTypeFlags read new MemoryBarrierTypeFlags($00000400);
    public static property TRANSFORM_FEEDBACK_BARRIER_BIT:  MemoryBarrierTypeFlags read new MemoryBarrierTypeFlags($00000800);
    public static property ATOMIC_COUNTER_BARRIER_BIT:      MemoryBarrierTypeFlags read new MemoryBarrierTypeFlags($00001000);
    public static property SHADER_STORAGE_BARRIER_BIT:      MemoryBarrierTypeFlags read new MemoryBarrierTypeFlags($00002000);
    public static property QUERY_BUFFER_BARRIER_BIT:        MemoryBarrierTypeFlags read new MemoryBarrierTypeFlags($00008000);
    public static property ALL_BARRIER_BITS:                MemoryBarrierTypeFlags read new MemoryBarrierTypeFlags($FFFFFFFF);
    
    public static function operator or(f1,f2: MemoryBarrierTypeFlags): MemoryBarrierTypeFlags := new MemoryBarrierTypeFlags(f1.val or f2.val);
    
  end;
  
  //S
  ProgramStagesFlags = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property VERTEX_SHADER_BIT:           ProgramStagesFlags read new ProgramStagesFlags($00000001);
    public static property FRAGMENT_SHADER_BIT:         ProgramStagesFlags read new ProgramStagesFlags($00000002);
    public static property GEOMETRY_SHADER_BIT:         ProgramStagesFlags read new ProgramStagesFlags($00000004);
    public static property TESS_CONTROL_SHADER_BIT:     ProgramStagesFlags read new ProgramStagesFlags($00000008);
    public static property TESS_EVALUATION_SHADER_BIT:  ProgramStagesFlags read new ProgramStagesFlags($00000010);
    public static property COMPUTE_SHADER_BIT:          ProgramStagesFlags read new ProgramStagesFlags($00000020);
    public static property ALL_SHADER_BITS:             ProgramStagesFlags read new ProgramStagesFlags($FFFFFFFF);
    
    public static function operator or(f1,f2: ProgramStagesFlags): ProgramStagesFlags := new ProgramStagesFlags(f1.val or f2.val);
    
  end;
  
  //S
  ReservedFlags = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property NONE: ReservedFlags read new ReservedFlags($0);
    
    public static function operator or(f1,f2: ReservedFlags): ReservedFlags := new ReservedFlags(f1.val or f2.val);
    
  end;
  
  //S
  CommandFlushingBehaviorFlags = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property SYNC_FLUSH_COMMANDS:  CommandFlushingBehaviorFlags read new CommandFlushingBehaviorFlags($00000001);
    
    public static function operator or(f1,f2: CommandFlushingBehaviorFlags): CommandFlushingBehaviorFlags := new CommandFlushingBehaviorFlags(f1.val or f2.val);
    
  end;
  
  //S
  BufferMapFlags = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property READ_BIT:              BufferMapFlags read new BufferMapFlags($0001);
    public static property WRITE_BIT:             BufferMapFlags read new BufferMapFlags($0002);
    public static property INVALIDATE_RANGE_BIT:  BufferMapFlags read new BufferMapFlags($0004);
    public static property INVALIDATE_BUFFER_BIT: BufferMapFlags read new BufferMapFlags($0008);
    public static property FLUSH_EXPLICIT_BIT:    BufferMapFlags read new BufferMapFlags($0010);
    public static property UNSYNCHRONIZED_BIT:    BufferMapFlags read new BufferMapFlags($0020);
    public static property PERSISTENT_BIT:        BufferMapFlags read new BufferMapFlags($0040);
    public static property COHERENT_BIT:          BufferMapFlags read new BufferMapFlags($0080);
    
    public static function operator or(f1,f2: BufferMapFlags): BufferMapFlags := new BufferMapFlags(f1.val or f2.val);
    
  end;
  
  //S
  BufferStorageFlags = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property DYNAMIC_STORAGE_BIT: BufferStorageFlags read new BufferStorageFlags($0100);
    public static property CLIENT_STORAGE_BIT:  BufferStorageFlags read new BufferStorageFlags($0200);
    
    public static function operator implicit(f: BufferMapFlags): BufferStorageFlags := new BufferStorageFlags(f.val);
    
    public static function operator or(f1,f2: BufferStorageFlags): BufferStorageFlags := new BufferStorageFlags(f1.val or f2.val);
    
  end;
  
  {$endregion Флаги}
  
{$endregion Энумы}

{$region Делегаты}

type
  [UnmanagedFunctionPointer(CallingConvention.StdCall)]
  GLDEBUGPROC = procedure(source: DebugSourceType; &type: DebugMessageType; id: UInt32; severity: DebugSeverityLevel; length: Int32; message_text: IntPtr; userParam: pointer);
  
  [UnmanagedFunctionPointer(CallingConvention.StdCall)]
  GLVULKANPROCNV = procedure;
  
{$endregion Делегаты}

{$region Записи} type
  
  {$region Vec}
  
  {$region Vec1}
  
  Vec1b = record
    public val0: SByte;
    
    public constructor(val0: SByte);
    begin
      self.val0 := val0;
    end;
    
    private function GetValAt(i: integer): SByte;
    begin
      case i of
        0: Result := self.val0;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..0');
      end;
    end;
    private procedure SetValAt(i: integer; val: SByte);
    begin
      case i of
        0: self.val0 := val;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..0');
      end;
    end;
    public property val[i: integer]: SByte read GetValAt write SetValAt; default;
    
    public static function operator-(v: Vec1b): Vec1b := new Vec1b(-v.val0);
    public static function operator*(v: Vec1b; k: SByte): Vec1b := new Vec1b(v.val0*k);
    public static function operator+(v1, v2: Vec1b): Vec1b := new Vec1b(v1.val0+v2.val0);
    public static function operator-(v1, v2: Vec1b): Vec1b := new Vec1b(v1.val0-v2.val0);
    
  end;
  
  Vec1ub = record
    public val0: Byte;
    
    public constructor(val0: Byte);
    begin
      self.val0 := val0;
    end;
    
    private function GetValAt(i: integer): Byte;
    begin
      case i of
        0: Result := self.val0;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..0');
      end;
    end;
    private procedure SetValAt(i: integer; val: Byte);
    begin
      case i of
        0: self.val0 := val;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..0');
      end;
    end;
    public property val[i: integer]: Byte read GetValAt write SetValAt; default;
    
    public static function operator*(v: Vec1ub; k: Byte): Vec1ub := new Vec1ub(v.val0*k);
    public static function operator+(v1, v2: Vec1ub): Vec1ub := new Vec1ub(v1.val0+v2.val0);
    public static function operator-(v1, v2: Vec1ub): Vec1ub := new Vec1ub(v1.val0-v2.val0);
    
    public static function operator implicit(v: Vec1b): Vec1ub := new Vec1ub(v.val0);
    public static function operator implicit(v: Vec1ub): Vec1b := new Vec1b(v.val0);
    
  end;
  
  Vec1s = record
    public val0: Int16;
    
    public constructor(val0: Int16);
    begin
      self.val0 := val0;
    end;
    
    private function GetValAt(i: integer): Int16;
    begin
      case i of
        0: Result := self.val0;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..0');
      end;
    end;
    private procedure SetValAt(i: integer; val: Int16);
    begin
      case i of
        0: self.val0 := val;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..0');
      end;
    end;
    public property val[i: integer]: Int16 read GetValAt write SetValAt; default;
    
    public static function operator-(v: Vec1s): Vec1s := new Vec1s(-v.val0);
    public static function operator*(v: Vec1s; k: Int16): Vec1s := new Vec1s(v.val0*k);
    public static function operator+(v1, v2: Vec1s): Vec1s := new Vec1s(v1.val0+v2.val0);
    public static function operator-(v1, v2: Vec1s): Vec1s := new Vec1s(v1.val0-v2.val0);
    
    public static function operator implicit(v: Vec1b): Vec1s := new Vec1s(v.val0);
    public static function operator implicit(v: Vec1s): Vec1b := new Vec1b(v.val0);
    
    public static function operator implicit(v: Vec1ub): Vec1s := new Vec1s(v.val0);
    public static function operator implicit(v: Vec1s): Vec1ub := new Vec1ub(v.val0);
    
  end;
  
  Vec1us = record
    public val0: UInt16;
    
    public constructor(val0: UInt16);
    begin
      self.val0 := val0;
    end;
    
    private function GetValAt(i: integer): UInt16;
    begin
      case i of
        0: Result := self.val0;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..0');
      end;
    end;
    private procedure SetValAt(i: integer; val: UInt16);
    begin
      case i of
        0: self.val0 := val;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..0');
      end;
    end;
    public property val[i: integer]: UInt16 read GetValAt write SetValAt; default;
    
    public static function operator*(v: Vec1us; k: UInt16): Vec1us := new Vec1us(v.val0*k);
    public static function operator+(v1, v2: Vec1us): Vec1us := new Vec1us(v1.val0+v2.val0);
    public static function operator-(v1, v2: Vec1us): Vec1us := new Vec1us(v1.val0-v2.val0);
    
    public static function operator implicit(v: Vec1b): Vec1us := new Vec1us(v.val0);
    public static function operator implicit(v: Vec1us): Vec1b := new Vec1b(v.val0);
    
    public static function operator implicit(v: Vec1ub): Vec1us := new Vec1us(v.val0);
    public static function operator implicit(v: Vec1us): Vec1ub := new Vec1ub(v.val0);
    
    public static function operator implicit(v: Vec1s): Vec1us := new Vec1us(v.val0);
    public static function operator implicit(v: Vec1us): Vec1s := new Vec1s(v.val0);
    
  end;
  
  Vec1i = record
    public val0: Int32;
    
    public constructor(val0: Int32);
    begin
      self.val0 := val0;
    end;
    
    private function GetValAt(i: integer): Int32;
    begin
      case i of
        0: Result := self.val0;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..0');
      end;
    end;
    private procedure SetValAt(i: integer; val: Int32);
    begin
      case i of
        0: self.val0 := val;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..0');
      end;
    end;
    public property val[i: integer]: Int32 read GetValAt write SetValAt; default;
    
    public static function operator-(v: Vec1i): Vec1i := new Vec1i(-v.val0);
    public static function operator*(v: Vec1i; k: Int32): Vec1i := new Vec1i(v.val0*k);
    public static function operator+(v1, v2: Vec1i): Vec1i := new Vec1i(v1.val0+v2.val0);
    public static function operator-(v1, v2: Vec1i): Vec1i := new Vec1i(v1.val0-v2.val0);
    
    public static function operator implicit(v: Vec1b): Vec1i := new Vec1i(v.val0);
    public static function operator implicit(v: Vec1i): Vec1b := new Vec1b(v.val0);
    
    public static function operator implicit(v: Vec1ub): Vec1i := new Vec1i(v.val0);
    public static function operator implicit(v: Vec1i): Vec1ub := new Vec1ub(v.val0);
    
    public static function operator implicit(v: Vec1s): Vec1i := new Vec1i(v.val0);
    public static function operator implicit(v: Vec1i): Vec1s := new Vec1s(v.val0);
    
    public static function operator implicit(v: Vec1us): Vec1i := new Vec1i(v.val0);
    public static function operator implicit(v: Vec1i): Vec1us := new Vec1us(v.val0);
    
  end;
  
  Vec1ui = record
    public val0: UInt32;
    
    public constructor(val0: UInt32);
    begin
      self.val0 := val0;
    end;
    
    private function GetValAt(i: integer): UInt32;
    begin
      case i of
        0: Result := self.val0;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..0');
      end;
    end;
    private procedure SetValAt(i: integer; val: UInt32);
    begin
      case i of
        0: self.val0 := val;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..0');
      end;
    end;
    public property val[i: integer]: UInt32 read GetValAt write SetValAt; default;
    
    public static function operator*(v: Vec1ui; k: UInt32): Vec1ui := new Vec1ui(v.val0*k);
    public static function operator+(v1, v2: Vec1ui): Vec1ui := new Vec1ui(v1.val0+v2.val0);
    public static function operator-(v1, v2: Vec1ui): Vec1ui := new Vec1ui(v1.val0-v2.val0);
    
    public static function operator implicit(v: Vec1b): Vec1ui := new Vec1ui(v.val0);
    public static function operator implicit(v: Vec1ui): Vec1b := new Vec1b(v.val0);
    
    public static function operator implicit(v: Vec1ub): Vec1ui := new Vec1ui(v.val0);
    public static function operator implicit(v: Vec1ui): Vec1ub := new Vec1ub(v.val0);
    
    public static function operator implicit(v: Vec1s): Vec1ui := new Vec1ui(v.val0);
    public static function operator implicit(v: Vec1ui): Vec1s := new Vec1s(v.val0);
    
    public static function operator implicit(v: Vec1us): Vec1ui := new Vec1ui(v.val0);
    public static function operator implicit(v: Vec1ui): Vec1us := new Vec1us(v.val0);
    
    public static function operator implicit(v: Vec1i): Vec1ui := new Vec1ui(v.val0);
    public static function operator implicit(v: Vec1ui): Vec1i := new Vec1i(v.val0);
    
  end;
  
  Vec1i64 = record
    public val0: Int64;
    
    public constructor(val0: Int64);
    begin
      self.val0 := val0;
    end;
    
    private function GetValAt(i: integer): Int64;
    begin
      case i of
        0: Result := self.val0;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..0');
      end;
    end;
    private procedure SetValAt(i: integer; val: Int64);
    begin
      case i of
        0: self.val0 := val;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..0');
      end;
    end;
    public property val[i: integer]: Int64 read GetValAt write SetValAt; default;
    
    public static function operator-(v: Vec1i64): Vec1i64 := new Vec1i64(-v.val0);
    public static function operator*(v: Vec1i64; k: Int64): Vec1i64 := new Vec1i64(v.val0*k);
    public static function operator+(v1, v2: Vec1i64): Vec1i64 := new Vec1i64(v1.val0+v2.val0);
    public static function operator-(v1, v2: Vec1i64): Vec1i64 := new Vec1i64(v1.val0-v2.val0);
    
    public static function operator implicit(v: Vec1b): Vec1i64 := new Vec1i64(v.val0);
    public static function operator implicit(v: Vec1i64): Vec1b := new Vec1b(v.val0);
    
    public static function operator implicit(v: Vec1ub): Vec1i64 := new Vec1i64(v.val0);
    public static function operator implicit(v: Vec1i64): Vec1ub := new Vec1ub(v.val0);
    
    public static function operator implicit(v: Vec1s): Vec1i64 := new Vec1i64(v.val0);
    public static function operator implicit(v: Vec1i64): Vec1s := new Vec1s(v.val0);
    
    public static function operator implicit(v: Vec1us): Vec1i64 := new Vec1i64(v.val0);
    public static function operator implicit(v: Vec1i64): Vec1us := new Vec1us(v.val0);
    
    public static function operator implicit(v: Vec1i): Vec1i64 := new Vec1i64(v.val0);
    public static function operator implicit(v: Vec1i64): Vec1i := new Vec1i(v.val0);
    
    public static function operator implicit(v: Vec1ui): Vec1i64 := new Vec1i64(v.val0);
    public static function operator implicit(v: Vec1i64): Vec1ui := new Vec1ui(v.val0);
    
  end;
  
  Vec1ui64 = record
    public val0: UInt64;
    
    public constructor(val0: UInt64);
    begin
      self.val0 := val0;
    end;
    
    private function GetValAt(i: integer): UInt64;
    begin
      case i of
        0: Result := self.val0;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..0');
      end;
    end;
    private procedure SetValAt(i: integer; val: UInt64);
    begin
      case i of
        0: self.val0 := val;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..0');
      end;
    end;
    public property val[i: integer]: UInt64 read GetValAt write SetValAt; default;
    
    public static function operator*(v: Vec1ui64; k: UInt64): Vec1ui64 := new Vec1ui64(v.val0*k);
    public static function operator+(v1, v2: Vec1ui64): Vec1ui64 := new Vec1ui64(v1.val0+v2.val0);
    public static function operator-(v1, v2: Vec1ui64): Vec1ui64 := new Vec1ui64(v1.val0-v2.val0);
    
    public static function operator implicit(v: Vec1b): Vec1ui64 := new Vec1ui64(v.val0);
    public static function operator implicit(v: Vec1ui64): Vec1b := new Vec1b(v.val0);
    
    public static function operator implicit(v: Vec1ub): Vec1ui64 := new Vec1ui64(v.val0);
    public static function operator implicit(v: Vec1ui64): Vec1ub := new Vec1ub(v.val0);
    
    public static function operator implicit(v: Vec1s): Vec1ui64 := new Vec1ui64(v.val0);
    public static function operator implicit(v: Vec1ui64): Vec1s := new Vec1s(v.val0);
    
    public static function operator implicit(v: Vec1us): Vec1ui64 := new Vec1ui64(v.val0);
    public static function operator implicit(v: Vec1ui64): Vec1us := new Vec1us(v.val0);
    
    public static function operator implicit(v: Vec1i): Vec1ui64 := new Vec1ui64(v.val0);
    public static function operator implicit(v: Vec1ui64): Vec1i := new Vec1i(v.val0);
    
    public static function operator implicit(v: Vec1ui): Vec1ui64 := new Vec1ui64(v.val0);
    public static function operator implicit(v: Vec1ui64): Vec1ui := new Vec1ui(v.val0);
    
    public static function operator implicit(v: Vec1i64): Vec1ui64 := new Vec1ui64(v.val0);
    public static function operator implicit(v: Vec1ui64): Vec1i64 := new Vec1i64(v.val0);
    
  end;
  
  Vec1f = record
    public val0: single;
    
    public constructor(val0: single);
    begin
      self.val0 := val0;
    end;
    
    private function GetValAt(i: integer): single;
    begin
      case i of
        0: Result := self.val0;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..0');
      end;
    end;
    private procedure SetValAt(i: integer; val: single);
    begin
      case i of
        0: self.val0 := val;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..0');
      end;
    end;
    public property val[i: integer]: single read GetValAt write SetValAt; default;
    
    public static function operator-(v: Vec1f): Vec1f := new Vec1f(-v.val0);
    public static function operator*(v: Vec1f; k: single): Vec1f := new Vec1f(v.val0*k);
    public static function operator+(v1, v2: Vec1f): Vec1f := new Vec1f(v1.val0+v2.val0);
    public static function operator-(v1, v2: Vec1f): Vec1f := new Vec1f(v1.val0-v2.val0);
    
    public static function operator implicit(v: Vec1b): Vec1f := new Vec1f(v.val0);
    public static function operator implicit(v: Vec1f): Vec1b := new Vec1b(Convert.ToSByte(v.val0));
    
    public static function operator implicit(v: Vec1ub): Vec1f := new Vec1f(v.val0);
    public static function operator implicit(v: Vec1f): Vec1ub := new Vec1ub(Convert.ToByte(v.val0));
    
    public static function operator implicit(v: Vec1s): Vec1f := new Vec1f(v.val0);
    public static function operator implicit(v: Vec1f): Vec1s := new Vec1s(Convert.ToInt16(v.val0));
    
    public static function operator implicit(v: Vec1us): Vec1f := new Vec1f(v.val0);
    public static function operator implicit(v: Vec1f): Vec1us := new Vec1us(Convert.ToUInt16(v.val0));
    
    public static function operator implicit(v: Vec1i): Vec1f := new Vec1f(v.val0);
    public static function operator implicit(v: Vec1f): Vec1i := new Vec1i(Convert.ToInt32(v.val0));
    
    public static function operator implicit(v: Vec1ui): Vec1f := new Vec1f(v.val0);
    public static function operator implicit(v: Vec1f): Vec1ui := new Vec1ui(Convert.ToUInt32(v.val0));
    
    public static function operator implicit(v: Vec1i64): Vec1f := new Vec1f(v.val0);
    public static function operator implicit(v: Vec1f): Vec1i64 := new Vec1i64(Convert.ToInt64(v.val0));
    
    public static function operator implicit(v: Vec1ui64): Vec1f := new Vec1f(v.val0);
    public static function operator implicit(v: Vec1f): Vec1ui64 := new Vec1ui64(Convert.ToUInt64(v.val0));
    
  end;
  
  Vec1d = record
    public val0: real;
    
    public constructor(val0: real);
    begin
      self.val0 := val0;
    end;
    
    private function GetValAt(i: integer): real;
    begin
      case i of
        0: Result := self.val0;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..0');
      end;
    end;
    private procedure SetValAt(i: integer; val: real);
    begin
      case i of
        0: self.val0 := val;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..0');
      end;
    end;
    public property val[i: integer]: real read GetValAt write SetValAt; default;
    
    public static function operator-(v: Vec1d): Vec1d := new Vec1d(-v.val0);
    public static function operator*(v: Vec1d; k: real): Vec1d := new Vec1d(v.val0*k);
    public static function operator+(v1, v2: Vec1d): Vec1d := new Vec1d(v1.val0+v2.val0);
    public static function operator-(v1, v2: Vec1d): Vec1d := new Vec1d(v1.val0-v2.val0);
    
    public static function operator implicit(v: Vec1b): Vec1d := new Vec1d(v.val0);
    public static function operator implicit(v: Vec1d): Vec1b := new Vec1b(Convert.ToSByte(v.val0));
    
    public static function operator implicit(v: Vec1ub): Vec1d := new Vec1d(v.val0);
    public static function operator implicit(v: Vec1d): Vec1ub := new Vec1ub(Convert.ToByte(v.val0));
    
    public static function operator implicit(v: Vec1s): Vec1d := new Vec1d(v.val0);
    public static function operator implicit(v: Vec1d): Vec1s := new Vec1s(Convert.ToInt16(v.val0));
    
    public static function operator implicit(v: Vec1us): Vec1d := new Vec1d(v.val0);
    public static function operator implicit(v: Vec1d): Vec1us := new Vec1us(Convert.ToUInt16(v.val0));
    
    public static function operator implicit(v: Vec1i): Vec1d := new Vec1d(v.val0);
    public static function operator implicit(v: Vec1d): Vec1i := new Vec1i(Convert.ToInt32(v.val0));
    
    public static function operator implicit(v: Vec1ui): Vec1d := new Vec1d(v.val0);
    public static function operator implicit(v: Vec1d): Vec1ui := new Vec1ui(Convert.ToUInt32(v.val0));
    
    public static function operator implicit(v: Vec1i64): Vec1d := new Vec1d(v.val0);
    public static function operator implicit(v: Vec1d): Vec1i64 := new Vec1i64(Convert.ToInt64(v.val0));
    
    public static function operator implicit(v: Vec1ui64): Vec1d := new Vec1d(v.val0);
    public static function operator implicit(v: Vec1d): Vec1ui64 := new Vec1ui64(Convert.ToUInt64(v.val0));
    
    public static function operator implicit(v: Vec1f): Vec1d := new Vec1d(v.val0);
    public static function operator implicit(v: Vec1d): Vec1f := new Vec1f(v.val0);
    
  end;
  {$endregion Vec1}
  
  {$region Vec2}
  
  Vec2b = record
    public val0: SByte;
    public val1: SByte;
    
    public constructor(val0, val1: SByte);
    begin
      self.val0 := val0;
      self.val1 := val1;
    end;
    
    private function GetValAt(i: integer): SByte;
    begin
      case i of
        0: Result := self.val0;
        1: Result := self.val1;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..1');
      end;
    end;
    private procedure SetValAt(i: integer; val: SByte);
    begin
      case i of
        0: self.val0 := val;
        1: self.val1 := val;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..1');
      end;
    end;
    public property val[i: integer]: SByte read GetValAt write SetValAt; default;
    
    public static function operator-(v: Vec2b): Vec2b := new Vec2b(-v.val0, -v.val1);
    public static function operator*(v: Vec2b; k: SByte): Vec2b := new Vec2b(v.val0*k, v.val1*k);
    public static function operator+(v1, v2: Vec2b): Vec2b := new Vec2b(v1.val0+v2.val0, v1.val1+v2.val1);
    public static function operator-(v1, v2: Vec2b): Vec2b := new Vec2b(v1.val0-v2.val0, v1.val1-v2.val1);
    
    public static function operator implicit(v: Vec1b): Vec2b := new Vec2b(v.val0, 0);
    public static function operator implicit(v: Vec2b): Vec1b := new Vec1b(v.val0);
    
    public static function operator implicit(v: Vec1ub): Vec2b := new Vec2b(v.val0, 0);
    public static function operator implicit(v: Vec2b): Vec1ub := new Vec1ub(v.val0);
    
    public static function operator implicit(v: Vec1s): Vec2b := new Vec2b(v.val0, 0);
    public static function operator implicit(v: Vec2b): Vec1s := new Vec1s(v.val0);
    
    public static function operator implicit(v: Vec1us): Vec2b := new Vec2b(v.val0, 0);
    public static function operator implicit(v: Vec2b): Vec1us := new Vec1us(v.val0);
    
    public static function operator implicit(v: Vec1i): Vec2b := new Vec2b(v.val0, 0);
    public static function operator implicit(v: Vec2b): Vec1i := new Vec1i(v.val0);
    
    public static function operator implicit(v: Vec1ui): Vec2b := new Vec2b(v.val0, 0);
    public static function operator implicit(v: Vec2b): Vec1ui := new Vec1ui(v.val0);
    
    public static function operator implicit(v: Vec1i64): Vec2b := new Vec2b(v.val0, 0);
    public static function operator implicit(v: Vec2b): Vec1i64 := new Vec1i64(v.val0);
    
    public static function operator implicit(v: Vec1ui64): Vec2b := new Vec2b(v.val0, 0);
    public static function operator implicit(v: Vec2b): Vec1ui64 := new Vec1ui64(v.val0);
    
    public static function operator implicit(v: Vec1f): Vec2b := new Vec2b(Convert.ToSByte(v.val0), 0);
    public static function operator implicit(v: Vec2b): Vec1f := new Vec1f(v.val0);
    
    public static function operator implicit(v: Vec1d): Vec2b := new Vec2b(Convert.ToSByte(v.val0), 0);
    public static function operator implicit(v: Vec2b): Vec1d := new Vec1d(v.val0);
    
  end;
  
  Vec2ub = record
    public val0: Byte;
    public val1: Byte;
    
    public constructor(val0, val1: Byte);
    begin
      self.val0 := val0;
      self.val1 := val1;
    end;
    
    private function GetValAt(i: integer): Byte;
    begin
      case i of
        0: Result := self.val0;
        1: Result := self.val1;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..1');
      end;
    end;
    private procedure SetValAt(i: integer; val: Byte);
    begin
      case i of
        0: self.val0 := val;
        1: self.val1 := val;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..1');
      end;
    end;
    public property val[i: integer]: Byte read GetValAt write SetValAt; default;
    
    public static function operator*(v: Vec2ub; k: Byte): Vec2ub := new Vec2ub(v.val0*k, v.val1*k);
    public static function operator+(v1, v2: Vec2ub): Vec2ub := new Vec2ub(v1.val0+v2.val0, v1.val1+v2.val1);
    public static function operator-(v1, v2: Vec2ub): Vec2ub := new Vec2ub(v1.val0-v2.val0, v1.val1-v2.val1);
    
    public static function operator implicit(v: Vec1b): Vec2ub := new Vec2ub(v.val0, 0);
    public static function operator implicit(v: Vec2ub): Vec1b := new Vec1b(v.val0);
    
    public static function operator implicit(v: Vec1ub): Vec2ub := new Vec2ub(v.val0, 0);
    public static function operator implicit(v: Vec2ub): Vec1ub := new Vec1ub(v.val0);
    
    public static function operator implicit(v: Vec1s): Vec2ub := new Vec2ub(v.val0, 0);
    public static function operator implicit(v: Vec2ub): Vec1s := new Vec1s(v.val0);
    
    public static function operator implicit(v: Vec1us): Vec2ub := new Vec2ub(v.val0, 0);
    public static function operator implicit(v: Vec2ub): Vec1us := new Vec1us(v.val0);
    
    public static function operator implicit(v: Vec1i): Vec2ub := new Vec2ub(v.val0, 0);
    public static function operator implicit(v: Vec2ub): Vec1i := new Vec1i(v.val0);
    
    public static function operator implicit(v: Vec1ui): Vec2ub := new Vec2ub(v.val0, 0);
    public static function operator implicit(v: Vec2ub): Vec1ui := new Vec1ui(v.val0);
    
    public static function operator implicit(v: Vec1i64): Vec2ub := new Vec2ub(v.val0, 0);
    public static function operator implicit(v: Vec2ub): Vec1i64 := new Vec1i64(v.val0);
    
    public static function operator implicit(v: Vec1ui64): Vec2ub := new Vec2ub(v.val0, 0);
    public static function operator implicit(v: Vec2ub): Vec1ui64 := new Vec1ui64(v.val0);
    
    public static function operator implicit(v: Vec1f): Vec2ub := new Vec2ub(Convert.ToByte(v.val0), 0);
    public static function operator implicit(v: Vec2ub): Vec1f := new Vec1f(v.val0);
    
    public static function operator implicit(v: Vec1d): Vec2ub := new Vec2ub(Convert.ToByte(v.val0), 0);
    public static function operator implicit(v: Vec2ub): Vec1d := new Vec1d(v.val0);
    
    public static function operator implicit(v: Vec2b): Vec2ub := new Vec2ub(v.val0, v.val1);
    public static function operator implicit(v: Vec2ub): Vec2b := new Vec2b(v.val0, v.val1);
    
  end;
  
  Vec2s = record
    public val0: Int16;
    public val1: Int16;
    
    public constructor(val0, val1: Int16);
    begin
      self.val0 := val0;
      self.val1 := val1;
    end;
    
    private function GetValAt(i: integer): Int16;
    begin
      case i of
        0: Result := self.val0;
        1: Result := self.val1;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..1');
      end;
    end;
    private procedure SetValAt(i: integer; val: Int16);
    begin
      case i of
        0: self.val0 := val;
        1: self.val1 := val;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..1');
      end;
    end;
    public property val[i: integer]: Int16 read GetValAt write SetValAt; default;
    
    public static function operator-(v: Vec2s): Vec2s := new Vec2s(-v.val0, -v.val1);
    public static function operator*(v: Vec2s; k: Int16): Vec2s := new Vec2s(v.val0*k, v.val1*k);
    public static function operator+(v1, v2: Vec2s): Vec2s := new Vec2s(v1.val0+v2.val0, v1.val1+v2.val1);
    public static function operator-(v1, v2: Vec2s): Vec2s := new Vec2s(v1.val0-v2.val0, v1.val1-v2.val1);
    
    public static function operator implicit(v: Vec1b): Vec2s := new Vec2s(v.val0, 0);
    public static function operator implicit(v: Vec2s): Vec1b := new Vec1b(v.val0);
    
    public static function operator implicit(v: Vec1ub): Vec2s := new Vec2s(v.val0, 0);
    public static function operator implicit(v: Vec2s): Vec1ub := new Vec1ub(v.val0);
    
    public static function operator implicit(v: Vec1s): Vec2s := new Vec2s(v.val0, 0);
    public static function operator implicit(v: Vec2s): Vec1s := new Vec1s(v.val0);
    
    public static function operator implicit(v: Vec1us): Vec2s := new Vec2s(v.val0, 0);
    public static function operator implicit(v: Vec2s): Vec1us := new Vec1us(v.val0);
    
    public static function operator implicit(v: Vec1i): Vec2s := new Vec2s(v.val0, 0);
    public static function operator implicit(v: Vec2s): Vec1i := new Vec1i(v.val0);
    
    public static function operator implicit(v: Vec1ui): Vec2s := new Vec2s(v.val0, 0);
    public static function operator implicit(v: Vec2s): Vec1ui := new Vec1ui(v.val0);
    
    public static function operator implicit(v: Vec1i64): Vec2s := new Vec2s(v.val0, 0);
    public static function operator implicit(v: Vec2s): Vec1i64 := new Vec1i64(v.val0);
    
    public static function operator implicit(v: Vec1ui64): Vec2s := new Vec2s(v.val0, 0);
    public static function operator implicit(v: Vec2s): Vec1ui64 := new Vec1ui64(v.val0);
    
    public static function operator implicit(v: Vec1f): Vec2s := new Vec2s(Convert.ToInt16(v.val0), 0);
    public static function operator implicit(v: Vec2s): Vec1f := new Vec1f(v.val0);
    
    public static function operator implicit(v: Vec1d): Vec2s := new Vec2s(Convert.ToInt16(v.val0), 0);
    public static function operator implicit(v: Vec2s): Vec1d := new Vec1d(v.val0);
    
    public static function operator implicit(v: Vec2b): Vec2s := new Vec2s(v.val0, v.val1);
    public static function operator implicit(v: Vec2s): Vec2b := new Vec2b(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2ub): Vec2s := new Vec2s(v.val0, v.val1);
    public static function operator implicit(v: Vec2s): Vec2ub := new Vec2ub(v.val0, v.val1);
    
  end;
  
  Vec2us = record
    public val0: UInt16;
    public val1: UInt16;
    
    public constructor(val0, val1: UInt16);
    begin
      self.val0 := val0;
      self.val1 := val1;
    end;
    
    private function GetValAt(i: integer): UInt16;
    begin
      case i of
        0: Result := self.val0;
        1: Result := self.val1;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..1');
      end;
    end;
    private procedure SetValAt(i: integer; val: UInt16);
    begin
      case i of
        0: self.val0 := val;
        1: self.val1 := val;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..1');
      end;
    end;
    public property val[i: integer]: UInt16 read GetValAt write SetValAt; default;
    
    public static function operator*(v: Vec2us; k: UInt16): Vec2us := new Vec2us(v.val0*k, v.val1*k);
    public static function operator+(v1, v2: Vec2us): Vec2us := new Vec2us(v1.val0+v2.val0, v1.val1+v2.val1);
    public static function operator-(v1, v2: Vec2us): Vec2us := new Vec2us(v1.val0-v2.val0, v1.val1-v2.val1);
    
    public static function operator implicit(v: Vec1b): Vec2us := new Vec2us(v.val0, 0);
    public static function operator implicit(v: Vec2us): Vec1b := new Vec1b(v.val0);
    
    public static function operator implicit(v: Vec1ub): Vec2us := new Vec2us(v.val0, 0);
    public static function operator implicit(v: Vec2us): Vec1ub := new Vec1ub(v.val0);
    
    public static function operator implicit(v: Vec1s): Vec2us := new Vec2us(v.val0, 0);
    public static function operator implicit(v: Vec2us): Vec1s := new Vec1s(v.val0);
    
    public static function operator implicit(v: Vec1us): Vec2us := new Vec2us(v.val0, 0);
    public static function operator implicit(v: Vec2us): Vec1us := new Vec1us(v.val0);
    
    public static function operator implicit(v: Vec1i): Vec2us := new Vec2us(v.val0, 0);
    public static function operator implicit(v: Vec2us): Vec1i := new Vec1i(v.val0);
    
    public static function operator implicit(v: Vec1ui): Vec2us := new Vec2us(v.val0, 0);
    public static function operator implicit(v: Vec2us): Vec1ui := new Vec1ui(v.val0);
    
    public static function operator implicit(v: Vec1i64): Vec2us := new Vec2us(v.val0, 0);
    public static function operator implicit(v: Vec2us): Vec1i64 := new Vec1i64(v.val0);
    
    public static function operator implicit(v: Vec1ui64): Vec2us := new Vec2us(v.val0, 0);
    public static function operator implicit(v: Vec2us): Vec1ui64 := new Vec1ui64(v.val0);
    
    public static function operator implicit(v: Vec1f): Vec2us := new Vec2us(Convert.ToUInt16(v.val0), 0);
    public static function operator implicit(v: Vec2us): Vec1f := new Vec1f(v.val0);
    
    public static function operator implicit(v: Vec1d): Vec2us := new Vec2us(Convert.ToUInt16(v.val0), 0);
    public static function operator implicit(v: Vec2us): Vec1d := new Vec1d(v.val0);
    
    public static function operator implicit(v: Vec2b): Vec2us := new Vec2us(v.val0, v.val1);
    public static function operator implicit(v: Vec2us): Vec2b := new Vec2b(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2ub): Vec2us := new Vec2us(v.val0, v.val1);
    public static function operator implicit(v: Vec2us): Vec2ub := new Vec2ub(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2s): Vec2us := new Vec2us(v.val0, v.val1);
    public static function operator implicit(v: Vec2us): Vec2s := new Vec2s(v.val0, v.val1);
    
  end;
  
  Vec2i = record
    public val0: Int32;
    public val1: Int32;
    
    public constructor(val0, val1: Int32);
    begin
      self.val0 := val0;
      self.val1 := val1;
    end;
    
    private function GetValAt(i: integer): Int32;
    begin
      case i of
        0: Result := self.val0;
        1: Result := self.val1;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..1');
      end;
    end;
    private procedure SetValAt(i: integer; val: Int32);
    begin
      case i of
        0: self.val0 := val;
        1: self.val1 := val;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..1');
      end;
    end;
    public property val[i: integer]: Int32 read GetValAt write SetValAt; default;
    
    public static function operator-(v: Vec2i): Vec2i := new Vec2i(-v.val0, -v.val1);
    public static function operator*(v: Vec2i; k: Int32): Vec2i := new Vec2i(v.val0*k, v.val1*k);
    public static function operator+(v1, v2: Vec2i): Vec2i := new Vec2i(v1.val0+v2.val0, v1.val1+v2.val1);
    public static function operator-(v1, v2: Vec2i): Vec2i := new Vec2i(v1.val0-v2.val0, v1.val1-v2.val1);
    
    public static function operator implicit(v: Vec1b): Vec2i := new Vec2i(v.val0, 0);
    public static function operator implicit(v: Vec2i): Vec1b := new Vec1b(v.val0);
    
    public static function operator implicit(v: Vec1ub): Vec2i := new Vec2i(v.val0, 0);
    public static function operator implicit(v: Vec2i): Vec1ub := new Vec1ub(v.val0);
    
    public static function operator implicit(v: Vec1s): Vec2i := new Vec2i(v.val0, 0);
    public static function operator implicit(v: Vec2i): Vec1s := new Vec1s(v.val0);
    
    public static function operator implicit(v: Vec1us): Vec2i := new Vec2i(v.val0, 0);
    public static function operator implicit(v: Vec2i): Vec1us := new Vec1us(v.val0);
    
    public static function operator implicit(v: Vec1i): Vec2i := new Vec2i(v.val0, 0);
    public static function operator implicit(v: Vec2i): Vec1i := new Vec1i(v.val0);
    
    public static function operator implicit(v: Vec1ui): Vec2i := new Vec2i(v.val0, 0);
    public static function operator implicit(v: Vec2i): Vec1ui := new Vec1ui(v.val0);
    
    public static function operator implicit(v: Vec1i64): Vec2i := new Vec2i(v.val0, 0);
    public static function operator implicit(v: Vec2i): Vec1i64 := new Vec1i64(v.val0);
    
    public static function operator implicit(v: Vec1ui64): Vec2i := new Vec2i(v.val0, 0);
    public static function operator implicit(v: Vec2i): Vec1ui64 := new Vec1ui64(v.val0);
    
    public static function operator implicit(v: Vec1f): Vec2i := new Vec2i(Convert.ToInt32(v.val0), 0);
    public static function operator implicit(v: Vec2i): Vec1f := new Vec1f(v.val0);
    
    public static function operator implicit(v: Vec1d): Vec2i := new Vec2i(Convert.ToInt32(v.val0), 0);
    public static function operator implicit(v: Vec2i): Vec1d := new Vec1d(v.val0);
    
    public static function operator implicit(v: Vec2b): Vec2i := new Vec2i(v.val0, v.val1);
    public static function operator implicit(v: Vec2i): Vec2b := new Vec2b(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2ub): Vec2i := new Vec2i(v.val0, v.val1);
    public static function operator implicit(v: Vec2i): Vec2ub := new Vec2ub(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2s): Vec2i := new Vec2i(v.val0, v.val1);
    public static function operator implicit(v: Vec2i): Vec2s := new Vec2s(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2us): Vec2i := new Vec2i(v.val0, v.val1);
    public static function operator implicit(v: Vec2i): Vec2us := new Vec2us(v.val0, v.val1);
    
  end;
  
  Vec2ui = record
    public val0: UInt32;
    public val1: UInt32;
    
    public constructor(val0, val1: UInt32);
    begin
      self.val0 := val0;
      self.val1 := val1;
    end;
    
    private function GetValAt(i: integer): UInt32;
    begin
      case i of
        0: Result := self.val0;
        1: Result := self.val1;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..1');
      end;
    end;
    private procedure SetValAt(i: integer; val: UInt32);
    begin
      case i of
        0: self.val0 := val;
        1: self.val1 := val;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..1');
      end;
    end;
    public property val[i: integer]: UInt32 read GetValAt write SetValAt; default;
    
    public static function operator*(v: Vec2ui; k: UInt32): Vec2ui := new Vec2ui(v.val0*k, v.val1*k);
    public static function operator+(v1, v2: Vec2ui): Vec2ui := new Vec2ui(v1.val0+v2.val0, v1.val1+v2.val1);
    public static function operator-(v1, v2: Vec2ui): Vec2ui := new Vec2ui(v1.val0-v2.val0, v1.val1-v2.val1);
    
    public static function operator implicit(v: Vec1b): Vec2ui := new Vec2ui(v.val0, 0);
    public static function operator implicit(v: Vec2ui): Vec1b := new Vec1b(v.val0);
    
    public static function operator implicit(v: Vec1ub): Vec2ui := new Vec2ui(v.val0, 0);
    public static function operator implicit(v: Vec2ui): Vec1ub := new Vec1ub(v.val0);
    
    public static function operator implicit(v: Vec1s): Vec2ui := new Vec2ui(v.val0, 0);
    public static function operator implicit(v: Vec2ui): Vec1s := new Vec1s(v.val0);
    
    public static function operator implicit(v: Vec1us): Vec2ui := new Vec2ui(v.val0, 0);
    public static function operator implicit(v: Vec2ui): Vec1us := new Vec1us(v.val0);
    
    public static function operator implicit(v: Vec1i): Vec2ui := new Vec2ui(v.val0, 0);
    public static function operator implicit(v: Vec2ui): Vec1i := new Vec1i(v.val0);
    
    public static function operator implicit(v: Vec1ui): Vec2ui := new Vec2ui(v.val0, 0);
    public static function operator implicit(v: Vec2ui): Vec1ui := new Vec1ui(v.val0);
    
    public static function operator implicit(v: Vec1i64): Vec2ui := new Vec2ui(v.val0, 0);
    public static function operator implicit(v: Vec2ui): Vec1i64 := new Vec1i64(v.val0);
    
    public static function operator implicit(v: Vec1ui64): Vec2ui := new Vec2ui(v.val0, 0);
    public static function operator implicit(v: Vec2ui): Vec1ui64 := new Vec1ui64(v.val0);
    
    public static function operator implicit(v: Vec1f): Vec2ui := new Vec2ui(Convert.ToUInt32(v.val0), 0);
    public static function operator implicit(v: Vec2ui): Vec1f := new Vec1f(v.val0);
    
    public static function operator implicit(v: Vec1d): Vec2ui := new Vec2ui(Convert.ToUInt32(v.val0), 0);
    public static function operator implicit(v: Vec2ui): Vec1d := new Vec1d(v.val0);
    
    public static function operator implicit(v: Vec2b): Vec2ui := new Vec2ui(v.val0, v.val1);
    public static function operator implicit(v: Vec2ui): Vec2b := new Vec2b(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2ub): Vec2ui := new Vec2ui(v.val0, v.val1);
    public static function operator implicit(v: Vec2ui): Vec2ub := new Vec2ub(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2s): Vec2ui := new Vec2ui(v.val0, v.val1);
    public static function operator implicit(v: Vec2ui): Vec2s := new Vec2s(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2us): Vec2ui := new Vec2ui(v.val0, v.val1);
    public static function operator implicit(v: Vec2ui): Vec2us := new Vec2us(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2i): Vec2ui := new Vec2ui(v.val0, v.val1);
    public static function operator implicit(v: Vec2ui): Vec2i := new Vec2i(v.val0, v.val1);
    
  end;
  
  Vec2i64 = record
    public val0: Int64;
    public val1: Int64;
    
    public constructor(val0, val1: Int64);
    begin
      self.val0 := val0;
      self.val1 := val1;
    end;
    
    private function GetValAt(i: integer): Int64;
    begin
      case i of
        0: Result := self.val0;
        1: Result := self.val1;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..1');
      end;
    end;
    private procedure SetValAt(i: integer; val: Int64);
    begin
      case i of
        0: self.val0 := val;
        1: self.val1 := val;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..1');
      end;
    end;
    public property val[i: integer]: Int64 read GetValAt write SetValAt; default;
    
    public static function operator-(v: Vec2i64): Vec2i64 := new Vec2i64(-v.val0, -v.val1);
    public static function operator*(v: Vec2i64; k: Int64): Vec2i64 := new Vec2i64(v.val0*k, v.val1*k);
    public static function operator+(v1, v2: Vec2i64): Vec2i64 := new Vec2i64(v1.val0+v2.val0, v1.val1+v2.val1);
    public static function operator-(v1, v2: Vec2i64): Vec2i64 := new Vec2i64(v1.val0-v2.val0, v1.val1-v2.val1);
    
    public static function operator implicit(v: Vec1b): Vec2i64 := new Vec2i64(v.val0, 0);
    public static function operator implicit(v: Vec2i64): Vec1b := new Vec1b(v.val0);
    
    public static function operator implicit(v: Vec1ub): Vec2i64 := new Vec2i64(v.val0, 0);
    public static function operator implicit(v: Vec2i64): Vec1ub := new Vec1ub(v.val0);
    
    public static function operator implicit(v: Vec1s): Vec2i64 := new Vec2i64(v.val0, 0);
    public static function operator implicit(v: Vec2i64): Vec1s := new Vec1s(v.val0);
    
    public static function operator implicit(v: Vec1us): Vec2i64 := new Vec2i64(v.val0, 0);
    public static function operator implicit(v: Vec2i64): Vec1us := new Vec1us(v.val0);
    
    public static function operator implicit(v: Vec1i): Vec2i64 := new Vec2i64(v.val0, 0);
    public static function operator implicit(v: Vec2i64): Vec1i := new Vec1i(v.val0);
    
    public static function operator implicit(v: Vec1ui): Vec2i64 := new Vec2i64(v.val0, 0);
    public static function operator implicit(v: Vec2i64): Vec1ui := new Vec1ui(v.val0);
    
    public static function operator implicit(v: Vec1i64): Vec2i64 := new Vec2i64(v.val0, 0);
    public static function operator implicit(v: Vec2i64): Vec1i64 := new Vec1i64(v.val0);
    
    public static function operator implicit(v: Vec1ui64): Vec2i64 := new Vec2i64(v.val0, 0);
    public static function operator implicit(v: Vec2i64): Vec1ui64 := new Vec1ui64(v.val0);
    
    public static function operator implicit(v: Vec1f): Vec2i64 := new Vec2i64(Convert.ToInt64(v.val0), 0);
    public static function operator implicit(v: Vec2i64): Vec1f := new Vec1f(v.val0);
    
    public static function operator implicit(v: Vec1d): Vec2i64 := new Vec2i64(Convert.ToInt64(v.val0), 0);
    public static function operator implicit(v: Vec2i64): Vec1d := new Vec1d(v.val0);
    
    public static function operator implicit(v: Vec2b): Vec2i64 := new Vec2i64(v.val0, v.val1);
    public static function operator implicit(v: Vec2i64): Vec2b := new Vec2b(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2ub): Vec2i64 := new Vec2i64(v.val0, v.val1);
    public static function operator implicit(v: Vec2i64): Vec2ub := new Vec2ub(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2s): Vec2i64 := new Vec2i64(v.val0, v.val1);
    public static function operator implicit(v: Vec2i64): Vec2s := new Vec2s(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2us): Vec2i64 := new Vec2i64(v.val0, v.val1);
    public static function operator implicit(v: Vec2i64): Vec2us := new Vec2us(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2i): Vec2i64 := new Vec2i64(v.val0, v.val1);
    public static function operator implicit(v: Vec2i64): Vec2i := new Vec2i(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2ui): Vec2i64 := new Vec2i64(v.val0, v.val1);
    public static function operator implicit(v: Vec2i64): Vec2ui := new Vec2ui(v.val0, v.val1);
    
  end;
  
  Vec2ui64 = record
    public val0: UInt64;
    public val1: UInt64;
    
    public constructor(val0, val1: UInt64);
    begin
      self.val0 := val0;
      self.val1 := val1;
    end;
    
    private function GetValAt(i: integer): UInt64;
    begin
      case i of
        0: Result := self.val0;
        1: Result := self.val1;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..1');
      end;
    end;
    private procedure SetValAt(i: integer; val: UInt64);
    begin
      case i of
        0: self.val0 := val;
        1: self.val1 := val;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..1');
      end;
    end;
    public property val[i: integer]: UInt64 read GetValAt write SetValAt; default;
    
    public static function operator*(v: Vec2ui64; k: UInt64): Vec2ui64 := new Vec2ui64(v.val0*k, v.val1*k);
    public static function operator+(v1, v2: Vec2ui64): Vec2ui64 := new Vec2ui64(v1.val0+v2.val0, v1.val1+v2.val1);
    public static function operator-(v1, v2: Vec2ui64): Vec2ui64 := new Vec2ui64(v1.val0-v2.val0, v1.val1-v2.val1);
    
    public static function operator implicit(v: Vec1b): Vec2ui64 := new Vec2ui64(v.val0, 0);
    public static function operator implicit(v: Vec2ui64): Vec1b := new Vec1b(v.val0);
    
    public static function operator implicit(v: Vec1ub): Vec2ui64 := new Vec2ui64(v.val0, 0);
    public static function operator implicit(v: Vec2ui64): Vec1ub := new Vec1ub(v.val0);
    
    public static function operator implicit(v: Vec1s): Vec2ui64 := new Vec2ui64(v.val0, 0);
    public static function operator implicit(v: Vec2ui64): Vec1s := new Vec1s(v.val0);
    
    public static function operator implicit(v: Vec1us): Vec2ui64 := new Vec2ui64(v.val0, 0);
    public static function operator implicit(v: Vec2ui64): Vec1us := new Vec1us(v.val0);
    
    public static function operator implicit(v: Vec1i): Vec2ui64 := new Vec2ui64(v.val0, 0);
    public static function operator implicit(v: Vec2ui64): Vec1i := new Vec1i(v.val0);
    
    public static function operator implicit(v: Vec1ui): Vec2ui64 := new Vec2ui64(v.val0, 0);
    public static function operator implicit(v: Vec2ui64): Vec1ui := new Vec1ui(v.val0);
    
    public static function operator implicit(v: Vec1i64): Vec2ui64 := new Vec2ui64(v.val0, 0);
    public static function operator implicit(v: Vec2ui64): Vec1i64 := new Vec1i64(v.val0);
    
    public static function operator implicit(v: Vec1ui64): Vec2ui64 := new Vec2ui64(v.val0, 0);
    public static function operator implicit(v: Vec2ui64): Vec1ui64 := new Vec1ui64(v.val0);
    
    public static function operator implicit(v: Vec1f): Vec2ui64 := new Vec2ui64(Convert.ToUInt64(v.val0), 0);
    public static function operator implicit(v: Vec2ui64): Vec1f := new Vec1f(v.val0);
    
    public static function operator implicit(v: Vec1d): Vec2ui64 := new Vec2ui64(Convert.ToUInt64(v.val0), 0);
    public static function operator implicit(v: Vec2ui64): Vec1d := new Vec1d(v.val0);
    
    public static function operator implicit(v: Vec2b): Vec2ui64 := new Vec2ui64(v.val0, v.val1);
    public static function operator implicit(v: Vec2ui64): Vec2b := new Vec2b(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2ub): Vec2ui64 := new Vec2ui64(v.val0, v.val1);
    public static function operator implicit(v: Vec2ui64): Vec2ub := new Vec2ub(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2s): Vec2ui64 := new Vec2ui64(v.val0, v.val1);
    public static function operator implicit(v: Vec2ui64): Vec2s := new Vec2s(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2us): Vec2ui64 := new Vec2ui64(v.val0, v.val1);
    public static function operator implicit(v: Vec2ui64): Vec2us := new Vec2us(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2i): Vec2ui64 := new Vec2ui64(v.val0, v.val1);
    public static function operator implicit(v: Vec2ui64): Vec2i := new Vec2i(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2ui): Vec2ui64 := new Vec2ui64(v.val0, v.val1);
    public static function operator implicit(v: Vec2ui64): Vec2ui := new Vec2ui(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2i64): Vec2ui64 := new Vec2ui64(v.val0, v.val1);
    public static function operator implicit(v: Vec2ui64): Vec2i64 := new Vec2i64(v.val0, v.val1);
    
  end;
  
  Vec2f = record
    public val0: single;
    public val1: single;
    
    public constructor(val0, val1: single);
    begin
      self.val0 := val0;
      self.val1 := val1;
    end;
    
    private function GetValAt(i: integer): single;
    begin
      case i of
        0: Result := self.val0;
        1: Result := self.val1;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..1');
      end;
    end;
    private procedure SetValAt(i: integer; val: single);
    begin
      case i of
        0: self.val0 := val;
        1: self.val1 := val;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..1');
      end;
    end;
    public property val[i: integer]: single read GetValAt write SetValAt; default;
    
    public static function operator-(v: Vec2f): Vec2f := new Vec2f(-v.val0, -v.val1);
    public static function operator*(v: Vec2f; k: single): Vec2f := new Vec2f(v.val0*k, v.val1*k);
    public static function operator+(v1, v2: Vec2f): Vec2f := new Vec2f(v1.val0+v2.val0, v1.val1+v2.val1);
    public static function operator-(v1, v2: Vec2f): Vec2f := new Vec2f(v1.val0-v2.val0, v1.val1-v2.val1);
    
    public static function operator implicit(v: Vec1b): Vec2f := new Vec2f(v.val0, 0);
    public static function operator implicit(v: Vec2f): Vec1b := new Vec1b(Convert.ToSByte(v.val0));
    
    public static function operator implicit(v: Vec1ub): Vec2f := new Vec2f(v.val0, 0);
    public static function operator implicit(v: Vec2f): Vec1ub := new Vec1ub(Convert.ToByte(v.val0));
    
    public static function operator implicit(v: Vec1s): Vec2f := new Vec2f(v.val0, 0);
    public static function operator implicit(v: Vec2f): Vec1s := new Vec1s(Convert.ToInt16(v.val0));
    
    public static function operator implicit(v: Vec1us): Vec2f := new Vec2f(v.val0, 0);
    public static function operator implicit(v: Vec2f): Vec1us := new Vec1us(Convert.ToUInt16(v.val0));
    
    public static function operator implicit(v: Vec1i): Vec2f := new Vec2f(v.val0, 0);
    public static function operator implicit(v: Vec2f): Vec1i := new Vec1i(Convert.ToInt32(v.val0));
    
    public static function operator implicit(v: Vec1ui): Vec2f := new Vec2f(v.val0, 0);
    public static function operator implicit(v: Vec2f): Vec1ui := new Vec1ui(Convert.ToUInt32(v.val0));
    
    public static function operator implicit(v: Vec1i64): Vec2f := new Vec2f(v.val0, 0);
    public static function operator implicit(v: Vec2f): Vec1i64 := new Vec1i64(Convert.ToInt64(v.val0));
    
    public static function operator implicit(v: Vec1ui64): Vec2f := new Vec2f(v.val0, 0);
    public static function operator implicit(v: Vec2f): Vec1ui64 := new Vec1ui64(Convert.ToUInt64(v.val0));
    
    public static function operator implicit(v: Vec1f): Vec2f := new Vec2f(v.val0, 0);
    public static function operator implicit(v: Vec2f): Vec1f := new Vec1f(v.val0);
    
    public static function operator implicit(v: Vec1d): Vec2f := new Vec2f(v.val0, 0);
    public static function operator implicit(v: Vec2f): Vec1d := new Vec1d(v.val0);
    
    public static function operator implicit(v: Vec2b): Vec2f := new Vec2f(v.val0, v.val1);
    public static function operator implicit(v: Vec2f): Vec2b := new Vec2b(Convert.ToSByte(v.val0), Convert.ToSByte(v.val1));
    
    public static function operator implicit(v: Vec2ub): Vec2f := new Vec2f(v.val0, v.val1);
    public static function operator implicit(v: Vec2f): Vec2ub := new Vec2ub(Convert.ToByte(v.val0), Convert.ToByte(v.val1));
    
    public static function operator implicit(v: Vec2s): Vec2f := new Vec2f(v.val0, v.val1);
    public static function operator implicit(v: Vec2f): Vec2s := new Vec2s(Convert.ToInt16(v.val0), Convert.ToInt16(v.val1));
    
    public static function operator implicit(v: Vec2us): Vec2f := new Vec2f(v.val0, v.val1);
    public static function operator implicit(v: Vec2f): Vec2us := new Vec2us(Convert.ToUInt16(v.val0), Convert.ToUInt16(v.val1));
    
    public static function operator implicit(v: Vec2i): Vec2f := new Vec2f(v.val0, v.val1);
    public static function operator implicit(v: Vec2f): Vec2i := new Vec2i(Convert.ToInt32(v.val0), Convert.ToInt32(v.val1));
    
    public static function operator implicit(v: Vec2ui): Vec2f := new Vec2f(v.val0, v.val1);
    public static function operator implicit(v: Vec2f): Vec2ui := new Vec2ui(Convert.ToUInt32(v.val0), Convert.ToUInt32(v.val1));
    
    public static function operator implicit(v: Vec2i64): Vec2f := new Vec2f(v.val0, v.val1);
    public static function operator implicit(v: Vec2f): Vec2i64 := new Vec2i64(Convert.ToInt64(v.val0), Convert.ToInt64(v.val1));
    
    public static function operator implicit(v: Vec2ui64): Vec2f := new Vec2f(v.val0, v.val1);
    public static function operator implicit(v: Vec2f): Vec2ui64 := new Vec2ui64(Convert.ToUInt64(v.val0), Convert.ToUInt64(v.val1));
    
  end;
  
  Vec2d = record
    public val0: real;
    public val1: real;
    
    public constructor(val0, val1: real);
    begin
      self.val0 := val0;
      self.val1 := val1;
    end;
    
    private function GetValAt(i: integer): real;
    begin
      case i of
        0: Result := self.val0;
        1: Result := self.val1;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..1');
      end;
    end;
    private procedure SetValAt(i: integer; val: real);
    begin
      case i of
        0: self.val0 := val;
        1: self.val1 := val;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..1');
      end;
    end;
    public property val[i: integer]: real read GetValAt write SetValAt; default;
    
    public static function operator-(v: Vec2d): Vec2d := new Vec2d(-v.val0, -v.val1);
    public static function operator*(v: Vec2d; k: real): Vec2d := new Vec2d(v.val0*k, v.val1*k);
    public static function operator+(v1, v2: Vec2d): Vec2d := new Vec2d(v1.val0+v2.val0, v1.val1+v2.val1);
    public static function operator-(v1, v2: Vec2d): Vec2d := new Vec2d(v1.val0-v2.val0, v1.val1-v2.val1);
    
    public static function operator implicit(v: Vec1b): Vec2d := new Vec2d(v.val0, 0);
    public static function operator implicit(v: Vec2d): Vec1b := new Vec1b(Convert.ToSByte(v.val0));
    
    public static function operator implicit(v: Vec1ub): Vec2d := new Vec2d(v.val0, 0);
    public static function operator implicit(v: Vec2d): Vec1ub := new Vec1ub(Convert.ToByte(v.val0));
    
    public static function operator implicit(v: Vec1s): Vec2d := new Vec2d(v.val0, 0);
    public static function operator implicit(v: Vec2d): Vec1s := new Vec1s(Convert.ToInt16(v.val0));
    
    public static function operator implicit(v: Vec1us): Vec2d := new Vec2d(v.val0, 0);
    public static function operator implicit(v: Vec2d): Vec1us := new Vec1us(Convert.ToUInt16(v.val0));
    
    public static function operator implicit(v: Vec1i): Vec2d := new Vec2d(v.val0, 0);
    public static function operator implicit(v: Vec2d): Vec1i := new Vec1i(Convert.ToInt32(v.val0));
    
    public static function operator implicit(v: Vec1ui): Vec2d := new Vec2d(v.val0, 0);
    public static function operator implicit(v: Vec2d): Vec1ui := new Vec1ui(Convert.ToUInt32(v.val0));
    
    public static function operator implicit(v: Vec1i64): Vec2d := new Vec2d(v.val0, 0);
    public static function operator implicit(v: Vec2d): Vec1i64 := new Vec1i64(Convert.ToInt64(v.val0));
    
    public static function operator implicit(v: Vec1ui64): Vec2d := new Vec2d(v.val0, 0);
    public static function operator implicit(v: Vec2d): Vec1ui64 := new Vec1ui64(Convert.ToUInt64(v.val0));
    
    public static function operator implicit(v: Vec1f): Vec2d := new Vec2d(v.val0, 0);
    public static function operator implicit(v: Vec2d): Vec1f := new Vec1f(v.val0);
    
    public static function operator implicit(v: Vec1d): Vec2d := new Vec2d(v.val0, 0);
    public static function operator implicit(v: Vec2d): Vec1d := new Vec1d(v.val0);
    
    public static function operator implicit(v: Vec2b): Vec2d := new Vec2d(v.val0, v.val1);
    public static function operator implicit(v: Vec2d): Vec2b := new Vec2b(Convert.ToSByte(v.val0), Convert.ToSByte(v.val1));
    
    public static function operator implicit(v: Vec2ub): Vec2d := new Vec2d(v.val0, v.val1);
    public static function operator implicit(v: Vec2d): Vec2ub := new Vec2ub(Convert.ToByte(v.val0), Convert.ToByte(v.val1));
    
    public static function operator implicit(v: Vec2s): Vec2d := new Vec2d(v.val0, v.val1);
    public static function operator implicit(v: Vec2d): Vec2s := new Vec2s(Convert.ToInt16(v.val0), Convert.ToInt16(v.val1));
    
    public static function operator implicit(v: Vec2us): Vec2d := new Vec2d(v.val0, v.val1);
    public static function operator implicit(v: Vec2d): Vec2us := new Vec2us(Convert.ToUInt16(v.val0), Convert.ToUInt16(v.val1));
    
    public static function operator implicit(v: Vec2i): Vec2d := new Vec2d(v.val0, v.val1);
    public static function operator implicit(v: Vec2d): Vec2i := new Vec2i(Convert.ToInt32(v.val0), Convert.ToInt32(v.val1));
    
    public static function operator implicit(v: Vec2ui): Vec2d := new Vec2d(v.val0, v.val1);
    public static function operator implicit(v: Vec2d): Vec2ui := new Vec2ui(Convert.ToUInt32(v.val0), Convert.ToUInt32(v.val1));
    
    public static function operator implicit(v: Vec2i64): Vec2d := new Vec2d(v.val0, v.val1);
    public static function operator implicit(v: Vec2d): Vec2i64 := new Vec2i64(Convert.ToInt64(v.val0), Convert.ToInt64(v.val1));
    
    public static function operator implicit(v: Vec2ui64): Vec2d := new Vec2d(v.val0, v.val1);
    public static function operator implicit(v: Vec2d): Vec2ui64 := new Vec2ui64(Convert.ToUInt64(v.val0), Convert.ToUInt64(v.val1));
    
    public static function operator implicit(v: Vec2f): Vec2d := new Vec2d(v.val0, v.val1);
    public static function operator implicit(v: Vec2d): Vec2f := new Vec2f(v.val0, v.val1);
    
  end;
  {$endregion Vec2}
  
  {$region Vec3}
  
  Vec3b = record
    public val0: SByte;
    public val1: SByte;
    public val2: SByte;
    
    public constructor(val0, val1, val2: SByte);
    begin
      self.val0 := val0;
      self.val1 := val1;
      self.val2 := val2;
    end;
    
    private function GetValAt(i: integer): SByte;
    begin
      case i of
        0: Result := self.val0;
        1: Result := self.val1;
        2: Result := self.val2;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..2');
      end;
    end;
    private procedure SetValAt(i: integer; val: SByte);
    begin
      case i of
        0: self.val0 := val;
        1: self.val1 := val;
        2: self.val2 := val;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..2');
      end;
    end;
    public property val[i: integer]: SByte read GetValAt write SetValAt; default;
    
    public static function operator-(v: Vec3b): Vec3b := new Vec3b(-v.val0, -v.val1, -v.val2);
    public static function operator*(v: Vec3b; k: SByte): Vec3b := new Vec3b(v.val0*k, v.val1*k, v.val2*k);
    public static function operator+(v1, v2: Vec3b): Vec3b := new Vec3b(v1.val0+v2.val0, v1.val1+v2.val1, v1.val2+v2.val2);
    public static function operator-(v1, v2: Vec3b): Vec3b := new Vec3b(v1.val0-v2.val0, v1.val1-v2.val1, v1.val2-v2.val2);
    
    public static function operator implicit(v: Vec1b): Vec3b := new Vec3b(v.val0, 0, 0);
    public static function operator implicit(v: Vec3b): Vec1b := new Vec1b(v.val0);
    
    public static function operator implicit(v: Vec1ub): Vec3b := new Vec3b(v.val0, 0, 0);
    public static function operator implicit(v: Vec3b): Vec1ub := new Vec1ub(v.val0);
    
    public static function operator implicit(v: Vec1s): Vec3b := new Vec3b(v.val0, 0, 0);
    public static function operator implicit(v: Vec3b): Vec1s := new Vec1s(v.val0);
    
    public static function operator implicit(v: Vec1us): Vec3b := new Vec3b(v.val0, 0, 0);
    public static function operator implicit(v: Vec3b): Vec1us := new Vec1us(v.val0);
    
    public static function operator implicit(v: Vec1i): Vec3b := new Vec3b(v.val0, 0, 0);
    public static function operator implicit(v: Vec3b): Vec1i := new Vec1i(v.val0);
    
    public static function operator implicit(v: Vec1ui): Vec3b := new Vec3b(v.val0, 0, 0);
    public static function operator implicit(v: Vec3b): Vec1ui := new Vec1ui(v.val0);
    
    public static function operator implicit(v: Vec1i64): Vec3b := new Vec3b(v.val0, 0, 0);
    public static function operator implicit(v: Vec3b): Vec1i64 := new Vec1i64(v.val0);
    
    public static function operator implicit(v: Vec1ui64): Vec3b := new Vec3b(v.val0, 0, 0);
    public static function operator implicit(v: Vec3b): Vec1ui64 := new Vec1ui64(v.val0);
    
    public static function operator implicit(v: Vec1f): Vec3b := new Vec3b(Convert.ToSByte(v.val0), 0, 0);
    public static function operator implicit(v: Vec3b): Vec1f := new Vec1f(v.val0);
    
    public static function operator implicit(v: Vec1d): Vec3b := new Vec3b(Convert.ToSByte(v.val0), 0, 0);
    public static function operator implicit(v: Vec3b): Vec1d := new Vec1d(v.val0);
    
    public static function operator implicit(v: Vec2b): Vec3b := new Vec3b(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3b): Vec2b := new Vec2b(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2ub): Vec3b := new Vec3b(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3b): Vec2ub := new Vec2ub(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2s): Vec3b := new Vec3b(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3b): Vec2s := new Vec2s(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2us): Vec3b := new Vec3b(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3b): Vec2us := new Vec2us(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2i): Vec3b := new Vec3b(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3b): Vec2i := new Vec2i(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2ui): Vec3b := new Vec3b(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3b): Vec2ui := new Vec2ui(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2i64): Vec3b := new Vec3b(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3b): Vec2i64 := new Vec2i64(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2ui64): Vec3b := new Vec3b(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3b): Vec2ui64 := new Vec2ui64(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2f): Vec3b := new Vec3b(Convert.ToSByte(v.val0), Convert.ToSByte(v.val1), 0);
    public static function operator implicit(v: Vec3b): Vec2f := new Vec2f(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2d): Vec3b := new Vec3b(Convert.ToSByte(v.val0), Convert.ToSByte(v.val1), 0);
    public static function operator implicit(v: Vec3b): Vec2d := new Vec2d(v.val0, v.val1);
    
  end;
  
  Vec3ub = record
    public val0: Byte;
    public val1: Byte;
    public val2: Byte;
    
    public constructor(val0, val1, val2: Byte);
    begin
      self.val0 := val0;
      self.val1 := val1;
      self.val2 := val2;
    end;
    
    private function GetValAt(i: integer): Byte;
    begin
      case i of
        0: Result := self.val0;
        1: Result := self.val1;
        2: Result := self.val2;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..2');
      end;
    end;
    private procedure SetValAt(i: integer; val: Byte);
    begin
      case i of
        0: self.val0 := val;
        1: self.val1 := val;
        2: self.val2 := val;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..2');
      end;
    end;
    public property val[i: integer]: Byte read GetValAt write SetValAt; default;
    
    public static function operator*(v: Vec3ub; k: Byte): Vec3ub := new Vec3ub(v.val0*k, v.val1*k, v.val2*k);
    public static function operator+(v1, v2: Vec3ub): Vec3ub := new Vec3ub(v1.val0+v2.val0, v1.val1+v2.val1, v1.val2+v2.val2);
    public static function operator-(v1, v2: Vec3ub): Vec3ub := new Vec3ub(v1.val0-v2.val0, v1.val1-v2.val1, v1.val2-v2.val2);
    
    public static function operator implicit(v: Vec1b): Vec3ub := new Vec3ub(v.val0, 0, 0);
    public static function operator implicit(v: Vec3ub): Vec1b := new Vec1b(v.val0);
    
    public static function operator implicit(v: Vec1ub): Vec3ub := new Vec3ub(v.val0, 0, 0);
    public static function operator implicit(v: Vec3ub): Vec1ub := new Vec1ub(v.val0);
    
    public static function operator implicit(v: Vec1s): Vec3ub := new Vec3ub(v.val0, 0, 0);
    public static function operator implicit(v: Vec3ub): Vec1s := new Vec1s(v.val0);
    
    public static function operator implicit(v: Vec1us): Vec3ub := new Vec3ub(v.val0, 0, 0);
    public static function operator implicit(v: Vec3ub): Vec1us := new Vec1us(v.val0);
    
    public static function operator implicit(v: Vec1i): Vec3ub := new Vec3ub(v.val0, 0, 0);
    public static function operator implicit(v: Vec3ub): Vec1i := new Vec1i(v.val0);
    
    public static function operator implicit(v: Vec1ui): Vec3ub := new Vec3ub(v.val0, 0, 0);
    public static function operator implicit(v: Vec3ub): Vec1ui := new Vec1ui(v.val0);
    
    public static function operator implicit(v: Vec1i64): Vec3ub := new Vec3ub(v.val0, 0, 0);
    public static function operator implicit(v: Vec3ub): Vec1i64 := new Vec1i64(v.val0);
    
    public static function operator implicit(v: Vec1ui64): Vec3ub := new Vec3ub(v.val0, 0, 0);
    public static function operator implicit(v: Vec3ub): Vec1ui64 := new Vec1ui64(v.val0);
    
    public static function operator implicit(v: Vec1f): Vec3ub := new Vec3ub(Convert.ToByte(v.val0), 0, 0);
    public static function operator implicit(v: Vec3ub): Vec1f := new Vec1f(v.val0);
    
    public static function operator implicit(v: Vec1d): Vec3ub := new Vec3ub(Convert.ToByte(v.val0), 0, 0);
    public static function operator implicit(v: Vec3ub): Vec1d := new Vec1d(v.val0);
    
    public static function operator implicit(v: Vec2b): Vec3ub := new Vec3ub(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3ub): Vec2b := new Vec2b(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2ub): Vec3ub := new Vec3ub(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3ub): Vec2ub := new Vec2ub(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2s): Vec3ub := new Vec3ub(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3ub): Vec2s := new Vec2s(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2us): Vec3ub := new Vec3ub(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3ub): Vec2us := new Vec2us(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2i): Vec3ub := new Vec3ub(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3ub): Vec2i := new Vec2i(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2ui): Vec3ub := new Vec3ub(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3ub): Vec2ui := new Vec2ui(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2i64): Vec3ub := new Vec3ub(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3ub): Vec2i64 := new Vec2i64(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2ui64): Vec3ub := new Vec3ub(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3ub): Vec2ui64 := new Vec2ui64(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2f): Vec3ub := new Vec3ub(Convert.ToByte(v.val0), Convert.ToByte(v.val1), 0);
    public static function operator implicit(v: Vec3ub): Vec2f := new Vec2f(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2d): Vec3ub := new Vec3ub(Convert.ToByte(v.val0), Convert.ToByte(v.val1), 0);
    public static function operator implicit(v: Vec3ub): Vec2d := new Vec2d(v.val0, v.val1);
    
    public static function operator implicit(v: Vec3b): Vec3ub := new Vec3ub(v.val0, v.val1, v.val2);
    public static function operator implicit(v: Vec3ub): Vec3b := new Vec3b(v.val0, v.val1, v.val2);
    
  end;
  
  Vec3s = record
    public val0: Int16;
    public val1: Int16;
    public val2: Int16;
    
    public constructor(val0, val1, val2: Int16);
    begin
      self.val0 := val0;
      self.val1 := val1;
      self.val2 := val2;
    end;
    
    private function GetValAt(i: integer): Int16;
    begin
      case i of
        0: Result := self.val0;
        1: Result := self.val1;
        2: Result := self.val2;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..2');
      end;
    end;
    private procedure SetValAt(i: integer; val: Int16);
    begin
      case i of
        0: self.val0 := val;
        1: self.val1 := val;
        2: self.val2 := val;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..2');
      end;
    end;
    public property val[i: integer]: Int16 read GetValAt write SetValAt; default;
    
    public static function operator-(v: Vec3s): Vec3s := new Vec3s(-v.val0, -v.val1, -v.val2);
    public static function operator*(v: Vec3s; k: Int16): Vec3s := new Vec3s(v.val0*k, v.val1*k, v.val2*k);
    public static function operator+(v1, v2: Vec3s): Vec3s := new Vec3s(v1.val0+v2.val0, v1.val1+v2.val1, v1.val2+v2.val2);
    public static function operator-(v1, v2: Vec3s): Vec3s := new Vec3s(v1.val0-v2.val0, v1.val1-v2.val1, v1.val2-v2.val2);
    
    public static function operator implicit(v: Vec1b): Vec3s := new Vec3s(v.val0, 0, 0);
    public static function operator implicit(v: Vec3s): Vec1b := new Vec1b(v.val0);
    
    public static function operator implicit(v: Vec1ub): Vec3s := new Vec3s(v.val0, 0, 0);
    public static function operator implicit(v: Vec3s): Vec1ub := new Vec1ub(v.val0);
    
    public static function operator implicit(v: Vec1s): Vec3s := new Vec3s(v.val0, 0, 0);
    public static function operator implicit(v: Vec3s): Vec1s := new Vec1s(v.val0);
    
    public static function operator implicit(v: Vec1us): Vec3s := new Vec3s(v.val0, 0, 0);
    public static function operator implicit(v: Vec3s): Vec1us := new Vec1us(v.val0);
    
    public static function operator implicit(v: Vec1i): Vec3s := new Vec3s(v.val0, 0, 0);
    public static function operator implicit(v: Vec3s): Vec1i := new Vec1i(v.val0);
    
    public static function operator implicit(v: Vec1ui): Vec3s := new Vec3s(v.val0, 0, 0);
    public static function operator implicit(v: Vec3s): Vec1ui := new Vec1ui(v.val0);
    
    public static function operator implicit(v: Vec1i64): Vec3s := new Vec3s(v.val0, 0, 0);
    public static function operator implicit(v: Vec3s): Vec1i64 := new Vec1i64(v.val0);
    
    public static function operator implicit(v: Vec1ui64): Vec3s := new Vec3s(v.val0, 0, 0);
    public static function operator implicit(v: Vec3s): Vec1ui64 := new Vec1ui64(v.val0);
    
    public static function operator implicit(v: Vec1f): Vec3s := new Vec3s(Convert.ToInt16(v.val0), 0, 0);
    public static function operator implicit(v: Vec3s): Vec1f := new Vec1f(v.val0);
    
    public static function operator implicit(v: Vec1d): Vec3s := new Vec3s(Convert.ToInt16(v.val0), 0, 0);
    public static function operator implicit(v: Vec3s): Vec1d := new Vec1d(v.val0);
    
    public static function operator implicit(v: Vec2b): Vec3s := new Vec3s(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3s): Vec2b := new Vec2b(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2ub): Vec3s := new Vec3s(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3s): Vec2ub := new Vec2ub(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2s): Vec3s := new Vec3s(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3s): Vec2s := new Vec2s(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2us): Vec3s := new Vec3s(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3s): Vec2us := new Vec2us(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2i): Vec3s := new Vec3s(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3s): Vec2i := new Vec2i(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2ui): Vec3s := new Vec3s(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3s): Vec2ui := new Vec2ui(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2i64): Vec3s := new Vec3s(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3s): Vec2i64 := new Vec2i64(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2ui64): Vec3s := new Vec3s(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3s): Vec2ui64 := new Vec2ui64(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2f): Vec3s := new Vec3s(Convert.ToInt16(v.val0), Convert.ToInt16(v.val1), 0);
    public static function operator implicit(v: Vec3s): Vec2f := new Vec2f(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2d): Vec3s := new Vec3s(Convert.ToInt16(v.val0), Convert.ToInt16(v.val1), 0);
    public static function operator implicit(v: Vec3s): Vec2d := new Vec2d(v.val0, v.val1);
    
    public static function operator implicit(v: Vec3b): Vec3s := new Vec3s(v.val0, v.val1, v.val2);
    public static function operator implicit(v: Vec3s): Vec3b := new Vec3b(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3ub): Vec3s := new Vec3s(v.val0, v.val1, v.val2);
    public static function operator implicit(v: Vec3s): Vec3ub := new Vec3ub(v.val0, v.val1, v.val2);
    
  end;
  
  Vec3us = record
    public val0: UInt16;
    public val1: UInt16;
    public val2: UInt16;
    
    public constructor(val0, val1, val2: UInt16);
    begin
      self.val0 := val0;
      self.val1 := val1;
      self.val2 := val2;
    end;
    
    private function GetValAt(i: integer): UInt16;
    begin
      case i of
        0: Result := self.val0;
        1: Result := self.val1;
        2: Result := self.val2;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..2');
      end;
    end;
    private procedure SetValAt(i: integer; val: UInt16);
    begin
      case i of
        0: self.val0 := val;
        1: self.val1 := val;
        2: self.val2 := val;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..2');
      end;
    end;
    public property val[i: integer]: UInt16 read GetValAt write SetValAt; default;
    
    public static function operator*(v: Vec3us; k: UInt16): Vec3us := new Vec3us(v.val0*k, v.val1*k, v.val2*k);
    public static function operator+(v1, v2: Vec3us): Vec3us := new Vec3us(v1.val0+v2.val0, v1.val1+v2.val1, v1.val2+v2.val2);
    public static function operator-(v1, v2: Vec3us): Vec3us := new Vec3us(v1.val0-v2.val0, v1.val1-v2.val1, v1.val2-v2.val2);
    
    public static function operator implicit(v: Vec1b): Vec3us := new Vec3us(v.val0, 0, 0);
    public static function operator implicit(v: Vec3us): Vec1b := new Vec1b(v.val0);
    
    public static function operator implicit(v: Vec1ub): Vec3us := new Vec3us(v.val0, 0, 0);
    public static function operator implicit(v: Vec3us): Vec1ub := new Vec1ub(v.val0);
    
    public static function operator implicit(v: Vec1s): Vec3us := new Vec3us(v.val0, 0, 0);
    public static function operator implicit(v: Vec3us): Vec1s := new Vec1s(v.val0);
    
    public static function operator implicit(v: Vec1us): Vec3us := new Vec3us(v.val0, 0, 0);
    public static function operator implicit(v: Vec3us): Vec1us := new Vec1us(v.val0);
    
    public static function operator implicit(v: Vec1i): Vec3us := new Vec3us(v.val0, 0, 0);
    public static function operator implicit(v: Vec3us): Vec1i := new Vec1i(v.val0);
    
    public static function operator implicit(v: Vec1ui): Vec3us := new Vec3us(v.val0, 0, 0);
    public static function operator implicit(v: Vec3us): Vec1ui := new Vec1ui(v.val0);
    
    public static function operator implicit(v: Vec1i64): Vec3us := new Vec3us(v.val0, 0, 0);
    public static function operator implicit(v: Vec3us): Vec1i64 := new Vec1i64(v.val0);
    
    public static function operator implicit(v: Vec1ui64): Vec3us := new Vec3us(v.val0, 0, 0);
    public static function operator implicit(v: Vec3us): Vec1ui64 := new Vec1ui64(v.val0);
    
    public static function operator implicit(v: Vec1f): Vec3us := new Vec3us(Convert.ToUInt16(v.val0), 0, 0);
    public static function operator implicit(v: Vec3us): Vec1f := new Vec1f(v.val0);
    
    public static function operator implicit(v: Vec1d): Vec3us := new Vec3us(Convert.ToUInt16(v.val0), 0, 0);
    public static function operator implicit(v: Vec3us): Vec1d := new Vec1d(v.val0);
    
    public static function operator implicit(v: Vec2b): Vec3us := new Vec3us(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3us): Vec2b := new Vec2b(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2ub): Vec3us := new Vec3us(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3us): Vec2ub := new Vec2ub(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2s): Vec3us := new Vec3us(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3us): Vec2s := new Vec2s(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2us): Vec3us := new Vec3us(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3us): Vec2us := new Vec2us(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2i): Vec3us := new Vec3us(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3us): Vec2i := new Vec2i(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2ui): Vec3us := new Vec3us(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3us): Vec2ui := new Vec2ui(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2i64): Vec3us := new Vec3us(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3us): Vec2i64 := new Vec2i64(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2ui64): Vec3us := new Vec3us(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3us): Vec2ui64 := new Vec2ui64(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2f): Vec3us := new Vec3us(Convert.ToUInt16(v.val0), Convert.ToUInt16(v.val1), 0);
    public static function operator implicit(v: Vec3us): Vec2f := new Vec2f(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2d): Vec3us := new Vec3us(Convert.ToUInt16(v.val0), Convert.ToUInt16(v.val1), 0);
    public static function operator implicit(v: Vec3us): Vec2d := new Vec2d(v.val0, v.val1);
    
    public static function operator implicit(v: Vec3b): Vec3us := new Vec3us(v.val0, v.val1, v.val2);
    public static function operator implicit(v: Vec3us): Vec3b := new Vec3b(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3ub): Vec3us := new Vec3us(v.val0, v.val1, v.val2);
    public static function operator implicit(v: Vec3us): Vec3ub := new Vec3ub(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3s): Vec3us := new Vec3us(v.val0, v.val1, v.val2);
    public static function operator implicit(v: Vec3us): Vec3s := new Vec3s(v.val0, v.val1, v.val2);
    
  end;
  
  Vec3i = record
    public val0: Int32;
    public val1: Int32;
    public val2: Int32;
    
    public constructor(val0, val1, val2: Int32);
    begin
      self.val0 := val0;
      self.val1 := val1;
      self.val2 := val2;
    end;
    
    private function GetValAt(i: integer): Int32;
    begin
      case i of
        0: Result := self.val0;
        1: Result := self.val1;
        2: Result := self.val2;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..2');
      end;
    end;
    private procedure SetValAt(i: integer; val: Int32);
    begin
      case i of
        0: self.val0 := val;
        1: self.val1 := val;
        2: self.val2 := val;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..2');
      end;
    end;
    public property val[i: integer]: Int32 read GetValAt write SetValAt; default;
    
    public static function operator-(v: Vec3i): Vec3i := new Vec3i(-v.val0, -v.val1, -v.val2);
    public static function operator*(v: Vec3i; k: Int32): Vec3i := new Vec3i(v.val0*k, v.val1*k, v.val2*k);
    public static function operator+(v1, v2: Vec3i): Vec3i := new Vec3i(v1.val0+v2.val0, v1.val1+v2.val1, v1.val2+v2.val2);
    public static function operator-(v1, v2: Vec3i): Vec3i := new Vec3i(v1.val0-v2.val0, v1.val1-v2.val1, v1.val2-v2.val2);
    
    public static function operator implicit(v: Vec1b): Vec3i := new Vec3i(v.val0, 0, 0);
    public static function operator implicit(v: Vec3i): Vec1b := new Vec1b(v.val0);
    
    public static function operator implicit(v: Vec1ub): Vec3i := new Vec3i(v.val0, 0, 0);
    public static function operator implicit(v: Vec3i): Vec1ub := new Vec1ub(v.val0);
    
    public static function operator implicit(v: Vec1s): Vec3i := new Vec3i(v.val0, 0, 0);
    public static function operator implicit(v: Vec3i): Vec1s := new Vec1s(v.val0);
    
    public static function operator implicit(v: Vec1us): Vec3i := new Vec3i(v.val0, 0, 0);
    public static function operator implicit(v: Vec3i): Vec1us := new Vec1us(v.val0);
    
    public static function operator implicit(v: Vec1i): Vec3i := new Vec3i(v.val0, 0, 0);
    public static function operator implicit(v: Vec3i): Vec1i := new Vec1i(v.val0);
    
    public static function operator implicit(v: Vec1ui): Vec3i := new Vec3i(v.val0, 0, 0);
    public static function operator implicit(v: Vec3i): Vec1ui := new Vec1ui(v.val0);
    
    public static function operator implicit(v: Vec1i64): Vec3i := new Vec3i(v.val0, 0, 0);
    public static function operator implicit(v: Vec3i): Vec1i64 := new Vec1i64(v.val0);
    
    public static function operator implicit(v: Vec1ui64): Vec3i := new Vec3i(v.val0, 0, 0);
    public static function operator implicit(v: Vec3i): Vec1ui64 := new Vec1ui64(v.val0);
    
    public static function operator implicit(v: Vec1f): Vec3i := new Vec3i(Convert.ToInt32(v.val0), 0, 0);
    public static function operator implicit(v: Vec3i): Vec1f := new Vec1f(v.val0);
    
    public static function operator implicit(v: Vec1d): Vec3i := new Vec3i(Convert.ToInt32(v.val0), 0, 0);
    public static function operator implicit(v: Vec3i): Vec1d := new Vec1d(v.val0);
    
    public static function operator implicit(v: Vec2b): Vec3i := new Vec3i(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3i): Vec2b := new Vec2b(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2ub): Vec3i := new Vec3i(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3i): Vec2ub := new Vec2ub(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2s): Vec3i := new Vec3i(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3i): Vec2s := new Vec2s(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2us): Vec3i := new Vec3i(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3i): Vec2us := new Vec2us(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2i): Vec3i := new Vec3i(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3i): Vec2i := new Vec2i(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2ui): Vec3i := new Vec3i(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3i): Vec2ui := new Vec2ui(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2i64): Vec3i := new Vec3i(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3i): Vec2i64 := new Vec2i64(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2ui64): Vec3i := new Vec3i(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3i): Vec2ui64 := new Vec2ui64(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2f): Vec3i := new Vec3i(Convert.ToInt32(v.val0), Convert.ToInt32(v.val1), 0);
    public static function operator implicit(v: Vec3i): Vec2f := new Vec2f(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2d): Vec3i := new Vec3i(Convert.ToInt32(v.val0), Convert.ToInt32(v.val1), 0);
    public static function operator implicit(v: Vec3i): Vec2d := new Vec2d(v.val0, v.val1);
    
    public static function operator implicit(v: Vec3b): Vec3i := new Vec3i(v.val0, v.val1, v.val2);
    public static function operator implicit(v: Vec3i): Vec3b := new Vec3b(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3ub): Vec3i := new Vec3i(v.val0, v.val1, v.val2);
    public static function operator implicit(v: Vec3i): Vec3ub := new Vec3ub(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3s): Vec3i := new Vec3i(v.val0, v.val1, v.val2);
    public static function operator implicit(v: Vec3i): Vec3s := new Vec3s(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3us): Vec3i := new Vec3i(v.val0, v.val1, v.val2);
    public static function operator implicit(v: Vec3i): Vec3us := new Vec3us(v.val0, v.val1, v.val2);
    
  end;
  
  Vec3ui = record
    public val0: UInt32;
    public val1: UInt32;
    public val2: UInt32;
    
    public constructor(val0, val1, val2: UInt32);
    begin
      self.val0 := val0;
      self.val1 := val1;
      self.val2 := val2;
    end;
    
    private function GetValAt(i: integer): UInt32;
    begin
      case i of
        0: Result := self.val0;
        1: Result := self.val1;
        2: Result := self.val2;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..2');
      end;
    end;
    private procedure SetValAt(i: integer; val: UInt32);
    begin
      case i of
        0: self.val0 := val;
        1: self.val1 := val;
        2: self.val2 := val;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..2');
      end;
    end;
    public property val[i: integer]: UInt32 read GetValAt write SetValAt; default;
    
    public static function operator*(v: Vec3ui; k: UInt32): Vec3ui := new Vec3ui(v.val0*k, v.val1*k, v.val2*k);
    public static function operator+(v1, v2: Vec3ui): Vec3ui := new Vec3ui(v1.val0+v2.val0, v1.val1+v2.val1, v1.val2+v2.val2);
    public static function operator-(v1, v2: Vec3ui): Vec3ui := new Vec3ui(v1.val0-v2.val0, v1.val1-v2.val1, v1.val2-v2.val2);
    
    public static function operator implicit(v: Vec1b): Vec3ui := new Vec3ui(v.val0, 0, 0);
    public static function operator implicit(v: Vec3ui): Vec1b := new Vec1b(v.val0);
    
    public static function operator implicit(v: Vec1ub): Vec3ui := new Vec3ui(v.val0, 0, 0);
    public static function operator implicit(v: Vec3ui): Vec1ub := new Vec1ub(v.val0);
    
    public static function operator implicit(v: Vec1s): Vec3ui := new Vec3ui(v.val0, 0, 0);
    public static function operator implicit(v: Vec3ui): Vec1s := new Vec1s(v.val0);
    
    public static function operator implicit(v: Vec1us): Vec3ui := new Vec3ui(v.val0, 0, 0);
    public static function operator implicit(v: Vec3ui): Vec1us := new Vec1us(v.val0);
    
    public static function operator implicit(v: Vec1i): Vec3ui := new Vec3ui(v.val0, 0, 0);
    public static function operator implicit(v: Vec3ui): Vec1i := new Vec1i(v.val0);
    
    public static function operator implicit(v: Vec1ui): Vec3ui := new Vec3ui(v.val0, 0, 0);
    public static function operator implicit(v: Vec3ui): Vec1ui := new Vec1ui(v.val0);
    
    public static function operator implicit(v: Vec1i64): Vec3ui := new Vec3ui(v.val0, 0, 0);
    public static function operator implicit(v: Vec3ui): Vec1i64 := new Vec1i64(v.val0);
    
    public static function operator implicit(v: Vec1ui64): Vec3ui := new Vec3ui(v.val0, 0, 0);
    public static function operator implicit(v: Vec3ui): Vec1ui64 := new Vec1ui64(v.val0);
    
    public static function operator implicit(v: Vec1f): Vec3ui := new Vec3ui(Convert.ToUInt32(v.val0), 0, 0);
    public static function operator implicit(v: Vec3ui): Vec1f := new Vec1f(v.val0);
    
    public static function operator implicit(v: Vec1d): Vec3ui := new Vec3ui(Convert.ToUInt32(v.val0), 0, 0);
    public static function operator implicit(v: Vec3ui): Vec1d := new Vec1d(v.val0);
    
    public static function operator implicit(v: Vec2b): Vec3ui := new Vec3ui(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3ui): Vec2b := new Vec2b(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2ub): Vec3ui := new Vec3ui(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3ui): Vec2ub := new Vec2ub(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2s): Vec3ui := new Vec3ui(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3ui): Vec2s := new Vec2s(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2us): Vec3ui := new Vec3ui(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3ui): Vec2us := new Vec2us(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2i): Vec3ui := new Vec3ui(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3ui): Vec2i := new Vec2i(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2ui): Vec3ui := new Vec3ui(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3ui): Vec2ui := new Vec2ui(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2i64): Vec3ui := new Vec3ui(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3ui): Vec2i64 := new Vec2i64(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2ui64): Vec3ui := new Vec3ui(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3ui): Vec2ui64 := new Vec2ui64(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2f): Vec3ui := new Vec3ui(Convert.ToUInt32(v.val0), Convert.ToUInt32(v.val1), 0);
    public static function operator implicit(v: Vec3ui): Vec2f := new Vec2f(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2d): Vec3ui := new Vec3ui(Convert.ToUInt32(v.val0), Convert.ToUInt32(v.val1), 0);
    public static function operator implicit(v: Vec3ui): Vec2d := new Vec2d(v.val0, v.val1);
    
    public static function operator implicit(v: Vec3b): Vec3ui := new Vec3ui(v.val0, v.val1, v.val2);
    public static function operator implicit(v: Vec3ui): Vec3b := new Vec3b(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3ub): Vec3ui := new Vec3ui(v.val0, v.val1, v.val2);
    public static function operator implicit(v: Vec3ui): Vec3ub := new Vec3ub(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3s): Vec3ui := new Vec3ui(v.val0, v.val1, v.val2);
    public static function operator implicit(v: Vec3ui): Vec3s := new Vec3s(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3us): Vec3ui := new Vec3ui(v.val0, v.val1, v.val2);
    public static function operator implicit(v: Vec3ui): Vec3us := new Vec3us(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3i): Vec3ui := new Vec3ui(v.val0, v.val1, v.val2);
    public static function operator implicit(v: Vec3ui): Vec3i := new Vec3i(v.val0, v.val1, v.val2);
    
  end;
  
  Vec3i64 = record
    public val0: Int64;
    public val1: Int64;
    public val2: Int64;
    
    public constructor(val0, val1, val2: Int64);
    begin
      self.val0 := val0;
      self.val1 := val1;
      self.val2 := val2;
    end;
    
    private function GetValAt(i: integer): Int64;
    begin
      case i of
        0: Result := self.val0;
        1: Result := self.val1;
        2: Result := self.val2;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..2');
      end;
    end;
    private procedure SetValAt(i: integer; val: Int64);
    begin
      case i of
        0: self.val0 := val;
        1: self.val1 := val;
        2: self.val2 := val;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..2');
      end;
    end;
    public property val[i: integer]: Int64 read GetValAt write SetValAt; default;
    
    public static function operator-(v: Vec3i64): Vec3i64 := new Vec3i64(-v.val0, -v.val1, -v.val2);
    public static function operator*(v: Vec3i64; k: Int64): Vec3i64 := new Vec3i64(v.val0*k, v.val1*k, v.val2*k);
    public static function operator+(v1, v2: Vec3i64): Vec3i64 := new Vec3i64(v1.val0+v2.val0, v1.val1+v2.val1, v1.val2+v2.val2);
    public static function operator-(v1, v2: Vec3i64): Vec3i64 := new Vec3i64(v1.val0-v2.val0, v1.val1-v2.val1, v1.val2-v2.val2);
    
    public static function operator implicit(v: Vec1b): Vec3i64 := new Vec3i64(v.val0, 0, 0);
    public static function operator implicit(v: Vec3i64): Vec1b := new Vec1b(v.val0);
    
    public static function operator implicit(v: Vec1ub): Vec3i64 := new Vec3i64(v.val0, 0, 0);
    public static function operator implicit(v: Vec3i64): Vec1ub := new Vec1ub(v.val0);
    
    public static function operator implicit(v: Vec1s): Vec3i64 := new Vec3i64(v.val0, 0, 0);
    public static function operator implicit(v: Vec3i64): Vec1s := new Vec1s(v.val0);
    
    public static function operator implicit(v: Vec1us): Vec3i64 := new Vec3i64(v.val0, 0, 0);
    public static function operator implicit(v: Vec3i64): Vec1us := new Vec1us(v.val0);
    
    public static function operator implicit(v: Vec1i): Vec3i64 := new Vec3i64(v.val0, 0, 0);
    public static function operator implicit(v: Vec3i64): Vec1i := new Vec1i(v.val0);
    
    public static function operator implicit(v: Vec1ui): Vec3i64 := new Vec3i64(v.val0, 0, 0);
    public static function operator implicit(v: Vec3i64): Vec1ui := new Vec1ui(v.val0);
    
    public static function operator implicit(v: Vec1i64): Vec3i64 := new Vec3i64(v.val0, 0, 0);
    public static function operator implicit(v: Vec3i64): Vec1i64 := new Vec1i64(v.val0);
    
    public static function operator implicit(v: Vec1ui64): Vec3i64 := new Vec3i64(v.val0, 0, 0);
    public static function operator implicit(v: Vec3i64): Vec1ui64 := new Vec1ui64(v.val0);
    
    public static function operator implicit(v: Vec1f): Vec3i64 := new Vec3i64(Convert.ToInt64(v.val0), 0, 0);
    public static function operator implicit(v: Vec3i64): Vec1f := new Vec1f(v.val0);
    
    public static function operator implicit(v: Vec1d): Vec3i64 := new Vec3i64(Convert.ToInt64(v.val0), 0, 0);
    public static function operator implicit(v: Vec3i64): Vec1d := new Vec1d(v.val0);
    
    public static function operator implicit(v: Vec2b): Vec3i64 := new Vec3i64(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3i64): Vec2b := new Vec2b(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2ub): Vec3i64 := new Vec3i64(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3i64): Vec2ub := new Vec2ub(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2s): Vec3i64 := new Vec3i64(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3i64): Vec2s := new Vec2s(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2us): Vec3i64 := new Vec3i64(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3i64): Vec2us := new Vec2us(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2i): Vec3i64 := new Vec3i64(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3i64): Vec2i := new Vec2i(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2ui): Vec3i64 := new Vec3i64(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3i64): Vec2ui := new Vec2ui(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2i64): Vec3i64 := new Vec3i64(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3i64): Vec2i64 := new Vec2i64(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2ui64): Vec3i64 := new Vec3i64(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3i64): Vec2ui64 := new Vec2ui64(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2f): Vec3i64 := new Vec3i64(Convert.ToInt64(v.val0), Convert.ToInt64(v.val1), 0);
    public static function operator implicit(v: Vec3i64): Vec2f := new Vec2f(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2d): Vec3i64 := new Vec3i64(Convert.ToInt64(v.val0), Convert.ToInt64(v.val1), 0);
    public static function operator implicit(v: Vec3i64): Vec2d := new Vec2d(v.val0, v.val1);
    
    public static function operator implicit(v: Vec3b): Vec3i64 := new Vec3i64(v.val0, v.val1, v.val2);
    public static function operator implicit(v: Vec3i64): Vec3b := new Vec3b(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3ub): Vec3i64 := new Vec3i64(v.val0, v.val1, v.val2);
    public static function operator implicit(v: Vec3i64): Vec3ub := new Vec3ub(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3s): Vec3i64 := new Vec3i64(v.val0, v.val1, v.val2);
    public static function operator implicit(v: Vec3i64): Vec3s := new Vec3s(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3us): Vec3i64 := new Vec3i64(v.val0, v.val1, v.val2);
    public static function operator implicit(v: Vec3i64): Vec3us := new Vec3us(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3i): Vec3i64 := new Vec3i64(v.val0, v.val1, v.val2);
    public static function operator implicit(v: Vec3i64): Vec3i := new Vec3i(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3ui): Vec3i64 := new Vec3i64(v.val0, v.val1, v.val2);
    public static function operator implicit(v: Vec3i64): Vec3ui := new Vec3ui(v.val0, v.val1, v.val2);
    
  end;
  
  Vec3ui64 = record
    public val0: UInt64;
    public val1: UInt64;
    public val2: UInt64;
    
    public constructor(val0, val1, val2: UInt64);
    begin
      self.val0 := val0;
      self.val1 := val1;
      self.val2 := val2;
    end;
    
    private function GetValAt(i: integer): UInt64;
    begin
      case i of
        0: Result := self.val0;
        1: Result := self.val1;
        2: Result := self.val2;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..2');
      end;
    end;
    private procedure SetValAt(i: integer; val: UInt64);
    begin
      case i of
        0: self.val0 := val;
        1: self.val1 := val;
        2: self.val2 := val;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..2');
      end;
    end;
    public property val[i: integer]: UInt64 read GetValAt write SetValAt; default;
    
    public static function operator*(v: Vec3ui64; k: UInt64): Vec3ui64 := new Vec3ui64(v.val0*k, v.val1*k, v.val2*k);
    public static function operator+(v1, v2: Vec3ui64): Vec3ui64 := new Vec3ui64(v1.val0+v2.val0, v1.val1+v2.val1, v1.val2+v2.val2);
    public static function operator-(v1, v2: Vec3ui64): Vec3ui64 := new Vec3ui64(v1.val0-v2.val0, v1.val1-v2.val1, v1.val2-v2.val2);
    
    public static function operator implicit(v: Vec1b): Vec3ui64 := new Vec3ui64(v.val0, 0, 0);
    public static function operator implicit(v: Vec3ui64): Vec1b := new Vec1b(v.val0);
    
    public static function operator implicit(v: Vec1ub): Vec3ui64 := new Vec3ui64(v.val0, 0, 0);
    public static function operator implicit(v: Vec3ui64): Vec1ub := new Vec1ub(v.val0);
    
    public static function operator implicit(v: Vec1s): Vec3ui64 := new Vec3ui64(v.val0, 0, 0);
    public static function operator implicit(v: Vec3ui64): Vec1s := new Vec1s(v.val0);
    
    public static function operator implicit(v: Vec1us): Vec3ui64 := new Vec3ui64(v.val0, 0, 0);
    public static function operator implicit(v: Vec3ui64): Vec1us := new Vec1us(v.val0);
    
    public static function operator implicit(v: Vec1i): Vec3ui64 := new Vec3ui64(v.val0, 0, 0);
    public static function operator implicit(v: Vec3ui64): Vec1i := new Vec1i(v.val0);
    
    public static function operator implicit(v: Vec1ui): Vec3ui64 := new Vec3ui64(v.val0, 0, 0);
    public static function operator implicit(v: Vec3ui64): Vec1ui := new Vec1ui(v.val0);
    
    public static function operator implicit(v: Vec1i64): Vec3ui64 := new Vec3ui64(v.val0, 0, 0);
    public static function operator implicit(v: Vec3ui64): Vec1i64 := new Vec1i64(v.val0);
    
    public static function operator implicit(v: Vec1ui64): Vec3ui64 := new Vec3ui64(v.val0, 0, 0);
    public static function operator implicit(v: Vec3ui64): Vec1ui64 := new Vec1ui64(v.val0);
    
    public static function operator implicit(v: Vec1f): Vec3ui64 := new Vec3ui64(Convert.ToUInt64(v.val0), 0, 0);
    public static function operator implicit(v: Vec3ui64): Vec1f := new Vec1f(v.val0);
    
    public static function operator implicit(v: Vec1d): Vec3ui64 := new Vec3ui64(Convert.ToUInt64(v.val0), 0, 0);
    public static function operator implicit(v: Vec3ui64): Vec1d := new Vec1d(v.val0);
    
    public static function operator implicit(v: Vec2b): Vec3ui64 := new Vec3ui64(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3ui64): Vec2b := new Vec2b(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2ub): Vec3ui64 := new Vec3ui64(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3ui64): Vec2ub := new Vec2ub(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2s): Vec3ui64 := new Vec3ui64(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3ui64): Vec2s := new Vec2s(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2us): Vec3ui64 := new Vec3ui64(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3ui64): Vec2us := new Vec2us(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2i): Vec3ui64 := new Vec3ui64(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3ui64): Vec2i := new Vec2i(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2ui): Vec3ui64 := new Vec3ui64(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3ui64): Vec2ui := new Vec2ui(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2i64): Vec3ui64 := new Vec3ui64(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3ui64): Vec2i64 := new Vec2i64(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2ui64): Vec3ui64 := new Vec3ui64(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3ui64): Vec2ui64 := new Vec2ui64(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2f): Vec3ui64 := new Vec3ui64(Convert.ToUInt64(v.val0), Convert.ToUInt64(v.val1), 0);
    public static function operator implicit(v: Vec3ui64): Vec2f := new Vec2f(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2d): Vec3ui64 := new Vec3ui64(Convert.ToUInt64(v.val0), Convert.ToUInt64(v.val1), 0);
    public static function operator implicit(v: Vec3ui64): Vec2d := new Vec2d(v.val0, v.val1);
    
    public static function operator implicit(v: Vec3b): Vec3ui64 := new Vec3ui64(v.val0, v.val1, v.val2);
    public static function operator implicit(v: Vec3ui64): Vec3b := new Vec3b(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3ub): Vec3ui64 := new Vec3ui64(v.val0, v.val1, v.val2);
    public static function operator implicit(v: Vec3ui64): Vec3ub := new Vec3ub(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3s): Vec3ui64 := new Vec3ui64(v.val0, v.val1, v.val2);
    public static function operator implicit(v: Vec3ui64): Vec3s := new Vec3s(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3us): Vec3ui64 := new Vec3ui64(v.val0, v.val1, v.val2);
    public static function operator implicit(v: Vec3ui64): Vec3us := new Vec3us(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3i): Vec3ui64 := new Vec3ui64(v.val0, v.val1, v.val2);
    public static function operator implicit(v: Vec3ui64): Vec3i := new Vec3i(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3ui): Vec3ui64 := new Vec3ui64(v.val0, v.val1, v.val2);
    public static function operator implicit(v: Vec3ui64): Vec3ui := new Vec3ui(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3i64): Vec3ui64 := new Vec3ui64(v.val0, v.val1, v.val2);
    public static function operator implicit(v: Vec3ui64): Vec3i64 := new Vec3i64(v.val0, v.val1, v.val2);
    
  end;
  
  Vec3f = record
    public val0: single;
    public val1: single;
    public val2: single;
    
    public constructor(val0, val1, val2: single);
    begin
      self.val0 := val0;
      self.val1 := val1;
      self.val2 := val2;
    end;
    
    private function GetValAt(i: integer): single;
    begin
      case i of
        0: Result := self.val0;
        1: Result := self.val1;
        2: Result := self.val2;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..2');
      end;
    end;
    private procedure SetValAt(i: integer; val: single);
    begin
      case i of
        0: self.val0 := val;
        1: self.val1 := val;
        2: self.val2 := val;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..2');
      end;
    end;
    public property val[i: integer]: single read GetValAt write SetValAt; default;
    
    public static function operator-(v: Vec3f): Vec3f := new Vec3f(-v.val0, -v.val1, -v.val2);
    public static function operator*(v: Vec3f; k: single): Vec3f := new Vec3f(v.val0*k, v.val1*k, v.val2*k);
    public static function operator+(v1, v2: Vec3f): Vec3f := new Vec3f(v1.val0+v2.val0, v1.val1+v2.val1, v1.val2+v2.val2);
    public static function operator-(v1, v2: Vec3f): Vec3f := new Vec3f(v1.val0-v2.val0, v1.val1-v2.val1, v1.val2-v2.val2);
    
    public static function operator implicit(v: Vec1b): Vec3f := new Vec3f(v.val0, 0, 0);
    public static function operator implicit(v: Vec3f): Vec1b := new Vec1b(Convert.ToSByte(v.val0));
    
    public static function operator implicit(v: Vec1ub): Vec3f := new Vec3f(v.val0, 0, 0);
    public static function operator implicit(v: Vec3f): Vec1ub := new Vec1ub(Convert.ToByte(v.val0));
    
    public static function operator implicit(v: Vec1s): Vec3f := new Vec3f(v.val0, 0, 0);
    public static function operator implicit(v: Vec3f): Vec1s := new Vec1s(Convert.ToInt16(v.val0));
    
    public static function operator implicit(v: Vec1us): Vec3f := new Vec3f(v.val0, 0, 0);
    public static function operator implicit(v: Vec3f): Vec1us := new Vec1us(Convert.ToUInt16(v.val0));
    
    public static function operator implicit(v: Vec1i): Vec3f := new Vec3f(v.val0, 0, 0);
    public static function operator implicit(v: Vec3f): Vec1i := new Vec1i(Convert.ToInt32(v.val0));
    
    public static function operator implicit(v: Vec1ui): Vec3f := new Vec3f(v.val0, 0, 0);
    public static function operator implicit(v: Vec3f): Vec1ui := new Vec1ui(Convert.ToUInt32(v.val0));
    
    public static function operator implicit(v: Vec1i64): Vec3f := new Vec3f(v.val0, 0, 0);
    public static function operator implicit(v: Vec3f): Vec1i64 := new Vec1i64(Convert.ToInt64(v.val0));
    
    public static function operator implicit(v: Vec1ui64): Vec3f := new Vec3f(v.val0, 0, 0);
    public static function operator implicit(v: Vec3f): Vec1ui64 := new Vec1ui64(Convert.ToUInt64(v.val0));
    
    public static function operator implicit(v: Vec1f): Vec3f := new Vec3f(v.val0, 0, 0);
    public static function operator implicit(v: Vec3f): Vec1f := new Vec1f(v.val0);
    
    public static function operator implicit(v: Vec1d): Vec3f := new Vec3f(v.val0, 0, 0);
    public static function operator implicit(v: Vec3f): Vec1d := new Vec1d(v.val0);
    
    public static function operator implicit(v: Vec2b): Vec3f := new Vec3f(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3f): Vec2b := new Vec2b(Convert.ToSByte(v.val0), Convert.ToSByte(v.val1));
    
    public static function operator implicit(v: Vec2ub): Vec3f := new Vec3f(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3f): Vec2ub := new Vec2ub(Convert.ToByte(v.val0), Convert.ToByte(v.val1));
    
    public static function operator implicit(v: Vec2s): Vec3f := new Vec3f(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3f): Vec2s := new Vec2s(Convert.ToInt16(v.val0), Convert.ToInt16(v.val1));
    
    public static function operator implicit(v: Vec2us): Vec3f := new Vec3f(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3f): Vec2us := new Vec2us(Convert.ToUInt16(v.val0), Convert.ToUInt16(v.val1));
    
    public static function operator implicit(v: Vec2i): Vec3f := new Vec3f(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3f): Vec2i := new Vec2i(Convert.ToInt32(v.val0), Convert.ToInt32(v.val1));
    
    public static function operator implicit(v: Vec2ui): Vec3f := new Vec3f(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3f): Vec2ui := new Vec2ui(Convert.ToUInt32(v.val0), Convert.ToUInt32(v.val1));
    
    public static function operator implicit(v: Vec2i64): Vec3f := new Vec3f(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3f): Vec2i64 := new Vec2i64(Convert.ToInt64(v.val0), Convert.ToInt64(v.val1));
    
    public static function operator implicit(v: Vec2ui64): Vec3f := new Vec3f(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3f): Vec2ui64 := new Vec2ui64(Convert.ToUInt64(v.val0), Convert.ToUInt64(v.val1));
    
    public static function operator implicit(v: Vec2f): Vec3f := new Vec3f(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3f): Vec2f := new Vec2f(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2d): Vec3f := new Vec3f(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3f): Vec2d := new Vec2d(v.val0, v.val1);
    
    public static function operator implicit(v: Vec3b): Vec3f := new Vec3f(v.val0, v.val1, v.val2);
    public static function operator implicit(v: Vec3f): Vec3b := new Vec3b(Convert.ToSByte(v.val0), Convert.ToSByte(v.val1), Convert.ToSByte(v.val2));
    
    public static function operator implicit(v: Vec3ub): Vec3f := new Vec3f(v.val0, v.val1, v.val2);
    public static function operator implicit(v: Vec3f): Vec3ub := new Vec3ub(Convert.ToByte(v.val0), Convert.ToByte(v.val1), Convert.ToByte(v.val2));
    
    public static function operator implicit(v: Vec3s): Vec3f := new Vec3f(v.val0, v.val1, v.val2);
    public static function operator implicit(v: Vec3f): Vec3s := new Vec3s(Convert.ToInt16(v.val0), Convert.ToInt16(v.val1), Convert.ToInt16(v.val2));
    
    public static function operator implicit(v: Vec3us): Vec3f := new Vec3f(v.val0, v.val1, v.val2);
    public static function operator implicit(v: Vec3f): Vec3us := new Vec3us(Convert.ToUInt16(v.val0), Convert.ToUInt16(v.val1), Convert.ToUInt16(v.val2));
    
    public static function operator implicit(v: Vec3i): Vec3f := new Vec3f(v.val0, v.val1, v.val2);
    public static function operator implicit(v: Vec3f): Vec3i := new Vec3i(Convert.ToInt32(v.val0), Convert.ToInt32(v.val1), Convert.ToInt32(v.val2));
    
    public static function operator implicit(v: Vec3ui): Vec3f := new Vec3f(v.val0, v.val1, v.val2);
    public static function operator implicit(v: Vec3f): Vec3ui := new Vec3ui(Convert.ToUInt32(v.val0), Convert.ToUInt32(v.val1), Convert.ToUInt32(v.val2));
    
    public static function operator implicit(v: Vec3i64): Vec3f := new Vec3f(v.val0, v.val1, v.val2);
    public static function operator implicit(v: Vec3f): Vec3i64 := new Vec3i64(Convert.ToInt64(v.val0), Convert.ToInt64(v.val1), Convert.ToInt64(v.val2));
    
    public static function operator implicit(v: Vec3ui64): Vec3f := new Vec3f(v.val0, v.val1, v.val2);
    public static function operator implicit(v: Vec3f): Vec3ui64 := new Vec3ui64(Convert.ToUInt64(v.val0), Convert.ToUInt64(v.val1), Convert.ToUInt64(v.val2));
    
  end;
  
  Vec3d = record
    public val0: real;
    public val1: real;
    public val2: real;
    
    public constructor(val0, val1, val2: real);
    begin
      self.val0 := val0;
      self.val1 := val1;
      self.val2 := val2;
    end;
    
    private function GetValAt(i: integer): real;
    begin
      case i of
        0: Result := self.val0;
        1: Result := self.val1;
        2: Result := self.val2;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..2');
      end;
    end;
    private procedure SetValAt(i: integer; val: real);
    begin
      case i of
        0: self.val0 := val;
        1: self.val1 := val;
        2: self.val2 := val;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..2');
      end;
    end;
    public property val[i: integer]: real read GetValAt write SetValAt; default;
    
    public static function operator-(v: Vec3d): Vec3d := new Vec3d(-v.val0, -v.val1, -v.val2);
    public static function operator*(v: Vec3d; k: real): Vec3d := new Vec3d(v.val0*k, v.val1*k, v.val2*k);
    public static function operator+(v1, v2: Vec3d): Vec3d := new Vec3d(v1.val0+v2.val0, v1.val1+v2.val1, v1.val2+v2.val2);
    public static function operator-(v1, v2: Vec3d): Vec3d := new Vec3d(v1.val0-v2.val0, v1.val1-v2.val1, v1.val2-v2.val2);
    
    public static function operator implicit(v: Vec1b): Vec3d := new Vec3d(v.val0, 0, 0);
    public static function operator implicit(v: Vec3d): Vec1b := new Vec1b(Convert.ToSByte(v.val0));
    
    public static function operator implicit(v: Vec1ub): Vec3d := new Vec3d(v.val0, 0, 0);
    public static function operator implicit(v: Vec3d): Vec1ub := new Vec1ub(Convert.ToByte(v.val0));
    
    public static function operator implicit(v: Vec1s): Vec3d := new Vec3d(v.val0, 0, 0);
    public static function operator implicit(v: Vec3d): Vec1s := new Vec1s(Convert.ToInt16(v.val0));
    
    public static function operator implicit(v: Vec1us): Vec3d := new Vec3d(v.val0, 0, 0);
    public static function operator implicit(v: Vec3d): Vec1us := new Vec1us(Convert.ToUInt16(v.val0));
    
    public static function operator implicit(v: Vec1i): Vec3d := new Vec3d(v.val0, 0, 0);
    public static function operator implicit(v: Vec3d): Vec1i := new Vec1i(Convert.ToInt32(v.val0));
    
    public static function operator implicit(v: Vec1ui): Vec3d := new Vec3d(v.val0, 0, 0);
    public static function operator implicit(v: Vec3d): Vec1ui := new Vec1ui(Convert.ToUInt32(v.val0));
    
    public static function operator implicit(v: Vec1i64): Vec3d := new Vec3d(v.val0, 0, 0);
    public static function operator implicit(v: Vec3d): Vec1i64 := new Vec1i64(Convert.ToInt64(v.val0));
    
    public static function operator implicit(v: Vec1ui64): Vec3d := new Vec3d(v.val0, 0, 0);
    public static function operator implicit(v: Vec3d): Vec1ui64 := new Vec1ui64(Convert.ToUInt64(v.val0));
    
    public static function operator implicit(v: Vec1f): Vec3d := new Vec3d(v.val0, 0, 0);
    public static function operator implicit(v: Vec3d): Vec1f := new Vec1f(v.val0);
    
    public static function operator implicit(v: Vec1d): Vec3d := new Vec3d(v.val0, 0, 0);
    public static function operator implicit(v: Vec3d): Vec1d := new Vec1d(v.val0);
    
    public static function operator implicit(v: Vec2b): Vec3d := new Vec3d(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3d): Vec2b := new Vec2b(Convert.ToSByte(v.val0), Convert.ToSByte(v.val1));
    
    public static function operator implicit(v: Vec2ub): Vec3d := new Vec3d(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3d): Vec2ub := new Vec2ub(Convert.ToByte(v.val0), Convert.ToByte(v.val1));
    
    public static function operator implicit(v: Vec2s): Vec3d := new Vec3d(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3d): Vec2s := new Vec2s(Convert.ToInt16(v.val0), Convert.ToInt16(v.val1));
    
    public static function operator implicit(v: Vec2us): Vec3d := new Vec3d(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3d): Vec2us := new Vec2us(Convert.ToUInt16(v.val0), Convert.ToUInt16(v.val1));
    
    public static function operator implicit(v: Vec2i): Vec3d := new Vec3d(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3d): Vec2i := new Vec2i(Convert.ToInt32(v.val0), Convert.ToInt32(v.val1));
    
    public static function operator implicit(v: Vec2ui): Vec3d := new Vec3d(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3d): Vec2ui := new Vec2ui(Convert.ToUInt32(v.val0), Convert.ToUInt32(v.val1));
    
    public static function operator implicit(v: Vec2i64): Vec3d := new Vec3d(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3d): Vec2i64 := new Vec2i64(Convert.ToInt64(v.val0), Convert.ToInt64(v.val1));
    
    public static function operator implicit(v: Vec2ui64): Vec3d := new Vec3d(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3d): Vec2ui64 := new Vec2ui64(Convert.ToUInt64(v.val0), Convert.ToUInt64(v.val1));
    
    public static function operator implicit(v: Vec2f): Vec3d := new Vec3d(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3d): Vec2f := new Vec2f(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2d): Vec3d := new Vec3d(v.val0, v.val1, 0);
    public static function operator implicit(v: Vec3d): Vec2d := new Vec2d(v.val0, v.val1);
    
    public static function operator implicit(v: Vec3b): Vec3d := new Vec3d(v.val0, v.val1, v.val2);
    public static function operator implicit(v: Vec3d): Vec3b := new Vec3b(Convert.ToSByte(v.val0), Convert.ToSByte(v.val1), Convert.ToSByte(v.val2));
    
    public static function operator implicit(v: Vec3ub): Vec3d := new Vec3d(v.val0, v.val1, v.val2);
    public static function operator implicit(v: Vec3d): Vec3ub := new Vec3ub(Convert.ToByte(v.val0), Convert.ToByte(v.val1), Convert.ToByte(v.val2));
    
    public static function operator implicit(v: Vec3s): Vec3d := new Vec3d(v.val0, v.val1, v.val2);
    public static function operator implicit(v: Vec3d): Vec3s := new Vec3s(Convert.ToInt16(v.val0), Convert.ToInt16(v.val1), Convert.ToInt16(v.val2));
    
    public static function operator implicit(v: Vec3us): Vec3d := new Vec3d(v.val0, v.val1, v.val2);
    public static function operator implicit(v: Vec3d): Vec3us := new Vec3us(Convert.ToUInt16(v.val0), Convert.ToUInt16(v.val1), Convert.ToUInt16(v.val2));
    
    public static function operator implicit(v: Vec3i): Vec3d := new Vec3d(v.val0, v.val1, v.val2);
    public static function operator implicit(v: Vec3d): Vec3i := new Vec3i(Convert.ToInt32(v.val0), Convert.ToInt32(v.val1), Convert.ToInt32(v.val2));
    
    public static function operator implicit(v: Vec3ui): Vec3d := new Vec3d(v.val0, v.val1, v.val2);
    public static function operator implicit(v: Vec3d): Vec3ui := new Vec3ui(Convert.ToUInt32(v.val0), Convert.ToUInt32(v.val1), Convert.ToUInt32(v.val2));
    
    public static function operator implicit(v: Vec3i64): Vec3d := new Vec3d(v.val0, v.val1, v.val2);
    public static function operator implicit(v: Vec3d): Vec3i64 := new Vec3i64(Convert.ToInt64(v.val0), Convert.ToInt64(v.val1), Convert.ToInt64(v.val2));
    
    public static function operator implicit(v: Vec3ui64): Vec3d := new Vec3d(v.val0, v.val1, v.val2);
    public static function operator implicit(v: Vec3d): Vec3ui64 := new Vec3ui64(Convert.ToUInt64(v.val0), Convert.ToUInt64(v.val1), Convert.ToUInt64(v.val2));
    
    public static function operator implicit(v: Vec3f): Vec3d := new Vec3d(v.val0, v.val1, v.val2);
    public static function operator implicit(v: Vec3d): Vec3f := new Vec3f(v.val0, v.val1, v.val2);
    
  end;
  {$endregion Vec3}
  
  {$region Vec4}
  
  Vec4b = record
    public val0: SByte;
    public val1: SByte;
    public val2: SByte;
    public val3: SByte;
    
    public constructor(val0, val1, val2, val3: SByte);
    begin
      self.val0 := val0;
      self.val1 := val1;
      self.val2 := val2;
      self.val3 := val3;
    end;
    
    private function GetValAt(i: integer): SByte;
    begin
      case i of
        0: Result := self.val0;
        1: Result := self.val1;
        2: Result := self.val2;
        3: Result := self.val3;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..3');
      end;
    end;
    private procedure SetValAt(i: integer; val: SByte);
    begin
      case i of
        0: self.val0 := val;
        1: self.val1 := val;
        2: self.val2 := val;
        3: self.val3 := val;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..3');
      end;
    end;
    public property val[i: integer]: SByte read GetValAt write SetValAt; default;
    
    public static function operator-(v: Vec4b): Vec4b := new Vec4b(-v.val0, -v.val1, -v.val2, -v.val3);
    public static function operator*(v: Vec4b; k: SByte): Vec4b := new Vec4b(v.val0*k, v.val1*k, v.val2*k, v.val3*k);
    public static function operator+(v1, v2: Vec4b): Vec4b := new Vec4b(v1.val0+v2.val0, v1.val1+v2.val1, v1.val2+v2.val2, v1.val3+v2.val3);
    public static function operator-(v1, v2: Vec4b): Vec4b := new Vec4b(v1.val0-v2.val0, v1.val1-v2.val1, v1.val2-v2.val2, v1.val3-v2.val3);
    
    public static function operator implicit(v: Vec1b): Vec4b := new Vec4b(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4b): Vec1b := new Vec1b(v.val0);
    
    public static function operator implicit(v: Vec1ub): Vec4b := new Vec4b(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4b): Vec1ub := new Vec1ub(v.val0);
    
    public static function operator implicit(v: Vec1s): Vec4b := new Vec4b(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4b): Vec1s := new Vec1s(v.val0);
    
    public static function operator implicit(v: Vec1us): Vec4b := new Vec4b(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4b): Vec1us := new Vec1us(v.val0);
    
    public static function operator implicit(v: Vec1i): Vec4b := new Vec4b(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4b): Vec1i := new Vec1i(v.val0);
    
    public static function operator implicit(v: Vec1ui): Vec4b := new Vec4b(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4b): Vec1ui := new Vec1ui(v.val0);
    
    public static function operator implicit(v: Vec1i64): Vec4b := new Vec4b(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4b): Vec1i64 := new Vec1i64(v.val0);
    
    public static function operator implicit(v: Vec1ui64): Vec4b := new Vec4b(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4b): Vec1ui64 := new Vec1ui64(v.val0);
    
    public static function operator implicit(v: Vec1f): Vec4b := new Vec4b(Convert.ToSByte(v.val0), 0, 0, 0);
    public static function operator implicit(v: Vec4b): Vec1f := new Vec1f(v.val0);
    
    public static function operator implicit(v: Vec1d): Vec4b := new Vec4b(Convert.ToSByte(v.val0), 0, 0, 0);
    public static function operator implicit(v: Vec4b): Vec1d := new Vec1d(v.val0);
    
    public static function operator implicit(v: Vec2b): Vec4b := new Vec4b(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4b): Vec2b := new Vec2b(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2ub): Vec4b := new Vec4b(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4b): Vec2ub := new Vec2ub(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2s): Vec4b := new Vec4b(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4b): Vec2s := new Vec2s(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2us): Vec4b := new Vec4b(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4b): Vec2us := new Vec2us(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2i): Vec4b := new Vec4b(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4b): Vec2i := new Vec2i(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2ui): Vec4b := new Vec4b(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4b): Vec2ui := new Vec2ui(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2i64): Vec4b := new Vec4b(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4b): Vec2i64 := new Vec2i64(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2ui64): Vec4b := new Vec4b(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4b): Vec2ui64 := new Vec2ui64(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2f): Vec4b := new Vec4b(Convert.ToSByte(v.val0), Convert.ToSByte(v.val1), 0, 0);
    public static function operator implicit(v: Vec4b): Vec2f := new Vec2f(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2d): Vec4b := new Vec4b(Convert.ToSByte(v.val0), Convert.ToSByte(v.val1), 0, 0);
    public static function operator implicit(v: Vec4b): Vec2d := new Vec2d(v.val0, v.val1);
    
    public static function operator implicit(v: Vec3b): Vec4b := new Vec4b(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4b): Vec3b := new Vec3b(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3ub): Vec4b := new Vec4b(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4b): Vec3ub := new Vec3ub(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3s): Vec4b := new Vec4b(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4b): Vec3s := new Vec3s(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3us): Vec4b := new Vec4b(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4b): Vec3us := new Vec3us(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3i): Vec4b := new Vec4b(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4b): Vec3i := new Vec3i(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3ui): Vec4b := new Vec4b(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4b): Vec3ui := new Vec3ui(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3i64): Vec4b := new Vec4b(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4b): Vec3i64 := new Vec3i64(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3ui64): Vec4b := new Vec4b(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4b): Vec3ui64 := new Vec3ui64(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3f): Vec4b := new Vec4b(Convert.ToSByte(v.val0), Convert.ToSByte(v.val1), Convert.ToSByte(v.val2), 0);
    public static function operator implicit(v: Vec4b): Vec3f := new Vec3f(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3d): Vec4b := new Vec4b(Convert.ToSByte(v.val0), Convert.ToSByte(v.val1), Convert.ToSByte(v.val2), 0);
    public static function operator implicit(v: Vec4b): Vec3d := new Vec3d(v.val0, v.val1, v.val2);
    
  end;
  
  Vec4ub = record
    public val0: Byte;
    public val1: Byte;
    public val2: Byte;
    public val3: Byte;
    
    public constructor(val0, val1, val2, val3: Byte);
    begin
      self.val0 := val0;
      self.val1 := val1;
      self.val2 := val2;
      self.val3 := val3;
    end;
    
    private function GetValAt(i: integer): Byte;
    begin
      case i of
        0: Result := self.val0;
        1: Result := self.val1;
        2: Result := self.val2;
        3: Result := self.val3;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..3');
      end;
    end;
    private procedure SetValAt(i: integer; val: Byte);
    begin
      case i of
        0: self.val0 := val;
        1: self.val1 := val;
        2: self.val2 := val;
        3: self.val3 := val;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..3');
      end;
    end;
    public property val[i: integer]: Byte read GetValAt write SetValAt; default;
    
    public static function operator*(v: Vec4ub; k: Byte): Vec4ub := new Vec4ub(v.val0*k, v.val1*k, v.val2*k, v.val3*k);
    public static function operator+(v1, v2: Vec4ub): Vec4ub := new Vec4ub(v1.val0+v2.val0, v1.val1+v2.val1, v1.val2+v2.val2, v1.val3+v2.val3);
    public static function operator-(v1, v2: Vec4ub): Vec4ub := new Vec4ub(v1.val0-v2.val0, v1.val1-v2.val1, v1.val2-v2.val2, v1.val3-v2.val3);
    
    public static function operator implicit(v: Vec1b): Vec4ub := new Vec4ub(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4ub): Vec1b := new Vec1b(v.val0);
    
    public static function operator implicit(v: Vec1ub): Vec4ub := new Vec4ub(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4ub): Vec1ub := new Vec1ub(v.val0);
    
    public static function operator implicit(v: Vec1s): Vec4ub := new Vec4ub(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4ub): Vec1s := new Vec1s(v.val0);
    
    public static function operator implicit(v: Vec1us): Vec4ub := new Vec4ub(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4ub): Vec1us := new Vec1us(v.val0);
    
    public static function operator implicit(v: Vec1i): Vec4ub := new Vec4ub(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4ub): Vec1i := new Vec1i(v.val0);
    
    public static function operator implicit(v: Vec1ui): Vec4ub := new Vec4ub(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4ub): Vec1ui := new Vec1ui(v.val0);
    
    public static function operator implicit(v: Vec1i64): Vec4ub := new Vec4ub(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4ub): Vec1i64 := new Vec1i64(v.val0);
    
    public static function operator implicit(v: Vec1ui64): Vec4ub := new Vec4ub(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4ub): Vec1ui64 := new Vec1ui64(v.val0);
    
    public static function operator implicit(v: Vec1f): Vec4ub := new Vec4ub(Convert.ToByte(v.val0), 0, 0, 0);
    public static function operator implicit(v: Vec4ub): Vec1f := new Vec1f(v.val0);
    
    public static function operator implicit(v: Vec1d): Vec4ub := new Vec4ub(Convert.ToByte(v.val0), 0, 0, 0);
    public static function operator implicit(v: Vec4ub): Vec1d := new Vec1d(v.val0);
    
    public static function operator implicit(v: Vec2b): Vec4ub := new Vec4ub(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4ub): Vec2b := new Vec2b(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2ub): Vec4ub := new Vec4ub(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4ub): Vec2ub := new Vec2ub(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2s): Vec4ub := new Vec4ub(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4ub): Vec2s := new Vec2s(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2us): Vec4ub := new Vec4ub(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4ub): Vec2us := new Vec2us(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2i): Vec4ub := new Vec4ub(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4ub): Vec2i := new Vec2i(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2ui): Vec4ub := new Vec4ub(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4ub): Vec2ui := new Vec2ui(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2i64): Vec4ub := new Vec4ub(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4ub): Vec2i64 := new Vec2i64(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2ui64): Vec4ub := new Vec4ub(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4ub): Vec2ui64 := new Vec2ui64(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2f): Vec4ub := new Vec4ub(Convert.ToByte(v.val0), Convert.ToByte(v.val1), 0, 0);
    public static function operator implicit(v: Vec4ub): Vec2f := new Vec2f(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2d): Vec4ub := new Vec4ub(Convert.ToByte(v.val0), Convert.ToByte(v.val1), 0, 0);
    public static function operator implicit(v: Vec4ub): Vec2d := new Vec2d(v.val0, v.val1);
    
    public static function operator implicit(v: Vec3b): Vec4ub := new Vec4ub(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4ub): Vec3b := new Vec3b(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3ub): Vec4ub := new Vec4ub(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4ub): Vec3ub := new Vec3ub(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3s): Vec4ub := new Vec4ub(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4ub): Vec3s := new Vec3s(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3us): Vec4ub := new Vec4ub(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4ub): Vec3us := new Vec3us(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3i): Vec4ub := new Vec4ub(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4ub): Vec3i := new Vec3i(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3ui): Vec4ub := new Vec4ub(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4ub): Vec3ui := new Vec3ui(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3i64): Vec4ub := new Vec4ub(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4ub): Vec3i64 := new Vec3i64(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3ui64): Vec4ub := new Vec4ub(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4ub): Vec3ui64 := new Vec3ui64(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3f): Vec4ub := new Vec4ub(Convert.ToByte(v.val0), Convert.ToByte(v.val1), Convert.ToByte(v.val2), 0);
    public static function operator implicit(v: Vec4ub): Vec3f := new Vec3f(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3d): Vec4ub := new Vec4ub(Convert.ToByte(v.val0), Convert.ToByte(v.val1), Convert.ToByte(v.val2), 0);
    public static function operator implicit(v: Vec4ub): Vec3d := new Vec3d(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec4b): Vec4ub := new Vec4ub(v.val0, v.val1, v.val2, v.val3);
    public static function operator implicit(v: Vec4ub): Vec4b := new Vec4b(v.val0, v.val1, v.val2, v.val3);
    
  end;
  
  Vec4s = record
    public val0: Int16;
    public val1: Int16;
    public val2: Int16;
    public val3: Int16;
    
    public constructor(val0, val1, val2, val3: Int16);
    begin
      self.val0 := val0;
      self.val1 := val1;
      self.val2 := val2;
      self.val3 := val3;
    end;
    
    private function GetValAt(i: integer): Int16;
    begin
      case i of
        0: Result := self.val0;
        1: Result := self.val1;
        2: Result := self.val2;
        3: Result := self.val3;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..3');
      end;
    end;
    private procedure SetValAt(i: integer; val: Int16);
    begin
      case i of
        0: self.val0 := val;
        1: self.val1 := val;
        2: self.val2 := val;
        3: self.val3 := val;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..3');
      end;
    end;
    public property val[i: integer]: Int16 read GetValAt write SetValAt; default;
    
    public static function operator-(v: Vec4s): Vec4s := new Vec4s(-v.val0, -v.val1, -v.val2, -v.val3);
    public static function operator*(v: Vec4s; k: Int16): Vec4s := new Vec4s(v.val0*k, v.val1*k, v.val2*k, v.val3*k);
    public static function operator+(v1, v2: Vec4s): Vec4s := new Vec4s(v1.val0+v2.val0, v1.val1+v2.val1, v1.val2+v2.val2, v1.val3+v2.val3);
    public static function operator-(v1, v2: Vec4s): Vec4s := new Vec4s(v1.val0-v2.val0, v1.val1-v2.val1, v1.val2-v2.val2, v1.val3-v2.val3);
    
    public static function operator implicit(v: Vec1b): Vec4s := new Vec4s(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4s): Vec1b := new Vec1b(v.val0);
    
    public static function operator implicit(v: Vec1ub): Vec4s := new Vec4s(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4s): Vec1ub := new Vec1ub(v.val0);
    
    public static function operator implicit(v: Vec1s): Vec4s := new Vec4s(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4s): Vec1s := new Vec1s(v.val0);
    
    public static function operator implicit(v: Vec1us): Vec4s := new Vec4s(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4s): Vec1us := new Vec1us(v.val0);
    
    public static function operator implicit(v: Vec1i): Vec4s := new Vec4s(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4s): Vec1i := new Vec1i(v.val0);
    
    public static function operator implicit(v: Vec1ui): Vec4s := new Vec4s(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4s): Vec1ui := new Vec1ui(v.val0);
    
    public static function operator implicit(v: Vec1i64): Vec4s := new Vec4s(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4s): Vec1i64 := new Vec1i64(v.val0);
    
    public static function operator implicit(v: Vec1ui64): Vec4s := new Vec4s(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4s): Vec1ui64 := new Vec1ui64(v.val0);
    
    public static function operator implicit(v: Vec1f): Vec4s := new Vec4s(Convert.ToInt16(v.val0), 0, 0, 0);
    public static function operator implicit(v: Vec4s): Vec1f := new Vec1f(v.val0);
    
    public static function operator implicit(v: Vec1d): Vec4s := new Vec4s(Convert.ToInt16(v.val0), 0, 0, 0);
    public static function operator implicit(v: Vec4s): Vec1d := new Vec1d(v.val0);
    
    public static function operator implicit(v: Vec2b): Vec4s := new Vec4s(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4s): Vec2b := new Vec2b(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2ub): Vec4s := new Vec4s(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4s): Vec2ub := new Vec2ub(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2s): Vec4s := new Vec4s(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4s): Vec2s := new Vec2s(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2us): Vec4s := new Vec4s(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4s): Vec2us := new Vec2us(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2i): Vec4s := new Vec4s(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4s): Vec2i := new Vec2i(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2ui): Vec4s := new Vec4s(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4s): Vec2ui := new Vec2ui(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2i64): Vec4s := new Vec4s(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4s): Vec2i64 := new Vec2i64(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2ui64): Vec4s := new Vec4s(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4s): Vec2ui64 := new Vec2ui64(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2f): Vec4s := new Vec4s(Convert.ToInt16(v.val0), Convert.ToInt16(v.val1), 0, 0);
    public static function operator implicit(v: Vec4s): Vec2f := new Vec2f(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2d): Vec4s := new Vec4s(Convert.ToInt16(v.val0), Convert.ToInt16(v.val1), 0, 0);
    public static function operator implicit(v: Vec4s): Vec2d := new Vec2d(v.val0, v.val1);
    
    public static function operator implicit(v: Vec3b): Vec4s := new Vec4s(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4s): Vec3b := new Vec3b(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3ub): Vec4s := new Vec4s(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4s): Vec3ub := new Vec3ub(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3s): Vec4s := new Vec4s(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4s): Vec3s := new Vec3s(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3us): Vec4s := new Vec4s(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4s): Vec3us := new Vec3us(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3i): Vec4s := new Vec4s(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4s): Vec3i := new Vec3i(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3ui): Vec4s := new Vec4s(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4s): Vec3ui := new Vec3ui(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3i64): Vec4s := new Vec4s(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4s): Vec3i64 := new Vec3i64(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3ui64): Vec4s := new Vec4s(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4s): Vec3ui64 := new Vec3ui64(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3f): Vec4s := new Vec4s(Convert.ToInt16(v.val0), Convert.ToInt16(v.val1), Convert.ToInt16(v.val2), 0);
    public static function operator implicit(v: Vec4s): Vec3f := new Vec3f(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3d): Vec4s := new Vec4s(Convert.ToInt16(v.val0), Convert.ToInt16(v.val1), Convert.ToInt16(v.val2), 0);
    public static function operator implicit(v: Vec4s): Vec3d := new Vec3d(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec4b): Vec4s := new Vec4s(v.val0, v.val1, v.val2, v.val3);
    public static function operator implicit(v: Vec4s): Vec4b := new Vec4b(v.val0, v.val1, v.val2, v.val3);
    
    public static function operator implicit(v: Vec4ub): Vec4s := new Vec4s(v.val0, v.val1, v.val2, v.val3);
    public static function operator implicit(v: Vec4s): Vec4ub := new Vec4ub(v.val0, v.val1, v.val2, v.val3);
    
  end;
  
  Vec4us = record
    public val0: UInt16;
    public val1: UInt16;
    public val2: UInt16;
    public val3: UInt16;
    
    public constructor(val0, val1, val2, val3: UInt16);
    begin
      self.val0 := val0;
      self.val1 := val1;
      self.val2 := val2;
      self.val3 := val3;
    end;
    
    private function GetValAt(i: integer): UInt16;
    begin
      case i of
        0: Result := self.val0;
        1: Result := self.val1;
        2: Result := self.val2;
        3: Result := self.val3;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..3');
      end;
    end;
    private procedure SetValAt(i: integer; val: UInt16);
    begin
      case i of
        0: self.val0 := val;
        1: self.val1 := val;
        2: self.val2 := val;
        3: self.val3 := val;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..3');
      end;
    end;
    public property val[i: integer]: UInt16 read GetValAt write SetValAt; default;
    
    public static function operator*(v: Vec4us; k: UInt16): Vec4us := new Vec4us(v.val0*k, v.val1*k, v.val2*k, v.val3*k);
    public static function operator+(v1, v2: Vec4us): Vec4us := new Vec4us(v1.val0+v2.val0, v1.val1+v2.val1, v1.val2+v2.val2, v1.val3+v2.val3);
    public static function operator-(v1, v2: Vec4us): Vec4us := new Vec4us(v1.val0-v2.val0, v1.val1-v2.val1, v1.val2-v2.val2, v1.val3-v2.val3);
    
    public static function operator implicit(v: Vec1b): Vec4us := new Vec4us(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4us): Vec1b := new Vec1b(v.val0);
    
    public static function operator implicit(v: Vec1ub): Vec4us := new Vec4us(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4us): Vec1ub := new Vec1ub(v.val0);
    
    public static function operator implicit(v: Vec1s): Vec4us := new Vec4us(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4us): Vec1s := new Vec1s(v.val0);
    
    public static function operator implicit(v: Vec1us): Vec4us := new Vec4us(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4us): Vec1us := new Vec1us(v.val0);
    
    public static function operator implicit(v: Vec1i): Vec4us := new Vec4us(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4us): Vec1i := new Vec1i(v.val0);
    
    public static function operator implicit(v: Vec1ui): Vec4us := new Vec4us(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4us): Vec1ui := new Vec1ui(v.val0);
    
    public static function operator implicit(v: Vec1i64): Vec4us := new Vec4us(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4us): Vec1i64 := new Vec1i64(v.val0);
    
    public static function operator implicit(v: Vec1ui64): Vec4us := new Vec4us(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4us): Vec1ui64 := new Vec1ui64(v.val0);
    
    public static function operator implicit(v: Vec1f): Vec4us := new Vec4us(Convert.ToUInt16(v.val0), 0, 0, 0);
    public static function operator implicit(v: Vec4us): Vec1f := new Vec1f(v.val0);
    
    public static function operator implicit(v: Vec1d): Vec4us := new Vec4us(Convert.ToUInt16(v.val0), 0, 0, 0);
    public static function operator implicit(v: Vec4us): Vec1d := new Vec1d(v.val0);
    
    public static function operator implicit(v: Vec2b): Vec4us := new Vec4us(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4us): Vec2b := new Vec2b(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2ub): Vec4us := new Vec4us(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4us): Vec2ub := new Vec2ub(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2s): Vec4us := new Vec4us(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4us): Vec2s := new Vec2s(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2us): Vec4us := new Vec4us(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4us): Vec2us := new Vec2us(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2i): Vec4us := new Vec4us(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4us): Vec2i := new Vec2i(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2ui): Vec4us := new Vec4us(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4us): Vec2ui := new Vec2ui(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2i64): Vec4us := new Vec4us(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4us): Vec2i64 := new Vec2i64(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2ui64): Vec4us := new Vec4us(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4us): Vec2ui64 := new Vec2ui64(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2f): Vec4us := new Vec4us(Convert.ToUInt16(v.val0), Convert.ToUInt16(v.val1), 0, 0);
    public static function operator implicit(v: Vec4us): Vec2f := new Vec2f(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2d): Vec4us := new Vec4us(Convert.ToUInt16(v.val0), Convert.ToUInt16(v.val1), 0, 0);
    public static function operator implicit(v: Vec4us): Vec2d := new Vec2d(v.val0, v.val1);
    
    public static function operator implicit(v: Vec3b): Vec4us := new Vec4us(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4us): Vec3b := new Vec3b(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3ub): Vec4us := new Vec4us(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4us): Vec3ub := new Vec3ub(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3s): Vec4us := new Vec4us(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4us): Vec3s := new Vec3s(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3us): Vec4us := new Vec4us(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4us): Vec3us := new Vec3us(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3i): Vec4us := new Vec4us(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4us): Vec3i := new Vec3i(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3ui): Vec4us := new Vec4us(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4us): Vec3ui := new Vec3ui(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3i64): Vec4us := new Vec4us(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4us): Vec3i64 := new Vec3i64(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3ui64): Vec4us := new Vec4us(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4us): Vec3ui64 := new Vec3ui64(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3f): Vec4us := new Vec4us(Convert.ToUInt16(v.val0), Convert.ToUInt16(v.val1), Convert.ToUInt16(v.val2), 0);
    public static function operator implicit(v: Vec4us): Vec3f := new Vec3f(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3d): Vec4us := new Vec4us(Convert.ToUInt16(v.val0), Convert.ToUInt16(v.val1), Convert.ToUInt16(v.val2), 0);
    public static function operator implicit(v: Vec4us): Vec3d := new Vec3d(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec4b): Vec4us := new Vec4us(v.val0, v.val1, v.val2, v.val3);
    public static function operator implicit(v: Vec4us): Vec4b := new Vec4b(v.val0, v.val1, v.val2, v.val3);
    
    public static function operator implicit(v: Vec4ub): Vec4us := new Vec4us(v.val0, v.val1, v.val2, v.val3);
    public static function operator implicit(v: Vec4us): Vec4ub := new Vec4ub(v.val0, v.val1, v.val2, v.val3);
    
    public static function operator implicit(v: Vec4s): Vec4us := new Vec4us(v.val0, v.val1, v.val2, v.val3);
    public static function operator implicit(v: Vec4us): Vec4s := new Vec4s(v.val0, v.val1, v.val2, v.val3);
    
  end;
  
  Vec4i = record
    public val0: Int32;
    public val1: Int32;
    public val2: Int32;
    public val3: Int32;
    
    public constructor(val0, val1, val2, val3: Int32);
    begin
      self.val0 := val0;
      self.val1 := val1;
      self.val2 := val2;
      self.val3 := val3;
    end;
    
    private function GetValAt(i: integer): Int32;
    begin
      case i of
        0: Result := self.val0;
        1: Result := self.val1;
        2: Result := self.val2;
        3: Result := self.val3;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..3');
      end;
    end;
    private procedure SetValAt(i: integer; val: Int32);
    begin
      case i of
        0: self.val0 := val;
        1: self.val1 := val;
        2: self.val2 := val;
        3: self.val3 := val;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..3');
      end;
    end;
    public property val[i: integer]: Int32 read GetValAt write SetValAt; default;
    
    public static function operator-(v: Vec4i): Vec4i := new Vec4i(-v.val0, -v.val1, -v.val2, -v.val3);
    public static function operator*(v: Vec4i; k: Int32): Vec4i := new Vec4i(v.val0*k, v.val1*k, v.val2*k, v.val3*k);
    public static function operator+(v1, v2: Vec4i): Vec4i := new Vec4i(v1.val0+v2.val0, v1.val1+v2.val1, v1.val2+v2.val2, v1.val3+v2.val3);
    public static function operator-(v1, v2: Vec4i): Vec4i := new Vec4i(v1.val0-v2.val0, v1.val1-v2.val1, v1.val2-v2.val2, v1.val3-v2.val3);
    
    public static function operator implicit(v: Vec1b): Vec4i := new Vec4i(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4i): Vec1b := new Vec1b(v.val0);
    
    public static function operator implicit(v: Vec1ub): Vec4i := new Vec4i(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4i): Vec1ub := new Vec1ub(v.val0);
    
    public static function operator implicit(v: Vec1s): Vec4i := new Vec4i(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4i): Vec1s := new Vec1s(v.val0);
    
    public static function operator implicit(v: Vec1us): Vec4i := new Vec4i(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4i): Vec1us := new Vec1us(v.val0);
    
    public static function operator implicit(v: Vec1i): Vec4i := new Vec4i(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4i): Vec1i := new Vec1i(v.val0);
    
    public static function operator implicit(v: Vec1ui): Vec4i := new Vec4i(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4i): Vec1ui := new Vec1ui(v.val0);
    
    public static function operator implicit(v: Vec1i64): Vec4i := new Vec4i(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4i): Vec1i64 := new Vec1i64(v.val0);
    
    public static function operator implicit(v: Vec1ui64): Vec4i := new Vec4i(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4i): Vec1ui64 := new Vec1ui64(v.val0);
    
    public static function operator implicit(v: Vec1f): Vec4i := new Vec4i(Convert.ToInt32(v.val0), 0, 0, 0);
    public static function operator implicit(v: Vec4i): Vec1f := new Vec1f(v.val0);
    
    public static function operator implicit(v: Vec1d): Vec4i := new Vec4i(Convert.ToInt32(v.val0), 0, 0, 0);
    public static function operator implicit(v: Vec4i): Vec1d := new Vec1d(v.val0);
    
    public static function operator implicit(v: Vec2b): Vec4i := new Vec4i(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4i): Vec2b := new Vec2b(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2ub): Vec4i := new Vec4i(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4i): Vec2ub := new Vec2ub(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2s): Vec4i := new Vec4i(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4i): Vec2s := new Vec2s(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2us): Vec4i := new Vec4i(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4i): Vec2us := new Vec2us(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2i): Vec4i := new Vec4i(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4i): Vec2i := new Vec2i(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2ui): Vec4i := new Vec4i(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4i): Vec2ui := new Vec2ui(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2i64): Vec4i := new Vec4i(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4i): Vec2i64 := new Vec2i64(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2ui64): Vec4i := new Vec4i(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4i): Vec2ui64 := new Vec2ui64(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2f): Vec4i := new Vec4i(Convert.ToInt32(v.val0), Convert.ToInt32(v.val1), 0, 0);
    public static function operator implicit(v: Vec4i): Vec2f := new Vec2f(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2d): Vec4i := new Vec4i(Convert.ToInt32(v.val0), Convert.ToInt32(v.val1), 0, 0);
    public static function operator implicit(v: Vec4i): Vec2d := new Vec2d(v.val0, v.val1);
    
    public static function operator implicit(v: Vec3b): Vec4i := new Vec4i(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4i): Vec3b := new Vec3b(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3ub): Vec4i := new Vec4i(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4i): Vec3ub := new Vec3ub(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3s): Vec4i := new Vec4i(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4i): Vec3s := new Vec3s(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3us): Vec4i := new Vec4i(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4i): Vec3us := new Vec3us(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3i): Vec4i := new Vec4i(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4i): Vec3i := new Vec3i(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3ui): Vec4i := new Vec4i(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4i): Vec3ui := new Vec3ui(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3i64): Vec4i := new Vec4i(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4i): Vec3i64 := new Vec3i64(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3ui64): Vec4i := new Vec4i(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4i): Vec3ui64 := new Vec3ui64(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3f): Vec4i := new Vec4i(Convert.ToInt32(v.val0), Convert.ToInt32(v.val1), Convert.ToInt32(v.val2), 0);
    public static function operator implicit(v: Vec4i): Vec3f := new Vec3f(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3d): Vec4i := new Vec4i(Convert.ToInt32(v.val0), Convert.ToInt32(v.val1), Convert.ToInt32(v.val2), 0);
    public static function operator implicit(v: Vec4i): Vec3d := new Vec3d(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec4b): Vec4i := new Vec4i(v.val0, v.val1, v.val2, v.val3);
    public static function operator implicit(v: Vec4i): Vec4b := new Vec4b(v.val0, v.val1, v.val2, v.val3);
    
    public static function operator implicit(v: Vec4ub): Vec4i := new Vec4i(v.val0, v.val1, v.val2, v.val3);
    public static function operator implicit(v: Vec4i): Vec4ub := new Vec4ub(v.val0, v.val1, v.val2, v.val3);
    
    public static function operator implicit(v: Vec4s): Vec4i := new Vec4i(v.val0, v.val1, v.val2, v.val3);
    public static function operator implicit(v: Vec4i): Vec4s := new Vec4s(v.val0, v.val1, v.val2, v.val3);
    
    public static function operator implicit(v: Vec4us): Vec4i := new Vec4i(v.val0, v.val1, v.val2, v.val3);
    public static function operator implicit(v: Vec4i): Vec4us := new Vec4us(v.val0, v.val1, v.val2, v.val3);
    
  end;
  
  Vec4ui = record
    public val0: UInt32;
    public val1: UInt32;
    public val2: UInt32;
    public val3: UInt32;
    
    public constructor(val0, val1, val2, val3: UInt32);
    begin
      self.val0 := val0;
      self.val1 := val1;
      self.val2 := val2;
      self.val3 := val3;
    end;
    
    private function GetValAt(i: integer): UInt32;
    begin
      case i of
        0: Result := self.val0;
        1: Result := self.val1;
        2: Result := self.val2;
        3: Result := self.val3;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..3');
      end;
    end;
    private procedure SetValAt(i: integer; val: UInt32);
    begin
      case i of
        0: self.val0 := val;
        1: self.val1 := val;
        2: self.val2 := val;
        3: self.val3 := val;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..3');
      end;
    end;
    public property val[i: integer]: UInt32 read GetValAt write SetValAt; default;
    
    public static function operator*(v: Vec4ui; k: UInt32): Vec4ui := new Vec4ui(v.val0*k, v.val1*k, v.val2*k, v.val3*k);
    public static function operator+(v1, v2: Vec4ui): Vec4ui := new Vec4ui(v1.val0+v2.val0, v1.val1+v2.val1, v1.val2+v2.val2, v1.val3+v2.val3);
    public static function operator-(v1, v2: Vec4ui): Vec4ui := new Vec4ui(v1.val0-v2.val0, v1.val1-v2.val1, v1.val2-v2.val2, v1.val3-v2.val3);
    
    public static function operator implicit(v: Vec1b): Vec4ui := new Vec4ui(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4ui): Vec1b := new Vec1b(v.val0);
    
    public static function operator implicit(v: Vec1ub): Vec4ui := new Vec4ui(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4ui): Vec1ub := new Vec1ub(v.val0);
    
    public static function operator implicit(v: Vec1s): Vec4ui := new Vec4ui(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4ui): Vec1s := new Vec1s(v.val0);
    
    public static function operator implicit(v: Vec1us): Vec4ui := new Vec4ui(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4ui): Vec1us := new Vec1us(v.val0);
    
    public static function operator implicit(v: Vec1i): Vec4ui := new Vec4ui(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4ui): Vec1i := new Vec1i(v.val0);
    
    public static function operator implicit(v: Vec1ui): Vec4ui := new Vec4ui(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4ui): Vec1ui := new Vec1ui(v.val0);
    
    public static function operator implicit(v: Vec1i64): Vec4ui := new Vec4ui(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4ui): Vec1i64 := new Vec1i64(v.val0);
    
    public static function operator implicit(v: Vec1ui64): Vec4ui := new Vec4ui(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4ui): Vec1ui64 := new Vec1ui64(v.val0);
    
    public static function operator implicit(v: Vec1f): Vec4ui := new Vec4ui(Convert.ToUInt32(v.val0), 0, 0, 0);
    public static function operator implicit(v: Vec4ui): Vec1f := new Vec1f(v.val0);
    
    public static function operator implicit(v: Vec1d): Vec4ui := new Vec4ui(Convert.ToUInt32(v.val0), 0, 0, 0);
    public static function operator implicit(v: Vec4ui): Vec1d := new Vec1d(v.val0);
    
    public static function operator implicit(v: Vec2b): Vec4ui := new Vec4ui(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4ui): Vec2b := new Vec2b(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2ub): Vec4ui := new Vec4ui(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4ui): Vec2ub := new Vec2ub(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2s): Vec4ui := new Vec4ui(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4ui): Vec2s := new Vec2s(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2us): Vec4ui := new Vec4ui(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4ui): Vec2us := new Vec2us(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2i): Vec4ui := new Vec4ui(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4ui): Vec2i := new Vec2i(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2ui): Vec4ui := new Vec4ui(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4ui): Vec2ui := new Vec2ui(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2i64): Vec4ui := new Vec4ui(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4ui): Vec2i64 := new Vec2i64(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2ui64): Vec4ui := new Vec4ui(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4ui): Vec2ui64 := new Vec2ui64(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2f): Vec4ui := new Vec4ui(Convert.ToUInt32(v.val0), Convert.ToUInt32(v.val1), 0, 0);
    public static function operator implicit(v: Vec4ui): Vec2f := new Vec2f(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2d): Vec4ui := new Vec4ui(Convert.ToUInt32(v.val0), Convert.ToUInt32(v.val1), 0, 0);
    public static function operator implicit(v: Vec4ui): Vec2d := new Vec2d(v.val0, v.val1);
    
    public static function operator implicit(v: Vec3b): Vec4ui := new Vec4ui(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4ui): Vec3b := new Vec3b(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3ub): Vec4ui := new Vec4ui(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4ui): Vec3ub := new Vec3ub(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3s): Vec4ui := new Vec4ui(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4ui): Vec3s := new Vec3s(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3us): Vec4ui := new Vec4ui(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4ui): Vec3us := new Vec3us(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3i): Vec4ui := new Vec4ui(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4ui): Vec3i := new Vec3i(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3ui): Vec4ui := new Vec4ui(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4ui): Vec3ui := new Vec3ui(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3i64): Vec4ui := new Vec4ui(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4ui): Vec3i64 := new Vec3i64(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3ui64): Vec4ui := new Vec4ui(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4ui): Vec3ui64 := new Vec3ui64(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3f): Vec4ui := new Vec4ui(Convert.ToUInt32(v.val0), Convert.ToUInt32(v.val1), Convert.ToUInt32(v.val2), 0);
    public static function operator implicit(v: Vec4ui): Vec3f := new Vec3f(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3d): Vec4ui := new Vec4ui(Convert.ToUInt32(v.val0), Convert.ToUInt32(v.val1), Convert.ToUInt32(v.val2), 0);
    public static function operator implicit(v: Vec4ui): Vec3d := new Vec3d(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec4b): Vec4ui := new Vec4ui(v.val0, v.val1, v.val2, v.val3);
    public static function operator implicit(v: Vec4ui): Vec4b := new Vec4b(v.val0, v.val1, v.val2, v.val3);
    
    public static function operator implicit(v: Vec4ub): Vec4ui := new Vec4ui(v.val0, v.val1, v.val2, v.val3);
    public static function operator implicit(v: Vec4ui): Vec4ub := new Vec4ub(v.val0, v.val1, v.val2, v.val3);
    
    public static function operator implicit(v: Vec4s): Vec4ui := new Vec4ui(v.val0, v.val1, v.val2, v.val3);
    public static function operator implicit(v: Vec4ui): Vec4s := new Vec4s(v.val0, v.val1, v.val2, v.val3);
    
    public static function operator implicit(v: Vec4us): Vec4ui := new Vec4ui(v.val0, v.val1, v.val2, v.val3);
    public static function operator implicit(v: Vec4ui): Vec4us := new Vec4us(v.val0, v.val1, v.val2, v.val3);
    
    public static function operator implicit(v: Vec4i): Vec4ui := new Vec4ui(v.val0, v.val1, v.val2, v.val3);
    public static function operator implicit(v: Vec4ui): Vec4i := new Vec4i(v.val0, v.val1, v.val2, v.val3);
    
  end;
  
  Vec4i64 = record
    public val0: Int64;
    public val1: Int64;
    public val2: Int64;
    public val3: Int64;
    
    public constructor(val0, val1, val2, val3: Int64);
    begin
      self.val0 := val0;
      self.val1 := val1;
      self.val2 := val2;
      self.val3 := val3;
    end;
    
    private function GetValAt(i: integer): Int64;
    begin
      case i of
        0: Result := self.val0;
        1: Result := self.val1;
        2: Result := self.val2;
        3: Result := self.val3;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..3');
      end;
    end;
    private procedure SetValAt(i: integer; val: Int64);
    begin
      case i of
        0: self.val0 := val;
        1: self.val1 := val;
        2: self.val2 := val;
        3: self.val3 := val;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..3');
      end;
    end;
    public property val[i: integer]: Int64 read GetValAt write SetValAt; default;
    
    public static function operator-(v: Vec4i64): Vec4i64 := new Vec4i64(-v.val0, -v.val1, -v.val2, -v.val3);
    public static function operator*(v: Vec4i64; k: Int64): Vec4i64 := new Vec4i64(v.val0*k, v.val1*k, v.val2*k, v.val3*k);
    public static function operator+(v1, v2: Vec4i64): Vec4i64 := new Vec4i64(v1.val0+v2.val0, v1.val1+v2.val1, v1.val2+v2.val2, v1.val3+v2.val3);
    public static function operator-(v1, v2: Vec4i64): Vec4i64 := new Vec4i64(v1.val0-v2.val0, v1.val1-v2.val1, v1.val2-v2.val2, v1.val3-v2.val3);
    
    public static function operator implicit(v: Vec1b): Vec4i64 := new Vec4i64(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4i64): Vec1b := new Vec1b(v.val0);
    
    public static function operator implicit(v: Vec1ub): Vec4i64 := new Vec4i64(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4i64): Vec1ub := new Vec1ub(v.val0);
    
    public static function operator implicit(v: Vec1s): Vec4i64 := new Vec4i64(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4i64): Vec1s := new Vec1s(v.val0);
    
    public static function operator implicit(v: Vec1us): Vec4i64 := new Vec4i64(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4i64): Vec1us := new Vec1us(v.val0);
    
    public static function operator implicit(v: Vec1i): Vec4i64 := new Vec4i64(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4i64): Vec1i := new Vec1i(v.val0);
    
    public static function operator implicit(v: Vec1ui): Vec4i64 := new Vec4i64(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4i64): Vec1ui := new Vec1ui(v.val0);
    
    public static function operator implicit(v: Vec1i64): Vec4i64 := new Vec4i64(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4i64): Vec1i64 := new Vec1i64(v.val0);
    
    public static function operator implicit(v: Vec1ui64): Vec4i64 := new Vec4i64(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4i64): Vec1ui64 := new Vec1ui64(v.val0);
    
    public static function operator implicit(v: Vec1f): Vec4i64 := new Vec4i64(Convert.ToInt64(v.val0), 0, 0, 0);
    public static function operator implicit(v: Vec4i64): Vec1f := new Vec1f(v.val0);
    
    public static function operator implicit(v: Vec1d): Vec4i64 := new Vec4i64(Convert.ToInt64(v.val0), 0, 0, 0);
    public static function operator implicit(v: Vec4i64): Vec1d := new Vec1d(v.val0);
    
    public static function operator implicit(v: Vec2b): Vec4i64 := new Vec4i64(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4i64): Vec2b := new Vec2b(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2ub): Vec4i64 := new Vec4i64(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4i64): Vec2ub := new Vec2ub(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2s): Vec4i64 := new Vec4i64(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4i64): Vec2s := new Vec2s(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2us): Vec4i64 := new Vec4i64(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4i64): Vec2us := new Vec2us(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2i): Vec4i64 := new Vec4i64(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4i64): Vec2i := new Vec2i(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2ui): Vec4i64 := new Vec4i64(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4i64): Vec2ui := new Vec2ui(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2i64): Vec4i64 := new Vec4i64(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4i64): Vec2i64 := new Vec2i64(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2ui64): Vec4i64 := new Vec4i64(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4i64): Vec2ui64 := new Vec2ui64(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2f): Vec4i64 := new Vec4i64(Convert.ToInt64(v.val0), Convert.ToInt64(v.val1), 0, 0);
    public static function operator implicit(v: Vec4i64): Vec2f := new Vec2f(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2d): Vec4i64 := new Vec4i64(Convert.ToInt64(v.val0), Convert.ToInt64(v.val1), 0, 0);
    public static function operator implicit(v: Vec4i64): Vec2d := new Vec2d(v.val0, v.val1);
    
    public static function operator implicit(v: Vec3b): Vec4i64 := new Vec4i64(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4i64): Vec3b := new Vec3b(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3ub): Vec4i64 := new Vec4i64(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4i64): Vec3ub := new Vec3ub(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3s): Vec4i64 := new Vec4i64(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4i64): Vec3s := new Vec3s(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3us): Vec4i64 := new Vec4i64(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4i64): Vec3us := new Vec3us(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3i): Vec4i64 := new Vec4i64(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4i64): Vec3i := new Vec3i(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3ui): Vec4i64 := new Vec4i64(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4i64): Vec3ui := new Vec3ui(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3i64): Vec4i64 := new Vec4i64(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4i64): Vec3i64 := new Vec3i64(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3ui64): Vec4i64 := new Vec4i64(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4i64): Vec3ui64 := new Vec3ui64(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3f): Vec4i64 := new Vec4i64(Convert.ToInt64(v.val0), Convert.ToInt64(v.val1), Convert.ToInt64(v.val2), 0);
    public static function operator implicit(v: Vec4i64): Vec3f := new Vec3f(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3d): Vec4i64 := new Vec4i64(Convert.ToInt64(v.val0), Convert.ToInt64(v.val1), Convert.ToInt64(v.val2), 0);
    public static function operator implicit(v: Vec4i64): Vec3d := new Vec3d(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec4b): Vec4i64 := new Vec4i64(v.val0, v.val1, v.val2, v.val3);
    public static function operator implicit(v: Vec4i64): Vec4b := new Vec4b(v.val0, v.val1, v.val2, v.val3);
    
    public static function operator implicit(v: Vec4ub): Vec4i64 := new Vec4i64(v.val0, v.val1, v.val2, v.val3);
    public static function operator implicit(v: Vec4i64): Vec4ub := new Vec4ub(v.val0, v.val1, v.val2, v.val3);
    
    public static function operator implicit(v: Vec4s): Vec4i64 := new Vec4i64(v.val0, v.val1, v.val2, v.val3);
    public static function operator implicit(v: Vec4i64): Vec4s := new Vec4s(v.val0, v.val1, v.val2, v.val3);
    
    public static function operator implicit(v: Vec4us): Vec4i64 := new Vec4i64(v.val0, v.val1, v.val2, v.val3);
    public static function operator implicit(v: Vec4i64): Vec4us := new Vec4us(v.val0, v.val1, v.val2, v.val3);
    
    public static function operator implicit(v: Vec4i): Vec4i64 := new Vec4i64(v.val0, v.val1, v.val2, v.val3);
    public static function operator implicit(v: Vec4i64): Vec4i := new Vec4i(v.val0, v.val1, v.val2, v.val3);
    
    public static function operator implicit(v: Vec4ui): Vec4i64 := new Vec4i64(v.val0, v.val1, v.val2, v.val3);
    public static function operator implicit(v: Vec4i64): Vec4ui := new Vec4ui(v.val0, v.val1, v.val2, v.val3);
    
  end;
  
  Vec4ui64 = record
    public val0: UInt64;
    public val1: UInt64;
    public val2: UInt64;
    public val3: UInt64;
    
    public constructor(val0, val1, val2, val3: UInt64);
    begin
      self.val0 := val0;
      self.val1 := val1;
      self.val2 := val2;
      self.val3 := val3;
    end;
    
    private function GetValAt(i: integer): UInt64;
    begin
      case i of
        0: Result := self.val0;
        1: Result := self.val1;
        2: Result := self.val2;
        3: Result := self.val3;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..3');
      end;
    end;
    private procedure SetValAt(i: integer; val: UInt64);
    begin
      case i of
        0: self.val0 := val;
        1: self.val1 := val;
        2: self.val2 := val;
        3: self.val3 := val;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..3');
      end;
    end;
    public property val[i: integer]: UInt64 read GetValAt write SetValAt; default;
    
    public static function operator*(v: Vec4ui64; k: UInt64): Vec4ui64 := new Vec4ui64(v.val0*k, v.val1*k, v.val2*k, v.val3*k);
    public static function operator+(v1, v2: Vec4ui64): Vec4ui64 := new Vec4ui64(v1.val0+v2.val0, v1.val1+v2.val1, v1.val2+v2.val2, v1.val3+v2.val3);
    public static function operator-(v1, v2: Vec4ui64): Vec4ui64 := new Vec4ui64(v1.val0-v2.val0, v1.val1-v2.val1, v1.val2-v2.val2, v1.val3-v2.val3);
    
    public static function operator implicit(v: Vec1b): Vec4ui64 := new Vec4ui64(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4ui64): Vec1b := new Vec1b(v.val0);
    
    public static function operator implicit(v: Vec1ub): Vec4ui64 := new Vec4ui64(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4ui64): Vec1ub := new Vec1ub(v.val0);
    
    public static function operator implicit(v: Vec1s): Vec4ui64 := new Vec4ui64(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4ui64): Vec1s := new Vec1s(v.val0);
    
    public static function operator implicit(v: Vec1us): Vec4ui64 := new Vec4ui64(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4ui64): Vec1us := new Vec1us(v.val0);
    
    public static function operator implicit(v: Vec1i): Vec4ui64 := new Vec4ui64(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4ui64): Vec1i := new Vec1i(v.val0);
    
    public static function operator implicit(v: Vec1ui): Vec4ui64 := new Vec4ui64(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4ui64): Vec1ui := new Vec1ui(v.val0);
    
    public static function operator implicit(v: Vec1i64): Vec4ui64 := new Vec4ui64(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4ui64): Vec1i64 := new Vec1i64(v.val0);
    
    public static function operator implicit(v: Vec1ui64): Vec4ui64 := new Vec4ui64(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4ui64): Vec1ui64 := new Vec1ui64(v.val0);
    
    public static function operator implicit(v: Vec1f): Vec4ui64 := new Vec4ui64(Convert.ToUInt64(v.val0), 0, 0, 0);
    public static function operator implicit(v: Vec4ui64): Vec1f := new Vec1f(v.val0);
    
    public static function operator implicit(v: Vec1d): Vec4ui64 := new Vec4ui64(Convert.ToUInt64(v.val0), 0, 0, 0);
    public static function operator implicit(v: Vec4ui64): Vec1d := new Vec1d(v.val0);
    
    public static function operator implicit(v: Vec2b): Vec4ui64 := new Vec4ui64(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4ui64): Vec2b := new Vec2b(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2ub): Vec4ui64 := new Vec4ui64(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4ui64): Vec2ub := new Vec2ub(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2s): Vec4ui64 := new Vec4ui64(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4ui64): Vec2s := new Vec2s(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2us): Vec4ui64 := new Vec4ui64(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4ui64): Vec2us := new Vec2us(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2i): Vec4ui64 := new Vec4ui64(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4ui64): Vec2i := new Vec2i(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2ui): Vec4ui64 := new Vec4ui64(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4ui64): Vec2ui := new Vec2ui(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2i64): Vec4ui64 := new Vec4ui64(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4ui64): Vec2i64 := new Vec2i64(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2ui64): Vec4ui64 := new Vec4ui64(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4ui64): Vec2ui64 := new Vec2ui64(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2f): Vec4ui64 := new Vec4ui64(Convert.ToUInt64(v.val0), Convert.ToUInt64(v.val1), 0, 0);
    public static function operator implicit(v: Vec4ui64): Vec2f := new Vec2f(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2d): Vec4ui64 := new Vec4ui64(Convert.ToUInt64(v.val0), Convert.ToUInt64(v.val1), 0, 0);
    public static function operator implicit(v: Vec4ui64): Vec2d := new Vec2d(v.val0, v.val1);
    
    public static function operator implicit(v: Vec3b): Vec4ui64 := new Vec4ui64(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4ui64): Vec3b := new Vec3b(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3ub): Vec4ui64 := new Vec4ui64(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4ui64): Vec3ub := new Vec3ub(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3s): Vec4ui64 := new Vec4ui64(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4ui64): Vec3s := new Vec3s(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3us): Vec4ui64 := new Vec4ui64(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4ui64): Vec3us := new Vec3us(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3i): Vec4ui64 := new Vec4ui64(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4ui64): Vec3i := new Vec3i(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3ui): Vec4ui64 := new Vec4ui64(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4ui64): Vec3ui := new Vec3ui(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3i64): Vec4ui64 := new Vec4ui64(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4ui64): Vec3i64 := new Vec3i64(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3ui64): Vec4ui64 := new Vec4ui64(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4ui64): Vec3ui64 := new Vec3ui64(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3f): Vec4ui64 := new Vec4ui64(Convert.ToUInt64(v.val0), Convert.ToUInt64(v.val1), Convert.ToUInt64(v.val2), 0);
    public static function operator implicit(v: Vec4ui64): Vec3f := new Vec3f(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3d): Vec4ui64 := new Vec4ui64(Convert.ToUInt64(v.val0), Convert.ToUInt64(v.val1), Convert.ToUInt64(v.val2), 0);
    public static function operator implicit(v: Vec4ui64): Vec3d := new Vec3d(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec4b): Vec4ui64 := new Vec4ui64(v.val0, v.val1, v.val2, v.val3);
    public static function operator implicit(v: Vec4ui64): Vec4b := new Vec4b(v.val0, v.val1, v.val2, v.val3);
    
    public static function operator implicit(v: Vec4ub): Vec4ui64 := new Vec4ui64(v.val0, v.val1, v.val2, v.val3);
    public static function operator implicit(v: Vec4ui64): Vec4ub := new Vec4ub(v.val0, v.val1, v.val2, v.val3);
    
    public static function operator implicit(v: Vec4s): Vec4ui64 := new Vec4ui64(v.val0, v.val1, v.val2, v.val3);
    public static function operator implicit(v: Vec4ui64): Vec4s := new Vec4s(v.val0, v.val1, v.val2, v.val3);
    
    public static function operator implicit(v: Vec4us): Vec4ui64 := new Vec4ui64(v.val0, v.val1, v.val2, v.val3);
    public static function operator implicit(v: Vec4ui64): Vec4us := new Vec4us(v.val0, v.val1, v.val2, v.val3);
    
    public static function operator implicit(v: Vec4i): Vec4ui64 := new Vec4ui64(v.val0, v.val1, v.val2, v.val3);
    public static function operator implicit(v: Vec4ui64): Vec4i := new Vec4i(v.val0, v.val1, v.val2, v.val3);
    
    public static function operator implicit(v: Vec4ui): Vec4ui64 := new Vec4ui64(v.val0, v.val1, v.val2, v.val3);
    public static function operator implicit(v: Vec4ui64): Vec4ui := new Vec4ui(v.val0, v.val1, v.val2, v.val3);
    
    public static function operator implicit(v: Vec4i64): Vec4ui64 := new Vec4ui64(v.val0, v.val1, v.val2, v.val3);
    public static function operator implicit(v: Vec4ui64): Vec4i64 := new Vec4i64(v.val0, v.val1, v.val2, v.val3);
    
  end;
  
  Vec4f = record
    public val0: single;
    public val1: single;
    public val2: single;
    public val3: single;
    
    public constructor(val0, val1, val2, val3: single);
    begin
      self.val0 := val0;
      self.val1 := val1;
      self.val2 := val2;
      self.val3 := val3;
    end;
    
    private function GetValAt(i: integer): single;
    begin
      case i of
        0: Result := self.val0;
        1: Result := self.val1;
        2: Result := self.val2;
        3: Result := self.val3;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..3');
      end;
    end;
    private procedure SetValAt(i: integer; val: single);
    begin
      case i of
        0: self.val0 := val;
        1: self.val1 := val;
        2: self.val2 := val;
        3: self.val3 := val;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..3');
      end;
    end;
    public property val[i: integer]: single read GetValAt write SetValAt; default;
    
    public static function operator-(v: Vec4f): Vec4f := new Vec4f(-v.val0, -v.val1, -v.val2, -v.val3);
    public static function operator*(v: Vec4f; k: single): Vec4f := new Vec4f(v.val0*k, v.val1*k, v.val2*k, v.val3*k);
    public static function operator+(v1, v2: Vec4f): Vec4f := new Vec4f(v1.val0+v2.val0, v1.val1+v2.val1, v1.val2+v2.val2, v1.val3+v2.val3);
    public static function operator-(v1, v2: Vec4f): Vec4f := new Vec4f(v1.val0-v2.val0, v1.val1-v2.val1, v1.val2-v2.val2, v1.val3-v2.val3);
    
    public static function operator implicit(v: Vec1b): Vec4f := new Vec4f(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4f): Vec1b := new Vec1b(Convert.ToSByte(v.val0));
    
    public static function operator implicit(v: Vec1ub): Vec4f := new Vec4f(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4f): Vec1ub := new Vec1ub(Convert.ToByte(v.val0));
    
    public static function operator implicit(v: Vec1s): Vec4f := new Vec4f(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4f): Vec1s := new Vec1s(Convert.ToInt16(v.val0));
    
    public static function operator implicit(v: Vec1us): Vec4f := new Vec4f(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4f): Vec1us := new Vec1us(Convert.ToUInt16(v.val0));
    
    public static function operator implicit(v: Vec1i): Vec4f := new Vec4f(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4f): Vec1i := new Vec1i(Convert.ToInt32(v.val0));
    
    public static function operator implicit(v: Vec1ui): Vec4f := new Vec4f(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4f): Vec1ui := new Vec1ui(Convert.ToUInt32(v.val0));
    
    public static function operator implicit(v: Vec1i64): Vec4f := new Vec4f(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4f): Vec1i64 := new Vec1i64(Convert.ToInt64(v.val0));
    
    public static function operator implicit(v: Vec1ui64): Vec4f := new Vec4f(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4f): Vec1ui64 := new Vec1ui64(Convert.ToUInt64(v.val0));
    
    public static function operator implicit(v: Vec1f): Vec4f := new Vec4f(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4f): Vec1f := new Vec1f(v.val0);
    
    public static function operator implicit(v: Vec1d): Vec4f := new Vec4f(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4f): Vec1d := new Vec1d(v.val0);
    
    public static function operator implicit(v: Vec2b): Vec4f := new Vec4f(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4f): Vec2b := new Vec2b(Convert.ToSByte(v.val0), Convert.ToSByte(v.val1));
    
    public static function operator implicit(v: Vec2ub): Vec4f := new Vec4f(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4f): Vec2ub := new Vec2ub(Convert.ToByte(v.val0), Convert.ToByte(v.val1));
    
    public static function operator implicit(v: Vec2s): Vec4f := new Vec4f(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4f): Vec2s := new Vec2s(Convert.ToInt16(v.val0), Convert.ToInt16(v.val1));
    
    public static function operator implicit(v: Vec2us): Vec4f := new Vec4f(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4f): Vec2us := new Vec2us(Convert.ToUInt16(v.val0), Convert.ToUInt16(v.val1));
    
    public static function operator implicit(v: Vec2i): Vec4f := new Vec4f(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4f): Vec2i := new Vec2i(Convert.ToInt32(v.val0), Convert.ToInt32(v.val1));
    
    public static function operator implicit(v: Vec2ui): Vec4f := new Vec4f(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4f): Vec2ui := new Vec2ui(Convert.ToUInt32(v.val0), Convert.ToUInt32(v.val1));
    
    public static function operator implicit(v: Vec2i64): Vec4f := new Vec4f(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4f): Vec2i64 := new Vec2i64(Convert.ToInt64(v.val0), Convert.ToInt64(v.val1));
    
    public static function operator implicit(v: Vec2ui64): Vec4f := new Vec4f(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4f): Vec2ui64 := new Vec2ui64(Convert.ToUInt64(v.val0), Convert.ToUInt64(v.val1));
    
    public static function operator implicit(v: Vec2f): Vec4f := new Vec4f(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4f): Vec2f := new Vec2f(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2d): Vec4f := new Vec4f(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4f): Vec2d := new Vec2d(v.val0, v.val1);
    
    public static function operator implicit(v: Vec3b): Vec4f := new Vec4f(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4f): Vec3b := new Vec3b(Convert.ToSByte(v.val0), Convert.ToSByte(v.val1), Convert.ToSByte(v.val2));
    
    public static function operator implicit(v: Vec3ub): Vec4f := new Vec4f(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4f): Vec3ub := new Vec3ub(Convert.ToByte(v.val0), Convert.ToByte(v.val1), Convert.ToByte(v.val2));
    
    public static function operator implicit(v: Vec3s): Vec4f := new Vec4f(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4f): Vec3s := new Vec3s(Convert.ToInt16(v.val0), Convert.ToInt16(v.val1), Convert.ToInt16(v.val2));
    
    public static function operator implicit(v: Vec3us): Vec4f := new Vec4f(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4f): Vec3us := new Vec3us(Convert.ToUInt16(v.val0), Convert.ToUInt16(v.val1), Convert.ToUInt16(v.val2));
    
    public static function operator implicit(v: Vec3i): Vec4f := new Vec4f(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4f): Vec3i := new Vec3i(Convert.ToInt32(v.val0), Convert.ToInt32(v.val1), Convert.ToInt32(v.val2));
    
    public static function operator implicit(v: Vec3ui): Vec4f := new Vec4f(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4f): Vec3ui := new Vec3ui(Convert.ToUInt32(v.val0), Convert.ToUInt32(v.val1), Convert.ToUInt32(v.val2));
    
    public static function operator implicit(v: Vec3i64): Vec4f := new Vec4f(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4f): Vec3i64 := new Vec3i64(Convert.ToInt64(v.val0), Convert.ToInt64(v.val1), Convert.ToInt64(v.val2));
    
    public static function operator implicit(v: Vec3ui64): Vec4f := new Vec4f(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4f): Vec3ui64 := new Vec3ui64(Convert.ToUInt64(v.val0), Convert.ToUInt64(v.val1), Convert.ToUInt64(v.val2));
    
    public static function operator implicit(v: Vec3f): Vec4f := new Vec4f(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4f): Vec3f := new Vec3f(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3d): Vec4f := new Vec4f(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4f): Vec3d := new Vec3d(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec4b): Vec4f := new Vec4f(v.val0, v.val1, v.val2, v.val3);
    public static function operator implicit(v: Vec4f): Vec4b := new Vec4b(Convert.ToSByte(v.val0), Convert.ToSByte(v.val1), Convert.ToSByte(v.val2), Convert.ToSByte(v.val3));
    
    public static function operator implicit(v: Vec4ub): Vec4f := new Vec4f(v.val0, v.val1, v.val2, v.val3);
    public static function operator implicit(v: Vec4f): Vec4ub := new Vec4ub(Convert.ToByte(v.val0), Convert.ToByte(v.val1), Convert.ToByte(v.val2), Convert.ToByte(v.val3));
    
    public static function operator implicit(v: Vec4s): Vec4f := new Vec4f(v.val0, v.val1, v.val2, v.val3);
    public static function operator implicit(v: Vec4f): Vec4s := new Vec4s(Convert.ToInt16(v.val0), Convert.ToInt16(v.val1), Convert.ToInt16(v.val2), Convert.ToInt16(v.val3));
    
    public static function operator implicit(v: Vec4us): Vec4f := new Vec4f(v.val0, v.val1, v.val2, v.val3);
    public static function operator implicit(v: Vec4f): Vec4us := new Vec4us(Convert.ToUInt16(v.val0), Convert.ToUInt16(v.val1), Convert.ToUInt16(v.val2), Convert.ToUInt16(v.val3));
    
    public static function operator implicit(v: Vec4i): Vec4f := new Vec4f(v.val0, v.val1, v.val2, v.val3);
    public static function operator implicit(v: Vec4f): Vec4i := new Vec4i(Convert.ToInt32(v.val0), Convert.ToInt32(v.val1), Convert.ToInt32(v.val2), Convert.ToInt32(v.val3));
    
    public static function operator implicit(v: Vec4ui): Vec4f := new Vec4f(v.val0, v.val1, v.val2, v.val3);
    public static function operator implicit(v: Vec4f): Vec4ui := new Vec4ui(Convert.ToUInt32(v.val0), Convert.ToUInt32(v.val1), Convert.ToUInt32(v.val2), Convert.ToUInt32(v.val3));
    
    public static function operator implicit(v: Vec4i64): Vec4f := new Vec4f(v.val0, v.val1, v.val2, v.val3);
    public static function operator implicit(v: Vec4f): Vec4i64 := new Vec4i64(Convert.ToInt64(v.val0), Convert.ToInt64(v.val1), Convert.ToInt64(v.val2), Convert.ToInt64(v.val3));
    
    public static function operator implicit(v: Vec4ui64): Vec4f := new Vec4f(v.val0, v.val1, v.val2, v.val3);
    public static function operator implicit(v: Vec4f): Vec4ui64 := new Vec4ui64(Convert.ToUInt64(v.val0), Convert.ToUInt64(v.val1), Convert.ToUInt64(v.val2), Convert.ToUInt64(v.val3));
    
  end;
  
  Vec4d = record
    public val0: real;
    public val1: real;
    public val2: real;
    public val3: real;
    
    public constructor(val0, val1, val2, val3: real);
    begin
      self.val0 := val0;
      self.val1 := val1;
      self.val2 := val2;
      self.val3 := val3;
    end;
    
    private function GetValAt(i: integer): real;
    begin
      case i of
        0: Result := self.val0;
        1: Result := self.val1;
        2: Result := self.val2;
        3: Result := self.val3;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..3');
      end;
    end;
    private procedure SetValAt(i: integer; val: real);
    begin
      case i of
        0: self.val0 := val;
        1: self.val1 := val;
        2: self.val2 := val;
        3: self.val3 := val;
        else raise new IndexOutOfRangeException('Индекс должен иметь значение 0..3');
      end;
    end;
    public property val[i: integer]: real read GetValAt write SetValAt; default;
    
    public static function operator-(v: Vec4d): Vec4d := new Vec4d(-v.val0, -v.val1, -v.val2, -v.val3);
    public static function operator*(v: Vec4d; k: real): Vec4d := new Vec4d(v.val0*k, v.val1*k, v.val2*k, v.val3*k);
    public static function operator+(v1, v2: Vec4d): Vec4d := new Vec4d(v1.val0+v2.val0, v1.val1+v2.val1, v1.val2+v2.val2, v1.val3+v2.val3);
    public static function operator-(v1, v2: Vec4d): Vec4d := new Vec4d(v1.val0-v2.val0, v1.val1-v2.val1, v1.val2-v2.val2, v1.val3-v2.val3);
    
    public static function operator implicit(v: Vec1b): Vec4d := new Vec4d(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4d): Vec1b := new Vec1b(Convert.ToSByte(v.val0));
    
    public static function operator implicit(v: Vec1ub): Vec4d := new Vec4d(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4d): Vec1ub := new Vec1ub(Convert.ToByte(v.val0));
    
    public static function operator implicit(v: Vec1s): Vec4d := new Vec4d(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4d): Vec1s := new Vec1s(Convert.ToInt16(v.val0));
    
    public static function operator implicit(v: Vec1us): Vec4d := new Vec4d(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4d): Vec1us := new Vec1us(Convert.ToUInt16(v.val0));
    
    public static function operator implicit(v: Vec1i): Vec4d := new Vec4d(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4d): Vec1i := new Vec1i(Convert.ToInt32(v.val0));
    
    public static function operator implicit(v: Vec1ui): Vec4d := new Vec4d(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4d): Vec1ui := new Vec1ui(Convert.ToUInt32(v.val0));
    
    public static function operator implicit(v: Vec1i64): Vec4d := new Vec4d(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4d): Vec1i64 := new Vec1i64(Convert.ToInt64(v.val0));
    
    public static function operator implicit(v: Vec1ui64): Vec4d := new Vec4d(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4d): Vec1ui64 := new Vec1ui64(Convert.ToUInt64(v.val0));
    
    public static function operator implicit(v: Vec1f): Vec4d := new Vec4d(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4d): Vec1f := new Vec1f(v.val0);
    
    public static function operator implicit(v: Vec1d): Vec4d := new Vec4d(v.val0, 0, 0, 0);
    public static function operator implicit(v: Vec4d): Vec1d := new Vec1d(v.val0);
    
    public static function operator implicit(v: Vec2b): Vec4d := new Vec4d(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4d): Vec2b := new Vec2b(Convert.ToSByte(v.val0), Convert.ToSByte(v.val1));
    
    public static function operator implicit(v: Vec2ub): Vec4d := new Vec4d(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4d): Vec2ub := new Vec2ub(Convert.ToByte(v.val0), Convert.ToByte(v.val1));
    
    public static function operator implicit(v: Vec2s): Vec4d := new Vec4d(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4d): Vec2s := new Vec2s(Convert.ToInt16(v.val0), Convert.ToInt16(v.val1));
    
    public static function operator implicit(v: Vec2us): Vec4d := new Vec4d(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4d): Vec2us := new Vec2us(Convert.ToUInt16(v.val0), Convert.ToUInt16(v.val1));
    
    public static function operator implicit(v: Vec2i): Vec4d := new Vec4d(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4d): Vec2i := new Vec2i(Convert.ToInt32(v.val0), Convert.ToInt32(v.val1));
    
    public static function operator implicit(v: Vec2ui): Vec4d := new Vec4d(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4d): Vec2ui := new Vec2ui(Convert.ToUInt32(v.val0), Convert.ToUInt32(v.val1));
    
    public static function operator implicit(v: Vec2i64): Vec4d := new Vec4d(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4d): Vec2i64 := new Vec2i64(Convert.ToInt64(v.val0), Convert.ToInt64(v.val1));
    
    public static function operator implicit(v: Vec2ui64): Vec4d := new Vec4d(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4d): Vec2ui64 := new Vec2ui64(Convert.ToUInt64(v.val0), Convert.ToUInt64(v.val1));
    
    public static function operator implicit(v: Vec2f): Vec4d := new Vec4d(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4d): Vec2f := new Vec2f(v.val0, v.val1);
    
    public static function operator implicit(v: Vec2d): Vec4d := new Vec4d(v.val0, v.val1, 0, 0);
    public static function operator implicit(v: Vec4d): Vec2d := new Vec2d(v.val0, v.val1);
    
    public static function operator implicit(v: Vec3b): Vec4d := new Vec4d(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4d): Vec3b := new Vec3b(Convert.ToSByte(v.val0), Convert.ToSByte(v.val1), Convert.ToSByte(v.val2));
    
    public static function operator implicit(v: Vec3ub): Vec4d := new Vec4d(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4d): Vec3ub := new Vec3ub(Convert.ToByte(v.val0), Convert.ToByte(v.val1), Convert.ToByte(v.val2));
    
    public static function operator implicit(v: Vec3s): Vec4d := new Vec4d(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4d): Vec3s := new Vec3s(Convert.ToInt16(v.val0), Convert.ToInt16(v.val1), Convert.ToInt16(v.val2));
    
    public static function operator implicit(v: Vec3us): Vec4d := new Vec4d(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4d): Vec3us := new Vec3us(Convert.ToUInt16(v.val0), Convert.ToUInt16(v.val1), Convert.ToUInt16(v.val2));
    
    public static function operator implicit(v: Vec3i): Vec4d := new Vec4d(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4d): Vec3i := new Vec3i(Convert.ToInt32(v.val0), Convert.ToInt32(v.val1), Convert.ToInt32(v.val2));
    
    public static function operator implicit(v: Vec3ui): Vec4d := new Vec4d(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4d): Vec3ui := new Vec3ui(Convert.ToUInt32(v.val0), Convert.ToUInt32(v.val1), Convert.ToUInt32(v.val2));
    
    public static function operator implicit(v: Vec3i64): Vec4d := new Vec4d(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4d): Vec3i64 := new Vec3i64(Convert.ToInt64(v.val0), Convert.ToInt64(v.val1), Convert.ToInt64(v.val2));
    
    public static function operator implicit(v: Vec3ui64): Vec4d := new Vec4d(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4d): Vec3ui64 := new Vec3ui64(Convert.ToUInt64(v.val0), Convert.ToUInt64(v.val1), Convert.ToUInt64(v.val2));
    
    public static function operator implicit(v: Vec3f): Vec4d := new Vec4d(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4d): Vec3f := new Vec3f(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec3d): Vec4d := new Vec4d(v.val0, v.val1, v.val2, 0);
    public static function operator implicit(v: Vec4d): Vec3d := new Vec3d(v.val0, v.val1, v.val2);
    
    public static function operator implicit(v: Vec4b): Vec4d := new Vec4d(v.val0, v.val1, v.val2, v.val3);
    public static function operator implicit(v: Vec4d): Vec4b := new Vec4b(Convert.ToSByte(v.val0), Convert.ToSByte(v.val1), Convert.ToSByte(v.val2), Convert.ToSByte(v.val3));
    
    public static function operator implicit(v: Vec4ub): Vec4d := new Vec4d(v.val0, v.val1, v.val2, v.val3);
    public static function operator implicit(v: Vec4d): Vec4ub := new Vec4ub(Convert.ToByte(v.val0), Convert.ToByte(v.val1), Convert.ToByte(v.val2), Convert.ToByte(v.val3));
    
    public static function operator implicit(v: Vec4s): Vec4d := new Vec4d(v.val0, v.val1, v.val2, v.val3);
    public static function operator implicit(v: Vec4d): Vec4s := new Vec4s(Convert.ToInt16(v.val0), Convert.ToInt16(v.val1), Convert.ToInt16(v.val2), Convert.ToInt16(v.val3));
    
    public static function operator implicit(v: Vec4us): Vec4d := new Vec4d(v.val0, v.val1, v.val2, v.val3);
    public static function operator implicit(v: Vec4d): Vec4us := new Vec4us(Convert.ToUInt16(v.val0), Convert.ToUInt16(v.val1), Convert.ToUInt16(v.val2), Convert.ToUInt16(v.val3));
    
    public static function operator implicit(v: Vec4i): Vec4d := new Vec4d(v.val0, v.val1, v.val2, v.val3);
    public static function operator implicit(v: Vec4d): Vec4i := new Vec4i(Convert.ToInt32(v.val0), Convert.ToInt32(v.val1), Convert.ToInt32(v.val2), Convert.ToInt32(v.val3));
    
    public static function operator implicit(v: Vec4ui): Vec4d := new Vec4d(v.val0, v.val1, v.val2, v.val3);
    public static function operator implicit(v: Vec4d): Vec4ui := new Vec4ui(Convert.ToUInt32(v.val0), Convert.ToUInt32(v.val1), Convert.ToUInt32(v.val2), Convert.ToUInt32(v.val3));
    
    public static function operator implicit(v: Vec4i64): Vec4d := new Vec4d(v.val0, v.val1, v.val2, v.val3);
    public static function operator implicit(v: Vec4d): Vec4i64 := new Vec4i64(Convert.ToInt64(v.val0), Convert.ToInt64(v.val1), Convert.ToInt64(v.val2), Convert.ToInt64(v.val3));
    
    public static function operator implicit(v: Vec4ui64): Vec4d := new Vec4d(v.val0, v.val1, v.val2, v.val3);
    public static function operator implicit(v: Vec4d): Vec4ui64 := new Vec4ui64(Convert.ToUInt64(v.val0), Convert.ToUInt64(v.val1), Convert.ToUInt64(v.val2), Convert.ToUInt64(v.val3));
    
    public static function operator implicit(v: Vec4f): Vec4d := new Vec4d(v.val0, v.val1, v.val2, v.val3);
    public static function operator implicit(v: Vec4d): Vec4f := new Vec4f(v.val0, v.val1, v.val2, v.val3);
    
  end;
  
  {$endregion Vec4}
  
  {$endregion Vec}
  
  {$region Mtr}
  
  Mtr2x2f = record
    public val00, val01: single;
    public val10, val11: single;
    
    public constructor(val00, val01, val10, val11: single);
    begin
      self.val00 := val00;
      self.val01 := val01;
      self.val10 := val10;
      self.val11 := val11;
    end;
    
    private function GetValAt(y,x: integer): single;
    begin
      case y of
        0:
        case x of
          0: Result := self.val00;
          1: Result := self.val01;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..1');
        end;
        1:
        case x of
          0: Result := self.val10;
          1: Result := self.val11;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..1');
        end;
        else raise new IndexOutOfRangeException('Индекс "Y" должен иметь значение 0..1');
      end;
    end;
    private procedure SetValAt(y,x: integer; val: single);
    begin
      case y of
        0:
        case x of
          0: self.val00 := val;
          1: self.val01 := val;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..1');
        end;
        1:
        case x of
          0: self.val10 := val;
          1: self.val11 := val;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..1');
        end;
        else raise new IndexOutOfRangeException('Индекс "Y" должен иметь значение 0..1');
      end;
    end;
    public property val[y,x: integer]: single read GetValAt write SetValAt; default;
    
    public static property Identity: Mtr2x2f read new Mtr2x2f(1.0, 0.0, 0.0, 1.0);
    
    public property Row0: Vec2f read new Vec2f(self.val00, self.val01) write begin self.val00 := value.val0; self.val01 := value.val1; end;
    public property Row1: Vec2f read new Vec2f(self.val10, self.val11) write begin self.val10 := value.val0; self.val11 := value.val1; end;
    public property Row[y: integer]: Vec2f read y=0?Row0:y=1?Row1:Arr&<Vec2f>[y] write
    case y of
      0: Row0 := value;
      1: Row1 := value;
      else raise new IndexOutOfRangeException('Номер строчки должен иметь значение 0..1');
    end;
    
    public property Col0: Vec2f read new Vec2f(self.val00, self.val10) write begin self.val00 := value.val0; self.val10 := value.val1; end;
    public property Col1: Vec2f read new Vec2f(self.val01, self.val11) write begin self.val01 := value.val0; self.val11 := value.val1; end;
    public property Col[x: integer]: Vec2f read x=0?Col0:x=1?Col1:Arr&<Vec2f>[x] write
    case x of
      0: Col0 := value;
      1: Col1 := value;
      else raise new IndexOutOfRangeException('Номер столбца должен иметь значение 0..1');
    end;
    
    public property RowPtr0: ^Vec2f read pointer(IntPtr(pointer(@self)) + 0);
    public property RowPtr1: ^Vec2f read pointer(IntPtr(pointer(@self)) + 8);
    public property RowPtr[x: integer]: ^Vec2f read pointer(IntPtr(pointer(@self)) + x*8);
    
    public static function operator*(m: Mtr2x2f; v: Vec2f): Vec2f := new Vec2f(m.val00*v.val0+m.val01*v.val1, m.val10*v.val0+m.val11*v.val1);
    public static function operator*(v: Vec2f; m: Mtr2x2f): Vec2f := new Vec2f(m.val00*v.val0+m.val10*v.val1, m.val01*v.val0+m.val11*v.val1);
    
  end;
  Mtr2f = Mtr2x2f;
  
  Mtr3x3f = record
    public val00, val01, val02: single;
    public val10, val11, val12: single;
    public val20, val21, val22: single;
    
    public constructor(val00, val01, val02, val10, val11, val12, val20, val21, val22: single);
    begin
      self.val00 := val00;
      self.val01 := val01;
      self.val02 := val02;
      self.val10 := val10;
      self.val11 := val11;
      self.val12 := val12;
      self.val20 := val20;
      self.val21 := val21;
      self.val22 := val22;
    end;
    
    private function GetValAt(y,x: integer): single;
    begin
      case y of
        0:
        case x of
          0: Result := self.val00;
          1: Result := self.val01;
          2: Result := self.val02;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..2');
        end;
        1:
        case x of
          0: Result := self.val10;
          1: Result := self.val11;
          2: Result := self.val12;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..2');
        end;
        2:
        case x of
          0: Result := self.val20;
          1: Result := self.val21;
          2: Result := self.val22;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..2');
        end;
        else raise new IndexOutOfRangeException('Индекс "Y" должен иметь значение 0..2');
      end;
    end;
    private procedure SetValAt(y,x: integer; val: single);
    begin
      case y of
        0:
        case x of
          0: self.val00 := val;
          1: self.val01 := val;
          2: self.val02 := val;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..2');
        end;
        1:
        case x of
          0: self.val10 := val;
          1: self.val11 := val;
          2: self.val12 := val;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..2');
        end;
        2:
        case x of
          0: self.val20 := val;
          1: self.val21 := val;
          2: self.val22 := val;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..2');
        end;
        else raise new IndexOutOfRangeException('Индекс "Y" должен иметь значение 0..2');
      end;
    end;
    public property val[y,x: integer]: single read GetValAt write SetValAt; default;
    
    public static property Identity: Mtr3x3f read new Mtr3x3f(1.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 1.0);
    
    public property Row0: Vec3f read new Vec3f(self.val00, self.val01, self.val02) write begin self.val00 := value.val0; self.val01 := value.val1; self.val02 := value.val2; end;
    public property Row1: Vec3f read new Vec3f(self.val10, self.val11, self.val12) write begin self.val10 := value.val0; self.val11 := value.val1; self.val12 := value.val2; end;
    public property Row2: Vec3f read new Vec3f(self.val20, self.val21, self.val22) write begin self.val20 := value.val0; self.val21 := value.val1; self.val22 := value.val2; end;
    public property Row[y: integer]: Vec3f read y=0?Row0:y=1?Row1:y=2?Row2:Arr&<Vec3f>[y] write
    case y of
      0: Row0 := value;
      1: Row1 := value;
      2: Row2 := value;
      else raise new IndexOutOfRangeException('Номер строчки должен иметь значение 0..2');
    end;
    
    public property Col0: Vec3f read new Vec3f(self.val00, self.val10, self.val20) write begin self.val00 := value.val0; self.val10 := value.val1; self.val20 := value.val2; end;
    public property Col1: Vec3f read new Vec3f(self.val01, self.val11, self.val21) write begin self.val01 := value.val0; self.val11 := value.val1; self.val21 := value.val2; end;
    public property Col2: Vec3f read new Vec3f(self.val02, self.val12, self.val22) write begin self.val02 := value.val0; self.val12 := value.val1; self.val22 := value.val2; end;
    public property Col[x: integer]: Vec3f read x=0?Col0:x=1?Col1:x=2?Col2:Arr&<Vec3f>[x] write
    case x of
      0: Col0 := value;
      1: Col1 := value;
      2: Col2 := value;
      else raise new IndexOutOfRangeException('Номер столбца должен иметь значение 0..2');
    end;
    
    public property RowPtr0: ^Vec3f read pointer(IntPtr(pointer(@self)) + 0);
    public property RowPtr1: ^Vec3f read pointer(IntPtr(pointer(@self)) + 12);
    public property RowPtr2: ^Vec3f read pointer(IntPtr(pointer(@self)) + 24);
    public property RowPtr[x: integer]: ^Vec3f read pointer(IntPtr(pointer(@self)) + x*12);
    
    public static function operator*(m: Mtr3x3f; v: Vec3f): Vec3f := new Vec3f(m.val00*v.val0+m.val01*v.val1+m.val02*v.val2, m.val10*v.val0+m.val11*v.val1+m.val12*v.val2, m.val20*v.val0+m.val21*v.val1+m.val22*v.val2);
    public static function operator*(v: Vec3f; m: Mtr3x3f): Vec3f := new Vec3f(m.val00*v.val0+m.val10*v.val1+m.val20*v.val2, m.val01*v.val0+m.val11*v.val1+m.val21*v.val2, m.val02*v.val0+m.val12*v.val1+m.val22*v.val2);
    
    public static function operator implicit(m: Mtr2x2f): Mtr3x3f := new Mtr3x3f(m.val00, m.val01, 0.0, m.val10, m.val11, 0.0, 0.0, 0.0, 0.0);
    public static function operator implicit(m: Mtr3x3f): Mtr2x2f := new Mtr2x2f(m.val00, m.val01, m.val10, m.val11);
    
  end;
  Mtr3f = Mtr3x3f;
  
  Mtr4x4f = record
    public val00, val01, val02, val03: single;
    public val10, val11, val12, val13: single;
    public val20, val21, val22, val23: single;
    public val30, val31, val32, val33: single;
    
    public constructor(val00, val01, val02, val03, val10, val11, val12, val13, val20, val21, val22, val23, val30, val31, val32, val33: single);
    begin
      self.val00 := val00;
      self.val01 := val01;
      self.val02 := val02;
      self.val03 := val03;
      self.val10 := val10;
      self.val11 := val11;
      self.val12 := val12;
      self.val13 := val13;
      self.val20 := val20;
      self.val21 := val21;
      self.val22 := val22;
      self.val23 := val23;
      self.val30 := val30;
      self.val31 := val31;
      self.val32 := val32;
      self.val33 := val33;
    end;
    
    private function GetValAt(y,x: integer): single;
    begin
      case y of
        0:
        case x of
          0: Result := self.val00;
          1: Result := self.val01;
          2: Result := self.val02;
          3: Result := self.val03;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..3');
        end;
        1:
        case x of
          0: Result := self.val10;
          1: Result := self.val11;
          2: Result := self.val12;
          3: Result := self.val13;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..3');
        end;
        2:
        case x of
          0: Result := self.val20;
          1: Result := self.val21;
          2: Result := self.val22;
          3: Result := self.val23;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..3');
        end;
        3:
        case x of
          0: Result := self.val30;
          1: Result := self.val31;
          2: Result := self.val32;
          3: Result := self.val33;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..3');
        end;
        else raise new IndexOutOfRangeException('Индекс "Y" должен иметь значение 0..3');
      end;
    end;
    private procedure SetValAt(y,x: integer; val: single);
    begin
      case y of
        0:
        case x of
          0: self.val00 := val;
          1: self.val01 := val;
          2: self.val02 := val;
          3: self.val03 := val;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..3');
        end;
        1:
        case x of
          0: self.val10 := val;
          1: self.val11 := val;
          2: self.val12 := val;
          3: self.val13 := val;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..3');
        end;
        2:
        case x of
          0: self.val20 := val;
          1: self.val21 := val;
          2: self.val22 := val;
          3: self.val23 := val;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..3');
        end;
        3:
        case x of
          0: self.val30 := val;
          1: self.val31 := val;
          2: self.val32 := val;
          3: self.val33 := val;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..3');
        end;
        else raise new IndexOutOfRangeException('Индекс "Y" должен иметь значение 0..3');
      end;
    end;
    public property val[y,x: integer]: single read GetValAt write SetValAt; default;
    
    public static property Identity: Mtr4x4f read new Mtr4x4f(1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 1.0);
    
    public property Row0: Vec4f read new Vec4f(self.val00, self.val01, self.val02, self.val03) write begin self.val00 := value.val0; self.val01 := value.val1; self.val02 := value.val2; self.val03 := value.val3; end;
    public property Row1: Vec4f read new Vec4f(self.val10, self.val11, self.val12, self.val13) write begin self.val10 := value.val0; self.val11 := value.val1; self.val12 := value.val2; self.val13 := value.val3; end;
    public property Row2: Vec4f read new Vec4f(self.val20, self.val21, self.val22, self.val23) write begin self.val20 := value.val0; self.val21 := value.val1; self.val22 := value.val2; self.val23 := value.val3; end;
    public property Row3: Vec4f read new Vec4f(self.val30, self.val31, self.val32, self.val33) write begin self.val30 := value.val0; self.val31 := value.val1; self.val32 := value.val2; self.val33 := value.val3; end;
    public property Row[y: integer]: Vec4f read y=0?Row0:y=1?Row1:y=2?Row2:y=3?Row3:Arr&<Vec4f>[y] write
    case y of
      0: Row0 := value;
      1: Row1 := value;
      2: Row2 := value;
      3: Row3 := value;
      else raise new IndexOutOfRangeException('Номер строчки должен иметь значение 0..3');
    end;
    
    public property Col0: Vec4f read new Vec4f(self.val00, self.val10, self.val20, self.val30) write begin self.val00 := value.val0; self.val10 := value.val1; self.val20 := value.val2; self.val30 := value.val3; end;
    public property Col1: Vec4f read new Vec4f(self.val01, self.val11, self.val21, self.val31) write begin self.val01 := value.val0; self.val11 := value.val1; self.val21 := value.val2; self.val31 := value.val3; end;
    public property Col2: Vec4f read new Vec4f(self.val02, self.val12, self.val22, self.val32) write begin self.val02 := value.val0; self.val12 := value.val1; self.val22 := value.val2; self.val32 := value.val3; end;
    public property Col3: Vec4f read new Vec4f(self.val03, self.val13, self.val23, self.val33) write begin self.val03 := value.val0; self.val13 := value.val1; self.val23 := value.val2; self.val33 := value.val3; end;
    public property Col[x: integer]: Vec4f read x=0?Col0:x=1?Col1:x=2?Col2:x=3?Col3:Arr&<Vec4f>[x] write
    case x of
      0: Col0 := value;
      1: Col1 := value;
      2: Col2 := value;
      3: Col3 := value;
      else raise new IndexOutOfRangeException('Номер столбца должен иметь значение 0..3');
    end;
    
    public property RowPtr0: ^Vec4f read pointer(IntPtr(pointer(@self)) + 0);
    public property RowPtr1: ^Vec4f read pointer(IntPtr(pointer(@self)) + 16);
    public property RowPtr2: ^Vec4f read pointer(IntPtr(pointer(@self)) + 32);
    public property RowPtr3: ^Vec4f read pointer(IntPtr(pointer(@self)) + 48);
    public property RowPtr[x: integer]: ^Vec4f read pointer(IntPtr(pointer(@self)) + x*16);
    
    public static function operator*(m: Mtr4x4f; v: Vec4f): Vec4f := new Vec4f(m.val00*v.val0+m.val01*v.val1+m.val02*v.val2+m.val03*v.val3, m.val10*v.val0+m.val11*v.val1+m.val12*v.val2+m.val13*v.val3, m.val20*v.val0+m.val21*v.val1+m.val22*v.val2+m.val23*v.val3, m.val30*v.val0+m.val31*v.val1+m.val32*v.val2+m.val33*v.val3);
    public static function operator*(v: Vec4f; m: Mtr4x4f): Vec4f := new Vec4f(m.val00*v.val0+m.val10*v.val1+m.val20*v.val2+m.val30*v.val3, m.val01*v.val0+m.val11*v.val1+m.val21*v.val2+m.val31*v.val3, m.val02*v.val0+m.val12*v.val1+m.val22*v.val2+m.val32*v.val3, m.val03*v.val0+m.val13*v.val1+m.val23*v.val2+m.val33*v.val3);
    
    public static function operator implicit(m: Mtr2x2f): Mtr4x4f := new Mtr4x4f(m.val00, m.val01, 0.0, 0.0, m.val10, m.val11, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0);
    public static function operator implicit(m: Mtr4x4f): Mtr2x2f := new Mtr2x2f(m.val00, m.val01, m.val10, m.val11);
    
    public static function operator implicit(m: Mtr3x3f): Mtr4x4f := new Mtr4x4f(m.val00, m.val01, m.val02, 0.0, m.val10, m.val11, m.val12, 0.0, m.val20, m.val21, m.val22, 0.0, 0.0, 0.0, 0.0, 0.0);
    public static function operator implicit(m: Mtr4x4f): Mtr3x3f := new Mtr3x3f(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12, m.val20, m.val21, m.val22);
    
  end;
  Mtr4f = Mtr4x4f;
  
  Mtr2x3f = record
    public val00, val01, val02: single;
    public val10, val11, val12: single;
    
    public constructor(val00, val01, val02, val10, val11, val12: single);
    begin
      self.val00 := val00;
      self.val01 := val01;
      self.val02 := val02;
      self.val10 := val10;
      self.val11 := val11;
      self.val12 := val12;
    end;
    
    private function GetValAt(y,x: integer): single;
    begin
      case y of
        0:
        case x of
          0: Result := self.val00;
          1: Result := self.val01;
          2: Result := self.val02;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..2');
        end;
        1:
        case x of
          0: Result := self.val10;
          1: Result := self.val11;
          2: Result := self.val12;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..2');
        end;
        else raise new IndexOutOfRangeException('Индекс "Y" должен иметь значение 0..1');
      end;
    end;
    private procedure SetValAt(y,x: integer; val: single);
    begin
      case y of
        0:
        case x of
          0: self.val00 := val;
          1: self.val01 := val;
          2: self.val02 := val;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..2');
        end;
        1:
        case x of
          0: self.val10 := val;
          1: self.val11 := val;
          2: self.val12 := val;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..2');
        end;
        else raise new IndexOutOfRangeException('Индекс "Y" должен иметь значение 0..1');
      end;
    end;
    public property val[y,x: integer]: single read GetValAt write SetValAt; default;
    
    public static property Identity: Mtr2x3f read new Mtr2x3f(1.0, 0.0, 0.0, 0.0, 1.0, 0.0);
    
    public property Row0: Vec3f read new Vec3f(self.val00, self.val01, self.val02) write begin self.val00 := value.val0; self.val01 := value.val1; self.val02 := value.val2; end;
    public property Row1: Vec3f read new Vec3f(self.val10, self.val11, self.val12) write begin self.val10 := value.val0; self.val11 := value.val1; self.val12 := value.val2; end;
    public property Row[y: integer]: Vec3f read y=0?Row0:y=1?Row1:Arr&<Vec3f>[y] write
    case y of
      0: Row0 := value;
      1: Row1 := value;
      else raise new IndexOutOfRangeException('Номер строчки должен иметь значение 0..1');
    end;
    
    public property Col0: Vec2f read new Vec2f(self.val00, self.val10) write begin self.val00 := value.val0; self.val10 := value.val1; end;
    public property Col1: Vec2f read new Vec2f(self.val01, self.val11) write begin self.val01 := value.val0; self.val11 := value.val1; end;
    public property Col2: Vec2f read new Vec2f(self.val02, self.val12) write begin self.val02 := value.val0; self.val12 := value.val1; end;
    public property Col[x: integer]: Vec2f read x=0?Col0:x=1?Col1:x=2?Col2:Arr&<Vec2f>[x] write
    case x of
      0: Col0 := value;
      1: Col1 := value;
      2: Col2 := value;
      else raise new IndexOutOfRangeException('Номер столбца должен иметь значение 0..2');
    end;
    
    public property RowPtr0: ^Vec3f read pointer(IntPtr(pointer(@self)) + 0);
    public property RowPtr1: ^Vec3f read pointer(IntPtr(pointer(@self)) + 12);
    public property RowPtr[x: integer]: ^Vec3f read pointer(IntPtr(pointer(@self)) + x*12);
    
    public static function operator*(m: Mtr2x3f; v: Vec3f): Vec2f := new Vec2f(m.val00*v.val0+m.val01*v.val1+m.val02*v.val2, m.val10*v.val0+m.val11*v.val1+m.val12*v.val2);
    public static function operator*(v: Vec2f; m: Mtr2x3f): Vec3f := new Vec3f(m.val00*v.val0+m.val10*v.val1, m.val01*v.val0+m.val11*v.val1, m.val02*v.val0+m.val12*v.val1);
    
    public static function operator implicit(m: Mtr2x2f): Mtr2x3f := new Mtr2x3f(m.val00, m.val01, 0.0, m.val10, m.val11, 0.0);
    public static function operator implicit(m: Mtr2x3f): Mtr2x2f := new Mtr2x2f(m.val00, m.val01, m.val10, m.val11);
    
    public static function operator implicit(m: Mtr3x3f): Mtr2x3f := new Mtr2x3f(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12);
    public static function operator implicit(m: Mtr2x3f): Mtr3x3f := new Mtr3x3f(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12, 0.0, 0.0, 0.0);
    
    public static function operator implicit(m: Mtr4x4f): Mtr2x3f := new Mtr2x3f(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12);
    public static function operator implicit(m: Mtr2x3f): Mtr4x4f := new Mtr4x4f(m.val00, m.val01, m.val02, 0.0, m.val10, m.val11, m.val12, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0);
    
  end;
  
  Mtr3x2f = record
    public val00, val01: single;
    public val10, val11: single;
    public val20, val21: single;
    
    public constructor(val00, val01, val10, val11, val20, val21: single);
    begin
      self.val00 := val00;
      self.val01 := val01;
      self.val10 := val10;
      self.val11 := val11;
      self.val20 := val20;
      self.val21 := val21;
    end;
    
    private function GetValAt(y,x: integer): single;
    begin
      case y of
        0:
        case x of
          0: Result := self.val00;
          1: Result := self.val01;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..1');
        end;
        1:
        case x of
          0: Result := self.val10;
          1: Result := self.val11;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..1');
        end;
        2:
        case x of
          0: Result := self.val20;
          1: Result := self.val21;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..1');
        end;
        else raise new IndexOutOfRangeException('Индекс "Y" должен иметь значение 0..2');
      end;
    end;
    private procedure SetValAt(y,x: integer; val: single);
    begin
      case y of
        0:
        case x of
          0: self.val00 := val;
          1: self.val01 := val;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..1');
        end;
        1:
        case x of
          0: self.val10 := val;
          1: self.val11 := val;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..1');
        end;
        2:
        case x of
          0: self.val20 := val;
          1: self.val21 := val;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..1');
        end;
        else raise new IndexOutOfRangeException('Индекс "Y" должен иметь значение 0..2');
      end;
    end;
    public property val[y,x: integer]: single read GetValAt write SetValAt; default;
    
    public static property Identity: Mtr3x2f read new Mtr3x2f(1.0, 0.0, 0.0, 1.0, 0.0, 0.0);
    
    public property Row0: Vec2f read new Vec2f(self.val00, self.val01) write begin self.val00 := value.val0; self.val01 := value.val1; end;
    public property Row1: Vec2f read new Vec2f(self.val10, self.val11) write begin self.val10 := value.val0; self.val11 := value.val1; end;
    public property Row2: Vec2f read new Vec2f(self.val20, self.val21) write begin self.val20 := value.val0; self.val21 := value.val1; end;
    public property Row[y: integer]: Vec2f read y=0?Row0:y=1?Row1:y=2?Row2:Arr&<Vec2f>[y] write
    case y of
      0: Row0 := value;
      1: Row1 := value;
      2: Row2 := value;
      else raise new IndexOutOfRangeException('Номер строчки должен иметь значение 0..2');
    end;
    
    public property Col0: Vec3f read new Vec3f(self.val00, self.val10, self.val20) write begin self.val00 := value.val0; self.val10 := value.val1; self.val20 := value.val2; end;
    public property Col1: Vec3f read new Vec3f(self.val01, self.val11, self.val21) write begin self.val01 := value.val0; self.val11 := value.val1; self.val21 := value.val2; end;
    public property Col[x: integer]: Vec3f read x=0?Col0:x=1?Col1:Arr&<Vec3f>[x] write
    case x of
      0: Col0 := value;
      1: Col1 := value;
      else raise new IndexOutOfRangeException('Номер столбца должен иметь значение 0..1');
    end;
    
    public property RowPtr0: ^Vec2f read pointer(IntPtr(pointer(@self)) + 0);
    public property RowPtr1: ^Vec2f read pointer(IntPtr(pointer(@self)) + 8);
    public property RowPtr2: ^Vec2f read pointer(IntPtr(pointer(@self)) + 16);
    public property RowPtr[x: integer]: ^Vec2f read pointer(IntPtr(pointer(@self)) + x*8);
    
    public static function operator*(m: Mtr3x2f; v: Vec2f): Vec3f := new Vec3f(m.val00*v.val0+m.val01*v.val1, m.val10*v.val0+m.val11*v.val1, m.val20*v.val0+m.val21*v.val1);
    public static function operator*(v: Vec3f; m: Mtr3x2f): Vec2f := new Vec2f(m.val00*v.val0+m.val10*v.val1+m.val20*v.val2, m.val01*v.val0+m.val11*v.val1+m.val21*v.val2);
    
    public static function operator implicit(m: Mtr2x2f): Mtr3x2f := new Mtr3x2f(m.val00, m.val01, m.val10, m.val11, 0.0, 0.0);
    public static function operator implicit(m: Mtr3x2f): Mtr2x2f := new Mtr2x2f(m.val00, m.val01, m.val10, m.val11);
    
    public static function operator implicit(m: Mtr3x3f): Mtr3x2f := new Mtr3x2f(m.val00, m.val01, m.val10, m.val11, m.val20, m.val21);
    public static function operator implicit(m: Mtr3x2f): Mtr3x3f := new Mtr3x3f(m.val00, m.val01, 0.0, m.val10, m.val11, 0.0, m.val20, m.val21, 0.0);
    
    public static function operator implicit(m: Mtr4x4f): Mtr3x2f := new Mtr3x2f(m.val00, m.val01, m.val10, m.val11, m.val20, m.val21);
    public static function operator implicit(m: Mtr3x2f): Mtr4x4f := new Mtr4x4f(m.val00, m.val01, 0.0, 0.0, m.val10, m.val11, 0.0, 0.0, m.val20, m.val21, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0);
    
    public static function operator implicit(m: Mtr2x3f): Mtr3x2f := new Mtr3x2f(m.val00, m.val01, m.val10, m.val11, 0.0, 0.0);
    public static function operator implicit(m: Mtr3x2f): Mtr2x3f := new Mtr2x3f(m.val00, m.val01, 0.0, m.val10, m.val11, 0.0);
    
  end;
  
  Mtr2x4f = record
    public val00, val01, val02, val03: single;
    public val10, val11, val12, val13: single;
    
    public constructor(val00, val01, val02, val03, val10, val11, val12, val13: single);
    begin
      self.val00 := val00;
      self.val01 := val01;
      self.val02 := val02;
      self.val03 := val03;
      self.val10 := val10;
      self.val11 := val11;
      self.val12 := val12;
      self.val13 := val13;
    end;
    
    private function GetValAt(y,x: integer): single;
    begin
      case y of
        0:
        case x of
          0: Result := self.val00;
          1: Result := self.val01;
          2: Result := self.val02;
          3: Result := self.val03;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..3');
        end;
        1:
        case x of
          0: Result := self.val10;
          1: Result := self.val11;
          2: Result := self.val12;
          3: Result := self.val13;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..3');
        end;
        else raise new IndexOutOfRangeException('Индекс "Y" должен иметь значение 0..1');
      end;
    end;
    private procedure SetValAt(y,x: integer; val: single);
    begin
      case y of
        0:
        case x of
          0: self.val00 := val;
          1: self.val01 := val;
          2: self.val02 := val;
          3: self.val03 := val;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..3');
        end;
        1:
        case x of
          0: self.val10 := val;
          1: self.val11 := val;
          2: self.val12 := val;
          3: self.val13 := val;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..3');
        end;
        else raise new IndexOutOfRangeException('Индекс "Y" должен иметь значение 0..1');
      end;
    end;
    public property val[y,x: integer]: single read GetValAt write SetValAt; default;
    
    public static property Identity: Mtr2x4f read new Mtr2x4f(1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0);
    
    public property Row0: Vec4f read new Vec4f(self.val00, self.val01, self.val02, self.val03) write begin self.val00 := value.val0; self.val01 := value.val1; self.val02 := value.val2; self.val03 := value.val3; end;
    public property Row1: Vec4f read new Vec4f(self.val10, self.val11, self.val12, self.val13) write begin self.val10 := value.val0; self.val11 := value.val1; self.val12 := value.val2; self.val13 := value.val3; end;
    public property Row[y: integer]: Vec4f read y=0?Row0:y=1?Row1:Arr&<Vec4f>[y] write
    case y of
      0: Row0 := value;
      1: Row1 := value;
      else raise new IndexOutOfRangeException('Номер строчки должен иметь значение 0..1');
    end;
    
    public property Col0: Vec2f read new Vec2f(self.val00, self.val10) write begin self.val00 := value.val0; self.val10 := value.val1; end;
    public property Col1: Vec2f read new Vec2f(self.val01, self.val11) write begin self.val01 := value.val0; self.val11 := value.val1; end;
    public property Col2: Vec2f read new Vec2f(self.val02, self.val12) write begin self.val02 := value.val0; self.val12 := value.val1; end;
    public property Col3: Vec2f read new Vec2f(self.val03, self.val13) write begin self.val03 := value.val0; self.val13 := value.val1; end;
    public property Col[x: integer]: Vec2f read x=0?Col0:x=1?Col1:x=2?Col2:x=3?Col3:Arr&<Vec2f>[x] write
    case x of
      0: Col0 := value;
      1: Col1 := value;
      2: Col2 := value;
      3: Col3 := value;
      else raise new IndexOutOfRangeException('Номер столбца должен иметь значение 0..3');
    end;
    
    public property RowPtr0: ^Vec4f read pointer(IntPtr(pointer(@self)) + 0);
    public property RowPtr1: ^Vec4f read pointer(IntPtr(pointer(@self)) + 16);
    public property RowPtr[x: integer]: ^Vec4f read pointer(IntPtr(pointer(@self)) + x*16);
    
    public static function operator*(m: Mtr2x4f; v: Vec4f): Vec2f := new Vec2f(m.val00*v.val0+m.val01*v.val1+m.val02*v.val2+m.val03*v.val3, m.val10*v.val0+m.val11*v.val1+m.val12*v.val2+m.val13*v.val3);
    public static function operator*(v: Vec2f; m: Mtr2x4f): Vec4f := new Vec4f(m.val00*v.val0+m.val10*v.val1, m.val01*v.val0+m.val11*v.val1, m.val02*v.val0+m.val12*v.val1, m.val03*v.val0+m.val13*v.val1);
    
    public static function operator implicit(m: Mtr2x2f): Mtr2x4f := new Mtr2x4f(m.val00, m.val01, 0.0, 0.0, m.val10, m.val11, 0.0, 0.0);
    public static function operator implicit(m: Mtr2x4f): Mtr2x2f := new Mtr2x2f(m.val00, m.val01, m.val10, m.val11);
    
    public static function operator implicit(m: Mtr3x3f): Mtr2x4f := new Mtr2x4f(m.val00, m.val01, m.val02, 0.0, m.val10, m.val11, m.val12, 0.0);
    public static function operator implicit(m: Mtr2x4f): Mtr3x3f := new Mtr3x3f(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12, 0.0, 0.0, 0.0);
    
    public static function operator implicit(m: Mtr4x4f): Mtr2x4f := new Mtr2x4f(m.val00, m.val01, m.val02, m.val03, m.val10, m.val11, m.val12, m.val13);
    public static function operator implicit(m: Mtr2x4f): Mtr4x4f := new Mtr4x4f(m.val00, m.val01, m.val02, m.val03, m.val10, m.val11, m.val12, m.val13, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0);
    
    public static function operator implicit(m: Mtr2x3f): Mtr2x4f := new Mtr2x4f(m.val00, m.val01, m.val02, 0.0, m.val10, m.val11, m.val12, 0.0);
    public static function operator implicit(m: Mtr2x4f): Mtr2x3f := new Mtr2x3f(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12);
    
    public static function operator implicit(m: Mtr3x2f): Mtr2x4f := new Mtr2x4f(m.val00, m.val01, 0.0, 0.0, m.val10, m.val11, 0.0, 0.0);
    public static function operator implicit(m: Mtr2x4f): Mtr3x2f := new Mtr3x2f(m.val00, m.val01, m.val10, m.val11, 0.0, 0.0);
    
  end;
  
  Mtr4x2f = record
    public val00, val01: single;
    public val10, val11: single;
    public val20, val21: single;
    public val30, val31: single;
    
    public constructor(val00, val01, val10, val11, val20, val21, val30, val31: single);
    begin
      self.val00 := val00;
      self.val01 := val01;
      self.val10 := val10;
      self.val11 := val11;
      self.val20 := val20;
      self.val21 := val21;
      self.val30 := val30;
      self.val31 := val31;
    end;
    
    private function GetValAt(y,x: integer): single;
    begin
      case y of
        0:
        case x of
          0: Result := self.val00;
          1: Result := self.val01;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..1');
        end;
        1:
        case x of
          0: Result := self.val10;
          1: Result := self.val11;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..1');
        end;
        2:
        case x of
          0: Result := self.val20;
          1: Result := self.val21;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..1');
        end;
        3:
        case x of
          0: Result := self.val30;
          1: Result := self.val31;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..1');
        end;
        else raise new IndexOutOfRangeException('Индекс "Y" должен иметь значение 0..3');
      end;
    end;
    private procedure SetValAt(y,x: integer; val: single);
    begin
      case y of
        0:
        case x of
          0: self.val00 := val;
          1: self.val01 := val;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..1');
        end;
        1:
        case x of
          0: self.val10 := val;
          1: self.val11 := val;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..1');
        end;
        2:
        case x of
          0: self.val20 := val;
          1: self.val21 := val;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..1');
        end;
        3:
        case x of
          0: self.val30 := val;
          1: self.val31 := val;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..1');
        end;
        else raise new IndexOutOfRangeException('Индекс "Y" должен иметь значение 0..3');
      end;
    end;
    public property val[y,x: integer]: single read GetValAt write SetValAt; default;
    
    public static property Identity: Mtr4x2f read new Mtr4x2f(1.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0);
    
    public property Row0: Vec2f read new Vec2f(self.val00, self.val01) write begin self.val00 := value.val0; self.val01 := value.val1; end;
    public property Row1: Vec2f read new Vec2f(self.val10, self.val11) write begin self.val10 := value.val0; self.val11 := value.val1; end;
    public property Row2: Vec2f read new Vec2f(self.val20, self.val21) write begin self.val20 := value.val0; self.val21 := value.val1; end;
    public property Row3: Vec2f read new Vec2f(self.val30, self.val31) write begin self.val30 := value.val0; self.val31 := value.val1; end;
    public property Row[y: integer]: Vec2f read y=0?Row0:y=1?Row1:y=2?Row2:y=3?Row3:Arr&<Vec2f>[y] write
    case y of
      0: Row0 := value;
      1: Row1 := value;
      2: Row2 := value;
      3: Row3 := value;
      else raise new IndexOutOfRangeException('Номер строчки должен иметь значение 0..3');
    end;
    
    public property Col0: Vec4f read new Vec4f(self.val00, self.val10, self.val20, self.val30) write begin self.val00 := value.val0; self.val10 := value.val1; self.val20 := value.val2; self.val30 := value.val3; end;
    public property Col1: Vec4f read new Vec4f(self.val01, self.val11, self.val21, self.val31) write begin self.val01 := value.val0; self.val11 := value.val1; self.val21 := value.val2; self.val31 := value.val3; end;
    public property Col[x: integer]: Vec4f read x=0?Col0:x=1?Col1:Arr&<Vec4f>[x] write
    case x of
      0: Col0 := value;
      1: Col1 := value;
      else raise new IndexOutOfRangeException('Номер столбца должен иметь значение 0..1');
    end;
    
    public property RowPtr0: ^Vec2f read pointer(IntPtr(pointer(@self)) + 0);
    public property RowPtr1: ^Vec2f read pointer(IntPtr(pointer(@self)) + 8);
    public property RowPtr2: ^Vec2f read pointer(IntPtr(pointer(@self)) + 16);
    public property RowPtr3: ^Vec2f read pointer(IntPtr(pointer(@self)) + 24);
    public property RowPtr[x: integer]: ^Vec2f read pointer(IntPtr(pointer(@self)) + x*8);
    
    public static function operator*(m: Mtr4x2f; v: Vec2f): Vec4f := new Vec4f(m.val00*v.val0+m.val01*v.val1, m.val10*v.val0+m.val11*v.val1, m.val20*v.val0+m.val21*v.val1, m.val30*v.val0+m.val31*v.val1);
    public static function operator*(v: Vec4f; m: Mtr4x2f): Vec2f := new Vec2f(m.val00*v.val0+m.val10*v.val1+m.val20*v.val2+m.val30*v.val3, m.val01*v.val0+m.val11*v.val1+m.val21*v.val2+m.val31*v.val3);
    
    public static function operator implicit(m: Mtr2x2f): Mtr4x2f := new Mtr4x2f(m.val00, m.val01, m.val10, m.val11, 0.0, 0.0, 0.0, 0.0);
    public static function operator implicit(m: Mtr4x2f): Mtr2x2f := new Mtr2x2f(m.val00, m.val01, m.val10, m.val11);
    
    public static function operator implicit(m: Mtr3x3f): Mtr4x2f := new Mtr4x2f(m.val00, m.val01, m.val10, m.val11, m.val20, m.val21, 0.0, 0.0);
    public static function operator implicit(m: Mtr4x2f): Mtr3x3f := new Mtr3x3f(m.val00, m.val01, 0.0, m.val10, m.val11, 0.0, m.val20, m.val21, 0.0);
    
    public static function operator implicit(m: Mtr4x4f): Mtr4x2f := new Mtr4x2f(m.val00, m.val01, m.val10, m.val11, m.val20, m.val21, m.val30, m.val31);
    public static function operator implicit(m: Mtr4x2f): Mtr4x4f := new Mtr4x4f(m.val00, m.val01, 0.0, 0.0, m.val10, m.val11, 0.0, 0.0, m.val20, m.val21, 0.0, 0.0, m.val30, m.val31, 0.0, 0.0);
    
    public static function operator implicit(m: Mtr2x3f): Mtr4x2f := new Mtr4x2f(m.val00, m.val01, m.val10, m.val11, 0.0, 0.0, 0.0, 0.0);
    public static function operator implicit(m: Mtr4x2f): Mtr2x3f := new Mtr2x3f(m.val00, m.val01, 0.0, m.val10, m.val11, 0.0);
    
    public static function operator implicit(m: Mtr3x2f): Mtr4x2f := new Mtr4x2f(m.val00, m.val01, m.val10, m.val11, m.val20, m.val21, 0.0, 0.0);
    public static function operator implicit(m: Mtr4x2f): Mtr3x2f := new Mtr3x2f(m.val00, m.val01, m.val10, m.val11, m.val20, m.val21);
    
    public static function operator implicit(m: Mtr2x4f): Mtr4x2f := new Mtr4x2f(m.val00, m.val01, m.val10, m.val11, 0.0, 0.0, 0.0, 0.0);
    public static function operator implicit(m: Mtr4x2f): Mtr2x4f := new Mtr2x4f(m.val00, m.val01, 0.0, 0.0, m.val10, m.val11, 0.0, 0.0);
    
  end;
  
  Mtr3x4f = record
    public val00, val01, val02, val03: single;
    public val10, val11, val12, val13: single;
    public val20, val21, val22, val23: single;
    
    public constructor(val00, val01, val02, val03, val10, val11, val12, val13, val20, val21, val22, val23: single);
    begin
      self.val00 := val00;
      self.val01 := val01;
      self.val02 := val02;
      self.val03 := val03;
      self.val10 := val10;
      self.val11 := val11;
      self.val12 := val12;
      self.val13 := val13;
      self.val20 := val20;
      self.val21 := val21;
      self.val22 := val22;
      self.val23 := val23;
    end;
    
    private function GetValAt(y,x: integer): single;
    begin
      case y of
        0:
        case x of
          0: Result := self.val00;
          1: Result := self.val01;
          2: Result := self.val02;
          3: Result := self.val03;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..3');
        end;
        1:
        case x of
          0: Result := self.val10;
          1: Result := self.val11;
          2: Result := self.val12;
          3: Result := self.val13;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..3');
        end;
        2:
        case x of
          0: Result := self.val20;
          1: Result := self.val21;
          2: Result := self.val22;
          3: Result := self.val23;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..3');
        end;
        else raise new IndexOutOfRangeException('Индекс "Y" должен иметь значение 0..2');
      end;
    end;
    private procedure SetValAt(y,x: integer; val: single);
    begin
      case y of
        0:
        case x of
          0: self.val00 := val;
          1: self.val01 := val;
          2: self.val02 := val;
          3: self.val03 := val;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..3');
        end;
        1:
        case x of
          0: self.val10 := val;
          1: self.val11 := val;
          2: self.val12 := val;
          3: self.val13 := val;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..3');
        end;
        2:
        case x of
          0: self.val20 := val;
          1: self.val21 := val;
          2: self.val22 := val;
          3: self.val23 := val;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..3');
        end;
        else raise new IndexOutOfRangeException('Индекс "Y" должен иметь значение 0..2');
      end;
    end;
    public property val[y,x: integer]: single read GetValAt write SetValAt; default;
    
    public static property Identity: Mtr3x4f read new Mtr3x4f(1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0);
    
    public property Row0: Vec4f read new Vec4f(self.val00, self.val01, self.val02, self.val03) write begin self.val00 := value.val0; self.val01 := value.val1; self.val02 := value.val2; self.val03 := value.val3; end;
    public property Row1: Vec4f read new Vec4f(self.val10, self.val11, self.val12, self.val13) write begin self.val10 := value.val0; self.val11 := value.val1; self.val12 := value.val2; self.val13 := value.val3; end;
    public property Row2: Vec4f read new Vec4f(self.val20, self.val21, self.val22, self.val23) write begin self.val20 := value.val0; self.val21 := value.val1; self.val22 := value.val2; self.val23 := value.val3; end;
    public property Row[y: integer]: Vec4f read y=0?Row0:y=1?Row1:y=2?Row2:Arr&<Vec4f>[y] write
    case y of
      0: Row0 := value;
      1: Row1 := value;
      2: Row2 := value;
      else raise new IndexOutOfRangeException('Номер строчки должен иметь значение 0..2');
    end;
    
    public property Col0: Vec3f read new Vec3f(self.val00, self.val10, self.val20) write begin self.val00 := value.val0; self.val10 := value.val1; self.val20 := value.val2; end;
    public property Col1: Vec3f read new Vec3f(self.val01, self.val11, self.val21) write begin self.val01 := value.val0; self.val11 := value.val1; self.val21 := value.val2; end;
    public property Col2: Vec3f read new Vec3f(self.val02, self.val12, self.val22) write begin self.val02 := value.val0; self.val12 := value.val1; self.val22 := value.val2; end;
    public property Col3: Vec3f read new Vec3f(self.val03, self.val13, self.val23) write begin self.val03 := value.val0; self.val13 := value.val1; self.val23 := value.val2; end;
    public property Col[x: integer]: Vec3f read x=0?Col0:x=1?Col1:x=2?Col2:x=3?Col3:Arr&<Vec3f>[x] write
    case x of
      0: Col0 := value;
      1: Col1 := value;
      2: Col2 := value;
      3: Col3 := value;
      else raise new IndexOutOfRangeException('Номер столбца должен иметь значение 0..3');
    end;
    
    public property RowPtr0: ^Vec4f read pointer(IntPtr(pointer(@self)) + 0);
    public property RowPtr1: ^Vec4f read pointer(IntPtr(pointer(@self)) + 16);
    public property RowPtr2: ^Vec4f read pointer(IntPtr(pointer(@self)) + 32);
    public property RowPtr[x: integer]: ^Vec4f read pointer(IntPtr(pointer(@self)) + x*16);
    
    public static function operator*(m: Mtr3x4f; v: Vec4f): Vec3f := new Vec3f(m.val00*v.val0+m.val01*v.val1+m.val02*v.val2+m.val03*v.val3, m.val10*v.val0+m.val11*v.val1+m.val12*v.val2+m.val13*v.val3, m.val20*v.val0+m.val21*v.val1+m.val22*v.val2+m.val23*v.val3);
    public static function operator*(v: Vec3f; m: Mtr3x4f): Vec4f := new Vec4f(m.val00*v.val0+m.val10*v.val1+m.val20*v.val2, m.val01*v.val0+m.val11*v.val1+m.val21*v.val2, m.val02*v.val0+m.val12*v.val1+m.val22*v.val2, m.val03*v.val0+m.val13*v.val1+m.val23*v.val2);
    
    public static function operator implicit(m: Mtr2x2f): Mtr3x4f := new Mtr3x4f(m.val00, m.val01, 0.0, 0.0, m.val10, m.val11, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0);
    public static function operator implicit(m: Mtr3x4f): Mtr2x2f := new Mtr2x2f(m.val00, m.val01, m.val10, m.val11);
    
    public static function operator implicit(m: Mtr3x3f): Mtr3x4f := new Mtr3x4f(m.val00, m.val01, m.val02, 0.0, m.val10, m.val11, m.val12, 0.0, m.val20, m.val21, m.val22, 0.0);
    public static function operator implicit(m: Mtr3x4f): Mtr3x3f := new Mtr3x3f(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12, m.val20, m.val21, m.val22);
    
    public static function operator implicit(m: Mtr4x4f): Mtr3x4f := new Mtr3x4f(m.val00, m.val01, m.val02, m.val03, m.val10, m.val11, m.val12, m.val13, m.val20, m.val21, m.val22, m.val23);
    public static function operator implicit(m: Mtr3x4f): Mtr4x4f := new Mtr4x4f(m.val00, m.val01, m.val02, m.val03, m.val10, m.val11, m.val12, m.val13, m.val20, m.val21, m.val22, m.val23, 0.0, 0.0, 0.0, 0.0);
    
    public static function operator implicit(m: Mtr2x3f): Mtr3x4f := new Mtr3x4f(m.val00, m.val01, m.val02, 0.0, m.val10, m.val11, m.val12, 0.0, 0.0, 0.0, 0.0, 0.0);
    public static function operator implicit(m: Mtr3x4f): Mtr2x3f := new Mtr2x3f(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12);
    
    public static function operator implicit(m: Mtr3x2f): Mtr3x4f := new Mtr3x4f(m.val00, m.val01, 0.0, 0.0, m.val10, m.val11, 0.0, 0.0, m.val20, m.val21, 0.0, 0.0);
    public static function operator implicit(m: Mtr3x4f): Mtr3x2f := new Mtr3x2f(m.val00, m.val01, m.val10, m.val11, m.val20, m.val21);
    
    public static function operator implicit(m: Mtr2x4f): Mtr3x4f := new Mtr3x4f(m.val00, m.val01, m.val02, m.val03, m.val10, m.val11, m.val12, m.val13, 0.0, 0.0, 0.0, 0.0);
    public static function operator implicit(m: Mtr3x4f): Mtr2x4f := new Mtr2x4f(m.val00, m.val01, m.val02, m.val03, m.val10, m.val11, m.val12, m.val13);
    
    public static function operator implicit(m: Mtr4x2f): Mtr3x4f := new Mtr3x4f(m.val00, m.val01, 0.0, 0.0, m.val10, m.val11, 0.0, 0.0, m.val20, m.val21, 0.0, 0.0);
    public static function operator implicit(m: Mtr3x4f): Mtr4x2f := new Mtr4x2f(m.val00, m.val01, m.val10, m.val11, m.val20, m.val21, 0.0, 0.0);
    
  end;
  
  Mtr4x3f = record
    public val00, val01, val02: single;
    public val10, val11, val12: single;
    public val20, val21, val22: single;
    public val30, val31, val32: single;
    
    public constructor(val00, val01, val02, val10, val11, val12, val20, val21, val22, val30, val31, val32: single);
    begin
      self.val00 := val00;
      self.val01 := val01;
      self.val02 := val02;
      self.val10 := val10;
      self.val11 := val11;
      self.val12 := val12;
      self.val20 := val20;
      self.val21 := val21;
      self.val22 := val22;
      self.val30 := val30;
      self.val31 := val31;
      self.val32 := val32;
    end;
    
    private function GetValAt(y,x: integer): single;
    begin
      case y of
        0:
        case x of
          0: Result := self.val00;
          1: Result := self.val01;
          2: Result := self.val02;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..2');
        end;
        1:
        case x of
          0: Result := self.val10;
          1: Result := self.val11;
          2: Result := self.val12;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..2');
        end;
        2:
        case x of
          0: Result := self.val20;
          1: Result := self.val21;
          2: Result := self.val22;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..2');
        end;
        3:
        case x of
          0: Result := self.val30;
          1: Result := self.val31;
          2: Result := self.val32;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..2');
        end;
        else raise new IndexOutOfRangeException('Индекс "Y" должен иметь значение 0..3');
      end;
    end;
    private procedure SetValAt(y,x: integer; val: single);
    begin
      case y of
        0:
        case x of
          0: self.val00 := val;
          1: self.val01 := val;
          2: self.val02 := val;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..2');
        end;
        1:
        case x of
          0: self.val10 := val;
          1: self.val11 := val;
          2: self.val12 := val;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..2');
        end;
        2:
        case x of
          0: self.val20 := val;
          1: self.val21 := val;
          2: self.val22 := val;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..2');
        end;
        3:
        case x of
          0: self.val30 := val;
          1: self.val31 := val;
          2: self.val32 := val;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..2');
        end;
        else raise new IndexOutOfRangeException('Индекс "Y" должен иметь значение 0..3');
      end;
    end;
    public property val[y,x: integer]: single read GetValAt write SetValAt; default;
    
    public static property Identity: Mtr4x3f read new Mtr4x3f(1.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0);
    
    public property Row0: Vec3f read new Vec3f(self.val00, self.val01, self.val02) write begin self.val00 := value.val0; self.val01 := value.val1; self.val02 := value.val2; end;
    public property Row1: Vec3f read new Vec3f(self.val10, self.val11, self.val12) write begin self.val10 := value.val0; self.val11 := value.val1; self.val12 := value.val2; end;
    public property Row2: Vec3f read new Vec3f(self.val20, self.val21, self.val22) write begin self.val20 := value.val0; self.val21 := value.val1; self.val22 := value.val2; end;
    public property Row3: Vec3f read new Vec3f(self.val30, self.val31, self.val32) write begin self.val30 := value.val0; self.val31 := value.val1; self.val32 := value.val2; end;
    public property Row[y: integer]: Vec3f read y=0?Row0:y=1?Row1:y=2?Row2:y=3?Row3:Arr&<Vec3f>[y] write
    case y of
      0: Row0 := value;
      1: Row1 := value;
      2: Row2 := value;
      3: Row3 := value;
      else raise new IndexOutOfRangeException('Номер строчки должен иметь значение 0..3');
    end;
    
    public property Col0: Vec4f read new Vec4f(self.val00, self.val10, self.val20, self.val30) write begin self.val00 := value.val0; self.val10 := value.val1; self.val20 := value.val2; self.val30 := value.val3; end;
    public property Col1: Vec4f read new Vec4f(self.val01, self.val11, self.val21, self.val31) write begin self.val01 := value.val0; self.val11 := value.val1; self.val21 := value.val2; self.val31 := value.val3; end;
    public property Col2: Vec4f read new Vec4f(self.val02, self.val12, self.val22, self.val32) write begin self.val02 := value.val0; self.val12 := value.val1; self.val22 := value.val2; self.val32 := value.val3; end;
    public property Col[x: integer]: Vec4f read x=0?Col0:x=1?Col1:x=2?Col2:Arr&<Vec4f>[x] write
    case x of
      0: Col0 := value;
      1: Col1 := value;
      2: Col2 := value;
      else raise new IndexOutOfRangeException('Номер столбца должен иметь значение 0..2');
    end;
    
    public property RowPtr0: ^Vec3f read pointer(IntPtr(pointer(@self)) + 0);
    public property RowPtr1: ^Vec3f read pointer(IntPtr(pointer(@self)) + 12);
    public property RowPtr2: ^Vec3f read pointer(IntPtr(pointer(@self)) + 24);
    public property RowPtr3: ^Vec3f read pointer(IntPtr(pointer(@self)) + 36);
    public property RowPtr[x: integer]: ^Vec3f read pointer(IntPtr(pointer(@self)) + x*12);
    
    public static function operator*(m: Mtr4x3f; v: Vec3f): Vec4f := new Vec4f(m.val00*v.val0+m.val01*v.val1+m.val02*v.val2, m.val10*v.val0+m.val11*v.val1+m.val12*v.val2, m.val20*v.val0+m.val21*v.val1+m.val22*v.val2, m.val30*v.val0+m.val31*v.val1+m.val32*v.val2);
    public static function operator*(v: Vec4f; m: Mtr4x3f): Vec3f := new Vec3f(m.val00*v.val0+m.val10*v.val1+m.val20*v.val2+m.val30*v.val3, m.val01*v.val0+m.val11*v.val1+m.val21*v.val2+m.val31*v.val3, m.val02*v.val0+m.val12*v.val1+m.val22*v.val2+m.val32*v.val3);
    
    public static function operator implicit(m: Mtr2x2f): Mtr4x3f := new Mtr4x3f(m.val00, m.val01, 0.0, m.val10, m.val11, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0);
    public static function operator implicit(m: Mtr4x3f): Mtr2x2f := new Mtr2x2f(m.val00, m.val01, m.val10, m.val11);
    
    public static function operator implicit(m: Mtr3x3f): Mtr4x3f := new Mtr4x3f(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12, m.val20, m.val21, m.val22, 0.0, 0.0, 0.0);
    public static function operator implicit(m: Mtr4x3f): Mtr3x3f := new Mtr3x3f(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12, m.val20, m.val21, m.val22);
    
    public static function operator implicit(m: Mtr4x4f): Mtr4x3f := new Mtr4x3f(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12, m.val20, m.val21, m.val22, m.val30, m.val31, m.val32);
    public static function operator implicit(m: Mtr4x3f): Mtr4x4f := new Mtr4x4f(m.val00, m.val01, m.val02, 0.0, m.val10, m.val11, m.val12, 0.0, m.val20, m.val21, m.val22, 0.0, m.val30, m.val31, m.val32, 0.0);
    
    public static function operator implicit(m: Mtr2x3f): Mtr4x3f := new Mtr4x3f(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0);
    public static function operator implicit(m: Mtr4x3f): Mtr2x3f := new Mtr2x3f(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12);
    
    public static function operator implicit(m: Mtr3x2f): Mtr4x3f := new Mtr4x3f(m.val00, m.val01, 0.0, m.val10, m.val11, 0.0, m.val20, m.val21, 0.0, 0.0, 0.0, 0.0);
    public static function operator implicit(m: Mtr4x3f): Mtr3x2f := new Mtr3x2f(m.val00, m.val01, m.val10, m.val11, m.val20, m.val21);
    
    public static function operator implicit(m: Mtr2x4f): Mtr4x3f := new Mtr4x3f(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0);
    public static function operator implicit(m: Mtr4x3f): Mtr2x4f := new Mtr2x4f(m.val00, m.val01, m.val02, 0.0, m.val10, m.val11, m.val12, 0.0);
    
    public static function operator implicit(m: Mtr4x2f): Mtr4x3f := new Mtr4x3f(m.val00, m.val01, 0.0, m.val10, m.val11, 0.0, m.val20, m.val21, 0.0, m.val30, m.val31, 0.0);
    public static function operator implicit(m: Mtr4x3f): Mtr4x2f := new Mtr4x2f(m.val00, m.val01, m.val10, m.val11, m.val20, m.val21, m.val30, m.val31);
    
    public static function operator implicit(m: Mtr3x4f): Mtr4x3f := new Mtr4x3f(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12, m.val20, m.val21, m.val22, 0.0, 0.0, 0.0);
    public static function operator implicit(m: Mtr4x3f): Mtr3x4f := new Mtr3x4f(m.val00, m.val01, m.val02, 0.0, m.val10, m.val11, m.val12, 0.0, m.val20, m.val21, m.val22, 0.0);
    
  end;
  
  Mtr2x2d = record
    public val00, val01: real;
    public val10, val11: real;
    
    public constructor(val00, val01, val10, val11: real);
    begin
      self.val00 := val00;
      self.val01 := val01;
      self.val10 := val10;
      self.val11 := val11;
    end;
    
    private function GetValAt(y,x: integer): real;
    begin
      case y of
        0:
        case x of
          0: Result := self.val00;
          1: Result := self.val01;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..1');
        end;
        1:
        case x of
          0: Result := self.val10;
          1: Result := self.val11;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..1');
        end;
        else raise new IndexOutOfRangeException('Индекс "Y" должен иметь значение 0..1');
      end;
    end;
    private procedure SetValAt(y,x: integer; val: real);
    begin
      case y of
        0:
        case x of
          0: self.val00 := val;
          1: self.val01 := val;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..1');
        end;
        1:
        case x of
          0: self.val10 := val;
          1: self.val11 := val;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..1');
        end;
        else raise new IndexOutOfRangeException('Индекс "Y" должен иметь значение 0..1');
      end;
    end;
    public property val[y,x: integer]: real read GetValAt write SetValAt; default;
    
    public static property Identity: Mtr2x2d read new Mtr2x2d(1.0, 0.0, 0.0, 1.0);
    
    public property Row0: Vec2d read new Vec2d(self.val00, self.val01) write begin self.val00 := value.val0; self.val01 := value.val1; end;
    public property Row1: Vec2d read new Vec2d(self.val10, self.val11) write begin self.val10 := value.val0; self.val11 := value.val1; end;
    public property Row[y: integer]: Vec2d read y=0?Row0:y=1?Row1:Arr&<Vec2d>[y] write
    case y of
      0: Row0 := value;
      1: Row1 := value;
      else raise new IndexOutOfRangeException('Номер строчки должен иметь значение 0..1');
    end;
    
    public property Col0: Vec2d read new Vec2d(self.val00, self.val10) write begin self.val00 := value.val0; self.val10 := value.val1; end;
    public property Col1: Vec2d read new Vec2d(self.val01, self.val11) write begin self.val01 := value.val0; self.val11 := value.val1; end;
    public property Col[x: integer]: Vec2d read x=0?Col0:x=1?Col1:Arr&<Vec2d>[x] write
    case x of
      0: Col0 := value;
      1: Col1 := value;
      else raise new IndexOutOfRangeException('Номер столбца должен иметь значение 0..1');
    end;
    
    public property RowPtr0: ^Vec2d read pointer(IntPtr(pointer(@self)) + 0);
    public property RowPtr1: ^Vec2d read pointer(IntPtr(pointer(@self)) + 16);
    public property RowPtr[x: integer]: ^Vec2d read pointer(IntPtr(pointer(@self)) + x*16);
    
    public static function operator*(m: Mtr2x2d; v: Vec2d): Vec2d := new Vec2d(m.val00*v.val0+m.val01*v.val1, m.val10*v.val0+m.val11*v.val1);
    public static function operator*(v: Vec2d; m: Mtr2x2d): Vec2d := new Vec2d(m.val00*v.val0+m.val10*v.val1, m.val01*v.val0+m.val11*v.val1);
    
    public static function operator implicit(m: Mtr2x2f): Mtr2x2d := new Mtr2x2d(m.val00, m.val01, m.val10, m.val11);
    public static function operator implicit(m: Mtr2x2d): Mtr2x2f := new Mtr2x2f(m.val00, m.val01, m.val10, m.val11);
    
    public static function operator implicit(m: Mtr3x3f): Mtr2x2d := new Mtr2x2d(m.val00, m.val01, m.val10, m.val11);
    public static function operator implicit(m: Mtr2x2d): Mtr3x3f := new Mtr3x3f(m.val00, m.val01, 0.0, m.val10, m.val11, 0.0, 0.0, 0.0, 0.0);
    
    public static function operator implicit(m: Mtr4x4f): Mtr2x2d := new Mtr2x2d(m.val00, m.val01, m.val10, m.val11);
    public static function operator implicit(m: Mtr2x2d): Mtr4x4f := new Mtr4x4f(m.val00, m.val01, 0.0, 0.0, m.val10, m.val11, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0);
    
    public static function operator implicit(m: Mtr2x3f): Mtr2x2d := new Mtr2x2d(m.val00, m.val01, m.val10, m.val11);
    public static function operator implicit(m: Mtr2x2d): Mtr2x3f := new Mtr2x3f(m.val00, m.val01, 0.0, m.val10, m.val11, 0.0);
    
    public static function operator implicit(m: Mtr3x2f): Mtr2x2d := new Mtr2x2d(m.val00, m.val01, m.val10, m.val11);
    public static function operator implicit(m: Mtr2x2d): Mtr3x2f := new Mtr3x2f(m.val00, m.val01, m.val10, m.val11, 0.0, 0.0);
    
    public static function operator implicit(m: Mtr2x4f): Mtr2x2d := new Mtr2x2d(m.val00, m.val01, m.val10, m.val11);
    public static function operator implicit(m: Mtr2x2d): Mtr2x4f := new Mtr2x4f(m.val00, m.val01, 0.0, 0.0, m.val10, m.val11, 0.0, 0.0);
    
    public static function operator implicit(m: Mtr4x2f): Mtr2x2d := new Mtr2x2d(m.val00, m.val01, m.val10, m.val11);
    public static function operator implicit(m: Mtr2x2d): Mtr4x2f := new Mtr4x2f(m.val00, m.val01, m.val10, m.val11, 0.0, 0.0, 0.0, 0.0);
    
    public static function operator implicit(m: Mtr3x4f): Mtr2x2d := new Mtr2x2d(m.val00, m.val01, m.val10, m.val11);
    public static function operator implicit(m: Mtr2x2d): Mtr3x4f := new Mtr3x4f(m.val00, m.val01, 0.0, 0.0, m.val10, m.val11, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0);
    
    public static function operator implicit(m: Mtr4x3f): Mtr2x2d := new Mtr2x2d(m.val00, m.val01, m.val10, m.val11);
    public static function operator implicit(m: Mtr2x2d): Mtr4x3f := new Mtr4x3f(m.val00, m.val01, 0.0, m.val10, m.val11, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0);
    
  end;
  Mtr2d = Mtr2x2d;
  
  Mtr3x3d = record
    public val00, val01, val02: real;
    public val10, val11, val12: real;
    public val20, val21, val22: real;
    
    public constructor(val00, val01, val02, val10, val11, val12, val20, val21, val22: real);
    begin
      self.val00 := val00;
      self.val01 := val01;
      self.val02 := val02;
      self.val10 := val10;
      self.val11 := val11;
      self.val12 := val12;
      self.val20 := val20;
      self.val21 := val21;
      self.val22 := val22;
    end;
    
    private function GetValAt(y,x: integer): real;
    begin
      case y of
        0:
        case x of
          0: Result := self.val00;
          1: Result := self.val01;
          2: Result := self.val02;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..2');
        end;
        1:
        case x of
          0: Result := self.val10;
          1: Result := self.val11;
          2: Result := self.val12;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..2');
        end;
        2:
        case x of
          0: Result := self.val20;
          1: Result := self.val21;
          2: Result := self.val22;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..2');
        end;
        else raise new IndexOutOfRangeException('Индекс "Y" должен иметь значение 0..2');
      end;
    end;
    private procedure SetValAt(y,x: integer; val: real);
    begin
      case y of
        0:
        case x of
          0: self.val00 := val;
          1: self.val01 := val;
          2: self.val02 := val;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..2');
        end;
        1:
        case x of
          0: self.val10 := val;
          1: self.val11 := val;
          2: self.val12 := val;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..2');
        end;
        2:
        case x of
          0: self.val20 := val;
          1: self.val21 := val;
          2: self.val22 := val;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..2');
        end;
        else raise new IndexOutOfRangeException('Индекс "Y" должен иметь значение 0..2');
      end;
    end;
    public property val[y,x: integer]: real read GetValAt write SetValAt; default;
    
    public static property Identity: Mtr3x3d read new Mtr3x3d(1.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 1.0);
    
    public property Row0: Vec3d read new Vec3d(self.val00, self.val01, self.val02) write begin self.val00 := value.val0; self.val01 := value.val1; self.val02 := value.val2; end;
    public property Row1: Vec3d read new Vec3d(self.val10, self.val11, self.val12) write begin self.val10 := value.val0; self.val11 := value.val1; self.val12 := value.val2; end;
    public property Row2: Vec3d read new Vec3d(self.val20, self.val21, self.val22) write begin self.val20 := value.val0; self.val21 := value.val1; self.val22 := value.val2; end;
    public property Row[y: integer]: Vec3d read y=0?Row0:y=1?Row1:y=2?Row2:Arr&<Vec3d>[y] write
    case y of
      0: Row0 := value;
      1: Row1 := value;
      2: Row2 := value;
      else raise new IndexOutOfRangeException('Номер строчки должен иметь значение 0..2');
    end;
    
    public property Col0: Vec3d read new Vec3d(self.val00, self.val10, self.val20) write begin self.val00 := value.val0; self.val10 := value.val1; self.val20 := value.val2; end;
    public property Col1: Vec3d read new Vec3d(self.val01, self.val11, self.val21) write begin self.val01 := value.val0; self.val11 := value.val1; self.val21 := value.val2; end;
    public property Col2: Vec3d read new Vec3d(self.val02, self.val12, self.val22) write begin self.val02 := value.val0; self.val12 := value.val1; self.val22 := value.val2; end;
    public property Col[x: integer]: Vec3d read x=0?Col0:x=1?Col1:x=2?Col2:Arr&<Vec3d>[x] write
    case x of
      0: Col0 := value;
      1: Col1 := value;
      2: Col2 := value;
      else raise new IndexOutOfRangeException('Номер столбца должен иметь значение 0..2');
    end;
    
    public property RowPtr0: ^Vec3d read pointer(IntPtr(pointer(@self)) + 0);
    public property RowPtr1: ^Vec3d read pointer(IntPtr(pointer(@self)) + 24);
    public property RowPtr2: ^Vec3d read pointer(IntPtr(pointer(@self)) + 48);
    public property RowPtr[x: integer]: ^Vec3d read pointer(IntPtr(pointer(@self)) + x*24);
    
    public static function operator*(m: Mtr3x3d; v: Vec3d): Vec3d := new Vec3d(m.val00*v.val0+m.val01*v.val1+m.val02*v.val2, m.val10*v.val0+m.val11*v.val1+m.val12*v.val2, m.val20*v.val0+m.val21*v.val1+m.val22*v.val2);
    public static function operator*(v: Vec3d; m: Mtr3x3d): Vec3d := new Vec3d(m.val00*v.val0+m.val10*v.val1+m.val20*v.val2, m.val01*v.val0+m.val11*v.val1+m.val21*v.val2, m.val02*v.val0+m.val12*v.val1+m.val22*v.val2);
    
    public static function operator implicit(m: Mtr2x2f): Mtr3x3d := new Mtr3x3d(m.val00, m.val01, 0.0, m.val10, m.val11, 0.0, 0.0, 0.0, 0.0);
    public static function operator implicit(m: Mtr3x3d): Mtr2x2f := new Mtr2x2f(m.val00, m.val01, m.val10, m.val11);
    
    public static function operator implicit(m: Mtr3x3f): Mtr3x3d := new Mtr3x3d(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12, m.val20, m.val21, m.val22);
    public static function operator implicit(m: Mtr3x3d): Mtr3x3f := new Mtr3x3f(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12, m.val20, m.val21, m.val22);
    
    public static function operator implicit(m: Mtr4x4f): Mtr3x3d := new Mtr3x3d(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12, m.val20, m.val21, m.val22);
    public static function operator implicit(m: Mtr3x3d): Mtr4x4f := new Mtr4x4f(m.val00, m.val01, m.val02, 0.0, m.val10, m.val11, m.val12, 0.0, m.val20, m.val21, m.val22, 0.0, 0.0, 0.0, 0.0, 0.0);
    
    public static function operator implicit(m: Mtr2x3f): Mtr3x3d := new Mtr3x3d(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12, 0.0, 0.0, 0.0);
    public static function operator implicit(m: Mtr3x3d): Mtr2x3f := new Mtr2x3f(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12);
    
    public static function operator implicit(m: Mtr3x2f): Mtr3x3d := new Mtr3x3d(m.val00, m.val01, 0.0, m.val10, m.val11, 0.0, m.val20, m.val21, 0.0);
    public static function operator implicit(m: Mtr3x3d): Mtr3x2f := new Mtr3x2f(m.val00, m.val01, m.val10, m.val11, m.val20, m.val21);
    
    public static function operator implicit(m: Mtr2x4f): Mtr3x3d := new Mtr3x3d(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12, 0.0, 0.0, 0.0);
    public static function operator implicit(m: Mtr3x3d): Mtr2x4f := new Mtr2x4f(m.val00, m.val01, m.val02, 0.0, m.val10, m.val11, m.val12, 0.0);
    
    public static function operator implicit(m: Mtr4x2f): Mtr3x3d := new Mtr3x3d(m.val00, m.val01, 0.0, m.val10, m.val11, 0.0, m.val20, m.val21, 0.0);
    public static function operator implicit(m: Mtr3x3d): Mtr4x2f := new Mtr4x2f(m.val00, m.val01, m.val10, m.val11, m.val20, m.val21, 0.0, 0.0);
    
    public static function operator implicit(m: Mtr3x4f): Mtr3x3d := new Mtr3x3d(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12, m.val20, m.val21, m.val22);
    public static function operator implicit(m: Mtr3x3d): Mtr3x4f := new Mtr3x4f(m.val00, m.val01, m.val02, 0.0, m.val10, m.val11, m.val12, 0.0, m.val20, m.val21, m.val22, 0.0);
    
    public static function operator implicit(m: Mtr4x3f): Mtr3x3d := new Mtr3x3d(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12, m.val20, m.val21, m.val22);
    public static function operator implicit(m: Mtr3x3d): Mtr4x3f := new Mtr4x3f(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12, m.val20, m.val21, m.val22, 0.0, 0.0, 0.0);
    
    public static function operator implicit(m: Mtr2x2d): Mtr3x3d := new Mtr3x3d(m.val00, m.val01, 0.0, m.val10, m.val11, 0.0, 0.0, 0.0, 0.0);
    public static function operator implicit(m: Mtr3x3d): Mtr2x2d := new Mtr2x2d(m.val00, m.val01, m.val10, m.val11);
    
  end;
  Mtr3d = Mtr3x3d;
  
  Mtr4x4d = record
    public val00, val01, val02, val03: real;
    public val10, val11, val12, val13: real;
    public val20, val21, val22, val23: real;
    public val30, val31, val32, val33: real;
    
    public constructor(val00, val01, val02, val03, val10, val11, val12, val13, val20, val21, val22, val23, val30, val31, val32, val33: real);
    begin
      self.val00 := val00;
      self.val01 := val01;
      self.val02 := val02;
      self.val03 := val03;
      self.val10 := val10;
      self.val11 := val11;
      self.val12 := val12;
      self.val13 := val13;
      self.val20 := val20;
      self.val21 := val21;
      self.val22 := val22;
      self.val23 := val23;
      self.val30 := val30;
      self.val31 := val31;
      self.val32 := val32;
      self.val33 := val33;
    end;
    
    private function GetValAt(y,x: integer): real;
    begin
      case y of
        0:
        case x of
          0: Result := self.val00;
          1: Result := self.val01;
          2: Result := self.val02;
          3: Result := self.val03;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..3');
        end;
        1:
        case x of
          0: Result := self.val10;
          1: Result := self.val11;
          2: Result := self.val12;
          3: Result := self.val13;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..3');
        end;
        2:
        case x of
          0: Result := self.val20;
          1: Result := self.val21;
          2: Result := self.val22;
          3: Result := self.val23;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..3');
        end;
        3:
        case x of
          0: Result := self.val30;
          1: Result := self.val31;
          2: Result := self.val32;
          3: Result := self.val33;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..3');
        end;
        else raise new IndexOutOfRangeException('Индекс "Y" должен иметь значение 0..3');
      end;
    end;
    private procedure SetValAt(y,x: integer; val: real);
    begin
      case y of
        0:
        case x of
          0: self.val00 := val;
          1: self.val01 := val;
          2: self.val02 := val;
          3: self.val03 := val;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..3');
        end;
        1:
        case x of
          0: self.val10 := val;
          1: self.val11 := val;
          2: self.val12 := val;
          3: self.val13 := val;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..3');
        end;
        2:
        case x of
          0: self.val20 := val;
          1: self.val21 := val;
          2: self.val22 := val;
          3: self.val23 := val;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..3');
        end;
        3:
        case x of
          0: self.val30 := val;
          1: self.val31 := val;
          2: self.val32 := val;
          3: self.val33 := val;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..3');
        end;
        else raise new IndexOutOfRangeException('Индекс "Y" должен иметь значение 0..3');
      end;
    end;
    public property val[y,x: integer]: real read GetValAt write SetValAt; default;
    
    public static property Identity: Mtr4x4d read new Mtr4x4d(1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 1.0);
    
    public property Row0: Vec4d read new Vec4d(self.val00, self.val01, self.val02, self.val03) write begin self.val00 := value.val0; self.val01 := value.val1; self.val02 := value.val2; self.val03 := value.val3; end;
    public property Row1: Vec4d read new Vec4d(self.val10, self.val11, self.val12, self.val13) write begin self.val10 := value.val0; self.val11 := value.val1; self.val12 := value.val2; self.val13 := value.val3; end;
    public property Row2: Vec4d read new Vec4d(self.val20, self.val21, self.val22, self.val23) write begin self.val20 := value.val0; self.val21 := value.val1; self.val22 := value.val2; self.val23 := value.val3; end;
    public property Row3: Vec4d read new Vec4d(self.val30, self.val31, self.val32, self.val33) write begin self.val30 := value.val0; self.val31 := value.val1; self.val32 := value.val2; self.val33 := value.val3; end;
    public property Row[y: integer]: Vec4d read y=0?Row0:y=1?Row1:y=2?Row2:y=3?Row3:Arr&<Vec4d>[y] write
    case y of
      0: Row0 := value;
      1: Row1 := value;
      2: Row2 := value;
      3: Row3 := value;
      else raise new IndexOutOfRangeException('Номер строчки должен иметь значение 0..3');
    end;
    
    public property Col0: Vec4d read new Vec4d(self.val00, self.val10, self.val20, self.val30) write begin self.val00 := value.val0; self.val10 := value.val1; self.val20 := value.val2; self.val30 := value.val3; end;
    public property Col1: Vec4d read new Vec4d(self.val01, self.val11, self.val21, self.val31) write begin self.val01 := value.val0; self.val11 := value.val1; self.val21 := value.val2; self.val31 := value.val3; end;
    public property Col2: Vec4d read new Vec4d(self.val02, self.val12, self.val22, self.val32) write begin self.val02 := value.val0; self.val12 := value.val1; self.val22 := value.val2; self.val32 := value.val3; end;
    public property Col3: Vec4d read new Vec4d(self.val03, self.val13, self.val23, self.val33) write begin self.val03 := value.val0; self.val13 := value.val1; self.val23 := value.val2; self.val33 := value.val3; end;
    public property Col[x: integer]: Vec4d read x=0?Col0:x=1?Col1:x=2?Col2:x=3?Col3:Arr&<Vec4d>[x] write
    case x of
      0: Col0 := value;
      1: Col1 := value;
      2: Col2 := value;
      3: Col3 := value;
      else raise new IndexOutOfRangeException('Номер столбца должен иметь значение 0..3');
    end;
    
    public property RowPtr0: ^Vec4d read pointer(IntPtr(pointer(@self)) + 0);
    public property RowPtr1: ^Vec4d read pointer(IntPtr(pointer(@self)) + 32);
    public property RowPtr2: ^Vec4d read pointer(IntPtr(pointer(@self)) + 64);
    public property RowPtr3: ^Vec4d read pointer(IntPtr(pointer(@self)) + 96);
    public property RowPtr[x: integer]: ^Vec4d read pointer(IntPtr(pointer(@self)) + x*32);
    
    public static function operator*(m: Mtr4x4d; v: Vec4d): Vec4d := new Vec4d(m.val00*v.val0+m.val01*v.val1+m.val02*v.val2+m.val03*v.val3, m.val10*v.val0+m.val11*v.val1+m.val12*v.val2+m.val13*v.val3, m.val20*v.val0+m.val21*v.val1+m.val22*v.val2+m.val23*v.val3, m.val30*v.val0+m.val31*v.val1+m.val32*v.val2+m.val33*v.val3);
    public static function operator*(v: Vec4d; m: Mtr4x4d): Vec4d := new Vec4d(m.val00*v.val0+m.val10*v.val1+m.val20*v.val2+m.val30*v.val3, m.val01*v.val0+m.val11*v.val1+m.val21*v.val2+m.val31*v.val3, m.val02*v.val0+m.val12*v.val1+m.val22*v.val2+m.val32*v.val3, m.val03*v.val0+m.val13*v.val1+m.val23*v.val2+m.val33*v.val3);
    
    public static function operator implicit(m: Mtr2x2f): Mtr4x4d := new Mtr4x4d(m.val00, m.val01, 0.0, 0.0, m.val10, m.val11, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0);
    public static function operator implicit(m: Mtr4x4d): Mtr2x2f := new Mtr2x2f(m.val00, m.val01, m.val10, m.val11);
    
    public static function operator implicit(m: Mtr3x3f): Mtr4x4d := new Mtr4x4d(m.val00, m.val01, m.val02, 0.0, m.val10, m.val11, m.val12, 0.0, m.val20, m.val21, m.val22, 0.0, 0.0, 0.0, 0.0, 0.0);
    public static function operator implicit(m: Mtr4x4d): Mtr3x3f := new Mtr3x3f(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12, m.val20, m.val21, m.val22);
    
    public static function operator implicit(m: Mtr4x4f): Mtr4x4d := new Mtr4x4d(m.val00, m.val01, m.val02, m.val03, m.val10, m.val11, m.val12, m.val13, m.val20, m.val21, m.val22, m.val23, m.val30, m.val31, m.val32, m.val33);
    public static function operator implicit(m: Mtr4x4d): Mtr4x4f := new Mtr4x4f(m.val00, m.val01, m.val02, m.val03, m.val10, m.val11, m.val12, m.val13, m.val20, m.val21, m.val22, m.val23, m.val30, m.val31, m.val32, m.val33);
    
    public static function operator implicit(m: Mtr2x3f): Mtr4x4d := new Mtr4x4d(m.val00, m.val01, m.val02, 0.0, m.val10, m.val11, m.val12, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0);
    public static function operator implicit(m: Mtr4x4d): Mtr2x3f := new Mtr2x3f(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12);
    
    public static function operator implicit(m: Mtr3x2f): Mtr4x4d := new Mtr4x4d(m.val00, m.val01, 0.0, 0.0, m.val10, m.val11, 0.0, 0.0, m.val20, m.val21, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0);
    public static function operator implicit(m: Mtr4x4d): Mtr3x2f := new Mtr3x2f(m.val00, m.val01, m.val10, m.val11, m.val20, m.val21);
    
    public static function operator implicit(m: Mtr2x4f): Mtr4x4d := new Mtr4x4d(m.val00, m.val01, m.val02, m.val03, m.val10, m.val11, m.val12, m.val13, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0);
    public static function operator implicit(m: Mtr4x4d): Mtr2x4f := new Mtr2x4f(m.val00, m.val01, m.val02, m.val03, m.val10, m.val11, m.val12, m.val13);
    
    public static function operator implicit(m: Mtr4x2f): Mtr4x4d := new Mtr4x4d(m.val00, m.val01, 0.0, 0.0, m.val10, m.val11, 0.0, 0.0, m.val20, m.val21, 0.0, 0.0, m.val30, m.val31, 0.0, 0.0);
    public static function operator implicit(m: Mtr4x4d): Mtr4x2f := new Mtr4x2f(m.val00, m.val01, m.val10, m.val11, m.val20, m.val21, m.val30, m.val31);
    
    public static function operator implicit(m: Mtr3x4f): Mtr4x4d := new Mtr4x4d(m.val00, m.val01, m.val02, m.val03, m.val10, m.val11, m.val12, m.val13, m.val20, m.val21, m.val22, m.val23, 0.0, 0.0, 0.0, 0.0);
    public static function operator implicit(m: Mtr4x4d): Mtr3x4f := new Mtr3x4f(m.val00, m.val01, m.val02, m.val03, m.val10, m.val11, m.val12, m.val13, m.val20, m.val21, m.val22, m.val23);
    
    public static function operator implicit(m: Mtr4x3f): Mtr4x4d := new Mtr4x4d(m.val00, m.val01, m.val02, 0.0, m.val10, m.val11, m.val12, 0.0, m.val20, m.val21, m.val22, 0.0, m.val30, m.val31, m.val32, 0.0);
    public static function operator implicit(m: Mtr4x4d): Mtr4x3f := new Mtr4x3f(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12, m.val20, m.val21, m.val22, m.val30, m.val31, m.val32);
    
    public static function operator implicit(m: Mtr2x2d): Mtr4x4d := new Mtr4x4d(m.val00, m.val01, 0.0, 0.0, m.val10, m.val11, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0);
    public static function operator implicit(m: Mtr4x4d): Mtr2x2d := new Mtr2x2d(m.val00, m.val01, m.val10, m.val11);
    
    public static function operator implicit(m: Mtr3x3d): Mtr4x4d := new Mtr4x4d(m.val00, m.val01, m.val02, 0.0, m.val10, m.val11, m.val12, 0.0, m.val20, m.val21, m.val22, 0.0, 0.0, 0.0, 0.0, 0.0);
    public static function operator implicit(m: Mtr4x4d): Mtr3x3d := new Mtr3x3d(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12, m.val20, m.val21, m.val22);
    
  end;
  Mtr4d = Mtr4x4d;
  
  Mtr2x3d = record
    public val00, val01, val02: real;
    public val10, val11, val12: real;
    
    public constructor(val00, val01, val02, val10, val11, val12: real);
    begin
      self.val00 := val00;
      self.val01 := val01;
      self.val02 := val02;
      self.val10 := val10;
      self.val11 := val11;
      self.val12 := val12;
    end;
    
    private function GetValAt(y,x: integer): real;
    begin
      case y of
        0:
        case x of
          0: Result := self.val00;
          1: Result := self.val01;
          2: Result := self.val02;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..2');
        end;
        1:
        case x of
          0: Result := self.val10;
          1: Result := self.val11;
          2: Result := self.val12;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..2');
        end;
        else raise new IndexOutOfRangeException('Индекс "Y" должен иметь значение 0..1');
      end;
    end;
    private procedure SetValAt(y,x: integer; val: real);
    begin
      case y of
        0:
        case x of
          0: self.val00 := val;
          1: self.val01 := val;
          2: self.val02 := val;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..2');
        end;
        1:
        case x of
          0: self.val10 := val;
          1: self.val11 := val;
          2: self.val12 := val;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..2');
        end;
        else raise new IndexOutOfRangeException('Индекс "Y" должен иметь значение 0..1');
      end;
    end;
    public property val[y,x: integer]: real read GetValAt write SetValAt; default;
    
    public static property Identity: Mtr2x3d read new Mtr2x3d(1.0, 0.0, 0.0, 0.0, 1.0, 0.0);
    
    public property Row0: Vec3d read new Vec3d(self.val00, self.val01, self.val02) write begin self.val00 := value.val0; self.val01 := value.val1; self.val02 := value.val2; end;
    public property Row1: Vec3d read new Vec3d(self.val10, self.val11, self.val12) write begin self.val10 := value.val0; self.val11 := value.val1; self.val12 := value.val2; end;
    public property Row[y: integer]: Vec3d read y=0?Row0:y=1?Row1:Arr&<Vec3d>[y] write
    case y of
      0: Row0 := value;
      1: Row1 := value;
      else raise new IndexOutOfRangeException('Номер строчки должен иметь значение 0..1');
    end;
    
    public property Col0: Vec2d read new Vec2d(self.val00, self.val10) write begin self.val00 := value.val0; self.val10 := value.val1; end;
    public property Col1: Vec2d read new Vec2d(self.val01, self.val11) write begin self.val01 := value.val0; self.val11 := value.val1; end;
    public property Col2: Vec2d read new Vec2d(self.val02, self.val12) write begin self.val02 := value.val0; self.val12 := value.val1; end;
    public property Col[x: integer]: Vec2d read x=0?Col0:x=1?Col1:x=2?Col2:Arr&<Vec2d>[x] write
    case x of
      0: Col0 := value;
      1: Col1 := value;
      2: Col2 := value;
      else raise new IndexOutOfRangeException('Номер столбца должен иметь значение 0..2');
    end;
    
    public property RowPtr0: ^Vec3d read pointer(IntPtr(pointer(@self)) + 0);
    public property RowPtr1: ^Vec3d read pointer(IntPtr(pointer(@self)) + 24);
    public property RowPtr[x: integer]: ^Vec3d read pointer(IntPtr(pointer(@self)) + x*24);
    
    public static function operator*(m: Mtr2x3d; v: Vec3d): Vec2d := new Vec2d(m.val00*v.val0+m.val01*v.val1+m.val02*v.val2, m.val10*v.val0+m.val11*v.val1+m.val12*v.val2);
    public static function operator*(v: Vec2d; m: Mtr2x3d): Vec3d := new Vec3d(m.val00*v.val0+m.val10*v.val1, m.val01*v.val0+m.val11*v.val1, m.val02*v.val0+m.val12*v.val1);
    
    public static function operator implicit(m: Mtr2x2f): Mtr2x3d := new Mtr2x3d(m.val00, m.val01, 0.0, m.val10, m.val11, 0.0);
    public static function operator implicit(m: Mtr2x3d): Mtr2x2f := new Mtr2x2f(m.val00, m.val01, m.val10, m.val11);
    
    public static function operator implicit(m: Mtr3x3f): Mtr2x3d := new Mtr2x3d(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12);
    public static function operator implicit(m: Mtr2x3d): Mtr3x3f := new Mtr3x3f(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12, 0.0, 0.0, 0.0);
    
    public static function operator implicit(m: Mtr4x4f): Mtr2x3d := new Mtr2x3d(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12);
    public static function operator implicit(m: Mtr2x3d): Mtr4x4f := new Mtr4x4f(m.val00, m.val01, m.val02, 0.0, m.val10, m.val11, m.val12, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0);
    
    public static function operator implicit(m: Mtr2x3f): Mtr2x3d := new Mtr2x3d(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12);
    public static function operator implicit(m: Mtr2x3d): Mtr2x3f := new Mtr2x3f(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12);
    
    public static function operator implicit(m: Mtr3x2f): Mtr2x3d := new Mtr2x3d(m.val00, m.val01, 0.0, m.val10, m.val11, 0.0);
    public static function operator implicit(m: Mtr2x3d): Mtr3x2f := new Mtr3x2f(m.val00, m.val01, m.val10, m.val11, 0.0, 0.0);
    
    public static function operator implicit(m: Mtr2x4f): Mtr2x3d := new Mtr2x3d(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12);
    public static function operator implicit(m: Mtr2x3d): Mtr2x4f := new Mtr2x4f(m.val00, m.val01, m.val02, 0.0, m.val10, m.val11, m.val12, 0.0);
    
    public static function operator implicit(m: Mtr4x2f): Mtr2x3d := new Mtr2x3d(m.val00, m.val01, 0.0, m.val10, m.val11, 0.0);
    public static function operator implicit(m: Mtr2x3d): Mtr4x2f := new Mtr4x2f(m.val00, m.val01, m.val10, m.val11, 0.0, 0.0, 0.0, 0.0);
    
    public static function operator implicit(m: Mtr3x4f): Mtr2x3d := new Mtr2x3d(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12);
    public static function operator implicit(m: Mtr2x3d): Mtr3x4f := new Mtr3x4f(m.val00, m.val01, m.val02, 0.0, m.val10, m.val11, m.val12, 0.0, 0.0, 0.0, 0.0, 0.0);
    
    public static function operator implicit(m: Mtr4x3f): Mtr2x3d := new Mtr2x3d(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12);
    public static function operator implicit(m: Mtr2x3d): Mtr4x3f := new Mtr4x3f(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0);
    
    public static function operator implicit(m: Mtr2x2d): Mtr2x3d := new Mtr2x3d(m.val00, m.val01, 0.0, m.val10, m.val11, 0.0);
    public static function operator implicit(m: Mtr2x3d): Mtr2x2d := new Mtr2x2d(m.val00, m.val01, m.val10, m.val11);
    
    public static function operator implicit(m: Mtr3x3d): Mtr2x3d := new Mtr2x3d(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12);
    public static function operator implicit(m: Mtr2x3d): Mtr3x3d := new Mtr3x3d(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12, 0.0, 0.0, 0.0);
    
    public static function operator implicit(m: Mtr4x4d): Mtr2x3d := new Mtr2x3d(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12);
    public static function operator implicit(m: Mtr2x3d): Mtr4x4d := new Mtr4x4d(m.val00, m.val01, m.val02, 0.0, m.val10, m.val11, m.val12, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0);
    
  end;
  
  Mtr3x2d = record
    public val00, val01: real;
    public val10, val11: real;
    public val20, val21: real;
    
    public constructor(val00, val01, val10, val11, val20, val21: real);
    begin
      self.val00 := val00;
      self.val01 := val01;
      self.val10 := val10;
      self.val11 := val11;
      self.val20 := val20;
      self.val21 := val21;
    end;
    
    private function GetValAt(y,x: integer): real;
    begin
      case y of
        0:
        case x of
          0: Result := self.val00;
          1: Result := self.val01;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..1');
        end;
        1:
        case x of
          0: Result := self.val10;
          1: Result := self.val11;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..1');
        end;
        2:
        case x of
          0: Result := self.val20;
          1: Result := self.val21;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..1');
        end;
        else raise new IndexOutOfRangeException('Индекс "Y" должен иметь значение 0..2');
      end;
    end;
    private procedure SetValAt(y,x: integer; val: real);
    begin
      case y of
        0:
        case x of
          0: self.val00 := val;
          1: self.val01 := val;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..1');
        end;
        1:
        case x of
          0: self.val10 := val;
          1: self.val11 := val;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..1');
        end;
        2:
        case x of
          0: self.val20 := val;
          1: self.val21 := val;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..1');
        end;
        else raise new IndexOutOfRangeException('Индекс "Y" должен иметь значение 0..2');
      end;
    end;
    public property val[y,x: integer]: real read GetValAt write SetValAt; default;
    
    public static property Identity: Mtr3x2d read new Mtr3x2d(1.0, 0.0, 0.0, 1.0, 0.0, 0.0);
    
    public property Row0: Vec2d read new Vec2d(self.val00, self.val01) write begin self.val00 := value.val0; self.val01 := value.val1; end;
    public property Row1: Vec2d read new Vec2d(self.val10, self.val11) write begin self.val10 := value.val0; self.val11 := value.val1; end;
    public property Row2: Vec2d read new Vec2d(self.val20, self.val21) write begin self.val20 := value.val0; self.val21 := value.val1; end;
    public property Row[y: integer]: Vec2d read y=0?Row0:y=1?Row1:y=2?Row2:Arr&<Vec2d>[y] write
    case y of
      0: Row0 := value;
      1: Row1 := value;
      2: Row2 := value;
      else raise new IndexOutOfRangeException('Номер строчки должен иметь значение 0..2');
    end;
    
    public property Col0: Vec3d read new Vec3d(self.val00, self.val10, self.val20) write begin self.val00 := value.val0; self.val10 := value.val1; self.val20 := value.val2; end;
    public property Col1: Vec3d read new Vec3d(self.val01, self.val11, self.val21) write begin self.val01 := value.val0; self.val11 := value.val1; self.val21 := value.val2; end;
    public property Col[x: integer]: Vec3d read x=0?Col0:x=1?Col1:Arr&<Vec3d>[x] write
    case x of
      0: Col0 := value;
      1: Col1 := value;
      else raise new IndexOutOfRangeException('Номер столбца должен иметь значение 0..1');
    end;
    
    public property RowPtr0: ^Vec2d read pointer(IntPtr(pointer(@self)) + 0);
    public property RowPtr1: ^Vec2d read pointer(IntPtr(pointer(@self)) + 16);
    public property RowPtr2: ^Vec2d read pointer(IntPtr(pointer(@self)) + 32);
    public property RowPtr[x: integer]: ^Vec2d read pointer(IntPtr(pointer(@self)) + x*16);
    
    public static function operator*(m: Mtr3x2d; v: Vec2d): Vec3d := new Vec3d(m.val00*v.val0+m.val01*v.val1, m.val10*v.val0+m.val11*v.val1, m.val20*v.val0+m.val21*v.val1);
    public static function operator*(v: Vec3d; m: Mtr3x2d): Vec2d := new Vec2d(m.val00*v.val0+m.val10*v.val1+m.val20*v.val2, m.val01*v.val0+m.val11*v.val1+m.val21*v.val2);
    
    public static function operator implicit(m: Mtr2x2f): Mtr3x2d := new Mtr3x2d(m.val00, m.val01, m.val10, m.val11, 0.0, 0.0);
    public static function operator implicit(m: Mtr3x2d): Mtr2x2f := new Mtr2x2f(m.val00, m.val01, m.val10, m.val11);
    
    public static function operator implicit(m: Mtr3x3f): Mtr3x2d := new Mtr3x2d(m.val00, m.val01, m.val10, m.val11, m.val20, m.val21);
    public static function operator implicit(m: Mtr3x2d): Mtr3x3f := new Mtr3x3f(m.val00, m.val01, 0.0, m.val10, m.val11, 0.0, m.val20, m.val21, 0.0);
    
    public static function operator implicit(m: Mtr4x4f): Mtr3x2d := new Mtr3x2d(m.val00, m.val01, m.val10, m.val11, m.val20, m.val21);
    public static function operator implicit(m: Mtr3x2d): Mtr4x4f := new Mtr4x4f(m.val00, m.val01, 0.0, 0.0, m.val10, m.val11, 0.0, 0.0, m.val20, m.val21, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0);
    
    public static function operator implicit(m: Mtr2x3f): Mtr3x2d := new Mtr3x2d(m.val00, m.val01, m.val10, m.val11, 0.0, 0.0);
    public static function operator implicit(m: Mtr3x2d): Mtr2x3f := new Mtr2x3f(m.val00, m.val01, 0.0, m.val10, m.val11, 0.0);
    
    public static function operator implicit(m: Mtr3x2f): Mtr3x2d := new Mtr3x2d(m.val00, m.val01, m.val10, m.val11, m.val20, m.val21);
    public static function operator implicit(m: Mtr3x2d): Mtr3x2f := new Mtr3x2f(m.val00, m.val01, m.val10, m.val11, m.val20, m.val21);
    
    public static function operator implicit(m: Mtr2x4f): Mtr3x2d := new Mtr3x2d(m.val00, m.val01, m.val10, m.val11, 0.0, 0.0);
    public static function operator implicit(m: Mtr3x2d): Mtr2x4f := new Mtr2x4f(m.val00, m.val01, 0.0, 0.0, m.val10, m.val11, 0.0, 0.0);
    
    public static function operator implicit(m: Mtr4x2f): Mtr3x2d := new Mtr3x2d(m.val00, m.val01, m.val10, m.val11, m.val20, m.val21);
    public static function operator implicit(m: Mtr3x2d): Mtr4x2f := new Mtr4x2f(m.val00, m.val01, m.val10, m.val11, m.val20, m.val21, 0.0, 0.0);
    
    public static function operator implicit(m: Mtr3x4f): Mtr3x2d := new Mtr3x2d(m.val00, m.val01, m.val10, m.val11, m.val20, m.val21);
    public static function operator implicit(m: Mtr3x2d): Mtr3x4f := new Mtr3x4f(m.val00, m.val01, 0.0, 0.0, m.val10, m.val11, 0.0, 0.0, m.val20, m.val21, 0.0, 0.0);
    
    public static function operator implicit(m: Mtr4x3f): Mtr3x2d := new Mtr3x2d(m.val00, m.val01, m.val10, m.val11, m.val20, m.val21);
    public static function operator implicit(m: Mtr3x2d): Mtr4x3f := new Mtr4x3f(m.val00, m.val01, 0.0, m.val10, m.val11, 0.0, m.val20, m.val21, 0.0, 0.0, 0.0, 0.0);
    
    public static function operator implicit(m: Mtr2x2d): Mtr3x2d := new Mtr3x2d(m.val00, m.val01, m.val10, m.val11, 0.0, 0.0);
    public static function operator implicit(m: Mtr3x2d): Mtr2x2d := new Mtr2x2d(m.val00, m.val01, m.val10, m.val11);
    
    public static function operator implicit(m: Mtr3x3d): Mtr3x2d := new Mtr3x2d(m.val00, m.val01, m.val10, m.val11, m.val20, m.val21);
    public static function operator implicit(m: Mtr3x2d): Mtr3x3d := new Mtr3x3d(m.val00, m.val01, 0.0, m.val10, m.val11, 0.0, m.val20, m.val21, 0.0);
    
    public static function operator implicit(m: Mtr4x4d): Mtr3x2d := new Mtr3x2d(m.val00, m.val01, m.val10, m.val11, m.val20, m.val21);
    public static function operator implicit(m: Mtr3x2d): Mtr4x4d := new Mtr4x4d(m.val00, m.val01, 0.0, 0.0, m.val10, m.val11, 0.0, 0.0, m.val20, m.val21, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0);
    
    public static function operator implicit(m: Mtr2x3d): Mtr3x2d := new Mtr3x2d(m.val00, m.val01, m.val10, m.val11, 0.0, 0.0);
    public static function operator implicit(m: Mtr3x2d): Mtr2x3d := new Mtr2x3d(m.val00, m.val01, 0.0, m.val10, m.val11, 0.0);
    
  end;
  
  Mtr2x4d = record
    public val00, val01, val02, val03: real;
    public val10, val11, val12, val13: real;
    
    public constructor(val00, val01, val02, val03, val10, val11, val12, val13: real);
    begin
      self.val00 := val00;
      self.val01 := val01;
      self.val02 := val02;
      self.val03 := val03;
      self.val10 := val10;
      self.val11 := val11;
      self.val12 := val12;
      self.val13 := val13;
    end;
    
    private function GetValAt(y,x: integer): real;
    begin
      case y of
        0:
        case x of
          0: Result := self.val00;
          1: Result := self.val01;
          2: Result := self.val02;
          3: Result := self.val03;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..3');
        end;
        1:
        case x of
          0: Result := self.val10;
          1: Result := self.val11;
          2: Result := self.val12;
          3: Result := self.val13;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..3');
        end;
        else raise new IndexOutOfRangeException('Индекс "Y" должен иметь значение 0..1');
      end;
    end;
    private procedure SetValAt(y,x: integer; val: real);
    begin
      case y of
        0:
        case x of
          0: self.val00 := val;
          1: self.val01 := val;
          2: self.val02 := val;
          3: self.val03 := val;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..3');
        end;
        1:
        case x of
          0: self.val10 := val;
          1: self.val11 := val;
          2: self.val12 := val;
          3: self.val13 := val;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..3');
        end;
        else raise new IndexOutOfRangeException('Индекс "Y" должен иметь значение 0..1');
      end;
    end;
    public property val[y,x: integer]: real read GetValAt write SetValAt; default;
    
    public static property Identity: Mtr2x4d read new Mtr2x4d(1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0);
    
    public property Row0: Vec4d read new Vec4d(self.val00, self.val01, self.val02, self.val03) write begin self.val00 := value.val0; self.val01 := value.val1; self.val02 := value.val2; self.val03 := value.val3; end;
    public property Row1: Vec4d read new Vec4d(self.val10, self.val11, self.val12, self.val13) write begin self.val10 := value.val0; self.val11 := value.val1; self.val12 := value.val2; self.val13 := value.val3; end;
    public property Row[y: integer]: Vec4d read y=0?Row0:y=1?Row1:Arr&<Vec4d>[y] write
    case y of
      0: Row0 := value;
      1: Row1 := value;
      else raise new IndexOutOfRangeException('Номер строчки должен иметь значение 0..1');
    end;
    
    public property Col0: Vec2d read new Vec2d(self.val00, self.val10) write begin self.val00 := value.val0; self.val10 := value.val1; end;
    public property Col1: Vec2d read new Vec2d(self.val01, self.val11) write begin self.val01 := value.val0; self.val11 := value.val1; end;
    public property Col2: Vec2d read new Vec2d(self.val02, self.val12) write begin self.val02 := value.val0; self.val12 := value.val1; end;
    public property Col3: Vec2d read new Vec2d(self.val03, self.val13) write begin self.val03 := value.val0; self.val13 := value.val1; end;
    public property Col[x: integer]: Vec2d read x=0?Col0:x=1?Col1:x=2?Col2:x=3?Col3:Arr&<Vec2d>[x] write
    case x of
      0: Col0 := value;
      1: Col1 := value;
      2: Col2 := value;
      3: Col3 := value;
      else raise new IndexOutOfRangeException('Номер столбца должен иметь значение 0..3');
    end;
    
    public property RowPtr0: ^Vec4d read pointer(IntPtr(pointer(@self)) + 0);
    public property RowPtr1: ^Vec4d read pointer(IntPtr(pointer(@self)) + 32);
    public property RowPtr[x: integer]: ^Vec4d read pointer(IntPtr(pointer(@self)) + x*32);
    
    public static function operator*(m: Mtr2x4d; v: Vec4d): Vec2d := new Vec2d(m.val00*v.val0+m.val01*v.val1+m.val02*v.val2+m.val03*v.val3, m.val10*v.val0+m.val11*v.val1+m.val12*v.val2+m.val13*v.val3);
    public static function operator*(v: Vec2d; m: Mtr2x4d): Vec4d := new Vec4d(m.val00*v.val0+m.val10*v.val1, m.val01*v.val0+m.val11*v.val1, m.val02*v.val0+m.val12*v.val1, m.val03*v.val0+m.val13*v.val1);
    
    public static function operator implicit(m: Mtr2x2f): Mtr2x4d := new Mtr2x4d(m.val00, m.val01, 0.0, 0.0, m.val10, m.val11, 0.0, 0.0);
    public static function operator implicit(m: Mtr2x4d): Mtr2x2f := new Mtr2x2f(m.val00, m.val01, m.val10, m.val11);
    
    public static function operator implicit(m: Mtr3x3f): Mtr2x4d := new Mtr2x4d(m.val00, m.val01, m.val02, 0.0, m.val10, m.val11, m.val12, 0.0);
    public static function operator implicit(m: Mtr2x4d): Mtr3x3f := new Mtr3x3f(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12, 0.0, 0.0, 0.0);
    
    public static function operator implicit(m: Mtr4x4f): Mtr2x4d := new Mtr2x4d(m.val00, m.val01, m.val02, m.val03, m.val10, m.val11, m.val12, m.val13);
    public static function operator implicit(m: Mtr2x4d): Mtr4x4f := new Mtr4x4f(m.val00, m.val01, m.val02, m.val03, m.val10, m.val11, m.val12, m.val13, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0);
    
    public static function operator implicit(m: Mtr2x3f): Mtr2x4d := new Mtr2x4d(m.val00, m.val01, m.val02, 0.0, m.val10, m.val11, m.val12, 0.0);
    public static function operator implicit(m: Mtr2x4d): Mtr2x3f := new Mtr2x3f(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12);
    
    public static function operator implicit(m: Mtr3x2f): Mtr2x4d := new Mtr2x4d(m.val00, m.val01, 0.0, 0.0, m.val10, m.val11, 0.0, 0.0);
    public static function operator implicit(m: Mtr2x4d): Mtr3x2f := new Mtr3x2f(m.val00, m.val01, m.val10, m.val11, 0.0, 0.0);
    
    public static function operator implicit(m: Mtr2x4f): Mtr2x4d := new Mtr2x4d(m.val00, m.val01, m.val02, m.val03, m.val10, m.val11, m.val12, m.val13);
    public static function operator implicit(m: Mtr2x4d): Mtr2x4f := new Mtr2x4f(m.val00, m.val01, m.val02, m.val03, m.val10, m.val11, m.val12, m.val13);
    
    public static function operator implicit(m: Mtr4x2f): Mtr2x4d := new Mtr2x4d(m.val00, m.val01, 0.0, 0.0, m.val10, m.val11, 0.0, 0.0);
    public static function operator implicit(m: Mtr2x4d): Mtr4x2f := new Mtr4x2f(m.val00, m.val01, m.val10, m.val11, 0.0, 0.0, 0.0, 0.0);
    
    public static function operator implicit(m: Mtr3x4f): Mtr2x4d := new Mtr2x4d(m.val00, m.val01, m.val02, m.val03, m.val10, m.val11, m.val12, m.val13);
    public static function operator implicit(m: Mtr2x4d): Mtr3x4f := new Mtr3x4f(m.val00, m.val01, m.val02, m.val03, m.val10, m.val11, m.val12, m.val13, 0.0, 0.0, 0.0, 0.0);
    
    public static function operator implicit(m: Mtr4x3f): Mtr2x4d := new Mtr2x4d(m.val00, m.val01, m.val02, 0.0, m.val10, m.val11, m.val12, 0.0);
    public static function operator implicit(m: Mtr2x4d): Mtr4x3f := new Mtr4x3f(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0);
    
    public static function operator implicit(m: Mtr2x2d): Mtr2x4d := new Mtr2x4d(m.val00, m.val01, 0.0, 0.0, m.val10, m.val11, 0.0, 0.0);
    public static function operator implicit(m: Mtr2x4d): Mtr2x2d := new Mtr2x2d(m.val00, m.val01, m.val10, m.val11);
    
    public static function operator implicit(m: Mtr3x3d): Mtr2x4d := new Mtr2x4d(m.val00, m.val01, m.val02, 0.0, m.val10, m.val11, m.val12, 0.0);
    public static function operator implicit(m: Mtr2x4d): Mtr3x3d := new Mtr3x3d(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12, 0.0, 0.0, 0.0);
    
    public static function operator implicit(m: Mtr4x4d): Mtr2x4d := new Mtr2x4d(m.val00, m.val01, m.val02, m.val03, m.val10, m.val11, m.val12, m.val13);
    public static function operator implicit(m: Mtr2x4d): Mtr4x4d := new Mtr4x4d(m.val00, m.val01, m.val02, m.val03, m.val10, m.val11, m.val12, m.val13, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0);
    
    public static function operator implicit(m: Mtr2x3d): Mtr2x4d := new Mtr2x4d(m.val00, m.val01, m.val02, 0.0, m.val10, m.val11, m.val12, 0.0);
    public static function operator implicit(m: Mtr2x4d): Mtr2x3d := new Mtr2x3d(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12);
    
    public static function operator implicit(m: Mtr3x2d): Mtr2x4d := new Mtr2x4d(m.val00, m.val01, 0.0, 0.0, m.val10, m.val11, 0.0, 0.0);
    public static function operator implicit(m: Mtr2x4d): Mtr3x2d := new Mtr3x2d(m.val00, m.val01, m.val10, m.val11, 0.0, 0.0);
    
  end;
  
  Mtr4x2d = record
    public val00, val01: real;
    public val10, val11: real;
    public val20, val21: real;
    public val30, val31: real;
    
    public constructor(val00, val01, val10, val11, val20, val21, val30, val31: real);
    begin
      self.val00 := val00;
      self.val01 := val01;
      self.val10 := val10;
      self.val11 := val11;
      self.val20 := val20;
      self.val21 := val21;
      self.val30 := val30;
      self.val31 := val31;
    end;
    
    private function GetValAt(y,x: integer): real;
    begin
      case y of
        0:
        case x of
          0: Result := self.val00;
          1: Result := self.val01;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..1');
        end;
        1:
        case x of
          0: Result := self.val10;
          1: Result := self.val11;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..1');
        end;
        2:
        case x of
          0: Result := self.val20;
          1: Result := self.val21;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..1');
        end;
        3:
        case x of
          0: Result := self.val30;
          1: Result := self.val31;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..1');
        end;
        else raise new IndexOutOfRangeException('Индекс "Y" должен иметь значение 0..3');
      end;
    end;
    private procedure SetValAt(y,x: integer; val: real);
    begin
      case y of
        0:
        case x of
          0: self.val00 := val;
          1: self.val01 := val;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..1');
        end;
        1:
        case x of
          0: self.val10 := val;
          1: self.val11 := val;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..1');
        end;
        2:
        case x of
          0: self.val20 := val;
          1: self.val21 := val;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..1');
        end;
        3:
        case x of
          0: self.val30 := val;
          1: self.val31 := val;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..1');
        end;
        else raise new IndexOutOfRangeException('Индекс "Y" должен иметь значение 0..3');
      end;
    end;
    public property val[y,x: integer]: real read GetValAt write SetValAt; default;
    
    public static property Identity: Mtr4x2d read new Mtr4x2d(1.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0);
    
    public property Row0: Vec2d read new Vec2d(self.val00, self.val01) write begin self.val00 := value.val0; self.val01 := value.val1; end;
    public property Row1: Vec2d read new Vec2d(self.val10, self.val11) write begin self.val10 := value.val0; self.val11 := value.val1; end;
    public property Row2: Vec2d read new Vec2d(self.val20, self.val21) write begin self.val20 := value.val0; self.val21 := value.val1; end;
    public property Row3: Vec2d read new Vec2d(self.val30, self.val31) write begin self.val30 := value.val0; self.val31 := value.val1; end;
    public property Row[y: integer]: Vec2d read y=0?Row0:y=1?Row1:y=2?Row2:y=3?Row3:Arr&<Vec2d>[y] write
    case y of
      0: Row0 := value;
      1: Row1 := value;
      2: Row2 := value;
      3: Row3 := value;
      else raise new IndexOutOfRangeException('Номер строчки должен иметь значение 0..3');
    end;
    
    public property Col0: Vec4d read new Vec4d(self.val00, self.val10, self.val20, self.val30) write begin self.val00 := value.val0; self.val10 := value.val1; self.val20 := value.val2; self.val30 := value.val3; end;
    public property Col1: Vec4d read new Vec4d(self.val01, self.val11, self.val21, self.val31) write begin self.val01 := value.val0; self.val11 := value.val1; self.val21 := value.val2; self.val31 := value.val3; end;
    public property Col[x: integer]: Vec4d read x=0?Col0:x=1?Col1:Arr&<Vec4d>[x] write
    case x of
      0: Col0 := value;
      1: Col1 := value;
      else raise new IndexOutOfRangeException('Номер столбца должен иметь значение 0..1');
    end;
    
    public property RowPtr0: ^Vec2d read pointer(IntPtr(pointer(@self)) + 0);
    public property RowPtr1: ^Vec2d read pointer(IntPtr(pointer(@self)) + 16);
    public property RowPtr2: ^Vec2d read pointer(IntPtr(pointer(@self)) + 32);
    public property RowPtr3: ^Vec2d read pointer(IntPtr(pointer(@self)) + 48);
    public property RowPtr[x: integer]: ^Vec2d read pointer(IntPtr(pointer(@self)) + x*16);
    
    public static function operator*(m: Mtr4x2d; v: Vec2d): Vec4d := new Vec4d(m.val00*v.val0+m.val01*v.val1, m.val10*v.val0+m.val11*v.val1, m.val20*v.val0+m.val21*v.val1, m.val30*v.val0+m.val31*v.val1);
    public static function operator*(v: Vec4d; m: Mtr4x2d): Vec2d := new Vec2d(m.val00*v.val0+m.val10*v.val1+m.val20*v.val2+m.val30*v.val3, m.val01*v.val0+m.val11*v.val1+m.val21*v.val2+m.val31*v.val3);
    
    public static function operator implicit(m: Mtr2x2f): Mtr4x2d := new Mtr4x2d(m.val00, m.val01, m.val10, m.val11, 0.0, 0.0, 0.0, 0.0);
    public static function operator implicit(m: Mtr4x2d): Mtr2x2f := new Mtr2x2f(m.val00, m.val01, m.val10, m.val11);
    
    public static function operator implicit(m: Mtr3x3f): Mtr4x2d := new Mtr4x2d(m.val00, m.val01, m.val10, m.val11, m.val20, m.val21, 0.0, 0.0);
    public static function operator implicit(m: Mtr4x2d): Mtr3x3f := new Mtr3x3f(m.val00, m.val01, 0.0, m.val10, m.val11, 0.0, m.val20, m.val21, 0.0);
    
    public static function operator implicit(m: Mtr4x4f): Mtr4x2d := new Mtr4x2d(m.val00, m.val01, m.val10, m.val11, m.val20, m.val21, m.val30, m.val31);
    public static function operator implicit(m: Mtr4x2d): Mtr4x4f := new Mtr4x4f(m.val00, m.val01, 0.0, 0.0, m.val10, m.val11, 0.0, 0.0, m.val20, m.val21, 0.0, 0.0, m.val30, m.val31, 0.0, 0.0);
    
    public static function operator implicit(m: Mtr2x3f): Mtr4x2d := new Mtr4x2d(m.val00, m.val01, m.val10, m.val11, 0.0, 0.0, 0.0, 0.0);
    public static function operator implicit(m: Mtr4x2d): Mtr2x3f := new Mtr2x3f(m.val00, m.val01, 0.0, m.val10, m.val11, 0.0);
    
    public static function operator implicit(m: Mtr3x2f): Mtr4x2d := new Mtr4x2d(m.val00, m.val01, m.val10, m.val11, m.val20, m.val21, 0.0, 0.0);
    public static function operator implicit(m: Mtr4x2d): Mtr3x2f := new Mtr3x2f(m.val00, m.val01, m.val10, m.val11, m.val20, m.val21);
    
    public static function operator implicit(m: Mtr2x4f): Mtr4x2d := new Mtr4x2d(m.val00, m.val01, m.val10, m.val11, 0.0, 0.0, 0.0, 0.0);
    public static function operator implicit(m: Mtr4x2d): Mtr2x4f := new Mtr2x4f(m.val00, m.val01, 0.0, 0.0, m.val10, m.val11, 0.0, 0.0);
    
    public static function operator implicit(m: Mtr4x2f): Mtr4x2d := new Mtr4x2d(m.val00, m.val01, m.val10, m.val11, m.val20, m.val21, m.val30, m.val31);
    public static function operator implicit(m: Mtr4x2d): Mtr4x2f := new Mtr4x2f(m.val00, m.val01, m.val10, m.val11, m.val20, m.val21, m.val30, m.val31);
    
    public static function operator implicit(m: Mtr3x4f): Mtr4x2d := new Mtr4x2d(m.val00, m.val01, m.val10, m.val11, m.val20, m.val21, 0.0, 0.0);
    public static function operator implicit(m: Mtr4x2d): Mtr3x4f := new Mtr3x4f(m.val00, m.val01, 0.0, 0.0, m.val10, m.val11, 0.0, 0.0, m.val20, m.val21, 0.0, 0.0);
    
    public static function operator implicit(m: Mtr4x3f): Mtr4x2d := new Mtr4x2d(m.val00, m.val01, m.val10, m.val11, m.val20, m.val21, m.val30, m.val31);
    public static function operator implicit(m: Mtr4x2d): Mtr4x3f := new Mtr4x3f(m.val00, m.val01, 0.0, m.val10, m.val11, 0.0, m.val20, m.val21, 0.0, m.val30, m.val31, 0.0);
    
    public static function operator implicit(m: Mtr2x2d): Mtr4x2d := new Mtr4x2d(m.val00, m.val01, m.val10, m.val11, 0.0, 0.0, 0.0, 0.0);
    public static function operator implicit(m: Mtr4x2d): Mtr2x2d := new Mtr2x2d(m.val00, m.val01, m.val10, m.val11);
    
    public static function operator implicit(m: Mtr3x3d): Mtr4x2d := new Mtr4x2d(m.val00, m.val01, m.val10, m.val11, m.val20, m.val21, 0.0, 0.0);
    public static function operator implicit(m: Mtr4x2d): Mtr3x3d := new Mtr3x3d(m.val00, m.val01, 0.0, m.val10, m.val11, 0.0, m.val20, m.val21, 0.0);
    
    public static function operator implicit(m: Mtr4x4d): Mtr4x2d := new Mtr4x2d(m.val00, m.val01, m.val10, m.val11, m.val20, m.val21, m.val30, m.val31);
    public static function operator implicit(m: Mtr4x2d): Mtr4x4d := new Mtr4x4d(m.val00, m.val01, 0.0, 0.0, m.val10, m.val11, 0.0, 0.0, m.val20, m.val21, 0.0, 0.0, m.val30, m.val31, 0.0, 0.0);
    
    public static function operator implicit(m: Mtr2x3d): Mtr4x2d := new Mtr4x2d(m.val00, m.val01, m.val10, m.val11, 0.0, 0.0, 0.0, 0.0);
    public static function operator implicit(m: Mtr4x2d): Mtr2x3d := new Mtr2x3d(m.val00, m.val01, 0.0, m.val10, m.val11, 0.0);
    
    public static function operator implicit(m: Mtr3x2d): Mtr4x2d := new Mtr4x2d(m.val00, m.val01, m.val10, m.val11, m.val20, m.val21, 0.0, 0.0);
    public static function operator implicit(m: Mtr4x2d): Mtr3x2d := new Mtr3x2d(m.val00, m.val01, m.val10, m.val11, m.val20, m.val21);
    
    public static function operator implicit(m: Mtr2x4d): Mtr4x2d := new Mtr4x2d(m.val00, m.val01, m.val10, m.val11, 0.0, 0.0, 0.0, 0.0);
    public static function operator implicit(m: Mtr4x2d): Mtr2x4d := new Mtr2x4d(m.val00, m.val01, 0.0, 0.0, m.val10, m.val11, 0.0, 0.0);
    
  end;
  
  Mtr3x4d = record
    public val00, val01, val02, val03: real;
    public val10, val11, val12, val13: real;
    public val20, val21, val22, val23: real;
    
    public constructor(val00, val01, val02, val03, val10, val11, val12, val13, val20, val21, val22, val23: real);
    begin
      self.val00 := val00;
      self.val01 := val01;
      self.val02 := val02;
      self.val03 := val03;
      self.val10 := val10;
      self.val11 := val11;
      self.val12 := val12;
      self.val13 := val13;
      self.val20 := val20;
      self.val21 := val21;
      self.val22 := val22;
      self.val23 := val23;
    end;
    
    private function GetValAt(y,x: integer): real;
    begin
      case y of
        0:
        case x of
          0: Result := self.val00;
          1: Result := self.val01;
          2: Result := self.val02;
          3: Result := self.val03;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..3');
        end;
        1:
        case x of
          0: Result := self.val10;
          1: Result := self.val11;
          2: Result := self.val12;
          3: Result := self.val13;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..3');
        end;
        2:
        case x of
          0: Result := self.val20;
          1: Result := self.val21;
          2: Result := self.val22;
          3: Result := self.val23;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..3');
        end;
        else raise new IndexOutOfRangeException('Индекс "Y" должен иметь значение 0..2');
      end;
    end;
    private procedure SetValAt(y,x: integer; val: real);
    begin
      case y of
        0:
        case x of
          0: self.val00 := val;
          1: self.val01 := val;
          2: self.val02 := val;
          3: self.val03 := val;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..3');
        end;
        1:
        case x of
          0: self.val10 := val;
          1: self.val11 := val;
          2: self.val12 := val;
          3: self.val13 := val;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..3');
        end;
        2:
        case x of
          0: self.val20 := val;
          1: self.val21 := val;
          2: self.val22 := val;
          3: self.val23 := val;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..3');
        end;
        else raise new IndexOutOfRangeException('Индекс "Y" должен иметь значение 0..2');
      end;
    end;
    public property val[y,x: integer]: real read GetValAt write SetValAt; default;
    
    public static property Identity: Mtr3x4d read new Mtr3x4d(1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0);
    
    public property Row0: Vec4d read new Vec4d(self.val00, self.val01, self.val02, self.val03) write begin self.val00 := value.val0; self.val01 := value.val1; self.val02 := value.val2; self.val03 := value.val3; end;
    public property Row1: Vec4d read new Vec4d(self.val10, self.val11, self.val12, self.val13) write begin self.val10 := value.val0; self.val11 := value.val1; self.val12 := value.val2; self.val13 := value.val3; end;
    public property Row2: Vec4d read new Vec4d(self.val20, self.val21, self.val22, self.val23) write begin self.val20 := value.val0; self.val21 := value.val1; self.val22 := value.val2; self.val23 := value.val3; end;
    public property Row[y: integer]: Vec4d read y=0?Row0:y=1?Row1:y=2?Row2:Arr&<Vec4d>[y] write
    case y of
      0: Row0 := value;
      1: Row1 := value;
      2: Row2 := value;
      else raise new IndexOutOfRangeException('Номер строчки должен иметь значение 0..2');
    end;
    
    public property Col0: Vec3d read new Vec3d(self.val00, self.val10, self.val20) write begin self.val00 := value.val0; self.val10 := value.val1; self.val20 := value.val2; end;
    public property Col1: Vec3d read new Vec3d(self.val01, self.val11, self.val21) write begin self.val01 := value.val0; self.val11 := value.val1; self.val21 := value.val2; end;
    public property Col2: Vec3d read new Vec3d(self.val02, self.val12, self.val22) write begin self.val02 := value.val0; self.val12 := value.val1; self.val22 := value.val2; end;
    public property Col3: Vec3d read new Vec3d(self.val03, self.val13, self.val23) write begin self.val03 := value.val0; self.val13 := value.val1; self.val23 := value.val2; end;
    public property Col[x: integer]: Vec3d read x=0?Col0:x=1?Col1:x=2?Col2:x=3?Col3:Arr&<Vec3d>[x] write
    case x of
      0: Col0 := value;
      1: Col1 := value;
      2: Col2 := value;
      3: Col3 := value;
      else raise new IndexOutOfRangeException('Номер столбца должен иметь значение 0..3');
    end;
    
    public property RowPtr0: ^Vec4d read pointer(IntPtr(pointer(@self)) + 0);
    public property RowPtr1: ^Vec4d read pointer(IntPtr(pointer(@self)) + 32);
    public property RowPtr2: ^Vec4d read pointer(IntPtr(pointer(@self)) + 64);
    public property RowPtr[x: integer]: ^Vec4d read pointer(IntPtr(pointer(@self)) + x*32);
    
    public static function operator*(m: Mtr3x4d; v: Vec4d): Vec3d := new Vec3d(m.val00*v.val0+m.val01*v.val1+m.val02*v.val2+m.val03*v.val3, m.val10*v.val0+m.val11*v.val1+m.val12*v.val2+m.val13*v.val3, m.val20*v.val0+m.val21*v.val1+m.val22*v.val2+m.val23*v.val3);
    public static function operator*(v: Vec3d; m: Mtr3x4d): Vec4d := new Vec4d(m.val00*v.val0+m.val10*v.val1+m.val20*v.val2, m.val01*v.val0+m.val11*v.val1+m.val21*v.val2, m.val02*v.val0+m.val12*v.val1+m.val22*v.val2, m.val03*v.val0+m.val13*v.val1+m.val23*v.val2);
    
    public static function operator implicit(m: Mtr2x2f): Mtr3x4d := new Mtr3x4d(m.val00, m.val01, 0.0, 0.0, m.val10, m.val11, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0);
    public static function operator implicit(m: Mtr3x4d): Mtr2x2f := new Mtr2x2f(m.val00, m.val01, m.val10, m.val11);
    
    public static function operator implicit(m: Mtr3x3f): Mtr3x4d := new Mtr3x4d(m.val00, m.val01, m.val02, 0.0, m.val10, m.val11, m.val12, 0.0, m.val20, m.val21, m.val22, 0.0);
    public static function operator implicit(m: Mtr3x4d): Mtr3x3f := new Mtr3x3f(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12, m.val20, m.val21, m.val22);
    
    public static function operator implicit(m: Mtr4x4f): Mtr3x4d := new Mtr3x4d(m.val00, m.val01, m.val02, m.val03, m.val10, m.val11, m.val12, m.val13, m.val20, m.val21, m.val22, m.val23);
    public static function operator implicit(m: Mtr3x4d): Mtr4x4f := new Mtr4x4f(m.val00, m.val01, m.val02, m.val03, m.val10, m.val11, m.val12, m.val13, m.val20, m.val21, m.val22, m.val23, 0.0, 0.0, 0.0, 0.0);
    
    public static function operator implicit(m: Mtr2x3f): Mtr3x4d := new Mtr3x4d(m.val00, m.val01, m.val02, 0.0, m.val10, m.val11, m.val12, 0.0, 0.0, 0.0, 0.0, 0.0);
    public static function operator implicit(m: Mtr3x4d): Mtr2x3f := new Mtr2x3f(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12);
    
    public static function operator implicit(m: Mtr3x2f): Mtr3x4d := new Mtr3x4d(m.val00, m.val01, 0.0, 0.0, m.val10, m.val11, 0.0, 0.0, m.val20, m.val21, 0.0, 0.0);
    public static function operator implicit(m: Mtr3x4d): Mtr3x2f := new Mtr3x2f(m.val00, m.val01, m.val10, m.val11, m.val20, m.val21);
    
    public static function operator implicit(m: Mtr2x4f): Mtr3x4d := new Mtr3x4d(m.val00, m.val01, m.val02, m.val03, m.val10, m.val11, m.val12, m.val13, 0.0, 0.0, 0.0, 0.0);
    public static function operator implicit(m: Mtr3x4d): Mtr2x4f := new Mtr2x4f(m.val00, m.val01, m.val02, m.val03, m.val10, m.val11, m.val12, m.val13);
    
    public static function operator implicit(m: Mtr4x2f): Mtr3x4d := new Mtr3x4d(m.val00, m.val01, 0.0, 0.0, m.val10, m.val11, 0.0, 0.0, m.val20, m.val21, 0.0, 0.0);
    public static function operator implicit(m: Mtr3x4d): Mtr4x2f := new Mtr4x2f(m.val00, m.val01, m.val10, m.val11, m.val20, m.val21, 0.0, 0.0);
    
    public static function operator implicit(m: Mtr3x4f): Mtr3x4d := new Mtr3x4d(m.val00, m.val01, m.val02, m.val03, m.val10, m.val11, m.val12, m.val13, m.val20, m.val21, m.val22, m.val23);
    public static function operator implicit(m: Mtr3x4d): Mtr3x4f := new Mtr3x4f(m.val00, m.val01, m.val02, m.val03, m.val10, m.val11, m.val12, m.val13, m.val20, m.val21, m.val22, m.val23);
    
    public static function operator implicit(m: Mtr4x3f): Mtr3x4d := new Mtr3x4d(m.val00, m.val01, m.val02, 0.0, m.val10, m.val11, m.val12, 0.0, m.val20, m.val21, m.val22, 0.0);
    public static function operator implicit(m: Mtr3x4d): Mtr4x3f := new Mtr4x3f(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12, m.val20, m.val21, m.val22, 0.0, 0.0, 0.0);
    
    public static function operator implicit(m: Mtr2x2d): Mtr3x4d := new Mtr3x4d(m.val00, m.val01, 0.0, 0.0, m.val10, m.val11, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0);
    public static function operator implicit(m: Mtr3x4d): Mtr2x2d := new Mtr2x2d(m.val00, m.val01, m.val10, m.val11);
    
    public static function operator implicit(m: Mtr3x3d): Mtr3x4d := new Mtr3x4d(m.val00, m.val01, m.val02, 0.0, m.val10, m.val11, m.val12, 0.0, m.val20, m.val21, m.val22, 0.0);
    public static function operator implicit(m: Mtr3x4d): Mtr3x3d := new Mtr3x3d(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12, m.val20, m.val21, m.val22);
    
    public static function operator implicit(m: Mtr4x4d): Mtr3x4d := new Mtr3x4d(m.val00, m.val01, m.val02, m.val03, m.val10, m.val11, m.val12, m.val13, m.val20, m.val21, m.val22, m.val23);
    public static function operator implicit(m: Mtr3x4d): Mtr4x4d := new Mtr4x4d(m.val00, m.val01, m.val02, m.val03, m.val10, m.val11, m.val12, m.val13, m.val20, m.val21, m.val22, m.val23, 0.0, 0.0, 0.0, 0.0);
    
    public static function operator implicit(m: Mtr2x3d): Mtr3x4d := new Mtr3x4d(m.val00, m.val01, m.val02, 0.0, m.val10, m.val11, m.val12, 0.0, 0.0, 0.0, 0.0, 0.0);
    public static function operator implicit(m: Mtr3x4d): Mtr2x3d := new Mtr2x3d(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12);
    
    public static function operator implicit(m: Mtr3x2d): Mtr3x4d := new Mtr3x4d(m.val00, m.val01, 0.0, 0.0, m.val10, m.val11, 0.0, 0.0, m.val20, m.val21, 0.0, 0.0);
    public static function operator implicit(m: Mtr3x4d): Mtr3x2d := new Mtr3x2d(m.val00, m.val01, m.val10, m.val11, m.val20, m.val21);
    
    public static function operator implicit(m: Mtr2x4d): Mtr3x4d := new Mtr3x4d(m.val00, m.val01, m.val02, m.val03, m.val10, m.val11, m.val12, m.val13, 0.0, 0.0, 0.0, 0.0);
    public static function operator implicit(m: Mtr3x4d): Mtr2x4d := new Mtr2x4d(m.val00, m.val01, m.val02, m.val03, m.val10, m.val11, m.val12, m.val13);
    
    public static function operator implicit(m: Mtr4x2d): Mtr3x4d := new Mtr3x4d(m.val00, m.val01, 0.0, 0.0, m.val10, m.val11, 0.0, 0.0, m.val20, m.val21, 0.0, 0.0);
    public static function operator implicit(m: Mtr3x4d): Mtr4x2d := new Mtr4x2d(m.val00, m.val01, m.val10, m.val11, m.val20, m.val21, 0.0, 0.0);
    
  end;
  
  Mtr4x3d = record
    public val00, val01, val02: real;
    public val10, val11, val12: real;
    public val20, val21, val22: real;
    public val30, val31, val32: real;
    
    public constructor(val00, val01, val02, val10, val11, val12, val20, val21, val22, val30, val31, val32: real);
    begin
      self.val00 := val00;
      self.val01 := val01;
      self.val02 := val02;
      self.val10 := val10;
      self.val11 := val11;
      self.val12 := val12;
      self.val20 := val20;
      self.val21 := val21;
      self.val22 := val22;
      self.val30 := val30;
      self.val31 := val31;
      self.val32 := val32;
    end;
    
    private function GetValAt(y,x: integer): real;
    begin
      case y of
        0:
        case x of
          0: Result := self.val00;
          1: Result := self.val01;
          2: Result := self.val02;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..2');
        end;
        1:
        case x of
          0: Result := self.val10;
          1: Result := self.val11;
          2: Result := self.val12;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..2');
        end;
        2:
        case x of
          0: Result := self.val20;
          1: Result := self.val21;
          2: Result := self.val22;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..2');
        end;
        3:
        case x of
          0: Result := self.val30;
          1: Result := self.val31;
          2: Result := self.val32;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..2');
        end;
        else raise new IndexOutOfRangeException('Индекс "Y" должен иметь значение 0..3');
      end;
    end;
    private procedure SetValAt(y,x: integer; val: real);
    begin
      case y of
        0:
        case x of
          0: self.val00 := val;
          1: self.val01 := val;
          2: self.val02 := val;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..2');
        end;
        1:
        case x of
          0: self.val10 := val;
          1: self.val11 := val;
          2: self.val12 := val;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..2');
        end;
        2:
        case x of
          0: self.val20 := val;
          1: self.val21 := val;
          2: self.val22 := val;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..2');
        end;
        3:
        case x of
          0: self.val30 := val;
          1: self.val31 := val;
          2: self.val32 := val;
          else raise new IndexOutOfRangeException('Индекс "X" должен иметь значение 0..2');
        end;
        else raise new IndexOutOfRangeException('Индекс "Y" должен иметь значение 0..3');
      end;
    end;
    public property val[y,x: integer]: real read GetValAt write SetValAt; default;
    
    public static property Identity: Mtr4x3d read new Mtr4x3d(1.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0);
    
    public property Row0: Vec3d read new Vec3d(self.val00, self.val01, self.val02) write begin self.val00 := value.val0; self.val01 := value.val1; self.val02 := value.val2; end;
    public property Row1: Vec3d read new Vec3d(self.val10, self.val11, self.val12) write begin self.val10 := value.val0; self.val11 := value.val1; self.val12 := value.val2; end;
    public property Row2: Vec3d read new Vec3d(self.val20, self.val21, self.val22) write begin self.val20 := value.val0; self.val21 := value.val1; self.val22 := value.val2; end;
    public property Row3: Vec3d read new Vec3d(self.val30, self.val31, self.val32) write begin self.val30 := value.val0; self.val31 := value.val1; self.val32 := value.val2; end;
    public property Row[y: integer]: Vec3d read y=0?Row0:y=1?Row1:y=2?Row2:y=3?Row3:Arr&<Vec3d>[y] write
    case y of
      0: Row0 := value;
      1: Row1 := value;
      2: Row2 := value;
      3: Row3 := value;
      else raise new IndexOutOfRangeException('Номер строчки должен иметь значение 0..3');
    end;
    
    public property Col0: Vec4d read new Vec4d(self.val00, self.val10, self.val20, self.val30) write begin self.val00 := value.val0; self.val10 := value.val1; self.val20 := value.val2; self.val30 := value.val3; end;
    public property Col1: Vec4d read new Vec4d(self.val01, self.val11, self.val21, self.val31) write begin self.val01 := value.val0; self.val11 := value.val1; self.val21 := value.val2; self.val31 := value.val3; end;
    public property Col2: Vec4d read new Vec4d(self.val02, self.val12, self.val22, self.val32) write begin self.val02 := value.val0; self.val12 := value.val1; self.val22 := value.val2; self.val32 := value.val3; end;
    public property Col[x: integer]: Vec4d read x=0?Col0:x=1?Col1:x=2?Col2:Arr&<Vec4d>[x] write
    case x of
      0: Col0 := value;
      1: Col1 := value;
      2: Col2 := value;
      else raise new IndexOutOfRangeException('Номер столбца должен иметь значение 0..2');
    end;
    
    public property RowPtr0: ^Vec3d read pointer(IntPtr(pointer(@self)) + 0);
    public property RowPtr1: ^Vec3d read pointer(IntPtr(pointer(@self)) + 24);
    public property RowPtr2: ^Vec3d read pointer(IntPtr(pointer(@self)) + 48);
    public property RowPtr3: ^Vec3d read pointer(IntPtr(pointer(@self)) + 72);
    public property RowPtr[x: integer]: ^Vec3d read pointer(IntPtr(pointer(@self)) + x*24);
    
    public static function operator*(m: Mtr4x3d; v: Vec3d): Vec4d := new Vec4d(m.val00*v.val0+m.val01*v.val1+m.val02*v.val2, m.val10*v.val0+m.val11*v.val1+m.val12*v.val2, m.val20*v.val0+m.val21*v.val1+m.val22*v.val2, m.val30*v.val0+m.val31*v.val1+m.val32*v.val2);
    public static function operator*(v: Vec4d; m: Mtr4x3d): Vec3d := new Vec3d(m.val00*v.val0+m.val10*v.val1+m.val20*v.val2+m.val30*v.val3, m.val01*v.val0+m.val11*v.val1+m.val21*v.val2+m.val31*v.val3, m.val02*v.val0+m.val12*v.val1+m.val22*v.val2+m.val32*v.val3);
    
    public static function operator implicit(m: Mtr2x2f): Mtr4x3d := new Mtr4x3d(m.val00, m.val01, 0.0, m.val10, m.val11, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0);
    public static function operator implicit(m: Mtr4x3d): Mtr2x2f := new Mtr2x2f(m.val00, m.val01, m.val10, m.val11);
    
    public static function operator implicit(m: Mtr3x3f): Mtr4x3d := new Mtr4x3d(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12, m.val20, m.val21, m.val22, 0.0, 0.0, 0.0);
    public static function operator implicit(m: Mtr4x3d): Mtr3x3f := new Mtr3x3f(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12, m.val20, m.val21, m.val22);
    
    public static function operator implicit(m: Mtr4x4f): Mtr4x3d := new Mtr4x3d(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12, m.val20, m.val21, m.val22, m.val30, m.val31, m.val32);
    public static function operator implicit(m: Mtr4x3d): Mtr4x4f := new Mtr4x4f(m.val00, m.val01, m.val02, 0.0, m.val10, m.val11, m.val12, 0.0, m.val20, m.val21, m.val22, 0.0, m.val30, m.val31, m.val32, 0.0);
    
    public static function operator implicit(m: Mtr2x3f): Mtr4x3d := new Mtr4x3d(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0);
    public static function operator implicit(m: Mtr4x3d): Mtr2x3f := new Mtr2x3f(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12);
    
    public static function operator implicit(m: Mtr3x2f): Mtr4x3d := new Mtr4x3d(m.val00, m.val01, 0.0, m.val10, m.val11, 0.0, m.val20, m.val21, 0.0, 0.0, 0.0, 0.0);
    public static function operator implicit(m: Mtr4x3d): Mtr3x2f := new Mtr3x2f(m.val00, m.val01, m.val10, m.val11, m.val20, m.val21);
    
    public static function operator implicit(m: Mtr2x4f): Mtr4x3d := new Mtr4x3d(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0);
    public static function operator implicit(m: Mtr4x3d): Mtr2x4f := new Mtr2x4f(m.val00, m.val01, m.val02, 0.0, m.val10, m.val11, m.val12, 0.0);
    
    public static function operator implicit(m: Mtr4x2f): Mtr4x3d := new Mtr4x3d(m.val00, m.val01, 0.0, m.val10, m.val11, 0.0, m.val20, m.val21, 0.0, m.val30, m.val31, 0.0);
    public static function operator implicit(m: Mtr4x3d): Mtr4x2f := new Mtr4x2f(m.val00, m.val01, m.val10, m.val11, m.val20, m.val21, m.val30, m.val31);
    
    public static function operator implicit(m: Mtr3x4f): Mtr4x3d := new Mtr4x3d(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12, m.val20, m.val21, m.val22, 0.0, 0.0, 0.0);
    public static function operator implicit(m: Mtr4x3d): Mtr3x4f := new Mtr3x4f(m.val00, m.val01, m.val02, 0.0, m.val10, m.val11, m.val12, 0.0, m.val20, m.val21, m.val22, 0.0);
    
    public static function operator implicit(m: Mtr4x3f): Mtr4x3d := new Mtr4x3d(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12, m.val20, m.val21, m.val22, m.val30, m.val31, m.val32);
    public static function operator implicit(m: Mtr4x3d): Mtr4x3f := new Mtr4x3f(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12, m.val20, m.val21, m.val22, m.val30, m.val31, m.val32);
    
    public static function operator implicit(m: Mtr2x2d): Mtr4x3d := new Mtr4x3d(m.val00, m.val01, 0.0, m.val10, m.val11, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0);
    public static function operator implicit(m: Mtr4x3d): Mtr2x2d := new Mtr2x2d(m.val00, m.val01, m.val10, m.val11);
    
    public static function operator implicit(m: Mtr3x3d): Mtr4x3d := new Mtr4x3d(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12, m.val20, m.val21, m.val22, 0.0, 0.0, 0.0);
    public static function operator implicit(m: Mtr4x3d): Mtr3x3d := new Mtr3x3d(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12, m.val20, m.val21, m.val22);
    
    public static function operator implicit(m: Mtr4x4d): Mtr4x3d := new Mtr4x3d(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12, m.val20, m.val21, m.val22, m.val30, m.val31, m.val32);
    public static function operator implicit(m: Mtr4x3d): Mtr4x4d := new Mtr4x4d(m.val00, m.val01, m.val02, 0.0, m.val10, m.val11, m.val12, 0.0, m.val20, m.val21, m.val22, 0.0, m.val30, m.val31, m.val32, 0.0);
    
    public static function operator implicit(m: Mtr2x3d): Mtr4x3d := new Mtr4x3d(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0);
    public static function operator implicit(m: Mtr4x3d): Mtr2x3d := new Mtr2x3d(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12);
    
    public static function operator implicit(m: Mtr3x2d): Mtr4x3d := new Mtr4x3d(m.val00, m.val01, 0.0, m.val10, m.val11, 0.0, m.val20, m.val21, 0.0, 0.0, 0.0, 0.0);
    public static function operator implicit(m: Mtr4x3d): Mtr3x2d := new Mtr3x2d(m.val00, m.val01, m.val10, m.val11, m.val20, m.val21);
    
    public static function operator implicit(m: Mtr2x4d): Mtr4x3d := new Mtr4x3d(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0);
    public static function operator implicit(m: Mtr4x3d): Mtr2x4d := new Mtr2x4d(m.val00, m.val01, m.val02, 0.0, m.val10, m.val11, m.val12, 0.0);
    
    public static function operator implicit(m: Mtr4x2d): Mtr4x3d := new Mtr4x3d(m.val00, m.val01, 0.0, m.val10, m.val11, 0.0, m.val20, m.val21, 0.0, m.val30, m.val31, 0.0);
    public static function operator implicit(m: Mtr4x3d): Mtr4x2d := new Mtr4x2d(m.val00, m.val01, m.val10, m.val11, m.val20, m.val21, m.val30, m.val31);
    
    public static function operator implicit(m: Mtr3x4d): Mtr4x3d := new Mtr4x3d(m.val00, m.val01, m.val02, m.val10, m.val11, m.val12, m.val20, m.val21, m.val22, 0.0, 0.0, 0.0);
    public static function operator implicit(m: Mtr4x3d): Mtr3x4d := new Mtr3x4d(m.val00, m.val01, m.val02, 0.0, m.val10, m.val11, m.val12, 0.0, m.val20, m.val21, m.val22, 0.0);
    
  end;
  
  {$endregion Mtr}
  
  {$region MtrMlt}
    
    function operator*(m1: Mtr2x2f; m2: Mtr2x2f): Mtr2x2f; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11;
    end;
    
    function operator*(m1: Mtr2x2f; m2: Mtr2x3f): Mtr2x3f; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12;
    end;
    
    function operator*(m1: Mtr2x2f; m2: Mtr2x4f): Mtr2x4f; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12;
      Result.val03 := m1.val00*m2.val03 + m1.val01*m2.val13;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12;
      Result.val13 := m1.val10*m2.val03 + m1.val11*m2.val13;
    end;
    
    function operator*(m1: Mtr2x2f; m2: Mtr2x2d): Mtr2x2f; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11;
    end;
    
    function operator*(m1: Mtr2x2f; m2: Mtr2x3d): Mtr2x3f; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12;
    end;
    
    function operator*(m1: Mtr2x2f; m2: Mtr2x4d): Mtr2x4f; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12;
      Result.val03 := m1.val00*m2.val03 + m1.val01*m2.val13;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12;
      Result.val13 := m1.val10*m2.val03 + m1.val11*m2.val13;
    end;
    
    function operator*(m1: Mtr3x3f; m2: Mtr3x3f): Mtr3x3f; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12 + m1.val02*m2.val22;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12 + m1.val12*m2.val22;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10 + m1.val22*m2.val20;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11 + m1.val22*m2.val21;
      Result.val22 := m1.val20*m2.val02 + m1.val21*m2.val12 + m1.val22*m2.val22;
    end;
    
    function operator*(m1: Mtr3x3f; m2: Mtr3x2f): Mtr3x2f; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10 + m1.val22*m2.val20;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11 + m1.val22*m2.val21;
    end;
    
    function operator*(m1: Mtr3x3f; m2: Mtr3x4f): Mtr3x4f; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12 + m1.val02*m2.val22;
      Result.val03 := m1.val00*m2.val03 + m1.val01*m2.val13 + m1.val02*m2.val23;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12 + m1.val12*m2.val22;
      Result.val13 := m1.val10*m2.val03 + m1.val11*m2.val13 + m1.val12*m2.val23;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10 + m1.val22*m2.val20;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11 + m1.val22*m2.val21;
      Result.val22 := m1.val20*m2.val02 + m1.val21*m2.val12 + m1.val22*m2.val22;
      Result.val23 := m1.val20*m2.val03 + m1.val21*m2.val13 + m1.val22*m2.val23;
    end;
    
    function operator*(m1: Mtr3x3f; m2: Mtr3x3d): Mtr3x3f; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12 + m1.val02*m2.val22;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12 + m1.val12*m2.val22;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10 + m1.val22*m2.val20;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11 + m1.val22*m2.val21;
      Result.val22 := m1.val20*m2.val02 + m1.val21*m2.val12 + m1.val22*m2.val22;
    end;
    
    function operator*(m1: Mtr3x3f; m2: Mtr3x2d): Mtr3x2f; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10 + m1.val22*m2.val20;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11 + m1.val22*m2.val21;
    end;
    
    function operator*(m1: Mtr3x3f; m2: Mtr3x4d): Mtr3x4f; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12 + m1.val02*m2.val22;
      Result.val03 := m1.val00*m2.val03 + m1.val01*m2.val13 + m1.val02*m2.val23;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12 + m1.val12*m2.val22;
      Result.val13 := m1.val10*m2.val03 + m1.val11*m2.val13 + m1.val12*m2.val23;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10 + m1.val22*m2.val20;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11 + m1.val22*m2.val21;
      Result.val22 := m1.val20*m2.val02 + m1.val21*m2.val12 + m1.val22*m2.val22;
      Result.val23 := m1.val20*m2.val03 + m1.val21*m2.val13 + m1.val22*m2.val23;
    end;
    
    function operator*(m1: Mtr4x4f; m2: Mtr4x4f): Mtr4x4f; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20 + m1.val03*m2.val30;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21 + m1.val03*m2.val31;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12 + m1.val02*m2.val22 + m1.val03*m2.val32;
      Result.val03 := m1.val00*m2.val03 + m1.val01*m2.val13 + m1.val02*m2.val23 + m1.val03*m2.val33;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20 + m1.val13*m2.val30;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21 + m1.val13*m2.val31;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12 + m1.val12*m2.val22 + m1.val13*m2.val32;
      Result.val13 := m1.val10*m2.val03 + m1.val11*m2.val13 + m1.val12*m2.val23 + m1.val13*m2.val33;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10 + m1.val22*m2.val20 + m1.val23*m2.val30;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11 + m1.val22*m2.val21 + m1.val23*m2.val31;
      Result.val22 := m1.val20*m2.val02 + m1.val21*m2.val12 + m1.val22*m2.val22 + m1.val23*m2.val32;
      Result.val23 := m1.val20*m2.val03 + m1.val21*m2.val13 + m1.val22*m2.val23 + m1.val23*m2.val33;
      Result.val30 := m1.val30*m2.val00 + m1.val31*m2.val10 + m1.val32*m2.val20 + m1.val33*m2.val30;
      Result.val31 := m1.val30*m2.val01 + m1.val31*m2.val11 + m1.val32*m2.val21 + m1.val33*m2.val31;
      Result.val32 := m1.val30*m2.val02 + m1.val31*m2.val12 + m1.val32*m2.val22 + m1.val33*m2.val32;
      Result.val33 := m1.val30*m2.val03 + m1.val31*m2.val13 + m1.val32*m2.val23 + m1.val33*m2.val33;
    end;
    
    function operator*(m1: Mtr4x4f; m2: Mtr4x2f): Mtr4x2f; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20 + m1.val03*m2.val30;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21 + m1.val03*m2.val31;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20 + m1.val13*m2.val30;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21 + m1.val13*m2.val31;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10 + m1.val22*m2.val20 + m1.val23*m2.val30;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11 + m1.val22*m2.val21 + m1.val23*m2.val31;
      Result.val30 := m1.val30*m2.val00 + m1.val31*m2.val10 + m1.val32*m2.val20 + m1.val33*m2.val30;
      Result.val31 := m1.val30*m2.val01 + m1.val31*m2.val11 + m1.val32*m2.val21 + m1.val33*m2.val31;
    end;
    
    function operator*(m1: Mtr4x4f; m2: Mtr4x3f): Mtr4x3f; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20 + m1.val03*m2.val30;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21 + m1.val03*m2.val31;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12 + m1.val02*m2.val22 + m1.val03*m2.val32;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20 + m1.val13*m2.val30;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21 + m1.val13*m2.val31;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12 + m1.val12*m2.val22 + m1.val13*m2.val32;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10 + m1.val22*m2.val20 + m1.val23*m2.val30;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11 + m1.val22*m2.val21 + m1.val23*m2.val31;
      Result.val22 := m1.val20*m2.val02 + m1.val21*m2.val12 + m1.val22*m2.val22 + m1.val23*m2.val32;
      Result.val30 := m1.val30*m2.val00 + m1.val31*m2.val10 + m1.val32*m2.val20 + m1.val33*m2.val30;
      Result.val31 := m1.val30*m2.val01 + m1.val31*m2.val11 + m1.val32*m2.val21 + m1.val33*m2.val31;
      Result.val32 := m1.val30*m2.val02 + m1.val31*m2.val12 + m1.val32*m2.val22 + m1.val33*m2.val32;
    end;
    
    function operator*(m1: Mtr4x4f; m2: Mtr4x4d): Mtr4x4f; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20 + m1.val03*m2.val30;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21 + m1.val03*m2.val31;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12 + m1.val02*m2.val22 + m1.val03*m2.val32;
      Result.val03 := m1.val00*m2.val03 + m1.val01*m2.val13 + m1.val02*m2.val23 + m1.val03*m2.val33;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20 + m1.val13*m2.val30;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21 + m1.val13*m2.val31;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12 + m1.val12*m2.val22 + m1.val13*m2.val32;
      Result.val13 := m1.val10*m2.val03 + m1.val11*m2.val13 + m1.val12*m2.val23 + m1.val13*m2.val33;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10 + m1.val22*m2.val20 + m1.val23*m2.val30;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11 + m1.val22*m2.val21 + m1.val23*m2.val31;
      Result.val22 := m1.val20*m2.val02 + m1.val21*m2.val12 + m1.val22*m2.val22 + m1.val23*m2.val32;
      Result.val23 := m1.val20*m2.val03 + m1.val21*m2.val13 + m1.val22*m2.val23 + m1.val23*m2.val33;
      Result.val30 := m1.val30*m2.val00 + m1.val31*m2.val10 + m1.val32*m2.val20 + m1.val33*m2.val30;
      Result.val31 := m1.val30*m2.val01 + m1.val31*m2.val11 + m1.val32*m2.val21 + m1.val33*m2.val31;
      Result.val32 := m1.val30*m2.val02 + m1.val31*m2.val12 + m1.val32*m2.val22 + m1.val33*m2.val32;
      Result.val33 := m1.val30*m2.val03 + m1.val31*m2.val13 + m1.val32*m2.val23 + m1.val33*m2.val33;
    end;
    
    function operator*(m1: Mtr4x4f; m2: Mtr4x2d): Mtr4x2f; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20 + m1.val03*m2.val30;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21 + m1.val03*m2.val31;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20 + m1.val13*m2.val30;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21 + m1.val13*m2.val31;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10 + m1.val22*m2.val20 + m1.val23*m2.val30;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11 + m1.val22*m2.val21 + m1.val23*m2.val31;
      Result.val30 := m1.val30*m2.val00 + m1.val31*m2.val10 + m1.val32*m2.val20 + m1.val33*m2.val30;
      Result.val31 := m1.val30*m2.val01 + m1.val31*m2.val11 + m1.val32*m2.val21 + m1.val33*m2.val31;
    end;
    
    function operator*(m1: Mtr4x4f; m2: Mtr4x3d): Mtr4x3f; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20 + m1.val03*m2.val30;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21 + m1.val03*m2.val31;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12 + m1.val02*m2.val22 + m1.val03*m2.val32;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20 + m1.val13*m2.val30;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21 + m1.val13*m2.val31;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12 + m1.val12*m2.val22 + m1.val13*m2.val32;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10 + m1.val22*m2.val20 + m1.val23*m2.val30;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11 + m1.val22*m2.val21 + m1.val23*m2.val31;
      Result.val22 := m1.val20*m2.val02 + m1.val21*m2.val12 + m1.val22*m2.val22 + m1.val23*m2.val32;
      Result.val30 := m1.val30*m2.val00 + m1.val31*m2.val10 + m1.val32*m2.val20 + m1.val33*m2.val30;
      Result.val31 := m1.val30*m2.val01 + m1.val31*m2.val11 + m1.val32*m2.val21 + m1.val33*m2.val31;
      Result.val32 := m1.val30*m2.val02 + m1.val31*m2.val12 + m1.val32*m2.val22 + m1.val33*m2.val32;
    end;
    
    function operator*(m1: Mtr2x3f; m2: Mtr3x3f): Mtr2x3f; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12 + m1.val02*m2.val22;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12 + m1.val12*m2.val22;
    end;
    
    function operator*(m1: Mtr2x3f; m2: Mtr3x2f): Mtr2x2f; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21;
    end;
    
    function operator*(m1: Mtr2x3f; m2: Mtr3x4f): Mtr2x4f; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12 + m1.val02*m2.val22;
      Result.val03 := m1.val00*m2.val03 + m1.val01*m2.val13 + m1.val02*m2.val23;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12 + m1.val12*m2.val22;
      Result.val13 := m1.val10*m2.val03 + m1.val11*m2.val13 + m1.val12*m2.val23;
    end;
    
    function operator*(m1: Mtr2x3f; m2: Mtr3x3d): Mtr2x3f; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12 + m1.val02*m2.val22;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12 + m1.val12*m2.val22;
    end;
    
    function operator*(m1: Mtr2x3f; m2: Mtr3x2d): Mtr2x2f; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21;
    end;
    
    function operator*(m1: Mtr2x3f; m2: Mtr3x4d): Mtr2x4f; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12 + m1.val02*m2.val22;
      Result.val03 := m1.val00*m2.val03 + m1.val01*m2.val13 + m1.val02*m2.val23;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12 + m1.val12*m2.val22;
      Result.val13 := m1.val10*m2.val03 + m1.val11*m2.val13 + m1.val12*m2.val23;
    end;
    
    function operator*(m1: Mtr3x2f; m2: Mtr2x2f): Mtr3x2f; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11;
    end;
    
    function operator*(m1: Mtr3x2f; m2: Mtr2x3f): Mtr3x3f; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11;
      Result.val22 := m1.val20*m2.val02 + m1.val21*m2.val12;
    end;
    
    function operator*(m1: Mtr3x2f; m2: Mtr2x4f): Mtr3x4f; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12;
      Result.val03 := m1.val00*m2.val03 + m1.val01*m2.val13;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12;
      Result.val13 := m1.val10*m2.val03 + m1.val11*m2.val13;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11;
      Result.val22 := m1.val20*m2.val02 + m1.val21*m2.val12;
      Result.val23 := m1.val20*m2.val03 + m1.val21*m2.val13;
    end;
    
    function operator*(m1: Mtr3x2f; m2: Mtr2x2d): Mtr3x2f; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11;
    end;
    
    function operator*(m1: Mtr3x2f; m2: Mtr2x3d): Mtr3x3f; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11;
      Result.val22 := m1.val20*m2.val02 + m1.val21*m2.val12;
    end;
    
    function operator*(m1: Mtr3x2f; m2: Mtr2x4d): Mtr3x4f; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12;
      Result.val03 := m1.val00*m2.val03 + m1.val01*m2.val13;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12;
      Result.val13 := m1.val10*m2.val03 + m1.val11*m2.val13;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11;
      Result.val22 := m1.val20*m2.val02 + m1.val21*m2.val12;
      Result.val23 := m1.val20*m2.val03 + m1.val21*m2.val13;
    end;
    
    function operator*(m1: Mtr2x4f; m2: Mtr4x4f): Mtr2x4f; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20 + m1.val03*m2.val30;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21 + m1.val03*m2.val31;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12 + m1.val02*m2.val22 + m1.val03*m2.val32;
      Result.val03 := m1.val00*m2.val03 + m1.val01*m2.val13 + m1.val02*m2.val23 + m1.val03*m2.val33;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20 + m1.val13*m2.val30;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21 + m1.val13*m2.val31;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12 + m1.val12*m2.val22 + m1.val13*m2.val32;
      Result.val13 := m1.val10*m2.val03 + m1.val11*m2.val13 + m1.val12*m2.val23 + m1.val13*m2.val33;
    end;
    
    function operator*(m1: Mtr2x4f; m2: Mtr4x2f): Mtr2x2f; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20 + m1.val03*m2.val30;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21 + m1.val03*m2.val31;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20 + m1.val13*m2.val30;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21 + m1.val13*m2.val31;
    end;
    
    function operator*(m1: Mtr2x4f; m2: Mtr4x3f): Mtr2x3f; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20 + m1.val03*m2.val30;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21 + m1.val03*m2.val31;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12 + m1.val02*m2.val22 + m1.val03*m2.val32;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20 + m1.val13*m2.val30;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21 + m1.val13*m2.val31;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12 + m1.val12*m2.val22 + m1.val13*m2.val32;
    end;
    
    function operator*(m1: Mtr2x4f; m2: Mtr4x4d): Mtr2x4f; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20 + m1.val03*m2.val30;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21 + m1.val03*m2.val31;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12 + m1.val02*m2.val22 + m1.val03*m2.val32;
      Result.val03 := m1.val00*m2.val03 + m1.val01*m2.val13 + m1.val02*m2.val23 + m1.val03*m2.val33;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20 + m1.val13*m2.val30;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21 + m1.val13*m2.val31;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12 + m1.val12*m2.val22 + m1.val13*m2.val32;
      Result.val13 := m1.val10*m2.val03 + m1.val11*m2.val13 + m1.val12*m2.val23 + m1.val13*m2.val33;
    end;
    
    function operator*(m1: Mtr2x4f; m2: Mtr4x2d): Mtr2x2f; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20 + m1.val03*m2.val30;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21 + m1.val03*m2.val31;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20 + m1.val13*m2.val30;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21 + m1.val13*m2.val31;
    end;
    
    function operator*(m1: Mtr2x4f; m2: Mtr4x3d): Mtr2x3f; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20 + m1.val03*m2.val30;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21 + m1.val03*m2.val31;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12 + m1.val02*m2.val22 + m1.val03*m2.val32;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20 + m1.val13*m2.val30;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21 + m1.val13*m2.val31;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12 + m1.val12*m2.val22 + m1.val13*m2.val32;
    end;
    
    function operator*(m1: Mtr4x2f; m2: Mtr2x2f): Mtr4x2f; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11;
      Result.val30 := m1.val30*m2.val00 + m1.val31*m2.val10;
      Result.val31 := m1.val30*m2.val01 + m1.val31*m2.val11;
    end;
    
    function operator*(m1: Mtr4x2f; m2: Mtr2x3f): Mtr4x3f; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11;
      Result.val22 := m1.val20*m2.val02 + m1.val21*m2.val12;
      Result.val30 := m1.val30*m2.val00 + m1.val31*m2.val10;
      Result.val31 := m1.val30*m2.val01 + m1.val31*m2.val11;
      Result.val32 := m1.val30*m2.val02 + m1.val31*m2.val12;
    end;
    
    function operator*(m1: Mtr4x2f; m2: Mtr2x4f): Mtr4x4f; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12;
      Result.val03 := m1.val00*m2.val03 + m1.val01*m2.val13;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12;
      Result.val13 := m1.val10*m2.val03 + m1.val11*m2.val13;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11;
      Result.val22 := m1.val20*m2.val02 + m1.val21*m2.val12;
      Result.val23 := m1.val20*m2.val03 + m1.val21*m2.val13;
      Result.val30 := m1.val30*m2.val00 + m1.val31*m2.val10;
      Result.val31 := m1.val30*m2.val01 + m1.val31*m2.val11;
      Result.val32 := m1.val30*m2.val02 + m1.val31*m2.val12;
      Result.val33 := m1.val30*m2.val03 + m1.val31*m2.val13;
    end;
    
    function operator*(m1: Mtr4x2f; m2: Mtr2x2d): Mtr4x2f; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11;
      Result.val30 := m1.val30*m2.val00 + m1.val31*m2.val10;
      Result.val31 := m1.val30*m2.val01 + m1.val31*m2.val11;
    end;
    
    function operator*(m1: Mtr4x2f; m2: Mtr2x3d): Mtr4x3f; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11;
      Result.val22 := m1.val20*m2.val02 + m1.val21*m2.val12;
      Result.val30 := m1.val30*m2.val00 + m1.val31*m2.val10;
      Result.val31 := m1.val30*m2.val01 + m1.val31*m2.val11;
      Result.val32 := m1.val30*m2.val02 + m1.val31*m2.val12;
    end;
    
    function operator*(m1: Mtr4x2f; m2: Mtr2x4d): Mtr4x4f; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12;
      Result.val03 := m1.val00*m2.val03 + m1.val01*m2.val13;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12;
      Result.val13 := m1.val10*m2.val03 + m1.val11*m2.val13;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11;
      Result.val22 := m1.val20*m2.val02 + m1.val21*m2.val12;
      Result.val23 := m1.val20*m2.val03 + m1.val21*m2.val13;
      Result.val30 := m1.val30*m2.val00 + m1.val31*m2.val10;
      Result.val31 := m1.val30*m2.val01 + m1.val31*m2.val11;
      Result.val32 := m1.val30*m2.val02 + m1.val31*m2.val12;
      Result.val33 := m1.val30*m2.val03 + m1.val31*m2.val13;
    end;
    
    function operator*(m1: Mtr3x4f; m2: Mtr4x4f): Mtr3x4f; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20 + m1.val03*m2.val30;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21 + m1.val03*m2.val31;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12 + m1.val02*m2.val22 + m1.val03*m2.val32;
      Result.val03 := m1.val00*m2.val03 + m1.val01*m2.val13 + m1.val02*m2.val23 + m1.val03*m2.val33;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20 + m1.val13*m2.val30;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21 + m1.val13*m2.val31;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12 + m1.val12*m2.val22 + m1.val13*m2.val32;
      Result.val13 := m1.val10*m2.val03 + m1.val11*m2.val13 + m1.val12*m2.val23 + m1.val13*m2.val33;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10 + m1.val22*m2.val20 + m1.val23*m2.val30;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11 + m1.val22*m2.val21 + m1.val23*m2.val31;
      Result.val22 := m1.val20*m2.val02 + m1.val21*m2.val12 + m1.val22*m2.val22 + m1.val23*m2.val32;
      Result.val23 := m1.val20*m2.val03 + m1.val21*m2.val13 + m1.val22*m2.val23 + m1.val23*m2.val33;
    end;
    
    function operator*(m1: Mtr3x4f; m2: Mtr4x2f): Mtr3x2f; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20 + m1.val03*m2.val30;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21 + m1.val03*m2.val31;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20 + m1.val13*m2.val30;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21 + m1.val13*m2.val31;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10 + m1.val22*m2.val20 + m1.val23*m2.val30;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11 + m1.val22*m2.val21 + m1.val23*m2.val31;
    end;
    
    function operator*(m1: Mtr3x4f; m2: Mtr4x3f): Mtr3x3f; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20 + m1.val03*m2.val30;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21 + m1.val03*m2.val31;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12 + m1.val02*m2.val22 + m1.val03*m2.val32;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20 + m1.val13*m2.val30;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21 + m1.val13*m2.val31;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12 + m1.val12*m2.val22 + m1.val13*m2.val32;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10 + m1.val22*m2.val20 + m1.val23*m2.val30;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11 + m1.val22*m2.val21 + m1.val23*m2.val31;
      Result.val22 := m1.val20*m2.val02 + m1.val21*m2.val12 + m1.val22*m2.val22 + m1.val23*m2.val32;
    end;
    
    function operator*(m1: Mtr3x4f; m2: Mtr4x4d): Mtr3x4f; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20 + m1.val03*m2.val30;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21 + m1.val03*m2.val31;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12 + m1.val02*m2.val22 + m1.val03*m2.val32;
      Result.val03 := m1.val00*m2.val03 + m1.val01*m2.val13 + m1.val02*m2.val23 + m1.val03*m2.val33;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20 + m1.val13*m2.val30;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21 + m1.val13*m2.val31;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12 + m1.val12*m2.val22 + m1.val13*m2.val32;
      Result.val13 := m1.val10*m2.val03 + m1.val11*m2.val13 + m1.val12*m2.val23 + m1.val13*m2.val33;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10 + m1.val22*m2.val20 + m1.val23*m2.val30;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11 + m1.val22*m2.val21 + m1.val23*m2.val31;
      Result.val22 := m1.val20*m2.val02 + m1.val21*m2.val12 + m1.val22*m2.val22 + m1.val23*m2.val32;
      Result.val23 := m1.val20*m2.val03 + m1.val21*m2.val13 + m1.val22*m2.val23 + m1.val23*m2.val33;
    end;
    
    function operator*(m1: Mtr3x4f; m2: Mtr4x2d): Mtr3x2f; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20 + m1.val03*m2.val30;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21 + m1.val03*m2.val31;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20 + m1.val13*m2.val30;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21 + m1.val13*m2.val31;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10 + m1.val22*m2.val20 + m1.val23*m2.val30;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11 + m1.val22*m2.val21 + m1.val23*m2.val31;
    end;
    
    function operator*(m1: Mtr3x4f; m2: Mtr4x3d): Mtr3x3f; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20 + m1.val03*m2.val30;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21 + m1.val03*m2.val31;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12 + m1.val02*m2.val22 + m1.val03*m2.val32;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20 + m1.val13*m2.val30;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21 + m1.val13*m2.val31;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12 + m1.val12*m2.val22 + m1.val13*m2.val32;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10 + m1.val22*m2.val20 + m1.val23*m2.val30;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11 + m1.val22*m2.val21 + m1.val23*m2.val31;
      Result.val22 := m1.val20*m2.val02 + m1.val21*m2.val12 + m1.val22*m2.val22 + m1.val23*m2.val32;
    end;
    
    function operator*(m1: Mtr4x3f; m2: Mtr3x3f): Mtr4x3f; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12 + m1.val02*m2.val22;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12 + m1.val12*m2.val22;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10 + m1.val22*m2.val20;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11 + m1.val22*m2.val21;
      Result.val22 := m1.val20*m2.val02 + m1.val21*m2.val12 + m1.val22*m2.val22;
      Result.val30 := m1.val30*m2.val00 + m1.val31*m2.val10 + m1.val32*m2.val20;
      Result.val31 := m1.val30*m2.val01 + m1.val31*m2.val11 + m1.val32*m2.val21;
      Result.val32 := m1.val30*m2.val02 + m1.val31*m2.val12 + m1.val32*m2.val22;
    end;
    
    function operator*(m1: Mtr4x3f; m2: Mtr3x2f): Mtr4x2f; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10 + m1.val22*m2.val20;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11 + m1.val22*m2.val21;
      Result.val30 := m1.val30*m2.val00 + m1.val31*m2.val10 + m1.val32*m2.val20;
      Result.val31 := m1.val30*m2.val01 + m1.val31*m2.val11 + m1.val32*m2.val21;
    end;
    
    function operator*(m1: Mtr4x3f; m2: Mtr3x4f): Mtr4x4f; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12 + m1.val02*m2.val22;
      Result.val03 := m1.val00*m2.val03 + m1.val01*m2.val13 + m1.val02*m2.val23;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12 + m1.val12*m2.val22;
      Result.val13 := m1.val10*m2.val03 + m1.val11*m2.val13 + m1.val12*m2.val23;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10 + m1.val22*m2.val20;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11 + m1.val22*m2.val21;
      Result.val22 := m1.val20*m2.val02 + m1.val21*m2.val12 + m1.val22*m2.val22;
      Result.val23 := m1.val20*m2.val03 + m1.val21*m2.val13 + m1.val22*m2.val23;
      Result.val30 := m1.val30*m2.val00 + m1.val31*m2.val10 + m1.val32*m2.val20;
      Result.val31 := m1.val30*m2.val01 + m1.val31*m2.val11 + m1.val32*m2.val21;
      Result.val32 := m1.val30*m2.val02 + m1.val31*m2.val12 + m1.val32*m2.val22;
      Result.val33 := m1.val30*m2.val03 + m1.val31*m2.val13 + m1.val32*m2.val23;
    end;
    
    function operator*(m1: Mtr4x3f; m2: Mtr3x3d): Mtr4x3f; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12 + m1.val02*m2.val22;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12 + m1.val12*m2.val22;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10 + m1.val22*m2.val20;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11 + m1.val22*m2.val21;
      Result.val22 := m1.val20*m2.val02 + m1.val21*m2.val12 + m1.val22*m2.val22;
      Result.val30 := m1.val30*m2.val00 + m1.val31*m2.val10 + m1.val32*m2.val20;
      Result.val31 := m1.val30*m2.val01 + m1.val31*m2.val11 + m1.val32*m2.val21;
      Result.val32 := m1.val30*m2.val02 + m1.val31*m2.val12 + m1.val32*m2.val22;
    end;
    
    function operator*(m1: Mtr4x3f; m2: Mtr3x2d): Mtr4x2f; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10 + m1.val22*m2.val20;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11 + m1.val22*m2.val21;
      Result.val30 := m1.val30*m2.val00 + m1.val31*m2.val10 + m1.val32*m2.val20;
      Result.val31 := m1.val30*m2.val01 + m1.val31*m2.val11 + m1.val32*m2.val21;
    end;
    
    function operator*(m1: Mtr4x3f; m2: Mtr3x4d): Mtr4x4f; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12 + m1.val02*m2.val22;
      Result.val03 := m1.val00*m2.val03 + m1.val01*m2.val13 + m1.val02*m2.val23;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12 + m1.val12*m2.val22;
      Result.val13 := m1.val10*m2.val03 + m1.val11*m2.val13 + m1.val12*m2.val23;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10 + m1.val22*m2.val20;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11 + m1.val22*m2.val21;
      Result.val22 := m1.val20*m2.val02 + m1.val21*m2.val12 + m1.val22*m2.val22;
      Result.val23 := m1.val20*m2.val03 + m1.val21*m2.val13 + m1.val22*m2.val23;
      Result.val30 := m1.val30*m2.val00 + m1.val31*m2.val10 + m1.val32*m2.val20;
      Result.val31 := m1.val30*m2.val01 + m1.val31*m2.val11 + m1.val32*m2.val21;
      Result.val32 := m1.val30*m2.val02 + m1.val31*m2.val12 + m1.val32*m2.val22;
      Result.val33 := m1.val30*m2.val03 + m1.val31*m2.val13 + m1.val32*m2.val23;
    end;
    
    function operator*(m1: Mtr2x2d; m2: Mtr2x2f): Mtr2x2d; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11;
    end;
    
    function operator*(m1: Mtr2x2d; m2: Mtr2x3f): Mtr2x3d; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12;
    end;
    
    function operator*(m1: Mtr2x2d; m2: Mtr2x4f): Mtr2x4d; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12;
      Result.val03 := m1.val00*m2.val03 + m1.val01*m2.val13;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12;
      Result.val13 := m1.val10*m2.val03 + m1.val11*m2.val13;
    end;
    
    function operator*(m1: Mtr2x2d; m2: Mtr2x2d): Mtr2x2d; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11;
    end;
    
    function operator*(m1: Mtr2x2d; m2: Mtr2x3d): Mtr2x3d; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12;
    end;
    
    function operator*(m1: Mtr2x2d; m2: Mtr2x4d): Mtr2x4d; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12;
      Result.val03 := m1.val00*m2.val03 + m1.val01*m2.val13;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12;
      Result.val13 := m1.val10*m2.val03 + m1.val11*m2.val13;
    end;
    
    function operator*(m1: Mtr3x3d; m2: Mtr3x3f): Mtr3x3d; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12 + m1.val02*m2.val22;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12 + m1.val12*m2.val22;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10 + m1.val22*m2.val20;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11 + m1.val22*m2.val21;
      Result.val22 := m1.val20*m2.val02 + m1.val21*m2.val12 + m1.val22*m2.val22;
    end;
    
    function operator*(m1: Mtr3x3d; m2: Mtr3x2f): Mtr3x2d; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10 + m1.val22*m2.val20;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11 + m1.val22*m2.val21;
    end;
    
    function operator*(m1: Mtr3x3d; m2: Mtr3x4f): Mtr3x4d; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12 + m1.val02*m2.val22;
      Result.val03 := m1.val00*m2.val03 + m1.val01*m2.val13 + m1.val02*m2.val23;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12 + m1.val12*m2.val22;
      Result.val13 := m1.val10*m2.val03 + m1.val11*m2.val13 + m1.val12*m2.val23;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10 + m1.val22*m2.val20;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11 + m1.val22*m2.val21;
      Result.val22 := m1.val20*m2.val02 + m1.val21*m2.val12 + m1.val22*m2.val22;
      Result.val23 := m1.val20*m2.val03 + m1.val21*m2.val13 + m1.val22*m2.val23;
    end;
    
    function operator*(m1: Mtr3x3d; m2: Mtr3x3d): Mtr3x3d; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12 + m1.val02*m2.val22;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12 + m1.val12*m2.val22;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10 + m1.val22*m2.val20;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11 + m1.val22*m2.val21;
      Result.val22 := m1.val20*m2.val02 + m1.val21*m2.val12 + m1.val22*m2.val22;
    end;
    
    function operator*(m1: Mtr3x3d; m2: Mtr3x2d): Mtr3x2d; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10 + m1.val22*m2.val20;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11 + m1.val22*m2.val21;
    end;
    
    function operator*(m1: Mtr3x3d; m2: Mtr3x4d): Mtr3x4d; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12 + m1.val02*m2.val22;
      Result.val03 := m1.val00*m2.val03 + m1.val01*m2.val13 + m1.val02*m2.val23;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12 + m1.val12*m2.val22;
      Result.val13 := m1.val10*m2.val03 + m1.val11*m2.val13 + m1.val12*m2.val23;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10 + m1.val22*m2.val20;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11 + m1.val22*m2.val21;
      Result.val22 := m1.val20*m2.val02 + m1.val21*m2.val12 + m1.val22*m2.val22;
      Result.val23 := m1.val20*m2.val03 + m1.val21*m2.val13 + m1.val22*m2.val23;
    end;
    
    function operator*(m1: Mtr4x4d; m2: Mtr4x4f): Mtr4x4d; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20 + m1.val03*m2.val30;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21 + m1.val03*m2.val31;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12 + m1.val02*m2.val22 + m1.val03*m2.val32;
      Result.val03 := m1.val00*m2.val03 + m1.val01*m2.val13 + m1.val02*m2.val23 + m1.val03*m2.val33;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20 + m1.val13*m2.val30;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21 + m1.val13*m2.val31;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12 + m1.val12*m2.val22 + m1.val13*m2.val32;
      Result.val13 := m1.val10*m2.val03 + m1.val11*m2.val13 + m1.val12*m2.val23 + m1.val13*m2.val33;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10 + m1.val22*m2.val20 + m1.val23*m2.val30;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11 + m1.val22*m2.val21 + m1.val23*m2.val31;
      Result.val22 := m1.val20*m2.val02 + m1.val21*m2.val12 + m1.val22*m2.val22 + m1.val23*m2.val32;
      Result.val23 := m1.val20*m2.val03 + m1.val21*m2.val13 + m1.val22*m2.val23 + m1.val23*m2.val33;
      Result.val30 := m1.val30*m2.val00 + m1.val31*m2.val10 + m1.val32*m2.val20 + m1.val33*m2.val30;
      Result.val31 := m1.val30*m2.val01 + m1.val31*m2.val11 + m1.val32*m2.val21 + m1.val33*m2.val31;
      Result.val32 := m1.val30*m2.val02 + m1.val31*m2.val12 + m1.val32*m2.val22 + m1.val33*m2.val32;
      Result.val33 := m1.val30*m2.val03 + m1.val31*m2.val13 + m1.val32*m2.val23 + m1.val33*m2.val33;
    end;
    
    function operator*(m1: Mtr4x4d; m2: Mtr4x2f): Mtr4x2d; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20 + m1.val03*m2.val30;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21 + m1.val03*m2.val31;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20 + m1.val13*m2.val30;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21 + m1.val13*m2.val31;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10 + m1.val22*m2.val20 + m1.val23*m2.val30;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11 + m1.val22*m2.val21 + m1.val23*m2.val31;
      Result.val30 := m1.val30*m2.val00 + m1.val31*m2.val10 + m1.val32*m2.val20 + m1.val33*m2.val30;
      Result.val31 := m1.val30*m2.val01 + m1.val31*m2.val11 + m1.val32*m2.val21 + m1.val33*m2.val31;
    end;
    
    function operator*(m1: Mtr4x4d; m2: Mtr4x3f): Mtr4x3d; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20 + m1.val03*m2.val30;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21 + m1.val03*m2.val31;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12 + m1.val02*m2.val22 + m1.val03*m2.val32;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20 + m1.val13*m2.val30;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21 + m1.val13*m2.val31;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12 + m1.val12*m2.val22 + m1.val13*m2.val32;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10 + m1.val22*m2.val20 + m1.val23*m2.val30;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11 + m1.val22*m2.val21 + m1.val23*m2.val31;
      Result.val22 := m1.val20*m2.val02 + m1.val21*m2.val12 + m1.val22*m2.val22 + m1.val23*m2.val32;
      Result.val30 := m1.val30*m2.val00 + m1.val31*m2.val10 + m1.val32*m2.val20 + m1.val33*m2.val30;
      Result.val31 := m1.val30*m2.val01 + m1.val31*m2.val11 + m1.val32*m2.val21 + m1.val33*m2.val31;
      Result.val32 := m1.val30*m2.val02 + m1.val31*m2.val12 + m1.val32*m2.val22 + m1.val33*m2.val32;
    end;
    
    function operator*(m1: Mtr4x4d; m2: Mtr4x4d): Mtr4x4d; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20 + m1.val03*m2.val30;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21 + m1.val03*m2.val31;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12 + m1.val02*m2.val22 + m1.val03*m2.val32;
      Result.val03 := m1.val00*m2.val03 + m1.val01*m2.val13 + m1.val02*m2.val23 + m1.val03*m2.val33;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20 + m1.val13*m2.val30;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21 + m1.val13*m2.val31;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12 + m1.val12*m2.val22 + m1.val13*m2.val32;
      Result.val13 := m1.val10*m2.val03 + m1.val11*m2.val13 + m1.val12*m2.val23 + m1.val13*m2.val33;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10 + m1.val22*m2.val20 + m1.val23*m2.val30;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11 + m1.val22*m2.val21 + m1.val23*m2.val31;
      Result.val22 := m1.val20*m2.val02 + m1.val21*m2.val12 + m1.val22*m2.val22 + m1.val23*m2.val32;
      Result.val23 := m1.val20*m2.val03 + m1.val21*m2.val13 + m1.val22*m2.val23 + m1.val23*m2.val33;
      Result.val30 := m1.val30*m2.val00 + m1.val31*m2.val10 + m1.val32*m2.val20 + m1.val33*m2.val30;
      Result.val31 := m1.val30*m2.val01 + m1.val31*m2.val11 + m1.val32*m2.val21 + m1.val33*m2.val31;
      Result.val32 := m1.val30*m2.val02 + m1.val31*m2.val12 + m1.val32*m2.val22 + m1.val33*m2.val32;
      Result.val33 := m1.val30*m2.val03 + m1.val31*m2.val13 + m1.val32*m2.val23 + m1.val33*m2.val33;
    end;
    
    function operator*(m1: Mtr4x4d; m2: Mtr4x2d): Mtr4x2d; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20 + m1.val03*m2.val30;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21 + m1.val03*m2.val31;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20 + m1.val13*m2.val30;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21 + m1.val13*m2.val31;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10 + m1.val22*m2.val20 + m1.val23*m2.val30;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11 + m1.val22*m2.val21 + m1.val23*m2.val31;
      Result.val30 := m1.val30*m2.val00 + m1.val31*m2.val10 + m1.val32*m2.val20 + m1.val33*m2.val30;
      Result.val31 := m1.val30*m2.val01 + m1.val31*m2.val11 + m1.val32*m2.val21 + m1.val33*m2.val31;
    end;
    
    function operator*(m1: Mtr4x4d; m2: Mtr4x3d): Mtr4x3d; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20 + m1.val03*m2.val30;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21 + m1.val03*m2.val31;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12 + m1.val02*m2.val22 + m1.val03*m2.val32;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20 + m1.val13*m2.val30;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21 + m1.val13*m2.val31;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12 + m1.val12*m2.val22 + m1.val13*m2.val32;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10 + m1.val22*m2.val20 + m1.val23*m2.val30;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11 + m1.val22*m2.val21 + m1.val23*m2.val31;
      Result.val22 := m1.val20*m2.val02 + m1.val21*m2.val12 + m1.val22*m2.val22 + m1.val23*m2.val32;
      Result.val30 := m1.val30*m2.val00 + m1.val31*m2.val10 + m1.val32*m2.val20 + m1.val33*m2.val30;
      Result.val31 := m1.val30*m2.val01 + m1.val31*m2.val11 + m1.val32*m2.val21 + m1.val33*m2.val31;
      Result.val32 := m1.val30*m2.val02 + m1.val31*m2.val12 + m1.val32*m2.val22 + m1.val33*m2.val32;
    end;
    
    function operator*(m1: Mtr2x3d; m2: Mtr3x3f): Mtr2x3d; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12 + m1.val02*m2.val22;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12 + m1.val12*m2.val22;
    end;
    
    function operator*(m1: Mtr2x3d; m2: Mtr3x2f): Mtr2x2d; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21;
    end;
    
    function operator*(m1: Mtr2x3d; m2: Mtr3x4f): Mtr2x4d; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12 + m1.val02*m2.val22;
      Result.val03 := m1.val00*m2.val03 + m1.val01*m2.val13 + m1.val02*m2.val23;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12 + m1.val12*m2.val22;
      Result.val13 := m1.val10*m2.val03 + m1.val11*m2.val13 + m1.val12*m2.val23;
    end;
    
    function operator*(m1: Mtr2x3d; m2: Mtr3x3d): Mtr2x3d; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12 + m1.val02*m2.val22;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12 + m1.val12*m2.val22;
    end;
    
    function operator*(m1: Mtr2x3d; m2: Mtr3x2d): Mtr2x2d; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21;
    end;
    
    function operator*(m1: Mtr2x3d; m2: Mtr3x4d): Mtr2x4d; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12 + m1.val02*m2.val22;
      Result.val03 := m1.val00*m2.val03 + m1.val01*m2.val13 + m1.val02*m2.val23;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12 + m1.val12*m2.val22;
      Result.val13 := m1.val10*m2.val03 + m1.val11*m2.val13 + m1.val12*m2.val23;
    end;
    
    function operator*(m1: Mtr3x2d; m2: Mtr2x2f): Mtr3x2d; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11;
    end;
    
    function operator*(m1: Mtr3x2d; m2: Mtr2x3f): Mtr3x3d; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11;
      Result.val22 := m1.val20*m2.val02 + m1.val21*m2.val12;
    end;
    
    function operator*(m1: Mtr3x2d; m2: Mtr2x4f): Mtr3x4d; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12;
      Result.val03 := m1.val00*m2.val03 + m1.val01*m2.val13;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12;
      Result.val13 := m1.val10*m2.val03 + m1.val11*m2.val13;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11;
      Result.val22 := m1.val20*m2.val02 + m1.val21*m2.val12;
      Result.val23 := m1.val20*m2.val03 + m1.val21*m2.val13;
    end;
    
    function operator*(m1: Mtr3x2d; m2: Mtr2x2d): Mtr3x2d; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11;
    end;
    
    function operator*(m1: Mtr3x2d; m2: Mtr2x3d): Mtr3x3d; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11;
      Result.val22 := m1.val20*m2.val02 + m1.val21*m2.val12;
    end;
    
    function operator*(m1: Mtr3x2d; m2: Mtr2x4d): Mtr3x4d; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12;
      Result.val03 := m1.val00*m2.val03 + m1.val01*m2.val13;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12;
      Result.val13 := m1.val10*m2.val03 + m1.val11*m2.val13;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11;
      Result.val22 := m1.val20*m2.val02 + m1.val21*m2.val12;
      Result.val23 := m1.val20*m2.val03 + m1.val21*m2.val13;
    end;
    
    function operator*(m1: Mtr2x4d; m2: Mtr4x4f): Mtr2x4d; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20 + m1.val03*m2.val30;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21 + m1.val03*m2.val31;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12 + m1.val02*m2.val22 + m1.val03*m2.val32;
      Result.val03 := m1.val00*m2.val03 + m1.val01*m2.val13 + m1.val02*m2.val23 + m1.val03*m2.val33;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20 + m1.val13*m2.val30;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21 + m1.val13*m2.val31;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12 + m1.val12*m2.val22 + m1.val13*m2.val32;
      Result.val13 := m1.val10*m2.val03 + m1.val11*m2.val13 + m1.val12*m2.val23 + m1.val13*m2.val33;
    end;
    
    function operator*(m1: Mtr2x4d; m2: Mtr4x2f): Mtr2x2d; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20 + m1.val03*m2.val30;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21 + m1.val03*m2.val31;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20 + m1.val13*m2.val30;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21 + m1.val13*m2.val31;
    end;
    
    function operator*(m1: Mtr2x4d; m2: Mtr4x3f): Mtr2x3d; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20 + m1.val03*m2.val30;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21 + m1.val03*m2.val31;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12 + m1.val02*m2.val22 + m1.val03*m2.val32;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20 + m1.val13*m2.val30;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21 + m1.val13*m2.val31;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12 + m1.val12*m2.val22 + m1.val13*m2.val32;
    end;
    
    function operator*(m1: Mtr2x4d; m2: Mtr4x4d): Mtr2x4d; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20 + m1.val03*m2.val30;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21 + m1.val03*m2.val31;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12 + m1.val02*m2.val22 + m1.val03*m2.val32;
      Result.val03 := m1.val00*m2.val03 + m1.val01*m2.val13 + m1.val02*m2.val23 + m1.val03*m2.val33;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20 + m1.val13*m2.val30;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21 + m1.val13*m2.val31;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12 + m1.val12*m2.val22 + m1.val13*m2.val32;
      Result.val13 := m1.val10*m2.val03 + m1.val11*m2.val13 + m1.val12*m2.val23 + m1.val13*m2.val33;
    end;
    
    function operator*(m1: Mtr2x4d; m2: Mtr4x2d): Mtr2x2d; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20 + m1.val03*m2.val30;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21 + m1.val03*m2.val31;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20 + m1.val13*m2.val30;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21 + m1.val13*m2.val31;
    end;
    
    function operator*(m1: Mtr2x4d; m2: Mtr4x3d): Mtr2x3d; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20 + m1.val03*m2.val30;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21 + m1.val03*m2.val31;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12 + m1.val02*m2.val22 + m1.val03*m2.val32;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20 + m1.val13*m2.val30;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21 + m1.val13*m2.val31;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12 + m1.val12*m2.val22 + m1.val13*m2.val32;
    end;
    
    function operator*(m1: Mtr4x2d; m2: Mtr2x2f): Mtr4x2d; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11;
      Result.val30 := m1.val30*m2.val00 + m1.val31*m2.val10;
      Result.val31 := m1.val30*m2.val01 + m1.val31*m2.val11;
    end;
    
    function operator*(m1: Mtr4x2d; m2: Mtr2x3f): Mtr4x3d; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11;
      Result.val22 := m1.val20*m2.val02 + m1.val21*m2.val12;
      Result.val30 := m1.val30*m2.val00 + m1.val31*m2.val10;
      Result.val31 := m1.val30*m2.val01 + m1.val31*m2.val11;
      Result.val32 := m1.val30*m2.val02 + m1.val31*m2.val12;
    end;
    
    function operator*(m1: Mtr4x2d; m2: Mtr2x4f): Mtr4x4d; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12;
      Result.val03 := m1.val00*m2.val03 + m1.val01*m2.val13;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12;
      Result.val13 := m1.val10*m2.val03 + m1.val11*m2.val13;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11;
      Result.val22 := m1.val20*m2.val02 + m1.val21*m2.val12;
      Result.val23 := m1.val20*m2.val03 + m1.val21*m2.val13;
      Result.val30 := m1.val30*m2.val00 + m1.val31*m2.val10;
      Result.val31 := m1.val30*m2.val01 + m1.val31*m2.val11;
      Result.val32 := m1.val30*m2.val02 + m1.val31*m2.val12;
      Result.val33 := m1.val30*m2.val03 + m1.val31*m2.val13;
    end;
    
    function operator*(m1: Mtr4x2d; m2: Mtr2x2d): Mtr4x2d; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11;
      Result.val30 := m1.val30*m2.val00 + m1.val31*m2.val10;
      Result.val31 := m1.val30*m2.val01 + m1.val31*m2.val11;
    end;
    
    function operator*(m1: Mtr4x2d; m2: Mtr2x3d): Mtr4x3d; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11;
      Result.val22 := m1.val20*m2.val02 + m1.val21*m2.val12;
      Result.val30 := m1.val30*m2.val00 + m1.val31*m2.val10;
      Result.val31 := m1.val30*m2.val01 + m1.val31*m2.val11;
      Result.val32 := m1.val30*m2.val02 + m1.val31*m2.val12;
    end;
    
    function operator*(m1: Mtr4x2d; m2: Mtr2x4d): Mtr4x4d; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12;
      Result.val03 := m1.val00*m2.val03 + m1.val01*m2.val13;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12;
      Result.val13 := m1.val10*m2.val03 + m1.val11*m2.val13;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11;
      Result.val22 := m1.val20*m2.val02 + m1.val21*m2.val12;
      Result.val23 := m1.val20*m2.val03 + m1.val21*m2.val13;
      Result.val30 := m1.val30*m2.val00 + m1.val31*m2.val10;
      Result.val31 := m1.val30*m2.val01 + m1.val31*m2.val11;
      Result.val32 := m1.val30*m2.val02 + m1.val31*m2.val12;
      Result.val33 := m1.val30*m2.val03 + m1.val31*m2.val13;
    end;
    
    function operator*(m1: Mtr3x4d; m2: Mtr4x4f): Mtr3x4d; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20 + m1.val03*m2.val30;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21 + m1.val03*m2.val31;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12 + m1.val02*m2.val22 + m1.val03*m2.val32;
      Result.val03 := m1.val00*m2.val03 + m1.val01*m2.val13 + m1.val02*m2.val23 + m1.val03*m2.val33;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20 + m1.val13*m2.val30;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21 + m1.val13*m2.val31;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12 + m1.val12*m2.val22 + m1.val13*m2.val32;
      Result.val13 := m1.val10*m2.val03 + m1.val11*m2.val13 + m1.val12*m2.val23 + m1.val13*m2.val33;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10 + m1.val22*m2.val20 + m1.val23*m2.val30;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11 + m1.val22*m2.val21 + m1.val23*m2.val31;
      Result.val22 := m1.val20*m2.val02 + m1.val21*m2.val12 + m1.val22*m2.val22 + m1.val23*m2.val32;
      Result.val23 := m1.val20*m2.val03 + m1.val21*m2.val13 + m1.val22*m2.val23 + m1.val23*m2.val33;
    end;
    
    function operator*(m1: Mtr3x4d; m2: Mtr4x2f): Mtr3x2d; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20 + m1.val03*m2.val30;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21 + m1.val03*m2.val31;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20 + m1.val13*m2.val30;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21 + m1.val13*m2.val31;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10 + m1.val22*m2.val20 + m1.val23*m2.val30;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11 + m1.val22*m2.val21 + m1.val23*m2.val31;
    end;
    
    function operator*(m1: Mtr3x4d; m2: Mtr4x3f): Mtr3x3d; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20 + m1.val03*m2.val30;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21 + m1.val03*m2.val31;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12 + m1.val02*m2.val22 + m1.val03*m2.val32;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20 + m1.val13*m2.val30;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21 + m1.val13*m2.val31;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12 + m1.val12*m2.val22 + m1.val13*m2.val32;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10 + m1.val22*m2.val20 + m1.val23*m2.val30;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11 + m1.val22*m2.val21 + m1.val23*m2.val31;
      Result.val22 := m1.val20*m2.val02 + m1.val21*m2.val12 + m1.val22*m2.val22 + m1.val23*m2.val32;
    end;
    
    function operator*(m1: Mtr3x4d; m2: Mtr4x4d): Mtr3x4d; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20 + m1.val03*m2.val30;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21 + m1.val03*m2.val31;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12 + m1.val02*m2.val22 + m1.val03*m2.val32;
      Result.val03 := m1.val00*m2.val03 + m1.val01*m2.val13 + m1.val02*m2.val23 + m1.val03*m2.val33;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20 + m1.val13*m2.val30;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21 + m1.val13*m2.val31;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12 + m1.val12*m2.val22 + m1.val13*m2.val32;
      Result.val13 := m1.val10*m2.val03 + m1.val11*m2.val13 + m1.val12*m2.val23 + m1.val13*m2.val33;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10 + m1.val22*m2.val20 + m1.val23*m2.val30;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11 + m1.val22*m2.val21 + m1.val23*m2.val31;
      Result.val22 := m1.val20*m2.val02 + m1.val21*m2.val12 + m1.val22*m2.val22 + m1.val23*m2.val32;
      Result.val23 := m1.val20*m2.val03 + m1.val21*m2.val13 + m1.val22*m2.val23 + m1.val23*m2.val33;
    end;
    
    function operator*(m1: Mtr3x4d; m2: Mtr4x2d): Mtr3x2d; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20 + m1.val03*m2.val30;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21 + m1.val03*m2.val31;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20 + m1.val13*m2.val30;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21 + m1.val13*m2.val31;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10 + m1.val22*m2.val20 + m1.val23*m2.val30;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11 + m1.val22*m2.val21 + m1.val23*m2.val31;
    end;
    
    function operator*(m1: Mtr3x4d; m2: Mtr4x3d): Mtr3x3d; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20 + m1.val03*m2.val30;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21 + m1.val03*m2.val31;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12 + m1.val02*m2.val22 + m1.val03*m2.val32;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20 + m1.val13*m2.val30;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21 + m1.val13*m2.val31;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12 + m1.val12*m2.val22 + m1.val13*m2.val32;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10 + m1.val22*m2.val20 + m1.val23*m2.val30;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11 + m1.val22*m2.val21 + m1.val23*m2.val31;
      Result.val22 := m1.val20*m2.val02 + m1.val21*m2.val12 + m1.val22*m2.val22 + m1.val23*m2.val32;
    end;
    
    function operator*(m1: Mtr4x3d; m2: Mtr3x3f): Mtr4x3d; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12 + m1.val02*m2.val22;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12 + m1.val12*m2.val22;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10 + m1.val22*m2.val20;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11 + m1.val22*m2.val21;
      Result.val22 := m1.val20*m2.val02 + m1.val21*m2.val12 + m1.val22*m2.val22;
      Result.val30 := m1.val30*m2.val00 + m1.val31*m2.val10 + m1.val32*m2.val20;
      Result.val31 := m1.val30*m2.val01 + m1.val31*m2.val11 + m1.val32*m2.val21;
      Result.val32 := m1.val30*m2.val02 + m1.val31*m2.val12 + m1.val32*m2.val22;
    end;
    
    function operator*(m1: Mtr4x3d; m2: Mtr3x2f): Mtr4x2d; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10 + m1.val22*m2.val20;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11 + m1.val22*m2.val21;
      Result.val30 := m1.val30*m2.val00 + m1.val31*m2.val10 + m1.val32*m2.val20;
      Result.val31 := m1.val30*m2.val01 + m1.val31*m2.val11 + m1.val32*m2.val21;
    end;
    
    function operator*(m1: Mtr4x3d; m2: Mtr3x4f): Mtr4x4d; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12 + m1.val02*m2.val22;
      Result.val03 := m1.val00*m2.val03 + m1.val01*m2.val13 + m1.val02*m2.val23;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12 + m1.val12*m2.val22;
      Result.val13 := m1.val10*m2.val03 + m1.val11*m2.val13 + m1.val12*m2.val23;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10 + m1.val22*m2.val20;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11 + m1.val22*m2.val21;
      Result.val22 := m1.val20*m2.val02 + m1.val21*m2.val12 + m1.val22*m2.val22;
      Result.val23 := m1.val20*m2.val03 + m1.val21*m2.val13 + m1.val22*m2.val23;
      Result.val30 := m1.val30*m2.val00 + m1.val31*m2.val10 + m1.val32*m2.val20;
      Result.val31 := m1.val30*m2.val01 + m1.val31*m2.val11 + m1.val32*m2.val21;
      Result.val32 := m1.val30*m2.val02 + m1.val31*m2.val12 + m1.val32*m2.val22;
      Result.val33 := m1.val30*m2.val03 + m1.val31*m2.val13 + m1.val32*m2.val23;
    end;
    
    function operator*(m1: Mtr4x3d; m2: Mtr3x3d): Mtr4x3d; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12 + m1.val02*m2.val22;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12 + m1.val12*m2.val22;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10 + m1.val22*m2.val20;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11 + m1.val22*m2.val21;
      Result.val22 := m1.val20*m2.val02 + m1.val21*m2.val12 + m1.val22*m2.val22;
      Result.val30 := m1.val30*m2.val00 + m1.val31*m2.val10 + m1.val32*m2.val20;
      Result.val31 := m1.val30*m2.val01 + m1.val31*m2.val11 + m1.val32*m2.val21;
      Result.val32 := m1.val30*m2.val02 + m1.val31*m2.val12 + m1.val32*m2.val22;
    end;
    
    function operator*(m1: Mtr4x3d; m2: Mtr3x2d): Mtr4x2d; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10 + m1.val22*m2.val20;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11 + m1.val22*m2.val21;
      Result.val30 := m1.val30*m2.val00 + m1.val31*m2.val10 + m1.val32*m2.val20;
      Result.val31 := m1.val30*m2.val01 + m1.val31*m2.val11 + m1.val32*m2.val21;
    end;
    
    function operator*(m1: Mtr4x3d; m2: Mtr3x4d): Mtr4x4d; extensionmethod;
    begin
      Result.val00 := m1.val00*m2.val00 + m1.val01*m2.val10 + m1.val02*m2.val20;
      Result.val01 := m1.val00*m2.val01 + m1.val01*m2.val11 + m1.val02*m2.val21;
      Result.val02 := m1.val00*m2.val02 + m1.val01*m2.val12 + m1.val02*m2.val22;
      Result.val03 := m1.val00*m2.val03 + m1.val01*m2.val13 + m1.val02*m2.val23;
      Result.val10 := m1.val10*m2.val00 + m1.val11*m2.val10 + m1.val12*m2.val20;
      Result.val11 := m1.val10*m2.val01 + m1.val11*m2.val11 + m1.val12*m2.val21;
      Result.val12 := m1.val10*m2.val02 + m1.val11*m2.val12 + m1.val12*m2.val22;
      Result.val13 := m1.val10*m2.val03 + m1.val11*m2.val13 + m1.val12*m2.val23;
      Result.val20 := m1.val20*m2.val00 + m1.val21*m2.val10 + m1.val22*m2.val20;
      Result.val21 := m1.val20*m2.val01 + m1.val21*m2.val11 + m1.val22*m2.val21;
      Result.val22 := m1.val20*m2.val02 + m1.val21*m2.val12 + m1.val22*m2.val22;
      Result.val23 := m1.val20*m2.val03 + m1.val21*m2.val13 + m1.val22*m2.val23;
      Result.val30 := m1.val30*m2.val00 + m1.val31*m2.val10 + m1.val32*m2.val20;
      Result.val31 := m1.val30*m2.val01 + m1.val31*m2.val11 + m1.val32*m2.val21;
      Result.val32 := m1.val30*m2.val02 + m1.val31*m2.val12 + m1.val32*m2.val22;
      Result.val33 := m1.val30*m2.val03 + m1.val31*m2.val13 + m1.val32*m2.val23;
    end;
  
  {$endregion MtrMlt}
  
  {$region MtrTranspose}
  
  function Transpose(self: Mtr2x2f); extensionmethod :=
  new Mtr2x2f(self.val00, self.val10, self.val01, self.val11);
  
  function Transpose(self: Mtr3x3f); extensionmethod :=
  new Mtr3x3f(self.val00, self.val10, self.val20, self.val01, self.val11, self.val21, self.val02, self.val12, self.val22);
  
  function Transpose(self: Mtr4x4f); extensionmethod :=
  new Mtr4x4f(self.val00, self.val10, self.val20, self.val30, self.val01, self.val11, self.val21, self.val31, self.val02, self.val12, self.val22, self.val32, self.val03, self.val13, self.val23, self.val33);
  
  function Transpose(self: Mtr2x3f); extensionmethod :=
  new Mtr3x2f(self.val00, self.val10, self.val01, self.val11, self.val02, self.val12);
  function Transpose(self: Mtr3x2f); extensionmethod :=
  new Mtr2x3f(self.val00, self.val10, self.val20, self.val01, self.val11, self.val21);
  
  function Transpose(self: Mtr2x4f); extensionmethod :=
  new Mtr4x2f(self.val00, self.val10, self.val01, self.val11, self.val02, self.val12, self.val03, self.val13);
  function Transpose(self: Mtr4x2f); extensionmethod :=
  new Mtr2x4f(self.val00, self.val10, self.val20, self.val30, self.val01, self.val11, self.val21, self.val31);
  
  function Transpose(self: Mtr3x4f); extensionmethod :=
  new Mtr4x3f(self.val00, self.val10, self.val20, self.val01, self.val11, self.val21, self.val02, self.val12, self.val22, self.val03, self.val13, self.val23);
  function Transpose(self: Mtr4x3f); extensionmethod :=
  new Mtr3x4f(self.val00, self.val10, self.val20, self.val30, self.val01, self.val11, self.val21, self.val31, self.val02, self.val12, self.val22, self.val32);
  
  function Transpose(self: Mtr2x2d); extensionmethod :=
  new Mtr2x2d(self.val00, self.val10, self.val01, self.val11);
  
  function Transpose(self: Mtr3x3d); extensionmethod :=
  new Mtr3x3d(self.val00, self.val10, self.val20, self.val01, self.val11, self.val21, self.val02, self.val12, self.val22);
  
  function Transpose(self: Mtr4x4d); extensionmethod :=
  new Mtr4x4d(self.val00, self.val10, self.val20, self.val30, self.val01, self.val11, self.val21, self.val31, self.val02, self.val12, self.val22, self.val32, self.val03, self.val13, self.val23, self.val33);
  
  function Transpose(self: Mtr2x3d); extensionmethod :=
  new Mtr3x2d(self.val00, self.val10, self.val01, self.val11, self.val02, self.val12);
  function Transpose(self: Mtr3x2d); extensionmethod :=
  new Mtr2x3d(self.val00, self.val10, self.val20, self.val01, self.val11, self.val21);
  
  function Transpose(self: Mtr2x4d); extensionmethod :=
  new Mtr4x2d(self.val00, self.val10, self.val01, self.val11, self.val02, self.val12, self.val03, self.val13);
  function Transpose(self: Mtr4x2d); extensionmethod :=
  new Mtr2x4d(self.val00, self.val10, self.val20, self.val30, self.val01, self.val11, self.val21, self.val31);
  
  function Transpose(self: Mtr3x4d); extensionmethod :=
  new Mtr4x3d(self.val00, self.val10, self.val20, self.val01, self.val11, self.val21, self.val02, self.val12, self.val22, self.val03, self.val13, self.val23);
  function Transpose(self: Mtr4x3d); extensionmethod :=
  new Mtr3x4d(self.val00, self.val10, self.val20, self.val30, self.val01, self.val11, self.val21, self.val31, self.val02, self.val12, self.val22, self.val32);
  
  {$endregion MtrTranspose}
  
  {$region Misc} type
  
  [StructLayout(LayoutKind.&Explicit)]
  IntFloatUnion = record
    public [FieldOffset(0)] i: integer;
    public [FieldOffset(0)] f: single;
    
    public constructor(i: integer) := self.i := i;
    public constructor(f: single) := self.f := f;
    
  end;
  
  DrawArraysIndirectCommand = record
    public count:         UInt32;
    public instanceCount: UInt32;
    public first:         UInt32;
    public baseInstance:  UInt32;
    
    public constructor(count, instanceCount, first, baseInstance: UInt32);
    begin
      self.count := count;
      self.instanceCount := instanceCount;
      self.first := first;
      self.baseInstance := baseInstance;
    end;
    
  end;
  
  {$endregion Misc}
  
{$endregion Записи}

type
  gl = static class
    
    {$region 2.0 - OpenGL Fundamentals}
    
    {$region 2.3 - Command Execution}
    
    // 2.3.1
    
    public static function GetError: ErrorCode;
    external 'opengl32.dll' name 'glGetError';
    
    // 2.3.3
    
    static procedure Finish;
    external 'opengl32.dll' name 'glFinish';
    
    static procedure Flush;
    external 'opengl32.dll' name 'glFlush';
    
    {$endregion 2.3 - Command Execution}
    
    {$endregion 2.0 - OpenGL Fundamentals}
    
    {$region 4.0 - Event Model}
    
    {$region 4.1 - Sync Objects and Fences}
    
    static function FenceSync(condition: FenceCondition; flags: ReservedFlags): GLsync;
    external 'opengl32.dll' name 'glFenceSync';
    
    static procedure DeleteSync(sync: GLsync);
    external 'opengl32.dll' name 'glDeleteSync';
    
    // 4.1.1
    
    static function ClientWaitSync(sync: GLsync; flags: CommandFlushingBehaviorFlags; timeout: UInt64): ClientWaitSyncResult;
    external 'opengl32.dll' name 'glClientWaitSync';
    
    static procedure WaitSync(sync: GLsync; flags: ReservedFlags; timeout: ReservedTimeoutMode);
    external 'opengl32.dll' name 'glWaitSync';
    
    // 4.1.3
    
    static procedure GetSynciv(sync: GLsync; pname: SyncObjInfoType; bufSize: Int32; var length: Int32; values: pointer);
    external 'opengl32.dll' name 'glGetSynciv';
    static procedure GetSynciv(sync: GLsync; pname: SyncObjInfoType; bufSize: Int32; length: ^Int32; values: pointer);
    external 'opengl32.dll' name 'glGetSynciv';
    
    static function IsSync(sync: GLsync): boolean;
    external 'opengl32.dll' name 'glIsSync';
    
    {$endregion 4.1 - Sync Objects and Fences}
    
    {$region 4.2 - Query Objects and Asynchronous Queries}
    
    // 4.2.2
    
    static procedure GenQueries(n: Int32; [MarshalAs(UnmanagedType.LPArray)] ids: array of QueryName);
    external 'opengl32.dll' name 'glGenQueries';
    static procedure GenQueries(n: Int32; var ids: QueryName);
    external 'opengl32.dll' name 'glGenQueries';
    static procedure GenQueries(n: Int32; ids: pointer);
    external 'opengl32.dll' name 'glGenQueries';
    
    static procedure CreateQueries(target: QueryInfoType; n: Int32; [MarshalAs(UnmanagedType.LPArray)] ids: array of QueryName);
    external 'opengl32.dll' name 'glCreateQueries';
    static procedure CreateQueries(target: QueryInfoType; n: Int32; ids: ^QueryName);
    external 'opengl32.dll' name 'glCreateQueries';
    
    static procedure DeleteQueries(n: Int32; [MarshalAs(UnmanagedType.LPArray)] ids: array of QueryName);
    external 'opengl32.dll' name 'glDeleteQueries';
    static procedure DeleteQueries(n: Int32; ids: ^QueryName);
    external 'opengl32.dll' name 'glDeleteQueries';
    
    static procedure BeginQueryIndexed(target: QueryInfoType; index: UInt32; id: QueryName);
    external 'opengl32.dll' name 'glBeginQueryIndexed';
    static procedure BeginQuery(target: QueryInfoType; id: QueryName);
    external 'opengl32.dll' name 'glBeginQuery';
    
    static procedure EndQueryIndexed(target: QueryInfoType; index: UInt32);
    external 'opengl32.dll' name 'glEndQueryIndexed';
    static procedure EndQuery(target: QueryInfoType);
    external 'opengl32.dll' name 'glEndQuery';
    
    // 4.2.3
    
    static function IsQuery(id: QueryName): boolean;
    external 'opengl32.dll' name 'glIsQuery';
    
    static procedure GetQueryIndexediv(target: QueryInfoType; index: UInt32; pname: GetQueryInfoName; [MarshalAs(UnmanagedType.LPArray)] &params: array of Int32);
    external 'opengl32.dll' name 'glGetQueryIndexediv';
    static procedure GetQueryIndexediv(target: QueryInfoType; index: UInt32; pname: GetQueryInfoName; var &params: Int32);
    external 'opengl32.dll' name 'glGetQueryIndexediv';
    static procedure GetQueryIndexediv(target: QueryInfoType; index: UInt32; pname: GetQueryInfoName; &params: pointer);
    external 'opengl32.dll' name 'glGetQueryIndexediv';
    
    static procedure GetQueryiv(target: QueryInfoType; pname: GetQueryInfoName; [MarshalAs(UnmanagedType.LPArray)] &params: array of Int32);
    external 'opengl32.dll' name 'glGetQueryiv';
    static procedure GetQueryiv(target: QueryInfoType; pname: GetQueryInfoName; var &params: Int32);
    external 'opengl32.dll' name 'glGetQueryiv';
    static procedure GetQueryiv(target: QueryInfoType; pname: GetQueryInfoName; &params: pointer);
    external 'opengl32.dll' name 'glGetQueryiv';
    
    static procedure GetQueryObjectiv(id: QueryName; pname: GetQueryObjectInfoName; [MarshalAs(UnmanagedType.LPArray)] &params: array of Int32);
    external 'opengl32.dll' name 'glGetQueryObjectiv';
    static procedure GetQueryObjectiv(id: QueryName; pname: GetQueryObjectInfoName; var &params: Int32);
    external 'opengl32.dll' name 'glGetQueryObjectiv';
    static procedure GetQueryObjectiv(id: QueryName; pname: GetQueryObjectInfoName; &params: pointer);
    external 'opengl32.dll' name 'glGetQueryObjectiv';
    
    static procedure GetQueryObjectuiv(id: QueryName; pname: GetQueryObjectInfoName; [MarshalAs(UnmanagedType.LPArray)] &params: array of UInt32);
    external 'opengl32.dll' name 'glGetQueryObjectuiv';
    static procedure GetQueryObjectuiv(id: QueryName; pname: GetQueryObjectInfoName; var &params: UInt32);
    external 'opengl32.dll' name 'glGetQueryObjectuiv';
    static procedure GetQueryObjectuiv(id: QueryName; pname: GetQueryObjectInfoName; &params: pointer);
    external 'opengl32.dll' name 'glGetQueryObjectuiv';
    
    static procedure GetQueryObjecti64v(id: QueryName; pname: GetQueryObjectInfoName; [MarshalAs(UnmanagedType.LPArray)] &params: array of Int64);
    external 'opengl32.dll' name 'glGetQueryObjecti64v';
    static procedure GetQueryObjecti64v(id: QueryName; pname: GetQueryObjectInfoName; var &params: Int64);
    external 'opengl32.dll' name 'glGetQueryObjecti64v';
    static procedure GetQueryObjecti64v(id: QueryName; pname: GetQueryObjectInfoName; &params: pointer);
    external 'opengl32.dll' name 'glGetQueryObjecti64v';
    
    static procedure GetQueryObjectui64v(id: QueryName; pname: GetQueryObjectInfoName; [MarshalAs(UnmanagedType.LPArray)] &params: array of UInt64);
    external 'opengl32.dll' name 'glGetQueryObjectui64v';
    static procedure GetQueryObjectui64v(id: QueryName; pname: GetQueryObjectInfoName; var &params: UInt64);
    external 'opengl32.dll' name 'glGetQueryObjectui64v';
    static procedure GetQueryObjectui64v(id: QueryName; pname: GetQueryObjectInfoName; &params: pointer);
    external 'opengl32.dll' name 'glGetQueryObjectui64v';
    
    static procedure GetQueryBufferObjectiv(id: QueryName; buffer: BufferName; pname: GetQueryObjectInfoName; offset: IntPtr);
    external 'opengl32.dll' name 'glGetQueryBufferObjectiv';
    
    static procedure GetQueryBufferObjectuiv(id: QueryName; buffer: BufferName; pname: GetQueryObjectInfoName; offset: IntPtr);
    external 'opengl32.dll' name 'glGetQueryBufferObjectuiv';
    
    static procedure GetQueryBufferObjecti64v(id: QueryName; buffer: BufferName; pname: GetQueryObjectInfoName; offset: IntPtr);
    external 'opengl32.dll' name 'glGetQueryBufferObjecti64v';
    
    static procedure GetQueryBufferObjectui64v(id: QueryName; buffer: BufferName; pname: GetQueryObjectInfoName; offset: IntPtr);
    external 'opengl32.dll' name 'glGetQueryBufferObjectui64v';
    
    {$endregion 4.2 - Query Objects and Asynchronous Queries}
    
    {$region 4.3 - Time Queries}
    
    static procedure QueryCounter(id: QueryName; target: QueryInfoType);
    external 'opengl32.dll' name 'glQueryCounter';
    
    {$endregion 4.3 - Time Queries}
    
    {$endregion 4.0 - Event Model}
    
    {$region 6.0 - Buffer Objects}
    
    static procedure GenBuffers(n: Int32; [MarshalAs(UnmanagedType.LPArray)] buffers: array of BufferName);
    external 'opengl32.dll' name 'glGenBuffers';
    static procedure GenBuffers(n: Int32; var buffers: BufferName);
    external 'opengl32.dll' name 'glGenBuffers';
    static procedure GenBuffers(n: Int32; buffers: pointer);
    external 'opengl32.dll' name 'glGenBuffers';
    
    static procedure CreateBuffers(n: Int32; [MarshalAs(UnmanagedType.LPArray)] buffers: array of BufferName);
    external 'opengl32.dll' name 'glCreateBuffers';
    static procedure CreateBuffers(n: Int32; buffers: ^UInt32);
    external 'opengl32.dll' name 'glCreateBuffers';
    
    static procedure DeleteBuffers(n: Int32; [MarshalAs(UnmanagedType.LPArray)] buffers: array of BufferName);
    external 'opengl32.dll' name 'glDeleteBuffers';
    static procedure DeleteBuffers(n: Int32; buffers: ^BufferName);
    external 'opengl32.dll' name 'glDeleteBuffers';
    
    static function IsBuffer(buffer: BufferName): boolean;
    external 'opengl32.dll' name 'glIsBuffer';
    
    {$region 6.1 - Creating and Binding Buffer Objects}
    
    static procedure BindBuffer(target: BufferBindType; buffer: BufferName);
    external 'opengl32.dll' name 'glBindBuffer';
    
    // 6.1.1
    
    static procedure BindBufferRange(target: BufferBindType; index: UInt32; buffer: BufferName; offset: IntPtr; size: UIntPtr);
    external 'opengl32.dll' name 'glBindBufferRange';
    
    static procedure BindBufferBase(target: BufferBindType; index: UInt32; buffer: BufferName);
    external 'opengl32.dll' name 'glBindBufferBase';
    
    static procedure BindBuffersBase(target: BufferBindType; first: UInt32; count: Int32; [MarshalAs(UnmanagedType.LPArray)] buffers: array of BufferName);
    external 'opengl32.dll' name 'glBindBuffersBase';
    static procedure BindBuffersBase(target: BufferBindType; first: UInt32; count: Int32; buffers: ^BufferName);
    external 'opengl32.dll' name 'glBindBuffersBase';
    
    static procedure BindBuffersRange(target: BufferBindType; first: UInt32; count: Int32; [MarshalAs(UnmanagedType.LPArray)] buffers: array of BufferName; [MarshalAs(UnmanagedType.LPArray)] offsets: array of IntPtr; [MarshalAs(UnmanagedType.LPArray)] sizes: array of UIntPtr);
    external 'opengl32.dll' name 'glBindBuffersRange';
    static procedure BindBuffersRange(target: BufferBindType; first: UInt32; count: Int32; buffers: ^BufferName; offsets: ^IntPtr; sizes: ^UIntPtr);
    external 'opengl32.dll' name 'glBindBuffersRange';
    
    {$endregion 6.1 - Creating and Binding Buffer Objects}
    
    {$region 6.2 - Creating and Modifying Buffer Object Data Stores}
    
    // BufferMapFlags автоматически преобразовывается в BufferStorageFlags
    static procedure BufferStorage(target: BufferBindType; size: UIntPtr; data: IntPtr; flags: BufferStorageFlags);
    external 'opengl32.dll' name 'glBufferStorage';
    static procedure BufferStorage(target: BufferBindType; size: UIntPtr; data: pointer; flags: BufferStorageFlags);
    external 'opengl32.dll' name 'glBufferStorage';
    
    static procedure NamedBufferStorage(buffer: BufferName; size: UIntPtr; data: IntPtr; flags: BufferStorageFlags);
    external 'opengl32.dll' name 'glNamedBufferStorage';
    static procedure NamedBufferStorage(buffer: BufferName; size: UIntPtr; data: pointer; flags: BufferStorageFlags);
    external 'opengl32.dll' name 'glNamedBufferStorage';
    
    static procedure BufferData(target: BufferBindType; size: UIntPtr; data: IntPtr; usage: BufferDataUsage);
    external 'opengl32.dll' name 'glBufferData';
    static procedure BufferData(target: BufferBindType; size: UIntPtr; data: pointer; usage: BufferDataUsage);
    external 'opengl32.dll' name 'glBufferData';
    
    static procedure NamedBufferData(buffer: BufferName; size: UIntPtr; data: IntPtr; usage: UInt32);
    external 'opengl32.dll' name 'glNamedBufferData';
    static procedure NamedBufferData(buffer: BufferName; size: UIntPtr; data: pointer; usage: UInt32);
    external 'opengl32.dll' name 'glNamedBufferData';
    
    static procedure BufferSubData(target: BufferBindType; offset: IntPtr; size: UIntPtr; data: IntPtr);
    external 'opengl32.dll' name 'glBufferSubData';
    static procedure BufferSubData(target: BufferBindType; offset: IntPtr; size: UIntPtr; data: pointer);
    external 'opengl32.dll' name 'glBufferSubData';
    
    static procedure NamedBufferSubData(buffer: BufferName; offset: IntPtr; size: UIntPtr; data: IntPtr);
    external 'opengl32.dll' name 'glNamedBufferSubData';
    static procedure NamedBufferSubData(buffer: BufferName; offset: IntPtr; size: UIntPtr; data: pointer);
    external 'opengl32.dll' name 'glNamedBufferSubData';
    
    static procedure ClearBufferSubData(target: BufferBindType; internalformat: InternalDataFormat; offset: IntPtr; size: UIntPtr; format: DataFormat; &type: DataType; data: IntPtr);
    external 'opengl32.dll' name 'glClearBufferSubData';
    static procedure ClearBufferSubData(target: BufferBindType; internalformat: InternalDataFormat; offset: IntPtr; size: UIntPtr; format: DataFormat; &type: DataType; data: pointer);
    external 'opengl32.dll' name 'glClearBufferSubData';
    
    static procedure ClearNamedBufferSubData(buffer: BufferName; internalformat: InternalDataFormat; offset: IntPtr; size: UIntPtr; format: DataFormat; &type: DataType; data: IntPtr);
    external 'opengl32.dll' name 'glClearNamedBufferSubData';
    static procedure ClearNamedBufferSubData(buffer: BufferName; internalformat: InternalDataFormat; offset: IntPtr; size: UIntPtr; format: DataFormat; &type: DataType; data: pointer);
    external 'opengl32.dll' name 'glClearNamedBufferSubData';
    
    static procedure ClearBufferData(target: BufferBindType; internalformat: InternalDataFormat; format: DataFormat; &type: DataType; data: IntPtr);
    external 'opengl32.dll' name 'glClearBufferData';
    static procedure ClearBufferData(target: BufferBindType; internalformat: InternalDataFormat; format: DataFormat; &type: DataType; data: pointer);
    external 'opengl32.dll' name 'glClearBufferData';
    
    static procedure ClearNamedBufferData(buffer: BufferName; internalformat: InternalDataFormat; format: DataFormat; &type: DataType; data: IntPtr);
    external 'opengl32.dll' name 'glClearNamedBufferData';
    static procedure ClearNamedBufferData(buffer: BufferName; internalformat: InternalDataFormat; format: DataFormat; &type: DataType; data: pointer);
    external 'opengl32.dll' name 'glClearNamedBufferData';
    
    {$endregion 6.2 - Creating and Modifying Buffer Object Data Stores}
    
    {$region 6.3 - Mapping and Unmapping Buffer Data}
    
    static function MapBufferRange(target: BufferBindType; offset: IntPtr; length: UIntPtr; access: BufferMapFlags): IntPtr;
    external 'opengl32.dll' name 'glMapBufferRange';
    static function MapNamedBufferRange(buffer: BufferName; offset: IntPtr; length: UIntPtr; access: BufferMapFlags): IntPtr;
    external 'opengl32.dll' name 'glMapNamedBufferRange';
    
    static function MapBuffer(target: BufferBindType; access: BufferMapFlags): IntPtr;
    external 'opengl32.dll' name 'glMapBuffer';
    static function MapNamedBuffer(buffer: BufferName; access: BufferMapFlags): IntPtr;
    external 'opengl32.dll' name 'glMapNamedBuffer';
    
    static procedure FlushMappedBufferRange(target: BufferBindType; offset: IntPtr; length: UIntPtr);
    external 'opengl32.dll' name 'glFlushMappedBufferRange';
    static procedure FlushMappedNamedBufferRange(buffer: BufferName; offset: IntPtr; length: UIntPtr);
    external 'opengl32.dll' name 'glFlushMappedNamedBufferRange';
    
    // 6.3.1
    
    static function UnmapBuffer(target: BufferBindType): boolean;
    external 'opengl32.dll' name 'glUnmapBuffer';
    static function UnmapNamedBuffer(buffer: BufferName): boolean;
    external 'opengl32.dll' name 'glUnmapNamedBuffer';
    
    {$endregion 6.3 - Mapping and Unmapping Buffer Data}
    
    {$region 6.5 - Invalidating Buffer Data}
    
    static procedure InvalidateBufferSubData(buffer: BufferName; offset: IntPtr; length: UIntPtr);
    external 'opengl32.dll' name 'glInvalidateBufferSubData';
    
    static procedure InvalidateBufferData(buffer: BufferName);
    external 'opengl32.dll' name 'glInvalidateBufferData';
    
    {$endregion 6.5 - Invalidating Buffer Data}
    
    {$region 6.6 - Copying Between Buffers}
    
    static procedure CopyBufferSubData(readTarget, writeTarget: BufferBindType; readOffset, writeOffset: IntPtr; size: UIntPtr);
    external 'opengl32.dll' name 'glCopyBufferSubData';
    static procedure CopyNamedBufferSubData(readBuffer, writeBuffer: BufferName; readOffset, writeOffset: IntPtr; size: UIntPtr);
    external 'opengl32.dll' name 'glCopyNamedBufferSubData';
    
    {$endregion 6.6 - Copying Between Buffers}
    
    {$region 6.7 - Buffer Object Queries}
    
    static procedure GetBufferParameteriv(target: BufferBindType; pname: BufferInfoType; [MarshalAs(UnmanagedType.LPArray)] &params: array of Int32);
    external 'opengl32.dll' name 'glGetBufferParameteriv';
    static procedure GetBufferParameteriv(target: BufferBindType; pname: BufferInfoType; var &params: Int32);
    external 'opengl32.dll' name 'glGetBufferParameteriv';
    static procedure GetBufferParameteriv(target: BufferBindType; pname: BufferInfoType; &params: pointer);
    external 'opengl32.dll' name 'glGetBufferParameteriv';
    
    static procedure GetBufferParameteri64v(target: BufferBindType; pname: BufferInfoType; [MarshalAs(UnmanagedType.LPArray)] &params: array of Int64);
    external 'opengl32.dll' name 'glGetBufferParameteri64v';
    static procedure GetBufferParameteri64v(target: BufferBindType; pname: BufferInfoType; var &params: Int64);
    external 'opengl32.dll' name 'glGetBufferParameteri64v';
    static procedure GetBufferParameteri64v(target: BufferBindType; pname: BufferInfoType; &params: pointer);
    external 'opengl32.dll' name 'glGetBufferParameteri64v';
    
    static procedure GetNamedBufferParameteriv(target: BufferName; pname: BufferInfoType; [MarshalAs(UnmanagedType.LPArray)] &params: array of Int32);
    external 'opengl32.dll' name 'glGetNamedBufferParameteriv';
    static procedure GetNamedBufferParameteriv(target: BufferName; pname: BufferInfoType; var &params: Int32);
    external 'opengl32.dll' name 'glGetNamedBufferParameteriv';
    static procedure GetNamedBufferParameteriv(target: BufferName; pname: BufferInfoType; &params: pointer);
    external 'opengl32.dll' name 'glGetNamedBufferParameteriv';
    
    static procedure GetNamedBufferParameteri64v(target: BufferName; pname: BufferInfoType; [MarshalAs(UnmanagedType.LPArray)] &params: array of Int64);
    external 'opengl32.dll' name 'glGetNamedBufferParameteri64v';
    static procedure GetNamedBufferParameteri64v(target: BufferName; pname: BufferInfoType; var &params: Int64);
    external 'opengl32.dll' name 'glGetNamedBufferParameteri64v';
    static procedure GetNamedBufferParameteri64v(target: BufferName; pname: BufferInfoType; &params: pointer);
    external 'opengl32.dll' name 'glGetNamedBufferParameteri64v';
    
    static procedure GetBufferSubData(target: BufferBindType; offset: IntPtr; size: UIntPtr; data: IntPtr);
    external 'opengl32.dll' name 'glGetBufferSubData';
    static procedure GetBufferSubData(target: BufferBindType; offset: IntPtr; size: UIntPtr; data: pointer);
    external 'opengl32.dll' name 'glGetBufferSubData';
    
    static procedure GetNamedBufferSubData(buffer: BufferName; offset: IntPtr; size: UIntPtr; data: IntPtr);
    external 'opengl32.dll' name 'glGetNamedBufferSubData';
    static procedure GetNamedBufferSubData(buffer: BufferName; offset: IntPtr; size: UIntPtr; data: pointer);
    external 'opengl32.dll' name 'glGetNamedBufferSubData';
    
    static procedure GetBufferPointerv(target: BufferBindType; pname: BufferInfoType; var &params: IntPtr);
    external 'opengl32.dll' name 'glGetBufferPointerv';
    static procedure GetBufferPointerv(target: BufferBindType; pname: BufferInfoType; &params: ^IntPtr);
    external 'opengl32.dll' name 'glGetBufferPointerv';
    static procedure GetBufferPointerv(target: BufferBindType; pname: BufferInfoType; var &params: pointer);
    external 'opengl32.dll' name 'glGetBufferPointerv';
    static procedure GetBufferPointerv(target: BufferBindType; pname: BufferInfoType; &params: ^pointer);
    external 'opengl32.dll' name 'glGetBufferPointerv';
    
    static procedure GetNamedBufferPointerv(buffer: BufferName; pname: BufferInfoType; var &params: IntPtr);
    external 'opengl32.dll' name 'glGetNamedBufferPointerv';
    static procedure GetNamedBufferPointerv(buffer: BufferName; pname: BufferInfoType; &params: ^IntPtr);
    external 'opengl32.dll' name 'glGetNamedBufferPointerv';
    static procedure GetNamedBufferPointerv(buffer: BufferName; pname: BufferInfoType; var &params: pointer);
    external 'opengl32.dll' name 'glGetNamedBufferPointerv';
    static procedure GetNamedBufferPointerv(buffer: BufferName; pname: BufferInfoType; &params: ^pointer);
    external 'opengl32.dll' name 'glGetNamedBufferPointerv';
    
    {$endregion 6.7 - Buffer Object Queries}
    
    {$endregion 6.0 - Buffer Objects}
    
    {$region 7.0 - Programs and Shaders}
    
    {$region 7.1 - Shader Objects}
    
    static function CreateShader(&type: ShaderType): ShaderName;
    external 'opengl32.dll' name 'glCreateShader';
    
    static procedure ShaderSource(shader: ShaderName; count: Int32; [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] strings: array of string; [MarshalAs(UnmanagedType.LPArray)] lengths: array of Int32);
    external 'opengl32.dll' name 'glShaderSource';
    static procedure ShaderSource(shader: ShaderName; count: Int32; strings: ^IntPtr; lengths: ^Int32);
    external 'opengl32.dll' name 'glShaderSource';
    
    static procedure CompileShader(shader: ShaderName);
    external 'opengl32.dll' name 'glCompileShader';
    
    static procedure ReleaseShaderCompiler;
    external 'opengl32.dll' name 'glReleaseShaderCompiler';
    
    static procedure DeleteShader(shader: ShaderName);
    external 'opengl32.dll' name 'glDeleteShader';
    
    static function IsShader(shader: ShaderName): boolean;
    external 'opengl32.dll' name 'glIsShader';
    
    {$endregion 7.1 - Shader Objects}
    
    {$region 7.2 - Shader Binaries}
    
    // для получения binaryformat
    // надо вызвать gl.Get... с параметрами:
    // GLGetQueries.NUM_SHADER_BINARY_FORMATS
    // GLGetQueries.SHADER_BINARY_FORMATS
    static procedure ShaderBinary(count: Int32; [MarshalAs(UnmanagedType.LPArray)] shaders: array of ShaderName; binaryformat: ShaderBinaryFormat; [MarshalAs(UnmanagedType.LPArray)] binary: array of byte; length: Int32);
    external 'opengl32.dll' name 'glShaderBinary';
    static procedure ShaderBinary(count: Int32; [MarshalAs(UnmanagedType.LPArray)] shaders: array of ShaderName; binaryformat: ShaderBinaryFormat; binary: IntPtr; length: Int32);
    external 'opengl32.dll' name 'glShaderBinary';
    static procedure ShaderBinary(count: Int32; shaders: ^ShaderName; binaryformat: ShaderBinaryFormat; [MarshalAs(UnmanagedType.LPArray)] binary: array of byte; length: Int32);
    external 'opengl32.dll' name 'glShaderBinary';
    static procedure ShaderBinary(count: Int32; shaders: ^ShaderName; binaryformat: ShaderBinaryFormat; binary: IntPtr; length: Int32);
    external 'opengl32.dll' name 'glShaderBinary';
    
    // 7.2.1
    
    static procedure SpecializeShader(shader: ShaderName; [MarshalAs(UnmanagedType.LPStr)] pEntryPoint: string; numSpecializationConstants: UInt32; [MarshalAs(UnmanagedType.LPArray)] pConstantIndex: array of UInt32; [MarshalAs(UnmanagedType.LPArray)] pConstantValue: array of IntFloatUnion);
    external 'opengl32.dll' name 'glSpecializeShader';
    static procedure SpecializeShader(shader: ShaderName; [MarshalAs(UnmanagedType.LPStr)] pEntryPoint: string; numSpecializationConstants: UInt32; pConstantIndex: ^UInt32; pConstantValue: ^IntFloatUnion);
    external 'opengl32.dll' name 'glSpecializeShader';
    static procedure SpecializeShader(shader: ShaderName; pEntryPoint: IntPtr; numSpecializationConstants: UInt32; [MarshalAs(UnmanagedType.LPArray)] pConstantIndex: array of UInt32; [MarshalAs(UnmanagedType.LPArray)] pConstantValue: array of IntFloatUnion);
    external 'opengl32.dll' name 'glSpecializeShader';
    static procedure SpecializeShader(shader: ShaderName; pEntryPoint: IntPtr; numSpecializationConstants: UInt32; pConstantIndex: ^UInt32; pConstantValue: ^IntFloatUnion);
    external 'opengl32.dll' name 'glSpecializeShader';
    
    {$endregion 7.2 - Shader Binaries}
    
    {$region 7.3 - Program Objects}
    
    static function CreateProgram: ProgramName;
    external 'opengl32.dll' name 'glCreateProgram';
    
    static procedure AttachShader(&program: ProgramName; shader: ShaderName);
    external 'opengl32.dll' name 'glAttachShader';
    
    static procedure DetachShader(&program: ProgramName; shader: ShaderName);
    external 'opengl32.dll' name 'glDetachShader';
    
    static procedure LinkProgram(&program: ProgramName);
    external 'opengl32.dll' name 'glLinkProgram';
    
    static procedure UseProgram(&program: ProgramName);
    external 'opengl32.dll' name 'glUseProgram';
    
    static procedure ProgramParameteri(&program: ProgramName; pname: ProgramParameterType; value: Int32);
    external 'opengl32.dll' name 'glProgramParameteri';
    
    static procedure DeleteProgram(&program: ProgramName);
    external 'opengl32.dll' name 'glDeleteProgram';
    
    static function IsProgram(&program: ProgramName): boolean;
    external 'opengl32.dll' name 'glIsProgram';
    
    static function CreateShaderProgramv(&type: ShaderType; count: Int32; [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] strings: array of string): ProgramName;
    external 'opengl32.dll' name 'glCreateShaderProgramv';
    static function CreateShaderProgramv(&type: ShaderType; count: Int32; strings: ^IntPtr): ProgramName;
    external 'opengl32.dll' name 'glCreateShaderProgramv';
    
    // 7.3.1
    
    // 7.3.1.1
    
    static procedure GetProgramInterfaceiv(&program: ProgramName; programInterface: ProgramInterfaceType; pname: ProgramInterfaceInfoType; [MarshalAs(UnmanagedType.LPArray)] &params: array of Int32);
    external 'opengl32.dll' name 'glGetProgramInterfaceiv';
    static procedure GetProgramInterfaceiv(&program: ProgramName; programInterface: ProgramInterfaceType; pname: ProgramInterfaceInfoType; var &params: Int32);
    external 'opengl32.dll' name 'glGetProgramInterfaceiv';
    static procedure GetProgramInterfaceiv(&program: ProgramName; programInterface: ProgramInterfaceType; pname: ProgramInterfaceInfoType; &params: pointer);
    external 'opengl32.dll' name 'glGetProgramInterfaceiv';
    
    static function GetProgramResourceIndex(&program: ProgramName; programInterface: ProgramInterfaceType; [MarshalAs(UnmanagedType.LPStr)] name: string): ProgramResourceIndex;
    external 'opengl32.dll' name 'glGetProgramResourceIndex';
    static function GetProgramResourceIndex(&program: ProgramName; programInterface: ProgramInterfaceType; name: IntPtr): ProgramResourceIndex;
    external 'opengl32.dll' name 'glGetProgramResourceIndex';
    
    static procedure GetProgramResourceName(&program: ProgramName; programInterface: ProgramInterfaceType; index: ProgramResourceIndex; bufSize: Int32; var length: Int32; [MarshalAs(UnmanagedType.LPStr)] name: string);
    external 'opengl32.dll' name 'glGetProgramResourceName';
    static procedure GetProgramResourceName(&program: ProgramName; programInterface: ProgramInterfaceType; index: ProgramResourceIndex; bufSize: Int32; var length: Int32; name: IntPtr);
    external 'opengl32.dll' name 'glGetProgramResourceName';
    static procedure GetProgramResourceName(&program: ProgramName; programInterface: ProgramInterfaceType; index: ProgramResourceIndex; bufSize: Int32; length: ^Int32; [MarshalAs(UnmanagedType.LPStr)] name: string);
    external 'opengl32.dll' name 'glGetProgramResourceName';
    static procedure GetProgramResourceName(&program: ProgramName; programInterface: ProgramInterfaceType; index: ProgramResourceIndex; bufSize: Int32; length: ^Int32; name: IntPtr);
    external 'opengl32.dll' name 'glGetProgramResourceName';
    
    // если ProgramInterfaceProperty.Type - вывод через ShadingLanguageTypeToken
    static procedure GetProgramResourceiv(&program: ProgramName; programInterface: ProgramInterfaceType; index: ProgramResourceIndex; propCount: Int32; [MarshalAs(UnmanagedType.LPArray)] props: array of ProgramInterfaceProperty; bufSize: Int32; var length: Int32; [MarshalAs(UnmanagedType.LPArray)] &params: array of Int32);
    external 'opengl32.dll' name 'glGetProgramResourceiv';
    static procedure GetProgramResourceiv(&program: ProgramName; programInterface: ProgramInterfaceType; index: ProgramResourceIndex; propCount: Int32; props: ^ProgramInterfaceProperty; bufSize: Int32; length: ^Int32; var &params: Int32);
    external 'opengl32.dll' name 'glGetProgramResourceiv';
    static procedure GetProgramResourceiv(&program: ProgramName; programInterface: ProgramInterfaceType; index: ProgramResourceIndex; propCount: Int32; props: ^ProgramInterfaceProperty; bufSize: Int32; length: ^Int32; &params: pointer);
    external 'opengl32.dll' name 'glGetProgramResourceiv';
    
    static function GetProgramResourceLocation(&program: ProgramName; programInterface: ProgramInterfaceType; [MarshalAs(UnmanagedType.LPStr)] name: string): Int32;
    external 'opengl32.dll' name 'glGetProgramResourceLocation';
    static function GetProgramResourceLocation(&program: ProgramName; programInterface: ProgramInterfaceType; name: IntPtr): Int32;
    external 'opengl32.dll' name 'glGetProgramResourceLocation';
    
    static function GetProgramResourceLocationIndex(&program: ProgramName; programInterface: ProgramInterfaceType; [MarshalAs(UnmanagedType.LPStr)] name: string): Int32;
    external 'opengl32.dll' name 'glGetProgramResourceLocationIndex';
    static function GetProgramResourceLocationIndex(&program: ProgramName; programInterface: ProgramInterfaceType; name: IntPtr): Int32;
    external 'opengl32.dll' name 'glGetProgramResourceLocationIndex';
    
    {$endregion 7.3 - Program Objects}
    
    {$region 7.4 - Program Pipeline Objects}
    
    static procedure GenProgramPipelines(n: Int32; [MarshalAs(UnmanagedType.LPArray)] pipelines: array of ProgramPipelineName);
    external 'opengl32.dll' name 'glGenProgramPipelines';
    static procedure GenProgramPipelines(n: Int32; var pipelines: ProgramPipelineName);
    external 'opengl32.dll' name 'glGenProgramPipelines';
    static procedure GenProgramPipelines(n: Int32; pipelines: pointer);
    external 'opengl32.dll' name 'glGenProgramPipelines';
    
    static procedure DeleteProgramPipelines(n: Int32; [MarshalAs(UnmanagedType.LPArray)] pipelines: array of ProgramPipelineName);
    external 'opengl32.dll' name 'glDeleteProgramPipelines';
    static procedure DeleteProgramPipelines(n: Int32; var pipelines: ProgramPipelineName);
    external 'opengl32.dll' name 'glDeleteProgramPipelines';
    static procedure DeleteProgramPipelines(n: Int32; pipelines: pointer);
    external 'opengl32.dll' name 'glDeleteProgramPipelines';
    
    static function IsProgramPipeline(pipeline: ProgramPipelineName): boolean;
    external 'opengl32.dll' name 'glIsProgramPipeline';
    
    static procedure BindProgramPipeline(pipeline: ProgramPipelineName);
    external 'opengl32.dll' name 'glBindProgramPipeline';
    
    static procedure CreateProgramPipelines(n: Int32; [MarshalAs(UnmanagedType.LPArray)] pipelines: array of ProgramPipelineName);
    external 'opengl32.dll' name 'glCreateProgramPipelines';
    static procedure CreateProgramPipelines(n: Int32; var pipelines: ProgramPipelineName);
    external 'opengl32.dll' name 'glCreateProgramPipelines';
    static procedure CreateProgramPipelines(n: Int32; pipelines: pointer);
    external 'opengl32.dll' name 'glCreateProgramPipelines';
    
    static procedure UseProgramStages(pipeline: ProgramPipelineName; stages: ProgramStagesFlags; &program: ProgramName);
    external 'opengl32.dll' name 'glUseProgramStages';
    
    static procedure ActiveShaderProgram(pipeline: ProgramPipelineName; &program: ProgramName);
    external 'opengl32.dll' name 'glActiveShaderProgram';
    
    {$endregion 7.4 - Program Pipeline Objects}
    
    {$region 7.5 - Program Binaries}
    
    static procedure GetProgramBinary(&program: ProgramName; bufSize: Int32; var length: Int32; var binaryFormat: ProgramBinaryFormat; [MarshalAs(UnmanagedType.LPArray)] binary: array of byte);
    external 'opengl32.dll' name 'glGetProgramBinary';
    static procedure GetProgramBinary(&program: ProgramName; bufSize: Int32; var length: Int32; var binaryFormat: ProgramBinaryFormat; binary: IntPtr);
    external 'opengl32.dll' name 'glGetProgramBinary';
    static procedure GetProgramBinary(&program: ProgramName; bufSize: Int32; length: ^Int32; binaryFormat: ^ProgramBinaryFormat; [MarshalAs(UnmanagedType.LPArray)] binary: array of byte);
    external 'opengl32.dll' name 'glGetProgramBinary';
    static procedure GetProgramBinary(&program: ProgramName; bufSize: Int32; length: ^Int32; binaryFormat: ^ProgramBinaryFormat; binary: IntPtr);
    external 'opengl32.dll' name 'glGetProgramBinary';
    
    static procedure ProgramBinary(&program: ProgramName; binaryFormat: ProgramBinaryFormat; [MarshalAs(UnmanagedType.LPArray)] binary: array of byte; length: Int32);
    external 'opengl32.dll' name 'glProgramBinary';
    static procedure ProgramBinary(&program: ProgramName; binaryFormat: ProgramBinaryFormat; binary: IntPtr; length: Int32);
    external 'opengl32.dll' name 'glProgramBinary';
    
    {$endregion 7.5 - Program Binaries}
    
    {$region 7.6 - Uniform Variables}
    
    static function GetUniformLocation(&program: ProgramName; [MarshalAs(UnmanagedType.LPStr)] name: string): Int32;
    external 'opengl32.dll' name 'glGetUniformLocation';
    static function GetUniformLocation(&program: ProgramName; name: IntPtr): Int32;
    external 'opengl32.dll' name 'glGetUniformLocation';
    
    static procedure GetActiveUniformName(&program: ProgramName; uniformIndex: UInt32; bufSize: Int32; var length: Int32; [MarshalAs(UnmanagedType.LPStr)] uniformName: string);
    external 'opengl32.dll' name 'glGetActiveUniformName';
    static procedure GetActiveUniformName(&program: ProgramName; uniformIndex: UInt32; bufSize: Int32; var length: Int32; uniformName: IntPtr);
    external 'opengl32.dll' name 'glGetActiveUniformName';
    static procedure GetActiveUniformName(&program: ProgramName; uniformIndex: UInt32; bufSize: Int32; length: ^Int32; [MarshalAs(UnmanagedType.LPStr)] uniformName: string);
    external 'opengl32.dll' name 'glGetActiveUniformName';
    static procedure GetActiveUniformName(&program: ProgramName; uniformIndex: UInt32; bufSize: Int32; length: ^Int32; uniformName: IntPtr);
    external 'opengl32.dll' name 'glGetActiveUniformName';
    
    static procedure GetUniformIndices(&program: ProgramName; uniformCount: Int32; [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] uniformNames: array of string; [MarshalAs(UnmanagedType.LPArray)] uniformIndices: array of UInt32);
    external 'opengl32.dll' name 'glGetUniformIndices';
    static procedure GetUniformIndices(&program: ProgramName; uniformCount: Int32; [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] uniformNames: array of string; uniformIndices: ^UInt32);
    external 'opengl32.dll' name 'glGetUniformIndices';
    static procedure GetUniformIndices(&program: ProgramName; uniformCount: Int32; uniformNames: ^IntPtr; [MarshalAs(UnmanagedType.LPArray)] uniformIndices: array of UInt32);
    external 'opengl32.dll' name 'glGetUniformIndices';
    static procedure GetUniformIndices(&program: ProgramName; uniformCount: Int32; uniformNames: ^IntPtr; uniformIndices: ^UInt32);
    external 'opengl32.dll' name 'glGetUniformIndices';
    
    static procedure GetActiveUniform(&program: ProgramName; index: UInt32; bufSize: Int32; var length: Int32; var size: Int32; &type: ^ShadingLanguageTypeToken; [MarshalAs(UnmanagedType.LPStr)] name: string);
    external 'opengl32.dll' name 'glGetActiveUniform';
    static procedure GetActiveUniform(&program: ProgramName; index: UInt32; bufSize: Int32; var length: Int32; var size: Int32; &type: ^ShadingLanguageTypeToken; name: IntPtr);
    external 'opengl32.dll' name 'glGetActiveUniform';
    static procedure GetActiveUniform(&program: ProgramName; index: UInt32; bufSize: Int32; length: ^Int32; size: ^Int32; &type: ^ShadingLanguageTypeToken; [MarshalAs(UnmanagedType.LPStr)] name: string);
    external 'opengl32.dll' name 'glGetActiveUniform';
    static procedure GetActiveUniform(&program: ProgramName; index: UInt32; bufSize: Int32; length: ^Int32; size: ^Int32; &type: ^ShadingLanguageTypeToken; name: IntPtr);
    external 'opengl32.dll' name 'glGetActiveUniform';
    
    static procedure GetActiveUniformsiv(&program: ProgramName; uniformCount: Int32; [MarshalAs(UnmanagedType.LPArray)] uniformIndices: array of UInt32; pname: ProgramInterfaceProperty; [MarshalAs(UnmanagedType.LPArray)] &params: array of Int32);
    external 'opengl32.dll' name 'glGetActiveUniformsiv';
    static procedure GetActiveUniformsiv(&program: ProgramName; uniformCount: Int32; uniformIndices: ^UInt32; pname: ProgramInterfaceProperty; &params: ^Int32);
    external 'opengl32.dll' name 'glGetActiveUniformsiv';
    
    static function GetUniformBlockIndex(&program: ProgramName; [MarshalAs(UnmanagedType.LPStr)] uniformBlockName: string): UInt32;
    external 'opengl32.dll' name 'glGetUniformBlockIndex';
    static function GetUniformBlockIndex(&program: ProgramName; uniformBlockName: IntPtr): UInt32;
    external 'opengl32.dll' name 'glGetUniformBlockIndex';
    
    static procedure GetActiveUniformBlockName(&program: ProgramName; uniformBlockIndex: UInt32; bufSize: Int32; var length: Int32; [MarshalAs(UnmanagedType.LPStr)] uniformBlockName: string);
    external 'opengl32.dll' name 'glGetActiveUniformBlockName';
    static procedure GetActiveUniformBlockName(&program: ProgramName; uniformBlockIndex: UInt32; bufSize: Int32; length: ^Int32; uniformBlockName: IntPtr);
    external 'opengl32.dll' name 'glGetActiveUniformBlockName';
    
    static procedure GetActiveUniformBlockiv(&program: ProgramName; uniformBlockIndex: UInt32; pname: ProgramInterfaceProperty; [MarshalAs(UnmanagedType.LPArray)] &params: array of Int32);
    external 'opengl32.dll' name 'glGetActiveUniformBlockiv';
    static procedure GetActiveUniformBlockiv(&program: ProgramName; uniformBlockIndex: UInt32; pname: ProgramInterfaceProperty; var &params: Int32);
    external 'opengl32.dll' name 'glGetActiveUniformBlockiv';
    static procedure GetActiveUniformBlockiv(&program: ProgramName; uniformBlockIndex: UInt32; pname: ProgramInterfaceProperty; &params: ^Int32);
    external 'opengl32.dll' name 'glGetActiveUniformBlockiv';
    
    static procedure GetActiveAtomicCounterBufferiv(&program: ProgramName; bufferIndex: UInt32; pname: ProgramInterfaceProperty; [MarshalAs(UnmanagedType.LPArray)] &params: array of Int32);
    external 'opengl32.dll' name 'glGetActiveAtomicCounterBufferiv';
    static procedure GetActiveAtomicCounterBufferiv(&program: ProgramName; bufferIndex: UInt32; pname: ProgramInterfaceProperty; var &params: Int32);
    external 'opengl32.dll' name 'glGetActiveAtomicCounterBufferiv';
    static procedure GetActiveAtomicCounterBufferiv(&program: ProgramName; bufferIndex: UInt32; pname: ProgramInterfaceProperty; &params: ^Int32);
    external 'opengl32.dll' name 'glGetActiveAtomicCounterBufferiv';
    
    // 7.6.1
    
    {$region Uniform[1,2,3,4][i,f,d,ui]}
    
    static procedure Uniform1i(location: Int32; v0: Int32);
    external 'opengl32.dll' name 'glUniform1i';
    
    static procedure Uniform2i(location: Int32; v0: Int32; v1: Int32);
    external 'opengl32.dll' name 'glUniform2i';
    
    static procedure Uniform3i(location: Int32; v0: Int32; v1: Int32; v2: Int32);
    external 'opengl32.dll' name 'glUniform3i';
    
    static procedure Uniform4i(location: Int32; v0: Int32; v1: Int32; v2: Int32; v3: Int32);
    external 'opengl32.dll' name 'glUniform4i';
    
    static procedure Uniform1f(location: Int32; v0: single);
    external 'opengl32.dll' name 'glUniform1f';
    
    static procedure Uniform2f(location: Int32; v0: single; v1: single);
    external 'opengl32.dll' name 'glUniform2f';
    
    static procedure Uniform3f(location: Int32; v0: single; v1: single; v2: single);
    external 'opengl32.dll' name 'glUniform3f';
    
    static procedure Uniform4f(location: Int32; v0: single; v1: single; v2: single; v3: single);
    external 'opengl32.dll' name 'glUniform4f';
    
    static procedure Uniform1d(location: Int32; x: real);
    external 'opengl32.dll' name 'glUniform1d';
    
    static procedure Uniform2d(location: Int32; x: real; y: real);
    external 'opengl32.dll' name 'glUniform2d';
    
    static procedure Uniform3d(location: Int32; x: real; y: real; z: real);
    external 'opengl32.dll' name 'glUniform3d';
    
    static procedure Uniform4d(location: Int32; x: real; y: real; z: real; w: real);
    external 'opengl32.dll' name 'glUniform4d';
    
    static procedure Uniform1ui(location: Int32; v0: UInt32);
    external 'opengl32.dll' name 'glUniform1ui';
    
    static procedure Uniform2ui(location: Int32; v0: UInt32; v1: UInt32);
    external 'opengl32.dll' name 'glUniform2ui';
    
    static procedure Uniform3ui(location: Int32; v0: UInt32; v1: UInt32; v2: UInt32);
    external 'opengl32.dll' name 'glUniform3ui';
    
    static procedure Uniform4ui(location: Int32; v0: UInt32; v1: UInt32; v2: UInt32; v3: UInt32);
    external 'opengl32.dll' name 'glUniform4ui';
    
    {$endregion Uniform[1,2,3,4][i,f,d,ui]}
    
    {$region Uniform[1,2,3,4][i,f,d,ui]v}
    
    static procedure Uniform1iv(location: Int32; count: Int32; [MarshalAs(UnmanagedType.LPArray)] value: array of Int32);
    external 'opengl32.dll' name 'glUniform1iv';
    static procedure Uniform1iv(location: Int32; count: Int32; var value: Int32);
    external 'opengl32.dll' name 'glUniform1iv';
    static procedure Uniform1iv(location: Int32; count: Int32; var value: Vec1i);
    external 'opengl32.dll' name 'glUniform1iv';
    static procedure Uniform1iv(location: Int32; count: Int32; value: ^Int32);
    external 'opengl32.dll' name 'glUniform1iv';
    
    static procedure Uniform2iv(location: Int32; count: Int32; [MarshalAs(UnmanagedType.LPArray)] value: array of Int32);
    external 'opengl32.dll' name 'glUniform2iv';
    static procedure Uniform2iv(location: Int32; count: Int32; var value: Int32);
    external 'opengl32.dll' name 'glUniform2iv';
    static procedure Uniform2iv(location: Int32; count: Int32; var value: Vec2i);
    external 'opengl32.dll' name 'glUniform2iv';
    static procedure Uniform2iv(location: Int32; count: Int32; value: ^Int32);
    external 'opengl32.dll' name 'glUniform2iv';
    
    static procedure Uniform3iv(location: Int32; count: Int32; [MarshalAs(UnmanagedType.LPArray)] value: array of Int32);
    external 'opengl32.dll' name 'glUniform3iv';
    static procedure Uniform3iv(location: Int32; count: Int32; var value: Int32);
    external 'opengl32.dll' name 'glUniform3iv';
    static procedure Uniform3iv(location: Int32; count: Int32; var value: Vec3i);
    external 'opengl32.dll' name 'glUniform3iv';
    static procedure Uniform3iv(location: Int32; count: Int32; value: ^Int32);
    external 'opengl32.dll' name 'glUniform3iv';
    
    static procedure Uniform4iv(location: Int32; count: Int32; [MarshalAs(UnmanagedType.LPArray)] value: array of Int32);
    external 'opengl32.dll' name 'glUniform4iv';
    static procedure Uniform4iv(location: Int32; count: Int32; var value: Int32);
    external 'opengl32.dll' name 'glUniform4iv';
    static procedure Uniform4iv(location: Int32; count: Int32; var value: Vec4i);
    external 'opengl32.dll' name 'glUniform4iv';
    static procedure Uniform4iv(location: Int32; count: Int32; value: ^Int32);
    external 'opengl32.dll' name 'glUniform4iv';
    
    static procedure Uniform1fv(location: Int32; count: Int32; [MarshalAs(UnmanagedType.LPArray)] value: array of single);
    external 'opengl32.dll' name 'glUniform1fv';
    static procedure Uniform1fv(location: Int32; count: Int32; var value: single);
    external 'opengl32.dll' name 'glUniform1fv';
    static procedure Uniform1fv(location: Int32; count: Int32; var value: Vec1f);
    external 'opengl32.dll' name 'glUniform1fv';
    static procedure Uniform1fv(location: Int32; count: Int32; value: ^single);
    external 'opengl32.dll' name 'glUniform1fv';
    
    static procedure Uniform2fv(location: Int32; count: Int32; [MarshalAs(UnmanagedType.LPArray)] value: array of single);
    external 'opengl32.dll' name 'glUniform2fv';
    static procedure Uniform2fv(location: Int32; count: Int32; var value: single);
    external 'opengl32.dll' name 'glUniform2fv';
    static procedure Uniform2fv(location: Int32; count: Int32; var value: Vec2f);
    external 'opengl32.dll' name 'glUniform2fv';
    static procedure Uniform2fv(location: Int32; count: Int32; value: ^single);
    external 'opengl32.dll' name 'glUniform2fv';
    
    static procedure Uniform3fv(location: Int32; count: Int32; [MarshalAs(UnmanagedType.LPArray)] value: array of single);
    external 'opengl32.dll' name 'glUniform3fv';
    static procedure Uniform3fv(location: Int32; count: Int32; var value: single);
    external 'opengl32.dll' name 'glUniform3fv';
    static procedure Uniform3fv(location: Int32; count: Int32; var value: Vec3f);
    external 'opengl32.dll' name 'glUniform3fv';
    static procedure Uniform3fv(location: Int32; count: Int32; value: ^single);
    external 'opengl32.dll' name 'glUniform3fv';
    
    static procedure Uniform4fv(location: Int32; count: Int32; [MarshalAs(UnmanagedType.LPArray)] value: array of single);
    external 'opengl32.dll' name 'glUniform4fv';
    static procedure Uniform4fv(location: Int32; count: Int32; var value: single);
    external 'opengl32.dll' name 'glUniform4fv';
    static procedure Uniform4fv(location: Int32; count: Int32; var value: Vec4f);
    external 'opengl32.dll' name 'glUniform4fv';
    static procedure Uniform4fv(location: Int32; count: Int32; value: ^single);
    external 'opengl32.dll' name 'glUniform4fv';
    
    static procedure Uniform1dv(location: Int32; count: Int32; [MarshalAs(UnmanagedType.LPArray)] value: array of real);
    external 'opengl32.dll' name 'glUniform1dv';
    static procedure Uniform1dv(location: Int32; count: Int32; var value: real);
    external 'opengl32.dll' name 'glUniform1dv';
    static procedure Uniform1dv(location: Int32; count: Int32; var value: Vec1d);
    external 'opengl32.dll' name 'glUniform1dv';
    static procedure Uniform1dv(location: Int32; count: Int32; value: ^real);
    external 'opengl32.dll' name 'glUniform1dv';
    
    static procedure Uniform2dv(location: Int32; count: Int32; [MarshalAs(UnmanagedType.LPArray)] value: array of real);
    external 'opengl32.dll' name 'glUniform2dv';
    static procedure Uniform2dv(location: Int32; count: Int32; var value: real);
    external 'opengl32.dll' name 'glUniform2dv';
    static procedure Uniform2dv(location: Int32; count: Int32; var value: Vec2d);
    external 'opengl32.dll' name 'glUniform2dv';
    static procedure Uniform2dv(location: Int32; count: Int32; value: ^real);
    external 'opengl32.dll' name 'glUniform2dv';
    
    static procedure Uniform3dv(location: Int32; count: Int32; [MarshalAs(UnmanagedType.LPArray)] value: array of real);
    external 'opengl32.dll' name 'glUniform3dv';
    static procedure Uniform3dv(location: Int32; count: Int32; var value: real);
    external 'opengl32.dll' name 'glUniform3dv';
    static procedure Uniform3dv(location: Int32; count: Int32; var value: Vec3d);
    external 'opengl32.dll' name 'glUniform3dv';
    static procedure Uniform3dv(location: Int32; count: Int32; value: ^real);
    external 'opengl32.dll' name 'glUniform3dv';
    
    static procedure Uniform4dv(location: Int32; count: Int32; [MarshalAs(UnmanagedType.LPArray)] value: array of real);
    external 'opengl32.dll' name 'glUniform4dv';
    static procedure Uniform4dv(location: Int32; count: Int32; var value: real);
    external 'opengl32.dll' name 'glUniform4dv';
    static procedure Uniform4dv(location: Int32; count: Int32; var value: Vec4d);
    external 'opengl32.dll' name 'glUniform4dv';
    static procedure Uniform4dv(location: Int32; count: Int32; value: ^real);
    external 'opengl32.dll' name 'glUniform4dv';
    
    static procedure Uniform1uiv(location: Int32; count: Int32; [MarshalAs(UnmanagedType.LPArray)] value: array of UInt32);
    external 'opengl32.dll' name 'glUniform1uiv';
    static procedure Uniform1uiv(location: Int32; count: Int32; var value: UInt32);
    external 'opengl32.dll' name 'glUniform1uiv';
    static procedure Uniform1uiv(location: Int32; count: Int32; var value: Vec1ui);
    external 'opengl32.dll' name 'glUniform1uiv';
    static procedure Uniform1uiv(location: Int32; count: Int32; value: ^UInt32);
    external 'opengl32.dll' name 'glUniform1uiv';
    
    static procedure Uniform2uiv(location: Int32; count: Int32; [MarshalAs(UnmanagedType.LPArray)] value: array of UInt32);
    external 'opengl32.dll' name 'glUniform2uiv';
    static procedure Uniform2uiv(location: Int32; count: Int32; var value: UInt32);
    external 'opengl32.dll' name 'glUniform2uiv';
    static procedure Uniform2uiv(location: Int32; count: Int32; var value: Vec2ui);
    external 'opengl32.dll' name 'glUniform2uiv';
    static procedure Uniform2uiv(location: Int32; count: Int32; value: ^UInt32);
    external 'opengl32.dll' name 'glUniform2uiv';
    
    static procedure Uniform3uiv(location: Int32; count: Int32; [MarshalAs(UnmanagedType.LPArray)] value: array of UInt32);
    external 'opengl32.dll' name 'glUniform3uiv';
    static procedure Uniform3uiv(location: Int32; count: Int32; var value: UInt32);
    external 'opengl32.dll' name 'glUniform3uiv';
    static procedure Uniform3uiv(location: Int32; count: Int32; var value: Vec3ui);
    external 'opengl32.dll' name 'glUniform3uiv';
    static procedure Uniform3uiv(location: Int32; count: Int32; value: ^UInt32);
    external 'opengl32.dll' name 'glUniform3uiv';
    
    static procedure Uniform4uiv(location: Int32; count: Int32; [MarshalAs(UnmanagedType.LPArray)] value: array of UInt32);
    external 'opengl32.dll' name 'glUniform4uiv';
    static procedure Uniform4uiv(location: Int32; count: Int32; var value: UInt32);
    external 'opengl32.dll' name 'glUniform4uiv';
    static procedure Uniform4uiv(location: Int32; count: Int32; var value: Vec4ui);
    external 'opengl32.dll' name 'glUniform4uiv';
    static procedure Uniform4uiv(location: Int32; count: Int32; value: ^UInt32);
    external 'opengl32.dll' name 'glUniform4uiv';
    
    {$endregion Uniform[1,2,3,4][i,f,d,ui]v}
    
    {$region UniformMatrix[2,3,4][f,d]v}
    
    static procedure UniformMatrix2fv(location: Int32; count: Int32; transpose: boolean; [MarshalAs(UnmanagedType.LPArray)] value: array of single);
    external 'opengl32.dll' name 'glUniformMatrix2fv';
    static procedure UniformMatrix2fv(location: Int32; count: Int32; transpose: boolean; var value: single);
    external 'opengl32.dll' name 'glUniformMatrix2fv';
    static procedure UniformMatrix2fv(location: Int32; count: Int32; transpose: boolean; var value: Mtr2f);
    external 'opengl32.dll' name 'glUniformMatrix2fv';
    static procedure UniformMatrix2fv(location: Int32; count: Int32; transpose: boolean; value: ^single);
    external 'opengl32.dll' name 'glUniformMatrix2fv';
    
    static procedure UniformMatrix3fv(location: Int32; count: Int32; transpose: boolean; [MarshalAs(UnmanagedType.LPArray)] value: array of single);
    external 'opengl32.dll' name 'glUniformMatrix3fv';
    static procedure UniformMatrix3fv(location: Int32; count: Int32; transpose: boolean; var value: single);
    external 'opengl32.dll' name 'glUniformMatrix3fv';
    static procedure UniformMatrix3fv(location: Int32; count: Int32; transpose: boolean; var value: Mtr3f);
    external 'opengl32.dll' name 'glUniformMatrix3fv';
    static procedure UniformMatrix3fv(location: Int32; count: Int32; transpose: boolean; value: ^single);
    external 'opengl32.dll' name 'glUniformMatrix3fv';
    
    static procedure UniformMatrix4fv(location: Int32; count: Int32; transpose: boolean; [MarshalAs(UnmanagedType.LPArray)] value: array of single);
    external 'opengl32.dll' name 'glUniformMatrix4fv';
    static procedure UniformMatrix4fv(location: Int32; count: Int32; transpose: boolean; var value: single);
    external 'opengl32.dll' name 'glUniformMatrix4fv';
    static procedure UniformMatrix4fv(location: Int32; count: Int32; transpose: boolean; var value: Mtr4f);
    external 'opengl32.dll' name 'glUniformMatrix4fv';
    static procedure UniformMatrix4fv(location: Int32; count: Int32; transpose: boolean; value: ^single);
    external 'opengl32.dll' name 'glUniformMatrix4fv';
    
    static procedure UniformMatrix2dv(location: Int32; count: Int32; transpose: boolean; [MarshalAs(UnmanagedType.LPArray)] value: array of real);
    external 'opengl32.dll' name 'glUniformMatrix2dv';
    static procedure UniformMatrix2dv(location: Int32; count: Int32; transpose: boolean; var value: real);
    external 'opengl32.dll' name 'glUniformMatrix2dv';
    static procedure UniformMatrix2dv(location: Int32; count: Int32; transpose: boolean; var value: Mtr2d);
    external 'opengl32.dll' name 'glUniformMatrix2dv';
    static procedure UniformMatrix2dv(location: Int32; count: Int32; transpose: boolean; value: ^real);
    external 'opengl32.dll' name 'glUniformMatrix2dv';
    
    static procedure UniformMatrix3dv(location: Int32; count: Int32; transpose: boolean; [MarshalAs(UnmanagedType.LPArray)] value: array of real);
    external 'opengl32.dll' name 'glUniformMatrix3dv';
    static procedure UniformMatrix3dv(location: Int32; count: Int32; transpose: boolean; var value: real);
    external 'opengl32.dll' name 'glUniformMatrix3dv';
    static procedure UniformMatrix3dv(location: Int32; count: Int32; transpose: boolean; var value: Mtr3d);
    external 'opengl32.dll' name 'glUniformMatrix3dv';
    static procedure UniformMatrix3dv(location: Int32; count: Int32; transpose: boolean; value: ^real);
    external 'opengl32.dll' name 'glUniformMatrix3dv';
    
    static procedure UniformMatrix4dv(location: Int32; count: Int32; transpose: boolean; [MarshalAs(UnmanagedType.LPArray)] value: array of real);
    external 'opengl32.dll' name 'glUniformMatrix4dv';
    static procedure UniformMatrix4dv(location: Int32; count: Int32; transpose: boolean; var value: real);
    external 'opengl32.dll' name 'glUniformMatrix4dv';
    static procedure UniformMatrix4dv(location: Int32; count: Int32; transpose: boolean; var value: Mtr4d);
    external 'opengl32.dll' name 'glUniformMatrix4dv';
    static procedure UniformMatrix4dv(location: Int32; count: Int32; transpose: boolean; value: ^real);
    external 'opengl32.dll' name 'glUniformMatrix4dv';
    
    {$endregion UniformMatrix[2,3,4][f,d]v}
    
    {$region UniformMatrix[2x3,3x2,2x4,4x2,3x4,4x3][f,d]v}
    
    static procedure UniformMatrix2x3fv(location: Int32; count: Int32; transpose: boolean; [MarshalAs(UnmanagedType.LPArray)] value: array of single);
    external 'opengl32.dll' name 'glUniformMatrix2x3fv';
    static procedure UniformMatrix2x3fv(location: Int32; count: Int32; transpose: boolean; var value: single);
    external 'opengl32.dll' name 'glUniformMatrix2x3fv';
    static procedure UniformMatrix2x3fv(location: Int32; count: Int32; transpose: boolean; var value: Mtr2x3f);
    external 'opengl32.dll' name 'glUniformMatrix2x3fv';
    static procedure UniformMatrix2x3fv(location: Int32; count: Int32; transpose: boolean; value: ^single);
    external 'opengl32.dll' name 'glUniformMatrix2x3fv';
    
    static procedure UniformMatrix3x2fv(location: Int32; count: Int32; transpose: boolean; [MarshalAs(UnmanagedType.LPArray)] value: array of single);
    external 'opengl32.dll' name 'glUniformMatrix3x2fv';
    static procedure UniformMatrix3x2fv(location: Int32; count: Int32; transpose: boolean; var value: single);
    external 'opengl32.dll' name 'glUniformMatrix3x2fv';
    static procedure UniformMatrix3x2fv(location: Int32; count: Int32; transpose: boolean; var value: Mtr3x2f);
    external 'opengl32.dll' name 'glUniformMatrix3x2fv';
    static procedure UniformMatrix3x2fv(location: Int32; count: Int32; transpose: boolean; value: ^single);
    external 'opengl32.dll' name 'glUniformMatrix3x2fv';
    
    static procedure UniformMatrix2x4fv(location: Int32; count: Int32; transpose: boolean; [MarshalAs(UnmanagedType.LPArray)] value: array of single);
    external 'opengl32.dll' name 'glUniformMatrix2x4fv';
    static procedure UniformMatrix2x4fv(location: Int32; count: Int32; transpose: boolean; var value: single);
    external 'opengl32.dll' name 'glUniformMatrix2x4fv';
    static procedure UniformMatrix2x4fv(location: Int32; count: Int32; transpose: boolean; var value: Mtr2x4f);
    external 'opengl32.dll' name 'glUniformMatrix2x4fv';
    static procedure UniformMatrix2x4fv(location: Int32; count: Int32; transpose: boolean; value: ^single);
    external 'opengl32.dll' name 'glUniformMatrix2x4fv';
    
    static procedure UniformMatrix4x2fv(location: Int32; count: Int32; transpose: boolean; [MarshalAs(UnmanagedType.LPArray)] value: array of single);
    external 'opengl32.dll' name 'glUniformMatrix4x2fv';
    static procedure UniformMatrix4x2fv(location: Int32; count: Int32; transpose: boolean; var value: single);
    external 'opengl32.dll' name 'glUniformMatrix4x2fv';
    static procedure UniformMatrix4x2fv(location: Int32; count: Int32; transpose: boolean; var value: Mtr4x2f);
    external 'opengl32.dll' name 'glUniformMatrix4x2fv';
    static procedure UniformMatrix4x2fv(location: Int32; count: Int32; transpose: boolean; value: ^single);
    external 'opengl32.dll' name 'glUniformMatrix4x2fv';
    
    static procedure UniformMatrix3x4fv(location: Int32; count: Int32; transpose: boolean; [MarshalAs(UnmanagedType.LPArray)] value: array of single);
    external 'opengl32.dll' name 'glUniformMatrix3x4fv';
    static procedure UniformMatrix3x4fv(location: Int32; count: Int32; transpose: boolean; var value: single);
    external 'opengl32.dll' name 'glUniformMatrix3x4fv';
    static procedure UniformMatrix3x4fv(location: Int32; count: Int32; transpose: boolean; var value: Mtr3x4f);
    external 'opengl32.dll' name 'glUniformMatrix3x4fv';
    static procedure UniformMatrix3x4fv(location: Int32; count: Int32; transpose: boolean; value: ^single);
    external 'opengl32.dll' name 'glUniformMatrix3x4fv';
    
    static procedure UniformMatrix4x3fv(location: Int32; count: Int32; transpose: boolean; [MarshalAs(UnmanagedType.LPArray)] value: array of single);
    external 'opengl32.dll' name 'glUniformMatrix4x3fv';
    static procedure UniformMatrix4x3fv(location: Int32; count: Int32; transpose: boolean; var value: single);
    external 'opengl32.dll' name 'glUniformMatrix4x3fv';
    static procedure UniformMatrix4x3fv(location: Int32; count: Int32; transpose: boolean; var value: Mtr4x3f);
    external 'opengl32.dll' name 'glUniformMatrix4x3fv';
    static procedure UniformMatrix4x3fv(location: Int32; count: Int32; transpose: boolean; value: ^single);
    external 'opengl32.dll' name 'glUniformMatrix4x3fv';
    
    static procedure UniformMatrix2x3dv(location: Int32; count: Int32; transpose: boolean; [MarshalAs(UnmanagedType.LPArray)] value: array of real);
    external 'opengl32.dll' name 'glUniformMatrix2x3dv';
    static procedure UniformMatrix2x3dv(location: Int32; count: Int32; transpose: boolean; var value: real);
    external 'opengl32.dll' name 'glUniformMatrix2x3dv';
    static procedure UniformMatrix2x3dv(location: Int32; count: Int32; transpose: boolean; var value: Mtr2x3d);
    external 'opengl32.dll' name 'glUniformMatrix2x3dv';
    static procedure UniformMatrix2x3dv(location: Int32; count: Int32; transpose: boolean; value: ^real);
    external 'opengl32.dll' name 'glUniformMatrix2x3dv';
    
    static procedure UniformMatrix3x2dv(location: Int32; count: Int32; transpose: boolean; [MarshalAs(UnmanagedType.LPArray)] value: array of real);
    external 'opengl32.dll' name 'glUniformMatrix3x2dv';
    static procedure UniformMatrix3x2dv(location: Int32; count: Int32; transpose: boolean; var value: real);
    external 'opengl32.dll' name 'glUniformMatrix3x2dv';
    static procedure UniformMatrix3x2dv(location: Int32; count: Int32; transpose: boolean; var value: Mtr3x2d);
    external 'opengl32.dll' name 'glUniformMatrix3x2dv';
    static procedure UniformMatrix3x2dv(location: Int32; count: Int32; transpose: boolean; value: ^real);
    external 'opengl32.dll' name 'glUniformMatrix3x2dv';
    
    static procedure UniformMatrix2x4dv(location: Int32; count: Int32; transpose: boolean; [MarshalAs(UnmanagedType.LPArray)] value: array of real);
    external 'opengl32.dll' name 'glUniformMatrix2x4dv';
    static procedure UniformMatrix2x4dv(location: Int32; count: Int32; transpose: boolean; var value: real);
    external 'opengl32.dll' name 'glUniformMatrix2x4dv';
    static procedure UniformMatrix2x4dv(location: Int32; count: Int32; transpose: boolean; var value: Mtr2x4d);
    external 'opengl32.dll' name 'glUniformMatrix2x4dv';
    static procedure UniformMatrix2x4dv(location: Int32; count: Int32; transpose: boolean; value: ^real);
    external 'opengl32.dll' name 'glUniformMatrix2x4dv';
    
    static procedure UniformMatrix4x2dv(location: Int32; count: Int32; transpose: boolean; [MarshalAs(UnmanagedType.LPArray)] value: array of real);
    external 'opengl32.dll' name 'glUniformMatrix4x2dv';
    static procedure UniformMatrix4x2dv(location: Int32; count: Int32; transpose: boolean; var value: real);
    external 'opengl32.dll' name 'glUniformMatrix4x2dv';
    static procedure UniformMatrix4x2dv(location: Int32; count: Int32; transpose: boolean; var value: Mtr4x2d);
    external 'opengl32.dll' name 'glUniformMatrix4x2dv';
    static procedure UniformMatrix4x2dv(location: Int32; count: Int32; transpose: boolean; value: ^real);
    external 'opengl32.dll' name 'glUniformMatrix4x2dv';
    
    static procedure UniformMatrix3x4dv(location: Int32; count: Int32; transpose: boolean; [MarshalAs(UnmanagedType.LPArray)] value: array of real);
    external 'opengl32.dll' name 'glUniformMatrix3x4dv';
    static procedure UniformMatrix3x4dv(location: Int32; count: Int32; transpose: boolean; var value: real);
    external 'opengl32.dll' name 'glUniformMatrix3x4dv';
    static procedure UniformMatrix3x4dv(location: Int32; count: Int32; transpose: boolean; var value: Mtr3x4d);
    external 'opengl32.dll' name 'glUniformMatrix3x4dv';
    static procedure UniformMatrix3x4dv(location: Int32; count: Int32; transpose: boolean; value: ^real);
    external 'opengl32.dll' name 'glUniformMatrix3x4dv';
    
    static procedure UniformMatrix4x3dv(location: Int32; count: Int32; transpose: boolean; [MarshalAs(UnmanagedType.LPArray)] value: array of real);
    external 'opengl32.dll' name 'glUniformMatrix4x3dv';
    static procedure UniformMatrix4x3dv(location: Int32; count: Int32; transpose: boolean; var value: real);
    external 'opengl32.dll' name 'glUniformMatrix4x3dv';
    static procedure UniformMatrix4x3dv(location: Int32; count: Int32; transpose: boolean; var value: Mtr4x3d);
    external 'opengl32.dll' name 'glUniformMatrix4x3dv';
    static procedure UniformMatrix4x3dv(location: Int32; count: Int32; transpose: boolean; value: ^real);
    external 'opengl32.dll' name 'glUniformMatrix4x3dv';
    
    {$endregion UniformMatrix[2x3,3x2,2x4,4x2,3x4,4x3][f,d]v}
    
    {$region ProgramUniform[1,2,3,4][i,f,d,ui]}
    
    static procedure ProgramUniform1i(&program: ProgramName; location: Int32; v0: Int32);
    external 'opengl32.dll' name 'glProgramUniform1i';
    
    static procedure ProgramUniform2i(&program: ProgramName; location: Int32; v0: Int32; v1: Int32);
    external 'opengl32.dll' name 'glProgramUniform2i';
    
    static procedure ProgramUniform3i(&program: ProgramName; location: Int32; v0: Int32; v1: Int32; v2: Int32);
    external 'opengl32.dll' name 'glProgramUniform3i';
    
    static procedure ProgramUniform4i(&program: ProgramName; location: Int32; v0: Int32; v1: Int32; v2: Int32; v3: Int32);
    external 'opengl32.dll' name 'glProgramUniform4i';
    
    static procedure ProgramUniform1f(&program: ProgramName; location: Int32; v0: single);
    external 'opengl32.dll' name 'glProgramUniform1f';
    
    static procedure ProgramUniform2f(&program: ProgramName; location: Int32; v0: single; v1: single);
    external 'opengl32.dll' name 'glProgramUniform2f';
    
    static procedure ProgramUniform3f(&program: ProgramName; location: Int32; v0: single; v1: single; v2: single);
    external 'opengl32.dll' name 'glProgramUniform3f';
    
    static procedure ProgramUniform4f(&program: ProgramName; location: Int32; v0: single; v1: single; v2: single; v3: single);
    external 'opengl32.dll' name 'glProgramUniform4f';
    
    static procedure ProgramUniform1d(&program: ProgramName; location: Int32; x: real);
    external 'opengl32.dll' name 'glProgramUniform1d';
    
    static procedure ProgramUniform2d(&program: ProgramName; location: Int32; x: real; y: real);
    external 'opengl32.dll' name 'glProgramUniform2d';
    
    static procedure ProgramUniform3d(&program: ProgramName; location: Int32; x: real; y: real; z: real);
    external 'opengl32.dll' name 'glProgramUniform3d';
    
    static procedure ProgramUniform4d(&program: ProgramName; location: Int32; x: real; y: real; z: real; w: real);
    external 'opengl32.dll' name 'glProgramUniform4d';
    
    static procedure ProgramUniform1ui(&program: ProgramName; location: Int32; v0: UInt32);
    external 'opengl32.dll' name 'glProgramUniform1ui';
    
    static procedure ProgramUniform2ui(&program: ProgramName; location: Int32; v0: UInt32; v1: UInt32);
    external 'opengl32.dll' name 'glProgramUniform2ui';
    
    static procedure ProgramUniform3ui(&program: ProgramName; location: Int32; v0: UInt32; v1: UInt32; v2: UInt32);
    external 'opengl32.dll' name 'glProgramUniform3ui';
    
    static procedure ProgramUniform4ui(&program: ProgramName; location: Int32; v0: UInt32; v1: UInt32; v2: UInt32; v3: UInt32);
    external 'opengl32.dll' name 'glProgramUniform4ui';
    
    {$endregion ProgramUniform[1,2,3,4][i,f,d,ui]}
    
    {$region ProgramUniform[1,2,3,4][i,f,d,ui]v}
    
    static procedure ProgramUniform1iv(&program: ProgramName; location: Int32; count: Int32; [MarshalAs(UnmanagedType.LPArray)] value: array of Int32);
    external 'opengl32.dll' name 'glProgramUniform1iv';
    static procedure ProgramUniform1iv(&program: ProgramName; location: Int32; count: Int32; var value: Int32);
    external 'opengl32.dll' name 'glProgramUniform1iv';
    static procedure ProgramUniform1iv(&program: ProgramName; location: Int32; count: Int32; var value: Vec1i);
    external 'opengl32.dll' name 'glProgramUniform1iv';
    static procedure ProgramUniform1iv(&program: ProgramName; location: Int32; count: Int32; value: ^Int32);
    external 'opengl32.dll' name 'glProgramUniform1iv';
    
    static procedure ProgramUniform2iv(&program: ProgramName; location: Int32; count: Int32; [MarshalAs(UnmanagedType.LPArray)] value: array of Int32);
    external 'opengl32.dll' name 'glProgramUniform2iv';
    static procedure ProgramUniform2iv(&program: ProgramName; location: Int32; count: Int32; var value: Int32);
    external 'opengl32.dll' name 'glProgramUniform2iv';
    static procedure ProgramUniform2iv(&program: ProgramName; location: Int32; count: Int32; var value: Vec2i);
    external 'opengl32.dll' name 'glProgramUniform2iv';
    static procedure ProgramUniform2iv(&program: ProgramName; location: Int32; count: Int32; value: ^Int32);
    external 'opengl32.dll' name 'glProgramUniform2iv';
    
    static procedure ProgramUniform3iv(&program: ProgramName; location: Int32; count: Int32; [MarshalAs(UnmanagedType.LPArray)] value: array of Int32);
    external 'opengl32.dll' name 'glProgramUniform3iv';
    static procedure ProgramUniform3iv(&program: ProgramName; location: Int32; count: Int32; var value: Int32);
    external 'opengl32.dll' name 'glProgramUniform3iv';
    static procedure ProgramUniform3iv(&program: ProgramName; location: Int32; count: Int32; var value: Vec3i);
    external 'opengl32.dll' name 'glProgramUniform3iv';
    static procedure ProgramUniform3iv(&program: ProgramName; location: Int32; count: Int32; value: ^Int32);
    external 'opengl32.dll' name 'glProgramUniform3iv';
    
    static procedure ProgramUniform4iv(&program: ProgramName; location: Int32; count: Int32; [MarshalAs(UnmanagedType.LPArray)] value: array of Int32);
    external 'opengl32.dll' name 'glProgramUniform4iv';
    static procedure ProgramUniform4iv(&program: ProgramName; location: Int32; count: Int32; var value: Int32);
    external 'opengl32.dll' name 'glProgramUniform4iv';
    static procedure ProgramUniform4iv(&program: ProgramName; location: Int32; count: Int32; var value: Vec4i);
    external 'opengl32.dll' name 'glProgramUniform4iv';
    static procedure ProgramUniform4iv(&program: ProgramName; location: Int32; count: Int32; value: ^Int32);
    external 'opengl32.dll' name 'glProgramUniform4iv';
    
    static procedure ProgramUniform1fv(&program: ProgramName; location: Int32; count: Int32; [MarshalAs(UnmanagedType.LPArray)] value: array of single);
    external 'opengl32.dll' name 'glProgramUniform1fv';
    static procedure ProgramUniform1fv(&program: ProgramName; location: Int32; count: Int32; var value: single);
    external 'opengl32.dll' name 'glProgramUniform1fv';
    static procedure ProgramUniform1fv(&program: ProgramName; location: Int32; count: Int32; var value: Vec1f);
    external 'opengl32.dll' name 'glProgramUniform1fv';
    static procedure ProgramUniform1fv(&program: ProgramName; location: Int32; count: Int32; value: ^single);
    external 'opengl32.dll' name 'glProgramUniform1fv';
    
    static procedure ProgramUniform2fv(&program: ProgramName; location: Int32; count: Int32; [MarshalAs(UnmanagedType.LPArray)] value: array of single);
    external 'opengl32.dll' name 'glProgramUniform2fv';
    static procedure ProgramUniform2fv(&program: ProgramName; location: Int32; count: Int32; var value: single);
    external 'opengl32.dll' name 'glProgramUniform2fv';
    static procedure ProgramUniform2fv(&program: ProgramName; location: Int32; count: Int32; var value: Vec2f);
    external 'opengl32.dll' name 'glProgramUniform2fv';
    static procedure ProgramUniform2fv(&program: ProgramName; location: Int32; count: Int32; value: ^single);
    external 'opengl32.dll' name 'glProgramUniform2fv';
    
    static procedure ProgramUniform3fv(&program: ProgramName; location: Int32; count: Int32; [MarshalAs(UnmanagedType.LPArray)] value: array of single);
    external 'opengl32.dll' name 'glProgramUniform3fv';
    static procedure ProgramUniform3fv(&program: ProgramName; location: Int32; count: Int32; var value: single);
    external 'opengl32.dll' name 'glProgramUniform3fv';
    static procedure ProgramUniform3fv(&program: ProgramName; location: Int32; count: Int32; var value: Vec3f);
    external 'opengl32.dll' name 'glProgramUniform3fv';
    static procedure ProgramUniform3fv(&program: ProgramName; location: Int32; count: Int32; value: ^single);
    external 'opengl32.dll' name 'glProgramUniform3fv';
    
    static procedure ProgramUniform4fv(&program: ProgramName; location: Int32; count: Int32; [MarshalAs(UnmanagedType.LPArray)] value: array of single);
    external 'opengl32.dll' name 'glProgramUniform4fv';
    static procedure ProgramUniform4fv(&program: ProgramName; location: Int32; count: Int32; var value: single);
    external 'opengl32.dll' name 'glProgramUniform4fv';
    static procedure ProgramUniform4fv(&program: ProgramName; location: Int32; count: Int32; var value: Vec4f);
    external 'opengl32.dll' name 'glProgramUniform4fv';
    static procedure ProgramUniform4fv(&program: ProgramName; location: Int32; count: Int32; value: ^single);
    external 'opengl32.dll' name 'glProgramUniform4fv';
    
    static procedure ProgramUniform1dv(&program: ProgramName; location: Int32; count: Int32; [MarshalAs(UnmanagedType.LPArray)] value: array of real);
    external 'opengl32.dll' name 'glProgramUniform1dv';
    static procedure ProgramUniform1dv(&program: ProgramName; location: Int32; count: Int32; var value: real);
    external 'opengl32.dll' name 'glProgramUniform1dv';
    static procedure ProgramUniform1dv(&program: ProgramName; location: Int32; count: Int32; var value: Vec1d);
    external 'opengl32.dll' name 'glProgramUniform1dv';
    static procedure ProgramUniform1dv(&program: ProgramName; location: Int32; count: Int32; value: ^real);
    external 'opengl32.dll' name 'glProgramUniform1dv';
    
    static procedure ProgramUniform2dv(&program: ProgramName; location: Int32; count: Int32; [MarshalAs(UnmanagedType.LPArray)] value: array of real);
    external 'opengl32.dll' name 'glProgramUniform2dv';
    static procedure ProgramUniform2dv(&program: ProgramName; location: Int32; count: Int32; var value: real);
    external 'opengl32.dll' name 'glProgramUniform2dv';
    static procedure ProgramUniform2dv(&program: ProgramName; location: Int32; count: Int32; var value: Vec2d);
    external 'opengl32.dll' name 'glProgramUniform2dv';
    static procedure ProgramUniform2dv(&program: ProgramName; location: Int32; count: Int32; value: ^real);
    external 'opengl32.dll' name 'glProgramUniform2dv';
    
    static procedure ProgramUniform3dv(&program: ProgramName; location: Int32; count: Int32; [MarshalAs(UnmanagedType.LPArray)] value: array of real);
    external 'opengl32.dll' name 'glProgramUniform3dv';
    static procedure ProgramUniform3dv(&program: ProgramName; location: Int32; count: Int32; var value: real);
    external 'opengl32.dll' name 'glProgramUniform3dv';
    static procedure ProgramUniform3dv(&program: ProgramName; location: Int32; count: Int32; var value: Vec3d);
    external 'opengl32.dll' name 'glProgramUniform3dv';
    static procedure ProgramUniform3dv(&program: ProgramName; location: Int32; count: Int32; value: ^real);
    external 'opengl32.dll' name 'glProgramUniform3dv';
    
    static procedure ProgramUniform4dv(&program: ProgramName; location: Int32; count: Int32; [MarshalAs(UnmanagedType.LPArray)] value: array of real);
    external 'opengl32.dll' name 'glProgramUniform4dv';
    static procedure ProgramUniform4dv(&program: ProgramName; location: Int32; count: Int32; var value: real);
    external 'opengl32.dll' name 'glProgramUniform4dv';
    static procedure ProgramUniform4dv(&program: ProgramName; location: Int32; count: Int32; var value: Vec4d);
    external 'opengl32.dll' name 'glProgramUniform4dv';
    static procedure ProgramUniform4dv(&program: ProgramName; location: Int32; count: Int32; value: ^real);
    external 'opengl32.dll' name 'glProgramUniform4dv';
    
    static procedure ProgramUniform1uiv(&program: ProgramName; location: Int32; count: Int32; [MarshalAs(UnmanagedType.LPArray)] value: array of UInt32);
    external 'opengl32.dll' name 'glProgramUniform1uiv';
    static procedure ProgramUniform1uiv(&program: ProgramName; location: Int32; count: Int32; var value: UInt32);
    external 'opengl32.dll' name 'glProgramUniform1uiv';
    static procedure ProgramUniform1uiv(&program: ProgramName; location: Int32; count: Int32; var value: Vec1ui);
    external 'opengl32.dll' name 'glProgramUniform1uiv';
    static procedure ProgramUniform1uiv(&program: ProgramName; location: Int32; count: Int32; value: ^UInt32);
    external 'opengl32.dll' name 'glProgramUniform1uiv';
    
    static procedure ProgramUniform2uiv(&program: ProgramName; location: Int32; count: Int32; [MarshalAs(UnmanagedType.LPArray)] value: array of UInt32);
    external 'opengl32.dll' name 'glProgramUniform2uiv';
    static procedure ProgramUniform2uiv(&program: ProgramName; location: Int32; count: Int32; var value: UInt32);
    external 'opengl32.dll' name 'glProgramUniform2uiv';
    static procedure ProgramUniform2uiv(&program: ProgramName; location: Int32; count: Int32; var value: Vec2ui);
    external 'opengl32.dll' name 'glProgramUniform2uiv';
    static procedure ProgramUniform2uiv(&program: ProgramName; location: Int32; count: Int32; value: ^UInt32);
    external 'opengl32.dll' name 'glProgramUniform2uiv';
    
    static procedure ProgramUniform3uiv(&program: ProgramName; location: Int32; count: Int32; [MarshalAs(UnmanagedType.LPArray)] value: array of UInt32);
    external 'opengl32.dll' name 'glProgramUniform3uiv';
    static procedure ProgramUniform3uiv(&program: ProgramName; location: Int32; count: Int32; var value: UInt32);
    external 'opengl32.dll' name 'glProgramUniform3uiv';
    static procedure ProgramUniform3uiv(&program: ProgramName; location: Int32; count: Int32; var value: Vec3ui);
    external 'opengl32.dll' name 'glProgramUniform3uiv';
    static procedure ProgramUniform3uiv(&program: ProgramName; location: Int32; count: Int32; value: ^UInt32);
    external 'opengl32.dll' name 'glProgramUniform3uiv';
    
    static procedure ProgramUniform4uiv(&program: ProgramName; location: Int32; count: Int32; [MarshalAs(UnmanagedType.LPArray)] value: array of UInt32);
    external 'opengl32.dll' name 'glProgramUniform4uiv';
    static procedure ProgramUniform4uiv(&program: ProgramName; location: Int32; count: Int32; var value: UInt32);
    external 'opengl32.dll' name 'glProgramUniform4uiv';
    static procedure ProgramUniform4uiv(&program: ProgramName; location: Int32; count: Int32; var value: Vec4ui);
    external 'opengl32.dll' name 'glProgramUniform4uiv';
    static procedure ProgramUniform4uiv(&program: ProgramName; location: Int32; count: Int32; value: ^UInt32);
    external 'opengl32.dll' name 'glProgramUniform4uiv';
    
    {$endregion ProgramUniform[1,2,3,4][i,f,d,ui]v}
    
    {$region ProgramUniformMatrix[2,3,4][f,d]v}
    
    static procedure ProgramUniformMatrix2fv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; [MarshalAs(UnmanagedType.LPArray)] value: array of single);
    external 'opengl32.dll' name 'glProgramUniformMatrix2fv';
    static procedure ProgramUniformMatrix2fv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; var value: single);
    external 'opengl32.dll' name 'glProgramUniformMatrix2fv';
    static procedure ProgramUniformMatrix2fv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; var value: Mtr2f);
    external 'opengl32.dll' name 'glProgramUniformMatrix2fv';
    static procedure ProgramUniformMatrix2fv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; value: ^single);
    external 'opengl32.dll' name 'glProgramUniformMatrix2fv';
    
    static procedure ProgramUniformMatrix3fv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; [MarshalAs(UnmanagedType.LPArray)] value: array of single);
    external 'opengl32.dll' name 'glProgramUniformMatrix3fv';
    static procedure ProgramUniformMatrix3fv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; var value: single);
    external 'opengl32.dll' name 'glProgramUniformMatrix3fv';
    static procedure ProgramUniformMatrix3fv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; var value: Mtr3f);
    external 'opengl32.dll' name 'glProgramUniformMatrix3fv';
    static procedure ProgramUniformMatrix3fv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; value: ^single);
    external 'opengl32.dll' name 'glProgramUniformMatrix3fv';
    
    static procedure ProgramUniformMatrix4fv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; [MarshalAs(UnmanagedType.LPArray)] value: array of single);
    external 'opengl32.dll' name 'glProgramUniformMatrix4fv';
    static procedure ProgramUniformMatrix4fv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; var value: single);
    external 'opengl32.dll' name 'glProgramUniformMatrix4fv';
    static procedure ProgramUniformMatrix4fv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; var value: Mtr4f);
    external 'opengl32.dll' name 'glProgramUniformMatrix4fv';
    static procedure ProgramUniformMatrix4fv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; value: ^single);
    external 'opengl32.dll' name 'glProgramUniformMatrix4fv';
    
    static procedure ProgramUniformMatrix2dv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; [MarshalAs(UnmanagedType.LPArray)] value: array of real);
    external 'opengl32.dll' name 'glProgramUniformMatrix2dv';
    static procedure ProgramUniformMatrix2dv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; var value: real);
    external 'opengl32.dll' name 'glProgramUniformMatrix2dv';
    static procedure ProgramUniformMatrix2dv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; var value: Mtr2d);
    external 'opengl32.dll' name 'glProgramUniformMatrix2dv';
    static procedure ProgramUniformMatrix2dv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; value: ^real);
    external 'opengl32.dll' name 'glProgramUniformMatrix2dv';
    
    static procedure ProgramUniformMatrix3dv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; [MarshalAs(UnmanagedType.LPArray)] value: array of real);
    external 'opengl32.dll' name 'glProgramUniformMatrix3dv';
    static procedure ProgramUniformMatrix3dv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; var value: real);
    external 'opengl32.dll' name 'glProgramUniformMatrix3dv';
    static procedure ProgramUniformMatrix3dv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; var value: Mtr3d);
    external 'opengl32.dll' name 'glProgramUniformMatrix3dv';
    static procedure ProgramUniformMatrix3dv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; value: ^real);
    external 'opengl32.dll' name 'glProgramUniformMatrix3dv';
    
    static procedure ProgramUniformMatrix4dv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; [MarshalAs(UnmanagedType.LPArray)] value: array of real);
    external 'opengl32.dll' name 'glProgramUniformMatrix4dv';
    static procedure ProgramUniformMatrix4dv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; var value: real);
    external 'opengl32.dll' name 'glProgramUniformMatrix4dv';
    static procedure ProgramUniformMatrix4dv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; var value: Mtr4d);
    external 'opengl32.dll' name 'glProgramUniformMatrix4dv';
    static procedure ProgramUniformMatrix4dv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; value: ^real);
    external 'opengl32.dll' name 'glProgramUniformMatrix4dv';
    
    {$endregion ProgramUniformMatrix[2,3,4][f,d]v}
    
    {$region ProgramUniformMatrix[2x3,3x2,2x4,4x2,3x4,4x3][f,d]v}
    
    static procedure ProgramUniformMatrix2x3fv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; [MarshalAs(UnmanagedType.LPArray)] value: array of single);
    external 'opengl32.dll' name 'glProgramUniformMatrix2x3fv';
    static procedure ProgramUniformMatrix2x3fv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; var value: single);
    external 'opengl32.dll' name 'glProgramUniformMatrix2x3fv';
    static procedure ProgramUniformMatrix2x3fv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; var value: Mtr2x3f);
    external 'opengl32.dll' name 'glProgramUniformMatrix2x3fv';
    static procedure ProgramUniformMatrix2x3fv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; value: ^single);
    external 'opengl32.dll' name 'glProgramUniformMatrix2x3fv';
    
    static procedure ProgramUniformMatrix3x2fv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; [MarshalAs(UnmanagedType.LPArray)] value: array of single);
    external 'opengl32.dll' name 'glProgramUniformMatrix3x2fv';
    static procedure ProgramUniformMatrix3x2fv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; var value: single);
    external 'opengl32.dll' name 'glProgramUniformMatrix3x2fv';
    static procedure ProgramUniformMatrix3x2fv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; var value: Mtr3x2f);
    external 'opengl32.dll' name 'glProgramUniformMatrix3x2fv';
    static procedure ProgramUniformMatrix3x2fv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; value: ^single);
    external 'opengl32.dll' name 'glProgramUniformMatrix3x2fv';
    
    static procedure ProgramUniformMatrix2x4fv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; [MarshalAs(UnmanagedType.LPArray)] value: array of single);
    external 'opengl32.dll' name 'glProgramUniformMatrix2x4fv';
    static procedure ProgramUniformMatrix2x4fv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; var value: single);
    external 'opengl32.dll' name 'glProgramUniformMatrix2x4fv';
    static procedure ProgramUniformMatrix2x4fv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; var value: Mtr2x4f);
    external 'opengl32.dll' name 'glProgramUniformMatrix2x4fv';
    static procedure ProgramUniformMatrix2x4fv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; value: ^single);
    external 'opengl32.dll' name 'glProgramUniformMatrix2x4fv';
    
    static procedure ProgramUniformMatrix4x2fv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; [MarshalAs(UnmanagedType.LPArray)] value: array of single);
    external 'opengl32.dll' name 'glProgramUniformMatrix4x2fv';
    static procedure ProgramUniformMatrix4x2fv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; var value: single);
    external 'opengl32.dll' name 'glProgramUniformMatrix4x2fv';
    static procedure ProgramUniformMatrix4x2fv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; var value: Mtr4x2f);
    external 'opengl32.dll' name 'glProgramUniformMatrix4x2fv';
    static procedure ProgramUniformMatrix4x2fv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; value: ^single);
    external 'opengl32.dll' name 'glProgramUniformMatrix4x2fv';
    
    static procedure ProgramUniformMatrix3x4fv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; [MarshalAs(UnmanagedType.LPArray)] value: array of single);
    external 'opengl32.dll' name 'glProgramUniformMatrix3x4fv';
    static procedure ProgramUniformMatrix3x4fv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; var value: single);
    external 'opengl32.dll' name 'glProgramUniformMatrix3x4fv';
    static procedure ProgramUniformMatrix3x4fv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; var value: Mtr3x4f);
    external 'opengl32.dll' name 'glProgramUniformMatrix3x4fv';
    static procedure ProgramUniformMatrix3x4fv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; value: ^single);
    external 'opengl32.dll' name 'glProgramUniformMatrix3x4fv';
    
    static procedure ProgramUniformMatrix4x3fv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; [MarshalAs(UnmanagedType.LPArray)] value: array of single);
    external 'opengl32.dll' name 'glProgramUniformMatrix4x3fv';
    static procedure ProgramUniformMatrix4x3fv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; var value: single);
    external 'opengl32.dll' name 'glProgramUniformMatrix4x3fv';
    static procedure ProgramUniformMatrix4x3fv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; var value: Mtr4x3f);
    external 'opengl32.dll' name 'glProgramUniformMatrix4x3fv';
    static procedure ProgramUniformMatrix4x3fv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; value: ^single);
    external 'opengl32.dll' name 'glProgramUniformMatrix4x3fv';
    
    static procedure ProgramUniformMatrix2x3dv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; [MarshalAs(UnmanagedType.LPArray)] value: array of real);
    external 'opengl32.dll' name 'glProgramUniformMatrix2x3dv';
    static procedure ProgramUniformMatrix2x3dv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; var value: real);
    external 'opengl32.dll' name 'glProgramUniformMatrix2x3dv';
    static procedure ProgramUniformMatrix2x3dv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; var value: Mtr2x3d);
    external 'opengl32.dll' name 'glProgramUniformMatrix2x3dv';
    static procedure ProgramUniformMatrix2x3dv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; value: ^real);
    external 'opengl32.dll' name 'glProgramUniformMatrix2x3dv';
    
    static procedure ProgramUniformMatrix3x2dv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; [MarshalAs(UnmanagedType.LPArray)] value: array of real);
    external 'opengl32.dll' name 'glProgramUniformMatrix3x2dv';
    static procedure ProgramUniformMatrix3x2dv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; var value: real);
    external 'opengl32.dll' name 'glProgramUniformMatrix3x2dv';
    static procedure ProgramUniformMatrix3x2dv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; var value: Mtr3x2d);
    external 'opengl32.dll' name 'glProgramUniformMatrix3x2dv';
    static procedure ProgramUniformMatrix3x2dv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; value: ^real);
    external 'opengl32.dll' name 'glProgramUniformMatrix3x2dv';
    
    static procedure ProgramUniformMatrix2x4dv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; [MarshalAs(UnmanagedType.LPArray)] value: array of real);
    external 'opengl32.dll' name 'glProgramUniformMatrix2x4dv';
    static procedure ProgramUniformMatrix2x4dv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; var value: real);
    external 'opengl32.dll' name 'glProgramUniformMatrix2x4dv';
    static procedure ProgramUniformMatrix2x4dv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; var value: Mtr2x4d);
    external 'opengl32.dll' name 'glProgramUniformMatrix2x4dv';
    static procedure ProgramUniformMatrix2x4dv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; value: ^real);
    external 'opengl32.dll' name 'glProgramUniformMatrix2x4dv';
    
    static procedure ProgramUniformMatrix4x2dv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; [MarshalAs(UnmanagedType.LPArray)] value: array of real);
    external 'opengl32.dll' name 'glProgramUniformMatrix4x2dv';
    static procedure ProgramUniformMatrix4x2dv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; var value: real);
    external 'opengl32.dll' name 'glProgramUniformMatrix4x2dv';
    static procedure ProgramUniformMatrix4x2dv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; var value: Mtr4x2d);
    external 'opengl32.dll' name 'glProgramUniformMatrix4x2dv';
    static procedure ProgramUniformMatrix4x2dv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; value: ^real);
    external 'opengl32.dll' name 'glProgramUniformMatrix4x2dv';
    
    static procedure ProgramUniformMatrix3x4dv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; [MarshalAs(UnmanagedType.LPArray)] value: array of real);
    external 'opengl32.dll' name 'glProgramUniformMatrix3x4dv';
    static procedure ProgramUniformMatrix3x4dv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; var value: real);
    external 'opengl32.dll' name 'glProgramUniformMatrix3x4dv';
    static procedure ProgramUniformMatrix3x4dv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; var value: Mtr3x4d);
    external 'opengl32.dll' name 'glProgramUniformMatrix3x4dv';
    static procedure ProgramUniformMatrix3x4dv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; value: ^real);
    external 'opengl32.dll' name 'glProgramUniformMatrix3x4dv';
    
    static procedure ProgramUniformMatrix4x3dv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; [MarshalAs(UnmanagedType.LPArray)] value: array of real);
    external 'opengl32.dll' name 'glProgramUniformMatrix4x3dv';
    static procedure ProgramUniformMatrix4x3dv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; var value: real);
    external 'opengl32.dll' name 'glProgramUniformMatrix4x3dv';
    static procedure ProgramUniformMatrix4x3dv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; var value: Mtr4x3d);
    external 'opengl32.dll' name 'glProgramUniformMatrix4x3dv';
    static procedure ProgramUniformMatrix4x3dv(&program: ProgramName; location: Int32; count: Int32; transpose: boolean; value: ^real);
    external 'opengl32.dll' name 'glProgramUniformMatrix4x3dv';
    
    {$endregion ProgramUniformMatrix[2x3,3x2,2x4,4x2,3x4,4x3][f,d]v}
    
    // 7.6.3
    
    static procedure UniformBlockBinding(&program: ProgramName; uniformBlockIndex: UInt32; uniformBlockBinding: UInt32);
    external 'opengl32.dll' name 'glUniformBlockBinding';
    
    {$endregion 7.6 - Uniform Variables}
    
    {$region 7.8 - Shader Buffer Variables and Shader Storage Blocks}
    
    static procedure ShaderStorageBlockBinding(&program: ProgramName; storageBlockIndex: UInt32; storageBlockBinding: UInt32);
    external 'opengl32.dll' name 'glShaderStorageBlockBinding';
    
    {$endregion 7.8 - Shader Buffer Variables and Shader Storage Blocks}
    
    {$region 7.10 - Subroutine Uniform Variables}
    
    static function GetSubroutineIndex(&program: ProgramName; __shadertype: ShaderType; [MarshalAs(UnmanagedType.LPStr)] name: string): UInt32;
    external 'opengl32.dll' name 'glGetSubroutineIndex';
    static function GetSubroutineIndex(&program: ProgramName; __shadertype: ShaderType; name: IntPtr): UInt32;
    external 'opengl32.dll' name 'glGetSubroutineIndex';
    
    static procedure GetActiveSubroutineName(&program: ProgramName; __shadertype: ShaderType; index: UInt32; bufsize: Int32; var length: Int32; [MarshalAs(UnmanagedType.LPStr)] name: string);
    external 'opengl32.dll' name 'glGetActiveSubroutineName';
    static procedure GetActiveSubroutineName(&program: ProgramName; __shadertype: ShaderType; index: UInt32; bufsize: Int32; var length: Int32; name: IntPtr);
    external 'opengl32.dll' name 'glGetActiveSubroutineName';
    static procedure GetActiveSubroutineName(&program: ProgramName; __shadertype: ShaderType; index: UInt32; bufsize: Int32; length: ^Int32; [MarshalAs(UnmanagedType.LPStr)] name: string);
    external 'opengl32.dll' name 'glGetActiveSubroutineName';
    static procedure GetActiveSubroutineName(&program: ProgramName; __shadertype: ShaderType; index: UInt32; bufsize: Int32; length: ^Int32; name: IntPtr);
    external 'opengl32.dll' name 'glGetActiveSubroutineName';
    
    static function GetSubroutineUniformLocation(&program: ProgramName; __shadertype: ShaderType; [MarshalAs(UnmanagedType.LPStr)] name: string): Int32;
    external 'opengl32.dll' name 'glGetSubroutineUniformLocation';
    static function GetSubroutineUniformLocation(&program: ProgramName; __shadertype: ShaderType; name: IntPtr): Int32;
    external 'opengl32.dll' name 'glGetSubroutineUniformLocation';
    
    static procedure GetActiveSubroutineUniformName(&program: ProgramName; __shadertype: ShaderType; index: UInt32; bufsize: Int32; var length: Int32; [MarshalAs(UnmanagedType.LPStr)] name: string);
    external 'opengl32.dll' name 'glGetActiveSubroutineUniformName';
    static procedure GetActiveSubroutineUniformName(&program: ProgramName; __shadertype: ShaderType; index: UInt32; bufsize: Int32; var length: Int32; name: IntPtr);
    external 'opengl32.dll' name 'glGetActiveSubroutineUniformName';
    static procedure GetActiveSubroutineUniformName(&program: ProgramName; __shadertype: ShaderType; index: UInt32; bufsize: Int32; length: ^Int32; [MarshalAs(UnmanagedType.LPStr)] name: string);
    external 'opengl32.dll' name 'glGetActiveSubroutineUniformName';
    static procedure GetActiveSubroutineUniformName(&program: ProgramName; __shadertype: ShaderType; index: UInt32; bufsize: Int32; length: ^Int32; name: IntPtr);
    external 'opengl32.dll' name 'glGetActiveSubroutineUniformName';
    
    static procedure GetActiveSubroutineUniformiv(&program: ProgramName; __shadertype: ShaderType; index: UInt32; pname: ProgramInterfaceProperty; [MarshalAs(UnmanagedType.LPArray)] values: array of Int32);
    external 'opengl32.dll' name 'glGetActiveSubroutineUniformiv';
    static procedure GetActiveSubroutineUniformiv(&program: ProgramName; __shadertype: ShaderType; index: UInt32; pname: ProgramInterfaceProperty; var values: Int32);
    external 'opengl32.dll' name 'glGetActiveSubroutineUniformiv';
    static procedure GetActiveSubroutineUniformiv(&program: ProgramName; __shadertype: ShaderType; index: UInt32; pname: ProgramInterfaceProperty; values: pointer);
    external 'opengl32.dll' name 'glGetActiveSubroutineUniformiv';
    
    static procedure UniformSubroutinesuiv(__shadertype: ShaderType; count: Int32; [MarshalAs(UnmanagedType.LPArray)] indices: array of UInt32);
    external 'opengl32.dll' name 'glUniformSubroutinesuiv';
    static procedure UniformSubroutinesuiv(__shadertype: ShaderType; count: Int32; indices: ^UInt32);
    external 'opengl32.dll' name 'glUniformSubroutinesuiv';
    
    {$endregion 7.10 - Subroutine Uniform Variables}
    
    {$region 7.13 - Shader Memory Access}
    
    //7.13.2
    
    static procedure MemoryBarrier(barriers: MemoryBarrierTypeFlags);
    external 'opengl32.dll' name 'glMemoryBarrier';
    
    static procedure MemoryBarrierByRegion(barriers: MemoryBarrierTypeFlags);
    external 'opengl32.dll' name 'glMemoryBarrierByRegion';
    
    {$endregion 7.13 - Shader Memory Access}
    
    {$region 7.14 - Shader, Program, and Program Pipeline Queries}
    
    static procedure GetShaderiv(shader: ShaderName; pname: ShaderInfoType; [MarshalAs(UnmanagedType.LPArray)] &params: array of Int32);
    external 'opengl32.dll' name 'glGetShaderiv';
    static procedure GetShaderiv(shader: ShaderName; pname: ShaderInfoType; var &params: Int32);
    external 'opengl32.dll' name 'glGetShaderiv';
    static procedure GetShaderiv(shader: ShaderName; pname: ShaderInfoType; &params: pointer);
    external 'opengl32.dll' name 'glGetShaderiv';
    
    static procedure GetProgramiv(&program: ProgramName; pname: ProgramInfoType; [MarshalAs(UnmanagedType.LPArray)] &params: array of Int32);
    external 'opengl32.dll' name 'glGetProgramiv';
    static procedure GetProgramiv(&program: ProgramName; pname: ProgramInfoType; var &params: Int32);
    external 'opengl32.dll' name 'glGetProgramiv';
    static procedure GetProgramiv(&program: ProgramName; pname: ProgramInfoType; &params: pointer);
    external 'opengl32.dll' name 'glGetProgramiv';
    
    // ShaderType автоматически конвертируется в ProgramPipelineInfoType
    static procedure GetProgramPipelineiv(pipeline: ProgramPipelineName; pname: ProgramPipelineInfoType; [MarshalAs(UnmanagedType.LPArray)] &params: array of Int32);
    external 'opengl32.dll' name 'glGetProgramPipelineiv';
    static procedure GetProgramPipelineiv(pipeline: ProgramPipelineName; pname: ProgramPipelineInfoType; var &params: Int32);
    external 'opengl32.dll' name 'glGetProgramPipelineiv';
    static procedure GetProgramPipelineiv(pipeline: ProgramPipelineName; pname: ProgramPipelineInfoType; &params: pointer);
    external 'opengl32.dll' name 'glGetProgramPipelineiv';
    
    static procedure GetAttachedShaders(&program: ProgramName; maxCount: Int32; var count: Int32; [MarshalAs(UnmanagedType.LPArray)] shaders: array of ShaderName);
    external 'opengl32.dll' name 'glGetAttachedShaders';
    static procedure GetAttachedShaders(&program: ProgramName; maxCount: Int32; var count: Int32; shaders: ^ShaderName);
    external 'opengl32.dll' name 'glGetAttachedShaders';
    static procedure GetAttachedShaders(&program: ProgramName; maxCount: Int32; count: ^Int32; [MarshalAs(UnmanagedType.LPArray)] shaders: array of ShaderName);
    external 'opengl32.dll' name 'glGetAttachedShaders';
    static procedure GetAttachedShaders(&program: ProgramName; maxCount: Int32; count: ^Int32; shaders: ^ShaderName);
    external 'opengl32.dll' name 'glGetAttachedShaders';
    
    static procedure GetShaderInfoLog(shader: ShaderName; bufSize: Int32; var length: Int32; [MarshalAs(UnmanagedType.LPStr)] infoLog: string);
    external 'opengl32.dll' name 'glGetShaderInfoLog';
    static procedure GetShaderInfoLog(shader: ShaderName; bufSize: Int32; var length: Int32; infoLog: IntPtr);
    external 'opengl32.dll' name 'glGetShaderInfoLog';
    static procedure GetShaderInfoLog(shader: ShaderName; bufSize: Int32; length: ^Int32; [MarshalAs(UnmanagedType.LPStr)] infoLog: string);
    external 'opengl32.dll' name 'glGetShaderInfoLog';
    static procedure GetShaderInfoLog(shader: ShaderName; bufSize: Int32; length: ^Int32; infoLog: IntPtr);
    external 'opengl32.dll' name 'glGetShaderInfoLog';
    
    static procedure GetProgramInfoLog(&program: ProgramName; bufSize: Int32; var length: Int32; [MarshalAs(UnmanagedType.LPStr)] infoLog: string);
    external 'opengl32.dll' name 'glGetProgramInfoLog';
    static procedure GetProgramInfoLog(&program: ProgramName; bufSize: Int32; var length: Int32; infoLog: IntPtr);
    external 'opengl32.dll' name 'glGetProgramInfoLog';
    static procedure GetProgramInfoLog(&program: ProgramName; bufSize: Int32; length: ^Int32; [MarshalAs(UnmanagedType.LPStr)] infoLog: string);
    external 'opengl32.dll' name 'glGetProgramInfoLog';
    static procedure GetProgramInfoLog(&program: ProgramName; bufSize: Int32; length: ^Int32; infoLog: IntPtr);
    external 'opengl32.dll' name 'glGetProgramInfoLog';
    
    static procedure GetProgramPipelineInfoLog(pipeline: ProgramPipelineName; bufSize: Int32; var length: Int32; [MarshalAs(UnmanagedType.LPStr)] infoLog: string);
    external 'opengl32.dll' name 'glGetProgramPipelineInfoLog';
    static procedure GetProgramPipelineInfoLog(pipeline: ProgramPipelineName; bufSize: Int32; var length: Int32; infoLog: IntPtr);
    external 'opengl32.dll' name 'glGetProgramPipelineInfoLog';
    static procedure GetProgramPipelineInfoLog(pipeline: ProgramPipelineName; bufSize: Int32; length: ^Int32; [MarshalAs(UnmanagedType.LPStr)] infoLog: string);
    external 'opengl32.dll' name 'glGetProgramPipelineInfoLog';
    static procedure GetProgramPipelineInfoLog(pipeline: ProgramPipelineName; bufSize: Int32; length: ^Int32; infoLog: IntPtr);
    external 'opengl32.dll' name 'glGetProgramPipelineInfoLog';
    
    static procedure GetShaderSource(shader: ShaderName; bufSize: Int32; var length: Int32; [MarshalAs(UnmanagedType.LPStr)] source: string);
    external 'opengl32.dll' name 'glGetShaderSource';
    static procedure GetShaderSource(shader: ShaderName; bufSize: Int32; var length: Int32; source: IntPtr);
    external 'opengl32.dll' name 'glGetShaderSource';
    static procedure GetShaderSource(shader: ShaderName; bufSize: Int32; length: ^Int32; [MarshalAs(UnmanagedType.LPStr)] source: string);
    external 'opengl32.dll' name 'glGetShaderSource';
    static procedure GetShaderSource(shader: ShaderName; bufSize: Int32; length: ^Int32; source: IntPtr);
    external 'opengl32.dll' name 'glGetShaderSource';
    
    static procedure GetShaderPrecisionFormat(_shadertype: ShaderType; precisiontype: ShaderPrecisionFormatType; var range: Vec2i; var precision: Int32);
    external 'opengl32.dll' name 'glGetShaderPrecisionFormat';
    static procedure GetShaderPrecisionFormat(_shadertype: ShaderType; precisiontype: ShaderPrecisionFormatType; var range: Vec2i; precision: ^Int32);
    external 'opengl32.dll' name 'glGetShaderPrecisionFormat';
    static procedure GetShaderPrecisionFormat(_shadertype: ShaderType; precisiontype: ShaderPrecisionFormatType; range: ^Vec2i; var precision: Int32);
    external 'opengl32.dll' name 'glGetShaderPrecisionFormat';
    static procedure GetShaderPrecisionFormat(_shadertype: ShaderType; precisiontype: ShaderPrecisionFormatType; range: ^Vec2i; precision: ^Int32);
    external 'opengl32.dll' name 'glGetShaderPrecisionFormat';
    
    static procedure GetUniformfv(&program: ProgramName; location: Int32; [MarshalAs(UnmanagedType.LPArray)] &params: array of single);
    external 'opengl32.dll' name 'glGetUniformfv';
    static procedure GetUniformfv(&program: ProgramName; location: Int32; var &params: single);
    external 'opengl32.dll' name 'glGetUniformfv';
    static procedure GetUniformfv(&program: ProgramName; location: Int32; &params: pointer);
    external 'opengl32.dll' name 'glGetUniformfv';
    
    static procedure GetUniformdv(&program: ProgramName; location: Int32; [MarshalAs(UnmanagedType.LPArray)] &params: array of real);
    external 'opengl32.dll' name 'glGetUniformdv';
    static procedure GetUniformdv(&program: ProgramName; location: Int32; var &params: real);
    external 'opengl32.dll' name 'glGetUniformdv';
    static procedure GetUniformdv(&program: ProgramName; location: Int32; &params: pointer);
    external 'opengl32.dll' name 'glGetUniformdv';
    
    static procedure GetUniformiv(&program: ProgramName; location: Int32; [MarshalAs(UnmanagedType.LPArray)] &params: array of Int32);
    external 'opengl32.dll' name 'glGetUniformiv';
    static procedure GetUniformiv(&program: ProgramName; location: Int32; var &params: Int32);
    external 'opengl32.dll' name 'glGetUniformiv';
    static procedure GetUniformiv(&program: ProgramName; location: Int32; &params: pointer);
    external 'opengl32.dll' name 'glGetUniformiv';
    
    static procedure GetUniformuiv(&program: ProgramName; location: Int32; [MarshalAs(UnmanagedType.LPArray)] &params: array of UInt32);
    external 'opengl32.dll' name 'glGetUniformuiv';
    static procedure GetUniformuiv(&program: ProgramName; location: Int32; var &params: UInt32);
    external 'opengl32.dll' name 'glGetUniformuiv';
    static procedure GetUniformuiv(&program: ProgramName; location: Int32; &params: pointer);
    external 'opengl32.dll' name 'glGetUniformuiv';
    
    static procedure GetnUniformfv(&program: ProgramName; location: Int32; bufSize: Int32; [MarshalAs(UnmanagedType.LPArray)] &params: array of single);
    external 'opengl32.dll' name 'glGetnUniformfv';
    static procedure GetnUniformfv(&program: ProgramName; location: Int32; bufSize: Int32; var &params: single);
    external 'opengl32.dll' name 'glGetnUniformfv';
    static procedure GetnUniformfv(&program: ProgramName; location: Int32; bufSize: Int32; &params: pointer);
    external 'opengl32.dll' name 'glGetnUniformfv';
    
    static procedure GetnUniformdv(&program: ProgramName; location: Int32; bufSize: Int32; [MarshalAs(UnmanagedType.LPArray)] &params: array of real);
    external 'opengl32.dll' name 'glGetnUniformdv';
    static procedure GetnUniformdv(&program: ProgramName; location: Int32; bufSize: Int32; var &params: real);
    external 'opengl32.dll' name 'glGetnUniformdv';
    static procedure GetnUniformdv(&program: ProgramName; location: Int32; bufSize: Int32; &params: pointer);
    external 'opengl32.dll' name 'glGetnUniformdv';
    
    static procedure GetnUniformiv(&program: ProgramName; location: Int32; bufSize: Int32; [MarshalAs(UnmanagedType.LPArray)] &params: array of Int32);
    external 'opengl32.dll' name 'glGetnUniformiv';
    static procedure GetnUniformiv(&program: ProgramName; location: Int32; bufSize: Int32; var &params: Int32);
    external 'opengl32.dll' name 'glGetnUniformiv';
    static procedure GetnUniformiv(&program: ProgramName; location: Int32; bufSize: Int32; &params: pointer);
    external 'opengl32.dll' name 'glGetnUniformiv';
    
    static procedure GetnUniformuiv(&program: ProgramName; location: Int32; bufSize: Int32; [MarshalAs(UnmanagedType.LPArray)] &params: array of UInt32);
    external 'opengl32.dll' name 'glGetnUniformuiv';
    static procedure GetnUniformuiv(&program: ProgramName; location: Int32; bufSize: Int32; var &params: UInt32);
    external 'opengl32.dll' name 'glGetnUniformuiv';
    static procedure GetnUniformuiv(&program: ProgramName; location: Int32; bufSize: Int32; &params: pointer);
    external 'opengl32.dll' name 'glGetnUniformuiv';
    
    static procedure GetUniformSubroutineuiv(_shadertype: ShaderType; location: Int32; [MarshalAs(UnmanagedType.LPArray)] &params: array of UInt32);
    external 'opengl32.dll' name 'glGetUniformSubroutineuiv';
    static procedure GetUniformSubroutineuiv(_shadertype: ShaderType; location: Int32; var &params: UInt32);
    external 'opengl32.dll' name 'glGetUniformSubroutineuiv';
    static procedure GetUniformSubroutineuiv(_shadertype: ShaderType; location: Int32; &params: pointer);
    external 'opengl32.dll' name 'glGetUniformSubroutineuiv';
    
    static procedure GetProgramStageiv(&program: ProgramName; _shadertype: ShaderType; pname: ActiveSubroutineInfoType; [MarshalAs(UnmanagedType.LPArray)] values: array of Int32);
    external 'opengl32.dll' name 'glGetProgramStageiv';
    static procedure GetProgramStageiv(&program: ProgramName; _shadertype: ShaderType; pname: ActiveSubroutineInfoType; var values: Int32);
    external 'opengl32.dll' name 'glGetProgramStageiv';
    static procedure GetProgramStageiv(&program: ProgramName; _shadertype: ShaderType; pname: ActiveSubroutineInfoType; values: pointer);
    external 'opengl32.dll' name 'glGetProgramStageiv';
    
    {$endregion 7.14 - Shader, Program, and Program Pipeline Queries}
    
    {$endregion 7.0 - Programs and Shaders}
    
    {$region 8.0 - Textures and Samplers}
    
    static procedure ActiveTexture(texture: TextureUnitId);
    external 'opengl32.dll' name 'glActiveTexture';
    
    {$region 8.1 - Texture Objects}
    
    static procedure GenTextures(n: Int32; [MarshalAs(UnmanagedType.LPArray)] textures: array of TextureName);
    external 'opengl32.dll' name 'glGenTextures';
    static procedure GenTextures(n: Int32; var textures: TextureName);
    external 'opengl32.dll' name 'glGenTextures';
    static procedure GenTextures(n: Int32; textures: pointer);
    external 'opengl32.dll' name 'glGenTextures';
    
    static procedure BindTexture(target: TextureBindTarget; texture: TextureName);
    external 'opengl32.dll' name 'glBindTexture';
    
    static procedure BindTextures(first: TextureUnitId; count: Int32; [MarshalAs(UnmanagedType.LPArray)] textures: array of TextureName);
    external 'opengl32.dll' name 'glBindTextures';
    static procedure BindTextures(first: TextureUnitId; count: Int32; textures: ^TextureName);
    external 'opengl32.dll' name 'glBindTextures';
    
    static procedure BindTextureUnit(&unit: TextureUnitId; texture: TextureName);
    external 'opengl32.dll' name 'glBindTextureUnit';
    
    static procedure CreateTextures(target: TextureBindTarget; n: Int32; [MarshalAs(UnmanagedType.LPArray)] textures: array of TextureName);
    external 'opengl32.dll' name 'glCreateTextures';
    static procedure CreateTextures(target: TextureBindTarget; n: Int32; textures: ^TextureName);
    external 'opengl32.dll' name 'glCreateTextures';
    
    static procedure DeleteTextures(n: Int32; [MarshalAs(UnmanagedType.LPArray)] textures: array of TextureName);
    external 'opengl32.dll' name 'glDeleteTextures';
    static procedure DeleteTextures(n: Int32; var textures: TextureName);
    external 'opengl32.dll' name 'glDeleteTextures';
    static procedure DeleteTextures(n: Int32; textures: ^TextureName);
    external 'opengl32.dll' name 'glDeleteTextures';
    
    static function IsTexture(texture: TextureName): boolean;
    external 'opengl32.dll' name 'glIsTexture';
    
    {$endregion 8.1 - Texture Objects}
    
    {$region 8.2 - Sampler Objects}
    
    static procedure GenSamplers(count: Int32; [MarshalAs(UnmanagedType.LPArray)] samplers: array of SamplerName);
    external 'opengl32.dll' name 'glGenSamplers';
    static procedure GenSamplers(count: Int32; var samplers: SamplerName);
    external 'opengl32.dll' name 'glGenSamplers';
    static procedure GenSamplers(count: Int32; samplers: pointer);
    external 'opengl32.dll' name 'glGenSamplers';
    
    static procedure CreateSamplers(n: Int32; [MarshalAs(UnmanagedType.LPArray)] samplers: array of SamplerName);
    external 'opengl32.dll' name 'glCreateSamplers';
    static procedure CreateSamplers(n: Int32; var samplers: SamplerName);
    external 'opengl32.dll' name 'glCreateSamplers';
    static procedure CreateSamplers(n: Int32; samplers: ^SamplerName);
    external 'opengl32.dll' name 'glCreateSamplers';
    
    static procedure BindSampler(&unit: Int32; sampler: SamplerName);
    external 'opengl32.dll' name 'glBindSampler';
    
    static procedure BindSamplers(first: Int32; count: Int32; [MarshalAs(UnmanagedType.LPArray)] samplers: array of SamplerName);
    external 'opengl32.dll' name 'glBindSamplers';
    static procedure BindSamplers(first: Int32; count: Int32; samplers: ^SamplerName);
    external 'opengl32.dll' name 'glBindSamplers';
    
    static procedure SamplerParameteri(sampler: SamplerName; pname: TextureInfoType; param: Int32);
    external 'opengl32.dll' name 'glSamplerParameteri';
    
    static procedure SamplerParameterf(sampler: SamplerName; pname: TextureInfoType; param: single);
    external 'opengl32.dll' name 'glSamplerParameterf';
    
    static procedure SamplerParameteriv(sampler: SamplerName; pname: TextureInfoType; var param: Int32);
    external 'opengl32.dll' name 'glSamplerParameteriv';
    static procedure SamplerParameteriv(sampler: SamplerName; pname: TextureInfoType; param: ^Int32);
    external 'opengl32.dll' name 'glSamplerParameteriv';
    
    static procedure SamplerParameterfv(sampler: SamplerName; pname: TextureInfoType; var param: single);
    external 'opengl32.dll' name 'glSamplerParameterfv';
    static procedure SamplerParameterfv(sampler: SamplerName; pname: TextureInfoType; param: ^single);
    external 'opengl32.dll' name 'glSamplerParameterfv';
    
    static procedure SamplerParameterIiv(sampler: SamplerName; pname: TextureInfoType; var param: Int32);
    external 'opengl32.dll' name 'glSamplerParameterIiv';
    static procedure SamplerParameterIiv(sampler: SamplerName; pname: TextureInfoType; param: ^Int32);
    external 'opengl32.dll' name 'glSamplerParameterIiv';
    
    static procedure SamplerParameterIuiv(sampler: SamplerName; pname: TextureInfoType; var param: UInt32);
    external 'opengl32.dll' name 'glSamplerParameterIuiv';
    static procedure SamplerParameterIuiv(sampler: SamplerName; pname: TextureInfoType; param: ^UInt32);
    external 'opengl32.dll' name 'glSamplerParameterIuiv';
    
    static procedure DeleteSamplers(count: Int32; [MarshalAs(UnmanagedType.LPArray)] samplers: array of SamplerName);
    external 'opengl32.dll' name 'glDeleteSamplers';
    static procedure DeleteSamplers(count: Int32; var samplers: SamplerName);
    external 'opengl32.dll' name 'glDeleteSamplers';
    static procedure DeleteSamplers(count: Int32; samplers: ^SamplerName);
    external 'opengl32.dll' name 'glDeleteSamplers';
    
    static function IsSampler(sampler: SamplerName): boolean;
    external 'opengl32.dll' name 'glIsSampler';
    
    {$endregion 8.2 - Sampler Objects}
    
    {$region 8.3 - Sampler Object Queries}
    
    static procedure GetSamplerParameteriv(sampler: SamplerName; pname: TextureInfoType; var &params: Int32);
    external 'opengl32.dll' name 'glGetSamplerParameteriv';
    static procedure GetSamplerParameteriv(sampler: SamplerName; pname: TextureInfoType; &params: ^Int32);
    external 'opengl32.dll' name 'glGetSamplerParameteriv';
    
    static procedure GetSamplerParameterfv(sampler: SamplerName; pname: TextureInfoType; var &params: single);
    external 'opengl32.dll' name 'glGetSamplerParameterfv';
    static procedure GetSamplerParameterfv(sampler: SamplerName; pname: TextureInfoType; &params: ^single);
    external 'opengl32.dll' name 'glGetSamplerParameterfv';
    
    static procedure GetSamplerParameterIiv(sampler: SamplerName; pname: TextureInfoType; var &params: Int32);
    external 'opengl32.dll' name 'glGetSamplerParameterIiv';
    static procedure GetSamplerParameterIiv(sampler: SamplerName; pname: TextureInfoType; &params: ^Int32);
    external 'opengl32.dll' name 'glGetSamplerParameterIiv';
    
    static procedure GetSamplerParameterIuiv(sampler: SamplerName; pname: TextureInfoType; var &params: UInt32);
    external 'opengl32.dll' name 'glGetSamplerParameterIuiv';
    static procedure GetSamplerParameterIuiv(sampler: SamplerName; pname: TextureInfoType; &params: ^UInt32);
    external 'opengl32.dll' name 'glGetSamplerParameterIuiv';
    
    {$endregion 8.3 - Sampler Object Queries}
    
    {$region 8.4 - Pixel Rectangles}
    
    // 8.4.1
    
    static procedure PixelStorei(pname: PixelInfoType; param: Int32);
    external 'opengl32.dll' name 'glPixelStorei';
    
    static procedure PixelStoref(pname: PixelInfoType; param: single);
    external 'opengl32.dll' name 'glPixelStoref';
    
    {$endregion 8.4 - Pixel Rectangles}
    
    {$region 8.5 - Texture Image Specification}
    
    static procedure TexImage3D(target: TextureBindTarget; level: Int32; internalformat: InternalDataFormat; width: Int32; height: Int32; depth: Int32; border: Int32; format: DataFormat; &type: DataType; pixels: IntPtr);
    external 'opengl32.dll' name 'glTexImage3D';
    static procedure TexImage3D(target: TextureBindTarget; level: Int32; internalformat: InternalDataFormat; width: Int32; height: Int32; depth: Int32; border: Int32; format: DataFormat; &type: DataType; pixels: pointer);
    external 'opengl32.dll' name 'glTexImage3D';
    
    static procedure TexImage2D(target: TextureBindTarget; level: Int32; internalformat: InternalDataFormat; width: Int32; height: Int32; border: Int32; format: DataFormat; &type: DataType; pixels: IntPtr);
    external 'opengl32.dll' name 'glTexImage2D';
    static procedure TexImage2D(target: TextureBindTarget; level: Int32; internalformat: InternalDataFormat; width: Int32; height: Int32; border: Int32; format: DataFormat; &type: DataType; pixels: pointer);
    external 'opengl32.dll' name 'glTexImage2D';
    
    static procedure TexImage1D(target: TextureBindTarget; level: Int32; internalformat: InternalDataFormat; width: Int32; border: Int32; format: DataFormat; &type: DataType; pixels: IntPtr);
    external 'opengl32.dll' name 'glTexImage1D';
    static procedure TexImage1D(target: TextureBindTarget; level: Int32; internalformat: InternalDataFormat; width: Int32; border: Int32; format: DataFormat; &type: DataType; pixels: pointer);
    external 'opengl32.dll' name 'glTexImage1D';
    
    {$endregion 8.5 - Texture Image Specification}
    
    {$region 8.6 - Alternate Texture Image Specification Commands}
    
    static procedure CopyTexImage2D(target: TextureBindTarget; level: Int32; internalformat: InternalDataFormat; x: Int32; y: Int32; width: Int32; height: Int32; border: Int32);
    external 'opengl32.dll' name 'glCopyTexImage2D';
    
    static procedure CopyTexImage1D(target: TextureBindTarget; level: Int32; internalformat: InternalDataFormat; x: Int32; y: Int32; width: Int32; border: Int32);
    external 'opengl32.dll' name 'glCopyTexImage1D';
    
    static procedure TexSubImage3D(target: TextureBindTarget; level: Int32; xoffset: Int32; yoffset: Int32; zoffset: Int32; width: Int32; height: Int32; depth: Int32; format: DataFormat; &type: DataType; pixels: IntPtr);
    external 'opengl32.dll' name 'glTexSubImage3D';
    static procedure TexSubImage3D(target: TextureBindTarget; level: Int32; xoffset: Int32; yoffset: Int32; zoffset: Int32; width: Int32; height: Int32; depth: Int32; format: DataFormat; &type: DataType; pixels: pointer);
    external 'opengl32.dll' name 'glTexSubImage3D';
    
    static procedure TexSubImage2D(target: TextureBindTarget; level: Int32; xoffset: Int32; yoffset: Int32; width: Int32; height: Int32; format: DataFormat; &type: DataType; pixels: IntPtr);
    external 'opengl32.dll' name 'glTexSubImage2D';
    static procedure TexSubImage2D(target: TextureBindTarget; level: Int32; xoffset: Int32; yoffset: Int32; width: Int32; height: Int32; format: DataFormat; &type: DataType; pixels: pointer);
    external 'opengl32.dll' name 'glTexSubImage2D';
    
    static procedure TexSubImage1D(target: TextureBindTarget; level: Int32; xoffset: Int32; width: Int32; format: DataFormat; &type: DataType; pixels: IntPtr);
    external 'opengl32.dll' name 'glTexSubImage1D';
    static procedure TexSubImage1D(target: TextureBindTarget; level: Int32; xoffset: Int32; width: Int32; format: DataFormat; &type: DataType; pixels: pointer);
    external 'opengl32.dll' name 'glTexSubImage1D';
    
    static procedure CopyTexSubImage3D(target: TextureBindTarget; level: Int32; xoffset: Int32; yoffset: Int32; zoffset: Int32; x: Int32; y: Int32; width: Int32; height: Int32);
    external 'opengl32.dll' name 'glCopyTexSubImage3D';
    
    static procedure CopyTexSubImage2D(target: TextureBindTarget; level: Int32; xoffset: Int32; yoffset: Int32; x: Int32; y: Int32; width: Int32; height: Int32);
    external 'opengl32.dll' name 'glCopyTexSubImage2D';
    
    static procedure CopyTexSubImage1D(target: TextureBindTarget; level: Int32; xoffset: Int32; x: Int32; y: Int32; width: Int32);
    external 'opengl32.dll' name 'glCopyTexSubImage1D';
    
    static procedure TextureSubImage3D(texture: TextureName; level: Int32; xoffset: Int32; yoffset: Int32; zoffset: Int32; width: Int32; height: Int32; depth: Int32; format: DataFormat; &type: DataType; pixels: IntPtr);
    external 'opengl32.dll' name 'glTextureSubImage3D';
    static procedure TextureSubImage3D(texture: TextureName; level: Int32; xoffset: Int32; yoffset: Int32; zoffset: Int32; width: Int32; height: Int32; depth: Int32; format: DataFormat; &type: DataType; pixels: pointer);
    external 'opengl32.dll' name 'glTextureSubImage3D';
    
    static procedure TextureSubImage2D(texture: TextureName; level: Int32; xoffset: Int32; yoffset: Int32; width: Int32; height: Int32; format: DataFormat; &type: DataType; pixels: IntPtr);
    external 'opengl32.dll' name 'glTextureSubImage2D';
    static procedure TextureSubImage2D(texture: TextureName; level: Int32; xoffset: Int32; yoffset: Int32; width: Int32; height: Int32; format: DataFormat; &type: DataType; pixels: pointer);
    external 'opengl32.dll' name 'glTextureSubImage2D';
    
    static procedure TextureSubImage1D(texture: TextureName; level: Int32; xoffset: Int32; width: Int32; DataFormat: UInt32; &type: DataType; pixels: IntPtr);
    external 'opengl32.dll' name 'glTextureSubImage1D';
    static procedure TextureSubImage1D(texture: TextureName; level: Int32; xoffset: Int32; width: Int32; DataFormat: UInt32; &type: DataType; pixels: pointer);
    external 'opengl32.dll' name 'glTextureSubImage1D';
    
    static procedure CopyTextureSubImage3D(texture: TextureName; level: Int32; xoffset: Int32; yoffset: Int32; zoffset: Int32; x: Int32; y: Int32; width: Int32; height: Int32);
    external 'opengl32.dll' name 'glCopyTextureSubImage3D';
    
    static procedure CopyTextureSubImage2D(texture: TextureName; level: Int32; xoffset: Int32; yoffset: Int32; x: Int32; y: Int32; width: Int32; height: Int32);
    external 'opengl32.dll' name 'glCopyTextureSubImage2D';
    
    static procedure CopyTextureSubImage1D(texture: TextureName; level: Int32; xoffset: Int32; x: Int32; y: Int32; width: Int32);
    external 'opengl32.dll' name 'glCopyTextureSubImage1D';
    
    {$endregion 8.6 - Alternate Texture Image Specification Commands}
    
    {$region 8.7 - Compressed Texture Images}
    
    static procedure CompressedTexImage1D(target: TextureBindTarget; level: Int32; internalformat: InternalDataFormat; width: Int32; border: Int32; imageSize: Int32; data: IntPtr);
    external 'opengl32.dll' name 'glCompressedTexImage1D';
    static procedure CompressedTexImage1D(target: TextureBindTarget; level: Int32; internalformat: InternalDataFormat; width: Int32; border: Int32; imageSize: Int32; data: pointer);
    external 'opengl32.dll' name 'glCompressedTexImage1D';
    
    static procedure CompressedTexImage2D(target: TextureBindTarget; level: Int32; internalformat: InternalDataFormat; width: Int32; height: Int32; border: Int32; imageSize: Int32; data: IntPtr);
    external 'opengl32.dll' name 'glCompressedTexImage2D';
    static procedure CompressedTexImage2D(target: TextureBindTarget; level: Int32; internalformat: InternalDataFormat; width: Int32; height: Int32; border: Int32; imageSize: Int32; data: pointer);
    external 'opengl32.dll' name 'glCompressedTexImage2D';
    
    static procedure CompressedTexImage3D(target: TextureBindTarget; level: Int32; internalformat: InternalDataFormat; width: Int32; height: Int32; depth: Int32; border: Int32; imageSize: Int32; data: IntPtr);
    external 'opengl32.dll' name 'glCompressedTexImage3D';
    static procedure CompressedTexImage3D(target: TextureBindTarget; level: Int32; internalformat: InternalDataFormat; width: Int32; height: Int32; depth: Int32; border: Int32; imageSize: Int32; data: pointer);
    external 'opengl32.dll' name 'glCompressedTexImage3D';
    
    static procedure CompressedTexSubImage1D(target: TextureBindTarget; level: Int32; xoffset: Int32; width: Int32; format: DataFormat; imageSize: Int32; data: IntPtr);
    external 'opengl32.dll' name 'glCompressedTexSubImage1D';
    static procedure CompressedTexSubImage1D(target: TextureBindTarget; level: Int32; xoffset: Int32; width: Int32; format: DataFormat; imageSize: Int32; data: pointer);
    external 'opengl32.dll' name 'glCompressedTexSubImage1D';
    
    static procedure CompressedTexSubImage2D(target: TextureBindTarget; level: Int32; xoffset: Int32; yoffset: Int32; width: Int32; height: Int32; format: DataFormat; imageSize: Int32; data: IntPtr);
    external 'opengl32.dll' name 'glCompressedTexSubImage2D';
    static procedure CompressedTexSubImage2D(target: TextureBindTarget; level: Int32; xoffset: Int32; yoffset: Int32; width: Int32; height: Int32; format: DataFormat; imageSize: Int32; data: pointer);
    external 'opengl32.dll' name 'glCompressedTexSubImage2D';
    
    static procedure CompressedTexSubImage3D(target: TextureBindTarget; level: Int32; xoffset: Int32; yoffset: Int32; zoffset: Int32; width: Int32; height: Int32; depth: Int32; format: DataFormat; imageSize: Int32; data: IntPtr);
    external 'opengl32.dll' name 'glCompressedTexSubImage3D';
    static procedure CompressedTexSubImage3D(target: TextureBindTarget; level: Int32; xoffset: Int32; yoffset: Int32; zoffset: Int32; width: Int32; height: Int32; depth: Int32; format: DataFormat; imageSize: Int32; data: pointer);
    external 'opengl32.dll' name 'glCompressedTexSubImage3D';
    
    static procedure CompressedTextureSubImage1D(texture: TextureBindTarget; level: Int32; xoffset: Int32; width: Int32; format: DataFormat; imageSize: Int32; data: IntPtr);
    external 'opengl32.dll' name 'glCompressedTextureSubImage1D';
    static procedure CompressedTextureSubImage1D(texture: TextureBindTarget; level: Int32; xoffset: Int32; width: Int32; format: DataFormat; imageSize: Int32; data: pointer);
    external 'opengl32.dll' name 'glCompressedTextureSubImage1D';
    
    static procedure CompressedTextureSubImage2D(texture: TextureBindTarget; level: Int32; xoffset: Int32; yoffset: Int32; width: Int32; height: Int32; format: DataFormat; imageSize: Int32; data: IntPtr);
    external 'opengl32.dll' name 'glCompressedTextureSubImage2D';
    static procedure CompressedTextureSubImage2D(texture: TextureBindTarget; level: Int32; xoffset: Int32; yoffset: Int32; width: Int32; height: Int32; format: DataFormat; imageSize: Int32; data: pointer);
    external 'opengl32.dll' name 'glCompressedTextureSubImage2D';
    
    static procedure CompressedTextureSubImage3D(texture: TextureBindTarget; level: Int32; xoffset: Int32; yoffset: Int32; zoffset: Int32; width: Int32; height: Int32; depth: Int32; format: DataFormat; imageSize: Int32; data: IntPtr);
    external 'opengl32.dll' name 'glCompressedTextureSubImage3D';
    static procedure CompressedTextureSubImage3D(texture: TextureBindTarget; level: Int32; xoffset: Int32; yoffset: Int32; zoffset: Int32; width: Int32; height: Int32; depth: Int32; format: DataFormat; imageSize: Int32; data: pointer);
    external 'opengl32.dll' name 'glCompressedTextureSubImage3D';
    
    {$endregion 8.7 - Compressed Texture Images}
    
    {$region 8.8 - Multisample Textures}
    
    static procedure TexImage2DMultisample(target: TextureBindTarget; samples: Int32; internalformat: InternalDataFormat; width: Int32; height: Int32; fixedsamplelocations: boolean);
    external 'opengl32.dll' name 'glTexImage2DMultisample';
    
    static procedure TexImage3DMultisample(target: TextureBindTarget; samples: Int32; internalformat: InternalDataFormat; width: Int32; height: Int32; depth: Int32; fixedsamplelocations: boolean);
    external 'opengl32.dll' name 'glTexImage3DMultisample';
    
    {$endregion 8.8 - Multisample Textures}
    
    {$region 8.9 - Buffer Textures}
    
    static procedure TexBufferRange(target: TextureBindTarget; internalformat: InternalDataFormat; buffer: BufferName; offset: IntPtr; size: UIntPtr);
    external 'opengl32.dll' name 'glTexBufferRange';
    
    static procedure TextureBufferRange(texture: TextureName; internalformat: InternalDataFormat; buffer: BufferName; offset: IntPtr; size: UIntPtr);
    external 'opengl32.dll' name 'glTextureBufferRange';
    
    static procedure TexBuffer(target: TextureBindTarget; internalformat: InternalDataFormat; buffer: BufferName);
    external 'opengl32.dll' name 'glTexBuffer';
    
    static procedure TextureBuffer(texture: TextureName; internalformat: InternalDataFormat; buffer: BufferName);
    external 'opengl32.dll' name 'glTextureBuffer';
    
    {$endregion 8.9 - Buffer Textures}
    
    {$region 8.10 - Texture Parameters}
    //ToDo передавать можно энумы и массивы (и энумов тоже)...
    // - в конце проверить чтоб все энумы существовали
    // - и возможно ещё сделать для них перегрузки
    // - это так же касается 8.11
    
    static procedure TexParameteri(target: TextureBindTarget; pname: TextureInfoType; param: Int32);
    external 'opengl32.dll' name 'glTexParameteri';
    
    static procedure TexParameterf(target: TextureBindTarget; pname: TextureInfoType; param: single);
    external 'opengl32.dll' name 'glTexParameterf';
    
    static procedure TexParameteriv(target: TextureBindTarget; pname: TextureInfoType; var &params: Int32);
    external 'opengl32.dll' name 'glTexParameteriv';
    static procedure TexParameteriv(target: TextureBindTarget; pname: TextureInfoType; &params: ^Int32);
    external 'opengl32.dll' name 'glTexParameteriv';
    
    static procedure TexParameterfv(target: TextureBindTarget; pname: TextureInfoType; var &params: single);
    external 'opengl32.dll' name 'glTexParameterfv';
    static procedure TexParameterfv(target: TextureBindTarget; pname: TextureInfoType; &params: ^single);
    external 'opengl32.dll' name 'glTexParameterfv';
    
    static procedure TexParameterIiv(target: TextureBindTarget; pname: TextureInfoType; var &params: Int32);
    external 'opengl32.dll' name 'glTexParameterIiv';
    static procedure TexParameterIiv(target: TextureBindTarget; pname: TextureInfoType; &params: ^Int32);
    external 'opengl32.dll' name 'glTexParameterIiv';
    
    static procedure TexParameterIuiv(target: TextureBindTarget; pname: TextureInfoType; var &params: UInt32);
    external 'opengl32.dll' name 'glTexParameterIuiv';
    static procedure TexParameterIuiv(target: TextureBindTarget; pname: TextureInfoType; &params: ^UInt32);
    external 'opengl32.dll' name 'glTexParameterIuiv';
    
    static procedure TextureParameteri(texture: TextureName; pname: TextureInfoType; param: Int32);
    external 'opengl32.dll' name 'glTextureParameteri';
    
    static procedure TextureParameterf(texture: TextureName; pname: TextureInfoType; param: single);
    external 'opengl32.dll' name 'glTextureParameterf';
    
    static procedure TextureParameteriv(texture: TextureName; pname: TextureInfoType; var param: Int32);
    external 'opengl32.dll' name 'glTextureParameteriv';
    static procedure TextureParameteriv(texture: TextureName; pname: TextureInfoType; param: ^Int32);
    external 'opengl32.dll' name 'glTextureParameteriv';
    
    static procedure TextureParameterfv(texture: TextureName; pname: TextureInfoType; var param: single);
    external 'opengl32.dll' name 'glTextureParameterfv';
    static procedure TextureParameterfv(texture: TextureName; pname: TextureInfoType; param: ^single);
    external 'opengl32.dll' name 'glTextureParameterfv';
    
    static procedure TextureParameterIiv(texture: TextureName; pname: TextureInfoType; var &params: Int32);
    external 'opengl32.dll' name 'glTextureParameterIiv';
    static procedure TextureParameterIiv(texture: TextureName; pname: TextureInfoType; &params: ^Int32);
    external 'opengl32.dll' name 'glTextureParameterIiv';
    
    static procedure TextureParameterIuiv(texture: TextureName; pname: TextureInfoType; var &params: UInt32);
    external 'opengl32.dll' name 'glTextureParameterIuiv';
    static procedure TextureParameterIuiv(texture: TextureName; pname: TextureInfoType; &params: ^UInt32);
    external 'opengl32.dll' name 'glTextureParameterIuiv';
    
    {$endregion 8.10 - Texture Parameters}
    
    {$region 8.11 - Texture Queries}
    
    // 8.11.2
    
    static procedure GetTexParameteriv(target: TextureBindTarget; pname: TextureInfoType; var &params: Int32);
    external 'opengl32.dll' name 'glGetTexParameteriv';
    static procedure GetTexParameteriv(target: TextureBindTarget; pname: TextureInfoType; &params: ^Int32);
    external 'opengl32.dll' name 'glGetTexParameteriv';
    
    static procedure GetTexParameterfv(target: TextureBindTarget; pname: TextureInfoType; var &params: single);
    external 'opengl32.dll' name 'glGetTexParameterfv';
    static procedure GetTexParameterfv(target: TextureBindTarget; pname: TextureInfoType; &params: ^single);
    external 'opengl32.dll' name 'glGetTexParameterfv';
    
    static procedure GetTexParameterIiv(target: TextureBindTarget; pname: TextureInfoType; var &params: Int32);
    external 'opengl32.dll' name 'glGetTexParameterIiv';
    static procedure GetTexParameterIiv(target: TextureBindTarget; pname: TextureInfoType; &params: ^Int32);
    external 'opengl32.dll' name 'glGetTexParameterIiv';
    
    static procedure GetTexParameterIuiv(target: TextureBindTarget; pname: TextureInfoType; var &params: UInt32);
    external 'opengl32.dll' name 'glGetTexParameterIuiv';
    static procedure GetTexParameterIuiv(target: TextureBindTarget; pname: TextureInfoType; &params: ^UInt32);
    external 'opengl32.dll' name 'glGetTexParameterIuiv';
    
    static procedure GetTextureParameteriv(texture: TextureName; pname: TextureInfoType; var &params: Int32);
    external 'opengl32.dll' name 'glGetTextureParameteriv';
    static procedure GetTextureParameteriv(texture: TextureName; pname: TextureInfoType; &params: ^Int32);
    external 'opengl32.dll' name 'glGetTextureParameteriv';
    
    static procedure GetTextureParameterfv(texture: TextureName; pname: TextureInfoType; var &params: single);
    external 'opengl32.dll' name 'glGetTextureParameterfv';
    static procedure GetTextureParameterfv(texture: TextureName; pname: TextureInfoType; &params: ^single);
    external 'opengl32.dll' name 'glGetTextureParameterfv';
    
    static procedure GetTextureParameterIiv(texture: TextureName; pname: TextureInfoType; var &params: Int32);
    external 'opengl32.dll' name 'glGetTextureParameterIiv';
    static procedure GetTextureParameterIiv(texture: TextureName; pname: TextureInfoType; &params: ^Int32);
    external 'opengl32.dll' name 'glGetTextureParameterIiv';
    
    static procedure GetTextureParameterIuiv(texture: TextureName; pname: TextureInfoType; var &params: UInt32);
    external 'opengl32.dll' name 'glGetTextureParameterIuiv';
    static procedure GetTextureParameterIuiv(texture: TextureName; pname: TextureInfoType; &params: ^UInt32);
    external 'opengl32.dll' name 'glGetTextureParameterIuiv';
    
    // 8.11.3
    
    static procedure GetTexLevelParameterfv(target: TextureBindTarget; level: Int32; pname: TextureInfoType; var &params: single);
    external 'opengl32.dll' name 'glGetTexLevelParameterfv';
    static procedure GetTexLevelParameterfv(target: TextureBindTarget; level: Int32; pname: TextureInfoType; &params: ^single);
    external 'opengl32.dll' name 'glGetTexLevelParameterfv';
    
    static procedure GetTexLevelParameteriv(target: TextureBindTarget; level: Int32; pname: TextureInfoType; var &params: Int32);
    external 'opengl32.dll' name 'glGetTexLevelParameteriv';
    static procedure GetTexLevelParameteriv(target: TextureBindTarget; level: Int32; pname: TextureInfoType; &params: ^Int32);
    external 'opengl32.dll' name 'glGetTexLevelParameteriv';
    
    static procedure GetTextureLevelParameterfv(texture: TextureName; level: Int32; pname: TextureInfoType; var &params: single);
    external 'opengl32.dll' name 'glGetTextureLevelParameterfv';
    static procedure GetTextureLevelParameterfv(texture: TextureName; level: Int32; pname: TextureInfoType; &params: ^single);
    external 'opengl32.dll' name 'glGetTextureLevelParameterfv';
    
    static procedure GetTextureLevelParameteriv(texture: TextureName; level: Int32; pname: TextureInfoType; var &params: Int32);
    external 'opengl32.dll' name 'glGetTextureLevelParameteriv';
    static procedure GetTextureLevelParameteriv(texture: TextureName; level: Int32; pname: TextureInfoType; &params: ^Int32);
    external 'opengl32.dll' name 'glGetTextureLevelParameteriv';
    
    // 8.11.4
    
    static procedure GetTexImage(target: TextureBindTarget; level: Int32; format: DataFormat; &type: DataType; pixels: IntPtr);
    external 'opengl32.dll' name 'glGetTexImage';
    static procedure GetTexImage(target: TextureBindTarget; level: Int32; format: DataFormat; &type: DataType; pixels: pointer);
    external 'opengl32.dll' name 'glGetTexImage';
    
    static procedure GetTextureImage(texture: TextureName; level: Int32; format: DataFormat; &type: DataType; bufSize: Int32; pixels: IntPtr);
    external 'opengl32.dll' name 'glGetTextureImage';
    static procedure GetTextureImage(texture: TextureName; level: Int32; format: DataFormat; &type: DataType; bufSize: Int32; pixels: pointer);
    external 'opengl32.dll' name 'glGetTextureImage';
    
    static procedure GetnTexImage(target: TextureBindTarget; level: Int32; format: DataFormat; &type: DataType; bufSize: Int32; pixels: IntPtr);
    external 'opengl32.dll' name 'glGetnTexImage';
    static procedure GetnTexImage(target: TextureBindTarget; level: Int32; format: DataFormat; &type: DataType; bufSize: Int32; pixels: pointer);
    external 'opengl32.dll' name 'glGetnTexImage';
    
    static procedure GetTextureSubImage(texture: TextureName; level: Int32; xoffset: Int32; yoffset: Int32; zoffset: Int32; width: Int32; height: Int32; depth: Int32; format: DataFormat; &type: DataType; bufSize: Int32; pixels: IntPtr);
    external 'opengl32.dll' name 'glGetTextureSubImage';
    static procedure GetTextureSubImage(texture: TextureName; level: Int32; xoffset: Int32; yoffset: Int32; zoffset: Int32; width: Int32; height: Int32; depth: Int32; format: DataFormat; &type: DataType; bufSize: Int32; pixels: pointer);
    external 'opengl32.dll' name 'glGetTextureSubImage';
    
    static procedure GetCompressedTexImage(target: TextureBindTarget; level: Int32; pixels: IntPtr);
    external 'opengl32.dll' name 'glGetCompressedTexImage';
    static procedure GetCompressedTexImage(target: TextureBindTarget; level: Int32; pixels: pointer);
    external 'opengl32.dll' name 'glGetCompressedTexImage';
    
    static procedure GetCompressedTextureImage(texture: TextureName; level: Int32; bufSize: Int32; pixels: IntPtr);
    external 'opengl32.dll' name 'glGetCompressedTextureImage';
    static procedure GetCompressedTextureImage(texture: TextureName; level: Int32; bufSize: Int32; pixels: pointer);
    external 'opengl32.dll' name 'glGetCompressedTextureImage';
    
    static procedure GetnCompressedTexImage(target: TextureBindTarget; level: Int32; bufSize: Int32; pixels: IntPtr);
    external 'opengl32.dll' name 'glGetnCompressedTexImage';
    static procedure GetnCompressedTexImage(target: TextureBindTarget; level: Int32; bufSize: Int32; pixels: pointer);
    external 'opengl32.dll' name 'glGetnCompressedTexImage';
    
    static procedure GetCompressedTextureSubImage(texture: TextureName; level: Int32; xoffset: Int32; yoffset: Int32; zoffset: Int32; width: Int32; height: Int32; depth: Int32; bufSize: Int32; pixels: IntPtr);
    external 'opengl32.dll' name 'glGetCompressedTextureSubImage';
    static procedure GetCompressedTextureSubImage(texture: TextureName; level: Int32; xoffset: Int32; yoffset: Int32; zoffset: Int32; width: Int32; height: Int32; depth: Int32; bufSize: Int32; pixels: pointer);
    external 'opengl32.dll' name 'glGetCompressedTextureSubImage';
    
    {$endregion 8.11 - Texture Queries}
    
    {$region 8.14 - Texture Minification}
    
    // 8.14.4
    
    static procedure GenerateMipmap(target: TextureBindTarget);
    external 'opengl32.dll' name 'glGenerateMipmap';
    
    static procedure GenerateTextureMipmap(texture: TextureName);
    external 'opengl32.dll' name 'glGenerateTextureMipmap';
    
    {$endregion 8.14 - Texture Minification}
    
    {$region 8.18 - Texture Views}
    
    static procedure TextureView(texture: TextureName; target: TextureBindTarget; origtexture: TextureName; internalformat: InternalDataFormat; minlevel: UInt32; numlevels: UInt32; minlayer: UInt32; numlayers: UInt32);
    external 'opengl32.dll' name 'glTextureView';
    
    {$endregion 8.18 - Texture Views}
    
    {$region 8.19 - Immutable-Format Texture Images}
    
    static procedure TexStorage1D(target: TextureBindTarget; levels: Int32; internalformat: InternalDataFormat; width: Int32);
    external 'opengl32.dll' name 'glTexStorage1D';
    
    static procedure TextureStorage1D(texture: TextureName; levels: Int32; internalformat: InternalDataFormat; width: Int32);
    external 'opengl32.dll' name 'glTextureStorage1D';
    
    static procedure TexStorage2D(target: TextureBindTarget; levels: Int32; internalformat: InternalDataFormat; width: Int32; height: Int32);
    external 'opengl32.dll' name 'glTexStorage2D';
    
    static procedure TextureStorage2D(texture: TextureName; levels: Int32; internalformat: InternalDataFormat; width: Int32; height: Int32);
    external 'opengl32.dll' name 'glTextureStorage2D';
    
    static procedure TexStorage3D(target: TextureBindTarget; levels: Int32; internalformat: InternalDataFormat; width: Int32; height: Int32; depth: Int32);
    external 'opengl32.dll' name 'glTexStorage3D';
    
    static procedure TextureStorage3D(texture: TextureName; levels: Int32; internalformat: InternalDataFormat; width: Int32; height: Int32; depth: Int32);
    external 'opengl32.dll' name 'glTextureStorage3D';
    
    static procedure TexStorage2DMultisample(target: TextureBindTarget; samples: Int32; internalformat: InternalDataFormat; width: Int32; height: Int32; fixedsamplelocations: boolean);
    external 'opengl32.dll' name 'glTexStorage2DMultisample';
    
    static procedure TextureStorage2DMultisample(texture: TextureName; samples: Int32; internalformat: InternalDataFormat; width: Int32; height: Int32; fixedsamplelocations: boolean);
    external 'opengl32.dll' name 'glTextureStorage2DMultisample';
    
    static procedure TexStorage3DMultisample(target: TextureBindTarget; samples: Int32; internalformat: InternalDataFormat; width: Int32; height: Int32; depth: Int32; fixedsamplelocations: boolean);
    external 'opengl32.dll' name 'glTexStorage3DMultisample';
    
    static procedure TextureStorage3DMultisample(texture: TextureName; samples: Int32; internalformat: InternalDataFormat; width: Int32; height: Int32; depth: Int32; fixedsamplelocations: boolean);
    external 'opengl32.dll' name 'glTextureStorage3DMultisample';
    
    {$endregion 8.19 - Immutable-Format Texture Images}
    
    {$region 8.20 - Invalidating Texture Image Data}
    
    static procedure InvalidateTexSubImage(texture: TextureName; level: Int32; xoffset: Int32; yoffset: Int32; zoffset: Int32; width: Int32; height: Int32; depth: Int32);
    external 'opengl32.dll' name 'glInvalidateTexSubImage';
    
    static procedure InvalidateTexImage(texture: TextureName; level: Int32);
    external 'opengl32.dll' name 'glInvalidateTexImage';
    
    {$endregion 8.20 - Invalidating Texture Image Data}
    
    {$region 8.21 - Clearing Texture Image Data}
    
    static procedure ClearTexSubImage(texture: TextureName; level: Int32; xoffset: Int32; yoffset: Int32; zoffset: Int32; width: Int32; height: Int32; depth: Int32; format: DataFormat; &type: DataType; data: IntPtr);
    external 'opengl32.dll' name 'glClearTexSubImage';
    static procedure ClearTexSubImage(texture: TextureName; level: Int32; xoffset: Int32; yoffset: Int32; zoffset: Int32; width: Int32; height: Int32; depth: Int32; format: DataFormat; &type: DataType; data: pointer);
    external 'opengl32.dll' name 'glClearTexSubImage';
    
    static procedure ClearTexImage(texture: TextureName; level: Int32; format: DataFormat; &type: DataType; data: IntPtr);
    external 'opengl32.dll' name 'glClearTexImage';
    static procedure ClearTexImage(texture: TextureName; level: Int32; format: DataFormat; &type: DataType; data: pointer);
    external 'opengl32.dll' name 'glClearTexImage';
    
    {$endregion 8.21 - Clearing Texture Image Data}
    
    {$region 8.26 - Texture Image Loads and Stores}
    
    static procedure BindImageTexture(&unit: TextureUnitId; texture: TextureName; level: Int32; layered: boolean; layer: Int32; access: AccessType; format: DataFormat);
    external 'opengl32.dll' name 'glBindImageTexture';
    
    static procedure BindImageTextures(first: UInt32; count: Int32; [MarshalAs(UnmanagedType.LPArray)] textures: array of TextureName);
    external 'opengl32.dll' name 'glBindImageTextures';
    static procedure BindImageTextures(first: UInt32; count: Int32; var textures: TextureName);
    external 'opengl32.dll' name 'glBindImageTextures';
    static procedure BindImageTextures(first: UInt32; count: Int32; textures: pointer);
    external 'opengl32.dll' name 'glBindImageTextures';
    
    {$endregion 8.26 - Texture Image Loads and Stores}
    
    {$endregion 8.0 - Textures and Samplers}
    
    {$region 9.0 - Framebuffers and Framebuffer Objects}
    
    {$region 9.2 - Binding and Managing Framebuffer Objects}
    
    static procedure BindFramebuffer(target: FramebufferBindTarget; framebuffer: FramebufferName);
    external 'opengl32.dll' name 'glBindFramebuffer';
    
    static procedure CreateFramebuffers(n: Int32; [MarshalAs(UnmanagedType.LPArray)] framebuffers: array of FramebufferName);
    external 'opengl32.dll' name 'glCreateFramebuffers';
    static procedure CreateFramebuffers(n: Int32; var framebuffers: FramebufferName);
    external 'opengl32.dll' name 'glCreateFramebuffers';
    static procedure CreateFramebuffers(n: Int32; framebuffers: pointer);
    external 'opengl32.dll' name 'glCreateFramebuffers';
    
    static procedure GenFramebuffers(n: Int32; [MarshalAs(UnmanagedType.LPArray)] framebuffers: array of FramebufferName);
    external 'opengl32.dll' name 'glGenFramebuffers';
    static procedure GenFramebuffers(n: Int32; var framebuffers: FramebufferName);
    external 'opengl32.dll' name 'glGenFramebuffers';
    static procedure GenFramebuffers(n: Int32; framebuffers: pointer);
    external 'opengl32.dll' name 'glGenFramebuffers';
    
    static procedure DeleteFramebuffers(n: Int32; [MarshalAs(UnmanagedType.LPArray)] framebuffers: array of FramebufferName);
    external 'opengl32.dll' name 'glDeleteFramebuffers';
    static procedure DeleteFramebuffers(n: Int32; var framebuffers: FramebufferName);
    external 'opengl32.dll' name 'glDeleteFramebuffers';
    static procedure DeleteFramebuffers(n: Int32; framebuffers: pointer);
    external 'opengl32.dll' name 'glDeleteFramebuffers';
    
    static function IsFramebuffer(framebuffer: FramebufferName): boolean;
    external 'opengl32.dll' name 'glIsFramebuffer';
    
    // 9.2.1
    
    static procedure FramebufferParameteri(target: FramebufferBindTarget; pname: FramebufferInfoType; param: Int32);
    external 'opengl32.dll' name 'glFramebufferParameteri';
    
    static procedure NamedFramebufferParameteri(framebuffer: FramebufferName; pname: FramebufferInfoType; param: Int32);
    external 'opengl32.dll' name 'glNamedFramebufferParameteri';
    
    // 9.2.3
    
    static procedure GetFramebufferParameteriv(target: FramebufferBindTarget; pname: FramebufferInfoType; var &params: Int32);
    external 'opengl32.dll' name 'glGetFramebufferParameteriv';
    static procedure GetFramebufferParameteriv(target: FramebufferBindTarget; pname: FramebufferInfoType; &params: pointer);
    external 'opengl32.dll' name 'glGetFramebufferParameteriv';
    
    static procedure GetNamedFramebufferParameteriv(framebuffer: FramebufferName; pname: FramebufferInfoType; var param: Int32);
    external 'opengl32.dll' name 'glGetNamedFramebufferParameteriv';
    static procedure GetNamedFramebufferParameteriv(framebuffer: FramebufferName; pname: FramebufferInfoType; param: pointer);
    external 'opengl32.dll' name 'glGetNamedFramebufferParameteriv';
    
    static procedure GetFramebufferAttachmentParameteriv(target: FramebufferBindTarget; attachment: FramebufferAttachmentPoint; pname: FramebufferAttachmentInfoType; var &params: FramebufferAttachmentObjectType);
    external 'opengl32.dll' name 'glGetFramebufferAttachmentParameteriv';
    static procedure GetFramebufferAttachmentParameteriv(target: FramebufferBindTarget; attachment: FramebufferAttachmentPoint; pname: FramebufferAttachmentInfoType; var &params: UInt32);
    external 'opengl32.dll' name 'glGetFramebufferAttachmentParameteriv';
    static procedure GetFramebufferAttachmentParameteriv(target: FramebufferBindTarget; attachment: FramebufferAttachmentPoint; pname: FramebufferAttachmentInfoType; var &params: DataType);
    external 'opengl32.dll' name 'glGetFramebufferAttachmentParameteriv';
    static procedure GetFramebufferAttachmentParameteriv(target: FramebufferBindTarget; attachment: FramebufferAttachmentPoint; pname: FramebufferAttachmentInfoType; var &params: ColorEncodingMode);
    external 'opengl32.dll' name 'glGetFramebufferAttachmentParameteriv';
    static procedure GetFramebufferAttachmentParameteriv(target: FramebufferBindTarget; attachment: FramebufferAttachmentPoint; pname: FramebufferAttachmentInfoType; var &params: Int32);
    external 'opengl32.dll' name 'glGetFramebufferAttachmentParameteriv';
    static procedure GetFramebufferAttachmentParameteriv(target: FramebufferBindTarget; attachment: FramebufferAttachmentPoint; pname: FramebufferAttachmentInfoType; &params: pointer);
    external 'opengl32.dll' name 'glGetFramebufferAttachmentParameteriv';
    
    static procedure GetNamedFramebufferAttachmentParameteriv(framebuffer: FramebufferName; attachment: FramebufferAttachmentPoint; pname: FramebufferAttachmentInfoType; var &params: FramebufferAttachmentObjectType);
    external 'opengl32.dll' name 'glGetNamedFramebufferAttachmentParameteriv';
    static procedure GetNamedFramebufferAttachmentParameteriv(framebuffer: FramebufferName; attachment: FramebufferAttachmentPoint; pname: FramebufferAttachmentInfoType; var &params: UInt32);
    external 'opengl32.dll' name 'glGetNamedFramebufferAttachmentParameteriv';
    static procedure GetNamedFramebufferAttachmentParameteriv(framebuffer: FramebufferName; attachment: FramebufferAttachmentPoint; pname: FramebufferAttachmentInfoType; var &params: DataType);
    external 'opengl32.dll' name 'glGetNamedFramebufferAttachmentParameteriv';
    static procedure GetNamedFramebufferAttachmentParameteriv(framebuffer: FramebufferName; attachment: FramebufferAttachmentPoint; pname: FramebufferAttachmentInfoType; var &params: ColorEncodingMode);
    external 'opengl32.dll' name 'glGetNamedFramebufferAttachmentParameteriv';
    static procedure GetNamedFramebufferAttachmentParameteriv(framebuffer: FramebufferName; attachment: FramebufferAttachmentPoint; pname: FramebufferAttachmentInfoType; var &params: Int32);
    external 'opengl32.dll' name 'glGetNamedFramebufferAttachmentParameteriv';
    static procedure GetNamedFramebufferAttachmentParameteriv(framebuffer: FramebufferName; attachment: FramebufferAttachmentPoint; pname: FramebufferAttachmentInfoType; &params: pointer);
    external 'opengl32.dll' name 'glGetNamedFramebufferAttachmentParameteriv';
    
    // 9.2.4
    
    static procedure BindRenderbuffer(target: RenderbufferBindTarget; renderbuffer: RenderbufferName);
    external 'opengl32.dll' name 'glBindRenderbuffer';
    
    static procedure CreateRenderbuffers(n: Int32; [MarshalAs(UnmanagedType.LPArray)] renderbuffers: array of RenderbufferName);
    external 'opengl32.dll' name 'glCreateRenderbuffers';
    static procedure CreateRenderbuffers(n: Int32; var renderbuffers: RenderbufferName);
    external 'opengl32.dll' name 'glCreateRenderbuffers';
    static procedure CreateRenderbuffers(n: Int32; renderbuffers: pointer);
    external 'opengl32.dll' name 'glCreateRenderbuffers';
    
    static procedure GenRenderbuffers(n: Int32; [MarshalAs(UnmanagedType.LPArray)] renderbuffers: array of RenderbufferName);
    external 'opengl32.dll' name 'glGenRenderbuffers';
    static procedure GenRenderbuffers(n: Int32; var renderbuffers: RenderbufferName);
    external 'opengl32.dll' name 'glGenRenderbuffers';
    static procedure GenRenderbuffers(n: Int32; renderbuffers: pointer);
    external 'opengl32.dll' name 'glGenRenderbuffers';
    
    static procedure DeleteRenderbuffers(n: Int32; [MarshalAs(UnmanagedType.LPArray)] renderbuffers: array of RenderbufferName);
    external 'opengl32.dll' name 'glDeleteRenderbuffers';
    static procedure DeleteRenderbuffers(n: Int32; var renderbuffers: RenderbufferName);
    external 'opengl32.dll' name 'glDeleteRenderbuffers';
    static procedure DeleteRenderbuffers(n: Int32; renderbuffers: pointer);
    external 'opengl32.dll' name 'glDeleteRenderbuffers';
    
    static function IsRenderbuffer(renderbuffer: RenderbufferName): boolean;
    external 'opengl32.dll' name 'glIsRenderbuffer';
    
    static procedure RenderbufferStorageMultisample(target: RenderbufferBindTarget; samples: Int32; internalformat: InternalDataFormat; width: Int32; height: Int32);
    external 'opengl32.dll' name 'glRenderbufferStorageMultisample';
    
    static procedure NamedRenderbufferStorageMultisample(renderbuffer: RenderbufferName; samples: Int32; internalformat: InternalDataFormat; width: Int32; height: Int32);
    external 'opengl32.dll' name 'glNamedRenderbufferStorageMultisample';
    
    static procedure RenderbufferStorage(target: RenderbufferBindTarget; internalformat: InternalDataFormat; width: Int32; height: Int32);
    external 'opengl32.dll' name 'glRenderbufferStorage';
    
    static procedure NamedRenderbufferStorage(renderbuffer: RenderbufferName; internalformat: InternalDataFormat; width: Int32; height: Int32);
    external 'opengl32.dll' name 'glNamedRenderbufferStorage';
    
    // 9.2.6
    
    static procedure GetRenderbufferParameteriv(target: RenderbufferBindTarget; pname: RenderbufferInfoType; var &params: InternalDataFormat);
    external 'opengl32.dll' name 'glGetRenderbufferParameteriv';
    static procedure GetRenderbufferParameteriv(target: RenderbufferBindTarget; pname: RenderbufferInfoType; var &params: Int32);
    external 'opengl32.dll' name 'glGetRenderbufferParameteriv';
    static procedure GetRenderbufferParameteriv(target: RenderbufferBindTarget; pname: RenderbufferInfoType; &params: pointer);
    external 'opengl32.dll' name 'glGetRenderbufferParameteriv';
    
    static procedure GetNamedRenderbufferParameteriv(renderbuffer: RenderbufferName; pname: RenderbufferInfoType; var &params: InternalDataFormat);
    external 'opengl32.dll' name 'glGetNamedRenderbufferParameteriv';
    static procedure GetNamedRenderbufferParameteriv(renderbuffer: RenderbufferName; pname: RenderbufferInfoType; var &params: Int32);
    external 'opengl32.dll' name 'glGetNamedRenderbufferParameteriv';
    static procedure GetNamedRenderbufferParameteriv(renderbuffer: RenderbufferName; pname: RenderbufferInfoType; &params: pointer);
    external 'opengl32.dll' name 'glGetNamedRenderbufferParameteriv';
    
    // 9.2.7
    
    static procedure FramebufferRenderbuffer(target: FramebufferBindTarget; attachment: FramebufferAttachmentPoint; renderbuffertarget: RenderbufferBindTarget; renderbuffer: RenderbufferName);
    external 'opengl32.dll' name 'glFramebufferRenderbuffer';
    
    static procedure NamedFramebufferRenderbuffer(framebuffer: FramebufferName; attachment: FramebufferAttachmentPoint; renderbuffertarget: RenderbufferBindTarget; renderbuffer: RenderbufferName);
    external 'opengl32.dll' name 'glNamedFramebufferRenderbuffer';
    
    // 9.2.8
    
    static procedure FramebufferTexture(target: FramebufferBindTarget; attachment: FramebufferAttachmentPoint; texture: TextureName; level: Int32);
    external 'opengl32.dll' name 'glFramebufferTexture';
    
    static procedure NamedFramebufferTexture(framebuffer: FramebufferName; attachment: FramebufferAttachmentPoint; texture: TextureName; level: Int32);
    external 'opengl32.dll' name 'glNamedFramebufferTexture';
    
    static procedure FramebufferTexture1D(target: FramebufferBindTarget; attachment: FramebufferAttachmentPoint; textarget: TextureBindTarget; texture: TextureName; level: Int32);
    external 'opengl32.dll' name 'glFramebufferTexture1D';
    static procedure FramebufferTexture1D(target: FramebufferBindTarget; attachment: FramebufferAttachmentPoint; textarget: TextureCubeSide; texture: TextureName; level: Int32);
    external 'opengl32.dll' name 'glFramebufferTexture1D';
    
    static procedure FramebufferTexture2D(target: FramebufferBindTarget; attachment: FramebufferAttachmentPoint; textarget: TextureBindTarget; texture: TextureName; level: Int32);
    external 'opengl32.dll' name 'glFramebufferTexture2D';
    static procedure FramebufferTexture2D(target: FramebufferBindTarget; attachment: FramebufferAttachmentPoint; textarget: TextureCubeSide; texture: TextureName; level: Int32);
    external 'opengl32.dll' name 'glFramebufferTexture2D';
    
    static procedure FramebufferTexture3D(target: FramebufferBindTarget; attachment: FramebufferAttachmentPoint; textarget: TextureBindTarget; texture: TextureName; level: Int32; zoffset: Int32);
    external 'opengl32.dll' name 'glFramebufferTexture3D';
    static procedure FramebufferTexture3D(target: FramebufferBindTarget; attachment: FramebufferAttachmentPoint; textarget: TextureCubeSide; texture: TextureName; level: Int32; zoffset: Int32);
    external 'opengl32.dll' name 'glFramebufferTexture3D';
    
    static procedure FramebufferTextureLayer(target: FramebufferBindTarget; attachment: FramebufferAttachmentPoint; texture: TextureName; level: Int32; layer: Int32);
    external 'opengl32.dll' name 'glFramebufferTextureLayer';
    
    static procedure NamedFramebufferTextureLayer(framebuffer: FramebufferName; attachment: FramebufferAttachmentPoint; texture: TextureName; level: Int32; layer: Int32);
    external 'opengl32.dll' name 'glNamedFramebufferTextureLayer';
    
    {$endregion 9.2 - Binding and Managing Framebuffer Objects}
    
    {$region 9.3 - Feedback Loops Between Textures and the Framebuffer}
    
    // 9.3.1
    
    static procedure TextureBarrier;
    external 'opengl32.dll' name 'glTextureBarrier';
    
    {$endregion 9.3 - Feedback Loops Between Textures and the Framebuffer}
    
    {$region 9.4 - Framebuffer Completeness}
    
    // 9.4.2
    
    static function CheckFramebufferStatus(target: FramebufferBindTarget): ErrorCode;
    external 'opengl32.dll' name 'glCheckFramebufferStatus';
    
    static function CheckNamedFramebufferStatus(framebuffer: FramebufferName; target: FramebufferBindTarget): ErrorCode;
    external 'opengl32.dll' name 'glCheckNamedFramebufferStatus';
    
    {$endregion 9.4 - Framebuffer Completeness}
    
    {$endregion 9.0 - Framebuffers and Framebuffer Objects}
    
    {$region 10.0 - Vertex Specification and Drawing Commands}
    
    {$region 10.1 - Primitive Types}
    
    // 10.1.15
    
    static procedure PatchParameteri(pname: PatchMode; value: Int32);
    external 'opengl32.dll' name 'glPatchParameteri';
    
    {$endregion 10.1 - Primitive Types}
    
    {$region 10.2 - Current Vertex Attribute Values}
    
    // 10.2.1
    
    {$region VertexAttrib[1,2,3,4][s,f,d]}
    
    static procedure VertexAttrib1s(index: UInt32; x: Int16);
    external 'opengl32.dll' name 'glVertexAttrib1s';
    
    static procedure VertexAttrib2s(index: UInt32; x: Int16; y: Int16);
    external 'opengl32.dll' name 'glVertexAttrib2s';
    
    static procedure VertexAttrib3s(index: UInt32; x: Int16; y: Int16; z: Int16);
    external 'opengl32.dll' name 'glVertexAttrib3s';
    
    static procedure VertexAttrib4s(index: UInt32; x: Int16; y: Int16; z: Int16; w: Int16);
    external 'opengl32.dll' name 'glVertexAttrib4s';
    
    static procedure VertexAttrib1f(index: UInt32; x: single);
    external 'opengl32.dll' name 'glVertexAttrib1f';
    
    static procedure VertexAttrib2f(index: UInt32; x: single; y: single);
    external 'opengl32.dll' name 'glVertexAttrib2f';
    
    static procedure VertexAttrib3f(index: UInt32; x: single; y: single; z: single);
    external 'opengl32.dll' name 'glVertexAttrib3f';
    
    static procedure VertexAttrib4f(index: UInt32; x: single; y: single; z: single; w: single);
    external 'opengl32.dll' name 'glVertexAttrib4f';
    
    static procedure VertexAttrib1d(index: UInt32; x: real);
    external 'opengl32.dll' name 'glVertexAttrib1d';
    
    static procedure VertexAttrib2d(index: UInt32; x: real; y: real);
    external 'opengl32.dll' name 'glVertexAttrib2d';
    
    static procedure VertexAttrib3d(index: UInt32; x: real; y: real; z: real);
    external 'opengl32.dll' name 'glVertexAttrib3d';
    
    static procedure VertexAttrib4d(index: UInt32; x: real; y: real; z: real; w: real);
    external 'opengl32.dll' name 'glVertexAttrib4d';
    
    {$endregion VertexAttrib[1,2,3,4][s,f,d]}
    
    {$region VertexAttrib[1,2,3][s,f,d]v}
    
    static procedure VertexAttrib1sv(index: UInt32; [MarshalAs(UnmanagedType.LPArray)] v: array of Int16);
    external 'opengl32.dll' name 'glVertexAttrib1sv';
    static procedure VertexAttrib1sv(index: UInt32; var v: Int16);
    external 'opengl32.dll' name 'glVertexAttrib1sv';
    static procedure VertexAttrib1sv(index: UInt32; var v: Vec1s);
    external 'opengl32.dll' name 'glVertexAttrib1sv';
    static procedure VertexAttrib1sv(index: UInt32; v: pointer);
    external 'opengl32.dll' name 'glVertexAttrib1sv';
    
    static procedure VertexAttrib2sv(index: UInt32; [MarshalAs(UnmanagedType.LPArray)] v: array of Int16);
    external 'opengl32.dll' name 'glVertexAttrib2sv';
    static procedure VertexAttrib2sv(index: UInt32; var v: Int16);
    external 'opengl32.dll' name 'glVertexAttrib2sv';
    static procedure VertexAttrib2sv(index: UInt32; var v: Vec2s);
    external 'opengl32.dll' name 'glVertexAttrib2sv';
    static procedure VertexAttrib2sv(index: UInt32; v: pointer);
    external 'opengl32.dll' name 'glVertexAttrib2sv';
    
    static procedure VertexAttrib3sv(index: UInt32; [MarshalAs(UnmanagedType.LPArray)] v: array of Int16);
    external 'opengl32.dll' name 'glVertexAttrib3sv';
    static procedure VertexAttrib3sv(index: UInt32; var v: Int16);
    external 'opengl32.dll' name 'glVertexAttrib3sv';
    static procedure VertexAttrib3sv(index: UInt32; var v: Vec3s);
    external 'opengl32.dll' name 'glVertexAttrib3sv';
    static procedure VertexAttrib3sv(index: UInt32; v: pointer);
    external 'opengl32.dll' name 'glVertexAttrib3sv';
    
    static procedure VertexAttrib1fv(index: UInt32; [MarshalAs(UnmanagedType.LPArray)] v: array of single);
    external 'opengl32.dll' name 'glVertexAttrib1fv';
    static procedure VertexAttrib1fv(index: UInt32; var v: single);
    external 'opengl32.dll' name 'glVertexAttrib1fv';
    static procedure VertexAttrib1fv(index: UInt32; var v: Vec1f);
    external 'opengl32.dll' name 'glVertexAttrib1fv';
    static procedure VertexAttrib1fv(index: UInt32; v: pointer);
    external 'opengl32.dll' name 'glVertexAttrib1fv';
    
    static procedure VertexAttrib2fv(index: UInt32; [MarshalAs(UnmanagedType.LPArray)] v: array of single);
    external 'opengl32.dll' name 'glVertexAttrib2fv';
    static procedure VertexAttrib2fv(index: UInt32; var v: single);
    external 'opengl32.dll' name 'glVertexAttrib2fv';
    static procedure VertexAttrib2fv(index: UInt32; var v: Vec2f);
    external 'opengl32.dll' name 'glVertexAttrib2fv';
    static procedure VertexAttrib2fv(index: UInt32; v: pointer);
    external 'opengl32.dll' name 'glVertexAttrib2fv';
    
    static procedure VertexAttrib3fv(index: UInt32; [MarshalAs(UnmanagedType.LPArray)] v: array of single);
    external 'opengl32.dll' name 'glVertexAttrib3fv';
    static procedure VertexAttrib3fv(index: UInt32; var v: single);
    external 'opengl32.dll' name 'glVertexAttrib3fv';
    static procedure VertexAttrib3fv(index: UInt32; var v: Vec3f);
    external 'opengl32.dll' name 'glVertexAttrib3fv';
    static procedure VertexAttrib3fv(index: UInt32; v: pointer);
    external 'opengl32.dll' name 'glVertexAttrib3fv';
    
    static procedure VertexAttrib1dv(index: UInt32; [MarshalAs(UnmanagedType.LPArray)] v: array of real);
    external 'opengl32.dll' name 'glVertexAttrib1dv';
    static procedure VertexAttrib1dv(index: UInt32; var v: real);
    external 'opengl32.dll' name 'glVertexAttrib1dv';
    static procedure VertexAttrib1dv(index: UInt32; var v: Vec1d);
    external 'opengl32.dll' name 'glVertexAttrib1dv';
    static procedure VertexAttrib1dv(index: UInt32; v: pointer);
    external 'opengl32.dll' name 'glVertexAttrib1dv';
    
    static procedure VertexAttrib2dv(index: UInt32; [MarshalAs(UnmanagedType.LPArray)] v: array of real);
    external 'opengl32.dll' name 'glVertexAttrib2dv';
    static procedure VertexAttrib2dv(index: UInt32; var v: real);
    external 'opengl32.dll' name 'glVertexAttrib2dv';
    static procedure VertexAttrib2dv(index: UInt32; var v: Vec2d);
    external 'opengl32.dll' name 'glVertexAttrib2dv';
    static procedure VertexAttrib2dv(index: UInt32; v: pointer);
    external 'opengl32.dll' name 'glVertexAttrib2dv';
    
    static procedure VertexAttrib3dv(index: UInt32; [MarshalAs(UnmanagedType.LPArray)] v: array of real);
    external 'opengl32.dll' name 'glVertexAttrib3dv';
    static procedure VertexAttrib3dv(index: UInt32; var v: real);
    external 'opengl32.dll' name 'glVertexAttrib3dv';
    static procedure VertexAttrib3dv(index: UInt32; var v: Vec3d);
    external 'opengl32.dll' name 'glVertexAttrib3dv';
    static procedure VertexAttrib3dv(index: UInt32; v: pointer);
    external 'opengl32.dll' name 'glVertexAttrib3dv';
    
    {$endregion VertexAttrib[1,2,3][s,f,d]v}
    
    {$region VertexAttrib4[b,s,i,f,d,ub,us,ui]v}
    
    static procedure VertexAttrib4bv(index: UInt32; [MarshalAs(UnmanagedType.LPArray)] v: array of SByte);
    external 'opengl32.dll' name 'glVertexAttrib4bv';
    static procedure VertexAttrib4bv(index: UInt32; var v: SByte);
    external 'opengl32.dll' name 'glVertexAttrib4bv';
    static procedure VertexAttrib4bv(index: UInt32; var v: Vec4b);
    external 'opengl32.dll' name 'glVertexAttrib4bv';
    static procedure VertexAttrib4bv(index: UInt32; v: pointer);
    external 'opengl32.dll' name 'glVertexAttrib4bv';
    
    static procedure VertexAttrib4sv(index: UInt32; [MarshalAs(UnmanagedType.LPArray)] v: array of Int16);
    external 'opengl32.dll' name 'glVertexAttrib4sv';
    static procedure VertexAttrib4sv(index: UInt32; var v: Int16);
    external 'opengl32.dll' name 'glVertexAttrib4sv';
    static procedure VertexAttrib4sv(index: UInt32; var v: Vec4s);
    external 'opengl32.dll' name 'glVertexAttrib4sv';
    static procedure VertexAttrib4sv(index: UInt32; v: pointer);
    external 'opengl32.dll' name 'glVertexAttrib4sv';
    
    static procedure VertexAttrib4iv(index: UInt32; [MarshalAs(UnmanagedType.LPArray)] v: array of Int32);
    external 'opengl32.dll' name 'glVertexAttrib4iv';
    static procedure VertexAttrib4iv(index: UInt32; var v: Int32);
    external 'opengl32.dll' name 'glVertexAttrib4iv';
    static procedure VertexAttrib4iv(index: UInt32; var v: Vec4i);
    external 'opengl32.dll' name 'glVertexAttrib4iv';
    static procedure VertexAttrib4iv(index: UInt32; v: pointer);
    external 'opengl32.dll' name 'glVertexAttrib4iv';
    
    static procedure VertexAttrib4fv(index: UInt32; [MarshalAs(UnmanagedType.LPArray)] v: array of single);
    external 'opengl32.dll' name 'glVertexAttrib4fv';
    static procedure VertexAttrib4fv(index: UInt32; var v: single);
    external 'opengl32.dll' name 'glVertexAttrib4fv';
    static procedure VertexAttrib4fv(index: UInt32; var v: Vec4f);
    external 'opengl32.dll' name 'glVertexAttrib4fv';
    static procedure VertexAttrib4fv(index: UInt32; v: pointer);
    external 'opengl32.dll' name 'glVertexAttrib4fv';
    
    static procedure VertexAttrib4dv(index: UInt32; [MarshalAs(UnmanagedType.LPArray)] v: array of real);
    external 'opengl32.dll' name 'glVertexAttrib4dv';
    static procedure VertexAttrib4dv(index: UInt32; var v: real);
    external 'opengl32.dll' name 'glVertexAttrib4dv';
    static procedure VertexAttrib4dv(index: UInt32; var v: Vec4d);
    external 'opengl32.dll' name 'glVertexAttrib4dv';
    static procedure VertexAttrib4dv(index: UInt32; v: pointer);
    external 'opengl32.dll' name 'glVertexAttrib4dv';
    
    static procedure VertexAttrib4ubv(index: UInt32; [MarshalAs(UnmanagedType.LPArray)] v: array of Byte);
    external 'opengl32.dll' name 'glVertexAttrib4ubv';
    static procedure VertexAttrib4ubv(index: UInt32; var v: Byte);
    external 'opengl32.dll' name 'glVertexAttrib4ubv';
    static procedure VertexAttrib4ubv(index: UInt32; var v: Vec4ub);
    external 'opengl32.dll' name 'glVertexAttrib4ubv';
    static procedure VertexAttrib4ubv(index: UInt32; v: pointer);
    external 'opengl32.dll' name 'glVertexAttrib4ubv';
    
    static procedure VertexAttrib4usv(index: UInt32; [MarshalAs(UnmanagedType.LPArray)] v: array of UInt16);
    external 'opengl32.dll' name 'glVertexAttrib4usv';
    static procedure VertexAttrib4usv(index: UInt32; var v: UInt16);
    external 'opengl32.dll' name 'glVertexAttrib4usv';
    static procedure VertexAttrib4usv(index: UInt32; var v: Vec4us);
    external 'opengl32.dll' name 'glVertexAttrib4usv';
    static procedure VertexAttrib4usv(index: UInt32; v: pointer);
    external 'opengl32.dll' name 'glVertexAttrib4usv';
    
    static procedure VertexAttrib4uiv(index: UInt32; [MarshalAs(UnmanagedType.LPArray)] v: array of UInt32);
    external 'opengl32.dll' name 'glVertexAttrib4uiv';
    static procedure VertexAttrib4uiv(index: UInt32; var v: UInt32);
    external 'opengl32.dll' name 'glVertexAttrib4uiv';
    static procedure VertexAttrib4uiv(index: UInt32; var v: Vec4ui);
    external 'opengl32.dll' name 'glVertexAttrib4uiv';
    static procedure VertexAttrib4uiv(index: UInt32; v: pointer);
    external 'opengl32.dll' name 'glVertexAttrib4uiv';
    
    {$endregion VertexAttrib4[b,s,i,f,d,ub,us,ui]v}
    
    {$region VertexAttrib4Nub}
    
    static procedure VertexAttrib4Nub(index: UInt32; x: Byte; y: Byte; z: Byte; w: Byte);
    external 'opengl32.dll' name 'glVertexAttrib4Nub';
    
    {$endregion VertexAttrib4Nub}
    
    {$region VertexAttrib4N[b,s,i,ub,us,ui]v}
    
    static procedure VertexAttrib4Nbv(index: UInt32; [MarshalAs(UnmanagedType.LPArray)] v: array of SByte);
    external 'opengl32.dll' name 'glVertexAttrib4Nbv';
    static procedure VertexAttrib4Nbv(index: UInt32; var v: SByte);
    external 'opengl32.dll' name 'glVertexAttrib4Nbv';
    static procedure VertexAttrib4Nbv(index: UInt32; var v: Vec4b);
    external 'opengl32.dll' name 'glVertexAttrib4Nbv';
    static procedure VertexAttrib4Nbv(index: UInt32; v: pointer);
    external 'opengl32.dll' name 'glVertexAttrib4Nbv';
    
    static procedure VertexAttrib4Nsv(index: UInt32; [MarshalAs(UnmanagedType.LPArray)] v: array of Int16);
    external 'opengl32.dll' name 'glVertexAttrib4Nsv';
    static procedure VertexAttrib4Nsv(index: UInt32; var v: Int16);
    external 'opengl32.dll' name 'glVertexAttrib4Nsv';
    static procedure VertexAttrib4Nsv(index: UInt32; var v: Vec4s);
    external 'opengl32.dll' name 'glVertexAttrib4Nsv';
    static procedure VertexAttrib4Nsv(index: UInt32; v: pointer);
    external 'opengl32.dll' name 'glVertexAttrib4Nsv';
    
    static procedure VertexAttrib4Niv(index: UInt32; [MarshalAs(UnmanagedType.LPArray)] v: array of Int32);
    external 'opengl32.dll' name 'glVertexAttrib4Niv';
    static procedure VertexAttrib4Niv(index: UInt32; var v: Int32);
    external 'opengl32.dll' name 'glVertexAttrib4Niv';
    static procedure VertexAttrib4Niv(index: UInt32; var v: Vec4i);
    external 'opengl32.dll' name 'glVertexAttrib4Niv';
    static procedure VertexAttrib4Niv(index: UInt32; v: pointer);
    external 'opengl32.dll' name 'glVertexAttrib4Niv';
    
    static procedure VertexAttrib4Nubv(index: UInt32; [MarshalAs(UnmanagedType.LPArray)] v: array of Byte);
    external 'opengl32.dll' name 'glVertexAttrib4Nubv';
    static procedure VertexAttrib4Nubv(index: UInt32; var v: Byte);
    external 'opengl32.dll' name 'glVertexAttrib4Nubv';
    static procedure VertexAttrib4Nubv(index: UInt32; var v: Vec4ub);
    external 'opengl32.dll' name 'glVertexAttrib4Nubv';
    static procedure VertexAttrib4Nubv(index: UInt32; v: pointer);
    external 'opengl32.dll' name 'glVertexAttrib4Nubv';
    
    static procedure VertexAttrib4Nusv(index: UInt32; [MarshalAs(UnmanagedType.LPArray)] v: array of UInt16);
    external 'opengl32.dll' name 'glVertexAttrib4Nusv';
    static procedure VertexAttrib4Nusv(index: UInt32; var v: UInt16);
    external 'opengl32.dll' name 'glVertexAttrib4Nusv';
    static procedure VertexAttrib4Nusv(index: UInt32; var v: Vec4us);
    external 'opengl32.dll' name 'glVertexAttrib4Nusv';
    static procedure VertexAttrib4Nusv(index: UInt32; v: pointer);
    external 'opengl32.dll' name 'glVertexAttrib4Nusv';
    
    static procedure VertexAttrib4Nuiv(index: UInt32; [MarshalAs(UnmanagedType.LPArray)] v: array of UInt32);
    external 'opengl32.dll' name 'glVertexAttrib4Nuiv';
    static procedure VertexAttrib4Nuiv(index: UInt32; var v: UInt32);
    external 'opengl32.dll' name 'glVertexAttrib4Nuiv';
    static procedure VertexAttrib4Nuiv(index: UInt32; var v: Vec4ui);
    external 'opengl32.dll' name 'glVertexAttrib4Nuiv';
    static procedure VertexAttrib4Nuiv(index: UInt32; v: pointer);
    external 'opengl32.dll' name 'glVertexAttrib4Nuiv';
    
    {$endregion VertexAttrib4N[b,s,i,ub,us,ui]v}
    
    {$region VertexAttribI[1,2,3,4][i,ui]}
    
    static procedure VertexAttribI1i(index: UInt32; x: Int32);
    external 'opengl32.dll' name 'glVertexAttribI1i';
    
    static procedure VertexAttribI2i(index: UInt32; x: Int32; y: Int32);
    external 'opengl32.dll' name 'glVertexAttribI2i';
    
    static procedure VertexAttribI3i(index: UInt32; x: Int32; y: Int32; z: Int32);
    external 'opengl32.dll' name 'glVertexAttribI3i';
    
    static procedure VertexAttribI4i(index: UInt32; x: Int32; y: Int32; z: Int32; w: Int32);
    external 'opengl32.dll' name 'glVertexAttribI4i';
    
    static procedure VertexAttribI1ui(index: UInt32; x: UInt32);
    external 'opengl32.dll' name 'glVertexAttribI1ui';
    
    static procedure VertexAttribI2ui(index: UInt32; x: UInt32; y: UInt32);
    external 'opengl32.dll' name 'glVertexAttribI2ui';
    
    static procedure VertexAttribI3ui(index: UInt32; x: UInt32; y: UInt32; z: UInt32);
    external 'opengl32.dll' name 'glVertexAttribI3ui';
    
    static procedure VertexAttribI4ui(index: UInt32; x: UInt32; y: UInt32; z: UInt32; w: UInt32);
    external 'opengl32.dll' name 'glVertexAttribI4ui';
    
    {$endregion VertexAttribI[1,2,3,4][i,ui]}
    
    {$region VertexAttribI[1,2,3,4][i,ui]v}
    
    static procedure VertexAttribI1iv(index: UInt32; [MarshalAs(UnmanagedType.LPArray)] v: array of Int32);
    external 'opengl32.dll' name 'glVertexAttribI1iv';
    static procedure VertexAttribI1iv(index: UInt32; var v: Int32);
    external 'opengl32.dll' name 'glVertexAttribI1iv';
    static procedure VertexAttribI1iv(index: UInt32; var v: Vec1i);
    external 'opengl32.dll' name 'glVertexAttribI1iv';
    static procedure VertexAttribI1iv(index: UInt32; v: pointer);
    external 'opengl32.dll' name 'glVertexAttribI1iv';
    
    static procedure VertexAttribI2iv(index: UInt32; [MarshalAs(UnmanagedType.LPArray)] v: array of Int32);
    external 'opengl32.dll' name 'glVertexAttribI2iv';
    static procedure VertexAttribI2iv(index: UInt32; var v: Int32);
    external 'opengl32.dll' name 'glVertexAttribI2iv';
    static procedure VertexAttribI2iv(index: UInt32; var v: Vec2i);
    external 'opengl32.dll' name 'glVertexAttribI2iv';
    static procedure VertexAttribI2iv(index: UInt32; v: pointer);
    external 'opengl32.dll' name 'glVertexAttribI2iv';
    
    static procedure VertexAttribI3iv(index: UInt32; [MarshalAs(UnmanagedType.LPArray)] v: array of Int32);
    external 'opengl32.dll' name 'glVertexAttribI3iv';
    static procedure VertexAttribI3iv(index: UInt32; var v: Int32);
    external 'opengl32.dll' name 'glVertexAttribI3iv';
    static procedure VertexAttribI3iv(index: UInt32; var v: Vec3i);
    external 'opengl32.dll' name 'glVertexAttribI3iv';
    static procedure VertexAttribI3iv(index: UInt32; v: pointer);
    external 'opengl32.dll' name 'glVertexAttribI3iv';
    
    static procedure VertexAttribI4iv(index: UInt32; [MarshalAs(UnmanagedType.LPArray)] v: array of Int32);
    external 'opengl32.dll' name 'glVertexAttribI4iv';
    static procedure VertexAttribI4iv(index: UInt32; var v: Int32);
    external 'opengl32.dll' name 'glVertexAttribI4iv';
    static procedure VertexAttribI4iv(index: UInt32; var v: Vec4i);
    external 'opengl32.dll' name 'glVertexAttribI4iv';
    static procedure VertexAttribI4iv(index: UInt32; v: pointer);
    external 'opengl32.dll' name 'glVertexAttribI4iv';
    
    static procedure VertexAttribI1uiv(index: UInt32; [MarshalAs(UnmanagedType.LPArray)] v: array of UInt32);
    external 'opengl32.dll' name 'glVertexAttribI1uiv';
    static procedure VertexAttribI1uiv(index: UInt32; var v: UInt32);
    external 'opengl32.dll' name 'glVertexAttribI1uiv';
    static procedure VertexAttribI1uiv(index: UInt32; var v: Vec1ui);
    external 'opengl32.dll' name 'glVertexAttribI1uiv';
    static procedure VertexAttribI1uiv(index: UInt32; v: pointer);
    external 'opengl32.dll' name 'glVertexAttribI1uiv';
    
    static procedure VertexAttribI2uiv(index: UInt32; [MarshalAs(UnmanagedType.LPArray)] v: array of UInt32);
    external 'opengl32.dll' name 'glVertexAttribI2uiv';
    static procedure VertexAttribI2uiv(index: UInt32; var v: UInt32);
    external 'opengl32.dll' name 'glVertexAttribI2uiv';
    static procedure VertexAttribI2uiv(index: UInt32; var v: Vec2ui);
    external 'opengl32.dll' name 'glVertexAttribI2uiv';
    static procedure VertexAttribI2uiv(index: UInt32; v: pointer);
    external 'opengl32.dll' name 'glVertexAttribI2uiv';
    
    static procedure VertexAttribI3uiv(index: UInt32; [MarshalAs(UnmanagedType.LPArray)] v: array of UInt32);
    external 'opengl32.dll' name 'glVertexAttribI3uiv';
    static procedure VertexAttribI3uiv(index: UInt32; var v: UInt32);
    external 'opengl32.dll' name 'glVertexAttribI3uiv';
    static procedure VertexAttribI3uiv(index: UInt32; var v: Vec3ui);
    external 'opengl32.dll' name 'glVertexAttribI3uiv';
    static procedure VertexAttribI3uiv(index: UInt32; v: pointer);
    external 'opengl32.dll' name 'glVertexAttribI3uiv';
    
    static procedure VertexAttribI4uiv(index: UInt32; [MarshalAs(UnmanagedType.LPArray)] v: array of UInt32);
    external 'opengl32.dll' name 'glVertexAttribI4uiv';
    static procedure VertexAttribI4uiv(index: UInt32; var v: UInt32);
    external 'opengl32.dll' name 'glVertexAttribI4uiv';
    static procedure VertexAttribI4uiv(index: UInt32; var v: Vec4ui);
    external 'opengl32.dll' name 'glVertexAttribI4uiv';
    static procedure VertexAttribI4uiv(index: UInt32; v: pointer);
    external 'opengl32.dll' name 'glVertexAttribI4uiv';
    
    {$endregion VertexAttribI[1,2,3,4][i,ui]v}
    
    {$region VertexAttribI4[b,s,ub,us]v}
    
    static procedure VertexAttribI4bv(index: UInt32; [MarshalAs(UnmanagedType.LPArray)] v: array of SByte);
    external 'opengl32.dll' name 'glVertexAttribI4bv';
    static procedure VertexAttribI4bv(index: UInt32; var v: SByte);
    external 'opengl32.dll' name 'glVertexAttribI4bv';
    static procedure VertexAttribI4bv(index: UInt32; var v: Vec4b);
    external 'opengl32.dll' name 'glVertexAttribI4bv';
    static procedure VertexAttribI4bv(index: UInt32; v: pointer);
    external 'opengl32.dll' name 'glVertexAttribI4bv';
    
    static procedure VertexAttribI4sv(index: UInt32; [MarshalAs(UnmanagedType.LPArray)] v: array of Int16);
    external 'opengl32.dll' name 'glVertexAttribI4sv';
    static procedure VertexAttribI4sv(index: UInt32; var v: Int16);
    external 'opengl32.dll' name 'glVertexAttribI4sv';
    static procedure VertexAttribI4sv(index: UInt32; var v: Vec4s);
    external 'opengl32.dll' name 'glVertexAttribI4sv';
    static procedure VertexAttribI4sv(index: UInt32; v: pointer);
    external 'opengl32.dll' name 'glVertexAttribI4sv';
    
    static procedure VertexAttribI4ubv(index: UInt32; [MarshalAs(UnmanagedType.LPArray)] v: array of Byte);
    external 'opengl32.dll' name 'glVertexAttribI4ubv';
    static procedure VertexAttribI4ubv(index: UInt32; var v: Byte);
    external 'opengl32.dll' name 'glVertexAttribI4ubv';
    static procedure VertexAttribI4ubv(index: UInt32; var v: Vec4ub);
    external 'opengl32.dll' name 'glVertexAttribI4ubv';
    static procedure VertexAttribI4ubv(index: UInt32; v: pointer);
    external 'opengl32.dll' name 'glVertexAttribI4ubv';
    
    static procedure VertexAttribI4usv(index: UInt32; [MarshalAs(UnmanagedType.LPArray)] v: array of UInt16);
    external 'opengl32.dll' name 'glVertexAttribI4usv';
    static procedure VertexAttribI4usv(index: UInt32; var v: UInt16);
    external 'opengl32.dll' name 'glVertexAttribI4usv';
    static procedure VertexAttribI4usv(index: UInt32; var v: Vec4us);
    external 'opengl32.dll' name 'glVertexAttribI4usv';
    static procedure VertexAttribI4usv(index: UInt32; v: pointer);
    external 'opengl32.dll' name 'glVertexAttribI4usv';
    
    {$endregion VertexAttribI4[b,s,ub,us]v}
    
    {$region VertexAttribL[1,2,3,4]d}
    
    static procedure VertexAttribL1d(index: UInt32; x: real);
    external 'opengl32.dll' name 'glVertexAttribL1d';
    
    static procedure VertexAttribL2d(index: UInt32; x: real; y: real);
    external 'opengl32.dll' name 'glVertexAttribL2d';
    
    static procedure VertexAttribL3d(index: UInt32; x: real; y: real; z: real);
    external 'opengl32.dll' name 'glVertexAttribL3d';
    
    static procedure VertexAttribL4d(index: UInt32; x: real; y: real; z: real; w: real);
    external 'opengl32.dll' name 'glVertexAttribL4d';
    
    {$endregion VertexAttribL[1,2,3,4]d}
    
    {$region VertexAttribL[1,2,3,4]dv}
    
    static procedure VertexAttribL1dv(index: UInt32; [MarshalAs(UnmanagedType.LPArray)] v: array of real);
    external 'opengl32.dll' name 'glVertexAttribL1dv';
    static procedure VertexAttribL1dv(index: UInt32; var v: real);
    external 'opengl32.dll' name 'glVertexAttribL1dv';
    static procedure VertexAttribL1dv(index: UInt32; var v: Vec1d);
    external 'opengl32.dll' name 'glVertexAttribL1dv';
    static procedure VertexAttribL1dv(index: UInt32; v: pointer);
    external 'opengl32.dll' name 'glVertexAttribL1dv';
    
    static procedure VertexAttribL2dv(index: UInt32; [MarshalAs(UnmanagedType.LPArray)] v: array of real);
    external 'opengl32.dll' name 'glVertexAttribL2dv';
    static procedure VertexAttribL2dv(index: UInt32; var v: real);
    external 'opengl32.dll' name 'glVertexAttribL2dv';
    static procedure VertexAttribL2dv(index: UInt32; var v: Vec2d);
    external 'opengl32.dll' name 'glVertexAttribL2dv';
    static procedure VertexAttribL2dv(index: UInt32; v: pointer);
    external 'opengl32.dll' name 'glVertexAttribL2dv';
    
    static procedure VertexAttribL3dv(index: UInt32; [MarshalAs(UnmanagedType.LPArray)] v: array of real);
    external 'opengl32.dll' name 'glVertexAttribL3dv';
    static procedure VertexAttribL3dv(index: UInt32; var v: real);
    external 'opengl32.dll' name 'glVertexAttribL3dv';
    static procedure VertexAttribL3dv(index: UInt32; var v: Vec3d);
    external 'opengl32.dll' name 'glVertexAttribL3dv';
    static procedure VertexAttribL3dv(index: UInt32; v: pointer);
    external 'opengl32.dll' name 'glVertexAttribL3dv';
    
    static procedure VertexAttribL4dv(index: UInt32; [MarshalAs(UnmanagedType.LPArray)] v: array of real);
    external 'opengl32.dll' name 'glVertexAttribL4dv';
    static procedure VertexAttribL4dv(index: UInt32; var v: real);
    external 'opengl32.dll' name 'glVertexAttribL4dv';
    static procedure VertexAttribL4dv(index: UInt32; var v: Vec4d);
    external 'opengl32.dll' name 'glVertexAttribL4dv';
    static procedure VertexAttribL4dv(index: UInt32; v: pointer);
    external 'opengl32.dll' name 'glVertexAttribL4dv';
    
    {$endregion VertexAttribL[1,2,3,4]dv}
    
    {$region VertexAttribP[1,2,3,4]ui}
    
    static procedure VertexAttribP1ui(index: UInt32; &type: DataType; normalized: boolean; value: UInt32);
    external 'opengl32.dll' name 'glVertexAttribP1ui';
    
    static procedure VertexAttribP2ui(index: UInt32; &type: DataType; normalized: boolean; value: UInt32);
    external 'opengl32.dll' name 'glVertexAttribP2ui';
    
    static procedure VertexAttribP3ui(index: UInt32; &type: DataType; normalized: boolean; value: UInt32);
    external 'opengl32.dll' name 'glVertexAttribP3ui';
    
    static procedure VertexAttribP4ui(index: UInt32; &type: DataType; normalized: boolean; value: UInt32);
    external 'opengl32.dll' name 'glVertexAttribP4ui';
    
    {$endregion VertexAttribP[1,2,3,4]ui}
    
    {$region VertexAttribP[1,2,3,4]uiv}
    
    static procedure VertexAttribP1uiv(index: UInt32; &type: DataType; normalized: boolean; var value: UInt32);
    external 'opengl32.dll' name 'glVertexAttribP1uiv';
    static procedure VertexAttribP1uiv(index: UInt32; &type: DataType; normalized: boolean; value: pointer);
    external 'opengl32.dll' name 'glVertexAttribP1uiv';
    
    static procedure VertexAttribP2uiv(index: UInt32; &type: DataType; normalized: boolean; var value: UInt32);
    external 'opengl32.dll' name 'glVertexAttribP2uiv';
    static procedure VertexAttribP2uiv(index: UInt32; &type: DataType; normalized: boolean; value: pointer);
    external 'opengl32.dll' name 'glVertexAttribP2uiv';
    
    static procedure VertexAttribP3uiv(index: UInt32; &type: DataType; normalized: boolean; var value: UInt32);
    external 'opengl32.dll' name 'glVertexAttribP3uiv';
    static procedure VertexAttribP3uiv(index: UInt32; &type: DataType; normalized: boolean; value: pointer);
    external 'opengl32.dll' name 'glVertexAttribP3uiv';
    
    static procedure VertexAttribP4uiv(index: UInt32; &type: DataType; normalized: boolean; var value: UInt32);
    external 'opengl32.dll' name 'glVertexAttribP4uiv';
    static procedure VertexAttribP4uiv(index: UInt32; &type: DataType; normalized: boolean; value: pointer);
    external 'opengl32.dll' name 'glVertexAttribP4uiv';
    
    {$endregion VertexAttribP[1,2,3,4]uiv}
    
    {$endregion 10.2 - Current Vertex Attribute Values}
    
    {$region 10.3 - Vertex Arrays}
    
    // 10.3.1
    
    static procedure GenVertexArrays(n: Int32; [MarshalAs(UnmanagedType.LPArray)] arrays: array of VertexArrayName);
    external 'opengl32.dll' name 'glGenVertexArrays';
    static procedure GenVertexArrays(n: Int32; var arrays: VertexArrayName);
    external 'opengl32.dll' name 'glGenVertexArrays';
    static procedure GenVertexArrays(n: Int32; arrays: pointer);
    external 'opengl32.dll' name 'glGenVertexArrays';
    
    static procedure DeleteVertexArrays(n: Int32; [MarshalAs(UnmanagedType.LPArray)] arrays: array of VertexArrayName);
    external 'opengl32.dll' name 'glDeleteVertexArrays';
    static procedure DeleteVertexArrays(n: Int32; var arrays: VertexArrayName);
    external 'opengl32.dll' name 'glDeleteVertexArrays';
    static procedure DeleteVertexArrays(n: Int32; arrays: pointer);
    external 'opengl32.dll' name 'glDeleteVertexArrays';
    
    static procedure BindVertexArray(&array: VertexArrayName);
    external 'opengl32.dll' name 'glBindVertexArray';
    
    static procedure CreateVertexArrays(n: Int32; [MarshalAs(UnmanagedType.LPArray)] arrays: array of VertexArrayName);
    external 'opengl32.dll' name 'glCreateVertexArrays';
    static procedure CreateVertexArrays(n: Int32; var arrays: VertexArrayName);
    external 'opengl32.dll' name 'glCreateVertexArrays';
    static procedure CreateVertexArrays(n: Int32; arrays: pointer);
    external 'opengl32.dll' name 'glCreateVertexArrays';
    
    static function IsVertexArray(&array: VertexArrayName): boolean;
    external 'opengl32.dll' name 'glIsVertexArray';
    
    static procedure VertexArrayElementBuffer(vaobj: VertexArrayName; buffer: BufferName);
    external 'opengl32.dll' name 'glVertexArrayElementBuffer';
    
    // 10.3.2
    
    static procedure VertexAttribFormat(attribindex: UInt32; size: Int32; &type: DataType; normalized: boolean; relativeoffset: UInt32);
    external 'opengl32.dll' name 'glVertexAttribFormat';
    
    static procedure VertexAttribIFormat(attribindex: UInt32; size: Int32; &type: DataType; relativeoffset: UInt32);
    external 'opengl32.dll' name 'glVertexAttribIFormat';
    
    static procedure VertexAttribLFormat(attribindex: UInt32; size: Int32; &type: DataType; relativeoffset: UInt32);
    external 'opengl32.dll' name 'glVertexAttribLFormat';
    
    static procedure VertexArrayAttribFormat(vaobj: VertexArrayName; attribindex: UInt32; size: Int32; &type: DataType; normalized: boolean; relativeoffset: UInt32);
    external 'opengl32.dll' name 'glVertexArrayAttribFormat';
    
    static procedure VertexArrayAttribIFormat(vaobj: VertexArrayName; attribindex: UInt32; size: Int32; &type: DataType; relativeoffset: UInt32);
    external 'opengl32.dll' name 'glVertexArrayAttribIFormat';
    
    static procedure VertexArrayAttribLFormat(vaobj: VertexArrayName; attribindex: UInt32; size: Int32; &type: DataType; relativeoffset: UInt32);
    external 'opengl32.dll' name 'glVertexArrayAttribLFormat';
    
    static procedure BindVertexBuffer(bindingindex: UInt32; buffer: BufferName; offset: IntPtr; stride: Int32);
    external 'opengl32.dll' name 'glBindVertexBuffer';
    
    static procedure VertexArrayVertexBuffer(vaobj: VertexArrayName; bindingindex: UInt32; buffer: BufferName; offset: IntPtr; stride: Int32);
    external 'opengl32.dll' name 'glVertexArrayVertexBuffer';
    
    static procedure BindVertexBuffers(first: UInt32; count: Int32; var buffers: BufferName; [MarshalAs(UnmanagedType.LPArray)] offsets: array of IntPtr; [MarshalAs(UnmanagedType.LPArray)] strides: array of Int32);
    external 'opengl32.dll' name 'glBindVertexBuffers';
    static procedure BindVertexBuffers(first: UInt32; count: Int32; var buffers: BufferName; var offsets: IntPtr; var strides: Int32);
    external 'opengl32.dll' name 'glBindVertexBuffers';
    static procedure BindVertexBuffers(first: UInt32; count: Int32; var buffers: BufferName; offsets: pointer; strides: pointer);
    external 'opengl32.dll' name 'glBindVertexBuffers';
    static procedure BindVertexBuffers(first: UInt32; count: Int32; buffers: pointer; var offsets: IntPtr; var strides: Int32);
    external 'opengl32.dll' name 'glBindVertexBuffers';
    static procedure BindVertexBuffers(first: UInt32; count: Int32; buffers: pointer; offsets: pointer; strides: pointer);
    external 'opengl32.dll' name 'glBindVertexBuffers';
    
    static procedure VertexArrayVertexBuffers(vaobj: VertexArrayName; first: UInt32; count: Int32; var buffers: BufferName; [MarshalAs(UnmanagedType.LPArray)] offsets: array of IntPtr; [MarshalAs(UnmanagedType.LPArray)] strides: array of Int32);
    external 'opengl32.dll' name 'glVertexArrayVertexBuffers';
    static procedure VertexArrayVertexBuffers(vaobj: VertexArrayName; first: UInt32; count: Int32; var buffers: BufferName; var offsets: IntPtr; var strides: Int32);
    external 'opengl32.dll' name 'glVertexArrayVertexBuffers';
    static procedure VertexArrayVertexBuffers(vaobj: VertexArrayName; first: UInt32; count: Int32; var buffers: BufferName; offsets: pointer; strides: pointer);
    external 'opengl32.dll' name 'glVertexArrayVertexBuffers';
    static procedure VertexArrayVertexBuffers(vaobj: VertexArrayName; first: UInt32; count: Int32; buffers: pointer; var offsets: IntPtr; var strides: Int32);
    external 'opengl32.dll' name 'glVertexArrayVertexBuffers';
    static procedure VertexArrayVertexBuffers(vaobj: VertexArrayName; first: UInt32; count: Int32; buffers: pointer; offsets: pointer; strides: pointer);
    external 'opengl32.dll' name 'glVertexArrayVertexBuffers';
    
    static procedure VertexAttribBinding(attribindex: UInt32; bindingindex: UInt32);
    external 'opengl32.dll' name 'glVertexAttribBinding';
    
    static procedure VertexArrayAttribBinding(vaobj: VertexArrayName; attribindex: UInt32; bindingindex: UInt32);
    external 'opengl32.dll' name 'glVertexArrayAttribBinding';
    
    static procedure VertexAttribPointer(index: UInt32; size: Int32; &type: DataType; normalized: boolean; stride: Int32; _pointer: IntPtr);
    external 'opengl32.dll' name 'glVertexAttribPointer';
    static procedure VertexAttribPointer(index: UInt32; size: Int32; &type: DataType; normalized: boolean; stride: Int32; _pointer: pointer);
    external 'opengl32.dll' name 'glVertexAttribPointer';
    
    static procedure VertexAttribIPointer(index: UInt32; size: Int32; &type: DataType; stride: Int32; _pointer: IntPtr);
    external 'opengl32.dll' name 'glVertexAttribIPointer';
    static procedure VertexAttribIPointer(index: UInt32; size: Int32; &type: DataType; stride: Int32; _pointer: pointer);
    external 'opengl32.dll' name 'glVertexAttribIPointer';
    
    static procedure VertexAttribLPointer(index: UInt32; size: Int32; &type: DataType; stride: Int32; _pointer: IntPtr);
    external 'opengl32.dll' name 'glVertexAttribLPointer';
    static procedure VertexAttribLPointer(index: UInt32; size: Int32; &type: DataType; stride: Int32; _pointer: pointer);
    external 'opengl32.dll' name 'glVertexAttribLPointer';
    
    static procedure EnableVertexAttribArray(index: UInt32);
    external 'opengl32.dll' name 'glEnableVertexAttribArray';
    
    static procedure EnableVertexArrayAttrib(vaobj: VertexArrayName; index: UInt32);
    external 'opengl32.dll' name 'glEnableVertexArrayAttrib';
    
    static procedure DisableVertexAttribArray(index: UInt32);
    external 'opengl32.dll' name 'glDisableVertexAttribArray';
    
    static procedure DisableVertexArrayAttrib(vaobj: VertexArrayName; index: UInt32);
    external 'opengl32.dll' name 'glDisableVertexArrayAttrib';
    
    static procedure VertexBindingDivisor(bindingindex: UInt32; divisor: UInt32);
    external 'opengl32.dll' name 'glVertexBindingDivisor';
    
    static procedure VertexArrayBindingDivisor(vaobj: VertexArrayName; bindingindex: UInt32; divisor: UInt32);
    external 'opengl32.dll' name 'glVertexArrayBindingDivisor';
    
    static procedure VertexAttribDivisor(index: UInt32; divisor: UInt32);
    external 'opengl32.dll' name 'glVertexAttribDivisor';
    
    // 10.3.6
    
    static procedure Disable(cap: EnablableName);
    external 'opengl32.dll' name 'glDisable';
    
    static procedure Enable(cap: EnablableName);
    external 'opengl32.dll' name 'glEnable';
    
    static procedure PrimitiveRestartIndex(index: UInt32);
    external 'opengl32.dll' name 'glPrimitiveRestartIndex';
    
    {$endregion 10.3 - Vertex Arrays}
    
    {$region 10.4 - Drawing Commands Using Vertex Arrays}
    
    static procedure DrawArrays(mode: PrimitiveType; first: Int32; count: Int32);
    external 'opengl32.dll' name 'glDrawArrays';
    
    static procedure DrawArraysInstancedBaseInstance(mode: PrimitiveType; first: Int32; count: Int32; instancecount: Int32; baseinstance: UInt32);
    external 'opengl32.dll' name 'glDrawArraysInstancedBaseInstance';
    
    static procedure DrawArraysInstanced(mode: PrimitiveType; first: Int32; count: Int32; instancecount: Int32);
    external 'opengl32.dll' name 'glDrawArraysInstanced';
    
    static procedure DrawArraysIndirect(mode: PrimitiveType; var indirect: DrawArraysIndirectCommand);
    external 'opengl32.dll' name 'glDrawArraysIndirect';
    static procedure DrawArraysIndirect(mode: PrimitiveType; indirect: pointer);
    external 'opengl32.dll' name 'glDrawArraysIndirect';
    
    static procedure MultiDrawArrays(mode: PrimitiveType; [MarshalAs(UnmanagedType.LPArray)] first: array of Int32; [MarshalAs(UnmanagedType.LPArray)] count: array of Int32; drawcount: Int32);
    external 'opengl32.dll' name 'glMultiDrawArrays';
    static procedure MultiDrawArrays(mode: PrimitiveType; var first: Int32; var count: Int32; drawcount: Int32);
    external 'opengl32.dll' name 'glMultiDrawArrays';
    static procedure MultiDrawArrays(mode: PrimitiveType; first: pointer; count: pointer; drawcount: Int32);
    external 'opengl32.dll' name 'glMultiDrawArrays';
    
    static procedure MultiDrawArraysIndirect(mode: PrimitiveType; [MarshalAs(UnmanagedType.LPArray)] indirect: array of DrawArraysIndirectCommand; drawcount: Int32; stride: Int32);
    external 'opengl32.dll' name 'glMultiDrawArraysIndirect';
    static procedure MultiDrawArraysIndirect(mode: PrimitiveType; var indirect: DrawArraysIndirectCommand; drawcount: Int32; stride: Int32);
    external 'opengl32.dll' name 'glMultiDrawArraysIndirect';
    static procedure MultiDrawArraysIndirect(mode: PrimitiveType; indirect: pointer; drawcount: Int32; stride: Int32);
    external 'opengl32.dll' name 'glMultiDrawArraysIndirect';
    
    static procedure MultiDrawArraysIndirectCount(mode: PrimitiveType; [MarshalAs(UnmanagedType.LPArray)] indirect: array of DrawArraysIndirectCommand; drawcount: IntPtr; maxdrawcount: Int32; stride: Int32);
    external 'opengl32.dll' name 'glMultiDrawArraysIndirectCount';
    static procedure MultiDrawArraysIndirectCount(mode: PrimitiveType; var indirect: DrawArraysIndirectCommand; drawcount: IntPtr; maxdrawcount: Int32; stride: Int32);
    external 'opengl32.dll' name 'glMultiDrawArraysIndirectCount';
    static procedure MultiDrawArraysIndirectCount(mode: PrimitiveType; indirect: pointer; drawcount: IntPtr; maxdrawcount: Int32; stride: Int32);
    external 'opengl32.dll' name 'glMultiDrawArraysIndirectCount';
    
    static procedure DrawElements(mode: PrimitiveType; count: Int32; &type: DataType; [MarshalAs(UnmanagedType.LPArray)] indices: array of UInt32);
    external 'opengl32.dll' name 'glDrawElements';
    static procedure DrawElements(mode: PrimitiveType; count: Int32; &type: DataType; var indices: UInt32);
    external 'opengl32.dll' name 'glDrawElements';
    static procedure DrawElements(mode: PrimitiveType; count: Int32; &type: DataType; [MarshalAs(UnmanagedType.LPArray)] indices: array of UInt16);
    external 'opengl32.dll' name 'glDrawElements';
    static procedure DrawElements(mode: PrimitiveType; count: Int32; &type: DataType; var indices: UInt16);
    external 'opengl32.dll' name 'glDrawElements';
    static procedure DrawElements(mode: PrimitiveType; count: Int32; &type: DataType; [MarshalAs(UnmanagedType.LPArray)] indices: array of Byte);
    external 'opengl32.dll' name 'glDrawElements';
    static procedure DrawElements(mode: PrimitiveType; count: Int32; &type: DataType; var indices: Byte);
    external 'opengl32.dll' name 'glDrawElements';
    static procedure DrawElements(mode: PrimitiveType; count: Int32; &type: DataType; indices: pointer);
    external 'opengl32.dll' name 'glDrawElements';
    
    static procedure DrawElementsInstancedBaseInstance(mode: PrimitiveType; count: Int32; &type: DataType; [MarshalAs(UnmanagedType.LPArray)] indices: array of UInt32; instancecount: Int32; baseinstance: UInt32);
    external 'opengl32.dll' name 'glDrawElementsInstancedBaseInstance';
    static procedure DrawElementsInstancedBaseInstance(mode: PrimitiveType; count: Int32; &type: DataType; var indices: UInt32; instancecount: Int32; baseinstance: UInt32);
    external 'opengl32.dll' name 'glDrawElementsInstancedBaseInstance';
    static procedure DrawElementsInstancedBaseInstance(mode: PrimitiveType; count: Int32; &type: DataType; [MarshalAs(UnmanagedType.LPArray)] indices: array of UInt16; instancecount: Int32; baseinstance: UInt32);
    external 'opengl32.dll' name 'glDrawElementsInstancedBaseInstance';
    static procedure DrawElementsInstancedBaseInstance(mode: PrimitiveType; count: Int32; &type: DataType; var indices: UInt16; instancecount: Int32; baseinstance: UInt32);
    external 'opengl32.dll' name 'glDrawElementsInstancedBaseInstance';
    static procedure DrawElementsInstancedBaseInstance(mode: PrimitiveType; count: Int32; &type: DataType; [MarshalAs(UnmanagedType.LPArray)] indices: array of Byte; instancecount: Int32; baseinstance: UInt32);
    external 'opengl32.dll' name 'glDrawElementsInstancedBaseInstance';
    static procedure DrawElementsInstancedBaseInstance(mode: PrimitiveType; count: Int32; &type: DataType; var indices: Byte; instancecount: Int32; baseinstance: UInt32);
    external 'opengl32.dll' name 'glDrawElementsInstancedBaseInstance';
    static procedure DrawElementsInstancedBaseInstance(mode: PrimitiveType; count: Int32; &type: DataType; indices: pointer; instancecount: Int32; baseinstance: UInt32);
    external 'opengl32.dll' name 'glDrawElementsInstancedBaseInstance';
    
    static procedure DrawElementsInstanced(mode: PrimitiveType; count: Int32; &type: DataType; [MarshalAs(UnmanagedType.LPArray)] indices: array of UInt32; instancecount: Int32);
    external 'opengl32.dll' name 'glDrawElementsInstanced';
    static procedure DrawElementsInstanced(mode: PrimitiveType; count: Int32; &type: DataType; var indices: UInt32; instancecount: Int32);
    external 'opengl32.dll' name 'glDrawElementsInstanced';
    static procedure DrawElementsInstanced(mode: PrimitiveType; count: Int32; &type: DataType; [MarshalAs(UnmanagedType.LPArray)] indices: array of UInt16; instancecount: Int32);
    external 'opengl32.dll' name 'glDrawElementsInstanced';
    static procedure DrawElementsInstanced(mode: PrimitiveType; count: Int32; &type: DataType; var indices: UInt16; instancecount: Int32);
    external 'opengl32.dll' name 'glDrawElementsInstanced';
    static procedure DrawElementsInstanced(mode: PrimitiveType; count: Int32; &type: DataType; [MarshalAs(UnmanagedType.LPArray)] indices: array of Byte; instancecount: Int32);
    external 'opengl32.dll' name 'glDrawElementsInstanced';
    static procedure DrawElementsInstanced(mode: PrimitiveType; count: Int32; &type: DataType; var indices: Byte; instancecount: Int32);
    external 'opengl32.dll' name 'glDrawElementsInstanced';
    static procedure DrawElementsInstanced(mode: PrimitiveType; count: Int32; &type: DataType; indices: pointer; instancecount: Int32);
    external 'opengl32.dll' name 'glDrawElementsInstanced';
    
    static procedure MultiDrawElements(mode: PrimitiveType; [MarshalAs(UnmanagedType.LPArray)] count: array of Int32; &type: DataType; [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPArray)] indices: array of array of UInt32; drawcount: Int32);
    external 'opengl32.dll' name 'glMultiDrawElements';
    static procedure MultiDrawElements(mode: PrimitiveType; [MarshalAs(UnmanagedType.LPArray)] count: array of Int32; &type: DataType; [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPArray)] indices: array of array of UInt16; drawcount: Int32);
    external 'opengl32.dll' name 'glMultiDrawElements';
    static procedure MultiDrawElements(mode: PrimitiveType; [MarshalAs(UnmanagedType.LPArray)] count: array of Int32; &type: DataType; [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPArray)] indices: array of array of Byte; drawcount: Int32);
    external 'opengl32.dll' name 'glMultiDrawElements';
    static procedure MultiDrawElements(mode: PrimitiveType; [MarshalAs(UnmanagedType.LPArray)] count: array of Int32; &type: DataType; [MarshalAs(UnmanagedType.LPArray)] indices: array of IntPtr; drawcount: Int32);
    external 'opengl32.dll' name 'glMultiDrawElements';
    static procedure MultiDrawElements(mode: PrimitiveType; var count: Int32; &type: DataType; [MarshalAs(UnmanagedType.SysInt, ArraySubType = UnmanagedType.LPArray)] var indices: array of UInt32; drawcount: Int32);
    external 'opengl32.dll' name 'glMultiDrawElements';
    static procedure MultiDrawElements(mode: PrimitiveType; var count: Int32; &type: DataType; [MarshalAs(UnmanagedType.SysInt, ArraySubType = UnmanagedType.LPArray)] var indices: array of UInt16; drawcount: Int32);
    external 'opengl32.dll' name 'glMultiDrawElements';
    static procedure MultiDrawElements(mode: PrimitiveType; var count: Int32; &type: DataType; [MarshalAs(UnmanagedType.SysInt, ArraySubType = UnmanagedType.LPArray)] var indices: array of Byte; drawcount: Int32);
    external 'opengl32.dll' name 'glMultiDrawElements';
    static procedure MultiDrawElements(mode: PrimitiveType; var count: Int32; &type: DataType; var indices: IntPtr; drawcount: Int32);
    external 'opengl32.dll' name 'glMultiDrawElements';
    static procedure MultiDrawElements(mode: PrimitiveType; var count: Int32; &type: DataType; var indices: pointer; drawcount: Int32);
    external 'opengl32.dll' name 'glMultiDrawElements';
    static procedure MultiDrawElements(mode: PrimitiveType; count: ^Int32; &type: DataType; indices: ^IntPtr; drawcount: Int32);
    external 'opengl32.dll' name 'glMultiDrawElements';
    static procedure MultiDrawElements(mode: PrimitiveType; count: ^Int32; &type: DataType; indices: ^pointer; drawcount: Int32);
    external 'opengl32.dll' name 'glMultiDrawElements';
    static procedure MultiDrawElements(mode: PrimitiveType; count: pointer; &type: DataType; indices: pointer; drawcount: Int32);
    external 'opengl32.dll' name 'glMultiDrawElements';
    
    static procedure DrawRangeElements(mode: PrimitiveType; start: UInt32; &end: UInt32; count: Int32; &type: DataType; [MarshalAs(UnmanagedType.LPArray)] indices: array of UInt32);
    external 'opengl32.dll' name 'glDrawRangeElements';
    static procedure DrawRangeElements(mode: PrimitiveType; start: UInt32; &end: UInt32; count: Int32; &type: DataType; var indices: UInt32);
    external 'opengl32.dll' name 'glDrawRangeElements';
    static procedure DrawRangeElements(mode: PrimitiveType; start: UInt32; &end: UInt32; count: Int32; &type: DataType; [MarshalAs(UnmanagedType.LPArray)] indices: array of UInt16);
    external 'opengl32.dll' name 'glDrawRangeElements';
    static procedure DrawRangeElements(mode: PrimitiveType; start: UInt32; &end: UInt32; count: Int32; &type: DataType; var indices: UInt16);
    external 'opengl32.dll' name 'glDrawRangeElements';
    static procedure DrawRangeElements(mode: PrimitiveType; start: UInt32; &end: UInt32; count: Int32; &type: DataType; [MarshalAs(UnmanagedType.LPArray)] indices: array of Byte);
    external 'opengl32.dll' name 'glDrawRangeElements';
    static procedure DrawRangeElements(mode: PrimitiveType; start: UInt32; &end: UInt32; count: Int32; &type: DataType; var indices: Byte);
    external 'opengl32.dll' name 'glDrawRangeElements';
    static procedure DrawRangeElements(mode: PrimitiveType; start: UInt32; &end: UInt32; count: Int32; &type: DataType; indices: pointer);
    external 'opengl32.dll' name 'glDrawRangeElements';
    
    static procedure DrawElementsBaseVertex(mode: PrimitiveType; count: Int32; &type: DataType; [MarshalAs(UnmanagedType.LPArray)] indices: array of UInt32; basevertex: Int32);
    external 'opengl32.dll' name 'glDrawElementsBaseVertex';
    static procedure DrawElementsBaseVertex(mode: PrimitiveType; count: Int32; &type: DataType; var indices: UInt32; basevertex: Int32);
    external 'opengl32.dll' name 'glDrawElementsBaseVertex';
    static procedure DrawElementsBaseVertex(mode: PrimitiveType; count: Int32; &type: DataType; [MarshalAs(UnmanagedType.LPArray)] indices: array of UInt16; basevertex: Int32);
    external 'opengl32.dll' name 'glDrawElementsBaseVertex';
    static procedure DrawElementsBaseVertex(mode: PrimitiveType; count: Int32; &type: DataType; var indices: UInt16; basevertex: Int32);
    external 'opengl32.dll' name 'glDrawElementsBaseVertex';
    static procedure DrawElementsBaseVertex(mode: PrimitiveType; count: Int32; &type: DataType; [MarshalAs(UnmanagedType.LPArray)] indices: array of Byte; basevertex: Int32);
    external 'opengl32.dll' name 'glDrawElementsBaseVertex';
    static procedure DrawElementsBaseVertex(mode: PrimitiveType; count: Int32; &type: DataType; var indices: Byte; basevertex: Int32);
    external 'opengl32.dll' name 'glDrawElementsBaseVertex';
    static procedure DrawElementsBaseVertex(mode: PrimitiveType; count: Int32; &type: DataType; indices: pointer; basevertex: Int32);
    external 'opengl32.dll' name 'glDrawElementsBaseVertex';
    
    static procedure DrawRangeElementsBaseVertex(mode: PrimitiveType; start: UInt32; &end: UInt32; count: Int32; &type: DataType; [MarshalAs(UnmanagedType.LPArray)] indices: array of UInt32; basevertex: Int32);
    external 'opengl32.dll' name 'glDrawRangeElementsBaseVertex';
    static procedure DrawRangeElementsBaseVertex(mode: PrimitiveType; start: UInt32; &end: UInt32; count: Int32; &type: DataType; var indices: UInt32; basevertex: Int32);
    external 'opengl32.dll' name 'glDrawRangeElementsBaseVertex';
    static procedure DrawRangeElementsBaseVertex(mode: PrimitiveType; start: UInt32; &end: UInt32; count: Int32; &type: DataType; [MarshalAs(UnmanagedType.LPArray)] indices: array of UInt16; basevertex: Int32);
    external 'opengl32.dll' name 'glDrawRangeElementsBaseVertex';
    static procedure DrawRangeElementsBaseVertex(mode: PrimitiveType; start: UInt32; &end: UInt32; count: Int32; &type: DataType; var indices: UInt16; basevertex: Int32);
    external 'opengl32.dll' name 'glDrawRangeElementsBaseVertex';
    static procedure DrawRangeElementsBaseVertex(mode: PrimitiveType; start: UInt32; &end: UInt32; count: Int32; &type: DataType; [MarshalAs(UnmanagedType.LPArray)] indices: array of Byte; basevertex: Int32);
    external 'opengl32.dll' name 'glDrawRangeElementsBaseVertex';
    static procedure DrawRangeElementsBaseVertex(mode: PrimitiveType; start: UInt32; &end: UInt32; count: Int32; &type: DataType; var indices: Byte; basevertex: Int32);
    external 'opengl32.dll' name 'glDrawRangeElementsBaseVertex';
    static procedure DrawRangeElementsBaseVertex(mode: PrimitiveType; start: UInt32; &end: UInt32; count: Int32; &type: DataType; indices: pointer; basevertex: Int32);
    external 'opengl32.dll' name 'glDrawRangeElementsBaseVertex';
    
    static procedure DrawElementsInstancedBaseVertex(mode: PrimitiveType; count: Int32; &type: DataType; [MarshalAs(UnmanagedType.LPArray)] indices: array of UInt32; instancecount: Int32; basevertex: Int32);
    external 'opengl32.dll' name 'glDrawElementsInstancedBaseVertex';
    static procedure DrawElementsInstancedBaseVertex(mode: PrimitiveType; count: Int32; &type: DataType; var indices: UInt32; instancecount: Int32; basevertex: Int32);
    external 'opengl32.dll' name 'glDrawElementsInstancedBaseVertex';
    static procedure DrawElementsInstancedBaseVertex(mode: PrimitiveType; count: Int32; &type: DataType; [MarshalAs(UnmanagedType.LPArray)] indices: array of UInt16; instancecount: Int32; basevertex: Int32);
    external 'opengl32.dll' name 'glDrawElementsInstancedBaseVertex';
    static procedure DrawElementsInstancedBaseVertex(mode: PrimitiveType; count: Int32; &type: DataType; var indices: UInt16; instancecount: Int32; basevertex: Int32);
    external 'opengl32.dll' name 'glDrawElementsInstancedBaseVertex';
    static procedure DrawElementsInstancedBaseVertex(mode: PrimitiveType; count: Int32; &type: DataType; [MarshalAs(UnmanagedType.LPArray)] indices: array of Byte; instancecount: Int32; basevertex: Int32);
    external 'opengl32.dll' name 'glDrawElementsInstancedBaseVertex';
    static procedure DrawElementsInstancedBaseVertex(mode: PrimitiveType; count: Int32; &type: DataType; var indices: Byte; instancecount: Int32; basevertex: Int32);
    external 'opengl32.dll' name 'glDrawElementsInstancedBaseVertex';
    static procedure DrawElementsInstancedBaseVertex(mode: PrimitiveType; count: Int32; &type: DataType; indices: pointer; instancecount: Int32; basevertex: Int32);
    external 'opengl32.dll' name 'glDrawElementsInstancedBaseVertex';
    
    static procedure DrawElementsInstancedBaseVertexBaseInstance(mode: PrimitiveType; count: Int32; &type: DataType; [MarshalAs(UnmanagedType.LPArray)] indices: array of UInt32; instancecount: Int32; basevertex: Int32; baseinstance: UInt32);
    external 'opengl32.dll' name 'glDrawElementsInstancedBaseVertexBaseInstance';
    static procedure DrawElementsInstancedBaseVertexBaseInstance(mode: PrimitiveType; count: Int32; &type: DataType; var indices: UInt32; instancecount: Int32; basevertex: Int32; baseinstance: UInt32);
    external 'opengl32.dll' name 'glDrawElementsInstancedBaseVertexBaseInstance';
    static procedure DrawElementsInstancedBaseVertexBaseInstance(mode: PrimitiveType; count: Int32; &type: DataType; [MarshalAs(UnmanagedType.LPArray)] indices: array of UInt16; instancecount: Int32; basevertex: Int32; baseinstance: UInt32);
    external 'opengl32.dll' name 'glDrawElementsInstancedBaseVertexBaseInstance';
    static procedure DrawElementsInstancedBaseVertexBaseInstance(mode: PrimitiveType; count: Int32; &type: DataType; var indices: UInt16; instancecount: Int32; basevertex: Int32; baseinstance: UInt32);
    external 'opengl32.dll' name 'glDrawElementsInstancedBaseVertexBaseInstance';
    static procedure DrawElementsInstancedBaseVertexBaseInstance(mode: PrimitiveType; count: Int32; &type: DataType; [MarshalAs(UnmanagedType.LPArray)] indices: array of Byte; instancecount: Int32; basevertex: Int32; baseinstance: UInt32);
    external 'opengl32.dll' name 'glDrawElementsInstancedBaseVertexBaseInstance';
    static procedure DrawElementsInstancedBaseVertexBaseInstance(mode: PrimitiveType; count: Int32; &type: DataType; var indices: Byte; instancecount: Int32; basevertex: Int32; baseinstance: UInt32);
    external 'opengl32.dll' name 'glDrawElementsInstancedBaseVertexBaseInstance';
    static procedure DrawElementsInstancedBaseVertexBaseInstance(mode: PrimitiveType; count: Int32; &type: DataType; indices: pointer; instancecount: Int32; basevertex: Int32; baseinstance: UInt32);
    external 'opengl32.dll' name 'glDrawElementsInstancedBaseVertexBaseInstance';
    
    static procedure DrawElementsIndirect(mode: PrimitiveType; &type: DataType; var indirect: DrawArraysIndirectCommand);
    external 'opengl32.dll' name 'glDrawElementsIndirect';
    static procedure DrawElementsIndirect(mode: PrimitiveType; &type: DataType; indirect: pointer);
    external 'opengl32.dll' name 'glDrawElementsIndirect';
    
    static procedure MultiDrawElementsIndirect(mode: PrimitiveType; &type: DataType; var indirect: DrawArraysIndirectCommand; drawcount: Int32; stride: Int32);
    external 'opengl32.dll' name 'glMultiDrawElementsIndirect';
    static procedure MultiDrawElementsIndirect(mode: PrimitiveType; &type: DataType; indirect: pointer; drawcount: Int32; stride: Int32);
    external 'opengl32.dll' name 'glMultiDrawElementsIndirect';
    
    static procedure MultiDrawElementsIndirectCount(mode: PrimitiveType; &type: DataType; var indirect: DrawArraysIndirectCommand; drawcount: IntPtr; maxdrawcount: Int32; stride: Int32);
    external 'opengl32.dll' name 'glMultiDrawElementsIndirectCount';
    static procedure MultiDrawElementsIndirectCount(mode: PrimitiveType; &type: DataType; indirect: pointer; drawcount: IntPtr; maxdrawcount: Int32; stride: Int32);
    external 'opengl32.dll' name 'glMultiDrawElementsIndirectCount';
    
    static procedure MultiDrawElementsBaseVertex(mode: PrimitiveType; [MarshalAs(UnmanagedType.LPArray)] count: array of Int32; &type: DataType; [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPArray)] indices: array of array of UInt32; drawcount: Int32; [MarshalAs(UnmanagedType.LPArray)] basevertex: array of Int32);
    external 'opengl32.dll' name 'glMultiDrawElementsBaseVertex';
    static procedure MultiDrawElementsBaseVertex(mode: PrimitiveType; [MarshalAs(UnmanagedType.LPArray)] count: array of Int32; &type: DataType; [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPArray)] indices: array of array of UInt16; drawcount: Int32; [MarshalAs(UnmanagedType.LPArray)] basevertex: array of Int32);
    external 'opengl32.dll' name 'glMultiDrawElementsBaseVertex';
    static procedure MultiDrawElementsBaseVertex(mode: PrimitiveType; [MarshalAs(UnmanagedType.LPArray)] count: array of Int32; &type: DataType; [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPArray)] indices: array of array of Byte; drawcount: Int32; [MarshalAs(UnmanagedType.LPArray)] basevertex: array of Int32);
    external 'opengl32.dll' name 'glMultiDrawElementsBaseVertex';
    static procedure MultiDrawElementsBaseVertex(mode: PrimitiveType; [MarshalAs(UnmanagedType.LPArray)] count: array of Int32; &type: DataType; [MarshalAs(UnmanagedType.LPArray)] indices: array of IntPtr; drawcount: Int32; var basevertex: Int32);
    external 'opengl32.dll' name 'glMultiDrawElementsBaseVertex';
    static procedure MultiDrawElementsBaseVertex(mode: PrimitiveType; var count: Int32; &type: DataType; [MarshalAs(UnmanagedType.SysInt, ArraySubType = UnmanagedType.LPArray)] var indices: array of UInt32; drawcount: Int32; var basevertex: Int32);
    external 'opengl32.dll' name 'glMultiDrawElementsBaseVertex';
    static procedure MultiDrawElementsBaseVertex(mode: PrimitiveType; var count: Int32; &type: DataType; [MarshalAs(UnmanagedType.SysInt, ArraySubType = UnmanagedType.LPArray)] var indices: array of UInt16; drawcount: Int32; var basevertex: Int32);
    external 'opengl32.dll' name 'glMultiDrawElementsBaseVertex';
    static procedure MultiDrawElementsBaseVertex(mode: PrimitiveType; var count: Int32; &type: DataType; [MarshalAs(UnmanagedType.SysInt, ArraySubType = UnmanagedType.LPArray)] var indices: array of Byte; drawcount: Int32; var basevertex: Int32);
    external 'opengl32.dll' name 'glMultiDrawElementsBaseVertex';
    static procedure MultiDrawElementsBaseVertex(mode: PrimitiveType; var count: Int32; &type: DataType; var indices: IntPtr; drawcount: Int32; var basevertex: Int32);
    external 'opengl32.dll' name 'glMultiDrawElementsBaseVertex';
    static procedure MultiDrawElementsBaseVertex(mode: PrimitiveType; var count: Int32; &type: DataType; var indices: pointer; drawcount: Int32; var basevertex: Int32);
    external 'opengl32.dll' name 'glMultiDrawElementsBaseVertex';
    static procedure MultiDrawElementsBaseVertex(mode: PrimitiveType; count: ^Int32; &type: DataType; indices: ^IntPtr; drawcount: Int32; basevertex: ^Int32);
    external 'opengl32.dll' name 'glMultiDrawElementsBaseVertex';
    static procedure MultiDrawElementsBaseVertex(mode: PrimitiveType; count: ^Int32; &type: DataType; indices: ^pointer; drawcount: Int32; basevertex: ^Int32);
    external 'opengl32.dll' name 'glMultiDrawElementsBaseVertex';
    static procedure MultiDrawElementsBaseVertex(mode: PrimitiveType; count: pointer; &type: DataType; indices: pointer; drawcount: Int32; basevertex: pointer);
    external 'opengl32.dll' name 'glMultiDrawElementsBaseVertex';
    
    {$endregion 10.4 - Drawing Commands Using Vertex Arrays}
    
    {$region 10.5 - Vertex Array and Vertex Array Object Queries}
    
    static procedure GetVertexArrayiv(vaobj: VertexArrayName; pname: VertexAttribInfoType; var param: Int32);
    external 'opengl32.dll' name 'glGetVertexArrayiv';
    static procedure GetVertexArrayiv(vaobj: VertexArrayName; pname: VertexAttribInfoType; param: pointer);
    external 'opengl32.dll' name 'glGetVertexArrayiv';
    
    static procedure GetVertexArrayIndexediv(vaobj: VertexArrayName; index: UInt32; pname: VertexAttribInfoType; var param: Int32);
    external 'opengl32.dll' name 'glGetVertexArrayIndexediv';
    static procedure GetVertexArrayIndexediv(vaobj: VertexArrayName; index: UInt32; pname: VertexAttribInfoType; param: pointer);
    external 'opengl32.dll' name 'glGetVertexArrayIndexediv';
    
    static procedure GetVertexArrayIndexed64iv(vaobj: VertexArrayName; index: UInt32; pname: VertexAttribInfoType; var param: Int64);
    external 'opengl32.dll' name 'glGetVertexArrayIndexed64iv';
    static procedure GetVertexArrayIndexed64iv(vaobj: VertexArrayName; index: UInt32; pname: VertexAttribInfoType; param: pointer);
    external 'opengl32.dll' name 'glGetVertexArrayIndexed64iv';
    
    static procedure GetVertexAttribdv(index: UInt32; pname: VertexAttribInfoType; [MarshalAs(UnmanagedType.LPArray)] &params: array of real);
    external 'opengl32.dll' name 'glGetVertexAttribdv';
    static procedure GetVertexAttribdv(index: UInt32; pname: VertexAttribInfoType; var &params: real);
    external 'opengl32.dll' name 'glGetVertexAttribdv';
    static procedure GetVertexAttribdv(index: UInt32; pname: VertexAttribInfoType; &params: pointer);
    external 'opengl32.dll' name 'glGetVertexAttribdv';
    
    static procedure GetVertexAttribfv(index: UInt32; pname: VertexAttribInfoType; [MarshalAs(UnmanagedType.LPArray)] &params: array of single);
    external 'opengl32.dll' name 'glGetVertexAttribfv';
    static procedure GetVertexAttribfv(index: UInt32; pname: VertexAttribInfoType; var &params: single);
    external 'opengl32.dll' name 'glGetVertexAttribfv';
    static procedure GetVertexAttribfv(index: UInt32; pname: VertexAttribInfoType; &params: pointer);
    external 'opengl32.dll' name 'glGetVertexAttribfv';
    
    static procedure GetVertexAttribiv(index: UInt32; pname: VertexAttribInfoType; [MarshalAs(UnmanagedType.LPArray)] &params: array of Int32);
    external 'opengl32.dll' name 'glGetVertexAttribiv';
    static procedure GetVertexAttribiv(index: UInt32; pname: VertexAttribInfoType; var &params: Int32);
    external 'opengl32.dll' name 'glGetVertexAttribiv';
    static procedure GetVertexAttribiv(index: UInt32; pname: VertexAttribInfoType; &params: pointer);
    external 'opengl32.dll' name 'glGetVertexAttribiv';
    
    static procedure GetVertexAttribIiv(index: UInt32; pname: VertexAttribInfoType; [MarshalAs(UnmanagedType.LPArray)] &params: array of Int32);
    external 'opengl32.dll' name 'glGetVertexAttribIiv';
    static procedure GetVertexAttribIiv(index: UInt32; pname: VertexAttribInfoType; var &params: Int32);
    external 'opengl32.dll' name 'glGetVertexAttribIiv';
    static procedure GetVertexAttribIiv(index: UInt32; pname: VertexAttribInfoType; &params: pointer);
    external 'opengl32.dll' name 'glGetVertexAttribIiv';
    
    static procedure GetVertexAttribIuiv(index: UInt32; pname: VertexAttribInfoType; [MarshalAs(UnmanagedType.LPArray)] &params: array of UInt32);
    external 'opengl32.dll' name 'glGetVertexAttribIuiv';
    static procedure GetVertexAttribIuiv(index: UInt32; pname: VertexAttribInfoType; var &params: UInt32);
    external 'opengl32.dll' name 'glGetVertexAttribIuiv';
    static procedure GetVertexAttribIuiv(index: UInt32; pname: VertexAttribInfoType; &params: pointer);
    external 'opengl32.dll' name 'glGetVertexAttribIuiv';
    
    static procedure GetVertexAttribLdv(index: UInt32; pname: VertexAttribInfoType; [MarshalAs(UnmanagedType.LPArray)] &params: array of real);
    external 'opengl32.dll' name 'glGetVertexAttribLdv';
    static procedure GetVertexAttribLdv(index: UInt32; pname: VertexAttribInfoType; var &params: real);
    external 'opengl32.dll' name 'glGetVertexAttribLdv';
    static procedure GetVertexAttribLdv(index: UInt32; pname: VertexAttribInfoType; &params: pointer);
    external 'opengl32.dll' name 'glGetVertexAttribLdv';
    
    static procedure GetVertexAttribPointerv(index: UInt32; pname: UInt32; [MarshalAs(UnmanagedType.LPArray)] _pointer: array of IntPtr);
    external 'opengl32.dll' name 'glGetVertexAttribPointerv';
    static procedure GetVertexAttribPointerv(index: UInt32; pname: UInt32; var _pointer: IntPtr);
    external 'opengl32.dll' name 'glGetVertexAttribPointerv';
    static procedure GetVertexAttribPointerv(index: UInt32; pname: UInt32; var _pointer: pointer);
    external 'opengl32.dll' name 'glGetVertexAttribPointerv';
    static procedure GetVertexAttribPointerv(index: UInt32; pname: UInt32; _pointer: ^pointer);
    external 'opengl32.dll' name 'glGetVertexAttribPointerv';
    
    {$endregion 10.5 - Vertex Array and Vertex Array Object Queries}
    
    {$region 10.9 - Conditional Rendering}
    
    static procedure BeginConditionalRender(id: UInt32; mode: ConditionalRenderingMode);
    external 'opengl32.dll' name 'glBeginConditionalRender';
    
    static procedure EndConditionalRender;
    external 'opengl32.dll' name 'glEndConditionalRender';
    
    {$endregion 10.9 - Conditional Rendering}
    
    {$endregion 10.0 - Vertex Specification and Drawing Commands}
    
    {$region 11.0 - Programmable Vertex Processing}
    
    {$region 11.1 - Vertex Shaders}
    
    // 11.1.1
    
    static procedure BindAttribLocation(&program: ProgramName; index: UInt32; [MarshalAs(UnmanagedType.LPStr)] name: string);
    external 'opengl32.dll' name 'glBindAttribLocation';
    static procedure BindAttribLocation(&program: ProgramName; index: UInt32; name: IntPtr);
    external 'opengl32.dll' name 'glBindAttribLocation';
    
    static procedure GetActiveAttrib(&program: ProgramName; index: UInt32; bufSize: Int32; var length: Int32; var size: Int32; var &type: ProgramVarType; [MarshalAs(UnmanagedType.LPStr)] name: string);
    external 'opengl32.dll' name 'glGetActiveAttrib';
    static procedure GetActiveAttrib(&program: ProgramName; index: UInt32; bufSize: Int32; var length: Int32; var size: Int32; var &type: ProgramVarType; name: IntPtr);
    external 'opengl32.dll' name 'glGetActiveAttrib';
    static procedure GetActiveAttrib(&program: ProgramName; index: UInt32; bufSize: Int32; var length: Int32; var size: Int32; &type: pointer; [MarshalAs(UnmanagedType.LPStr)] name: string);
    external 'opengl32.dll' name 'glGetActiveAttrib';
    static procedure GetActiveAttrib(&program: ProgramName; index: UInt32; bufSize: Int32; var length: Int32; var size: Int32; &type: pointer; name: IntPtr);
    external 'opengl32.dll' name 'glGetActiveAttrib';
    static procedure GetActiveAttrib(&program: ProgramName; index: UInt32; bufSize: Int32; length: ^Int32; size: ^Int32; var &type: ProgramVarType; [MarshalAs(UnmanagedType.LPStr)] name: string);
    external 'opengl32.dll' name 'glGetActiveAttrib';
    static procedure GetActiveAttrib(&program: ProgramName; index: UInt32; bufSize: Int32; length: ^Int32; size: ^Int32; var &type: ProgramVarType; name: IntPtr);
    external 'opengl32.dll' name 'glGetActiveAttrib';
    static procedure GetActiveAttrib(&program: ProgramName; index: UInt32; bufSize: Int32; length: ^Int32; size: ^Int32; &type: pointer; [MarshalAs(UnmanagedType.LPStr)] name: string);
    external 'opengl32.dll' name 'glGetActiveAttrib';
    static procedure GetActiveAttrib(&program: ProgramName; index: UInt32; bufSize: Int32; length: ^Int32; size: ^Int32; &type: pointer; name: IntPtr);
    external 'opengl32.dll' name 'glGetActiveAttrib';
    
    static function GetAttribLocation(&program: ProgramName; [MarshalAs(UnmanagedType.LPStr)] name: string): Int32;
    external 'opengl32.dll' name 'glGetAttribLocation';
    static function GetAttribLocation(&program: ProgramName; name: IntPtr): Int32;
    external 'opengl32.dll' name 'glGetAttribLocation';
    
    //11.1.2
    
    //11.1.2.1
    
    static procedure TransformFeedbackVaryings(&program: ProgramName; count: Int32; [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] varyings: array of string; bufferMode: TransformFeedbackBufferMode);
    external 'opengl32.dll' name 'glTransformFeedbackVaryings';
    static procedure TransformFeedbackVaryings(&program: ProgramName; count: Int32; [MarshalAs(UnmanagedType.SysInt, ArraySubType = UnmanagedType.LPStr)] var varyings: string; bufferMode: TransformFeedbackBufferMode);
    external 'opengl32.dll' name 'glTransformFeedbackVaryings';
    static procedure TransformFeedbackVaryings(&program: ProgramName; count: Int32; var varyings: IntPtr; bufferMode: TransformFeedbackBufferMode);
    external 'opengl32.dll' name 'glTransformFeedbackVaryings';
    static procedure TransformFeedbackVaryings(&program: ProgramName; count: Int32; varyings: pointer; bufferMode: TransformFeedbackBufferMode);
    external 'opengl32.dll' name 'glTransformFeedbackVaryings';
    
    static procedure GetTransformFeedbackVarying(&program: ProgramName; index: UInt32; bufSize: Int32; var length: Int32; var size: Int32; var &type: ProgramVarType; [MarshalAs(UnmanagedType.LPStr)] name: string);
    external 'opengl32.dll' name 'glGetTransformFeedbackVarying';
    static procedure GetTransformFeedbackVarying(&program: ProgramName; index: UInt32; bufSize: Int32; var length: Int32; var size: Int32; var &type: ProgramVarType; name: IntPtr);
    external 'opengl32.dll' name 'glGetTransformFeedbackVarying';
    static procedure GetTransformFeedbackVarying(&program: ProgramName; index: UInt32; bufSize: Int32; length: ^Int32; size: ^Int32; &type: pointer; [MarshalAs(UnmanagedType.LPStr)] name: string);
    external 'opengl32.dll' name 'glGetTransformFeedbackVarying';
    static procedure GetTransformFeedbackVarying(&program: ProgramName; index: UInt32; bufSize: Int32; length: ^Int32; size: ^Int32; &type: pointer; name: IntPtr);
    external 'opengl32.dll' name 'glGetTransformFeedbackVarying';
    
    //11.1.3
    
    //11.1.3.11
    
    static procedure ValidateProgram(&program: ProgramName);
    external 'opengl32.dll' name 'glValidateProgram';
    
    static procedure ValidateProgramPipeline(pipeline: ProgramPipelineName);
    external 'opengl32.dll' name 'glValidateProgramPipeline';
    
    {$endregion 11.1 - Vertex Shaders}
    
    {$region 11.2 - Tessellation}
    
    // 11.2.2
    
    static procedure PatchParameterfv(pname: PatchMode; [MarshalAs(UnmanagedType.LPArray)] values: array of single);
    external 'opengl32.dll' name 'glPatchParameterfv';
    static procedure PatchParameterfv(pname: PatchMode; var values: single);
    external 'opengl32.dll' name 'glPatchParameterfv';
    static procedure PatchParameterfv(pname: PatchMode; values: pointer);
    external 'opengl32.dll' name 'glPatchParameterfv';
    
    {$endregion 11.2 - Tessellation}
    
    {$endregion 11.0 - Programmable Vertex Processing}
    
    {$region 13.0 - Fixed-Function Vertex Post-Processing}
    
    {$region 13.3 - Transform Feedback}
    
    // 13.3.1
    
    static procedure GenTransformFeedbacks(n: Int32; [MarshalAs(UnmanagedType.LPArray)] ids: array of TransformFeedbackName);
    external 'opengl32.dll' name 'glGenTransformFeedbacks';
    static procedure GenTransformFeedbacks(n: Int32; var ids: TransformFeedbackName);
    external 'opengl32.dll' name 'glGenTransformFeedbacks';
    static procedure GenTransformFeedbacks(n: Int32; ids: pointer);
    external 'opengl32.dll' name 'glGenTransformFeedbacks';
    
    static procedure DeleteTransformFeedbacks(n: Int32; [MarshalAs(UnmanagedType.LPArray)] ids: array of TransformFeedbackName);
    external 'opengl32.dll' name 'glDeleteTransformFeedbacks';
    static procedure DeleteTransformFeedbacks(n: Int32; var ids: TransformFeedbackName);
    external 'opengl32.dll' name 'glDeleteTransformFeedbacks';
    static procedure DeleteTransformFeedbacks(n: Int32; ids: pointer);
    external 'opengl32.dll' name 'glDeleteTransformFeedbacks';
    
    static function IsTransformFeedback(id: TransformFeedbackName): boolean;
    external 'opengl32.dll' name 'glIsTransformFeedback';
    
    static procedure BindTransformFeedback(target: TransformFeedbackBindTarget; id: TransformFeedbackName);
    external 'opengl32.dll' name 'glBindTransformFeedback';
    
    static procedure CreateTransformFeedbacks(n: Int32; [MarshalAs(UnmanagedType.LPArray)] ids: array of TransformFeedbackName);
    external 'opengl32.dll' name 'glCreateTransformFeedbacks';
    static procedure CreateTransformFeedbacks(n: Int32; var ids: TransformFeedbackName);
    external 'opengl32.dll' name 'glCreateTransformFeedbacks';
    static procedure CreateTransformFeedbacks(n: Int32; ids: pointer);
    external 'opengl32.dll' name 'glCreateTransformFeedbacks';
    
    // 13.3.2
    
    static procedure BeginTransformFeedback(primitiveMode: PrimitiveType);
    external 'opengl32.dll' name 'glBeginTransformFeedback';
    
    static procedure EndTransformFeedback;
    external 'opengl32.dll' name 'glEndTransformFeedback';
    
    static procedure PauseTransformFeedback;
    external 'opengl32.dll' name 'glPauseTransformFeedback';
    
    static procedure ResumeTransformFeedback;
    external 'opengl32.dll' name 'glResumeTransformFeedback';
    
    static procedure TransformFeedbackBufferRange(xfb: TransformFeedbackName; index: UInt32; buffer: BufferName; offset: IntPtr; size: UIntPtr);
    external 'opengl32.dll' name 'glTransformFeedbackBufferRange';
    
    static procedure TransformFeedbackBufferBase(xfb: TransformFeedbackName; index: UInt32; buffer: BufferName);
    external 'opengl32.dll' name 'glTransformFeedbackBufferBase';
    
    // 13.3.3
    
    static procedure DrawTransformFeedback(mode: PrimitiveType; id: TransformFeedbackName);
    external 'opengl32.dll' name 'glDrawTransformFeedback';
    
    static procedure DrawTransformFeedbackInstanced(mode: PrimitiveType; id: TransformFeedbackName; instancecount: Int32);
    external 'opengl32.dll' name 'glDrawTransformFeedbackInstanced';
    
    static procedure DrawTransformFeedbackStream(mode: PrimitiveType; id: TransformFeedbackName; stream: UInt32);
    external 'opengl32.dll' name 'glDrawTransformFeedbackStream';
    
    static procedure DrawTransformFeedbackStreamInstanced(mode: PrimitiveType; id: TransformFeedbackName; stream: UInt32; instancecount: Int32);
    external 'opengl32.dll' name 'glDrawTransformFeedbackStreamInstanced';
    
    {$endregion 13.3 - Transform Feedback}
    
    {$region 13.7 - Primitive Clipping}
    
    static procedure ClipControl(origin: ClipOriginMode; depth: ClipDepthMode);
    external 'opengl32.dll' name 'glClipControl';
    
    {$endregion 13.7 - Primitive Clipping}
    
    {$region 13.8 - Coordinate Transformations}
    
    // 13.8.1
    
    static procedure DepthRangeArrayv(first: UInt32; count: Int32; [MarshalAs(UnmanagedType.LPArray)] v: array of real);
    external 'opengl32.dll' name 'glDepthRangeArrayv';
    static procedure DepthRangeArrayv(first: UInt32; count: Int32; var v: real);
    external 'opengl32.dll' name 'glDepthRangeArrayv';
    static procedure DepthRangeArrayv(first: UInt32; count: Int32; v: pointer);
    external 'opengl32.dll' name 'glDepthRangeArrayv';
    
    static procedure DepthRangeIndexed(index: UInt32; n: real; f: real);
    external 'opengl32.dll' name 'glDepthRangeIndexed';
    
    static procedure DepthRange(n: real; f: real);
    external 'opengl32.dll' name 'glDepthRange';
    
    static procedure DepthRangef(n: single; f: single);
    external 'opengl32.dll' name 'glDepthRangef';
    
    static procedure ViewportArrayv(first: UInt32; count: Int32; [MarshalAs(UnmanagedType.LPArray)] v: array of single);
    external 'opengl32.dll' name 'glViewportArrayv';
    static procedure ViewportArrayv(first: UInt32; count: Int32; var v: single);
    external 'opengl32.dll' name 'glViewportArrayv';
    static procedure ViewportArrayv(first: UInt32; count: Int32; v: pointer);
    external 'opengl32.dll' name 'glViewportArrayv';
    
    static procedure ViewportIndexedf(index: UInt32; x: single; y: single; w: single; h: single);
    external 'opengl32.dll' name 'glViewportIndexedf';
    
    static procedure ViewportIndexedfv(index: UInt32; [MarshalAs(UnmanagedType.LPArray)] v: array of single);
    external 'opengl32.dll' name 'glViewportIndexedfv';
    static procedure ViewportIndexedfv(index: UInt32; var v: single);
    external 'opengl32.dll' name 'glViewportIndexedfv';
    static procedure ViewportIndexedfv(index: UInt32; v: pointer);
    external 'opengl32.dll' name 'glViewportIndexedfv';
    
    static procedure Viewport(x: Int32; y: Int32; width: Int32; height: Int32);
    external 'opengl32.dll' name 'glViewport';
    
    {$endregion 13.8 - Coordinate Transformations}
    
    {$endregion 13.0 - Fixed-Function Vertex Post-Processing}
    
    {$region 14.0 Fixed-Function Primitive Assembly and Rasterization}
    
    {$region 14.3 - Antialiasing}
    
    // 14.3.1
    
    static procedure GetMultisamplefv(pname: MultisampleInfoType; index: UInt32; var val: Vec2f);
    external 'opengl32.dll' name 'glGetMultisamplefv';
    static procedure GetMultisamplefv(pname: MultisampleInfoType; index: UInt32; [MarshalAs(UnmanagedType.LPArray)] val: array of single);
    external 'opengl32.dll' name 'glGetMultisamplefv';
    static procedure GetMultisamplefv(pname: MultisampleInfoType; index: UInt32; var val: single);
    external 'opengl32.dll' name 'glGetMultisamplefv';
    static procedure GetMultisamplefv(pname: MultisampleInfoType; index: UInt32; val: pointer);
    external 'opengl32.dll' name 'glGetMultisamplefv';
    
    // 14.3.1.1
    
    static procedure MinSampleShading(value: single);
    external 'opengl32.dll' name 'glMinSampleShading';
    
    {$endregion 14.3 - Antialiasing}
    
    {$region 14.4 - Points}
    
    static procedure PointSize(size: single);
    external 'opengl32.dll' name 'glPointSize';
    
    static procedure PointParameteri(pname: PointInfoType; param: Int32);
    external 'opengl32.dll' name 'glPointParameteri';
    static procedure PointParameteri(pname: PointInfoType; param: ClipOriginMode);
    external 'opengl32.dll' name 'glPointParameteri';
    
    static procedure PointParameterf(pname: PointInfoType; param: single);
    external 'opengl32.dll' name 'glPointParameterf';
    
    static procedure PointParameteriv(pname: PointInfoType; [MarshalAs(UnmanagedType.LPArray)] &params: array of Int32);
    external 'opengl32.dll' name 'glPointParameteriv';
    static procedure PointParameteriv(pname: PointInfoType; var &params: Int32);
    external 'opengl32.dll' name 'glPointParameteriv';
    static procedure PointParameteriv(pname: PointInfoType; &params: pointer);
    external 'opengl32.dll' name 'glPointParameteriv';
    
    static procedure PointParameterfv(pname: PointInfoType; [MarshalAs(UnmanagedType.LPArray)] &params: array of single);
    external 'opengl32.dll' name 'glPointParameterfv';
    static procedure PointParameterfv(pname: PointInfoType; var &params: single);
    external 'opengl32.dll' name 'glPointParameterfv';
    static procedure PointParameterfv(pname: PointInfoType; &params: pointer);
    external 'opengl32.dll' name 'glPointParameterfv';
    
    {$endregion 14.4 - Points}
    
    {$region 14.5 - Line Segments}
    
    static procedure LineWidth(width: single);
    external 'opengl32.dll' name 'glLineWidth';
    
    {$endregion 14.5 - Line Segments}
    
    {$region 14.6 - Polygons}
    
    // 14.6.1
    
    static procedure FrontFace(mode: FrontFaceDirection);
    external 'opengl32.dll' name 'glFrontFace';
    
    static procedure CullFace(mode: PolygonFace);
    external 'opengl32.dll' name 'glCullFace';
    
    // 14.6.4
    
    static procedure PolygonMode(face: PolygonFace; mode: PolygonRasterizationMode);
    external 'opengl32.dll' name 'glPolygonMode';
    
    // 14.6.5
    
    static procedure PolygonOffsetClamp(factor: single; units: single; clamp: single);
    external 'opengl32.dll' name 'glPolygonOffsetClamp';
    
    static procedure PolygonOffset(factor: single; units: single);
    external 'opengl32.dll' name 'glPolygonOffset';
    
    {$endregion 14.6 - Polygons}
    
    {$region 14.9 - Early Per-Fragment Tests}
    
    // 14.9.2
    
    static procedure ScissorArrayv(first: UInt32; count: Int32; [MarshalAs(UnmanagedType.LPArray)] v: array of Int32);
    external 'opengl32.dll' name 'glScissorArrayv';
    static procedure ScissorArrayv(first: UInt32; count: Int32; var v: Int32);
    external 'opengl32.dll' name 'glScissorArrayv';
    static procedure ScissorArrayv(first: UInt32; count: Int32; v: pointer);
    external 'opengl32.dll' name 'glScissorArrayv';
    
    static procedure ScissorIndexed(index: UInt32; left: Int32; bottom: Int32; width: Int32; height: Int32);
    external 'opengl32.dll' name 'glScissorIndexed';
    
    static procedure ScissorIndexedv(index: UInt32; [MarshalAs(UnmanagedType.LPArray)] v: array of Int32);
    external 'opengl32.dll' name 'glScissorIndexedv';
    static procedure ScissorIndexedv(index: UInt32; var v: Int32);
    external 'opengl32.dll' name 'glScissorIndexedv';
    static procedure ScissorIndexedv(index: UInt32; v: pointer);
    external 'opengl32.dll' name 'glScissorIndexedv';
    
    static procedure Scissor(x: Int32; y: Int32; width: Int32; height: Int32);
    external 'opengl32.dll' name 'glScissor';
    
    // 14.9.3
    
    static procedure SampleCoverage(value: single; invert: boolean);
    external 'opengl32.dll' name 'glSampleCoverage';
    
    static procedure SampleMaski(maskNumber: UInt32; mask: UInt32);
    external 'opengl32.dll' name 'glSampleMaski';
    
    {$endregion 14.9 - Early Per-Fragment Tests}
    
    {$endregion 14.0 Fixed-Function Primitive Assembly and Rasterization}
    
    {$region 15.0 - Programmable Fragment Processing}
    
    {$region 15.2 - Shader Execution}
    
    // 15.2.3
    
    static procedure BindFragDataLocationIndexed(&program: ProgramName; colorNumber: UInt32; index: UInt32; [MarshalAs(UnmanagedType.LPStr)] name: string);
    external 'opengl32.dll' name 'glBindFragDataLocationIndexed';
    static procedure BindFragDataLocationIndexed(&program: ProgramName; colorNumber: UInt32; index: UInt32; name: IntPtr);
    external 'opengl32.dll' name 'glBindFragDataLocationIndexed';
    
    static procedure BindFragDataLocation(&program: ProgramName; color: UInt32; [MarshalAs(UnmanagedType.LPStr)] name: string);
    external 'opengl32.dll' name 'glBindFragDataLocation';
    static procedure BindFragDataLocation(&program: ProgramName; color: UInt32; name: IntPtr);
    external 'opengl32.dll' name 'glBindFragDataLocation';
    
    static function GetFragDataLocation(&program: ProgramName; [MarshalAs(UnmanagedType.LPStr)] name: string): Int32;
    external 'opengl32.dll' name 'glGetFragDataLocation';
    static function GetFragDataLocation(&program: ProgramName; name: IntPtr): Int32;
    external 'opengl32.dll' name 'glGetFragDataLocation';
    
    static function GetFragDataIndex(&program: ProgramName; [MarshalAs(UnmanagedType.LPStr)] name: string): Int32;
    external 'opengl32.dll' name 'glGetFragDataIndex';
    static function GetFragDataIndex(&program: ProgramName; name: IntPtr): Int32;
    external 'opengl32.dll' name 'glGetFragDataIndex';
    
    {$endregion 15.2 - Shader Execution}
    
    {$endregion 15.0 - Programmable Fragment Processing}
    
    {$region 17.0 - Writing Fragments and Samples to the Framebuffer}
    
    {$region 17.3 - Per-Fragment Operations}
    
    // 17.3.3
    
    static procedure StencilFunc(func: FuncActivationMode; ref: Int32; mask: UInt32);
    external 'opengl32.dll' name 'glStencilFunc';
    
    static procedure StencilFuncSeparate(face: PolygonFace; func: FuncActivationMode; ref: Int32; mask: UInt32);
    external 'opengl32.dll' name 'glStencilFuncSeparate';
    
    static procedure StencilOp(fail: StencilOpFailMode; zfail: StencilOpFailMode; zpass: StencilOpFailMode);
    external 'opengl32.dll' name 'glStencilOp';
    
    static procedure StencilOpSeparate(face: PolygonFace; sfail: StencilOpFailMode; dpfail: StencilOpFailMode; dppass: StencilOpFailMode);
    external 'opengl32.dll' name 'glStencilOpSeparate';
    
    // 17.3.4
    
    static procedure DepthFunc(func: FuncActivationMode);
    external 'opengl32.dll' name 'glDepthFunc';
    
    // 17.3.6
    
    static procedure Enablei(target: EnablableName; index: UInt32);
    external 'opengl32.dll' name 'glEnablei';
    
    static procedure Disablei(target: EnablableName; index: UInt32);
    external 'opengl32.dll' name 'glDisablei';
    
    // 17.3.6.1
    
    static procedure BlendEquation(mode: BlendEquationMode);
    external 'opengl32.dll' name 'glBlendEquation';
    
    static procedure BlendEquationSeparate(modeRGB: BlendEquationMode; modeAlpha: BlendEquationMode);
    external 'opengl32.dll' name 'glBlendEquationSeparate';
    
    static procedure BlendEquationi(buf: UInt32; mode: BlendEquationMode);
    external 'opengl32.dll' name 'glBlendEquationi';
    
    static procedure BlendEquationSeparatei(buf: UInt32; modeRGB: BlendEquationMode; modeAlpha: BlendEquationMode);
    external 'opengl32.dll' name 'glBlendEquationSeparatei';
    
    // 17.3.6.2
    
    static procedure BlendFunc(sfactor: BlendFuncMode; dfactor: BlendFuncMode);
    external 'opengl32.dll' name 'glBlendFunc';
    
    static procedure BlendFuncSeparate(sfactorRGB: BlendFuncMode; dfactorRGB: BlendFuncMode; sfactorAlpha: BlendFuncMode; dfactorAlpha: BlendFuncMode);
    external 'opengl32.dll' name 'glBlendFuncSeparate';
    
    static procedure BlendFunci(buf: UInt32; src: BlendFuncMode; dst: BlendFuncMode);
    external 'opengl32.dll' name 'glBlendFunci';
    
    static procedure BlendFuncSeparatei(buf: UInt32; srcRGB: BlendFuncMode; dstRGB: BlendFuncMode; srcAlpha: BlendFuncMode; dstAlpha: BlendFuncMode);
    external 'opengl32.dll' name 'glBlendFuncSeparatei';
    
    // 17.3.6.5
    
    static procedure BlendColor(red: single; green: single; blue: single; alpha: single);
    external 'opengl32.dll' name 'glBlendColor';
    
    // 17.3.9
    
    static procedure LogicOp(opcode: LogicOpCode);
    external 'opengl32.dll' name 'glLogicOp';
    
    {$endregion 17.3 - Per-Fragment Operations}
    
    {$region 17.4 - Whole Framebuffer Operations}
    
    // 17.4.1
    
    static procedure DrawBuffer(buf: FrameBufferPart);
    external 'opengl32.dll' name 'glDrawBuffer';
    
    static procedure NamedFramebufferDrawBuffer(framebuffer: FramebufferName; buf: FrameBufferPart);
    external 'opengl32.dll' name 'glNamedFramebufferDrawBuffer';
    
    static procedure DrawBuffers(n: Int32; [MarshalAs(UnmanagedType.LPArray)] bufs: array of FrameBufferPart);
    external 'opengl32.dll' name 'glDrawBuffers';
    static procedure DrawBuffers(n: Int32; var bufs: FrameBufferPart);
    external 'opengl32.dll' name 'glDrawBuffers';
    static procedure DrawBuffers(n: Int32; bufs: pointer);
    external 'opengl32.dll' name 'glDrawBuffers';
    
    static procedure NamedFramebufferDrawBuffers(framebuffer: FramebufferName; n: Int32; [MarshalAs(UnmanagedType.LPArray)] bufs: array of FrameBufferPart);
    external 'opengl32.dll' name 'glNamedFramebufferDrawBuffers';
    static procedure NamedFramebufferDrawBuffers(framebuffer: FramebufferName; n: Int32; var bufs: FrameBufferPart);
    external 'opengl32.dll' name 'glNamedFramebufferDrawBuffers';
    static procedure NamedFramebufferDrawBuffers(framebuffer: FramebufferName; n: Int32; bufs: pointer);
    external 'opengl32.dll' name 'glNamedFramebufferDrawBuffers';
    
    // 17.4.2
    
    static procedure ColorMask(red: boolean; green: boolean; blue: boolean; alpha: boolean);
    external 'opengl32.dll' name 'glColorMask';
    
    static procedure ColorMaski(index: UInt32; r: boolean; g: boolean; b: boolean; a: boolean);
    external 'opengl32.dll' name 'glColorMaski';
    
    static procedure DepthMask(flag: boolean);
    external 'opengl32.dll' name 'glDepthMask';
    
    static procedure StencilMask(mask: UInt32);
    external 'opengl32.dll' name 'glStencilMask';
    
    static procedure StencilMaskSeparate(face: PolygonFace; mask: UInt32);
    external 'opengl32.dll' name 'glStencilMaskSeparate';
    
    // 17.4.3
    
    static procedure Clear(mask: BufferTypeFlags);
    external 'opengl32.dll' name 'glClear';
    
    static procedure ClearColor(red: single; green: single; blue: single; alpha: single);
    external 'opengl32.dll' name 'glClearColor';
    
    static procedure ClearDepth(depth: real);
    external 'opengl32.dll' name 'glClearDepth';
    
    static procedure ClearDepthf(d: single);
    external 'opengl32.dll' name 'glClearDepthf';
    
    static procedure ClearStencil(s: Int32);
    external 'opengl32.dll' name 'glClearStencil';
    
    // 17.4.3.1
    
    static procedure ClearBufferiv(buffer: FramebufferAttachmentPoint; drawbuffer: Int32; [MarshalAs(UnmanagedType.LPArray)] value: array of Int32);
    external 'opengl32.dll' name 'glClearBufferiv';
    static procedure ClearBufferiv(buffer: FramebufferAttachmentPoint; drawbuffer: Int32; var value: Int32);
    external 'opengl32.dll' name 'glClearBufferiv';
    static procedure ClearBufferiv(buffer: FramebufferAttachmentPoint; drawbuffer: Int32; value: pointer);
    external 'opengl32.dll' name 'glClearBufferiv';
    
    static procedure ClearBufferfv(buffer: FramebufferAttachmentPoint; drawbuffer: Int32; [MarshalAs(UnmanagedType.LPArray)] value: array of single);
    external 'opengl32.dll' name 'glClearBufferfv';
    static procedure ClearBufferfv(buffer: FramebufferAttachmentPoint; drawbuffer: Int32; var value: single);
    external 'opengl32.dll' name 'glClearBufferfv';
    static procedure ClearBufferfv(buffer: FramebufferAttachmentPoint; drawbuffer: Int32; value: pointer);
    external 'opengl32.dll' name 'glClearBufferfv';
    
    static procedure ClearBufferuiv(buffer: FramebufferAttachmentPoint; drawbuffer: Int32; [MarshalAs(UnmanagedType.LPArray)] value: array of UInt32);
    external 'opengl32.dll' name 'glClearBufferuiv';
    static procedure ClearBufferuiv(buffer: FramebufferAttachmentPoint; drawbuffer: Int32; var value: UInt32);
    external 'opengl32.dll' name 'glClearBufferuiv';
    static procedure ClearBufferuiv(buffer: FramebufferAttachmentPoint; drawbuffer: Int32; value: pointer);
    external 'opengl32.dll' name 'glClearBufferuiv';
    
    static procedure ClearNamedFramebufferiv(framebuffer: FramebufferName; buffer: FramebufferAttachmentPoint; drawbuffer: Int32; [MarshalAs(UnmanagedType.LPArray)] value: array of Int32);
    external 'opengl32.dll' name 'glClearNamedFramebufferiv';
    static procedure ClearNamedFramebufferiv(framebuffer: FramebufferName; buffer: FramebufferAttachmentPoint; drawbuffer: Int32; var value: Int32);
    external 'opengl32.dll' name 'glClearNamedFramebufferiv';
    static procedure ClearNamedFramebufferiv(framebuffer: FramebufferName; buffer: FramebufferAttachmentPoint; drawbuffer: Int32; value: pointer);
    external 'opengl32.dll' name 'glClearNamedFramebufferiv';
    
    static procedure ClearNamedFramebufferfv(framebuffer: FramebufferName; buffer: FramebufferAttachmentPoint; drawbuffer: Int32; [MarshalAs(UnmanagedType.LPArray)] value: array of single);
    external 'opengl32.dll' name 'glClearNamedFramebufferfv';
    static procedure ClearNamedFramebufferfv(framebuffer: FramebufferName; buffer: FramebufferAttachmentPoint; drawbuffer: Int32; var value: single);
    external 'opengl32.dll' name 'glClearNamedFramebufferfv';
    static procedure ClearNamedFramebufferfv(framebuffer: FramebufferName; buffer: FramebufferAttachmentPoint; drawbuffer: Int32; value: pointer);
    external 'opengl32.dll' name 'glClearNamedFramebufferfv';
    
    static procedure ClearNamedFramebufferuiv(framebuffer: FramebufferName; buffer: FramebufferAttachmentPoint; drawbuffer: Int32; [MarshalAs(UnmanagedType.LPArray)] value: array of UInt32);
    external 'opengl32.dll' name 'glClearNamedFramebufferuiv';
    static procedure ClearNamedFramebufferuiv(framebuffer: FramebufferName; buffer: FramebufferAttachmentPoint; drawbuffer: Int32; var value: UInt32);
    external 'opengl32.dll' name 'glClearNamedFramebufferuiv';
    static procedure ClearNamedFramebufferuiv(framebuffer: FramebufferName; buffer: FramebufferAttachmentPoint; drawbuffer: Int32; value: pointer);
    external 'opengl32.dll' name 'glClearNamedFramebufferuiv';
    
    static procedure ClearBufferfi(buffer: FramebufferAttachmentPoint; drawbuffer: Int32; depth: single; stencil: Int32);
    external 'opengl32.dll' name 'glClearBufferfi';
    
    static procedure ClearNamedFramebufferfi(framebuffer: FramebufferName; buffer: FramebufferAttachmentPoint; drawbuffer: Int32; depth: single; stencil: Int32);
    external 'opengl32.dll' name 'glClearNamedFramebufferfi';
    
    // 17.4.4
    
    static procedure InvalidateSubFramebuffer(target: FramebufferBindTarget; numAttachments: Int32; [MarshalAs(UnmanagedType.LPArray)] attachments: array of FramebufferAttachmentPoint);
    external 'opengl32.dll' name 'glInvalidateSubFramebuffer';
    static procedure InvalidateSubFramebuffer(target: FramebufferBindTarget; numAttachments: Int32; var attachments: FramebufferAttachmentPoint);
    external 'opengl32.dll' name 'glInvalidateSubFramebuffer';
    static procedure InvalidateSubFramebuffer(target: FramebufferBindTarget; numAttachments: Int32; attachments: pointer);
    external 'opengl32.dll' name 'glInvalidateSubFramebuffer';
    
    static procedure InvalidateNamedFramebufferSubData(framebuffer: FramebufferName; numAttachments: Int32; [MarshalAs(UnmanagedType.LPArray)] attachments: array of FramebufferAttachmentPoint);
    external 'opengl32.dll' name 'glInvalidateNamedFramebufferSubData';
    static procedure InvalidateNamedFramebufferSubData(framebuffer: FramebufferName; numAttachments: Int32; var attachments: FramebufferAttachmentPoint);
    external 'opengl32.dll' name 'glInvalidateNamedFramebufferSubData';
    static procedure InvalidateNamedFramebufferSubData(framebuffer: FramebufferName; numAttachments: Int32; attachments: pointer);
    external 'opengl32.dll' name 'glInvalidateNamedFramebufferSubData';
    
    static procedure InvalidateFramebuffer(target: FramebufferBindTarget; numAttachments: Int32; [MarshalAs(UnmanagedType.LPArray)] attachments: array of FramebufferAttachmentPoint);
    external 'opengl32.dll' name 'glInvalidateFramebuffer';
    static procedure InvalidateFramebuffer(target: FramebufferBindTarget; numAttachments: Int32; var attachments: FramebufferAttachmentPoint);
    external 'opengl32.dll' name 'glInvalidateFramebuffer';
    static procedure InvalidateFramebuffer(target: FramebufferBindTarget; numAttachments: Int32; attachments: pointer);
    external 'opengl32.dll' name 'glInvalidateFramebuffer';
    
    static procedure InvalidateNamedFramebufferData(framebuffer: FramebufferName; numAttachments: Int32; [MarshalAs(UnmanagedType.LPArray)] attachments: array of FramebufferAttachmentPoint);
    external 'opengl32.dll' name 'glInvalidateNamedFramebufferData';
    static procedure InvalidateNamedFramebufferData(framebuffer: FramebufferName; numAttachments: Int32; var attachments: FramebufferAttachmentPoint);
    external 'opengl32.dll' name 'glInvalidateNamedFramebufferData';
    static procedure InvalidateNamedFramebufferData(framebuffer: FramebufferName; numAttachments: Int32; attachments: pointer);
    external 'opengl32.dll' name 'glInvalidateNamedFramebufferData';
    
    {$endregion 17.4 - Whole Framebuffer Operations}
    
    {$endregion 17.0 - Writing Fragments and Samples to the Framebuffer}
    
    {$region 18.0 - Reading and Copying Pixels}
    
    {$region 18.2 - Reading Pixels}
    
    // 18.2.1
    
    static procedure ReadBuffer(src: FramebufferAttachmentPoint);
    external 'opengl32.dll' name 'glReadBuffer';
    
    static procedure NamedFramebufferReadBuffer(framebuffer: FramebufferName; src: FramebufferAttachmentPoint);
    external 'opengl32.dll' name 'glNamedFramebufferReadBuffer';
    
    // 18.2.2
    
    static procedure ReadPixels(x: Int32; y: Int32; width: Int32; height: Int32; format: DataFormat; &type: DataType; pixels: IntPtr);
    external 'opengl32.dll' name 'glReadPixels';
    static procedure ReadPixels(x: Int32; y: Int32; width: Int32; height: Int32; format: DataFormat; &type: DataType; pixels: pointer);
    external 'opengl32.dll' name 'glReadPixels';
    
    static procedure ReadnPixels(x: Int32; y: Int32; width: Int32; height: Int32; format: DataFormat; &type: DataType; bufSize: Int32; data: IntPtr);
    external 'opengl32.dll' name 'glReadnPixels';
    static procedure ReadnPixels(x: Int32; y: Int32; width: Int32; height: Int32; format: DataFormat; &type: DataType; bufSize: Int32; data: pointer);
    external 'opengl32.dll' name 'glReadnPixels';
    
    // 18.2.8
    
    static procedure ClampColor(target: ColorClampTarget; clamp: UInt32);
    external 'opengl32.dll' name 'glClampColor';
    
    {$endregion 18.2 - Reading Pixels}
    
    {$region 18.3 - Copying Pixels}
    
    // 18.3.1
    
    static procedure BlitFramebuffer(srcX0: Int32; srcY0: Int32; srcX1: Int32; srcY1: Int32; dstX0: Int32; dstY0: Int32; dstX1: Int32; dstY1: Int32; mask: BufferTypeFlags; filter: PixelFilterMode);
    external 'opengl32.dll' name 'glBlitFramebuffer';
    
    static procedure BlitNamedFramebuffer(readFramebuffer: FramebufferName; drawFramebuffer: FramebufferName; srcX0: Int32; srcY0: Int32; srcX1: Int32; srcY1: Int32; dstX0: Int32; dstY0: Int32; dstX1: Int32; dstY1: Int32; mask: BufferTypeFlags; filter: PixelFilterMode);
    external 'opengl32.dll' name 'glBlitNamedFramebuffer';
    
    // 18.3.2
    
    // BufferBindType автоматически конвертируется в CopyableImageType
    static procedure CopyImageSubData(srcName: UInt32; srcTarget: CopyableImageType; srcLevel: Int32; srcX: Int32; srcY: Int32; srcZ: Int32; dstName: UInt32; dstTarget: CopyableImageType; dstLevel: Int32; dstX: Int32; dstY: Int32; dstZ: Int32; srcWidth: Int32; srcHeight: Int32; srcDepth: Int32);
    external 'opengl32.dll' name 'glCopyImageSubData';
    
    {$endregion 18.3 - Copying Pixels}
    
    {$endregion 18.0 - Reading and Copying Pixels}
    
    {$region 19.0 - Compute Shaders}
    
    static procedure DispatchCompute(num_groups_x: UInt32; num_groups_y: UInt32; num_groups_z: UInt32);
    external 'opengl32.dll' name 'glDispatchCompute';
    
    static procedure DispatchComputeIndirect(indirect: IntPtr);
    external 'opengl32.dll' name 'glDispatchComputeIndirect';
    
    {$endregion 19.0 - Compute Shaders}
    
    {$region 20.0 - Debug Output}
    
    {$region 20.2 - Debug Message Callback}
    
    static procedure DebugMessageCallback(callback: GLDEBUGPROC; userParam: IntPtr);
    external 'opengl32.dll' name 'glDebugMessageCallback';
    static procedure DebugMessageCallback(callback: GLDEBUGPROC; userParam: pointer);
    external 'opengl32.dll' name 'glDebugMessageCallback';
    
    {$endregion 20.2 - Debug Message Callback}
    
    {$region 20.4 - Controlling Debug Messages}
    
    static procedure DebugMessageControl(source: DebugSourceType; &type: DebugMessageType; severity: DebugSeverityLevel; count: Int32; [MarshalAs(UnmanagedType.LPArray)] ids: array of UInt32; enabled: boolean);
    external 'opengl32.dll' name 'glDebugMessageControl';
    static procedure DebugMessageControl(source: DebugSourceType; &type: DebugMessageType; severity: DebugSeverityLevel; count: Int32; var ids: UInt32; enabled: boolean);
    external 'opengl32.dll' name 'glDebugMessageControl';
    static procedure DebugMessageControl(source: DebugSourceType; &type: DebugMessageType; severity: DebugSeverityLevel; count: Int32; ids: IntPtr; enabled: boolean);
    external 'opengl32.dll' name 'glDebugMessageControl';
    
    {$endregion 20.4 - Controlling Debug Messages}
    
    {$region 20.5 - Externally Generated Messages}
    
    static procedure DebugMessageInsert(source: DebugSourceType; &type: DebugMessageType; id: UInt32; severity: DebugSeverityLevel; length: Int32; [MarshalAs(UnmanagedType.LPStr)] buf: string);
    external 'opengl32.dll' name 'glDebugMessageInsert';
    static procedure DebugMessageInsert(source: DebugSourceType; &type: DebugMessageType; id: UInt32; severity: DebugSeverityLevel; length: Int32; buf: IntPtr);
    external 'opengl32.dll' name 'glDebugMessageInsert';
    
    {$endregion 20.5 - Externally Generated Messages}
    
    {$region 20.6 - Debug Groups}
    
    static procedure PushDebugGroup(source: DebugSourceType; id: UInt32; length: Int32; [MarshalAs(UnmanagedType.LPStr)] message: string);
    external 'opengl32.dll' name 'glPushDebugGroup';
    static procedure PushDebugGroup(source: DebugSourceType; id: UInt32; length: Int32; message: IntPtr);
    external 'opengl32.dll' name 'glPushDebugGroup';
    
    static procedure PopDebugGroup;
    external 'opengl32.dll' name 'glPopDebugGroup';
    
    {$endregion 20.6 - Debug Groups}
    
    {$region 20.7 - Debug Labels}
    
    static procedure ObjectLabel(identifier: ObjectType; name: UInt32; length: Int32; [MarshalAs(UnmanagedType.LPStr)] &label: string);
    external 'opengl32.dll' name 'glObjectLabel';
    static procedure ObjectLabel(identifier: ObjectType; name: UInt32; length: Int32; &label: IntPtr);
    external 'opengl32.dll' name 'glObjectLabel';
    
    static procedure ObjectPtrLabel(ptr: IntPtr; length: Int32; [MarshalAs(UnmanagedType.LPStr)] &label: string);
    external 'opengl32.dll' name 'glObjectPtrLabel';
    static procedure ObjectPtrLabel(ptr: IntPtr; length: Int32; &label: IntPtr);
    external 'opengl32.dll' name 'glObjectPtrLabel';
    static procedure ObjectPtrLabel(ptr: pointer; length: Int32; [MarshalAs(UnmanagedType.LPStr)] &label: string);
    external 'opengl32.dll' name 'glObjectPtrLabel';
    static procedure ObjectPtrLabel(ptr: pointer; length: Int32; &label: IntPtr);
    external 'opengl32.dll' name 'glObjectPtrLabel';
    
    {$endregion 20.7 - Debug Labels}
    
    {$region 20.9 - Debug Output Queries}
    
    // ВНИМАНИЕ! messageLog является суммой count нуль-терминированных строк, то есть символ 0 будет после каждого сообщения. Обычные методы перевода тут не будут работать, надо ручками
    static function GetDebugMessageLog(count: UInt32; bufSize: Int32; [MarshalAs(UnmanagedType.LPArray)] sources: array of DebugSourceType; [MarshalAs(UnmanagedType.LPArray)] types: array of DebugMessageType; [MarshalAs(UnmanagedType.LPArray)] ids: array of UInt32; [MarshalAs(UnmanagedType.LPArray)] severities: array of DebugSeverityLevel; [MarshalAs(UnmanagedType.LPArray)] lengths: array of Int32; [MarshalAs(UnmanagedType.HString)] &messageLog: string): UInt32;
    external 'opengl32.dll' name 'glGetDebugMessageLog';
    static function GetDebugMessageLog(count: UInt32; bufSize: Int32; [MarshalAs(UnmanagedType.LPArray)] sources: array of DebugSourceType; [MarshalAs(UnmanagedType.LPArray)] types: array of DebugMessageType; [MarshalAs(UnmanagedType.LPArray)] ids: array of UInt32; [MarshalAs(UnmanagedType.LPArray)] severities: array of DebugSeverityLevel; [MarshalAs(UnmanagedType.LPArray)] lengths: array of Int32; messageLog: IntPtr): UInt32;
    external 'opengl32.dll' name 'glGetDebugMessageLog';
    static function GetDebugMessageLog(count: UInt32; bufSize: Int32; var sources: DebugSourceType; var types: DebugMessageType; var ids: UInt32; var severities: DebugSeverityLevel; var lengths: Int32; [MarshalAs(UnmanagedType.HString)] &messageLog: string): UInt32;
    external 'opengl32.dll' name 'glGetDebugMessageLog';
    static function GetDebugMessageLog(count: UInt32; bufSize: Int32; var sources: DebugSourceType; var types: DebugMessageType; var ids: UInt32; var severities: DebugSeverityLevel; var lengths: Int32; messageLog: IntPtr): UInt32;
    external 'opengl32.dll' name 'glGetDebugMessageLog';
    static function GetDebugMessageLog(count: UInt32; bufSize: Int32; sources: IntPtr; types: IntPtr; ids: IntPtr; severities: IntPtr; lengths: IntPtr; [MarshalAs(UnmanagedType.HString)] &messageLog: string): UInt32;
    external 'opengl32.dll' name 'glGetDebugMessageLog';
    static function GetDebugMessageLog(count: UInt32; bufSize: Int32; sources: IntPtr; types: IntPtr; ids: IntPtr; severities: IntPtr; lengths: IntPtr; messageLog: IntPtr): UInt32;
    external 'opengl32.dll' name 'glGetDebugMessageLog';
    
    static procedure GetObjectLabel(identifier: ObjectType; name: UInt32; bufSize: Int32; var length: Int32; [MarshalAs(UnmanagedType.LPStr)] &label: string);
    external 'opengl32.dll' name 'glGetObjectLabel';
    static procedure GetObjectLabel(identifier: ObjectType; name: UInt32; bufSize: Int32; var length: Int32; &label: IntPtr);
    external 'opengl32.dll' name 'glGetObjectLabel';
    static procedure GetObjectLabel(identifier: ObjectType; name: UInt32; bufSize: Int32; length: ^Int32; [MarshalAs(UnmanagedType.LPStr)] &label: string);
    external 'opengl32.dll' name 'glGetObjectLabel';
    static procedure GetObjectLabel(identifier: ObjectType; name: UInt32; bufSize: Int32; length: ^Int32; &label: IntPtr);
    external 'opengl32.dll' name 'glGetObjectLabel';
    
    static procedure GetObjectPtrLabel(ptr: pointer; bufSize: Int32; var length: Int32; [MarshalAs(UnmanagedType.LPStr)] &label: string);
    external 'opengl32.dll' name 'glGetObjectPtrLabel';
    static procedure GetObjectPtrLabel(ptr: pointer; bufSize: Int32; var length: Int32; &label: IntPtr);
    external 'opengl32.dll' name 'glGetObjectPtrLabel';
    static procedure GetObjectPtrLabel(ptr: pointer; bufSize: Int32; length: ^Int32; [MarshalAs(UnmanagedType.LPStr)] &label: string);
    external 'opengl32.dll' name 'glGetObjectPtrLabel';
    static procedure GetObjectPtrLabel(ptr: pointer; bufSize: Int32; length: ^Int32; &label: IntPtr);
    external 'opengl32.dll' name 'glGetObjectPtrLabel';
    
    {$endregion 20.9 - Debug Output Queries}
    
    {$endregion 20.0 - Debug Output}
    
    {$region 21.0 - Special Functions}
    
    {$endregion 21.5 - Hints}
    
    static procedure Hint(target: HintType; mode: HintValue);
    external 'opengl32.dll' name 'glHint';
    
    {$region 21.5 - Hints}
    
    {$endregion 21.0 - Special Functions}
    
    {$region 22.0 - Context State Queries}
    
    {$region 22.1 - Simple Queries}
    
    static procedure GetBooleanv(pname: GLGetQueries; [MarshalAs(UnmanagedType.LPArray)] data: array of boolean);
    external 'opengl32.dll' name 'glGetBooleanv';
    static procedure GetBooleanv(pname: GLGetQueries; var data: boolean);
    external 'opengl32.dll' name 'glGetBooleanv';
    static procedure GetBooleanv(pname: GLGetQueries; data: pointer);
    external 'opengl32.dll' name 'glGetBooleanv';
    
    static procedure GetIntegerv(pname: GLGetQueries; [MarshalAs(UnmanagedType.LPArray)] data: array of Int32);
    external 'opengl32.dll' name 'glGetIntegerv';
    static procedure GetIntegerv(pname: GLGetQueries; var data: Int32);
    external 'opengl32.dll' name 'glGetIntegerv';
    static procedure GetIntegerv(pname: QueryInfoType; var data: Int32);
    external 'opengl32.dll' name 'glGetIntegerv';
    static procedure GetIntegerv(pname: GLGetQueries; data: pointer);
    external 'opengl32.dll' name 'glGetIntegerv';
    
    static procedure GetInteger64v(pname: GLGetQueries; [MarshalAs(UnmanagedType.LPArray)] data: array of Int64);
    external 'opengl32.dll' name 'glGetInteger64v';
    static procedure GetInteger64v(pname: GLGetQueries; var data: Int64);
    external 'opengl32.dll' name 'glGetInteger64v';
    static procedure GetInteger64v(pname: QueryInfoType; var data: Int64);
    external 'opengl32.dll' name 'glGetInteger64v';
    static procedure GetInteger64v(pname: GLGetQueries; data: pointer);
    external 'opengl32.dll' name 'glGetInteger64v';
    
    static procedure GetFloatv(pname: GLGetQueries; [MarshalAs(UnmanagedType.LPArray)] data: array of single);
    external 'opengl32.dll' name 'glGetFloatv';
    static procedure GetFloatv(pname: GLGetQueries; var data: single);
    external 'opengl32.dll' name 'glGetFloatv';
    static procedure GetFloatv(pname: GLGetQueries; data: pointer);
    external 'opengl32.dll' name 'glGetFloatv';
    
    static procedure GetDoublev(pname: GLGetQueries; [MarshalAs(UnmanagedType.LPArray)] data: array of real);
    external 'opengl32.dll' name 'glGetDoublev';
    static procedure GetDoublev(pname: GLGetQueries; var data: real);
    external 'opengl32.dll' name 'glGetDoublev';
    static procedure GetDoublev(pname: GLGetQueries; data: pointer);
    external 'opengl32.dll' name 'glGetDoublev';
    
    static procedure GetBooleani_v(target: GLGetQueries; index: UInt32; [MarshalAs(UnmanagedType.LPArray)] data: array of boolean);
    external 'opengl32.dll' name 'glGetBooleani_v';
    static procedure GetBooleani_v(target: GLGetQueries; index: UInt32; var data: boolean);
    external 'opengl32.dll' name 'glGetBooleani_v';
    static procedure GetBooleani_v(target: GLGetQueries; index: UInt32; data: pointer);
    external 'opengl32.dll' name 'glGetBooleani_v';
    
    static procedure GetFloati_v(target: GLGetQueries; index: UInt32; [MarshalAs(UnmanagedType.LPArray)] data: array of single);
    external 'opengl32.dll' name 'glGetFloati_v';
    static procedure GetFloati_v(target: GLGetQueries; index: UInt32; var data: single);
    external 'opengl32.dll' name 'glGetFloati_v';
    static procedure GetFloati_v(target: GLGetQueries; index: UInt32; data: pointer);
    external 'opengl32.dll' name 'glGetFloati_v';
    
    static procedure GetIntegeri_v(target: GLGetQueries; index: UInt32; [MarshalAs(UnmanagedType.LPArray)] data: array of Int32);
    external 'opengl32.dll' name 'glGetIntegeri_v';
    static procedure GetIntegeri_v(target: GLGetQueries; index: UInt32; var data: Int32);
    external 'opengl32.dll' name 'glGetIntegeri_v';
    static procedure GetIntegeri_v(target: BufferBindType; index: UInt32; var data: BufferName);
    external 'opengl32.dll' name 'glGetIntegeri_v';
    static procedure GetIntegeri_v(target: GLGetQueries; index: UInt32; data: pointer);
    external 'opengl32.dll' name 'glGetIntegeri_v';
    
    static procedure GetInteger64i_v(target: GLGetQueries; index: UInt32; [MarshalAs(UnmanagedType.LPArray)] data: array of Int64);
    external 'opengl32.dll' name 'glGetInteger64i_v';
    static procedure GetInteger64i_v(target: GLGetQueries; index: UInt32; var data: Int64);
    external 'opengl32.dll' name 'glGetInteger64i_v';
    static procedure GetInteger64i_v(target: BufferBindType; index: UInt32; var data: Vec2i64);
    external 'opengl32.dll' name 'glGetInteger64i_v';
    static procedure GetInteger64i_v(target: GLGetQueries; index: UInt32; data: pointer);
    external 'opengl32.dll' name 'glGetInteger64i_v';
	  
    static procedure GetDoublei_v(target: GLGetQueries; index: UInt32; [MarshalAs(UnmanagedType.LPArray)] data: array of real);
    external 'opengl32.dll' name 'glGetDoublei_v';
    static procedure GetDoublei_v(target: GLGetQueries; index: UInt32; var data: real);
    external 'opengl32.dll' name 'glGetDoublei_v';
    static procedure GetDoublei_v(target: GLGetQueries; index: UInt32; data: pointer);
    external 'opengl32.dll' name 'glGetDoublei_v';
    
    static function IsEnabled(cap: EnablableName): boolean;
    external 'opengl32.dll' name 'glIsEnabled';
    
    static function IsEnabledi(target: EnablableName; index: UInt32): boolean;
    external 'opengl32.dll' name 'glIsEnabledi';
    
    {$endregion 22.1 - Simple Queries}
    
    {$region 22.2 - Pointer, String, and Related Context Queries}
    
    static procedure GetPointerv(pname: GLGetQueries; [MarshalAs(UnmanagedType.LPArray)] &params: array of IntPtr);
    external 'opengl32.dll' name 'glGetPointerv';
    static procedure GetPointerv(pname: GLGetQueries; var &params: IntPtr);
    external 'opengl32.dll' name 'glGetPointerv';
    static procedure GetPointerv(pname: GLGetQueries; &params: ^pointer);
    external 'opengl32.dll' name 'glGetPointerv';
    
    static function GetString(name: GLGetStringQueries): string; //ToDo #2029
    external 'opengl32.dll' name 'glGetString';
    static function GetStringPtr(name: GLGetStringQueries): IntPtr;
    external 'opengl32.dll' name 'glGetString';
    
    static function GetStringi(name: GLGetStringQueries; index: UInt32): string; //ToDo #2029
    external 'opengl32.dll' name 'glGetStringi';
    static function GetStringPtri(name: GLGetStringQueries; index: UInt32): IntPtr;
    external 'opengl32.dll' name 'glGetStringi';
    
    {$endregion 22.2 - Pointer, String, and Related Context Queries}
    
    {$region 22.3 - Internal Format Queries}
    
    static procedure GetInternalformativ(target: TextureBindTarget; internalformat: InternalDataFormat; pname: InternalFormatInfoType; bufSize: Int32; [MarshalAs(UnmanagedType.LPArray)] &params: array of Int32);
    external 'opengl32.dll' name 'glGetInternalformativ';
    static procedure GetInternalformativ(target: TextureBindTarget; internalformat: InternalDataFormat; pname: InternalFormatInfoType; bufSize: Int32; var &params: Int32);
    external 'opengl32.dll' name 'glGetInternalformativ';
    static procedure GetInternalformativ(target: TextureBindTarget; internalformat: InternalDataFormat; pname: InternalFormatInfoType; bufSize: Int32; &params: pointer);
    external 'opengl32.dll' name 'glGetInternalformativ';
    
    static procedure GetInternalformati64v(target: TextureBindTarget; internalformat: InternalDataFormat; pname: InternalFormatInfoType; bufSize: Int32; [MarshalAs(UnmanagedType.LPArray)] &params: array of Int64);
    external 'opengl32.dll' name 'glGetInternalformati64v';
    static procedure GetInternalformati64v(target: TextureBindTarget; internalformat: InternalDataFormat; pname: InternalFormatInfoType; bufSize: Int32; var &params: Int64);
    external 'opengl32.dll' name 'glGetInternalformati64v';
    static procedure GetInternalformati64v(target: TextureBindTarget; internalformat: InternalDataFormat; pname: InternalFormatInfoType; bufSize: Int32; &params: pointer);
    external 'opengl32.dll' name 'glGetInternalformati64v';
    
    {$endregion 22.3 - Internal Format Queries}
    
    {$region 22.4 - Transform Feedback State Queries}
    
    static procedure GetTransformFeedbackiv(xfb: TransformFeedbackName; pname: TransformFeedbackInfoType; [MarshalAs(UnmanagedType.LPArray)] param: array of Int32);
    external 'opengl32.dll' name 'glGetTransformFeedbackiv';
    static procedure GetTransformFeedbackiv(xfb: TransformFeedbackName; pname: TransformFeedbackInfoType; var param: Int32);
    external 'opengl32.dll' name 'glGetTransformFeedbackiv';
    static procedure GetTransformFeedbackiv(xfb: TransformFeedbackName; pname: TransformFeedbackInfoType; param: pointer);
    external 'opengl32.dll' name 'glGetTransformFeedbackiv';
    
    static procedure GetTransformFeedbacki_v(xfb: TransformFeedbackName; pname: TransformFeedbackInfoType; index: UInt32; [MarshalAs(UnmanagedType.LPArray)] param: array of Int32);
    external 'opengl32.dll' name 'glGetTransformFeedbacki_v';
    static procedure GetTransformFeedbacki_v(xfb: TransformFeedbackName; pname: TransformFeedbackInfoType; index: UInt32; var param: Int32);
    external 'opengl32.dll' name 'glGetTransformFeedbacki_v';
    static procedure GetTransformFeedbacki_v(xfb: TransformFeedbackName; pname: TransformFeedbackInfoType; index: UInt32; param: pointer);
    external 'opengl32.dll' name 'glGetTransformFeedbacki_v';
    
    static procedure GetTransformFeedbacki64_v(xfb: TransformFeedbackName; pname: TransformFeedbackInfoType; index: UInt32; [MarshalAs(UnmanagedType.LPArray)] param: array of Int64);
    external 'opengl32.dll' name 'glGetTransformFeedbacki64_v';
    static procedure GetTransformFeedbacki64_v(xfb: TransformFeedbackName; pname: TransformFeedbackInfoType; index: UInt32; var param: Int64);
    external 'opengl32.dll' name 'glGetTransformFeedbacki64_v';
    static procedure GetTransformFeedbacki64_v(xfb: TransformFeedbackName; pname: TransformFeedbackInfoType; index: UInt32; param: pointer);
    external 'opengl32.dll' name 'glGetTransformFeedbacki64_v';
    
    {$endregion 22.4 - Transform Feedback State Queries}
    
    {$endregion 22.0 - Context State Queries}
    
    
    
    {$region unsorted}
    
    static procedure ProvokingVertex(mode: UInt32);
    external 'opengl32.dll' name 'glProvokingVertex';
    
    static procedure CopyPixels(x: Int32; y: Int32; width: Int32; height: Int32; &type: UInt32);
    external 'opengl32.dll' name 'glCopyPixels';
    
    static function GetGraphicsResetStatus: UInt32;
    external 'opengl32.dll' name 'glGetGraphicsResetStatus';
    
    static procedure PrimitiveBoundingBoxARB(minX: single; minY: single; minZ: single; minW: single; maxX: single; maxY: single; maxZ: single; maxW: single);
    external 'opengl32.dll' name 'glPrimitiveBoundingBoxARB';
    
    static function GetTextureHandleARB(texture: UInt32): UInt64;
    external 'opengl32.dll' name 'glGetTextureHandleARB';
    
    static function GetTextureSamplerHandleARB(texture: UInt32; sampler: UInt32): UInt64;
    external 'opengl32.dll' name 'glGetTextureSamplerHandleARB';
    
    static procedure MakeTextureHandleResidentARB(handle: UInt64);
    external 'opengl32.dll' name 'glMakeTextureHandleResidentARB';
    
    static procedure MakeTextureHandleNonResidentARB(handle: UInt64);
    external 'opengl32.dll' name 'glMakeTextureHandleNonResidentARB';
    
    static function GetImageHandleARB(texture: UInt32; level: Int32; layered: boolean; layer: Int32; format: UInt32): UInt64;
    external 'opengl32.dll' name 'glGetImageHandleARB';
    
    static procedure MakeImageHandleResidentARB(handle: UInt64; access: UInt32);
    external 'opengl32.dll' name 'glMakeImageHandleResidentARB';
    
    static procedure MakeImageHandleNonResidentARB(handle: UInt64);
    external 'opengl32.dll' name 'glMakeImageHandleNonResidentARB';
    
    static procedure UniformHandleui64ARB(location: Int32; value: UInt64);
    external 'opengl32.dll' name 'glUniformHandleui64ARB';
    
    static procedure UniformHandleui64vARB(location: Int32; count: Int32; value: ^UInt64);
    external 'opengl32.dll' name 'glUniformHandleui64vARB';
    
    static procedure ProgramUniformHandleui64ARB(&program: UInt32; location: Int32; value: UInt64);
    external 'opengl32.dll' name 'glProgramUniformHandleui64ARB';
    
    static procedure ProgramUniformHandleui64vARB(&program: UInt32; location: Int32; count: Int32; values: ^UInt64);
    external 'opengl32.dll' name 'glProgramUniformHandleui64vARB';
    
    static function IsTextureHandleResidentARB(handle: UInt64): boolean;
    external 'opengl32.dll' name 'glIsTextureHandleResidentARB';
    
    static function IsImageHandleResidentARB(handle: UInt64): boolean;
    external 'opengl32.dll' name 'glIsImageHandleResidentARB';
    
    static function CreateSyncFromCLeventARB(context: cl_context; &event: cl_event; flags: UInt32): GLsync;
    external 'opengl32.dll' name 'glCreateSyncFromCLeventARB';
    
    static procedure DispatchComputeGroupSizeARB(num_groups_x: UInt32; num_groups_y: UInt32; num_groups_z: UInt32; group_size_x: UInt32; group_size_y: UInt32; group_size_z: UInt32);
    external 'opengl32.dll' name 'glDispatchComputeGroupSizeARB';
    
    static procedure DebugMessageControlARB(source: UInt32; &type: UInt32; severity: UInt32; count: Int32; ids: ^UInt32; enabled: boolean);
    external 'opengl32.dll' name 'glDebugMessageControlARB';
    
    static procedure DebugMessageInsertARB(source: UInt32; &type: UInt32; id: UInt32; severity: UInt32; length: Int32; buf: ^SByte);
    external 'opengl32.dll' name 'glDebugMessageInsertARB';
    
    static procedure DebugMessageCallbackARB(callback: GLDEBUGPROC; userParam: pointer);
    external 'opengl32.dll' name 'glDebugMessageCallbackARB';
    
    static function GetDebugMessageLogARB(count: UInt32; bufSize: Int32; sources: ^UInt32; types: ^UInt32; ids: ^UInt32; severities: ^UInt32; lengths: ^Int32; messageLog: ^SByte): UInt32;
    external 'opengl32.dll' name 'glGetDebugMessageLogARB';
    
    static procedure BlendEquationiARB(buf: UInt32; mode: UInt32);
    external 'opengl32.dll' name 'glBlendEquationiARB';
    
    static procedure BlendEquationSeparateiARB(buf: UInt32; modeRGB: UInt32; modeAlpha: UInt32);
    external 'opengl32.dll' name 'glBlendEquationSeparateiARB';
    
    static procedure BlendFunciARB(buf: UInt32; src: UInt32; dst: UInt32);
    external 'opengl32.dll' name 'glBlendFunciARB';
    
    static procedure BlendFuncSeparateiARB(buf: UInt32; srcRGB: UInt32; dstRGB: UInt32; srcAlpha: UInt32; dstAlpha: UInt32);
    external 'opengl32.dll' name 'glBlendFuncSeparateiARB';
    
    static procedure DrawArraysInstancedARB(mode: UInt32; first: Int32; count: Int32; primcount: Int32);
    external 'opengl32.dll' name 'glDrawArraysInstancedARB';
    
    static procedure DrawElementsInstancedARB(mode: UInt32; count: Int32; &type: UInt32; indices: pointer; primcount: Int32);
    external 'opengl32.dll' name 'glDrawElementsInstancedARB';
    
    static procedure ProgramParameteriARB(&program: UInt32; pname: UInt32; value: Int32);
    external 'opengl32.dll' name 'glProgramParameteriARB';
    
    static procedure FramebufferTextureARB(target: UInt32; attachment: UInt32; texture: UInt32; level: Int32);
    external 'opengl32.dll' name 'glFramebufferTextureARB';
    
    static procedure FramebufferTextureLayerARB(target: UInt32; attachment: UInt32; texture: UInt32; level: Int32; layer: Int32);
    external 'opengl32.dll' name 'glFramebufferTextureLayerARB';
    
    static procedure FramebufferTextureFaceARB(target: UInt32; attachment: UInt32; texture: UInt32; level: Int32; face: UInt32);
    external 'opengl32.dll' name 'glFramebufferTextureFaceARB';
    
    static procedure SpecializeShaderARB(shader: UInt32; pEntryPoint: ^SByte; numSpecializationConstants: UInt32; pConstantIndex: ^UInt32; pConstantValue: ^UInt32);
    external 'opengl32.dll' name 'glSpecializeShaderARB';
    
    static procedure Uniform1i64ARB(location: Int32; x: Int64);
    external 'opengl32.dll' name 'glUniform1i64ARB';
    
    static procedure Uniform2i64ARB(location: Int32; x: Int64; y: Int64);
    external 'opengl32.dll' name 'glUniform2i64ARB';
    
    static procedure Uniform3i64ARB(location: Int32; x: Int64; y: Int64; z: Int64);
    external 'opengl32.dll' name 'glUniform3i64ARB';
    
    static procedure Uniform4i64ARB(location: Int32; x: Int64; y: Int64; z: Int64; w: Int64);
    external 'opengl32.dll' name 'glUniform4i64ARB';
    
    static procedure Uniform1i64vARB(location: Int32; count: Int32; value: ^Int64);
    external 'opengl32.dll' name 'glUniform1i64vARB';
    
    static procedure Uniform2i64vARB(location: Int32; count: Int32; value: ^Int64);
    external 'opengl32.dll' name 'glUniform2i64vARB';
    
    static procedure Uniform3i64vARB(location: Int32; count: Int32; value: ^Int64);
    external 'opengl32.dll' name 'glUniform3i64vARB';
    
    static procedure Uniform4i64vARB(location: Int32; count: Int32; value: ^Int64);
    external 'opengl32.dll' name 'glUniform4i64vARB';
    
    static procedure Uniform1ui64ARB(location: Int32; x: UInt64);
    external 'opengl32.dll' name 'glUniform1ui64ARB';
    
    static procedure Uniform2ui64ARB(location: Int32; x: UInt64; y: UInt64);
    external 'opengl32.dll' name 'glUniform2ui64ARB';
    
    static procedure Uniform3ui64ARB(location: Int32; x: UInt64; y: UInt64; z: UInt64);
    external 'opengl32.dll' name 'glUniform3ui64ARB';
    
    static procedure Uniform4ui64ARB(location: Int32; x: UInt64; y: UInt64; z: UInt64; w: UInt64);
    external 'opengl32.dll' name 'glUniform4ui64ARB';
    
    static procedure Uniform1ui64vARB(location: Int32; count: Int32; value: ^UInt64);
    external 'opengl32.dll' name 'glUniform1ui64vARB';
    
    static procedure Uniform2ui64vARB(location: Int32; count: Int32; value: ^UInt64);
    external 'opengl32.dll' name 'glUniform2ui64vARB';
    
    static procedure Uniform3ui64vARB(location: Int32; count: Int32; value: ^UInt64);
    external 'opengl32.dll' name 'glUniform3ui64vARB';
    
    static procedure Uniform4ui64vARB(location: Int32; count: Int32; value: ^UInt64);
    external 'opengl32.dll' name 'glUniform4ui64vARB';
    
    static procedure GetUniformi64vARB(&program: UInt32; location: Int32; &params: ^Int64);
    external 'opengl32.dll' name 'glGetUniformi64vARB';
    
    static procedure GetUniformui64vARB(&program: UInt32; location: Int32; &params: ^UInt64);
    external 'opengl32.dll' name 'glGetUniformui64vARB';
    
    static procedure GetnUniformi64vARB(&program: UInt32; location: Int32; bufSize: Int32; &params: ^Int64);
    external 'opengl32.dll' name 'glGetnUniformi64vARB';
    
    static procedure GetnUniformui64vARB(&program: UInt32; location: Int32; bufSize: Int32; &params: ^UInt64);
    external 'opengl32.dll' name 'glGetnUniformui64vARB';
    
    static procedure ProgramUniform1i64ARB(&program: UInt32; location: Int32; x: Int64);
    external 'opengl32.dll' name 'glProgramUniform1i64ARB';
    
    static procedure ProgramUniform2i64ARB(&program: UInt32; location: Int32; x: Int64; y: Int64);
    external 'opengl32.dll' name 'glProgramUniform2i64ARB';
    
    static procedure ProgramUniform3i64ARB(&program: UInt32; location: Int32; x: Int64; y: Int64; z: Int64);
    external 'opengl32.dll' name 'glProgramUniform3i64ARB';
    
    static procedure ProgramUniform4i64ARB(&program: UInt32; location: Int32; x: Int64; y: Int64; z: Int64; w: Int64);
    external 'opengl32.dll' name 'glProgramUniform4i64ARB';
    
    static procedure ProgramUniform1i64vARB(&program: UInt32; location: Int32; count: Int32; value: ^Int64);
    external 'opengl32.dll' name 'glProgramUniform1i64vARB';
    
    static procedure ProgramUniform2i64vARB(&program: UInt32; location: Int32; count: Int32; value: ^Int64);
    external 'opengl32.dll' name 'glProgramUniform2i64vARB';
    
    static procedure ProgramUniform3i64vARB(&program: UInt32; location: Int32; count: Int32; value: ^Int64);
    external 'opengl32.dll' name 'glProgramUniform3i64vARB';
    
    static procedure ProgramUniform4i64vARB(&program: UInt32; location: Int32; count: Int32; value: ^Int64);
    external 'opengl32.dll' name 'glProgramUniform4i64vARB';
    
    static procedure ProgramUniform1ui64ARB(&program: UInt32; location: Int32; x: UInt64);
    external 'opengl32.dll' name 'glProgramUniform1ui64ARB';
    
    static procedure ProgramUniform2ui64ARB(&program: UInt32; location: Int32; x: UInt64; y: UInt64);
    external 'opengl32.dll' name 'glProgramUniform2ui64ARB';
    
    static procedure ProgramUniform3ui64ARB(&program: UInt32; location: Int32; x: UInt64; y: UInt64; z: UInt64);
    external 'opengl32.dll' name 'glProgramUniform3ui64ARB';
    
    static procedure ProgramUniform4ui64ARB(&program: UInt32; location: Int32; x: UInt64; y: UInt64; z: UInt64; w: UInt64);
    external 'opengl32.dll' name 'glProgramUniform4ui64ARB';
    
    static procedure ProgramUniform1ui64vARB(&program: UInt32; location: Int32; count: Int32; value: ^UInt64);
    external 'opengl32.dll' name 'glProgramUniform1ui64vARB';
    
    static procedure ProgramUniform2ui64vARB(&program: UInt32; location: Int32; count: Int32; value: ^UInt64);
    external 'opengl32.dll' name 'glProgramUniform2ui64vARB';
    
    static procedure ProgramUniform3ui64vARB(&program: UInt32; location: Int32; count: Int32; value: ^UInt64);
    external 'opengl32.dll' name 'glProgramUniform3ui64vARB';
    
    static procedure ProgramUniform4ui64vARB(&program: UInt32; location: Int32; count: Int32; value: ^UInt64);
    external 'opengl32.dll' name 'glProgramUniform4ui64vARB';
    
    static procedure MultiDrawArraysIndirectCountARB(mode: UInt32; indirect: pointer; drawcount: IntPtr; maxdrawcount: Int32; stride: Int32);
    external 'opengl32.dll' name 'glMultiDrawArraysIndirectCountARB';
    
    static procedure MultiDrawElementsIndirectCountARB(mode: UInt32; &type: UInt32; indirect: pointer; drawcount: IntPtr; maxdrawcount: Int32; stride: Int32);
    external 'opengl32.dll' name 'glMultiDrawElementsIndirectCountARB';
    
    static procedure MaxShaderCompilerThreadsARB(count: UInt32);
    external 'opengl32.dll' name 'glMaxShaderCompilerThreadsARB';
    
    static function GetGraphicsResetStatusARB: UInt32;
    external 'opengl32.dll' name 'glGetGraphicsResetStatusARB';
    
    static procedure GetnTexImageARB(target: UInt32; level: Int32; format: UInt32; &type: UInt32; bufSize: Int32; img: pointer);
    external 'opengl32.dll' name 'glGetnTexImageARB';
    
    static procedure ReadnPixelsARB(x: Int32; y: Int32; width: Int32; height: Int32; format: UInt32; &type: UInt32; bufSize: Int32; data: pointer);
    external 'opengl32.dll' name 'glReadnPixelsARB';
    
    static procedure GetnCompressedTexImageARB(target: UInt32; lod: Int32; bufSize: Int32; img: pointer);
    external 'opengl32.dll' name 'glGetnCompressedTexImageARB';
    
    static procedure GetnUniformfvARB(&program: UInt32; location: Int32; bufSize: Int32; &params: ^single);
    external 'opengl32.dll' name 'glGetnUniformfvARB';
    
    static procedure GetnUniformivARB(&program: UInt32; location: Int32; bufSize: Int32; &params: ^Int32);
    external 'opengl32.dll' name 'glGetnUniformivARB';
    
    static procedure GetnUniformuivARB(&program: UInt32; location: Int32; bufSize: Int32; &params: ^UInt32);
    external 'opengl32.dll' name 'glGetnUniformuivARB';
    
    static procedure GetnUniformdvARB(&program: UInt32; location: Int32; bufSize: Int32; &params: ^real);
    external 'opengl32.dll' name 'glGetnUniformdvARB';
    
    static procedure FramebufferSampleLocationsfvARB(target: UInt32; start: UInt32; count: Int32; v: ^single);
    external 'opengl32.dll' name 'glFramebufferSampleLocationsfvARB';
    
    static procedure NamedFramebufferSampleLocationsfvARB(framebuffer: UInt32; start: UInt32; count: Int32; v: ^single);
    external 'opengl32.dll' name 'glNamedFramebufferSampleLocationsfvARB';
    
    static procedure EvaluateDepthValuesARB;
    external 'opengl32.dll' name 'glEvaluateDepthValuesARB';
    
    static procedure MinSampleShadingARB(value: single);
    external 'opengl32.dll' name 'glMinSampleShadingARB';
    
    static procedure NamedStringARB(&type: UInt32; namelen: Int32; name: ^SByte; stringlen: Int32; string: ^SByte);
    external 'opengl32.dll' name 'glNamedStringARB';
    
    static procedure DeleteNamedStringARB(namelen: Int32; name: ^SByte);
    external 'opengl32.dll' name 'glDeleteNamedStringARB';
    
    static procedure CompileShaderIncludeARB(shader: UInt32; count: Int32; path: ^IntPtr; length: ^Int32);
    external 'opengl32.dll' name 'glCompileShaderIncludeARB';
    
    static function IsNamedStringARB(namelen: Int32; name: ^SByte): boolean;
    external 'opengl32.dll' name 'glIsNamedStringARB';
    
    static procedure GetNamedStringARB(namelen: Int32; name: ^SByte; bufSize: Int32; stringlen: ^Int32; string: ^SByte);
    external 'opengl32.dll' name 'glGetNamedStringARB';
    
    static procedure GetNamedStringivARB(namelen: Int32; name: ^SByte; pname: UInt32; &params: ^Int32);
    external 'opengl32.dll' name 'glGetNamedStringivARB';
    
    static procedure BufferPageCommitmentARB(target: UInt32; offset: IntPtr; size: UIntPtr; commit: boolean);
    external 'opengl32.dll' name 'glBufferPageCommitmentARB';
    
    static procedure NamedBufferPageCommitmentEXT(buffer: UInt32; offset: IntPtr; size: UIntPtr; commit: boolean);
    external 'opengl32.dll' name 'glNamedBufferPageCommitmentEXT';
    
    static procedure NamedBufferPageCommitmentARB(buffer: UInt32; offset: IntPtr; size: UIntPtr; commit: boolean);
    external 'opengl32.dll' name 'glNamedBufferPageCommitmentARB';
    
    static procedure TexPageCommitmentARB(target: UInt32; level: Int32; xoffset: Int32; yoffset: Int32; zoffset: Int32; width: Int32; height: Int32; depth: Int32; commit: boolean);
    external 'opengl32.dll' name 'glTexPageCommitmentARB';
    
    static procedure TexBufferARB(target: UInt32; internalformat: UInt32; buffer: UInt32);
    external 'opengl32.dll' name 'glTexBufferARB';
    
    static procedure BlendBarrierKHR;
    external 'opengl32.dll' name 'glBlendBarrierKHR';
    
    static procedure MaxShaderCompilerThreadsKHR(count: UInt32);
    external 'opengl32.dll' name 'glMaxShaderCompilerThreadsKHR';
    
    static procedure RenderbufferStorageMultisampleAdvancedAMD(target: UInt32; samples: Int32; storageSamples: Int32; internalformat: UInt32; width: Int32; height: Int32);
    external 'opengl32.dll' name 'glRenderbufferStorageMultisampleAdvancedAMD';
    
    static procedure NamedRenderbufferStorageMultisampleAdvancedAMD(renderbuffer: UInt32; samples: Int32; storageSamples: Int32; internalformat: UInt32; width: Int32; height: Int32);
    external 'opengl32.dll' name 'glNamedRenderbufferStorageMultisampleAdvancedAMD';
    
    static procedure GetPerfMonitorGroupsAMD(numGroups: ^Int32; groupsSize: Int32; groups: ^UInt32);
    external 'opengl32.dll' name 'glGetPerfMonitorGroupsAMD';
    
    static procedure GetPerfMonitorCountersAMD(group: UInt32; numCounters: ^Int32; maxActiveCounters: ^Int32; counterSize: Int32; counters: ^UInt32);
    external 'opengl32.dll' name 'glGetPerfMonitorCountersAMD';
    
    static procedure GetPerfMonitorGroupStringAMD(group: UInt32; bufSize: Int32; length: ^Int32; groupString: ^SByte);
    external 'opengl32.dll' name 'glGetPerfMonitorGroupStringAMD';
    
    static procedure GetPerfMonitorCounterStringAMD(group: UInt32; counter: UInt32; bufSize: Int32; length: ^Int32; counterString: ^SByte);
    external 'opengl32.dll' name 'glGetPerfMonitorCounterStringAMD';
    
    static procedure GetPerfMonitorCounterInfoAMD(group: UInt32; counter: UInt32; pname: UInt32; data: pointer);
    external 'opengl32.dll' name 'glGetPerfMonitorCounterInfoAMD';
    
    static procedure GenPerfMonitorsAMD(n: Int32; monitors: ^UInt32);
    external 'opengl32.dll' name 'glGenPerfMonitorsAMD';
    
    static procedure DeletePerfMonitorsAMD(n: Int32; monitors: ^UInt32);
    external 'opengl32.dll' name 'glDeletePerfMonitorsAMD';
    
    static procedure SelectPerfMonitorCountersAMD(monitor: UInt32; enable: boolean; group: UInt32; numCounters: Int32; counterList: ^UInt32);
    external 'opengl32.dll' name 'glSelectPerfMonitorCountersAMD';
    
    static procedure BeginPerfMonitorAMD(monitor: UInt32);
    external 'opengl32.dll' name 'glBeginPerfMonitorAMD';
    
    static procedure EndPerfMonitorAMD(monitor: UInt32);
    external 'opengl32.dll' name 'glEndPerfMonitorAMD';
    
    static procedure GetPerfMonitorCounterDataAMD(monitor: UInt32; pname: UInt32; dataSize: Int32; data: ^UInt32; bytesWritten: ^Int32);
    external 'opengl32.dll' name 'glGetPerfMonitorCounterDataAMD';
    
    static procedure EGLImageTargetTexStorageEXT(target: UInt32; image: GLeglImageOES; attrib_list: ^Int32);
    external 'opengl32.dll' name 'glEGLImageTargetTexStorageEXT';
    
    static procedure EGLImageTargetTextureStorageEXT(texture: UInt32; image: GLeglImageOES; attrib_list: ^Int32);
    external 'opengl32.dll' name 'glEGLImageTargetTextureStorageEXT';
    
    static procedure LabelObjectEXT(&type: UInt32; object: UInt32; length: Int32; &label: ^SByte);
    external 'opengl32.dll' name 'glLabelObjectEXT';
    
    static procedure GetObjectLabelEXT(&type: UInt32; object: UInt32; bufSize: Int32; length: ^Int32; &label: ^SByte);
    external 'opengl32.dll' name 'glGetObjectLabelEXT';
    
    static procedure InsertEventMarkerEXT(length: Int32; marker: ^SByte);
    external 'opengl32.dll' name 'glInsertEventMarkerEXT';
    
    static procedure PushGroupMarkerEXT(length: Int32; marker: ^SByte);
    external 'opengl32.dll' name 'glPushGroupMarkerEXT';
    
    static procedure PopGroupMarkerEXT;
    external 'opengl32.dll' name 'glPopGroupMarkerEXT';
    
    static procedure MatrixLoadfEXT(mode: UInt32; m: ^single);
    external 'opengl32.dll' name 'glMatrixLoadfEXT';
    
    static procedure MatrixLoaddEXT(mode: UInt32; m: ^real);
    external 'opengl32.dll' name 'glMatrixLoaddEXT';
    
    static procedure MatrixMultfEXT(mode: UInt32; m: ^single);
    external 'opengl32.dll' name 'glMatrixMultfEXT';
    
    static procedure MatrixMultdEXT(mode: UInt32; m: ^real);
    external 'opengl32.dll' name 'glMatrixMultdEXT';
    
    static procedure MatrixLoadIdentityEXT(mode: UInt32);
    external 'opengl32.dll' name 'glMatrixLoadIdentityEXT';
    
    static procedure MatrixRotatefEXT(mode: UInt32; angle: single; x: single; y: single; z: single);
    external 'opengl32.dll' name 'glMatrixRotatefEXT';
    
    static procedure MatrixRotatedEXT(mode: UInt32; angle: real; x: real; y: real; z: real);
    external 'opengl32.dll' name 'glMatrixRotatedEXT';
    
    static procedure MatrixScalefEXT(mode: UInt32; x: single; y: single; z: single);
    external 'opengl32.dll' name 'glMatrixScalefEXT';
    
    static procedure MatrixScaledEXT(mode: UInt32; x: real; y: real; z: real);
    external 'opengl32.dll' name 'glMatrixScaledEXT';
    
    static procedure MatrixTranslatefEXT(mode: UInt32; x: single; y: single; z: single);
    external 'opengl32.dll' name 'glMatrixTranslatefEXT';
    
    static procedure MatrixTranslatedEXT(mode: UInt32; x: real; y: real; z: real);
    external 'opengl32.dll' name 'glMatrixTranslatedEXT';
    
    static procedure MatrixFrustumEXT(mode: UInt32; left: real; right: real; bottom: real; top: real; zNear: real; zFar: real);
    external 'opengl32.dll' name 'glMatrixFrustumEXT';
    
    static procedure MatrixOrthoEXT(mode: UInt32; left: real; right: real; bottom: real; top: real; zNear: real; zFar: real);
    external 'opengl32.dll' name 'glMatrixOrthoEXT';
    
    static procedure MatrixPopEXT(mode: UInt32);
    external 'opengl32.dll' name 'glMatrixPopEXT';
    
    static procedure MatrixPushEXT(mode: UInt32);
    external 'opengl32.dll' name 'glMatrixPushEXT';
    
    static procedure ClientAttribDefaultEXT(mask: UInt32);
    external 'opengl32.dll' name 'glClientAttribDefaultEXT';
    
    static procedure PushClientAttribDefaultEXT(mask: UInt32);
    external 'opengl32.dll' name 'glPushClientAttribDefaultEXT';
    
    static procedure TextureParameterfEXT(texture: UInt32; target: UInt32; pname: UInt32; param: single);
    external 'opengl32.dll' name 'glTextureParameterfEXT';
    
    static procedure TextureParameterfvEXT(texture: UInt32; target: UInt32; pname: UInt32; &params: ^single);
    external 'opengl32.dll' name 'glTextureParameterfvEXT';
    
    static procedure TextureParameteriEXT(texture: UInt32; target: UInt32; pname: UInt32; param: Int32);
    external 'opengl32.dll' name 'glTextureParameteriEXT';
    
    static procedure TextureParameterivEXT(texture: UInt32; target: UInt32; pname: UInt32; &params: ^Int32);
    external 'opengl32.dll' name 'glTextureParameterivEXT';
    
    static procedure TextureImage1DEXT(texture: UInt32; target: UInt32; level: Int32; internalformat: Int32; width: Int32; border: Int32; format: UInt32; &type: UInt32; pixels: pointer);
    external 'opengl32.dll' name 'glTextureImage1DEXT';
    
    static procedure TextureImage2DEXT(texture: UInt32; target: UInt32; level: Int32; internalformat: Int32; width: Int32; height: Int32; border: Int32; format: UInt32; &type: UInt32; pixels: pointer);
    external 'opengl32.dll' name 'glTextureImage2DEXT';
    
    static procedure TextureSubImage1DEXT(texture: UInt32; target: UInt32; level: Int32; xoffset: Int32; width: Int32; format: UInt32; &type: UInt32; pixels: pointer);
    external 'opengl32.dll' name 'glTextureSubImage1DEXT';
    
    static procedure TextureSubImage2DEXT(texture: UInt32; target: UInt32; level: Int32; xoffset: Int32; yoffset: Int32; width: Int32; height: Int32; format: UInt32; &type: UInt32; pixels: pointer);
    external 'opengl32.dll' name 'glTextureSubImage2DEXT';
    
    static procedure CopyTextureImage1DEXT(texture: UInt32; target: UInt32; level: Int32; internalformat: UInt32; x: Int32; y: Int32; width: Int32; border: Int32);
    external 'opengl32.dll' name 'glCopyTextureImage1DEXT';
    
    static procedure CopyTextureImage2DEXT(texture: UInt32; target: UInt32; level: Int32; internalformat: UInt32; x: Int32; y: Int32; width: Int32; height: Int32; border: Int32);
    external 'opengl32.dll' name 'glCopyTextureImage2DEXT';
    
    static procedure CopyTextureSubImage1DEXT(texture: UInt32; target: UInt32; level: Int32; xoffset: Int32; x: Int32; y: Int32; width: Int32);
    external 'opengl32.dll' name 'glCopyTextureSubImage1DEXT';
    
    static procedure CopyTextureSubImage2DEXT(texture: UInt32; target: UInt32; level: Int32; xoffset: Int32; yoffset: Int32; x: Int32; y: Int32; width: Int32; height: Int32);
    external 'opengl32.dll' name 'glCopyTextureSubImage2DEXT';
    
    static procedure GetTextureImageEXT(texture: UInt32; target: UInt32; level: Int32; format: UInt32; &type: UInt32; pixels: pointer);
    external 'opengl32.dll' name 'glGetTextureImageEXT';
    
    static procedure GetTextureParameterfvEXT(texture: UInt32; target: UInt32; pname: UInt32; &params: ^single);
    external 'opengl32.dll' name 'glGetTextureParameterfvEXT';
    
    static procedure GetTextureParameterivEXT(texture: UInt32; target: UInt32; pname: UInt32; &params: ^Int32);
    external 'opengl32.dll' name 'glGetTextureParameterivEXT';
    
    static procedure GetTextureLevelParameterfvEXT(texture: UInt32; target: UInt32; level: Int32; pname: UInt32; &params: ^single);
    external 'opengl32.dll' name 'glGetTextureLevelParameterfvEXT';
    
    static procedure GetTextureLevelParameterivEXT(texture: UInt32; target: UInt32; level: Int32; pname: UInt32; &params: ^Int32);
    external 'opengl32.dll' name 'glGetTextureLevelParameterivEXT';
    
    static procedure TextureImage3DEXT(texture: UInt32; target: UInt32; level: Int32; internalformat: Int32; width: Int32; height: Int32; depth: Int32; border: Int32; format: UInt32; &type: UInt32; pixels: pointer);
    external 'opengl32.dll' name 'glTextureImage3DEXT';
    
    static procedure TextureSubImage3DEXT(texture: UInt32; target: UInt32; level: Int32; xoffset: Int32; yoffset: Int32; zoffset: Int32; width: Int32; height: Int32; depth: Int32; format: UInt32; &type: UInt32; pixels: pointer);
    external 'opengl32.dll' name 'glTextureSubImage3DEXT';
    
    static procedure CopyTextureSubImage3DEXT(texture: UInt32; target: UInt32; level: Int32; xoffset: Int32; yoffset: Int32; zoffset: Int32; x: Int32; y: Int32; width: Int32; height: Int32);
    external 'opengl32.dll' name 'glCopyTextureSubImage3DEXT';
    
    static procedure BindMultiTextureEXT(texunit: UInt32; target: UInt32; texture: UInt32);
    external 'opengl32.dll' name 'glBindMultiTextureEXT';
    
    static procedure MultiTexCoordPointerEXT(texunit: UInt32; size: Int32; &type: UInt32; stride: Int32; _pointer: pointer);
    external 'opengl32.dll' name 'glMultiTexCoordPointerEXT';
    
    static procedure MultiTexEnvfEXT(texunit: UInt32; target: UInt32; pname: UInt32; param: single);
    external 'opengl32.dll' name 'glMultiTexEnvfEXT';
    
    static procedure MultiTexEnvfvEXT(texunit: UInt32; target: UInt32; pname: UInt32; &params: ^single);
    external 'opengl32.dll' name 'glMultiTexEnvfvEXT';
    
    static procedure MultiTexEnviEXT(texunit: UInt32; target: UInt32; pname: UInt32; param: Int32);
    external 'opengl32.dll' name 'glMultiTexEnviEXT';
    
    static procedure MultiTexEnvivEXT(texunit: UInt32; target: UInt32; pname: UInt32; &params: ^Int32);
    external 'opengl32.dll' name 'glMultiTexEnvivEXT';
    
    static procedure MultiTexGendEXT(texunit: UInt32; coord: UInt32; pname: UInt32; param: real);
    external 'opengl32.dll' name 'glMultiTexGendEXT';
    
    static procedure MultiTexGendvEXT(texunit: UInt32; coord: UInt32; pname: UInt32; &params: ^real);
    external 'opengl32.dll' name 'glMultiTexGendvEXT';
    
    static procedure MultiTexGenfEXT(texunit: UInt32; coord: UInt32; pname: UInt32; param: single);
    external 'opengl32.dll' name 'glMultiTexGenfEXT';
    
    static procedure MultiTexGenfvEXT(texunit: UInt32; coord: UInt32; pname: UInt32; &params: ^single);
    external 'opengl32.dll' name 'glMultiTexGenfvEXT';
    
    static procedure MultiTexGeniEXT(texunit: UInt32; coord: UInt32; pname: UInt32; param: Int32);
    external 'opengl32.dll' name 'glMultiTexGeniEXT';
    
    static procedure MultiTexGenivEXT(texunit: UInt32; coord: UInt32; pname: UInt32; &params: ^Int32);
    external 'opengl32.dll' name 'glMultiTexGenivEXT';
    
    static procedure GetMultiTexEnvfvEXT(texunit: UInt32; target: UInt32; pname: UInt32; &params: ^single);
    external 'opengl32.dll' name 'glGetMultiTexEnvfvEXT';
    
    static procedure GetMultiTexEnvivEXT(texunit: UInt32; target: UInt32; pname: UInt32; &params: ^Int32);
    external 'opengl32.dll' name 'glGetMultiTexEnvivEXT';
    
    static procedure GetMultiTexGendvEXT(texunit: UInt32; coord: UInt32; pname: UInt32; &params: ^real);
    external 'opengl32.dll' name 'glGetMultiTexGendvEXT';
    
    static procedure GetMultiTexGenfvEXT(texunit: UInt32; coord: UInt32; pname: UInt32; &params: ^single);
    external 'opengl32.dll' name 'glGetMultiTexGenfvEXT';
    
    static procedure GetMultiTexGenivEXT(texunit: UInt32; coord: UInt32; pname: UInt32; &params: ^Int32);
    external 'opengl32.dll' name 'glGetMultiTexGenivEXT';
    
    static procedure MultiTexParameteriEXT(texunit: UInt32; target: UInt32; pname: UInt32; param: Int32);
    external 'opengl32.dll' name 'glMultiTexParameteriEXT';
    
    static procedure MultiTexParameterivEXT(texunit: UInt32; target: UInt32; pname: UInt32; &params: ^Int32);
    external 'opengl32.dll' name 'glMultiTexParameterivEXT';
    
    static procedure MultiTexParameterfEXT(texunit: UInt32; target: UInt32; pname: UInt32; param: single);
    external 'opengl32.dll' name 'glMultiTexParameterfEXT';
    
    static procedure MultiTexParameterfvEXT(texunit: UInt32; target: UInt32; pname: UInt32; &params: ^single);
    external 'opengl32.dll' name 'glMultiTexParameterfvEXT';
    
    static procedure MultiTexImage1DEXT(texunit: UInt32; target: UInt32; level: Int32; internalformat: Int32; width: Int32; border: Int32; format: UInt32; &type: UInt32; pixels: pointer);
    external 'opengl32.dll' name 'glMultiTexImage1DEXT';
    
    static procedure MultiTexImage2DEXT(texunit: UInt32; target: UInt32; level: Int32; internalformat: Int32; width: Int32; height: Int32; border: Int32; format: UInt32; &type: UInt32; pixels: pointer);
    external 'opengl32.dll' name 'glMultiTexImage2DEXT';
    
    static procedure MultiTexSubImage1DEXT(texunit: UInt32; target: UInt32; level: Int32; xoffset: Int32; width: Int32; format: UInt32; &type: UInt32; pixels: pointer);
    external 'opengl32.dll' name 'glMultiTexSubImage1DEXT';
    
    static procedure MultiTexSubImage2DEXT(texunit: UInt32; target: UInt32; level: Int32; xoffset: Int32; yoffset: Int32; width: Int32; height: Int32; format: UInt32; &type: UInt32; pixels: pointer);
    external 'opengl32.dll' name 'glMultiTexSubImage2DEXT';
    
    static procedure CopyMultiTexImage1DEXT(texunit: UInt32; target: UInt32; level: Int32; internalformat: UInt32; x: Int32; y: Int32; width: Int32; border: Int32);
    external 'opengl32.dll' name 'glCopyMultiTexImage1DEXT';
    
    static procedure CopyMultiTexImage2DEXT(texunit: UInt32; target: UInt32; level: Int32; internalformat: UInt32; x: Int32; y: Int32; width: Int32; height: Int32; border: Int32);
    external 'opengl32.dll' name 'glCopyMultiTexImage2DEXT';
    
    static procedure CopyMultiTexSubImage1DEXT(texunit: UInt32; target: UInt32; level: Int32; xoffset: Int32; x: Int32; y: Int32; width: Int32);
    external 'opengl32.dll' name 'glCopyMultiTexSubImage1DEXT';
    
    static procedure CopyMultiTexSubImage2DEXT(texunit: UInt32; target: UInt32; level: Int32; xoffset: Int32; yoffset: Int32; x: Int32; y: Int32; width: Int32; height: Int32);
    external 'opengl32.dll' name 'glCopyMultiTexSubImage2DEXT';
    
    static procedure GetMultiTexImageEXT(texunit: UInt32; target: UInt32; level: Int32; format: UInt32; &type: UInt32; pixels: pointer);
    external 'opengl32.dll' name 'glGetMultiTexImageEXT';
    
    static procedure GetMultiTexParameterfvEXT(texunit: UInt32; target: UInt32; pname: UInt32; &params: ^single);
    external 'opengl32.dll' name 'glGetMultiTexParameterfvEXT';
    
    static procedure GetMultiTexParameterivEXT(texunit: UInt32; target: UInt32; pname: UInt32; &params: ^Int32);
    external 'opengl32.dll' name 'glGetMultiTexParameterivEXT';
    
    static procedure GetMultiTexLevelParameterfvEXT(texunit: UInt32; target: UInt32; level: Int32; pname: UInt32; &params: ^single);
    external 'opengl32.dll' name 'glGetMultiTexLevelParameterfvEXT';
    
    static procedure GetMultiTexLevelParameterivEXT(texunit: UInt32; target: UInt32; level: Int32; pname: UInt32; &params: ^Int32);
    external 'opengl32.dll' name 'glGetMultiTexLevelParameterivEXT';
    
    static procedure MultiTexImage3DEXT(texunit: UInt32; target: UInt32; level: Int32; internalformat: Int32; width: Int32; height: Int32; depth: Int32; border: Int32; format: UInt32; &type: UInt32; pixels: pointer);
    external 'opengl32.dll' name 'glMultiTexImage3DEXT';
    
    static procedure MultiTexSubImage3DEXT(texunit: UInt32; target: UInt32; level: Int32; xoffset: Int32; yoffset: Int32; zoffset: Int32; width: Int32; height: Int32; depth: Int32; format: UInt32; &type: UInt32; pixels: pointer);
    external 'opengl32.dll' name 'glMultiTexSubImage3DEXT';
    
    static procedure CopyMultiTexSubImage3DEXT(texunit: UInt32; target: UInt32; level: Int32; xoffset: Int32; yoffset: Int32; zoffset: Int32; x: Int32; y: Int32; width: Int32; height: Int32);
    external 'opengl32.dll' name 'glCopyMultiTexSubImage3DEXT';
    
    static procedure EnableClientStateIndexedEXT(&array: UInt32; index: UInt32);
    external 'opengl32.dll' name 'glEnableClientStateIndexedEXT';
    
    static procedure DisableClientStateIndexedEXT(&array: UInt32; index: UInt32);
    external 'opengl32.dll' name 'glDisableClientStateIndexedEXT';
    
    static procedure GetFloatIndexedvEXT(target: UInt32; index: UInt32; data: ^single);
    external 'opengl32.dll' name 'glGetFloatIndexedvEXT';
    
    static procedure GetDoubleIndexedvEXT(target: UInt32; index: UInt32; data: ^real);
    external 'opengl32.dll' name 'glGetDoubleIndexedvEXT';
    
    static procedure GetPointerIndexedvEXT(target: UInt32; index: UInt32; data: ^IntPtr);
    external 'opengl32.dll' name 'glGetPointerIndexedvEXT';
    
    static procedure EnableIndexedEXT(target: UInt32; index: UInt32);
    external 'opengl32.dll' name 'glEnableIndexedEXT';
    
    static procedure DisableIndexedEXT(target: UInt32; index: UInt32);
    external 'opengl32.dll' name 'glDisableIndexedEXT';
    
    static function IsEnabledIndexedEXT(target: UInt32; index: UInt32): boolean;
    external 'opengl32.dll' name 'glIsEnabledIndexedEXT';
    
    static procedure GetIntegerIndexedvEXT(target: UInt32; index: UInt32; data: ^Int32);
    external 'opengl32.dll' name 'glGetIntegerIndexedvEXT';
    
    static procedure GetBooleanIndexedvEXT(target: UInt32; index: UInt32; data: ^boolean);
    external 'opengl32.dll' name 'glGetBooleanIndexedvEXT';
    
    static procedure CompressedTextureImage3DEXT(texture: UInt32; target: UInt32; level: Int32; internalformat: UInt32; width: Int32; height: Int32; depth: Int32; border: Int32; imageSize: Int32; bits: pointer);
    external 'opengl32.dll' name 'glCompressedTextureImage3DEXT';
    
    static procedure CompressedTextureImage2DEXT(texture: UInt32; target: UInt32; level: Int32; internalformat: UInt32; width: Int32; height: Int32; border: Int32; imageSize: Int32; bits: pointer);
    external 'opengl32.dll' name 'glCompressedTextureImage2DEXT';
    
    static procedure CompressedTextureImage1DEXT(texture: UInt32; target: UInt32; level: Int32; internalformat: UInt32; width: Int32; border: Int32; imageSize: Int32; bits: pointer);
    external 'opengl32.dll' name 'glCompressedTextureImage1DEXT';
    
    static procedure CompressedTextureSubImage3DEXT(texture: UInt32; target: UInt32; level: Int32; xoffset: Int32; yoffset: Int32; zoffset: Int32; width: Int32; height: Int32; depth: Int32; format: UInt32; imageSize: Int32; bits: pointer);
    external 'opengl32.dll' name 'glCompressedTextureSubImage3DEXT';
    
    static procedure CompressedTextureSubImage2DEXT(texture: UInt32; target: UInt32; level: Int32; xoffset: Int32; yoffset: Int32; width: Int32; height: Int32; format: UInt32; imageSize: Int32; bits: pointer);
    external 'opengl32.dll' name 'glCompressedTextureSubImage2DEXT';
    
    static procedure CompressedTextureSubImage1DEXT(texture: UInt32; target: UInt32; level: Int32; xoffset: Int32; width: Int32; format: UInt32; imageSize: Int32; bits: pointer);
    external 'opengl32.dll' name 'glCompressedTextureSubImage1DEXT';
    
    static procedure GetCompressedTextureImageEXT(texture: UInt32; target: UInt32; lod: Int32; img: pointer);
    external 'opengl32.dll' name 'glGetCompressedTextureImageEXT';
    
    static procedure CompressedMultiTexImage3DEXT(texunit: UInt32; target: UInt32; level: Int32; internalformat: UInt32; width: Int32; height: Int32; depth: Int32; border: Int32; imageSize: Int32; bits: pointer);
    external 'opengl32.dll' name 'glCompressedMultiTexImage3DEXT';
    
    static procedure CompressedMultiTexImage2DEXT(texunit: UInt32; target: UInt32; level: Int32; internalformat: UInt32; width: Int32; height: Int32; border: Int32; imageSize: Int32; bits: pointer);
    external 'opengl32.dll' name 'glCompressedMultiTexImage2DEXT';
    
    static procedure CompressedMultiTexImage1DEXT(texunit: UInt32; target: UInt32; level: Int32; internalformat: UInt32; width: Int32; border: Int32; imageSize: Int32; bits: pointer);
    external 'opengl32.dll' name 'glCompressedMultiTexImage1DEXT';
    
    static procedure CompressedMultiTexSubImage3DEXT(texunit: UInt32; target: UInt32; level: Int32; xoffset: Int32; yoffset: Int32; zoffset: Int32; width: Int32; height: Int32; depth: Int32; format: UInt32; imageSize: Int32; bits: pointer);
    external 'opengl32.dll' name 'glCompressedMultiTexSubImage3DEXT';
    
    static procedure CompressedMultiTexSubImage2DEXT(texunit: UInt32; target: UInt32; level: Int32; xoffset: Int32; yoffset: Int32; width: Int32; height: Int32; format: UInt32; imageSize: Int32; bits: pointer);
    external 'opengl32.dll' name 'glCompressedMultiTexSubImage2DEXT';
    
    static procedure CompressedMultiTexSubImage1DEXT(texunit: UInt32; target: UInt32; level: Int32; xoffset: Int32; width: Int32; format: UInt32; imageSize: Int32; bits: pointer);
    external 'opengl32.dll' name 'glCompressedMultiTexSubImage1DEXT';
    
    static procedure GetCompressedMultiTexImageEXT(texunit: UInt32; target: UInt32; lod: Int32; img: pointer);
    external 'opengl32.dll' name 'glGetCompressedMultiTexImageEXT';
    
    static procedure MatrixLoadTransposefEXT(mode: UInt32; m: ^single);
    external 'opengl32.dll' name 'glMatrixLoadTransposefEXT';
    
    static procedure MatrixLoadTransposedEXT(mode: UInt32; m: ^real);
    external 'opengl32.dll' name 'glMatrixLoadTransposedEXT';
    
    static procedure MatrixMultTransposefEXT(mode: UInt32; m: ^single);
    external 'opengl32.dll' name 'glMatrixMultTransposefEXT';
    
    static procedure MatrixMultTransposedEXT(mode: UInt32; m: ^real);
    external 'opengl32.dll' name 'glMatrixMultTransposedEXT';
    
    static procedure NamedBufferDataEXT(buffer: UInt32; size: UIntPtr; data: pointer; usage: UInt32);
    external 'opengl32.dll' name 'glNamedBufferDataEXT';
    
    static procedure NamedBufferSubDataEXT(buffer: UInt32; offset: IntPtr; size: UIntPtr; data: pointer);
    external 'opengl32.dll' name 'glNamedBufferSubDataEXT';
    
    static function MapNamedBufferEXT(buffer: UInt32; access: UInt32): pointer;
    external 'opengl32.dll' name 'glMapNamedBufferEXT';
    
    static function UnmapNamedBufferEXT(buffer: UInt32): boolean;
    external 'opengl32.dll' name 'glUnmapNamedBufferEXT';
    
    static procedure GetNamedBufferParameterivEXT(buffer: UInt32; pname: UInt32; &params: ^Int32);
    external 'opengl32.dll' name 'glGetNamedBufferParameterivEXT';
    
    static procedure GetNamedBufferPointervEXT(buffer: UInt32; pname: UInt32; &params: ^IntPtr);
    external 'opengl32.dll' name 'glGetNamedBufferPointervEXT';
    
    static procedure GetNamedBufferSubDataEXT(buffer: UInt32; offset: IntPtr; size: UIntPtr; data: pointer);
    external 'opengl32.dll' name 'glGetNamedBufferSubDataEXT';
    
    static procedure ProgramUniform1fEXT(&program: UInt32; location: Int32; v0: single);
    external 'opengl32.dll' name 'glProgramUniform1fEXT';
    
    static procedure ProgramUniform2fEXT(&program: UInt32; location: Int32; v0: single; v1: single);
    external 'opengl32.dll' name 'glProgramUniform2fEXT';
    
    static procedure ProgramUniform3fEXT(&program: UInt32; location: Int32; v0: single; v1: single; v2: single);
    external 'opengl32.dll' name 'glProgramUniform3fEXT';
    
    static procedure ProgramUniform4fEXT(&program: UInt32; location: Int32; v0: single; v1: single; v2: single; v3: single);
    external 'opengl32.dll' name 'glProgramUniform4fEXT';
    
    static procedure ProgramUniform1iEXT(&program: UInt32; location: Int32; v0: Int32);
    external 'opengl32.dll' name 'glProgramUniform1iEXT';
    
    static procedure ProgramUniform2iEXT(&program: UInt32; location: Int32; v0: Int32; v1: Int32);
    external 'opengl32.dll' name 'glProgramUniform2iEXT';
    
    static procedure ProgramUniform3iEXT(&program: UInt32; location: Int32; v0: Int32; v1: Int32; v2: Int32);
    external 'opengl32.dll' name 'glProgramUniform3iEXT';
    
    static procedure ProgramUniform4iEXT(&program: UInt32; location: Int32; v0: Int32; v1: Int32; v2: Int32; v3: Int32);
    external 'opengl32.dll' name 'glProgramUniform4iEXT';
    
    static procedure ProgramUniform1fvEXT(&program: UInt32; location: Int32; count: Int32; value: ^single);
    external 'opengl32.dll' name 'glProgramUniform1fvEXT';
    
    static procedure ProgramUniform2fvEXT(&program: UInt32; location: Int32; count: Int32; value: ^single);
    external 'opengl32.dll' name 'glProgramUniform2fvEXT';
    
    static procedure ProgramUniform3fvEXT(&program: UInt32; location: Int32; count: Int32; value: ^single);
    external 'opengl32.dll' name 'glProgramUniform3fvEXT';
    
    static procedure ProgramUniform4fvEXT(&program: UInt32; location: Int32; count: Int32; value: ^single);
    external 'opengl32.dll' name 'glProgramUniform4fvEXT';
    
    static procedure ProgramUniform1ivEXT(&program: UInt32; location: Int32; count: Int32; value: ^Int32);
    external 'opengl32.dll' name 'glProgramUniform1ivEXT';
    
    static procedure ProgramUniform2ivEXT(&program: UInt32; location: Int32; count: Int32; value: ^Int32);
    external 'opengl32.dll' name 'glProgramUniform2ivEXT';
    
    static procedure ProgramUniform3ivEXT(&program: UInt32; location: Int32; count: Int32; value: ^Int32);
    external 'opengl32.dll' name 'glProgramUniform3ivEXT';
    
    static procedure ProgramUniform4ivEXT(&program: UInt32; location: Int32; count: Int32; value: ^Int32);
    external 'opengl32.dll' name 'glProgramUniform4ivEXT';
    
    static procedure ProgramUniformMatrix2fvEXT(&program: UInt32; location: Int32; count: Int32; transpose: boolean; value: ^single);
    external 'opengl32.dll' name 'glProgramUniformMatrix2fvEXT';
    
    static procedure ProgramUniformMatrix3fvEXT(&program: UInt32; location: Int32; count: Int32; transpose: boolean; value: ^single);
    external 'opengl32.dll' name 'glProgramUniformMatrix3fvEXT';
    
    static procedure ProgramUniformMatrix4fvEXT(&program: UInt32; location: Int32; count: Int32; transpose: boolean; value: ^single);
    external 'opengl32.dll' name 'glProgramUniformMatrix4fvEXT';
    
    static procedure ProgramUniformMatrix2x3fvEXT(&program: UInt32; location: Int32; count: Int32; transpose: boolean; value: ^single);
    external 'opengl32.dll' name 'glProgramUniformMatrix2x3fvEXT';
    
    static procedure ProgramUniformMatrix3x2fvEXT(&program: UInt32; location: Int32; count: Int32; transpose: boolean; value: ^single);
    external 'opengl32.dll' name 'glProgramUniformMatrix3x2fvEXT';
    
    static procedure ProgramUniformMatrix2x4fvEXT(&program: UInt32; location: Int32; count: Int32; transpose: boolean; value: ^single);
    external 'opengl32.dll' name 'glProgramUniformMatrix2x4fvEXT';
    
    static procedure ProgramUniformMatrix4x2fvEXT(&program: UInt32; location: Int32; count: Int32; transpose: boolean; value: ^single);
    external 'opengl32.dll' name 'glProgramUniformMatrix4x2fvEXT';
    
    static procedure ProgramUniformMatrix3x4fvEXT(&program: UInt32; location: Int32; count: Int32; transpose: boolean; value: ^single);
    external 'opengl32.dll' name 'glProgramUniformMatrix3x4fvEXT';
    
    static procedure ProgramUniformMatrix4x3fvEXT(&program: UInt32; location: Int32; count: Int32; transpose: boolean; value: ^single);
    external 'opengl32.dll' name 'glProgramUniformMatrix4x3fvEXT';
    
    static procedure TextureBufferEXT(texture: UInt32; target: UInt32; internalformat: UInt32; buffer: UInt32);
    external 'opengl32.dll' name 'glTextureBufferEXT';
    
    static procedure MultiTexBufferEXT(texunit: UInt32; target: UInt32; internalformat: UInt32; buffer: UInt32);
    external 'opengl32.dll' name 'glMultiTexBufferEXT';
    
    static procedure TextureParameterIivEXT(texture: UInt32; target: UInt32; pname: UInt32; &params: ^Int32);
    external 'opengl32.dll' name 'glTextureParameterIivEXT';
    
    static procedure TextureParameterIuivEXT(texture: UInt32; target: UInt32; pname: UInt32; &params: ^UInt32);
    external 'opengl32.dll' name 'glTextureParameterIuivEXT';
    
    static procedure GetTextureParameterIivEXT(texture: UInt32; target: UInt32; pname: UInt32; &params: ^Int32);
    external 'opengl32.dll' name 'glGetTextureParameterIivEXT';
    
    static procedure GetTextureParameterIuivEXT(texture: UInt32; target: UInt32; pname: UInt32; &params: ^UInt32);
    external 'opengl32.dll' name 'glGetTextureParameterIuivEXT';
    
    static procedure MultiTexParameterIivEXT(texunit: UInt32; target: UInt32; pname: UInt32; &params: ^Int32);
    external 'opengl32.dll' name 'glMultiTexParameterIivEXT';
    
    static procedure MultiTexParameterIuivEXT(texunit: UInt32; target: UInt32; pname: UInt32; &params: ^UInt32);
    external 'opengl32.dll' name 'glMultiTexParameterIuivEXT';
    
    static procedure GetMultiTexParameterIivEXT(texunit: UInt32; target: UInt32; pname: UInt32; &params: ^Int32);
    external 'opengl32.dll' name 'glGetMultiTexParameterIivEXT';
    
    static procedure GetMultiTexParameterIuivEXT(texunit: UInt32; target: UInt32; pname: UInt32; &params: ^UInt32);
    external 'opengl32.dll' name 'glGetMultiTexParameterIuivEXT';
    
    static procedure ProgramUniform1uiEXT(&program: UInt32; location: Int32; v0: UInt32);
    external 'opengl32.dll' name 'glProgramUniform1uiEXT';
    
    static procedure ProgramUniform2uiEXT(&program: UInt32; location: Int32; v0: UInt32; v1: UInt32);
    external 'opengl32.dll' name 'glProgramUniform2uiEXT';
    
    static procedure ProgramUniform3uiEXT(&program: UInt32; location: Int32; v0: UInt32; v1: UInt32; v2: UInt32);
    external 'opengl32.dll' name 'glProgramUniform3uiEXT';
    
    static procedure ProgramUniform4uiEXT(&program: UInt32; location: Int32; v0: UInt32; v1: UInt32; v2: UInt32; v3: UInt32);
    external 'opengl32.dll' name 'glProgramUniform4uiEXT';
    
    static procedure ProgramUniform1uivEXT(&program: UInt32; location: Int32; count: Int32; value: ^UInt32);
    external 'opengl32.dll' name 'glProgramUniform1uivEXT';
    
    static procedure ProgramUniform2uivEXT(&program: UInt32; location: Int32; count: Int32; value: ^UInt32);
    external 'opengl32.dll' name 'glProgramUniform2uivEXT';
    
    static procedure ProgramUniform3uivEXT(&program: UInt32; location: Int32; count: Int32; value: ^UInt32);
    external 'opengl32.dll' name 'glProgramUniform3uivEXT';
    
    static procedure ProgramUniform4uivEXT(&program: UInt32; location: Int32; count: Int32; value: ^UInt32);
    external 'opengl32.dll' name 'glProgramUniform4uivEXT';
    
    static procedure NamedProgramLocalParameters4fvEXT(&program: UInt32; target: UInt32; index: UInt32; count: Int32; &params: ^single);
    external 'opengl32.dll' name 'glNamedProgramLocalParameters4fvEXT';
    
    static procedure NamedProgramLocalParameterI4iEXT(&program: UInt32; target: UInt32; index: UInt32; x: Int32; y: Int32; z: Int32; w: Int32);
    external 'opengl32.dll' name 'glNamedProgramLocalParameterI4iEXT';
    
    static procedure NamedProgramLocalParameterI4ivEXT(&program: UInt32; target: UInt32; index: UInt32; &params: ^Int32);
    external 'opengl32.dll' name 'glNamedProgramLocalParameterI4ivEXT';
    
    static procedure NamedProgramLocalParametersI4ivEXT(&program: UInt32; target: UInt32; index: UInt32; count: Int32; &params: ^Int32);
    external 'opengl32.dll' name 'glNamedProgramLocalParametersI4ivEXT';
    
    static procedure NamedProgramLocalParameterI4uiEXT(&program: UInt32; target: UInt32; index: UInt32; x: UInt32; y: UInt32; z: UInt32; w: UInt32);
    external 'opengl32.dll' name 'glNamedProgramLocalParameterI4uiEXT';
    
    static procedure NamedProgramLocalParameterI4uivEXT(&program: UInt32; target: UInt32; index: UInt32; &params: ^UInt32);
    external 'opengl32.dll' name 'glNamedProgramLocalParameterI4uivEXT';
    
    static procedure NamedProgramLocalParametersI4uivEXT(&program: UInt32; target: UInt32; index: UInt32; count: Int32; &params: ^UInt32);
    external 'opengl32.dll' name 'glNamedProgramLocalParametersI4uivEXT';
    
    static procedure GetNamedProgramLocalParameterIivEXT(&program: UInt32; target: UInt32; index: UInt32; &params: ^Int32);
    external 'opengl32.dll' name 'glGetNamedProgramLocalParameterIivEXT';
    
    static procedure GetNamedProgramLocalParameterIuivEXT(&program: UInt32; target: UInt32; index: UInt32; &params: ^UInt32);
    external 'opengl32.dll' name 'glGetNamedProgramLocalParameterIuivEXT';
    
    static procedure EnableClientStateiEXT(&array: UInt32; index: UInt32);
    external 'opengl32.dll' name 'glEnableClientStateiEXT';
    
    static procedure DisableClientStateiEXT(&array: UInt32; index: UInt32);
    external 'opengl32.dll' name 'glDisableClientStateiEXT';
    
    static procedure GetFloati_vEXT(pname: UInt32; index: UInt32; &params: ^single);
    external 'opengl32.dll' name 'glGetFloati_vEXT';
    
    static procedure GetDoublei_vEXT(pname: UInt32; index: UInt32; &params: ^real);
    external 'opengl32.dll' name 'glGetDoublei_vEXT';
    
    static procedure GetPointeri_vEXT(pname: UInt32; index: UInt32; &params: ^IntPtr);
    external 'opengl32.dll' name 'glGetPointeri_vEXT';
    
    static procedure NamedProgramStringEXT(&program: UInt32; target: UInt32; format: UInt32; len: Int32; string: pointer);
    external 'opengl32.dll' name 'glNamedProgramStringEXT';
    
    static procedure NamedProgramLocalParameter4dEXT(&program: UInt32; target: UInt32; index: UInt32; x: real; y: real; z: real; w: real);
    external 'opengl32.dll' name 'glNamedProgramLocalParameter4dEXT';
    
    static procedure NamedProgramLocalParameter4dvEXT(&program: UInt32; target: UInt32; index: UInt32; &params: ^real);
    external 'opengl32.dll' name 'glNamedProgramLocalParameter4dvEXT';
    
    static procedure NamedProgramLocalParameter4fEXT(&program: UInt32; target: UInt32; index: UInt32; x: single; y: single; z: single; w: single);
    external 'opengl32.dll' name 'glNamedProgramLocalParameter4fEXT';
    
    static procedure NamedProgramLocalParameter4fvEXT(&program: UInt32; target: UInt32; index: UInt32; &params: ^single);
    external 'opengl32.dll' name 'glNamedProgramLocalParameter4fvEXT';
    
    static procedure GetNamedProgramLocalParameterdvEXT(&program: UInt32; target: UInt32; index: UInt32; &params: ^real);
    external 'opengl32.dll' name 'glGetNamedProgramLocalParameterdvEXT';
    
    static procedure GetNamedProgramLocalParameterfvEXT(&program: UInt32; target: UInt32; index: UInt32; &params: ^single);
    external 'opengl32.dll' name 'glGetNamedProgramLocalParameterfvEXT';
    
    static procedure GetNamedProgramivEXT(&program: UInt32; target: UInt32; pname: UInt32; &params: ^Int32);
    external 'opengl32.dll' name 'glGetNamedProgramivEXT';
    
    static procedure GetNamedProgramStringEXT(&program: UInt32; target: UInt32; pname: UInt32; string: pointer);
    external 'opengl32.dll' name 'glGetNamedProgramStringEXT';
    
    static procedure NamedRenderbufferStorageEXT(renderbuffer: UInt32; internalformat: UInt32; width: Int32; height: Int32);
    external 'opengl32.dll' name 'glNamedRenderbufferStorageEXT';
    
    static procedure GetNamedRenderbufferParameterivEXT(renderbuffer: UInt32; pname: UInt32; &params: ^Int32);
    external 'opengl32.dll' name 'glGetNamedRenderbufferParameterivEXT';
    
    static procedure NamedRenderbufferStorageMultisampleEXT(renderbuffer: UInt32; samples: Int32; internalformat: UInt32; width: Int32; height: Int32);
    external 'opengl32.dll' name 'glNamedRenderbufferStorageMultisampleEXT';
    
    static procedure NamedRenderbufferStorageMultisampleCoverageEXT(renderbuffer: UInt32; coverageSamples: Int32; colorSamples: Int32; internalformat: UInt32; width: Int32; height: Int32);
    external 'opengl32.dll' name 'glNamedRenderbufferStorageMultisampleCoverageEXT';
    
    static function CheckNamedFramebufferStatusEXT(framebuffer: UInt32; target: UInt32): UInt32;
    external 'opengl32.dll' name 'glCheckNamedFramebufferStatusEXT';
    
    static procedure NamedFramebufferTexture1DEXT(framebuffer: UInt32; attachment: UInt32; textarget: UInt32; texture: UInt32; level: Int32);
    external 'opengl32.dll' name 'glNamedFramebufferTexture1DEXT';
    
    static procedure NamedFramebufferTexture2DEXT(framebuffer: UInt32; attachment: UInt32; textarget: UInt32; texture: UInt32; level: Int32);
    external 'opengl32.dll' name 'glNamedFramebufferTexture2DEXT';
    
    static procedure NamedFramebufferTexture3DEXT(framebuffer: UInt32; attachment: UInt32; textarget: UInt32; texture: UInt32; level: Int32; zoffset: Int32);
    external 'opengl32.dll' name 'glNamedFramebufferTexture3DEXT';
    
    static procedure NamedFramebufferRenderbufferEXT(framebuffer: UInt32; attachment: UInt32; renderbuffertarget: UInt32; renderbuffer: UInt32);
    external 'opengl32.dll' name 'glNamedFramebufferRenderbufferEXT';
    
    static procedure GetNamedFramebufferAttachmentParameterivEXT(framebuffer: UInt32; attachment: UInt32; pname: UInt32; &params: ^Int32);
    external 'opengl32.dll' name 'glGetNamedFramebufferAttachmentParameterivEXT';
    
    static procedure GenerateTextureMipmapEXT(texture: UInt32; target: UInt32);
    external 'opengl32.dll' name 'glGenerateTextureMipmapEXT';
    
    static procedure GenerateMultiTexMipmapEXT(texunit: UInt32; target: UInt32);
    external 'opengl32.dll' name 'glGenerateMultiTexMipmapEXT';
    
    static procedure FramebufferDrawBufferEXT(framebuffer: UInt32; mode: UInt32);
    external 'opengl32.dll' name 'glFramebufferDrawBufferEXT';
    
    static procedure FramebufferDrawBuffersEXT(framebuffer: UInt32; n: Int32; bufs: ^UInt32);
    external 'opengl32.dll' name 'glFramebufferDrawBuffersEXT';
    
    static procedure FramebufferReadBufferEXT(framebuffer: UInt32; mode: UInt32);
    external 'opengl32.dll' name 'glFramebufferReadBufferEXT';
    
    static procedure GetFramebufferParameterivEXT(framebuffer: UInt32; pname: UInt32; &params: ^Int32);
    external 'opengl32.dll' name 'glGetFramebufferParameterivEXT';
    
    static procedure NamedCopyBufferSubDataEXT(readBuffer: UInt32; writeBuffer: UInt32; readOffset: IntPtr; writeOffset: IntPtr; size: UIntPtr);
    external 'opengl32.dll' name 'glNamedCopyBufferSubDataEXT';
    
    static procedure NamedFramebufferTextureEXT(framebuffer: UInt32; attachment: UInt32; texture: UInt32; level: Int32);
    external 'opengl32.dll' name 'glNamedFramebufferTextureEXT';
    
    static procedure NamedFramebufferTextureLayerEXT(framebuffer: UInt32; attachment: UInt32; texture: UInt32; level: Int32; layer: Int32);
    external 'opengl32.dll' name 'glNamedFramebufferTextureLayerEXT';
    
    static procedure NamedFramebufferTextureFaceEXT(framebuffer: UInt32; attachment: UInt32; texture: UInt32; level: Int32; face: UInt32);
    external 'opengl32.dll' name 'glNamedFramebufferTextureFaceEXT';
    
    static procedure TextureRenderbufferEXT(texture: UInt32; target: UInt32; renderbuffer: UInt32);
    external 'opengl32.dll' name 'glTextureRenderbufferEXT';
    
    static procedure MultiTexRenderbufferEXT(texunit: UInt32; target: UInt32; renderbuffer: UInt32);
    external 'opengl32.dll' name 'glMultiTexRenderbufferEXT';
    
    static procedure VertexArrayVertexOffsetEXT(vaobj: UInt32; buffer: UInt32; size: Int32; &type: UInt32; stride: Int32; offset: IntPtr);
    external 'opengl32.dll' name 'glVertexArrayVertexOffsetEXT';
    
    static procedure VertexArrayColorOffsetEXT(vaobj: UInt32; buffer: UInt32; size: Int32; &type: UInt32; stride: Int32; offset: IntPtr);
    external 'opengl32.dll' name 'glVertexArrayColorOffsetEXT';
    
    static procedure VertexArrayEdgeFlagOffsetEXT(vaobj: UInt32; buffer: UInt32; stride: Int32; offset: IntPtr);
    external 'opengl32.dll' name 'glVertexArrayEdgeFlagOffsetEXT';
    
    static procedure VertexArrayIndexOffsetEXT(vaobj: UInt32; buffer: UInt32; &type: UInt32; stride: Int32; offset: IntPtr);
    external 'opengl32.dll' name 'glVertexArrayIndexOffsetEXT';
    
    static procedure VertexArrayNormalOffsetEXT(vaobj: UInt32; buffer: UInt32; &type: UInt32; stride: Int32; offset: IntPtr);
    external 'opengl32.dll' name 'glVertexArrayNormalOffsetEXT';
    
    static procedure VertexArrayTexCoordOffsetEXT(vaobj: UInt32; buffer: UInt32; size: Int32; &type: UInt32; stride: Int32; offset: IntPtr);
    external 'opengl32.dll' name 'glVertexArrayTexCoordOffsetEXT';
    
    static procedure VertexArrayMultiTexCoordOffsetEXT(vaobj: UInt32; buffer: UInt32; texunit: UInt32; size: Int32; &type: UInt32; stride: Int32; offset: IntPtr);
    external 'opengl32.dll' name 'glVertexArrayMultiTexCoordOffsetEXT';
    
    static procedure VertexArrayFogCoordOffsetEXT(vaobj: UInt32; buffer: UInt32; &type: UInt32; stride: Int32; offset: IntPtr);
    external 'opengl32.dll' name 'glVertexArrayFogCoordOffsetEXT';
    
    static procedure VertexArraySecondaryColorOffsetEXT(vaobj: UInt32; buffer: UInt32; size: Int32; &type: UInt32; stride: Int32; offset: IntPtr);
    external 'opengl32.dll' name 'glVertexArraySecondaryColorOffsetEXT';
    
    static procedure VertexArrayVertexAttribOffsetEXT(vaobj: UInt32; buffer: UInt32; index: UInt32; size: Int32; &type: UInt32; normalized: boolean; stride: Int32; offset: IntPtr);
    external 'opengl32.dll' name 'glVertexArrayVertexAttribOffsetEXT';
    
    static procedure VertexArrayVertexAttribIOffsetEXT(vaobj: UInt32; buffer: UInt32; index: UInt32; size: Int32; &type: UInt32; stride: Int32; offset: IntPtr);
    external 'opengl32.dll' name 'glVertexArrayVertexAttribIOffsetEXT';
    
    static procedure EnableVertexArrayEXT(vaobj: UInt32; &array: UInt32);
    external 'opengl32.dll' name 'glEnableVertexArrayEXT';
    
    static procedure DisableVertexArrayEXT(vaobj: UInt32; &array: UInt32);
    external 'opengl32.dll' name 'glDisableVertexArrayEXT';
    
    static procedure EnableVertexArrayAttribEXT(vaobj: UInt32; index: UInt32);
    external 'opengl32.dll' name 'glEnableVertexArrayAttribEXT';
    
    static procedure DisableVertexArrayAttribEXT(vaobj: UInt32; index: UInt32);
    external 'opengl32.dll' name 'glDisableVertexArrayAttribEXT';
    
    static procedure GetVertexArrayIntegervEXT(vaobj: UInt32; pname: UInt32; param: ^Int32);
    external 'opengl32.dll' name 'glGetVertexArrayIntegervEXT';
    
    static procedure GetVertexArrayPointervEXT(vaobj: UInt32; pname: UInt32; param: ^IntPtr);
    external 'opengl32.dll' name 'glGetVertexArrayPointervEXT';
    
    static procedure GetVertexArrayIntegeri_vEXT(vaobj: UInt32; index: UInt32; pname: UInt32; param: ^Int32);
    external 'opengl32.dll' name 'glGetVertexArrayIntegeri_vEXT';
    
    static procedure GetVertexArrayPointeri_vEXT(vaobj: UInt32; index: UInt32; pname: UInt32; param: ^IntPtr);
    external 'opengl32.dll' name 'glGetVertexArrayPointeri_vEXT';
    
    static function MapNamedBufferRangeEXT(buffer: UInt32; offset: IntPtr; length: UIntPtr; access: UInt32): pointer;
    external 'opengl32.dll' name 'glMapNamedBufferRangeEXT';
    
    static procedure FlushMappedNamedBufferRangeEXT(buffer: UInt32; offset: IntPtr; length: UIntPtr);
    external 'opengl32.dll' name 'glFlushMappedNamedBufferRangeEXT';
    
    static procedure NamedBufferStorageEXT(buffer: UInt32; size: UIntPtr; data: pointer; flags: UInt32);
    external 'opengl32.dll' name 'glNamedBufferStorageEXT';
    
    static procedure ClearNamedBufferDataEXT(buffer: UInt32; internalformat: UInt32; format: UInt32; &type: UInt32; data: pointer);
    external 'opengl32.dll' name 'glClearNamedBufferDataEXT';
    
    static procedure ClearNamedBufferSubDataEXT(buffer: UInt32; internalformat: UInt32; offset: UIntPtr; size: UIntPtr; format: UInt32; &type: UInt32; data: pointer);
    external 'opengl32.dll' name 'glClearNamedBufferSubDataEXT';
    
    static procedure NamedFramebufferParameteriEXT(framebuffer: UInt32; pname: UInt32; param: Int32);
    external 'opengl32.dll' name 'glNamedFramebufferParameteriEXT';
    
    static procedure GetNamedFramebufferParameterivEXT(framebuffer: UInt32; pname: UInt32; &params: ^Int32);
    external 'opengl32.dll' name 'glGetNamedFramebufferParameterivEXT';
    
    static procedure ProgramUniform1dEXT(&program: UInt32; location: Int32; x: real);
    external 'opengl32.dll' name 'glProgramUniform1dEXT';
    
    static procedure ProgramUniform2dEXT(&program: UInt32; location: Int32; x: real; y: real);
    external 'opengl32.dll' name 'glProgramUniform2dEXT';
    
    static procedure ProgramUniform3dEXT(&program: UInt32; location: Int32; x: real; y: real; z: real);
    external 'opengl32.dll' name 'glProgramUniform3dEXT';
    
    static procedure ProgramUniform4dEXT(&program: UInt32; location: Int32; x: real; y: real; z: real; w: real);
    external 'opengl32.dll' name 'glProgramUniform4dEXT';
    
    static procedure ProgramUniform1dvEXT(&program: UInt32; location: Int32; count: Int32; value: ^real);
    external 'opengl32.dll' name 'glProgramUniform1dvEXT';
    
    static procedure ProgramUniform2dvEXT(&program: UInt32; location: Int32; count: Int32; value: ^real);
    external 'opengl32.dll' name 'glProgramUniform2dvEXT';
    
    static procedure ProgramUniform3dvEXT(&program: UInt32; location: Int32; count: Int32; value: ^real);
    external 'opengl32.dll' name 'glProgramUniform3dvEXT';
    
    static procedure ProgramUniform4dvEXT(&program: UInt32; location: Int32; count: Int32; value: ^real);
    external 'opengl32.dll' name 'glProgramUniform4dvEXT';
    
    static procedure ProgramUniformMatrix2dvEXT(&program: UInt32; location: Int32; count: Int32; transpose: boolean; value: ^real);
    external 'opengl32.dll' name 'glProgramUniformMatrix2dvEXT';
    
    static procedure ProgramUniformMatrix3dvEXT(&program: UInt32; location: Int32; count: Int32; transpose: boolean; value: ^real);
    external 'opengl32.dll' name 'glProgramUniformMatrix3dvEXT';
    
    static procedure ProgramUniformMatrix4dvEXT(&program: UInt32; location: Int32; count: Int32; transpose: boolean; value: ^real);
    external 'opengl32.dll' name 'glProgramUniformMatrix4dvEXT';
    
    static procedure ProgramUniformMatrix2x3dvEXT(&program: UInt32; location: Int32; count: Int32; transpose: boolean; value: ^real);
    external 'opengl32.dll' name 'glProgramUniformMatrix2x3dvEXT';
    
    static procedure ProgramUniformMatrix2x4dvEXT(&program: UInt32; location: Int32; count: Int32; transpose: boolean; value: ^real);
    external 'opengl32.dll' name 'glProgramUniformMatrix2x4dvEXT';
    
    static procedure ProgramUniformMatrix3x2dvEXT(&program: UInt32; location: Int32; count: Int32; transpose: boolean; value: ^real);
    external 'opengl32.dll' name 'glProgramUniformMatrix3x2dvEXT';
    
    static procedure ProgramUniformMatrix3x4dvEXT(&program: UInt32; location: Int32; count: Int32; transpose: boolean; value: ^real);
    external 'opengl32.dll' name 'glProgramUniformMatrix3x4dvEXT';
    
    static procedure ProgramUniformMatrix4x2dvEXT(&program: UInt32; location: Int32; count: Int32; transpose: boolean; value: ^real);
    external 'opengl32.dll' name 'glProgramUniformMatrix4x2dvEXT';
    
    static procedure ProgramUniformMatrix4x3dvEXT(&program: UInt32; location: Int32; count: Int32; transpose: boolean; value: ^real);
    external 'opengl32.dll' name 'glProgramUniformMatrix4x3dvEXT';
    
    static procedure TextureBufferRangeEXT(texture: UInt32; target: UInt32; internalformat: UInt32; buffer: UInt32; offset: IntPtr; size: UIntPtr);
    external 'opengl32.dll' name 'glTextureBufferRangeEXT';
    
    static procedure TextureStorage1DEXT(texture: UInt32; target: UInt32; levels: Int32; internalformat: UInt32; width: Int32);
    external 'opengl32.dll' name 'glTextureStorage1DEXT';
    
    static procedure TextureStorage2DEXT(texture: UInt32; target: UInt32; levels: Int32; internalformat: UInt32; width: Int32; height: Int32);
    external 'opengl32.dll' name 'glTextureStorage2DEXT';
    
    static procedure TextureStorage3DEXT(texture: UInt32; target: UInt32; levels: Int32; internalformat: UInt32; width: Int32; height: Int32; depth: Int32);
    external 'opengl32.dll' name 'glTextureStorage3DEXT';
    
    static procedure TextureStorage2DMultisampleEXT(texture: UInt32; target: UInt32; samples: Int32; internalformat: UInt32; width: Int32; height: Int32; fixedsamplelocations: boolean);
    external 'opengl32.dll' name 'glTextureStorage2DMultisampleEXT';
    
    static procedure TextureStorage3DMultisampleEXT(texture: UInt32; target: UInt32; samples: Int32; internalformat: UInt32; width: Int32; height: Int32; depth: Int32; fixedsamplelocations: boolean);
    external 'opengl32.dll' name 'glTextureStorage3DMultisampleEXT';
    
    static procedure VertexArrayBindVertexBufferEXT(vaobj: UInt32; bindingindex: UInt32; buffer: UInt32; offset: IntPtr; stride: Int32);
    external 'opengl32.dll' name 'glVertexArrayBindVertexBufferEXT';
    
    static procedure VertexArrayVertexAttribFormatEXT(vaobj: UInt32; attribindex: UInt32; size: Int32; &type: UInt32; normalized: boolean; relativeoffset: UInt32);
    external 'opengl32.dll' name 'glVertexArrayVertexAttribFormatEXT';
    
    static procedure VertexArrayVertexAttribIFormatEXT(vaobj: UInt32; attribindex: UInt32; size: Int32; &type: UInt32; relativeoffset: UInt32);
    external 'opengl32.dll' name 'glVertexArrayVertexAttribIFormatEXT';
    
    static procedure VertexArrayVertexAttribLFormatEXT(vaobj: UInt32; attribindex: UInt32; size: Int32; &type: UInt32; relativeoffset: UInt32);
    external 'opengl32.dll' name 'glVertexArrayVertexAttribLFormatEXT';
    
    static procedure VertexArrayVertexAttribBindingEXT(vaobj: UInt32; attribindex: UInt32; bindingindex: UInt32);
    external 'opengl32.dll' name 'glVertexArrayVertexAttribBindingEXT';
    
    static procedure VertexArrayVertexBindingDivisorEXT(vaobj: UInt32; bindingindex: UInt32; divisor: UInt32);
    external 'opengl32.dll' name 'glVertexArrayVertexBindingDivisorEXT';
    
    static procedure VertexArrayVertexAttribLOffsetEXT(vaobj: UInt32; buffer: UInt32; index: UInt32; size: Int32; &type: UInt32; stride: Int32; offset: IntPtr);
    external 'opengl32.dll' name 'glVertexArrayVertexAttribLOffsetEXT';
    
    static procedure TexturePageCommitmentEXT(texture: UInt32; level: Int32; xoffset: Int32; yoffset: Int32; zoffset: Int32; width: Int32; height: Int32; depth: Int32; commit: boolean);
    external 'opengl32.dll' name 'glTexturePageCommitmentEXT';
    
    static procedure VertexArrayVertexAttribDivisorEXT(vaobj: UInt32; index: UInt32; divisor: UInt32);
    external 'opengl32.dll' name 'glVertexArrayVertexAttribDivisorEXT';
    
    static procedure DrawArraysInstancedEXT(mode: UInt32; start: Int32; count: Int32; primcount: Int32);
    external 'opengl32.dll' name 'glDrawArraysInstancedEXT';
    
    static procedure DrawElementsInstancedEXT(mode: UInt32; count: Int32; &type: UInt32; indices: pointer; primcount: Int32);
    external 'opengl32.dll' name 'glDrawElementsInstancedEXT';
    
    static procedure PolygonOffsetClampEXT(factor: single; units: single; clamp: single);
    external 'opengl32.dll' name 'glPolygonOffsetClampEXT';
    
    static procedure RasterSamplesEXT(samples: UInt32; fixedsamplelocations: boolean);
    external 'opengl32.dll' name 'glRasterSamplesEXT';
    
    static procedure UseShaderProgramEXT(&type: UInt32; &program: UInt32);
    external 'opengl32.dll' name 'glUseShaderProgramEXT';
    
    static procedure ActiveProgramEXT(&program: UInt32);
    external 'opengl32.dll' name 'glActiveProgramEXT';
    
    static function CreateShaderProgramEXT(&type: UInt32; string: ^SByte): UInt32;
    external 'opengl32.dll' name 'glCreateShaderProgramEXT';
    
    static procedure FramebufferFetchBarrierEXT;
    external 'opengl32.dll' name 'glFramebufferFetchBarrierEXT';
    
    static procedure WindowRectanglesEXT(mode: UInt32; count: Int32; box: ^Int32);
    external 'opengl32.dll' name 'glWindowRectanglesEXT';
    
    static procedure ApplyFramebufferAttachmentCMAAINTEL;
    external 'opengl32.dll' name 'glApplyFramebufferAttachmentCMAAINTEL';
    
    static procedure BeginPerfQueryINTEL(queryHandle: UInt32);
    external 'opengl32.dll' name 'glBeginPerfQueryINTEL';
    
    static procedure CreatePerfQueryINTEL(queryId: UInt32; queryHandle: ^UInt32);
    external 'opengl32.dll' name 'glCreatePerfQueryINTEL';
    
    static procedure DeletePerfQueryINTEL(queryHandle: UInt32);
    external 'opengl32.dll' name 'glDeletePerfQueryINTEL';
    
    static procedure EndPerfQueryINTEL(queryHandle: UInt32);
    external 'opengl32.dll' name 'glEndPerfQueryINTEL';
    
    static procedure GetFirstPerfQueryIdINTEL(queryId: ^UInt32);
    external 'opengl32.dll' name 'glGetFirstPerfQueryIdINTEL';
    
    static procedure GetNextPerfQueryIdINTEL(queryId: UInt32; nextQueryId: ^UInt32);
    external 'opengl32.dll' name 'glGetNextPerfQueryIdINTEL';
    
    static procedure GetPerfCounterInfoINTEL(queryId: UInt32; counterId: UInt32; counterNameLength: UInt32; counterName: ^SByte; counterDescLength: UInt32; counterDesc: ^SByte; counterOffset: ^UInt32; counterDataSize: ^UInt32; counterTypeEnum: ^UInt32; counterDataTypeEnum: ^UInt32; rawCounterMaxValue: ^UInt64);
    external 'opengl32.dll' name 'glGetPerfCounterInfoINTEL';
    
    static procedure GetPerfQueryDataINTEL(queryHandle: UInt32; flags: UInt32; dataSize: Int32; data: pointer; bytesWritten: ^UInt32);
    external 'opengl32.dll' name 'glGetPerfQueryDataINTEL';
    
    static procedure GetPerfQueryIdByNameINTEL(queryName: ^SByte; queryId: ^UInt32);
    external 'opengl32.dll' name 'glGetPerfQueryIdByNameINTEL';
    
    static procedure GetPerfQueryInfoINTEL(queryId: UInt32; queryNameLength: UInt32; queryName: ^SByte; dataSize: ^UInt32; noCounters: ^UInt32; noInstances: ^UInt32; capsMask: ^UInt32);
    external 'opengl32.dll' name 'glGetPerfQueryInfoINTEL';
    
    static procedure MultiDrawArraysIndirectBindlessNV(mode: UInt32; indirect: pointer; drawCount: Int32; stride: Int32; vertexBufferCount: Int32);
    external 'opengl32.dll' name 'glMultiDrawArraysIndirectBindlessNV';
    
    static procedure MultiDrawElementsIndirectBindlessNV(mode: UInt32; &type: UInt32; indirect: pointer; drawCount: Int32; stride: Int32; vertexBufferCount: Int32);
    external 'opengl32.dll' name 'glMultiDrawElementsIndirectBindlessNV';
    
    static procedure MultiDrawArraysIndirectBindlessCountNV(mode: UInt32; indirect: pointer; drawCount: Int32; maxDrawCount: Int32; stride: Int32; vertexBufferCount: Int32);
    external 'opengl32.dll' name 'glMultiDrawArraysIndirectBindlessCountNV';
    
    static procedure MultiDrawElementsIndirectBindlessCountNV(mode: UInt32; &type: UInt32; indirect: pointer; drawCount: Int32; maxDrawCount: Int32; stride: Int32; vertexBufferCount: Int32);
    external 'opengl32.dll' name 'glMultiDrawElementsIndirectBindlessCountNV';
    
    static function GetTextureHandleNV(texture: UInt32): UInt64;
    external 'opengl32.dll' name 'glGetTextureHandleNV';
    
    static function GetTextureSamplerHandleNV(texture: UInt32; sampler: UInt32): UInt64;
    external 'opengl32.dll' name 'glGetTextureSamplerHandleNV';
    
    static procedure MakeTextureHandleResidentNV(handle: UInt64);
    external 'opengl32.dll' name 'glMakeTextureHandleResidentNV';
    
    static procedure MakeTextureHandleNonResidentNV(handle: UInt64);
    external 'opengl32.dll' name 'glMakeTextureHandleNonResidentNV';
    
    static function GetImageHandleNV(texture: UInt32; level: Int32; layered: boolean; layer: Int32; format: UInt32): UInt64;
    external 'opengl32.dll' name 'glGetImageHandleNV';
    
    static procedure MakeImageHandleResidentNV(handle: UInt64; access: UInt32);
    external 'opengl32.dll' name 'glMakeImageHandleResidentNV';
    
    static procedure MakeImageHandleNonResidentNV(handle: UInt64);
    external 'opengl32.dll' name 'glMakeImageHandleNonResidentNV';
    
    static procedure UniformHandleui64NV(location: Int32; value: UInt64);
    external 'opengl32.dll' name 'glUniformHandleui64NV';
    
    static procedure UniformHandleui64vNV(location: Int32; count: Int32; value: ^UInt64);
    external 'opengl32.dll' name 'glUniformHandleui64vNV';
    
    static procedure ProgramUniformHandleui64NV(&program: UInt32; location: Int32; value: UInt64);
    external 'opengl32.dll' name 'glProgramUniformHandleui64NV';
    
    static procedure ProgramUniformHandleui64vNV(&program: UInt32; location: Int32; count: Int32; values: ^UInt64);
    external 'opengl32.dll' name 'glProgramUniformHandleui64vNV';
    
    static function IsTextureHandleResidentNV(handle: UInt64): boolean;
    external 'opengl32.dll' name 'glIsTextureHandleResidentNV';
    
    static function IsImageHandleResidentNV(handle: UInt64): boolean;
    external 'opengl32.dll' name 'glIsImageHandleResidentNV';
    
    static procedure BlendParameteriNV(pname: UInt32; value: Int32);
    external 'opengl32.dll' name 'glBlendParameteriNV';
    
    static procedure BlendBarrierNV;
    external 'opengl32.dll' name 'glBlendBarrierNV';
    
    static procedure ViewportPositionWScaleNV(index: UInt32; xcoeff: single; ycoeff: single);
    external 'opengl32.dll' name 'glViewportPositionWScaleNV';
    
    static procedure CreateStatesNV(n: Int32; states: ^UInt32);
    external 'opengl32.dll' name 'glCreateStatesNV';
    
    static procedure DeleteStatesNV(n: Int32; states: ^UInt32);
    external 'opengl32.dll' name 'glDeleteStatesNV';
    
    static function IsStateNV(state: UInt32): boolean;
    external 'opengl32.dll' name 'glIsStateNV';
    
    static procedure StateCaptureNV(state: UInt32; mode: UInt32);
    external 'opengl32.dll' name 'glStateCaptureNV';
    
    static function GetCommandHeaderNV(tokenID: UInt32; size: UInt32): UInt32;
    external 'opengl32.dll' name 'glGetCommandHeaderNV';
    
    static function GetStageIndexNV(shadertype: UInt32): UInt16;
    external 'opengl32.dll' name 'glGetStageIndexNV';
    
    static procedure DrawCommandsNV(primitiveMode: UInt32; buffer: UInt32; indirects: ^IntPtr; sizes: ^Int32; count: UInt32);
    external 'opengl32.dll' name 'glDrawCommandsNV';
    
    static procedure DrawCommandsAddressNV(primitiveMode: UInt32; indirects: ^UInt64; sizes: ^Int32; count: UInt32);
    external 'opengl32.dll' name 'glDrawCommandsAddressNV';
    
    static procedure DrawCommandsStatesNV(buffer: UInt32; indirects: ^IntPtr; sizes: ^Int32; states: ^UInt32; fbos: ^UInt32; count: UInt32);
    external 'opengl32.dll' name 'glDrawCommandsStatesNV';
    
    static procedure DrawCommandsStatesAddressNV(indirects: ^UInt64; sizes: ^Int32; states: ^UInt32; fbos: ^UInt32; count: UInt32);
    external 'opengl32.dll' name 'glDrawCommandsStatesAddressNV';
    
    static procedure CreateCommandListsNV(n: Int32; lists: ^UInt32);
    external 'opengl32.dll' name 'glCreateCommandListsNV';
    
    static procedure DeleteCommandListsNV(n: Int32; lists: ^UInt32);
    external 'opengl32.dll' name 'glDeleteCommandListsNV';
    
    static function IsCommandListNV(list: UInt32): boolean;
    external 'opengl32.dll' name 'glIsCommandListNV';
    
    static procedure ListDrawCommandsStatesClientNV(list: UInt32; segment: UInt32; indirects: ^IntPtr; sizes: ^Int32; states: ^UInt32; fbos: ^UInt32; count: UInt32);
    external 'opengl32.dll' name 'glListDrawCommandsStatesClientNV';
    
    static procedure CommandListSegmentsNV(list: UInt32; segments: UInt32);
    external 'opengl32.dll' name 'glCommandListSegmentsNV';
    
    static procedure CompileCommandListNV(list: UInt32);
    external 'opengl32.dll' name 'glCompileCommandListNV';
    
    static procedure CallCommandListNV(list: UInt32);
    external 'opengl32.dll' name 'glCallCommandListNV';
    
    static procedure BeginConditionalRenderNV(id: UInt32; mode: UInt32);
    external 'opengl32.dll' name 'glBeginConditionalRenderNV';
    
    static procedure EndConditionalRenderNV;
    external 'opengl32.dll' name 'glEndConditionalRenderNV';
    
    static procedure SubpixelPrecisionBiasNV(xbits: UInt32; ybits: UInt32);
    external 'opengl32.dll' name 'glSubpixelPrecisionBiasNV';
    
    static procedure ConservativeRasterParameterfNV(pname: UInt32; value: single);
    external 'opengl32.dll' name 'glConservativeRasterParameterfNV';
    
    static procedure ConservativeRasterParameteriNV(pname: UInt32; param: Int32);
    external 'opengl32.dll' name 'glConservativeRasterParameteriNV';
    
    static procedure DrawVkImageNV(vkImage: UInt64; sampler: UInt32; x0: single; y0: single; x1: single; y1: single; z: single; s0: single; t0: single; s1: single; t1: single);
    external 'opengl32.dll' name 'glDrawVkImageNV';
    
    static function GetVkProcAddrNV(name: ^SByte): GLVULKANPROCNV;
    external 'opengl32.dll' name 'glGetVkProcAddrNV';
    
    static procedure WaitVkSemaphoreNV(vkSemaphore: UInt64);
    external 'opengl32.dll' name 'glWaitVkSemaphoreNV';
    
    static procedure SignalVkSemaphoreNV(vkSemaphore: UInt64);
    external 'opengl32.dll' name 'glSignalVkSemaphoreNV';
    
    static procedure SignalVkFenceNV(vkFence: UInt64);
    external 'opengl32.dll' name 'glSignalVkFenceNV';
    
    static procedure FragmentCoverageColorNV(color: UInt32);
    external 'opengl32.dll' name 'glFragmentCoverageColorNV';
    
    static procedure CoverageModulationTableNV(n: Int32; v: ^single);
    external 'opengl32.dll' name 'glCoverageModulationTableNV';
    
    static procedure GetCoverageModulationTableNV(bufsize: Int32; v: ^single);
    external 'opengl32.dll' name 'glGetCoverageModulationTableNV';
    
    static procedure CoverageModulationNV(components: UInt32);
    external 'opengl32.dll' name 'glCoverageModulationNV';
    
    static procedure RenderbufferStorageMultisampleCoverageNV(target: UInt32; coverageSamples: Int32; colorSamples: Int32; internalformat: UInt32; width: Int32; height: Int32);
    external 'opengl32.dll' name 'glRenderbufferStorageMultisampleCoverageNV';
    
    static procedure Uniform1i64NV(location: Int32; x: Int64);
    external 'opengl32.dll' name 'glUniform1i64NV';
    
    static procedure Uniform2i64NV(location: Int32; x: Int64; y: Int64);
    external 'opengl32.dll' name 'glUniform2i64NV';
    
    static procedure Uniform3i64NV(location: Int32; x: Int64; y: Int64; z: Int64);
    external 'opengl32.dll' name 'glUniform3i64NV';
    
    static procedure Uniform4i64NV(location: Int32; x: Int64; y: Int64; z: Int64; w: Int64);
    external 'opengl32.dll' name 'glUniform4i64NV';
    
    static procedure Uniform1i64vNV(location: Int32; count: Int32; value: ^Int64);
    external 'opengl32.dll' name 'glUniform1i64vNV';
    
    static procedure Uniform2i64vNV(location: Int32; count: Int32; value: ^Int64);
    external 'opengl32.dll' name 'glUniform2i64vNV';
    
    static procedure Uniform3i64vNV(location: Int32; count: Int32; value: ^Int64);
    external 'opengl32.dll' name 'glUniform3i64vNV';
    
    static procedure Uniform4i64vNV(location: Int32; count: Int32; value: ^Int64);
    external 'opengl32.dll' name 'glUniform4i64vNV';
    
    static procedure Uniform1ui64NV(location: Int32; x: UInt64);
    external 'opengl32.dll' name 'glUniform1ui64NV';
    
    static procedure Uniform2ui64NV(location: Int32; x: UInt64; y: UInt64);
    external 'opengl32.dll' name 'glUniform2ui64NV';
    
    static procedure Uniform3ui64NV(location: Int32; x: UInt64; y: UInt64; z: UInt64);
    external 'opengl32.dll' name 'glUniform3ui64NV';
    
    static procedure Uniform4ui64NV(location: Int32; x: UInt64; y: UInt64; z: UInt64; w: UInt64);
    external 'opengl32.dll' name 'glUniform4ui64NV';
    
    static procedure Uniform1ui64vNV(location: Int32; count: Int32; value: ^UInt64);
    external 'opengl32.dll' name 'glUniform1ui64vNV';
    
    static procedure Uniform2ui64vNV(location: Int32; count: Int32; value: ^UInt64);
    external 'opengl32.dll' name 'glUniform2ui64vNV';
    
    static procedure Uniform3ui64vNV(location: Int32; count: Int32; value: ^UInt64);
    external 'opengl32.dll' name 'glUniform3ui64vNV';
    
    static procedure Uniform4ui64vNV(location: Int32; count: Int32; value: ^UInt64);
    external 'opengl32.dll' name 'glUniform4ui64vNV';
    
    static procedure GetUniformi64vNV(&program: UInt32; location: Int32; &params: ^Int64);
    external 'opengl32.dll' name 'glGetUniformi64vNV';
    
    static procedure ProgramUniform1i64NV(&program: UInt32; location: Int32; x: Int64);
    external 'opengl32.dll' name 'glProgramUniform1i64NV';
    
    static procedure ProgramUniform2i64NV(&program: UInt32; location: Int32; x: Int64; y: Int64);
    external 'opengl32.dll' name 'glProgramUniform2i64NV';
    
    static procedure ProgramUniform3i64NV(&program: UInt32; location: Int32; x: Int64; y: Int64; z: Int64);
    external 'opengl32.dll' name 'glProgramUniform3i64NV';
    
    static procedure ProgramUniform4i64NV(&program: UInt32; location: Int32; x: Int64; y: Int64; z: Int64; w: Int64);
    external 'opengl32.dll' name 'glProgramUniform4i64NV';
    
    static procedure ProgramUniform1i64vNV(&program: UInt32; location: Int32; count: Int32; value: ^Int64);
    external 'opengl32.dll' name 'glProgramUniform1i64vNV';
    
    static procedure ProgramUniform2i64vNV(&program: UInt32; location: Int32; count: Int32; value: ^Int64);
    external 'opengl32.dll' name 'glProgramUniform2i64vNV';
    
    static procedure ProgramUniform3i64vNV(&program: UInt32; location: Int32; count: Int32; value: ^Int64);
    external 'opengl32.dll' name 'glProgramUniform3i64vNV';
    
    static procedure ProgramUniform4i64vNV(&program: UInt32; location: Int32; count: Int32; value: ^Int64);
    external 'opengl32.dll' name 'glProgramUniform4i64vNV';
    
    static procedure ProgramUniform1ui64NV(&program: UInt32; location: Int32; x: UInt64);
    external 'opengl32.dll' name 'glProgramUniform1ui64NV';
    
    static procedure ProgramUniform2ui64NV(&program: UInt32; location: Int32; x: UInt64; y: UInt64);
    external 'opengl32.dll' name 'glProgramUniform2ui64NV';
    
    static procedure ProgramUniform3ui64NV(&program: UInt32; location: Int32; x: UInt64; y: UInt64; z: UInt64);
    external 'opengl32.dll' name 'glProgramUniform3ui64NV';
    
    static procedure ProgramUniform4ui64NV(&program: UInt32; location: Int32; x: UInt64; y: UInt64; z: UInt64; w: UInt64);
    external 'opengl32.dll' name 'glProgramUniform4ui64NV';
    
    static procedure ProgramUniform1ui64vNV(&program: UInt32; location: Int32; count: Int32; value: ^UInt64);
    external 'opengl32.dll' name 'glProgramUniform1ui64vNV';
    
    static procedure ProgramUniform2ui64vNV(&program: UInt32; location: Int32; count: Int32; value: ^UInt64);
    external 'opengl32.dll' name 'glProgramUniform2ui64vNV';
    
    static procedure ProgramUniform3ui64vNV(&program: UInt32; location: Int32; count: Int32; value: ^UInt64);
    external 'opengl32.dll' name 'glProgramUniform3ui64vNV';
    
    static procedure ProgramUniform4ui64vNV(&program: UInt32; location: Int32; count: Int32; value: ^UInt64);
    external 'opengl32.dll' name 'glProgramUniform4ui64vNV';
    
    static procedure GetInternalformatSampleivNV(target: UInt32; internalformat: UInt32; samples: Int32; pname: UInt32; bufSize: Int32; &params: ^Int32);
    external 'opengl32.dll' name 'glGetInternalformatSampleivNV';
    
    static procedure GetMemoryObjectDetachedResourcesuivNV(memory: UInt32; pname: UInt32; first: Int32; count: Int32; &params: ^UInt32);
    external 'opengl32.dll' name 'glGetMemoryObjectDetachedResourcesuivNV';
    
    static procedure ResetMemoryObjectParameterNV(memory: UInt32; pname: UInt32);
    external 'opengl32.dll' name 'glResetMemoryObjectParameterNV';
    
    static procedure TexAttachMemoryNV(target: UInt32; memory: UInt32; offset: UInt64);
    external 'opengl32.dll' name 'glTexAttachMemoryNV';
    
    static procedure BufferAttachMemoryNV(target: UInt32; memory: UInt32; offset: UInt64);
    external 'opengl32.dll' name 'glBufferAttachMemoryNV';
    
    static procedure TextureAttachMemoryNV(texture: UInt32; memory: UInt32; offset: UInt64);
    external 'opengl32.dll' name 'glTextureAttachMemoryNV';
    
    static procedure NamedBufferAttachMemoryNV(buffer: UInt32; memory: UInt32; offset: UInt64);
    external 'opengl32.dll' name 'glNamedBufferAttachMemoryNV';
    
    static procedure DrawMeshTasksNV(first: UInt32; count: UInt32);
    external 'opengl32.dll' name 'glDrawMeshTasksNV';
    
    static procedure DrawMeshTasksIndirectNV(indirect: IntPtr);
    external 'opengl32.dll' name 'glDrawMeshTasksIndirectNV';
    
    static procedure MultiDrawMeshTasksIndirectNV(indirect: IntPtr; drawcount: Int32; stride: Int32);
    external 'opengl32.dll' name 'glMultiDrawMeshTasksIndirectNV';
    
    static procedure MultiDrawMeshTasksIndirectCountNV(indirect: IntPtr; drawcount: IntPtr; maxdrawcount: Int32; stride: Int32);
    external 'opengl32.dll' name 'glMultiDrawMeshTasksIndirectCountNV';
    
    static function GenPathsNV(range: Int32): UInt32;
    external 'opengl32.dll' name 'glGenPathsNV';
    
    static procedure DeletePathsNV(path: UInt32; range: Int32);
    external 'opengl32.dll' name 'glDeletePathsNV';
    
    static function IsPathNV(path: UInt32): boolean;
    external 'opengl32.dll' name 'glIsPathNV';
    
    static procedure PathCommandsNV(path: UInt32; numCommands: Int32; commands: ^Byte; numCoords: Int32; coordType: UInt32; coords: pointer);
    external 'opengl32.dll' name 'glPathCommandsNV';
    
    static procedure PathCoordsNV(path: UInt32; numCoords: Int32; coordType: UInt32; coords: pointer);
    external 'opengl32.dll' name 'glPathCoordsNV';
    
    static procedure PathSubCommandsNV(path: UInt32; commandStart: Int32; commandsToDelete: Int32; numCommands: Int32; commands: ^Byte; numCoords: Int32; coordType: UInt32; coords: pointer);
    external 'opengl32.dll' name 'glPathSubCommandsNV';
    
    static procedure PathSubCoordsNV(path: UInt32; coordStart: Int32; numCoords: Int32; coordType: UInt32; coords: pointer);
    external 'opengl32.dll' name 'glPathSubCoordsNV';
    
    static procedure PathStringNV(path: UInt32; format: UInt32; length: Int32; pathString: pointer);
    external 'opengl32.dll' name 'glPathStringNV';
    
    static procedure PathGlyphsNV(firstPathName: UInt32; fontTarget: UInt32; fontName: pointer; fontStyle: UInt32; numGlyphs: Int32; &type: UInt32; charcodes: pointer; handleMissingGlyphs: UInt32; pathParameterTemplate: UInt32; emScale: single);
    external 'opengl32.dll' name 'glPathGlyphsNV';
    
    static procedure PathGlyphRangeNV(firstPathName: UInt32; fontTarget: UInt32; fontName: pointer; fontStyle: UInt32; firstGlyph: UInt32; numGlyphs: Int32; handleMissingGlyphs: UInt32; pathParameterTemplate: UInt32; emScale: single);
    external 'opengl32.dll' name 'glPathGlyphRangeNV';
    
    static procedure WeightPathsNV(resultPath: UInt32; numPaths: Int32; paths: ^UInt32; weights: ^single);
    external 'opengl32.dll' name 'glWeightPathsNV';
    
    static procedure CopyPathNV(resultPath: UInt32; srcPath: UInt32);
    external 'opengl32.dll' name 'glCopyPathNV';
    
    static procedure InterpolatePathsNV(resultPath: UInt32; pathA: UInt32; pathB: UInt32; weight: single);
    external 'opengl32.dll' name 'glInterpolatePathsNV';
    
    static procedure TransformPathNV(resultPath: UInt32; srcPath: UInt32; transformType: UInt32; transformValues: ^single);
    external 'opengl32.dll' name 'glTransformPathNV';
    
    static procedure PathParameterivNV(path: UInt32; pname: UInt32; value: ^Int32);
    external 'opengl32.dll' name 'glPathParameterivNV';
    
    static procedure PathParameteriNV(path: UInt32; pname: UInt32; value: Int32);
    external 'opengl32.dll' name 'glPathParameteriNV';
    
    static procedure PathParameterfvNV(path: UInt32; pname: UInt32; value: ^single);
    external 'opengl32.dll' name 'glPathParameterfvNV';
    
    static procedure PathParameterfNV(path: UInt32; pname: UInt32; value: single);
    external 'opengl32.dll' name 'glPathParameterfNV';
    
    static procedure PathDashArrayNV(path: UInt32; dashCount: Int32; dashArray: ^single);
    external 'opengl32.dll' name 'glPathDashArrayNV';
    
    static procedure PathStencilFuncNV(func: UInt32; ref: Int32; mask: UInt32);
    external 'opengl32.dll' name 'glPathStencilFuncNV';
    
    static procedure PathStencilDepthOffsetNV(factor: single; units: single);
    external 'opengl32.dll' name 'glPathStencilDepthOffsetNV';
    
    static procedure StencilFillPathNV(path: UInt32; fillMode: UInt32; mask: UInt32);
    external 'opengl32.dll' name 'glStencilFillPathNV';
    
    static procedure StencilStrokePathNV(path: UInt32; reference: Int32; mask: UInt32);
    external 'opengl32.dll' name 'glStencilStrokePathNV';
    
    static procedure StencilFillPathInstancedNV(numPaths: Int32; pathNameType: UInt32; paths: pointer; pathBase: UInt32; fillMode: UInt32; mask: UInt32; transformType: UInt32; transformValues: ^single);
    external 'opengl32.dll' name 'glStencilFillPathInstancedNV';
    
    static procedure StencilStrokePathInstancedNV(numPaths: Int32; pathNameType: UInt32; paths: pointer; pathBase: UInt32; reference: Int32; mask: UInt32; transformType: UInt32; transformValues: ^single);
    external 'opengl32.dll' name 'glStencilStrokePathInstancedNV';
    
    static procedure PathCoverDepthFuncNV(func: UInt32);
    external 'opengl32.dll' name 'glPathCoverDepthFuncNV';
    
    static procedure CoverFillPathNV(path: UInt32; coverMode: UInt32);
    external 'opengl32.dll' name 'glCoverFillPathNV';
    
    static procedure CoverStrokePathNV(path: UInt32; coverMode: UInt32);
    external 'opengl32.dll' name 'glCoverStrokePathNV';
    
    static procedure CoverFillPathInstancedNV(numPaths: Int32; pathNameType: UInt32; paths: pointer; pathBase: UInt32; coverMode: UInt32; transformType: UInt32; transformValues: ^single);
    external 'opengl32.dll' name 'glCoverFillPathInstancedNV';
    
    static procedure CoverStrokePathInstancedNV(numPaths: Int32; pathNameType: UInt32; paths: pointer; pathBase: UInt32; coverMode: UInt32; transformType: UInt32; transformValues: ^single);
    external 'opengl32.dll' name 'glCoverStrokePathInstancedNV';
    
    static procedure GetPathParameterivNV(path: UInt32; pname: UInt32; value: ^Int32);
    external 'opengl32.dll' name 'glGetPathParameterivNV';
    
    static procedure GetPathParameterfvNV(path: UInt32; pname: UInt32; value: ^single);
    external 'opengl32.dll' name 'glGetPathParameterfvNV';
    
    static procedure GetPathCommandsNV(path: UInt32; commands: ^Byte);
    external 'opengl32.dll' name 'glGetPathCommandsNV';
    
    static procedure GetPathCoordsNV(path: UInt32; coords: ^single);
    external 'opengl32.dll' name 'glGetPathCoordsNV';
    
    static procedure GetPathDashArrayNV(path: UInt32; dashArray: ^single);
    external 'opengl32.dll' name 'glGetPathDashArrayNV';
    
    static procedure GetPathMetricsNV(metricQueryMask: UInt32; numPaths: Int32; pathNameType: UInt32; paths: pointer; pathBase: UInt32; stride: Int32; metrics: ^single);
    external 'opengl32.dll' name 'glGetPathMetricsNV';
    
    static procedure GetPathMetricRangeNV(metricQueryMask: UInt32; firstPathName: UInt32; numPaths: Int32; stride: Int32; metrics: ^single);
    external 'opengl32.dll' name 'glGetPathMetricRangeNV';
    
    static procedure GetPathSpacingNV(pathListMode: UInt32; numPaths: Int32; pathNameType: UInt32; paths: pointer; pathBase: UInt32; advanceScale: single; kerningScale: single; transformType: UInt32; returnedSpacing: ^single);
    external 'opengl32.dll' name 'glGetPathSpacingNV';
    
    static function IsPointInFillPathNV(path: UInt32; mask: UInt32; x: single; y: single): boolean;
    external 'opengl32.dll' name 'glIsPointInFillPathNV';
    
    static function IsPointInStrokePathNV(path: UInt32; x: single; y: single): boolean;
    external 'opengl32.dll' name 'glIsPointInStrokePathNV';
    
    static function GetPathLengthNV(path: UInt32; startSegment: Int32; numSegments: Int32): single;
    external 'opengl32.dll' name 'glGetPathLengthNV';
    
    static function PointAlongPathNV(path: UInt32; startSegment: Int32; numSegments: Int32; distance: single; x: ^single; y: ^single; tangentX: ^single; tangentY: ^single): boolean;
    external 'opengl32.dll' name 'glPointAlongPathNV';
    
    static procedure MatrixLoad3x2fNV(matrixMode: UInt32; m: ^single);
    external 'opengl32.dll' name 'glMatrixLoad3x2fNV';
    
    static procedure MatrixLoad3x3fNV(matrixMode: UInt32; m: ^single);
    external 'opengl32.dll' name 'glMatrixLoad3x3fNV';
    
    static procedure MatrixLoadTranspose3x3fNV(matrixMode: UInt32; m: ^single);
    external 'opengl32.dll' name 'glMatrixLoadTranspose3x3fNV';
    
    static procedure MatrixMult3x2fNV(matrixMode: UInt32; m: ^single);
    external 'opengl32.dll' name 'glMatrixMult3x2fNV';
    
    static procedure MatrixMult3x3fNV(matrixMode: UInt32; m: ^single);
    external 'opengl32.dll' name 'glMatrixMult3x3fNV';
    
    static procedure MatrixMultTranspose3x3fNV(matrixMode: UInt32; m: ^single);
    external 'opengl32.dll' name 'glMatrixMultTranspose3x3fNV';
    
    static procedure StencilThenCoverFillPathNV(path: UInt32; fillMode: UInt32; mask: UInt32; coverMode: UInt32);
    external 'opengl32.dll' name 'glStencilThenCoverFillPathNV';
    
    static procedure StencilThenCoverStrokePathNV(path: UInt32; reference: Int32; mask: UInt32; coverMode: UInt32);
    external 'opengl32.dll' name 'glStencilThenCoverStrokePathNV';
    
    static procedure StencilThenCoverFillPathInstancedNV(numPaths: Int32; pathNameType: UInt32; paths: pointer; pathBase: UInt32; fillMode: UInt32; mask: UInt32; coverMode: UInt32; transformType: UInt32; transformValues: ^single);
    external 'opengl32.dll' name 'glStencilThenCoverFillPathInstancedNV';
    
    static procedure StencilThenCoverStrokePathInstancedNV(numPaths: Int32; pathNameType: UInt32; paths: pointer; pathBase: UInt32; reference: Int32; mask: UInt32; coverMode: UInt32; transformType: UInt32; transformValues: ^single);
    external 'opengl32.dll' name 'glStencilThenCoverStrokePathInstancedNV';
    
    static function PathGlyphIndexRangeNV(fontTarget: UInt32; fontName: pointer; fontStyle: UInt32; pathParameterTemplate: UInt32; emScale: single; baseAndCount: ^Vec2ui): UInt32;
    external 'opengl32.dll' name 'glPathGlyphIndexRangeNV';
    
    static function PathGlyphIndexArrayNV(firstPathName: UInt32; fontTarget: UInt32; fontName: pointer; fontStyle: UInt32; firstGlyphIndex: UInt32; numGlyphs: Int32; pathParameterTemplate: UInt32; emScale: single): UInt32;
    external 'opengl32.dll' name 'glPathGlyphIndexArrayNV';
    
    static function PathMemoryGlyphIndexArrayNV(firstPathName: UInt32; fontTarget: UInt32; fontSize: UIntPtr; fontData: pointer; faceIndex: Int32; firstGlyphIndex: UInt32; numGlyphs: Int32; pathParameterTemplate: UInt32; emScale: single): UInt32;
    external 'opengl32.dll' name 'glPathMemoryGlyphIndexArrayNV';
    
    static procedure ProgramPathFragmentInputGenNV(&program: UInt32; location: Int32; genMode: UInt32; components: Int32; coeffs: ^single);
    external 'opengl32.dll' name 'glProgramPathFragmentInputGenNV';
    
    static procedure GetProgramResourcefvNV(&program: UInt32; programInterface: UInt32; index: UInt32; propCount: Int32; props: ^UInt32; bufSize: Int32; length: ^Int32; &params: ^single);
    external 'opengl32.dll' name 'glGetProgramResourcefvNV';
    
    static procedure FramebufferSampleLocationsfvNV(target: UInt32; start: UInt32; count: Int32; v: ^single);
    external 'opengl32.dll' name 'glFramebufferSampleLocationsfvNV';
    
    static procedure NamedFramebufferSampleLocationsfvNV(framebuffer: UInt32; start: UInt32; count: Int32; v: ^single);
    external 'opengl32.dll' name 'glNamedFramebufferSampleLocationsfvNV';
    
    static procedure ResolveDepthValuesNV;
    external 'opengl32.dll' name 'glResolveDepthValuesNV';
    
    static procedure ScissorExclusiveNV(x: Int32; y: Int32; width: Int32; height: Int32);
    external 'opengl32.dll' name 'glScissorExclusiveNV';
    
    static procedure ScissorExclusiveArrayvNV(first: UInt32; count: Int32; v: ^Int32);
    external 'opengl32.dll' name 'glScissorExclusiveArrayvNV';
    
    static procedure MakeBufferResidentNV(target: UInt32; access: UInt32);
    external 'opengl32.dll' name 'glMakeBufferResidentNV';
    
    static procedure MakeBufferNonResidentNV(target: UInt32);
    external 'opengl32.dll' name 'glMakeBufferNonResidentNV';
    
    static function IsBufferResidentNV(target: UInt32): boolean;
    external 'opengl32.dll' name 'glIsBufferResidentNV';
    
    static procedure MakeNamedBufferResidentNV(buffer: UInt32; access: UInt32);
    external 'opengl32.dll' name 'glMakeNamedBufferResidentNV';
    
    static procedure MakeNamedBufferNonResidentNV(buffer: UInt32);
    external 'opengl32.dll' name 'glMakeNamedBufferNonResidentNV';
    
    static function IsNamedBufferResidentNV(buffer: UInt32): boolean;
    external 'opengl32.dll' name 'glIsNamedBufferResidentNV';
    
    static procedure GetBufferParameterui64vNV(target: UInt32; pname: UInt32; &params: ^UInt64);
    external 'opengl32.dll' name 'glGetBufferParameterui64vNV';
    
    static procedure GetNamedBufferParameterui64vNV(buffer: UInt32; pname: UInt32; &params: ^UInt64);
    external 'opengl32.dll' name 'glGetNamedBufferParameterui64vNV';
    
    static procedure GetIntegerui64vNV(value: UInt32; result: ^UInt64);
    external 'opengl32.dll' name 'glGetIntegerui64vNV';
    
    static procedure Uniformui64NV(location: Int32; value: UInt64);
    external 'opengl32.dll' name 'glUniformui64NV';
    
    static procedure Uniformui64vNV(location: Int32; count: Int32; value: ^UInt64);
    external 'opengl32.dll' name 'glUniformui64vNV';
    
    static procedure GetUniformui64vNV(&program: UInt32; location: Int32; &params: ^UInt64);
    external 'opengl32.dll' name 'glGetUniformui64vNV';
    
    static procedure ProgramUniformui64NV(&program: UInt32; location: Int32; value: UInt64);
    external 'opengl32.dll' name 'glProgramUniformui64NV';
    
    static procedure ProgramUniformui64vNV(&program: UInt32; location: Int32; count: Int32; value: ^UInt64);
    external 'opengl32.dll' name 'glProgramUniformui64vNV';
    
    static procedure BindShadingRateImageNV(texture: UInt32);
    external 'opengl32.dll' name 'glBindShadingRateImageNV';
    
    static procedure GetShadingRateImagePaletteNV(viewport: UInt32; entry: UInt32; rate: ^UInt32);
    external 'opengl32.dll' name 'glGetShadingRateImagePaletteNV';
    
    static procedure GetShadingRateSampleLocationivNV(rate: UInt32; samples: UInt32; index: UInt32; location: ^Int32);
    external 'opengl32.dll' name 'glGetShadingRateSampleLocationivNV';
    
    static procedure ShadingRateImageBarrierNV(synchronize: boolean);
    external 'opengl32.dll' name 'glShadingRateImageBarrierNV';
    
    static procedure ShadingRateImagePaletteNV(viewport: UInt32; first: UInt32; count: Int32; rates: ^UInt32);
    external 'opengl32.dll' name 'glShadingRateImagePaletteNV';
    
    static procedure ShadingRateSampleOrderNV(order: UInt32);
    external 'opengl32.dll' name 'glShadingRateSampleOrderNV';
    
    static procedure ShadingRateSampleOrderCustomNV(rate: UInt32; samples: UInt32; locations: ^Int32);
    external 'opengl32.dll' name 'glShadingRateSampleOrderCustomNV';
    
    static procedure TextureBarrierNV;
    external 'opengl32.dll' name 'glTextureBarrierNV';
    
    static procedure GetVertexAttribLi64vNV(index: UInt32; pname: UInt32; &params: ^Int64);
    external 'opengl32.dll' name 'glGetVertexAttribLi64vNV';
    
    static procedure GetVertexAttribLui64vNV(index: UInt32; pname: UInt32; &params: ^UInt64);
    external 'opengl32.dll' name 'glGetVertexAttribLui64vNV';
    
    static procedure BufferAddressRangeNV(pname: UInt32; index: UInt32; address: UInt64; length: UIntPtr);
    external 'opengl32.dll' name 'glBufferAddressRangeNV';
    
    static procedure VertexFormatNV(size: Int32; &type: UInt32; stride: Int32);
    external 'opengl32.dll' name 'glVertexFormatNV';
    
    static procedure NormalFormatNV(&type: UInt32; stride: Int32);
    external 'opengl32.dll' name 'glNormalFormatNV';
    
    static procedure ColorFormatNV(size: Int32; &type: UInt32; stride: Int32);
    external 'opengl32.dll' name 'glColorFormatNV';
    
    static procedure IndexFormatNV(&type: UInt32; stride: Int32);
    external 'opengl32.dll' name 'glIndexFormatNV';
    
    static procedure TexCoordFormatNV(size: Int32; &type: UInt32; stride: Int32);
    external 'opengl32.dll' name 'glTexCoordFormatNV';
    
    static procedure EdgeFlagFormatNV(stride: Int32);
    external 'opengl32.dll' name 'glEdgeFlagFormatNV';
    
    static procedure SecondaryColorFormatNV(size: Int32; &type: UInt32; stride: Int32);
    external 'opengl32.dll' name 'glSecondaryColorFormatNV';
    
    static procedure FogCoordFormatNV(&type: UInt32; stride: Int32);
    external 'opengl32.dll' name 'glFogCoordFormatNV';
    
    static procedure GetIntegerui64i_vNV(value: UInt32; index: UInt32; result: ^UInt64);
    external 'opengl32.dll' name 'glGetIntegerui64i_vNV';
    
    static procedure ViewportSwizzleNV(index: UInt32; swizzlex: UInt32; swizzley: UInt32; swizzlez: UInt32; swizzlew: UInt32);
    external 'opengl32.dll' name 'glViewportSwizzleNV';
    
    static procedure FramebufferTextureMultiviewOVR(target: UInt32; attachment: UInt32; texture: UInt32; level: Int32; baseViewIndex: Int32; numViews: Int32);
    external 'opengl32.dll' name 'glFramebufferTextureMultiviewOVR';
    
    static procedure AlphaFunc(func: UInt32; ref: single);
    external 'opengl32.dll' name 'glAlphaFunc';
    
    static procedure &Begin(mode: UInt32);
    external 'opengl32.dll' name 'glBegin';
    
    static procedure Bitmap(width: Int32; height: Int32; xorig: single; yorig: single; xmove: single; ymove: single; bitmap: ^Byte);
    external 'opengl32.dll' name 'glBitmap';
    
    static procedure CallLists(n: Int32; &type: UInt32; lists: pointer);
    external 'opengl32.dll' name 'glCallLists';
    
    static procedure ClientActiveTexture(texture: UInt32);
    external 'opengl32.dll' name 'glClientActiveTexture';
    
    static procedure Color4f(red: single; green: single; blue: single; alpha: single);
    external 'opengl32.dll' name 'glColor4f';
    
    static procedure Color4fv(v: ^single);
    external 'opengl32.dll' name 'glColor4fv';
    
    static procedure Color4ub(red: Byte; green: Byte; blue: Byte; alpha: Byte);
    external 'opengl32.dll' name 'glColor4ub';
    
    static procedure ColorPointer(size: Int32; &type: UInt32; stride: Int32; _pointer: pointer);
    external 'opengl32.dll' name 'glColorPointer';
    
    static procedure ColorSubTableEXT(target: UInt32; start: Int32; count: Int32; format: UInt32; &type: UInt32; table: pointer);
    external 'opengl32.dll' name 'glColorSubTableEXT';
    
    static procedure ColorTableEXT(target: UInt32; internalformat: UInt32; width: Int32; format: UInt32; &type: UInt32; table: pointer);
    external 'opengl32.dll' name 'glColorTableEXT';
    
    static procedure DisableClientState(&array: UInt32);
    external 'opengl32.dll' name 'glDisableClientState';
    
    static procedure DrawPixels(width: Int32; height: Int32; format: UInt32; &type: UInt32; pixels: pointer);
    external 'opengl32.dll' name 'glDrawPixels';
    
    static procedure EnableClientState(&array: UInt32);
    external 'opengl32.dll' name 'glEnableClientState';
    
    static procedure &End;
    external 'opengl32.dll' name 'glEnd';
    
    static procedure EndList;
    external 'opengl32.dll' name 'glEndList';
    
    static procedure Frustumf(left: single; right: single; bottom: single; top: single; zNear: single; zFar: single);
    external 'opengl32.dll' name 'glFrustumf';
    
    static function GenLists(range: Int32): UInt32;
    external 'opengl32.dll' name 'glGenLists';
    
    static procedure GetColorTableEXT(target: UInt32; format: UInt32; &type: UInt32; table: pointer);
    external 'opengl32.dll' name 'glGetColorTableEXT';
    
    static procedure GetColorTableParameterivEXT(target: UInt32; pname: UInt32; &params: ^Int32);
    external 'opengl32.dll' name 'glGetColorTableParameterivEXT';
    
    static procedure GetLightfv(light: UInt32; pname: UInt32; &params: ^single);
    external 'opengl32.dll' name 'glGetLightfv';
    
    static procedure GetMaterialfv(face: UInt32; pname: UInt32; &params: ^single);
    external 'opengl32.dll' name 'glGetMaterialfv';
    
    static procedure GetPolygonStipple(mask: ^Byte);
    external 'opengl32.dll' name 'glGetPolygonStipple';
    
    static procedure GetTexEnvfv(target: UInt32; pname: UInt32; &params: ^single);
    external 'opengl32.dll' name 'glGetTexEnvfv';
    
    static procedure GetTexEnviv(target: UInt32; pname: UInt32; &params: ^Int32);
    external 'opengl32.dll' name 'glGetTexEnviv';
    
    static procedure Lightfv(light: UInt32; pname: UInt32; &params: ^single);
    external 'opengl32.dll' name 'glLightfv';
    
    static procedure LightModelfv(pname: UInt32; &params: ^single);
    external 'opengl32.dll' name 'glLightModelfv';
    
    static procedure LineStipple(factor: Int32; pattern: UInt16);
    external 'opengl32.dll' name 'glLineStipple';
    
    static procedure ListBase(base: UInt32);
    external 'opengl32.dll' name 'glListBase';
    
    static procedure LoadIdentity;
    external 'opengl32.dll' name 'glLoadIdentity';
    
    static procedure LoadMatrixf(m: ^single);
    external 'opengl32.dll' name 'glLoadMatrixf';
    
    static procedure Materialf(face: UInt32; pname: UInt32; param: single);
    external 'opengl32.dll' name 'glMaterialf';
    
    static procedure Materialfv(face: UInt32; pname: UInt32; &params: ^single);
    external 'opengl32.dll' name 'glMaterialfv';
    
    static procedure MatrixMode(mode: UInt32);
    external 'opengl32.dll' name 'glMatrixMode';
    
    static procedure MultMatrixf(m: ^single);
    external 'opengl32.dll' name 'glMultMatrixf';
    
    static procedure MultiTexCoord2f(target: UInt32; s: single; t: single);
    external 'opengl32.dll' name 'glMultiTexCoord2f';
    
    static procedure MultiTexCoord2fv(target: UInt32; v: ^single);
    external 'opengl32.dll' name 'glMultiTexCoord2fv';
    
    static procedure NewList(list: UInt32; mode: UInt32);
    external 'opengl32.dll' name 'glNewList';
    
    static procedure Normal3f(nx: single; ny: single; nz: single);
    external 'opengl32.dll' name 'glNormal3f';
    
    static procedure Normal3fv(v: ^single);
    external 'opengl32.dll' name 'glNormal3fv';
    
    static procedure NormalPointer(&type: UInt32; stride: Int32; _pointer: pointer);
    external 'opengl32.dll' name 'glNormalPointer';
    
    static procedure Orthof(left: single; right: single; bottom: single; top: single; zNear: single; zFar: single);
    external 'opengl32.dll' name 'glOrthof';
    
    static procedure PolygonStipple(mask: ^Byte);
    external 'opengl32.dll' name 'glPolygonStipple';
    
    static procedure PopMatrix;
    external 'opengl32.dll' name 'glPopMatrix';
    
    static procedure PushMatrix;
    external 'opengl32.dll' name 'glPushMatrix';
    
    static procedure RasterPos3f(x: single; y: single; z: single);
    external 'opengl32.dll' name 'glRasterPos3f';
    
    static procedure Rotatef(angle: single; x: single; y: single; z: single);
    external 'opengl32.dll' name 'glRotatef';
    
    static procedure Scalef(x: single; y: single; z: single);
    external 'opengl32.dll' name 'glScalef';
    
    static procedure ShadeModel(mode: UInt32);
    external 'opengl32.dll' name 'glShadeModel';
    
    static procedure TexCoordPointer(size: Int32; &type: UInt32; stride: Int32; _pointer: pointer);
    external 'opengl32.dll' name 'glTexCoordPointer';
    
    static procedure TexEnvfv(target: UInt32; pname: UInt32; &params: ^single);
    external 'opengl32.dll' name 'glTexEnvfv';
    
    static procedure TexEnvi(target: UInt32; pname: UInt32; param: Int32);
    external 'opengl32.dll' name 'glTexEnvi';
    
    static procedure Translatef(x: single; y: single; z: single);
    external 'opengl32.dll' name 'glTranslatef';
    
    static procedure Vertex2f(x: single; y: single);
    external 'opengl32.dll' name 'glVertex2f';
    
    static procedure Vertex2fv(v: ^single);
    external 'opengl32.dll' name 'glVertex2fv';
    
    static procedure Vertex3f(x: single; y: single; z: single);
    external 'opengl32.dll' name 'glVertex3f';
    
    static procedure Vertex3fv(v: ^single);
    external 'opengl32.dll' name 'glVertex3fv';
    
    static procedure VertexPointer(size: Int32; &type: UInt32; stride: Int32; _pointer: pointer);
    external 'opengl32.dll' name 'glVertexPointer';
    
    {$endregion unsorted}
    
    {$region странные расширения}{
    
    static procedure VertexAttribFormatNV(index: UInt32; size: Int32; &type: UInt32; normalized: boolean; stride: Int32);
    external 'opengl32.dll' name 'glVertexAttribFormatNV';
    
    static procedure VertexAttribIFormatNV(index: UInt32; size: Int32; &type: UInt32; stride: Int32);
    external 'opengl32.dll' name 'glVertexAttribIFormatNV';
    
    static procedure VertexAttribLFormatNV(index: UInt32; size: Int32; &type: UInt32; stride: Int32);
    external 'opengl32.dll' name 'glVertexAttribLFormatNV';
    
    static procedure VertexAttribL1i64NV(index: UInt32; x: Int64);
    external 'opengl32.dll' name 'glVertexAttribL1i64NV';
    
    static procedure VertexAttribL2i64NV(index: UInt32; x: Int64; y: Int64);
    external 'opengl32.dll' name 'glVertexAttribL2i64NV';
    
    static procedure VertexAttribL3i64NV(index: UInt32; x: Int64; y: Int64; z: Int64);
    external 'opengl32.dll' name 'glVertexAttribL3i64NV';
    
    static procedure VertexAttribL4i64NV(index: UInt32; x: Int64; y: Int64; z: Int64; w: Int64);
    external 'opengl32.dll' name 'glVertexAttribL4i64NV';
    
    static procedure VertexAttribL1i64vNV(index: UInt32; v: ^Int64);
    external 'opengl32.dll' name 'glVertexAttribL1i64vNV';
    
    static procedure VertexAttribL2i64vNV(index: UInt32; v: ^Int64);
    external 'opengl32.dll' name 'glVertexAttribL2i64vNV';
    
    static procedure VertexAttribL3i64vNV(index: UInt32; v: ^Int64);
    external 'opengl32.dll' name 'glVertexAttribL3i64vNV';
    
    static procedure VertexAttribL4i64vNV(index: UInt32; v: ^Int64);
    external 'opengl32.dll' name 'glVertexAttribL4i64vNV';
    
    static procedure VertexAttribL1ui64NV(index: UInt32; x: UInt64);
    external 'opengl32.dll' name 'glVertexAttribL1ui64NV';
    
    static procedure VertexAttribL2ui64NV(index: UInt32; x: UInt64; y: UInt64);
    external 'opengl32.dll' name 'glVertexAttribL2ui64NV';
    
    static procedure VertexAttribL3ui64NV(index: UInt32; x: UInt64; y: UInt64; z: UInt64);
    external 'opengl32.dll' name 'glVertexAttribL3ui64NV';
    
    static procedure VertexAttribL4ui64NV(index: UInt32; x: UInt64; y: UInt64; z: UInt64; w: UInt64);
    external 'opengl32.dll' name 'glVertexAttribL4ui64NV';
    
    static procedure VertexAttribL1ui64vNV(index: UInt32; v: ^UInt64);
    external 'opengl32.dll' name 'glVertexAttribL1ui64vNV';
    
    static procedure VertexAttribL2ui64vNV(index: UInt32; v: ^UInt64);
    external 'opengl32.dll' name 'glVertexAttribL2ui64vNV';
    
    static procedure VertexAttribL3ui64vNV(index: UInt32; v: ^UInt64);
    external 'opengl32.dll' name 'glVertexAttribL3ui64vNV';
    
    static procedure VertexAttribL4ui64vNV(index: UInt32; v: ^UInt64);
    external 'opengl32.dll' name 'glVertexAttribL4ui64vNV';
    
    static procedure VertexAttribDivisorARB(index: UInt32; divisor: UInt32);
    external 'opengl32.dll' name 'glVertexAttribDivisorARB';
    
    static procedure GetVertexAttribLui64vARB(index: UInt32; pname: UInt32; &params: ^UInt64);
    external 'opengl32.dll' name 'glGetVertexAttribLui64vARB';
    
    static procedure VertexAttribL1ui64ARB(index: UInt32; x: UInt64);
    external 'opengl32.dll' name 'glVertexAttribL1ui64ARB';
    
    static procedure VertexAttribL1ui64vARB(index: UInt32; v: ^UInt64);
    external 'opengl32.dll' name 'glVertexAttribL1ui64vARB';
    
    }{$endregion странные расширения}
    
  end;

end.