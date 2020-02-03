
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
///Код переведён отсюда:
///   https://github.com/KhronosGroup/OpenCL-Headers/tree/master/CL
///
///Спецификации всех версий:
///   https://www.khronos.org/registry/OpenCL/
///
///Если не хватает функции, перечисления, или найдена ошибка - писать сюда:
///   https://github.com/SunSerega/POCGL/issues
///
unit OpenCL;

uses System;
uses System.Runtime.InteropServices;

{$region Debug}

{ $define DebugMode}
{$ifdef DebugMode}

{$endif DebugMode}

{$endregion Debug}

{$region Основные типы}

type
  
  cl_platform_id                = IntPtr;
  cl_device_id                  = IntPtr;
  cl_context                    = IntPtr;
  cl_command_queue              = IntPtr;
  cl_mem                        = IntPtr;
  cl_program                    = IntPtr;
  cl_kernel                     = IntPtr;
  cl_sampler                    = IntPtr;
  {$ifndef DebugMode}
  cl_event                      = IntPtr;
  {$endif !DebugMode}
  
  ///0 = false, остальное = true
  cl_bool                       = UInt32;
  cl_bitfield                   = UInt64;
  
type
  OpenCLException = sealed class(Exception)
    
    public constructor(text: string) :=
    inherited Create($'Ошибка OpenCL: "{text}"');
    
    public constructor(err_code: UInt32);
    
  end;
  
{$endregion Основные типы}

{$region Перечисления} type
  
  {$region case Result of}
  
  //SR
  ErrorCode = record
    public val: Int32;
    public constructor(val: Int32) := self.val := val;
    
    public const SUCCESS =                                 -0;
    
    public const DEVICE_NOT_FOUND =                        -1;
    public const DEVICE_NOT_AVAILABLE =                    -2;
    public const COMPILER_NOT_AVAILABLE =                  -3;
    public const MEM_OBJECT_ALLOCATION_FAILURE =           -4;
    public const OUT_OF_RESOURCES =                        -5;
    public const OUT_OF_HOST_MEMORY =                      -6;
    public const PROFILING_INFO_NOT_AVAILABLE =            -7;
    public const MEM_COPY_OVERLAP =                        -8;
    public const IMAGE_FORMAT_MISMATCH =                   -9;
    public const IMAGE_FORMAT_NOT_SUPPORTED =             -10;
    public const BUILD_PROGRAM_FAILURE =                  -11;
    public const MAP_FAILURE =                            -12;
    
    public const INVALID_VALUE =                          -30;
    public const INVALID_DEVICE_TYPE =                    -31;
    public const INVALID_PLATFORM =                       -32;
    public const INVALID_DEVICE =                         -33;
    public const INVALID_CONTEXT =                        -34;
    public const INVALID_QUEUE_PROPERTIES =               -35;
    public const INVALID_COMMAND_QUEUE =                  -36;
    public const INVALID_HOST_PTR =                       -37;
    public const INVALID_MEM_OBJECT =                     -38;
    public const INVALID_IMAGE_FORMAT_DESCRIPTOR =        -39;
    public const INVALID_IMAGE_SIZE =                     -40;
    public const INVALID_SAMPLER =                        -41;
    public const INVALID_BINARY =                         -42;
    public const INVALID_BUILD_OPTIONS =                  -43;
    public const INVALID_PROGRAM =                        -44;
    public const INVALID_PROGRAM_EXECUTABLE =             -45;
    public const INVALID_KERNEL_NAME =                    -46;
    public const INVALID_KERNEL_DEFINITION =              -47;
    public const INVALID_KERNEL =                         -48;
    public const INVALID_ARG_INDEX =                      -49;
    public const INVALID_ARG_VALUE =                      -50;
    public const INVALID_ARG_SIZE =                       -51;
    public const INVALID_KERNEL_ARGS =                    -52;
    public const INVALID_WORK_DIMENSION =                 -53;
    public const INVALID_WORK_GROUP_SIZE =                -54;
    public const INVALID_WORK_ITEM_SIZE =                 -55;
    public const INVALID_GLOBAL_OFFSET =                  -56;
    public const INVALID_EVENT_WAIT_LIST =                -57;
    public const INVALID_EVENT =                          -58;
    public const INVALID_OPERATION =                      -59;
    public const INVALID_GL_OBJECT =                      -60;
    public const INVALID_BUFFER_SIZE =                    -61;
    public const INVALID_MIP_LEVEL =                      -62;
    public const INVALID_GLOBAL_WORK_SIZE =               -63;
    
    // cl_gl
    public const INVALID_GL_SHAREGROUP_REFERENCE_KHR =  -1000;
    
    // cl_ext
    public const PLATFORM_NOT_FOUND_KHR =               -1001;
    public const DEVICE_PARTITION_FAILED_EXT =          -1057;
    public const INVALID_PARTITION_COUNT_EXT =          -1058;
    public const INVALID_PARTITION_NAME_EXT =           -1059;
    
    // cl_egl
    public const EGL_RESOURCE_NOT_ACQUIRED_KHR =        -1092;
    public const INVALID_EGL_OBJECT_KHR =               -1093;
    
    public function ToString: string; override;
    begin
      var res := typeof(ErrorCode).GetFields.Where(fi->fi.IsLiteral).FirstOrDefault(prop->integer(prop.GetValue(nil)) = self.val);
      Result := res=nil?
        $'ErrorCode[{self.val}]':
        res.Name.ToWords('_').Select(w->w[1].ToUpper+w.Substring(1).ToLower).JoinIntoString;
    end;
    
    public function IS_ERROR := val<>SUCCESS;
    
    public procedure RaiseIfError :=
    if IS_ERROR then raise new OpenCLException(self.ToString);
    
  end;
  
  //R
  BuildStatus = record
    public val: Int32;
    
    public const SUCCESS =      -0;
    public const NONE =         -1;
    public const ERROR =        -2;
    public const IN_PROGRESS =  -3;
    
    public function ToString: string; override;
    begin
      var res := typeof(BuildStatus).GetFields.Where(fi->fi.IsLiteral).FirstOrDefault(prop->integer(prop.GetValue(nil)) = self.val);
      Result := res=nil?
        $'BuildStatus[{self.val}]':
        res.Name;
    end;
    
  end;
  
  {$endregion case Result of}
  
  {$region 1 значение}
  
  {$region ...InfoType}
  
  //S
  PlatformInfoType = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property PROFILE:               PlatformInfoType read new PlatformInfoType($0900);
    public static property VERSION:               PlatformInfoType read new PlatformInfoType($0901);
    public static property NAME:                  PlatformInfoType read new PlatformInfoType($0902);
    public static property VENDOR:                PlatformInfoType read new PlatformInfoType($0903);
    public static property EXTENSIONS:            PlatformInfoType read new PlatformInfoType($0904);
    public static property HOST_TIMER_RESOLUTION: PlatformInfoType read new PlatformInfoType($0905);
    
    // cl_ext
    public static property ICD_SUFFIX_KHR:        PlatformInfoType read new PlatformInfoType($0920);
    
  end;
  
  //S
  DeviceInfoType = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property &TYPE:                                   DeviceInfoType read new DeviceInfoType($1000);
    public static property VENDOR_ID:                               DeviceInfoType read new DeviceInfoType($1001);
    public static property MAX_COMPUTE_UNITS:                       DeviceInfoType read new DeviceInfoType($1002);
    public static property MAX_WORK_ITEM_DIMENSIONS:                DeviceInfoType read new DeviceInfoType($1003);
    public static property MAX_WORK_GROUP_SIZE:                     DeviceInfoType read new DeviceInfoType($1004);
    public static property MAX_WORK_ITEM_SIZES:                     DeviceInfoType read new DeviceInfoType($1005);
    public static property PREFERRED_VECTOR_WIDTH_CHAR:             DeviceInfoType read new DeviceInfoType($1006);
    public static property PREFERRED_VECTOR_WIDTH_SHORT:            DeviceInfoType read new DeviceInfoType($1007);
    public static property PREFERRED_VECTOR_WIDTH_INT:              DeviceInfoType read new DeviceInfoType($1008);
    public static property PREFERRED_VECTOR_WIDTH_LONG:             DeviceInfoType read new DeviceInfoType($1009);
    public static property PREFERRED_VECTOR_WIDTH_FLOAT:            DeviceInfoType read new DeviceInfoType($100A);
    public static property PREFERRED_VECTOR_WIDTH_DOUBLE:           DeviceInfoType read new DeviceInfoType($100B);
    public static property MAX_CLOCK_FREQUENCY:                     DeviceInfoType read new DeviceInfoType($100C);
    public static property ADDRESS_BITS:                            DeviceInfoType read new DeviceInfoType($100D);
    public static property MAX_READ_IMAGE_ARGS:                     DeviceInfoType read new DeviceInfoType($100E);
    public static property MAX_WRITE_IMAGE_ARGS:                    DeviceInfoType read new DeviceInfoType($100F);
    public static property MAX_MEM_ALLOC_SIZE:                      DeviceInfoType read new DeviceInfoType($1010);
    public static property IMAGE2D_MAX_WIDTH:                       DeviceInfoType read new DeviceInfoType($1011);
    public static property IMAGE2D_MAX_HEIGHT:                      DeviceInfoType read new DeviceInfoType($1012);
    public static property IMAGE3D_MAX_WIDTH:                       DeviceInfoType read new DeviceInfoType($1013);
    public static property IMAGE3D_MAX_HEIGHT:                      DeviceInfoType read new DeviceInfoType($1014);
    public static property IMAGE3D_MAX_DEPTH:                       DeviceInfoType read new DeviceInfoType($1015);
    public static property IMAGE_SUPPORT:                           DeviceInfoType read new DeviceInfoType($1016);
    public static property MAX_PARAMETER_SIZE:                      DeviceInfoType read new DeviceInfoType($1017);
    public static property MAX_SAMPLERS:                            DeviceInfoType read new DeviceInfoType($1018);
    public static property MEM_BASE_ADDR_ALIGN:                     DeviceInfoType read new DeviceInfoType($1019);
    public static property MIN_DATA_TYPE_ALIGN_SIZE:                DeviceInfoType read new DeviceInfoType($101A);
    public static property SINGLE_FP_CONFIG:                        DeviceInfoType read new DeviceInfoType($101B);
    public static property GLOBAL_MEM_CACHE_TYPE:                   DeviceInfoType read new DeviceInfoType($101C);
    public static property GLOBAL_MEM_CACHELINE_SIZE:               DeviceInfoType read new DeviceInfoType($101D);
    public static property GLOBAL_MEM_CACHE_SIZE:                   DeviceInfoType read new DeviceInfoType($101E);
    public static property GLOBAL_MEM_SIZE:                         DeviceInfoType read new DeviceInfoType($101F);
    public static property MAX_CONSTANT_BUFFER_SIZE:                DeviceInfoType read new DeviceInfoType($1020);
    public static property MAX_CONSTANT_ARGS:                       DeviceInfoType read new DeviceInfoType($1021);
    public static property LOCAL_MEM_TYPE:                          DeviceInfoType read new DeviceInfoType($1022);
    public static property LOCAL_MEM_SIZE:                          DeviceInfoType read new DeviceInfoType($1023);
    public static property ERROR_CORRECTION_SUPPORT:                DeviceInfoType read new DeviceInfoType($1024);
    public static property PROFILING_TIMER_RESOLUTION:              DeviceInfoType read new DeviceInfoType($1025);
    public static property ENDIAN_LITTLE:                           DeviceInfoType read new DeviceInfoType($1026);
    public static property AVAILABLE:                               DeviceInfoType read new DeviceInfoType($1027);
    public static property COMPILER_AVAILABLE:                      DeviceInfoType read new DeviceInfoType($1028);
    public static property EXECUTION_CAPABILITIES:                  DeviceInfoType read new DeviceInfoType($1029);
    /// Устарело
    public static property QUEUE_PROPERTIES:                        DeviceInfoType read new DeviceInfoType($102A);
    public static property QUEUE_ON_HOST_PROPERTIES:                DeviceInfoType read new DeviceInfoType($102A);
    public static property NAME:                                    DeviceInfoType read new DeviceInfoType($102B);
    public static property VENDOR:                                  DeviceInfoType read new DeviceInfoType($102C);
    public static property CL_DRIVER_VERSION:                       DeviceInfoType read new DeviceInfoType($102D);
    public static property PROFILE:                                 DeviceInfoType read new DeviceInfoType($102E);
    public static property VERSION:                                 DeviceInfoType read new DeviceInfoType($102F);
    public static property EXTENSIONS:                              DeviceInfoType read new DeviceInfoType($1030);
    public static property PLATFORM:                                DeviceInfoType read new DeviceInfoType($1031);
    public static property DOUBLE_FP_CONFIG:                        DeviceInfoType read new DeviceInfoType($1032);
    public static property HALF_FP_CONFIG:                          DeviceInfoType read new DeviceInfoType($1033);
    public static property PREFERRED_VECTOR_WIDTH_HALF:             DeviceInfoType read new DeviceInfoType($1034);
    /// Устарело
    public static property HOST_UNIFIED_MEMORY:                     DeviceInfoType read new DeviceInfoType($1035);
    public static property NATIVE_VECTOR_WIDTH_CHAR:                DeviceInfoType read new DeviceInfoType($1036);
    public static property NATIVE_VECTOR_WIDTH_SHORT:               DeviceInfoType read new DeviceInfoType($1037);
    public static property NATIVE_VECTOR_WIDTH_INT:                 DeviceInfoType read new DeviceInfoType($1038);
    public static property NATIVE_VECTOR_WIDTH_LONG:                DeviceInfoType read new DeviceInfoType($1039);
    public static property NATIVE_VECTOR_WIDTH_FLOAT:               DeviceInfoType read new DeviceInfoType($103A);
    public static property NATIVE_VECTOR_WIDTH_DOUBLE:              DeviceInfoType read new DeviceInfoType($103B);
    public static property NATIVE_VECTOR_WIDTH_HALF:                DeviceInfoType read new DeviceInfoType($103C);
    public static property OPENCL_C_VERSION:                        DeviceInfoType read new DeviceInfoType($103D);
    public static property LINKER_AVAILABLE:                        DeviceInfoType read new DeviceInfoType($103E);
    public static property BUILT_IN_KERNELS:                        DeviceInfoType read new DeviceInfoType($103F);
    public static property IMAGE_MAX_BUFFER_SIZE:                   DeviceInfoType read new DeviceInfoType($1040);
    public static property IMAGE_MAX_ARRAY_SIZE:                    DeviceInfoType read new DeviceInfoType($1041);
    public static property PARENT_DEVICE:                           DeviceInfoType read new DeviceInfoType($1042);
    public static property PARTITION_MAX_SUB_DEVICES:               DeviceInfoType read new DeviceInfoType($1043);
    public static property PARTITION_PROPERTIES:                    DeviceInfoType read new DeviceInfoType($1044);
    public static property PARTITION_AFFINITY_DOMAIN:               DeviceInfoType read new DeviceInfoType($1045);
    public static property PARTITION_TYPE:                          DeviceInfoType read new DeviceInfoType($1046);
    public static property REFERENCE_COUNT:                         DeviceInfoType read new DeviceInfoType($1047);
    public static property PREFERRED_INTEROP_USER_SYNC:             DeviceInfoType read new DeviceInfoType($1048);
    public static property PRINTF_BUFFER_SIZE:                      DeviceInfoType read new DeviceInfoType($1049);
    public static property IMAGE_PITCH_ALIGNMENT:                   DeviceInfoType read new DeviceInfoType($104A);
    public static property IMAGE_BASE_ADDRESS_ALIGNMENT:            DeviceInfoType read new DeviceInfoType($104B);
    public static property MAX_READ_WRITE_IMAGE_ARGS:               DeviceInfoType read new DeviceInfoType($104C);
    public static property MAX_GLOBAL_VARIABLE_SIZE:                DeviceInfoType read new DeviceInfoType($104D);
    public static property QUEUE_ON_DEVICE_PROPERTIES:              DeviceInfoType read new DeviceInfoType($104E);
    public static property QUEUE_ON_DEVICE_PREFERRED_SIZE:          DeviceInfoType read new DeviceInfoType($104F);
    public static property QUEUE_ON_DEVICE_MAX_SIZE:                DeviceInfoType read new DeviceInfoType($1050);
    public static property MAX_ON_DEVICE_QUEUES:                    DeviceInfoType read new DeviceInfoType($1051);
    public static property MAX_ON_DEVICE_EVENTS:                    DeviceInfoType read new DeviceInfoType($1052);
    public static property SVM_CAPABILITIES:                        DeviceInfoType read new DeviceInfoType($1053);
    public static property GLOBAL_VARIABLE_PREFERRED_TOTAL_SIZE:    DeviceInfoType read new DeviceInfoType($1054);
    public static property MAX_PIPE_ARGS:                           DeviceInfoType read new DeviceInfoType($1055);
    public static property PIPE_MAX_ACTIVE_RESERVATIONS:            DeviceInfoType read new DeviceInfoType($1056);
    public static property PIPE_MAX_PACKET_SIZE:                    DeviceInfoType read new DeviceInfoType($1057);
    public static property PREFERRED_PLATFORM_ATOMIC_ALIGNMENT:     DeviceInfoType read new DeviceInfoType($1058);
    public static property PREFERRED_GLOBAL_ATOMIC_ALIGNMENT:       DeviceInfoType read new DeviceInfoType($1059);
    public static property PREFERRED_LOCAL_ATOMIC_ALIGNMENT:        DeviceInfoType read new DeviceInfoType($105A);
    public static property IL_VERSION:                              DeviceInfoType read new DeviceInfoType($105B);
    public static property MAX_NUM_SUB_GROUPS:                      DeviceInfoType read new DeviceInfoType($105C);
    public static property SUB_GROUP_INDEPENDENT_FORWARD_PROGRESS:  DeviceInfoType read new DeviceInfoType($105D);
    
    // cl_ext
    public static property TERMINATE_CAPABILITY_KHR:                DeviceInfoType read new DeviceInfoType($2031);
    public static property SPIR_VERSIONS:                           DeviceInfoType read new DeviceInfoType($40E0);
    public static property COMPUTE_CAPABILITY_MAJOR_NV:             DeviceInfoType read new DeviceInfoType($4000);
    public static property COMPUTE_CAPABILITY_MINOR_NV:             DeviceInfoType read new DeviceInfoType($4001);
    public static property REGISTERS_PER_BLOCK_NV:                  DeviceInfoType read new DeviceInfoType($4002);
    public static property WARP_SIZE_NV:                            DeviceInfoType read new DeviceInfoType($4003);
    public static property GPU_OVERLAP_NV:                          DeviceInfoType read new DeviceInfoType($4004);
    public static property KERNEL_EXEC_TIMEOUT_NV:                  DeviceInfoType read new DeviceInfoType($4005);
    public static property INTEGRATED_MEMORY_NV:                    DeviceInfoType read new DeviceInfoType($4006);
    public static property PROFILING_TIMER_OFFSET_AMD:              DeviceInfoType read new DeviceInfoType($4036);
    public static property PARENT_DEVICE_EXT:                       DeviceInfoType read new DeviceInfoType($4054);
    public static property PARTITION_TYPES_EXT:                     DeviceInfoType read new DeviceInfoType($4055);
    public static property AFFINITY_DOMAINS_EXT:                    DeviceInfoType read new DeviceInfoType($4056);
    public static property REFERENCE_COUNT_EXT:                     DeviceInfoType read new DeviceInfoType($4057);
    public static property PARTITION_STYLE_EXT:                     DeviceInfoType read new DeviceInfoType($4058);
    public static property MAX_NAMED_BARRIER_COUNT_KHR:             DeviceInfoType read new DeviceInfoType($2035);
    public static property SVM_CAPABILITIES_ARM:                    DeviceInfoType read new DeviceInfoType($40B6);
    public static property COMPUTE_UNITS_BITFIELD_ARM:              DeviceInfoType read new DeviceInfoType($40BF);
    
  end;
  
  //S
  ContextInfoType = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property CL_CONTEXT_REFERENCE_COUNT:  ContextInfoType read new ContextInfoType($1080);
    public static property CL_CONTEXT_DEVICES:          ContextInfoType read new ContextInfoType($1081);
    public static property CL_CONTEXT_PROPERTIES:       ContextInfoType read new ContextInfoType($1082);
    public static property CL_CONTEXT_NUM_DEVICES:      ContextInfoType read new ContextInfoType($1083);
    
  end;
  
  //S
  CommandQueueInfoType = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property CONTEXT:         CommandQueueInfoType read new CommandQueueInfoType($1090);
    public static property DEVICE:          CommandQueueInfoType read new CommandQueueInfoType($1091);
    public static property REFERENCE_COUNT: CommandQueueInfoType read new CommandQueueInfoType($1092);
    public static property PROPERTIES:      CommandQueueInfoType read new CommandQueueInfoType($1093);
    public static property SIZE:            CommandQueueInfoType read new CommandQueueInfoType($1094);
    public static property DEVICE_DEFAULT:  CommandQueueInfoType read new CommandQueueInfoType($1095);
    public static property PRIORITY_KHR:    CommandQueueInfoType read new CommandQueueInfoType($1096); // cl_ext
    public static property THROTTLE_KHR:    CommandQueueInfoType read new CommandQueueInfoType($1097); // cl_ext
    
  end;
  
  //S
  BufferCreateType = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property REGION: BufferCreateType read new BufferCreateType($1220);
    
  end;
  
  //S
  ImageInfoType = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property FORMAT:          ImageInfoType read new ImageInfoType($1110);
    public static property ELEMENT_SIZE:    ImageInfoType read new ImageInfoType($1111);
    public static property ROW_PITCH:       ImageInfoType read new ImageInfoType($1112);
    public static property SLICE_PITCH:     ImageInfoType read new ImageInfoType($1113);
    public static property WIDTH:           ImageInfoType read new ImageInfoType($1114);
    public static property HEIGHT:          ImageInfoType read new ImageInfoType($1115);
    public static property DEPTH:           ImageInfoType read new ImageInfoType($1116);
    public static property ARRAY_SIZE:      ImageInfoType read new ImageInfoType($1117);
    public static property BUFFER:          ImageInfoType read new ImageInfoType($1118);
    public static property NUM_MIP_LEVELS:  ImageInfoType read new ImageInfoType($1119);
    public static property NUM_SAMPLES:     ImageInfoType read new ImageInfoType($111A);
    
  end;
  
  //S
  PipeInfoType = record
    public val: IntPtr;
    public constructor(val: IntPtr) := self.val := val;
    public constructor(val: int64) := self.val := new IntPtr(val);
    
    public static property PACKET_SIZE: PipeInfoType read new PipeInfoType($1120);
    public static property MAX_PACKETS: PipeInfoType read new PipeInfoType($1121);
    
  end;
  
  //S
  MemObjInfoType = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property &TYPE:                 MemObjInfoType read new MemObjInfoType($1100);
    public static property FLAGS:                 MemObjInfoType read new MemObjInfoType($1101);
    public static property SIZE:                  MemObjInfoType read new MemObjInfoType($1102);
    public static property HOST_PTR:              MemObjInfoType read new MemObjInfoType($1103);
    public static property MAP_COUNT:             MemObjInfoType read new MemObjInfoType($1104);
    public static property REFERENCE_COUNT:       MemObjInfoType read new MemObjInfoType($1105);
    public static property CONTEXT:               MemObjInfoType read new MemObjInfoType($1106);
    public static property ASSOCIATED_MEMOBJECT:  MemObjInfoType read new MemObjInfoType($1107);
    public static property OFFSET:                MemObjInfoType read new MemObjInfoType($1108);
    public static property USES_SVM_POINTER:      MemObjInfoType read new MemObjInfoType($1109);
    public static property USES_SVM_POINTER_ARM:  MemObjInfoType read new MemObjInfoType($40B7); // cl_ext
    
  end;
  
  //S
  SamplerInfoType = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property REFERENCE_COUNT:   SamplerInfoType read new SamplerInfoType($1150);
    public static property CONTEXT:           SamplerInfoType read new SamplerInfoType($1151);
    public static property NORMALIZED_COORDS: SamplerInfoType read new SamplerInfoType($1152);
    public static property ADDRESSING_MODE:   SamplerInfoType read new SamplerInfoType($1153);
    public static property FILTER_MODE:       SamplerInfoType read new SamplerInfoType($1154);
    public static property MIP_FILTER_MODE:   SamplerInfoType read new SamplerInfoType($1155);
    public static property LOD_MIN:           SamplerInfoType read new SamplerInfoType($1156);
    public static property LOD_MAX:           SamplerInfoType read new SamplerInfoType($1157);
    
  end;
  
  //S
  ProgramInfoType = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property REFERENCE_COUNT:             ProgramInfoType read new ProgramInfoType($1160);
    public static property CONTEXT:                     ProgramInfoType read new ProgramInfoType($1161);
    public static property NUM_DEVICES:                 ProgramInfoType read new ProgramInfoType($1162);
    public static property DEVICES:                     ProgramInfoType read new ProgramInfoType($1163);
    public static property SOURCE:                      ProgramInfoType read new ProgramInfoType($1164);
    public static property BINARY_SIZES:                ProgramInfoType read new ProgramInfoType($1165);
    public static property BINARIES:                    ProgramInfoType read new ProgramInfoType($1166);
    public static property NUM_KERNELS:                 ProgramInfoType read new ProgramInfoType($1167);
    public static property KERNEL_NAMES:                ProgramInfoType read new ProgramInfoType($1168);
    public static property IL:                          ProgramInfoType read new ProgramInfoType($1169);
    public static property SCOPE_GLOBAL_CTORS_PRESENT:  ProgramInfoType read new ProgramInfoType($116A);
    public static property SCOPE_GLOBAL_DTORS_PRESENT:  ProgramInfoType read new ProgramInfoType($116B);
    
    // cl_ext
    public static property IL_KHR:                      ProgramInfoType read new ProgramInfoType($1169);
    
  end;
  
  //S
  ProgramBuildInfoType = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property STATUS:                      ProgramBuildInfoType read new ProgramBuildInfoType($1181);
    public static property OPTIONS:                     ProgramBuildInfoType read new ProgramBuildInfoType($1182);
    public static property LOG:                         ProgramBuildInfoType read new ProgramBuildInfoType($1183);
    public static property BINARY_TYPE:                 ProgramBuildInfoType read new ProgramBuildInfoType($1184);
    public static property GLOBAL_VARIABLE_TOTAL_SIZE:  ProgramBuildInfoType read new ProgramBuildInfoType($1185);
    
    // cl_ext
    public static property BINARY_TYPE_INTERMEDIATE:    ProgramBuildInfoType read new ProgramBuildInfoType($40E1);
    
  end;
  
  //S
  KernelExecInfoType = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property SVM_PTRS:              KernelExecInfoType read new KernelExecInfoType($11B6);
    public static property SVM_FINE_GRAIN_SYSTEM: KernelExecInfoType read new KernelExecInfoType($11B7);
    
  end;
  
  //S
  KernelExecARMInfoType = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property SVM_PTRS:              KernelExecARMInfoType read new KernelExecARMInfoType($40B8);
    public static property SVM_FINE_GRAIN_SYSTEM: KernelExecARMInfoType read new KernelExecARMInfoType($40B9);
    
  end;
  
  //S
  KernelArgInfoType = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property ADDRESS_QUALIFIER: KernelArgInfoType read new KernelArgInfoType($1196);
    public static property ACCESS_QUALIFIER:  KernelArgInfoType read new KernelArgInfoType($1197);
    public static property TYPE_NAME:         KernelArgInfoType read new KernelArgInfoType($1198);
    public static property TYPE_QUALIFIER:    KernelArgInfoType read new KernelArgInfoType($1199);
    public static property NAME:              KernelArgInfoType read new KernelArgInfoType($119A);
    
  end;
  
  //S
  KernelSubGroupInfoType = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property MAX_SUB_GROUP_SIZE_FOR_NDRANGE:  KernelSubGroupInfoType read new KernelSubGroupInfoType($2033);
    public static property SUB_GROUP_COUNT_FOR_NDRANGE:     KernelSubGroupInfoType read new KernelSubGroupInfoType($2034);
    public static property LOCAL_SIZE_FOR_SUB_GROUP_COUNT:  KernelSubGroupInfoType read new KernelSubGroupInfoType($11B8);
    
  end;
  
  //S
  KernelInfoType = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property FUNCTION_NAME:           KernelInfoType read new KernelInfoType($1190);
    public static property NUM_ARGS:                KernelInfoType read new KernelInfoType($1191);
    public static property REFERENCE_COUNT:         KernelInfoType read new KernelInfoType($1192);
    public static property CONTEXT:                 KernelInfoType read new KernelInfoType($1193);
    public static property &PROGRAM:                KernelInfoType read new KernelInfoType($1194);
    public static property ATTRIBUTES:              KernelInfoType read new KernelInfoType($1195);
    public static property MAX_NUM_SUB_GROUPS:      KernelInfoType read new KernelInfoType($11B9);
    public static property COMPILE_NUM_SUB_GROUPS:  KernelInfoType read new KernelInfoType($11BA);
    
  end;
  
  //S
  KernelWorkGroupInfoType = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property SIZE:                    KernelWorkGroupInfoType read new KernelWorkGroupInfoType($11B0);
    public static property COMPILE_SIZE:            KernelWorkGroupInfoType read new KernelWorkGroupInfoType($11B1);
    public static property LOCAL_MEM_SIZE:          KernelWorkGroupInfoType read new KernelWorkGroupInfoType($11B2);
    public static property PREFERRED_SIZE_MULTIPLE: KernelWorkGroupInfoType read new KernelWorkGroupInfoType($11B3);
    public static property PRIVATE_MEM_SIZE:        KernelWorkGroupInfoType read new KernelWorkGroupInfoType($11B4);
    public static property COMPILE_NUM_SUB_GROUPS:  KernelWorkGroupInfoType read new KernelWorkGroupInfoType($11B5);
    
  end;
  
  //S
  EventInfoType = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property COMMAND_QUEUE:             EventInfoType read new EventInfoType($11D0);
    public static property COMMAND_TYPE:              EventInfoType read new EventInfoType($11D1);
    public static property REFERENCE_COUNT:           EventInfoType read new EventInfoType($11D2);
    public static property COMMAND_EXECUTION_STATUS:  EventInfoType read new EventInfoType($11D3);
    public static property CONTEXT:                   EventInfoType read new EventInfoType($11D4);
    
  end;
  
  //S
  ProfilingInfoType = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property COMMAND_QUEUED:    ProfilingInfoType read new ProfilingInfoType($1280);
    public static property COMMAND_SUBMIT:    ProfilingInfoType read new ProfilingInfoType($1281);
    public static property COMMAND_START:     ProfilingInfoType read new ProfilingInfoType($1282);
    public static property COMMAND_END:       ProfilingInfoType read new ProfilingInfoType($1283);
    public static property COMMAND_COMPLETE:  ProfilingInfoType read new ProfilingInfoType($1284);
    
  end;
  
  //S
  GLTextureInfoType = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property TEXTURE_TARGET:  GLTextureInfoType read new GLTextureInfoType($2004);
    public static property MIPMAP_LEVEL:    GLTextureInfoType read new GLTextureInfoType($2005);
    public static property NUM_SAMPLES:     GLTextureInfoType read new GLTextureInfoType($2012);
    
  end;
  
  //S
  GLContextInfoType = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property CURRENT_DEVICE_FOR_GL_CONTEXT_KHR: GLContextInfoType read new GLContextInfoType($2006);
    public static property DEVICES_FOR_GL_CONTEXT_KHR:        GLContextInfoType read new GLContextInfoType($2007);
    
  end;
  
  {$endregion ...InfoType}
  
  //S
  ImportPropertiesARM = record
    public val: IntPtr;
    public constructor(val: IntPtr) := self.val := val;
    public constructor(val: int64) := self.val := new IntPtr(val);
    
    public static property TYPE_ARM:      ImportPropertiesARM read new ImportPropertiesARM($40B2);
    public static property HOST_ARM:      ImportPropertiesARM read new ImportPropertiesARM($40B3);
    public static property DMA_BUF_ARM:   ImportPropertiesARM read new ImportPropertiesARM($40B4);
    public static property PROTECTED_ARM: ImportPropertiesARM read new ImportPropertiesARM($40B5);
    
  end;
  
  //SR
  DevicePartitionProperties = record
    public val: IntPtr;
    public constructor(val: IntPtr) := self.val := val;
    public constructor(val: int64) := self.val := new IntPtr(val);
    
    
    
    public static property EQUALLY:                 DevicePartitionProperties read new DevicePartitionProperties($1086);
    public static property BY_COUNTS:               DevicePartitionProperties read new DevicePartitionProperties($1087);
    public static property BY_COUNTS_LIST_END:      DevicePartitionProperties read new DevicePartitionProperties(    0);
    public static property BY_AFFINITY_DOMAIN:      DevicePartitionProperties read new DevicePartitionProperties($1088);
    
    // cl_ext
    public static property EQUALLY_EXT:             DevicePartitionProperties read new DevicePartitionProperties($4050);
    public static property BY_COUNTS_EXT:           DevicePartitionProperties read new DevicePartitionProperties($4051);
    public static property BY_NAMES_EXT:            DevicePartitionProperties read new DevicePartitionProperties($4052);
    public static property BY_AFFINITY_DOMAIN_EXT:  DevicePartitionProperties read new DevicePartitionProperties($4053);
    public static property LIST_END_EXT:            DevicePartitionProperties read new DevicePartitionProperties(    0);
    public static property BY_COUNTS_LIST_END_EXT:  DevicePartitionProperties read new DevicePartitionProperties(    0);
    public static property BY_NAMES_LIST_END_EXT:   DevicePartitionProperties read new DevicePartitionProperties(   -1);
    
    
    
    public property IS_EQUALLY:                 boolean read self = DevicePartitionProperties.EQUALLY;
    public property IS_BY_COUNTS:               boolean read self = DevicePartitionProperties.BY_COUNTS;
    public property IS_BY_COUNTS_LIST_END:      boolean read self = DevicePartitionProperties.BY_COUNTS_LIST_END;
    public property IS_BY_AFFINITY_DOMAIN:      boolean read self = DevicePartitionProperties.BY_AFFINITY_DOMAIN;
    
    // cl_ext
    public property IS_EQUALLY_EXT:             boolean read self = DevicePartitionProperties.EQUALLY_EXT;
    public property IS_BY_COUNTS_EXT:           boolean read self = DevicePartitionProperties.BY_COUNTS_EXT;
    public property IS_BY_NAMES_EXT:            boolean read self = DevicePartitionProperties.BY_NAMES_EXT;
    public property IS_BY_AFFINITY_DOMAIN_EXT:  boolean read self = DevicePartitionProperties.BY_AFFINITY_DOMAIN_EXT;
    public property IS_LIST_END_EXT:            boolean read self = DevicePartitionProperties.LIST_END_EXT;
    public property IS_BY_COUNTS_LIST_END_EXT:  boolean read self = DevicePartitionProperties.BY_COUNTS_LIST_END_EXT;
    public property IS_BY_NAMES_LIST_END_EXT:   boolean read self = DevicePartitionProperties.BY_NAMES_LIST_END_EXT;
    
    
    
    public function ToString: string; override;
    begin
      var res := typeof(DevicePartitionProperties).GetProperties.Where(prop->prop.PropertyType=typeof(boolean)).Select(prop->(prop.Name,boolean(prop.GetValue(self)))).FirstOrDefault(t->t[1]);
      Result := res=nil?
        $'DevicePartitionProperties[{self.val}]':
        res[0].Substring(3);
    end;
    
  end;
  
  //SR
  ContextProperties = record
    public val: IntPtr;
    public constructor(val: IntPtr) := self.val := val;
    public constructor(val: int64) := self.val := new IntPtr(val);
    
    
    
    public static property PLATFORM:              ContextProperties read new ContextProperties($1084);
    public static property INTEROP_USER_SYNC:     ContextProperties read new ContextProperties($1085);
    
    // cl_ext
    public static property TERMINATE_KHR:         ContextProperties read new ContextProperties($2032);
    public static property PRINTF_CALLBACK_ARM:   ContextProperties read new ContextProperties($40B0);
    public static property PRINTF_BUFFERSIZE_ARM: ContextProperties read new ContextProperties($40B1);
    
    // cl_gl
    public static property GL_CONTEXT_KHR:        ContextProperties read new ContextProperties($2008);
    public static property EGL_DISPLAY_KHR:       ContextProperties read new ContextProperties($2009);
    public static property GLX_DISPLAY_KHR:       ContextProperties read new ContextProperties($200A);
    public static property WGL_HDC_KHR:           ContextProperties read new ContextProperties($200B);
    public static property CGL_SHAREGROUP_KHR:    ContextProperties read new ContextProperties($200C);
    
    
    public property IS_PLATFORM:              boolean read self = ContextProperties.PLATFORM;
    public property IS_INTEROP_USER_SYNC:     boolean read self = ContextProperties.INTEROP_USER_SYNC;
    
    // cl_ext
    public property IS_TERMINATE_KHR:         boolean read self = ContextProperties.TERMINATE_KHR;
    public property IS_PRINTF_CALLBACK_ARM:   boolean read self = ContextProperties.PRINTF_CALLBACK_ARM;
    public property IS_PRINTF_BUFFERSIZE_ARM: boolean read self = ContextProperties.PRINTF_BUFFERSIZE_ARM;
    
    // cl_gl
    public property IS_GL_CONTEXT_KHR:        boolean read self = ContextProperties.GL_CONTEXT_KHR; 
    public property IS_EGL_DISPLAY_KHR:       boolean read self = ContextProperties.EGL_DISPLAY_KHR;
    public property IS_GLX_DISPLAY_KHR:       boolean read self = ContextProperties.GLX_DISPLAY_KHR;
    public property IS_WGL_HDC_KHR:           boolean read self = ContextProperties.WGL_HDC_KHR;
    public property IS_CGL_SHAREGROUP_KHR:    boolean read self = ContextProperties.CGL_SHAREGROUP_KHR;
    
    
    
    public function ToString: string; override;
    begin
      var res := typeof(ContextProperties).GetProperties.Where(prop->prop.PropertyType=typeof(boolean)).Select(prop->(prop.Name,boolean(prop.GetValue(self)))).FirstOrDefault(t->t[1]);
      Result := res=nil?
        $'ContextProperties[{self.val}]':
        res[0].Substring(3);
    end;
    
  end;
  
  //SR
  ChannelOrder = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property R:             ChannelOrder read new ChannelOrder($10B0);
    public static property A:             ChannelOrder read new ChannelOrder($10B1);
    public static property RG:            ChannelOrder read new ChannelOrder($10B2);
    public static property RA:            ChannelOrder read new ChannelOrder($10B3);
    public static property RGB:           ChannelOrder read new ChannelOrder($10B4);
    public static property RGBA:          ChannelOrder read new ChannelOrder($10B5);
    public static property BGRA:          ChannelOrder read new ChannelOrder($10B6);
    public static property ARGB:          ChannelOrder read new ChannelOrder($10B7);
    public static property INTENSITY:     ChannelOrder read new ChannelOrder($10B8);
    public static property LUMINANCE:     ChannelOrder read new ChannelOrder($10B9);
    public static property Rx:            ChannelOrder read new ChannelOrder($10BA);
    public static property RGx:           ChannelOrder read new ChannelOrder($10BB);
    public static property RGBx:          ChannelOrder read new ChannelOrder($10BC);
    public static property DEPTH:         ChannelOrder read new ChannelOrder($10BD);
    public static property DEPTH_STENCIL: ChannelOrder read new ChannelOrder($10BE);
    public static property sRGB:          ChannelOrder read new ChannelOrder($10BF);
    public static property sRGBx:         ChannelOrder read new ChannelOrder($10C0);
    public static property sRGBA:         ChannelOrder read new ChannelOrder($10C1);
    public static property sBGRA:         ChannelOrder read new ChannelOrder($10C2);
    public static property ABGR:          ChannelOrder read new ChannelOrder($10C3);
    
    public property IS_R:             boolean read self = ChannelOrder.R;
    public property IS_A:             boolean read self = ChannelOrder.A;
    public property IS_RG:            boolean read self = ChannelOrder.RG;
    public property IS_RA:            boolean read self = ChannelOrder.RA;
    public property IS_RGB:           boolean read self = ChannelOrder.RGB;
    public property IS_RGBA:          boolean read self = ChannelOrder.RGBA;
    public property IS_BGRA:          boolean read self = ChannelOrder.BGRA;
    public property IS_ARGB:          boolean read self = ChannelOrder.ARGB;
    public property IS_INTENSITY:     boolean read self = ChannelOrder.INTENSITY;
    public property IS_LUMINANCE:     boolean read self = ChannelOrder.LUMINANCE;
    public property IS_Rx:            boolean read self = ChannelOrder.Rx;
    public property IS_RGx:           boolean read self = ChannelOrder.RGx;
    public property IS_RGBx:          boolean read self = ChannelOrder.RGBx;
    public property IS_DEPTH:         boolean read self = ChannelOrder.DEPTH;
    public property IS_DEPTH_STENCIL: boolean read self = ChannelOrder.DEPTH_STENCIL;
    public property IS_sRGB:          boolean read self = ChannelOrder.sRGB;
    public property IS_sRGBx:         boolean read self = ChannelOrder.sRGBx;
    public property IS_sRGBA:         boolean read self = ChannelOrder.sRGBA;
    public property IS_sBGRA:         boolean read self = ChannelOrder.sBGRA;
    public property IS_ABGR:          boolean read self = ChannelOrder.ABGR;
    
    public function ToString: string; override;
    begin
      var res := typeof(ChannelOrder).GetProperties.Where(prop->prop.PropertyType=typeof(boolean)).Select(prop->(prop.Name,boolean(prop.GetValue(self)))).FirstOrDefault(t->t[1]);
      Result := res=nil?
        $'ChannelOrder[{self.val}]':
        res[0].Substring(3);
    end;
    
  end;
  
  //SR
  ChannelType = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property SNORM_INT8:            ChannelType read new ChannelType($10D0);
    public static property SNORM_INT16:           ChannelType read new ChannelType($10D1);
    public static property UNORM_INT8:            ChannelType read new ChannelType($10D2);
    public static property UNORM_INT16:           ChannelType read new ChannelType($10D3);
    public static property UNORM_SHORT_565:       ChannelType read new ChannelType($10D4);
    public static property UNORM_SHORT_555:       ChannelType read new ChannelType($10D5);
    public static property UNORM_INT_101010:      ChannelType read new ChannelType($10D6);
    public static property SIGNED_INT8:           ChannelType read new ChannelType($10D7);
    public static property SIGNED_INT16:          ChannelType read new ChannelType($10D8);
    public static property SIGNED_INT32:          ChannelType read new ChannelType($10D9);
    public static property UNSIGNED_INT8:         ChannelType read new ChannelType($10DA);
    public static property UNSIGNED_INT16:        ChannelType read new ChannelType($10DB);
    public static property UNSIGNED_INT32:        ChannelType read new ChannelType($10DC);
    public static property HALF_FLOAT:            ChannelType read new ChannelType($10DD);
    public static property FLOAT:                 ChannelType read new ChannelType($10DE);
    public static property UNORM_INT24:           ChannelType read new ChannelType($10DF);
    public static property CL_UNORM_INT_101010_2: ChannelType read new ChannelType($10E0);
    
    public property IS_SNORM_INT8:            boolean read self = ChannelType.SNORM_INT8;
    public property IS_SNORM_INT16:           boolean read self = ChannelType.SNORM_INT16;
    public property IS_UNORM_INT8:            boolean read self = ChannelType.UNORM_INT8;
    public property IS_UNORM_INT16:           boolean read self = ChannelType.UNORM_INT16;
    public property IS_UNORM_SHORT_565:       boolean read self = ChannelType.UNORM_SHORT_565;
    public property IS_UNORM_SHORT_555:       boolean read self = ChannelType.UNORM_SHORT_555;
    public property IS_UNORM_INT_101010:      boolean read self = ChannelType.UNORM_INT_101010;
    public property IS_SIGNED_INT8:           boolean read self = ChannelType.SIGNED_INT8;
    public property IS_SIGNED_INT16:          boolean read self = ChannelType.SIGNED_INT16;
    public property IS_SIGNED_INT32:          boolean read self = ChannelType.SIGNED_INT32;
    public property IS_UNSIGNED_INT8:         boolean read self = ChannelType.UNSIGNED_INT8;
    public property IS_UNSIGNED_INT16:        boolean read self = ChannelType.UNSIGNED_INT16;
    public property IS_UNSIGNED_INT32:        boolean read self = ChannelType.UNSIGNED_INT32;
    public property IS_HALF_FLOAT:            boolean read self = ChannelType.HALF_FLOAT;
    public property IS_FLOAT:                 boolean read self = ChannelType.FLOAT;
    public property IS_UNORM_INT24:           boolean read self = ChannelType.UNORM_INT24;
    public property IS_CL_UNORM_INT_101010_2: boolean read self = ChannelType.CL_UNORM_INT_101010_2;
    
    public function ToString: string; override;
    begin
      var res := typeof(ChannelType).GetProperties.Where(prop->prop.PropertyType=typeof(boolean)).Select(prop->(prop.Name,boolean(prop.GetValue(self)))).FirstOrDefault(t->t[1]);
      Result := res=nil?
        $'ChannelType[{self.val}]':
        res[0].Substring(3);
    end;
    
  end;
  
  //SR
  MemObjectType = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property BUFFER:          MemObjectType read new MemObjectType($10F0);
    public static property IMAGE2D:         MemObjectType read new MemObjectType($10F1);
    public static property IMAGE3D:         MemObjectType read new MemObjectType($10F2);
    public static property IMAGE2D_ARRAY:   MemObjectType read new MemObjectType($10F3);
    public static property IMAGE1D:         MemObjectType read new MemObjectType($10F4);
    public static property IMAGE1D_ARRAY:   MemObjectType read new MemObjectType($10F5);
    public static property IMAGE1D_BUFFER:  MemObjectType read new MemObjectType($10F6);
    public static property PIPE:            MemObjectType read new MemObjectType($10F7);
    
    public property IS_BUFFER:          boolean read self = MemObjectType.BUFFER;
    public property IS_IMAGE2D:         boolean read self = MemObjectType.IMAGE2D;
    public property IS_IMAGE3D:         boolean read self = MemObjectType.IMAGE3D;
    public property IS_IMAGE2D_ARRAY:   boolean read self = MemObjectType.IMAGE2D_ARRAY;
    public property IS_IMAGE1D:         boolean read self = MemObjectType.IMAGE1D;
    public property IS_IMAGE1D_ARRAY:   boolean read self = MemObjectType.IMAGE1D_ARRAY;
    public property IS_IMAGE1D_BUFFER:  boolean read self = MemObjectType.IMAGE1D_BUFFER;
    public property IS_PIPE:            boolean read self = MemObjectType.PIPE;
    
    public function ToString: string; override;
    begin
      var res := typeof(MemObjectType).GetProperties.Where(prop->prop.PropertyType=typeof(boolean)).Select(prop->(prop.Name,boolean(prop.GetValue(self)))).FirstOrDefault(t->t[1]);
      Result := res=nil?
        $'MemObjectType[{self.val}]':
        res[0].Substring(3);
    end;
    
  end;
  
  //SR
  AddressingMode = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property NONE:            AddressingMode read new AddressingMode($1130);
    public static property CLAMP_TO_EDGE:   AddressingMode read new AddressingMode($1131);
    public static property CLAMP:           AddressingMode read new AddressingMode($1132);
    public static property &REPEAT:         AddressingMode read new AddressingMode($1133);
    public static property MIRRORED_REPEAT: AddressingMode read new AddressingMode($1134);
    
    public property IS_NONE:            boolean read self = AddressingMode.NONE;
    public property IS_CLAMP_TO_EDGE:   boolean read self = AddressingMode.CLAMP_TO_EDGE;
    public property IS_CLAMP:           boolean read self = AddressingMode.CLAMP;
    public property IS_REPEAT:          boolean read self = AddressingMode.REPEAT;
    public property IS_MIRRORED_REPEAT: boolean read self = AddressingMode.MIRRORED_REPEAT;
    
    public function ToString: string; override;
    begin
      var res := typeof(AddressingMode).GetProperties.Where(prop->prop.PropertyType=typeof(boolean)).Select(prop->(prop.Name,boolean(prop.GetValue(self)))).FirstOrDefault(t->t[1]);
      Result := res=nil?
        $'AddressingMode[{self.val}]':
        res[0].Substring(3);
    end;
    
  end;
  
  //SR
  FilterMode = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property NEAREST: FilterMode read new FilterMode($1140);
    public static property LINEAR:  FilterMode read new FilterMode($1141);
    
    public property IS_NEAREST: boolean read self = FilterMode.NEAREST;
    public property IS_LINEAR:  boolean read self = FilterMode.LINEAR;
    
    public function ToString: string; override;
    begin
      var res := typeof(FilterMode).GetProperties.Where(prop->prop.PropertyType=typeof(boolean)).Select(prop->(prop.Name,boolean(prop.GetValue(self)))).FirstOrDefault(t->t[1]);
      Result := res=nil?
        $'FilterMode[{self.val}]':
        res[0].Substring(3);
    end;
    
  end;
  
  //SR
  CommandExecutionStatus = record
    public val: Int32;
    public constructor(val: Int32) := self.val := val;
    
    public static property COMPLETE:  CommandExecutionStatus read new CommandExecutionStatus($0);
    public static property RUNNING:   CommandExecutionStatus read new CommandExecutionStatus($1);
    public static property SUBMITTED: CommandExecutionStatus read new CommandExecutionStatus($2);
    public static property QUEUED:    CommandExecutionStatus read new CommandExecutionStatus($3);
    
    public property IS_COMPLETE:  boolean read self = CommandExecutionStatus.COMPLETE;
    public property IS_RUNNING:   boolean read self = CommandExecutionStatus.RUNNING;
    public property IS_SUBMITTED: boolean read self = CommandExecutionStatus.SUBMITTED;
    public property IS_QUEUED:    boolean read self = CommandExecutionStatus.QUEUED;
    public property IS_ERROR:     boolean read self.val < 0;
    
    public function GetError :=
    new ErrorCode(self.val);
    
    public static function operator implicit(ec: ErrorCode): CommandExecutionStatus :=
    new CommandExecutionStatus(ec.val);
    
    public function ToString: string; override;
    begin
      var res := typeof(CommandExecutionStatus).GetProperties.Where(prop->prop.PropertyType=typeof(boolean)).Select(prop->(prop.Name,boolean(prop.GetValue(self)))).FirstOrDefault(t->t[1]);
      Result :=
        IS_ERROR?
          GetError.ToString:
        res<>nil?
          res[0].Substring(3):
          $'CommandExecutionStatus[{self.val}]';
    end;
    
    public procedure RaiseIfError :=
    if val<0 then ErrorCode.Create(val).RaiseIfError;
    
  end;
  
  //SR
  DeviceAffinityDomain = record
    public val: cl_bitfield;
    public constructor(val: cl_bitfield) := self.val := val;
    
    public static property NUMA:                    DeviceAffinityDomain read new DeviceAffinityDomain(1 shl 0);
    public static property L4_CACHE:                DeviceAffinityDomain read new DeviceAffinityDomain(1 shl 1);
    public static property L3_CACHE:                DeviceAffinityDomain read new DeviceAffinityDomain(1 shl 2);
    public static property L2_CACHE:                DeviceAffinityDomain read new DeviceAffinityDomain(1 shl 3);
    public static property L1_CACHE:                DeviceAffinityDomain read new DeviceAffinityDomain(1 shl 4);
    public static property NEXT_PARTITIONABLE:      DeviceAffinityDomain read new DeviceAffinityDomain(1 shl 5);
    
    // cl_ext
    public static property L1_CACHE_EXT:            DeviceAffinityDomain read new DeviceAffinityDomain(  $1);
    public static property L2_CACHE_EXT:            DeviceAffinityDomain read new DeviceAffinityDomain(  $2);
    public static property L3_CACHE_EXT:            DeviceAffinityDomain read new DeviceAffinityDomain(  $3);
    public static property L4_CACHE_EXT:            DeviceAffinityDomain read new DeviceAffinityDomain(  $4);
    public static property NUMA_EXT:                DeviceAffinityDomain read new DeviceAffinityDomain( $10);
    public static property NEXT_PARTITIONABLE_EXT:  DeviceAffinityDomain read new DeviceAffinityDomain($100);
    
    public property IS_NUMA:                    boolean read self = DeviceAffinityDomain.NUMA;
    public property IS_L4_CACHE:                boolean read self = DeviceAffinityDomain.L4_CACHE;
    public property IS_L3_CACHE:                boolean read self = DeviceAffinityDomain.L3_CACHE;
    public property IS_L2_CACHE:                boolean read self = DeviceAffinityDomain.L2_CACHE;
    public property IS_L1_CACHE:                boolean read self = DeviceAffinityDomain.L1_CACHE;
    public property IS_NEXT_PARTITIONABLE:      boolean read self = DeviceAffinityDomain.NEXT_PARTITIONABLE;
    
    public property IS_L1_CACHE_EXT:            boolean read self = DeviceAffinityDomain.L1_CACHE_EXT;
    public property IS_L2_CACHE_EXT:            boolean read self = DeviceAffinityDomain.L2_CACHE_EXT;
    public property IS_L3_CACHE_EXT:            boolean read self = DeviceAffinityDomain.L3_CACHE_EXT;
    public property IS_L4_CACHE_EXT:            boolean read self = DeviceAffinityDomain.L4_CACHE_EXT;
    public property IS_NUMA_EXT:                boolean read self = DeviceAffinityDomain.NUMA_EXT;
    public property IS_NEXT_PARTITIONABLE_EXT:  boolean read self = DeviceAffinityDomain.NEXT_PARTITIONABLE_EXT;
    
    public function ToString: string; override;
    begin
      var res := typeof(DeviceAffinityDomain).GetProperties.Where(prop->prop.PropertyType=typeof(boolean)).Select(prop->(prop.Name,boolean(prop.GetValue(self)))).FirstOrDefault(t->t[1]);
      Result := res=nil?
        $'DeviceAffinityDomain[{self.val}]':
        res[0].Substring(3);
    end;
    
  end;
  
  //SR
  CommandQueuePriority = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property HIGH:  CommandQueuePriority read new CommandQueuePriority(1 shl 0);
    public static property MED:   CommandQueuePriority read new CommandQueuePriority(1 shl 1);
    public static property LOW:   CommandQueuePriority read new CommandQueuePriority(1 shl 2);
    
    public property IS_HIGH:  boolean read self = CommandQueuePriority.HIGH;
    public property IS_MED:   boolean read self = CommandQueuePriority.MED;
    public property IS_LOW:   boolean read self = CommandQueuePriority.LOW;
    
    public function ToString: string; override;
    begin
      var res := typeof(DeviceAffinityDomain).GetProperties.Where(prop->prop.PropertyType=typeof(boolean)).Select(prop->(prop.Name,boolean(prop.GetValue(self)))).FirstOrDefault(t->t[1]);
      Result := res=nil?
        $'CommandQueuePriority[{self.val}]':
        res[0].Substring(3);
    end;
    
  end;
  
  //SR
  CommandQueueThrottle = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public static property HIGH:  CommandQueueThrottle read new CommandQueueThrottle(1 shl 0);
    public static property MED:   CommandQueueThrottle read new CommandQueueThrottle(1 shl 1);
    public static property LOW:   CommandQueueThrottle read new CommandQueueThrottle(1 shl 2);
    
    public property IS_HIGH:  boolean read self = CommandQueueThrottle.HIGH;
    public property IS_MED:   boolean read self = CommandQueueThrottle.MED;
    public property IS_LOW:   boolean read self = CommandQueueThrottle.LOW;
    
    public function ToString: string; override;
    begin
      var res := typeof(DeviceAffinityDomain).GetProperties.Where(prop->prop.PropertyType=typeof(boolean)).Select(prop->(prop.Name,boolean(prop.GetValue(self)))).FirstOrDefault(t->t[1]);
      Result := res=nil?
        $'CommandQueueThrottle[{self.val}]':
        res[0].Substring(3);
    end;
    
  end;
  
  //R
  DeviceMemCacheType = record
    public val: UInt32;
    
    public property NONE:             boolean read self.val = $0;
    public property READ_ONLY_CACHE:  boolean read self.val = $1;
    public property READ_WRITE_CACHE: boolean read self.val = $2;
    
    public function ToString: string; override;
    begin
      var res := typeof(DeviceMemCacheType).GetProperties.Select(prop->(prop.Name,boolean(prop.GetValue(self)))).FirstOrDefault(t->t[1]);
      Result := res=nil?
        $'DeviceMemCacheType[{self.val}]':
        res[0];
    end;
    
  end;
  
  //R
  DeviceLocalMemType = record
    public val: UInt32;
    
    public property NONE:   boolean read self.val = $0;
    public property LOCAL:  boolean read self.val = $1;
    public property GLOBAL: boolean read self.val = $2;
    
    public function ToString: string; override;
    begin
      var res := typeof(DeviceLocalMemType).GetProperties.Select(prop->(prop.Name,boolean(prop.GetValue(self)))).FirstOrDefault(t->t[1]);
      Result := res=nil?
        $'DeviceLocalMemType[{self.val}]':
        res[0];
    end;
    
  end;
  
  //R
  ProgramBinaryType = record
    public val: UInt32;
    
    public property NONE:             boolean read self.val = $0;
    public property COMPILED_OBJECT:  boolean read self.val = $1;
    public property &LIBRARY:         boolean read self.val = $2;
    public property EXECUTABLE:       boolean read self.val = $4;
    
    public function ToString: string; override;
    begin
      var res := typeof(ProgramBinaryType).GetProperties.Select(prop->(prop.Name,boolean(prop.GetValue(self)))).FirstOrDefault(t->t[1]);
      Result := res=nil?
        $'ProgramBinaryType[{self.val}]':
        res[0];
    end;
    
  end;
  
  //R
  KernelArgAddressQualifier = record
    public val: UInt32;
    
    public property GLOBAL:   boolean read self.val = $119B;
    public property LOCAL:    boolean read self.val = $119C;
    public property CONSTANT: boolean read self.val = $119D;
    public property &PRIVATE: boolean read self.val = $119E;
    
    public function ToString: string; override;
    begin
      var res := typeof(KernelArgAddressQualifier).GetProperties.Select(prop->(prop.Name,boolean(prop.GetValue(self)))).FirstOrDefault(t->t[1]);
      Result := res=nil?
        $'KernelArgAddressQualifier[{self.val}]':
        res[0];
    end;
    
  end;
  
  //R
  KernelArgAccessQualifier = record
    public val: UInt32;
    
    public property READ_ONLY:  boolean read self.val = $11A0;
    public property WRITE_ONLY: boolean read self.val = $11A1;
    public property READ_WRITE: boolean read self.val = $11A2;
    public property NONE:       boolean read self.val = $11A3;
    
    public function ToString: string; override;
    begin
      var res := typeof(KernelArgAccessQualifier).GetProperties.Select(prop->(prop.Name,boolean(prop.GetValue(self)))).FirstOrDefault(t->t[1]);
      Result := res=nil?
        $'KernelArgAccessQualifier[{self.val}]':
        res[0];
    end;
    
  end;
  
  //R
  CommandType = record
    public val: UInt32;
    
    public property NDRANGE_KERNEL:               boolean read self.val = $11F0;
    public property TASK:                         boolean read self.val = $11F1;
    public property NATIVE_KERNEL:                boolean read self.val = $11F2;
    public property READ_BUFFER:                  boolean read self.val = $11F3;
    public property WRITE_BUFFER:                 boolean read self.val = $11F4;
    public property COPY_BUFFER:                  boolean read self.val = $11F5;
    public property READ_IMAGE:                   boolean read self.val = $11F6;
    public property WRITE_IMAGE:                  boolean read self.val = $11F7;
    public property COPY_IMAGE:                   boolean read self.val = $11F8;
    public property COPY_IMAGE_TO_BUFFER:         boolean read self.val = $11F9;
    public property COPY_BUFFER_TO_IMAGE:         boolean read self.val = $11FA;
    public property MAP_BUFFER:                   boolean read self.val = $11FB;
    public property MAP_IMAGE:                    boolean read self.val = $11FC;
    public property UNMAP_MEM_OBJECT:             boolean read self.val = $11FD;
    public property MARKER:                       boolean read self.val = $11FE;
    public property ACQUIRE_GL_OBJECTS:           boolean read self.val = $11FF;
    public property RELEASE_GL_OBJECTS:           boolean read self.val = $1200;
    public property READ_BUFFER_RECT:             boolean read self.val = $1201;
    public property WRITE_BUFFER_RECT:            boolean read self.val = $1202;
    public property COPY_BUFFER_RECT:             boolean read self.val = $1203;
    public property USER:                         boolean read self.val = $1204;
    public property BARRIER:                      boolean read self.val = $1205;
    public property MIGRATE_MEM_OBJECTS:          boolean read self.val = $1206;
    public property FILL_BUFFER:                  boolean read self.val = $1207;
    public property FILL_IMAGE:                   boolean read self.val = $1208;
    public property SVM_FREE:                     boolean read self.val = $1209;
    public property SVM_MEMCPY:                   boolean read self.val = $120A;
    public property SVM_MEMFILL:                  boolean read self.val = $120B;
    public property SVM_MAP:                      boolean read self.val = $120C;
    public property SVM_UNMAP:                    boolean read self.val = $120D;
    
    // cl_ext
    public property ACQUIRE_GRALLOC_OBJECTS_IMG:  boolean read self.val = $40D2;
    public property RELEASE_GRALLOC_OBJECTS_IMG:  boolean read self.val = $40D3;
    public property SVM_FREE_ARM:                 boolean read self.val = $40BA;
    public property SVM_MEMCPY_ARM:               boolean read self.val = $40BB;
    public property SVM_MEMFILL_ARM:              boolean read self.val = $40BC;
    public property SVM_MAP_ARM:                  boolean read self.val = $40BD;
    public property SVM_UNMAP_ARM:                boolean read self.val = $40BE;
    
    // cl_gl_ext
    public property GL_FENCE_SYNC_OBJECT_KHR:     boolean read self.val = $200D;
    
    // cl_egl
    public property EGL_FENCE_SYNC_OBJECT_KHR:    boolean read self.val = $202F;
    public property ACQUIRE_EGL_OBJECTS_KHR:      boolean read self.val = $202D;
    public property RELEASE_EGL_OBJECTS_KHR:      boolean read self.val = $202E;
    
    public function ToString: string; override;
    begin
      var res := typeof(CommandType).GetProperties.Select(prop->(prop.Name,boolean(prop.GetValue(self)))).FirstOrDefault(t->t[1]);
      Result := res=nil?
        $'CommandType[{self.val}]':
        res[0];
    end;
    
  end;
  
  //R
  GLObjectType = record
    public val: UInt32;
    public constructor(val: UInt32) := self.val := val;
    
    public property BUFFER:          boolean read self.val = $2000;
    public property TEXTURE2D:       boolean read self.val = $2001;
    public property TEXTURE3D:       boolean read self.val = $2002;
    public property RENDERBUFFER:    boolean read self.val = $2003;
    public property TEXTURE2D_ARRAY: boolean read self.val = $200E;
    public property TEXTURE1D:       boolean read self.val = $200F;
    public property TEXTURE1D_ARRAY: boolean read self.val = $2010;
    public property TEXTURE_BUFFER:  boolean read self.val = $2011;
    
    public function ToString: string; override;
    begin
      var res := typeof(GLObjectType).GetProperties.Select(prop->(prop.Name,boolean(prop.GetValue(self)))).FirstOrDefault(t->t[1]);
      Result := res=nil?
        $'GLObjectType[{self.val}]':
        res[0];
    end;
    
  end;
  
  {$endregion 1 значение}
  
  {$region Флаги}
  
  //S
  DeviceTypeFlags = record
    public val: cl_bitfield;
    public constructor(val: cl_bitfield) := self.val := val;
    
    public static property &Default:    DeviceTypeFlags read new DeviceTypeFlags(1 shl 0);
    public static property CPU:         DeviceTypeFlags read new DeviceTypeFlags(1 shl 1);
    public static property GPU:         DeviceTypeFlags read new DeviceTypeFlags(1 shl 2);
    public static property Accelerator: DeviceTypeFlags read new DeviceTypeFlags(1 shl 3);
    public static property All:         DeviceTypeFlags read new DeviceTypeFlags($FFFFFFFF);
    
    public static function operator or(a,b: DeviceTypeFlags) :=
    new DeviceTypeFlags(a.val or b.val);
    
  end;
  
  //S
  CommandQueuePropertyFlags = record
    public val: cl_bitfield;
    public constructor(val: cl_bitfield) := self.val := val;
    
    public static property NONE:                                CommandQueuePropertyFlags read new CommandQueuePropertyFlags(0);
    public static property QUEUE_OUT_OF_ORDER_EXEC_MODE_ENABLE: CommandQueuePropertyFlags read new CommandQueuePropertyFlags(1 shl 0);
    public static property QUEUE_PROFILING_ENABLE:              CommandQueuePropertyFlags read new CommandQueuePropertyFlags(1 shl 1);
    
    public static function operator or(a,b: CommandQueuePropertyFlags) :=
    new CommandQueuePropertyFlags(a.val or b.val);
    
    public static function operator implicit(cqpf: CommandQueuePropertyFlags): CommandQueueInfoType :=
    new CommandQueueInfoType(cqpf.val);
    
  end;
  
  //S
  MapFlags = record
    public val: cl_bitfield;
    public constructor(val: cl_bitfield) := self.val := val;
    
    public static property MAP_READ:                MapFlags read new MapFlags(1 shl 0);
    public static property MAP_WRITE:               MapFlags read new MapFlags(1 shl 1);
    public static property WRITE_INVALIDATE_REGION: MapFlags read new MapFlags(1 shl 2);
    
    public static function operator or(a,b: MapFlags) :=
    new MapFlags(a.val or b.val);
    
  end;
  
  //S
  MemMigrationFlags = record
    public val: cl_bitfield;
    public constructor(val: cl_bitfield) := self.val := val;
    
    public static property NONE:              MemMigrationFlags read new MemMigrationFlags(0);
    public static property HOST:              MemMigrationFlags read new MemMigrationFlags(1 shl 0);
    public static property CONTENT_UNDEFINED: MemMigrationFlags read new MemMigrationFlags(1 shl 1);
    
    public static function operator or(a,b: MemMigrationFlags) :=
    new MemMigrationFlags(a.val or b.val);
    
  end;
  
  //SR
  MemoryFlags = record
    public val: cl_bitfield;
    public constructor(val: cl_bitfield) := self.val := val;
    
    
    
    public static property READ_WRITE:                  MemoryFlags read new MemoryFlags(1 shl 00);
    public static property WRITE_ONLY:                  MemoryFlags read new MemoryFlags(1 shl 01);
    public static property READ_ONLY:                   MemoryFlags read new MemoryFlags(1 shl 02);
    public static property USE_HOST_PTR:                MemoryFlags read new MemoryFlags(1 shl 03);
    public static property ALLOC_HOST_PTR:              MemoryFlags read new MemoryFlags(1 shl 04);
    public static property COPY_HOST_PTR:               MemoryFlags read new MemoryFlags(1 shl 05);
    public static property HOST_WRITE_ONLY:             MemoryFlags read new MemoryFlags(1 shl 07);
    public static property HOST_READ_ONLY:              MemoryFlags read new MemoryFlags(1 shl 08);
    public static property HOST_NO_ACCESS:              MemoryFlags read new MemoryFlags(1 shl 09);
    public static property SVM_FINE_GRAIN_BUFFER:       MemoryFlags read new MemoryFlags(1 shl 10);
    public static property SVM_ATOMICS:                 MemoryFlags read new MemoryFlags(1 shl 11);
    public static property KERNEL_READ_AND_WRITE:       MemoryFlags read new MemoryFlags(1 shl 12);
    
    // cl_ext
    public static property USE_UNCACHED_CPU_MEMORY_IMG: MemoryFlags read new MemoryFlags(1 shl 26);
    public static property USE_CACHED_CPU_MEMORY_IMG:   MemoryFlags read new MemoryFlags(1 shl 27);
    public static property CL_MEM_USE_GRALLOC_PTR_IMG:  MemoryFlags read new MemoryFlags(1 shl 28);
    public static property EXT_HOST_PTR_QCOM:           MemoryFlags read new MemoryFlags(1 shl 29);
    
    
    
    public property IS_READ_WRITE:                  boolean read self.val and MemoryFlags.READ_WRITE                  .val <> 0;
    public property IS_WRITE_ONLY:                  boolean read self.val and MemoryFlags.WRITE_ONLY                  .val <> 0;
    public property IS_READ_ONLY:                   boolean read self.val and MemoryFlags.READ_ONLY                   .val <> 0;
    public property IS_USE_HOST_PTR:                boolean read self.val and MemoryFlags.USE_HOST_PTR                .val <> 0;
    public property IS_ALLOC_HOST_PTR:              boolean read self.val and MemoryFlags.ALLOC_HOST_PTR              .val <> 0;
    public property IS_COPY_HOST_PTR:               boolean read self.val and MemoryFlags.COPY_HOST_PTR               .val <> 0;
    public property IS_HOST_WRITE_ONLY:             boolean read self.val and MemoryFlags.HOST_WRITE_ONLY             .val <> 0;
    public property IS_HOST_READ_ONLY:              boolean read self.val and MemoryFlags.HOST_READ_ONLY              .val <> 0;
    public property IS_HOST_NO_ACCESS:              boolean read self.val and MemoryFlags.HOST_NO_ACCESS              .val <> 0;
    public property IS_SVM_FINE_GRAIN_BUFFER:       boolean read self.val and MemoryFlags.SVM_FINE_GRAIN_BUFFER       .val <> 0;
    public property IS_SVM_ATOMICS:                 boolean read self.val and MemoryFlags.SVM_ATOMICS                 .val <> 0;
    public property IS_KERNEL_READ_AND_WRITE:       boolean read self.val and MemoryFlags.KERNEL_READ_AND_WRITE       .val <> 0;
    
    // cl_ext
    public property IS_USE_UNCACHED_CPU_MEMORY_IMG: boolean read self.val and MemoryFlags.USE_UNCACHED_CPU_MEMORY_IMG .val <> 0;
    public property IS_USE_CACHED_CPU_MEMORY_IMG:   boolean read self.val and MemoryFlags.USE_CACHED_CPU_MEMORY_IMG   .val <> 0;
    public property IS_CL_MEM_USE_GRALLOC_PTR_IMG:  boolean read self.val and MemoryFlags.CL_MEM_USE_GRALLOC_PTR_IMG  .val <> 0;
    public property IS_EXT_HOST_PTR_QCOM:           boolean read self.val and MemoryFlags.EXT_HOST_PTR_QCOM           .val <> 0;
    
    
    
    public static function operator or(a,b: MemoryFlags) :=
    new MemoryFlags(a.val or b.val);
    
    public function ToString: string; override;
    begin
      var res := typeof(MemoryFlags).GetProperties.Where(prop->prop.PropertyType=typeof(boolean)).Select(prop->(prop.Name,boolean(prop.GetValue(self)))).Where(t->t[1]).Select(t->t[0]).ToArray;
      Result := res.Length=0?
        $'MemoryFlags[{self.val}]':
        res.Select(pname->pname.Substring(3)).JoinIntoString('+');
    end;
    
  end;
  
  //R
  DeviceFPConfigFlags = record
    public val: cl_bitfield;
    
    public property DENORM:                         boolean read self.val and (1 shl 0) <> 0;
    public property INF_NAN:                        boolean read self.val and (1 shl 1) <> 0;
    public property ROUND_TO_NEAREST:               boolean read self.val and (1 shl 2) <> 0;
    public property ROUND_TO_ZERO:                  boolean read self.val and (1 shl 3) <> 0;
    public property ROUND_TO_INF:                   boolean read self.val and (1 shl 4) <> 0;
    public property FMA:                            boolean read self.val and (1 shl 5) <> 0;
    public property SOFT_FLOAT:                     boolean read self.val and (1 shl 6) <> 0;
    public property CORRECTLY_ROUNDED_DIVIDE_SQRT:  boolean read self.val and (1 shl 7) <> 0;
    
    public function ToString: string; override;
    begin
      var res := typeof(DeviceFPConfigFlags).GetProperties.Select(prop->(prop.Name,boolean(prop.GetValue(self)))).Where(t->t[1]).Select(t->t[0]).ToArray;
      Result := res.Length=0?
        $'DeviceFPConfigFlags[{self.val}]':
        res.JoinIntoString('+');
    end;
    
  end;
  
  //R
  DeviceExecCapabilities = record
    public val: cl_bitfield;
    
    public property KERNEL:         boolean read self.val and (1 shl 0) <> 0;
    public property NATIVE_KERNEL:  boolean read self.val and (1 shl 1) <> 0;
    
    public function ToString: string; override;
    begin
      var res := typeof(DeviceExecCapabilities).GetProperties.Select(prop->(prop.Name,boolean(prop.GetValue(self)))).Where(t->t[1]).Select(t->t[0]).ToArray;
      Result := res.Length=0?
        $'DeviceExecCapabilities[{self.val}]':
        res.JoinIntoString('+');
    end;
    
  end;
  
  //R
  DeviceSVMCapabilityFlags = record
    public val: cl_bitfield;
    
    public property NONE:                 boolean read self.val = 0;
    public property COARSE_GRAIN_BUFFER:  boolean read self.val = (1 shl 0);
    public property FINE_GRAIN_BUFFER:    boolean read self.val = (1 shl 1);
    public property FINE_GRAIN_SYSTEM:    boolean read self.val = (1 shl 2);
    public property ATOMICS:              boolean read self.val = (1 shl 3);
    
    public function ToString: string; override;
    begin
      var res := typeof(DeviceSVMCapabilityFlags).GetProperties.Skip(1).Select(prop->(prop.Name,boolean(prop.GetValue(self)))).Where(t->t[1]).Select(t->t[0]).ToArray;
      Result := res.Length=0?
        val=0?'NONE':$'DeviceSVMCapabilities[{self.val}]':
        res.JoinIntoString('+');
    end;
    
  end;
  
  //R
  KernelArgTypeQualifier = record
    public val: UInt32;
    
    public property NONE:     boolean read self.val = 0;
    public property &CONST:   boolean read self.val and (1 shl 0) <> 0;
    public property RESTRICT: boolean read self.val and (1 shl 1) <> 0;
    public property VOLATILE: boolean read self.val and (1 shl 2) <> 0;
    public property PIPE:     boolean read self.val and (1 shl 3) <> 0;
    
    public function ToString: string; override;
    begin
      var res := typeof(KernelArgTypeQualifier).GetProperties.Skip(1).Select(prop->(prop.Name,boolean(prop.GetValue(self)))).Where(t->t[1]).Select(t->t[0]).ToArray;
      Result := res.Length=0?
        val=0?'NONE':$'KernelArgTypeQualifier[{self.val}]':
        res.JoinIntoString('+');
    end;
    
  end;
  
  {$endregion Флаги}
  
{$endregion Перечисления}

{$region Debug}

{$ifdef DebugMode}

type
  cl_event = record
    public val: IntPtr;
    public constructor := exit;
    public static Zero := new cl_event;
    public static Size := sizeof(cl_event);
    
    private function GetRefCount: IntPtr;
    public property RefCount: IntPtr read GetRefCount;
    
    private function GetState: CommandExecutionStatus;
    public property State: CommandExecutionStatus read GetState;
    
  end;
  
{$endif DebugMode}

{$endregion Debug}

{$region Делегаты}

type
  [UnmanagedFunctionPointer(CallingConvention.StdCall)]
  CreateContext_Callback = procedure(errinfo_text: IntPtr; private_info: pointer; cb: UIntPtr; user_data: pointer);
  
  [UnmanagedFunctionPointer(CallingConvention.StdCall)]
  Program_Callback = procedure(&program: cl_program; user_data: pointer);
  
  [UnmanagedFunctionPointer(CallingConvention.StdCall)]
  EnqueueNativeKernel_Callback = procedure(args: pointer);
  
  [UnmanagedFunctionPointer(CallingConvention.StdCall)]
  MemObjectDestructor_Callback = procedure(mem_obj: cl_mem; user_data: pointer);
  
  [UnmanagedFunctionPointer(CallingConvention.StdCall)]
  EnqueueSVMFree_Callback = procedure(queue: cl_command_queue; num_svm_pointers: UInt32; svm_pointers: ^UIntPtr; user_data: pointer);
  
  [UnmanagedFunctionPointer(CallingConvention.StdCall)]
  Event_Callback = procedure(&event: cl_event; event_command_exec_status: CommandExecutionStatus; user_data: pointer);
  
{$endregion Делегаты}

{$region Записи}

type
  cl_buffer_region = record
    public origin: UIntPtr;
    public size:   UIntPtr;
    
    public constructor(origin, size: UIntPtr);
    begin
      self.origin := origin;
      self.size   := size;
    end;
    
  end;
  
  cl_image_format = record
    public image_channel_order:      ChannelOrder;
    public image_channel_data_type:  ChannelType;
    
    public constructor(image_channel_order: ChannelOrder; image_channel_data_type: ChannelType);
    begin
      self.image_channel_order := image_channel_order;
      self.image_channel_data_type := image_channel_data_type;
    end;
    
  end;
  
  cl_image_desc = record
    public image_type: MemObjectType;
    public image_width, image_height, image_depth, image_array_size, image_row_pitch, image_slice_pitch: UIntPtr;
    public num_mip_levels, num_samples: UInt32;
    public mem_object: cl_mem;
    
    public static function Create1D(image_width: UIntPtr): cl_image_desc;
    begin
      Result.image_type := MemObjectType.IMAGE1D;
      Result.image_width := image_width;
    end;
    
    public static function Create1D_BUFFER(image_width: UIntPtr; mem_object: cl_mem): cl_image_desc;
    begin
      Result.image_type := MemObjectType.IMAGE1D_BUFFER;
      Result.image_width := image_width;
      Result.mem_object := mem_object;
    end;
    
    public static function Create1D_ARRAY(image_width, image_array_size, image_row_pitch: UIntPtr): cl_image_desc;
    begin
      Result.image_type := MemObjectType.IMAGE1D_ARRAY;
      Result.image_width := image_width;
      Result.image_array_size := image_array_size;
      Result.image_row_pitch := image_row_pitch;
    end;
    
    public static function Create2D(image_width, image_height, image_row_pitch: UIntPtr; mem_object: cl_mem): cl_image_desc;
    begin
      Result.image_type := MemObjectType.IMAGE2D;
      Result.image_width := image_width;
      Result.image_height := image_height;
      Result.image_row_pitch := image_row_pitch;
      Result.mem_object := mem_object;
    end;
    
    public static function Create2D_ARRAY(image_width, image_height, image_array_size, image_row_pitch, image_slice_pitch: UIntPtr): cl_image_desc;
    begin
      Result.image_type := MemObjectType.IMAGE2D_ARRAY;
      Result.image_width := image_width;
      Result.image_height := image_height;
      Result.image_array_size := image_array_size;
      Result.image_row_pitch := image_row_pitch;
      Result.image_slice_pitch := image_slice_pitch;
    end;
    
    public static function Create3D(image_width, image_height, image_depth, image_row_pitch, image_slice_pitch: UIntPtr): cl_image_desc;
    begin
      Result.image_type := MemObjectType.IMAGE3D;
      Result.image_width := image_width;
      Result.image_height := image_height;
      Result.image_depth := image_depth;
      Result.image_row_pitch := image_row_pitch;
      Result.image_slice_pitch := image_slice_pitch;
    end;
    
  end;

{$endregion Записи}

type
  cl = static class
    
    {$region Platform}
    
    static function GetPlatformIDs(num_entries: UInt32; [MarshalAs(UnmanagedType.LPArray)] platforms: array of cl_platform_id; var num_platforms: UInt32): ErrorCode;
    external 'opencl.dll' name 'clGetPlatformIDs';
    static function GetPlatformIDs(num_entries: UInt32; platforms: ^cl_platform_id; num_platforms: ^UInt32): ErrorCode;
    external 'opencl.dll' name 'clGetPlatformIDs';
    
    static function GetPlatformInfo(platform: cl_platform_id; param_name: PlatformInfoType; param_value_size: UIntPtr; param_value: pointer; var param_value_size_ret: UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetPlatformInfo';
    static function GetPlatformInfo(platform: cl_platform_id; param_name: PlatformInfoType; param_value_size: UIntPtr; param_value: pointer; param_value_size_ret: ^UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetPlatformInfo';
    
    static function UnloadPlatformCompiler(platform: cl_platform_id): ErrorCode;
    external 'opencl.dll' name 'clUnloadPlatformCompiler';
    
    {$endregion Platform}
    
    {$region Device}
    
    static function GetDeviceIDs(platform: cl_platform_id; device_type: DeviceTypeFlags; num_entries: UInt32; [MarshalAs(UnmanagedType.LPArray)] devices: array of cl_device_id; var num_devices: UInt32): ErrorCode;
    external 'opencl.dll' name 'clGetDeviceIDs';
    static function GetDeviceIDs(platform: cl_platform_id; device_type: DeviceTypeFlags; num_entries: UInt32; devices: ^cl_device_id; num_devices: ^UInt32): ErrorCode;
    external 'opencl.dll' name 'clGetDeviceIDs';
    
    static function GetDeviceInfo(device: cl_device_id; param_name: DeviceInfoType; param_value_size: UIntPtr; param_value: pointer; var param_value_size_ret: UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetDeviceInfo';
    static function GetDeviceInfo(device: cl_device_id; param_name: DeviceInfoType; param_value_size: UIntPtr; param_value: pointer; param_value_size_ret: ^UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetDeviceInfo';
    
    static function GetDeviceAndHostTimer(device: cl_device_id; var device_timestamp: uint64; var host_timestamp: uint64): ErrorCode;
    external 'opencl.dll' name 'clGetDeviceAndHostTimer';
    static function GetDeviceAndHostTimer(device: cl_device_id; device_timestamp, host_timestamp: ^uint64): ErrorCode;
    external 'opencl.dll' name 'clGetDeviceAndHostTimer';
    
    static function GetHostTimer(device: cl_device_id; var host_timestamp: uint64): ErrorCode;
    external 'opencl.dll' name 'clGetHostTimer';
    static function GetHostTimer(device: cl_device_id; host_timestamp: ^uint64): ErrorCode;
    external 'opencl.dll' name 'clGetHostTimer';
    
    static function CreateSubDevices(in_device: cl_device_id; [MarshalAs(UnmanagedType.LPArray)] properties: array of DevicePartitionProperties; num_devices: UInt32; [MarshalAs(UnmanagedType.LPArray)] out_devices: array of cl_device_id; var num_devices_ret: UInt32): ErrorCode;
    external 'opencl.dll' name 'clCreateSubDevices';
    static function CreateSubDevices(in_device: cl_device_id; properties: ^DevicePartitionProperties; num_devices: UInt32; out_devices: ^cl_device_id; num_devices_ret: ^UInt32): ErrorCode;
    external 'opencl.dll' name 'clCreateSubDevices';
    
    static function RetainDevice(device: cl_device_id): ErrorCode;
    external 'opencl.dll' name 'clRetainDevice';
    
    static function ReleaseDevice(device: cl_device_id): ErrorCode;
    external 'opencl.dll' name 'clReleaseDevice';
    
    {$endregion Device}
    
    {$region Context}
    
    static function CreateContext([MarshalAs(UnmanagedType.LPArray)] properties: array of ContextProperties; num_devices: UInt32; [MarshalAs(UnmanagedType.LPArray)] devices: array of cl_device_id; pfn_notify: CreateContext_Callback; user_data: IntPtr; var ec: ErrorCode): cl_context;
    external 'opencl.dll' name 'clCreateContext';
    static function CreateContext(properties: ^ContextProperties; num_devices: UInt32; devices: ^cl_device_id; pfn_notify: CreateContext_Callback; user_data: pointer; ec: ^ErrorCode): cl_context;
    external 'opencl.dll' name 'clCreateContext';
    
    static function CreateContextFromType([MarshalAs(UnmanagedType.LPArray)] properties: array of ContextProperties; device_type: DeviceTypeFlags; pfn_notify: CreateContext_Callback; user_data: IntPtr; var ec: ErrorCode): cl_context;
    external 'opencl.dll' name 'clCreateContext';
    static function CreateContextFromType(properties: ^ContextProperties; device_type: DeviceTypeFlags; pfn_notify: CreateContext_Callback; user_data: pointer; ec: ^ErrorCode): cl_context;
    external 'opencl.dll' name 'clCreateContext';
    
    static function RetainContext(context: cl_context): ErrorCode;
    external 'opencl.dll' name 'clRetainContext';
    
    static function ReleaseContext(context: cl_context): ErrorCode;
    external 'opencl.dll' name 'clReleaseContext';
    
    static function GetContextInfo(context: cl_context; param_name: ContextInfoType; param_value_size: UIntPtr; param_value: pointer; var param_value_size_ret: UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetContextInfo';
    static function GetContextInfo(context: cl_context; param_name: ContextInfoType; param_value_size: UIntPtr; param_value: pointer; param_value_size_ret: ^UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetContextInfo';
    
    {$endregion Context}
    
    {$region CommandQueue}
    
    static function CreateCommandQueueWithProperties(context: cl_context; device: cl_device_id; [MarshalAs(UnmanagedType.LPArray)] properties: array of CommandQueueInfoType; var errcode_ret: ErrorCode): cl_command_queue;
    external 'opencl.dll' name 'clCreateCommandQueueWithProperties';
    static function CreateCommandQueueWithProperties(context: cl_context; device: cl_device_id; properties: ^CommandQueueInfoType; errcode_ret: ^ErrorCode): cl_command_queue;
    external 'opencl.dll' name 'clCreateCommandQueueWithProperties';
    
    /// Эта функция устарела
    static function CreateCommandQueue(context: cl_context; device: cl_device_id; properties: CommandQueuePropertyFlags; var errcode_ret: ErrorCode): cl_command_queue;
    external 'opencl.dll' name 'clCreateCommandQueue';
    /// Эта функция устарела
    static function CreateCommandQueue(context: cl_context; device: cl_device_id; properties: CommandQueuePropertyFlags; errcode_ret: ^ErrorCode): cl_command_queue;
    external 'opencl.dll' name 'clCreateCommandQueue';
    
    static function SetDefaultDeviceCommandQueue(context: cl_context; device: cl_device_id; command_queue: cl_command_queue): ErrorCode;
    external 'opencl.dll' name 'clSetDefaultDeviceCommandQueue';
    
    static function RetainCommandQueue(command_queue: cl_command_queue): ErrorCode;
    external 'opencl.dll' name 'clRetainCommandQueue';
    
    static function ReleaseCommandQueue(command_queue: cl_command_queue): ErrorCode;
    external 'opencl.dll' name 'clReleaseCommandQueue';
    
    static function GetCommandQueueInfo(command_queue: cl_command_queue; param_name: CommandQueueInfoType; param_value_size: UIntPtr; param_value: pointer; var param_value_size_ret: UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetCommandQueueInfo';
    static function GetCommandQueueInfo(command_queue: cl_command_queue; param_name: CommandQueueInfoType; param_value_size: UIntPtr; param_value: pointer; param_value_size_ret: ^UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetCommandQueueInfo';
    
    static function Flush(command_queue: cl_command_queue): ErrorCode;
    external 'opencl.dll' name 'clFlush';
    
    static function Finish(command_queue: cl_command_queue): ErrorCode;
    external 'opencl.dll' name 'clFinish';
    
    {$endregion CommandQueue}
    
    {$region cl_mem}
    
    {$region Buffer}
    
    static function CreateBuffer(context: cl_context; flags: MemoryFlags; size: UIntPtr; host_ptr: IntPtr; var errcode_ret: ErrorCode): cl_mem;
    external 'opencl.dll' name 'clCreateBuffer';
    static function CreateBuffer(context: cl_context; flags: MemoryFlags; size: UIntPtr; host_ptr: pointer; errcode_ret: ^ErrorCode): cl_mem;
    external 'opencl.dll' name 'clCreateBuffer';
    
    static function CreateSubBuffer(buffer: cl_mem; flags: MemoryFlags; buffer_create_type: BufferCreateType; buffer_create_info: pointer; var errcode_ret: ErrorCode): cl_mem;
    external 'opencl.dll' name 'clCreateSubBuffer';
    static function CreateSubBuffer(buffer: cl_mem; flags: MemoryFlags; buffer_create_type: BufferCreateType; buffer_create_info: pointer; errcode_ret: ^ErrorCode): cl_mem;
    external 'opencl.dll' name 'clCreateSubBuffer';
    
    static function EnqueueReadBuffer(command_queue: cl_command_queue; buffer: cl_mem; blocking_read: cl_bool; offset: UIntPtr; size: UIntPtr; ptr: IntPtr; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueReadBuffer';
    static function EnqueueReadBuffer(command_queue: cl_command_queue; buffer: cl_mem; blocking_read: cl_bool; offset: UIntPtr; size: UIntPtr; ptr: IntPtr; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueReadBuffer';
    static function EnqueueReadBuffer(command_queue: cl_command_queue; buffer: cl_mem; blocking_read: cl_bool; offset: UIntPtr; size: UIntPtr; [MarshalAs(UnmanagedType.LPArray)] data: &Array; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueReadBuffer';
    static function EnqueueReadBuffer(command_queue: cl_command_queue; buffer: cl_mem; blocking_read: cl_bool; offset: UIntPtr; size: UIntPtr; [MarshalAs(UnmanagedType.LPArray)] data: &Array; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueReadBuffer';
    
    static function EnqueueWriteBuffer(command_queue: cl_command_queue; buffer: cl_mem; blocking_write: cl_bool; offset: UIntPtr; size: UIntPtr; ptr: IntPtr; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueWriteBuffer';
    static function EnqueueWriteBuffer(command_queue: cl_command_queue; buffer: cl_mem; blocking_write: cl_bool; offset: UIntPtr; size: UIntPtr; ptr: IntPtr; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueWriteBuffer';
    static function EnqueueWriteBuffer(command_queue: cl_command_queue; buffer: cl_mem; blocking_write: cl_bool; offset: UIntPtr; size: UIntPtr; [MarshalAs(UnmanagedType.LPArray)] data: &Array; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueWriteBuffer';
    static function EnqueueWriteBuffer(command_queue: cl_command_queue; buffer: cl_mem; blocking_write: cl_bool; offset: UIntPtr; size: UIntPtr; [MarshalAs(UnmanagedType.LPArray)] data: &Array; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueWriteBuffer';
    
    static function EnqueueCopyBuffer(command_queue: cl_command_queue; src_buffer: cl_mem; dst_buffer: cl_mem; src_offset: UIntPtr; dst_offset: UIntPtr; size: UIntPtr; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueCopyBuffer';
    static function EnqueueCopyBuffer(command_queue: cl_command_queue; src_buffer: cl_mem; dst_buffer: cl_mem; src_offset: UIntPtr; dst_offset: UIntPtr; size: UIntPtr; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueCopyBuffer';
    
    static function EnqueueReadBufferRect(command_queue: cl_command_queue; buffer: cl_mem; blocking_read: cl_bool; [MarshalAs(UnmanagedType.LPArray)] buffer_offset: array of UIntPtr; [MarshalAs(UnmanagedType.LPArray)] host_offset: array of UIntPtr; [MarshalAs(UnmanagedType.LPArray)] region: array of UIntPtr; buffer_row_pitch: UIntPtr; buffer_slice_pitch: UIntPtr; host_row_pitch: UIntPtr; host_slice_pitch: UIntPtr; ptr: IntPtr; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueReadBufferRect';
    static function EnqueueReadBufferRect(command_queue: cl_command_queue; buffer: cl_mem; blocking_read: cl_bool; [MarshalAs(UnmanagedType.LPArray)] buffer_offset: array of UIntPtr; [MarshalAs(UnmanagedType.LPArray)] host_offset: array of UIntPtr; [MarshalAs(UnmanagedType.LPArray)] region: array of UIntPtr; buffer_row_pitch: UIntPtr; buffer_slice_pitch: UIntPtr; host_row_pitch: UIntPtr; host_slice_pitch: UIntPtr; ptr: IntPtr; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueReadBufferRect';
    static function EnqueueReadBufferRect(command_queue: cl_command_queue; buffer: cl_mem; blocking_read: cl_bool; buffer_offset: ^UIntPtr; host_offset: ^UIntPtr; region: ^UIntPtr; buffer_row_pitch: UIntPtr; buffer_slice_pitch: UIntPtr; host_row_pitch: UIntPtr; host_slice_pitch: UIntPtr; ptr: pointer; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueReadBufferRect';
    static function EnqueueReadBufferRect(command_queue: cl_command_queue; buffer: cl_mem; blocking_read: cl_bool; buffer_offset: ^UIntPtr; host_offset: ^UIntPtr; region: ^UIntPtr; buffer_row_pitch: UIntPtr; buffer_slice_pitch: UIntPtr; host_row_pitch: UIntPtr; host_slice_pitch: UIntPtr; ptr: pointer; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueReadBufferRect';
    
    static function EnqueueWriteBufferRect(command_queue: cl_command_queue; buffer: cl_mem; blocking_write: cl_bool; [MarshalAs(UnmanagedType.LPArray)] buffer_offset: array of UIntPtr; [MarshalAs(UnmanagedType.LPArray)] host_offset: array of UIntPtr; [MarshalAs(UnmanagedType.LPArray)] region: array of UIntPtr; buffer_row_pitch: UIntPtr; buffer_slice_pitch: UIntPtr; host_row_pitch: UIntPtr; host_slice_pitch: UIntPtr; ptr: IntPtr; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueWriteBufferRect';
    static function EnqueueWriteBufferRect(command_queue: cl_command_queue; buffer: cl_mem; blocking_write: cl_bool; [MarshalAs(UnmanagedType.LPArray)] buffer_offset: array of UIntPtr; [MarshalAs(UnmanagedType.LPArray)] host_offset: array of UIntPtr; [MarshalAs(UnmanagedType.LPArray)] region: array of UIntPtr; buffer_row_pitch: UIntPtr; buffer_slice_pitch: UIntPtr; host_row_pitch: UIntPtr; host_slice_pitch: UIntPtr; ptr: IntPtr; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueWriteBufferRect';
    static function EnqueueWriteBufferRect(command_queue: cl_command_queue; buffer: cl_mem; blocking_write: cl_bool; buffer_offset: ^UIntPtr; host_offset: ^UIntPtr; region: ^UIntPtr; buffer_row_pitch: UIntPtr; buffer_slice_pitch: UIntPtr; host_row_pitch: UIntPtr; host_slice_pitch: UIntPtr; ptr: pointer; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueWriteBufferRect';
    static function EnqueueWriteBufferRect(command_queue: cl_command_queue; buffer: cl_mem; blocking_write: cl_bool; buffer_offset: ^UIntPtr; host_offset: ^UIntPtr; region: ^UIntPtr; buffer_row_pitch: UIntPtr; buffer_slice_pitch: UIntPtr; host_row_pitch: UIntPtr; host_slice_pitch: UIntPtr; ptr: pointer; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueWriteBufferRect';
    
    static function EnqueueCopyBufferRect(command_queue: cl_command_queue; src_buffer: cl_mem; dst_buffer: cl_mem; [MarshalAs(UnmanagedType.LPArray)] src_origin: array of UIntPtr; [MarshalAs(UnmanagedType.LPArray)] dst_origin: array of UIntPtr; [MarshalAs(UnmanagedType.LPArray)] region: array of UIntPtr; src_row_pitch: UIntPtr; src_slice_pitch: UIntPtr; dst_row_pitch: UIntPtr; dst_slice_pitch: UIntPtr; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueCopyBufferRect';
    static function EnqueueCopyBufferRect(command_queue: cl_command_queue; src_buffer: cl_mem; dst_buffer: cl_mem; [MarshalAs(UnmanagedType.LPArray)] src_origin: array of UIntPtr; [MarshalAs(UnmanagedType.LPArray)] dst_origin: array of UIntPtr; [MarshalAs(UnmanagedType.LPArray)] region: array of UIntPtr; src_row_pitch: UIntPtr; src_slice_pitch: UIntPtr; dst_row_pitch: UIntPtr; dst_slice_pitch: UIntPtr; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueCopyBufferRect';
    static function EnqueueCopyBufferRect(command_queue: cl_command_queue; src_buffer: cl_mem; dst_buffer: cl_mem; src_origin: ^UIntPtr; dst_origin: ^UIntPtr; region: ^UIntPtr; src_row_pitch: UIntPtr; src_slice_pitch: UIntPtr; dst_row_pitch: UIntPtr; dst_slice_pitch: UIntPtr; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueCopyBufferRect';
    static function EnqueueCopyBufferRect(command_queue: cl_command_queue; src_buffer: cl_mem; dst_buffer: cl_mem; src_origin: ^UIntPtr; dst_origin: ^UIntPtr; region: ^UIntPtr; src_row_pitch: UIntPtr; src_slice_pitch: UIntPtr; dst_row_pitch: UIntPtr; dst_slice_pitch: UIntPtr; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueCopyBufferRect';
    
    static function EnqueueFillBuffer(command_queue: cl_command_queue; buffer: cl_mem; [MarshalAs(UnmanagedType.LPArray)] pattern: &Array; pattern_size: UIntPtr; offset: UIntPtr; size: UIntPtr; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueFillBuffer';
    static function EnqueueFillBuffer(command_queue: cl_command_queue; buffer: cl_mem; [MarshalAs(UnmanagedType.LPArray)] pattern: &Array; pattern_size: UIntPtr; offset: UIntPtr; size: UIntPtr; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueFillBuffer';
    static function EnqueueFillBuffer(command_queue: cl_command_queue; buffer: cl_mem; pattern: IntPtr; pattern_size: UIntPtr; offset: UIntPtr; size: UIntPtr; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueFillBuffer';
    static function EnqueueFillBuffer(command_queue: cl_command_queue; buffer: cl_mem; pattern: IntPtr; pattern_size: UIntPtr; offset: UIntPtr; size: UIntPtr; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueFillBuffer';
    static function EnqueueFillBuffer(command_queue: cl_command_queue; buffer: cl_mem; pattern: pointer; pattern_size: UIntPtr; offset: UIntPtr; size: UIntPtr; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueFillBuffer';
    static function EnqueueFillBuffer(command_queue: cl_command_queue; buffer: cl_mem; pattern: pointer; pattern_size: UIntPtr; offset: UIntPtr; size: UIntPtr; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueFillBuffer';
    
    static function EnqueueMapBuffer(command_queue: cl_command_queue; buffer: cl_mem; blocking_map: cl_bool; map_flags: MapFlags; offset: UIntPtr; size: UIntPtr; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event; var errcode_ret: ErrorCode): IntPtr;
    external 'opencl.dll' name 'clEnqueueMapBuffer';
    static function EnqueueMapBuffer(command_queue: cl_command_queue; buffer: cl_mem; blocking_map: cl_bool; map_flags: MapFlags; offset: UIntPtr; size: UIntPtr; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event; errcode_ret: ^ErrorCode): IntPtr;
    external 'opencl.dll' name 'clEnqueueMapBuffer';
    
    {$endregion Buffer}
    
    {$region Image}
    
    static function CreateImage(context: cl_context; flags: MemoryFlags; image_format: ^cl_image_format; image_desc: ^cl_image_desc; host_ptr: IntPtr; var errcode_ret: ErrorCode): cl_mem;
    external 'opencl.dll' name 'clCreateImage';
    static function CreateImage(context: cl_context; flags: MemoryFlags; image_format: ^cl_image_format; image_desc: ^cl_image_desc; host_ptr: pointer; errcode_ret: ^ErrorCode): cl_mem;
    external 'opencl.dll' name 'clCreateImage';
    
    static function GetSupportedImageFormats(context: cl_context; flags: MemoryFlags; image_type: MemObjectType; num_entries: UInt32; [MarshalAs(UnmanagedType.LPArray)] image_formats: array of cl_image_format; var num_image_formats: UInt32): ErrorCode;
    external 'opencl.dll' name 'clGetSupportedImageFormats';
    static function GetSupportedImageFormats(context: cl_context; flags: MemoryFlags; image_type: MemObjectType; num_entries: UInt32; image_formats: ^cl_image_format; num_image_formats: ^UInt32): ErrorCode;
    external 'opencl.dll' name 'clGetSupportedImageFormats';
    
    static function EnqueueReadImage(command_queue: cl_command_queue; image: cl_mem; blocking_read: cl_bool; [MarshalAs(UnmanagedType.LPArray)] origin: array of UIntPtr; [MarshalAs(UnmanagedType.LPArray)] region: array of UIntPtr; row_pitch: UIntPtr; slice_pitch: UIntPtr; ptr: pointer; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueReadImage';
    static function EnqueueReadImage(command_queue: cl_command_queue; image: cl_mem; blocking_read: cl_bool; [MarshalAs(UnmanagedType.LPArray)] origin: array of UIntPtr; [MarshalAs(UnmanagedType.LPArray)] region: array of UIntPtr; row_pitch: UIntPtr; slice_pitch: UIntPtr; ptr: pointer; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueReadImage';
    static function EnqueueReadImage(command_queue: cl_command_queue; image: cl_mem; blocking_read: cl_bool; origin: ^UIntPtr; region: ^UIntPtr; row_pitch: UIntPtr; slice_pitch: UIntPtr; ptr: pointer; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueReadImage';
    static function EnqueueReadImage(command_queue: cl_command_queue; image: cl_mem; blocking_read: cl_bool; origin: ^UIntPtr; region: ^UIntPtr; row_pitch: UIntPtr; slice_pitch: UIntPtr; ptr: pointer; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueReadImage';
    
    static function EnqueueWriteImage(command_queue: cl_command_queue; image: cl_mem; blocking_write: cl_bool; [MarshalAs(UnmanagedType.LPArray)] origin: array of UIntPtr; [MarshalAs(UnmanagedType.LPArray)] region: array of UIntPtr; input_row_pitch: UIntPtr; input_slice_pitch: UIntPtr; ptr: pointer; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueWriteImage';
    static function EnqueueWriteImage(command_queue: cl_command_queue; image: cl_mem; blocking_write: cl_bool; [MarshalAs(UnmanagedType.LPArray)] origin: array of UIntPtr; [MarshalAs(UnmanagedType.LPArray)] region: array of UIntPtr; input_row_pitch: UIntPtr; input_slice_pitch: UIntPtr; ptr: pointer; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueWriteImage';
    static function EnqueueWriteImage(command_queue: cl_command_queue; image: cl_mem; blocking_write: cl_bool; origin: ^UIntPtr; region: ^UIntPtr; input_row_pitch: UIntPtr; input_slice_pitch: UIntPtr; ptr: pointer; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueWriteImage';
    static function EnqueueWriteImage(command_queue: cl_command_queue; image: cl_mem; blocking_write: cl_bool; origin: ^UIntPtr; region: ^UIntPtr; input_row_pitch: UIntPtr; input_slice_pitch: UIntPtr; ptr: pointer; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueWriteImage';
    
    static function EnqueueCopyImage(command_queue: cl_command_queue; src_image: cl_mem; dst_image: cl_mem; [MarshalAs(UnmanagedType.LPArray)] src_origin: array of UIntPtr; [MarshalAs(UnmanagedType.LPArray)] region: array of UIntPtr; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueCopyImage';
    static function EnqueueCopyImage(command_queue: cl_command_queue; src_image: cl_mem; dst_image: cl_mem; [MarshalAs(UnmanagedType.LPArray)] src_origin: array of UIntPtr; [MarshalAs(UnmanagedType.LPArray)] region: array of UIntPtr; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueCopyImage';
    static function EnqueueCopyImage(command_queue: cl_command_queue; src_image: cl_mem; dst_image: cl_mem; src_origin: ^UIntPtr; dst_origin: ^UIntPtr; region: ^UIntPtr; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueCopyImage';
    static function EnqueueCopyImage(command_queue: cl_command_queue; src_image: cl_mem; dst_image: cl_mem; src_origin: ^UIntPtr; dst_origin: ^UIntPtr; region: ^UIntPtr; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueCopyImage';
    
    static function EnqueueFillImage(command_queue: cl_command_queue; image: cl_mem; fill_color: IntPtr; [MarshalAs(UnmanagedType.LPArray)] origin: array of UIntPtr; [MarshalAs(UnmanagedType.LPArray)] region: array of UIntPtr; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): System.Int32;
    external 'opencl.dll' name 'clEnqueueFillImage';
    static function EnqueueFillImage(command_queue: cl_command_queue; image: cl_mem; fill_color: IntPtr; [MarshalAs(UnmanagedType.LPArray)] origin: array of UIntPtr; [MarshalAs(UnmanagedType.LPArray)] region: array of UIntPtr; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): System.Int32;
    external 'opencl.dll' name 'clEnqueueFillImage';
    static function EnqueueFillImage(command_queue: cl_command_queue; image: cl_mem; fill_color: pointer; origin: ^UIntPtr; region: ^UIntPtr; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): System.Int32;
    external 'opencl.dll' name 'clEnqueueFillImage';
    static function EnqueueFillImage(command_queue: cl_command_queue; image: cl_mem; fill_color: pointer; origin: ^UIntPtr; region: ^UIntPtr; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): System.Int32;
    external 'opencl.dll' name 'clEnqueueFillImage';
    
    static function EnqueueMapImage(command_queue: cl_command_queue; image: cl_mem; blocking_map: cl_bool; map_flags: MapFlags; [MarshalAs(UnmanagedType.LPArray)] origin: array of UIntPtr; [MarshalAs(UnmanagedType.LPArray)] region: array of UIntPtr; var image_row_pitch: UIntPtr; var image_slice_pitch: UIntPtr; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event; var errcode_ret: ErrorCode): pointer;
    external 'opencl.dll' name 'clEnqueueMapImage';
    static function EnqueueMapImage(command_queue: cl_command_queue; image: cl_mem; blocking_map: cl_bool; map_flags: MapFlags; [MarshalAs(UnmanagedType.LPArray)] origin: array of UIntPtr; [MarshalAs(UnmanagedType.LPArray)] region: array of UIntPtr; var image_row_pitch: UIntPtr; var image_slice_pitch: UIntPtr; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event; var errcode_ret: ErrorCode): pointer;
    external 'opencl.dll' name 'clEnqueueMapImage';
    static function EnqueueMapImage(command_queue: cl_command_queue; image: cl_mem; blocking_map: cl_bool; map_flags: MapFlags; origin: ^UIntPtr; region: ^UIntPtr; image_row_pitch: ^UIntPtr; image_slice_pitch: ^UIntPtr; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event; errcode_ret: ^ErrorCode): pointer;
    external 'opencl.dll' name 'clEnqueueMapImage';
    static function EnqueueMapImage(command_queue: cl_command_queue; image: cl_mem; blocking_map: cl_bool; map_flags: MapFlags; origin: ^UIntPtr; region: ^UIntPtr; image_row_pitch: ^UIntPtr; image_slice_pitch: ^UIntPtr; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event; errcode_ret: ^ErrorCode): pointer;
    external 'opencl.dll' name 'clEnqueueMapImage';
    
    static function GetImageInfo(image: cl_mem; param_name: ImageInfoType; param_value_size: UIntPtr; param_value: pointer; var param_value_size_ret: UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetImageInfo';
    static function GetImageInfo(image: cl_mem; param_name: ImageInfoType; param_value_size: UIntPtr; param_value: pointer; param_value_size_ret: ^UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetImageInfo';
    
    {$endregion Image}
    
    {$region Pipe}
    
    static function CreatePipe(context: cl_context; flags: MemoryFlags; pipe_packet_size: UInt32; pipe_max_packets: UInt32; [MarshalAs(UnmanagedType.LPArray)] properties: array of PipeInfoType; var errcode_ret: ErrorCode): cl_mem;
    external 'opencl.dll' name 'clCreatePipe';
    static function CreatePipe(context: cl_context; flags: MemoryFlags; pipe_packet_size: UInt32; pipe_max_packets: UInt32; properties: ^PipeInfoType; errcode_ret: ^ErrorCode): cl_mem;
    external 'opencl.dll' name 'clCreatePipe';
    
    static function GetPipeInfo(pipe: cl_mem; param_name: PipeInfoType; param_value_size: UIntPtr; param_value: pointer; var param_value_size_ret: UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetPipeInfo';
    static function GetPipeInfo(pipe: cl_mem; param_name: PipeInfoType; param_value_size: UIntPtr; param_value: pointer; param_value_size_ret: ^UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetPipeInfo';
    
    {$endregion Pipe}
    
    {$region Общее}
    
    static function EnqueueCopyImageToBuffer(command_queue: cl_command_queue; src_image: cl_mem; dst_buffer: cl_mem; [MarshalAs(UnmanagedType.LPArray)] src_origin: array of UIntPtr; [MarshalAs(UnmanagedType.LPArray)] region: array of UIntPtr; dst_offset: UIntPtr; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueCopyImageToBuffer';
    static function EnqueueCopyImageToBuffer(command_queue: cl_command_queue; src_image: cl_mem; dst_buffer: cl_mem; [MarshalAs(UnmanagedType.LPArray)] src_origin: array of UIntPtr; [MarshalAs(UnmanagedType.LPArray)] region: array of UIntPtr; dst_offset: UIntPtr; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueCopyImageToBuffer';
    static function EnqueueCopyImageToBuffer(command_queue: cl_command_queue; src_image: cl_mem; dst_buffer: cl_mem; src_origin: ^UIntPtr; region: ^UIntPtr; dst_offset: UIntPtr; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueCopyImageToBuffer';
    static function EnqueueCopyImageToBuffer(command_queue: cl_command_queue; src_image: cl_mem; dst_buffer: cl_mem; src_origin: ^UIntPtr; region: ^UIntPtr; dst_offset: UIntPtr; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueCopyImageToBuffer';
    
    static function EnqueueCopyBufferToImage(command_queue: cl_command_queue; src_buffer: cl_mem; dst_image: cl_mem; src_offset: UIntPtr; [MarshalAs(UnmanagedType.LPArray)] dst_origin: array of UIntPtr; [MarshalAs(UnmanagedType.LPArray)] region: array of UIntPtr; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueCopyBufferToImage';
    static function EnqueueCopyBufferToImage(command_queue: cl_command_queue; src_buffer: cl_mem; dst_image: cl_mem; src_offset: UIntPtr; [MarshalAs(UnmanagedType.LPArray)] dst_origin: array of UIntPtr; [MarshalAs(UnmanagedType.LPArray)] region: array of UIntPtr; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueCopyBufferToImage';
    static function EnqueueCopyBufferToImage(command_queue: cl_command_queue; src_buffer: cl_mem; dst_image: cl_mem; src_offset: UIntPtr; dst_origin: ^UIntPtr; region: ^UIntPtr; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueCopyBufferToImage';
    static function EnqueueCopyBufferToImage(command_queue: cl_command_queue; src_buffer: cl_mem; dst_image: cl_mem; src_offset: UIntPtr; dst_origin: ^UIntPtr; region: ^UIntPtr; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueCopyBufferToImage';
    
    static function RetainMemObject(memobj: cl_mem): ErrorCode;
    external 'opencl.dll' name 'clRetainMemObject';
    
    static function ReleaseMemObject(memobj: cl_mem): ErrorCode;
    external 'opencl.dll' name 'clReleaseMemObject';
    
    static function SetMemObjectDestructor_Callback(memobj: cl_mem; pfn_notify: MemObjectDestructor_Callback; user_data: IntPtr): ErrorCode;
    external 'opencl.dll' name 'clSetMemObjectDestructor_Callback';
    static function SetMemObjectDestructor_Callback(memobj: cl_mem; pfn_notify: MemObjectDestructor_Callback; user_data: pointer): ErrorCode;
    external 'opencl.dll' name 'clSetMemObjectDestructor_Callback';
    
    static function EnqueueUnmapMemObject(command_queue: cl_command_queue; memobj: cl_mem; mapped_ptr: IntPtr; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueUnmapMemObject';
    static function EnqueueUnmapMemObject(command_queue: cl_command_queue; memobj: cl_mem; mapped_ptr: IntPtr; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueUnmapMemObject';
    
    static function EnqueueMigrateMemObjects(command_queue: cl_command_queue; num_mem_objects: UInt32; [MarshalAs(UnmanagedType.LPArray)] mem_objects: array of cl_mem; flags: MemMigrationFlags; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueMigrateMemObjects';
    static function EnqueueMigrateMemObjects(command_queue: cl_command_queue; num_mem_objects: UInt32; [MarshalAs(UnmanagedType.LPArray)] mem_objects: array of cl_mem; flags: MemMigrationFlags; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueMigrateMemObjects';
    static function EnqueueMigrateMemObjects(command_queue: cl_command_queue; num_mem_objects: UInt32; mem_objects: ^cl_mem; flags: MemMigrationFlags; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueMigrateMemObjects';
    static function EnqueueMigrateMemObjects(command_queue: cl_command_queue; num_mem_objects: UInt32; mem_objects: ^cl_mem; flags: MemMigrationFlags; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueMigrateMemObjects';
    
    static function GetMemObjectInfo(memobj: cl_mem; param_name: MemObjInfoType; param_value_size: UIntPtr; param_value: pointer; var param_value_size_ret: UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetMemObjectInfo';
    static function GetMemObjectInfo(memobj: cl_mem; param_name: MemObjInfoType; param_value_size: UIntPtr; param_value: pointer; param_value_size_ret: ^UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetMemObjectInfo';
    
    {$endregion Общее}
    
    {$region SVM}
    
    static function SVMAlloc(context: cl_context; flags: MemoryFlags; size: UIntPtr; alignment: UInt32): IntPtr;
    external 'opencl.dll' name 'clSVMAlloc';

    static procedure SVMFree(context: cl_context; svm_pointer: IntPtr);
    external 'opencl.dll' name 'clSVMFree';
    
    static function EnqueueSVMFree(command_queue: cl_command_queue; num_svm_pointers: UInt32; [MarshalAs(UnmanagedType.LPArray)] svm_pointers: array of IntPtr; pfn_free_func: EnqueueSVMFree_Callback; user_data: IntPtr; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueSVMFree';
    static function EnqueueSVMFree(command_queue: cl_command_queue; num_svm_pointers: UInt32; [MarshalAs(UnmanagedType.LPArray)] svm_pointers: array of IntPtr; pfn_free_func: EnqueueSVMFree_Callback; user_data: IntPtr; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueSVMFree';
    static function EnqueueSVMFree(command_queue: cl_command_queue; num_svm_pointers: UInt32; svm_pointers: ^IntPtr; pfn_free_func: EnqueueSVMFree_Callback; user_data: pointer; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueSVMFree';
    static function EnqueueSVMFree(command_queue: cl_command_queue; num_svm_pointers: UInt32; svm_pointers: ^IntPtr; pfn_free_func: EnqueueSVMFree_Callback; user_data: pointer; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueSVMFree';
    
    static function EnqueueSVMMemcpy(command_queue: cl_command_queue; blocking_copy: cl_bool; dst_ptr: IntPtr; src_ptr: IntPtr; size: UIntPtr; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueSVMMemcpy';
    static function EnqueueSVMMemcpy(command_queue: cl_command_queue; blocking_copy: cl_bool; dst_ptr: IntPtr; src_ptr: IntPtr; size: UIntPtr; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueSVMMemcpy';
    
    static function EnqueueSVMMemFill(command_queue: cl_command_queue; svm_ptr: IntPtr; pattern: IntPtr; pattern_size: UIntPtr; size: UIntPtr; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueSVMMemFill';
    static function EnqueueSVMMemFill(command_queue: cl_command_queue; svm_ptr: IntPtr; pattern: IntPtr; pattern_size: UIntPtr; size: UIntPtr; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueSVMMemFill';
    static function EnqueueSVMMemFill(command_queue: cl_command_queue; svm_ptr: IntPtr; pattern: pointer; pattern_size: UIntPtr; size: UIntPtr; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueSVMMemFill';
    static function EnqueueSVMMemFill(command_queue: cl_command_queue; svm_ptr: IntPtr; pattern: pointer; pattern_size: UIntPtr; size: UIntPtr; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueSVMMemFill';
    
    static function EnqueueSVMMap(command_queue: cl_command_queue; blocking_map: cl_bool; flags: MapFlags; svm_ptr: IntPtr; size: UIntPtr; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueSVMMap';
    static function EnqueueSVMMap(command_queue: cl_command_queue; blocking_map: cl_bool; flags: MapFlags; svm_ptr: IntPtr; size: UIntPtr; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueSVMMap';
    
    static function EnqueueSVMUnmap(command_queue: cl_command_queue; svm_ptr: IntPtr; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueSVMUnmap';
    static function EnqueueSVMUnmap(command_queue: cl_command_queue; svm_ptr: IntPtr; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueSVMUnmap';
    
    static function EnqueueSVMMigrateMem(command_queue: cl_command_queue; num_svm_pointers: UInt32; [MarshalAs(UnmanagedType.LPArray)] svm_pointers: array of IntPtr; [MarshalAs(UnmanagedType.LPArray)] sizes: array of UIntPtr; flags: MemMigrationFlags; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueSVMMigrateMem';
    static function EnqueueSVMMigrateMem(command_queue: cl_command_queue; num_svm_pointers: UInt32; [MarshalAs(UnmanagedType.LPArray)] svm_pointers: array of IntPtr; [MarshalAs(UnmanagedType.LPArray)] sizes: array of UIntPtr; flags: MemMigrationFlags; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueSVMMigrateMem';
    static function EnqueueSVMMigrateMem(command_queue: cl_command_queue; num_svm_pointers: UInt32; svm_pointers: ^IntPtr; sizes: ^UIntPtr; flags: MemMigrationFlags; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueSVMMigrateMem';
    static function EnqueueSVMMigrateMem(command_queue: cl_command_queue; num_svm_pointers: UInt32; svm_pointers: ^IntPtr; sizes: ^UIntPtr; flags: MemMigrationFlags; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueSVMMigrateMem';
    
    {$endregion SVM}
    
    {$endregion cl_mem}
    
    {$region Sampler}
    
    static function CreateSamplerWithProperties(context: cl_context; [MarshalAs(UnmanagedType.LPArray)] sampler_properties: array of SamplerInfoType; var errcode_ret: ErrorCode): cl_sampler;
    external 'opencl.dll' name 'clCreateSamplerWithProperties';
    static function CreateSamplerWithProperties(context: cl_context; sampler_properties: ^SamplerInfoType; errcode_ret: ^ErrorCode): cl_sampler;
    external 'opencl.dll' name 'clCreateSamplerWithProperties';
    
    ///Эта функция устарела
    static function CreateSampler(context: cl_context; normalized_coords: cl_bool; addressing_mode: AddressingMode; filter_mode: FilterMode; var errcode_ret: ErrorCode): cl_sampler;
    external 'opencl.dll' name 'clCreateSampler';
    ///Эта функция устарела
    static function CreateSampler(context: cl_context; normalized_coords: cl_bool; addressing_mode: AddressingMode; filter_mode: FilterMode; errcode_ret: ^ErrorCode): cl_sampler;
    external 'opencl.dll' name 'clCreateSampler';
    
    static function RetainSampler(sampler: cl_sampler): ErrorCode;
    external 'opencl.dll' name 'clRetainSampler';
    
    static function ReleaseSampler(sampler: cl_sampler): ErrorCode;
    external 'opencl.dll' name 'clReleaseSampler';
    
    static function GetSamplerInfo(sampler: cl_sampler; param_name: SamplerInfoType; param_value_size: UIntPtr; param_value: pointer; var param_value_size_ret: UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetSamplerInfo';
    static function GetSamplerInfo(sampler: cl_sampler; param_name: SamplerInfoType; param_value_size: UIntPtr; param_value: pointer; param_value_size_ret: ^UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetSamplerInfo';
    
    {$endregion Sampler}
    
    {$region Program}
    
    static function CreateProgramWithSource(context: cl_context; count: UInt32; [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] strings: array of string; [MarshalAs(UnmanagedType.LPArray)] lengths: array of UIntPtr; var errcode_ret: ErrorCode): cl_program;
    external 'opencl.dll' name 'clCreateProgramWithSource';
    static function CreateProgramWithSource(context: cl_context; count: UInt32; strings: ^IntPtr; lengths: ^UIntPtr; errcode_ret: ^ErrorCode): cl_program;
    external 'opencl.dll' name 'clCreateProgramWithSource';
    
    static function CreateProgramWithIL(context: cl_context; il: IntPtr; length: UIntPtr; var errcode_ret: ErrorCode): cl_program;
    external 'opencl.dll' name 'clCreateProgramWithIL';
    static function CreateProgramWithIL(context: cl_context; il: pointer; length: UIntPtr; errcode_ret: ^ErrorCode): cl_program;
    external 'opencl.dll' name 'clCreateProgramWithIL';
    
    static function CreateProgramWithBinary(context: cl_context; num_devices: UInt32; [MarshalAs(UnmanagedType.LPArray)] device_list: array of cl_device_id; [MarshalAs(UnmanagedType.LPArray)] lengths: array of UIntPtr; [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPArray)] binaries: array of array of byte; [MarshalAs(UnmanagedType.LPArray)] binary_status: array of ErrorCode; var errcode_ret: ErrorCode): cl_program;
    external 'opencl.dll' name 'clCreateProgramWithBinary';
    static function CreateProgramWithBinary(context: cl_context; num_devices: UInt32; [MarshalAs(UnmanagedType.LPArray)] device_list: array of cl_device_id; lengths: ^UIntPtr; binaries: ^^byte; binary_status: ^ErrorCode; errcode_ret: ^ErrorCode): cl_program;
    external 'opencl.dll' name 'clCreateProgramWithBinary';
    static function CreateProgramWithBinary(context: cl_context; num_devices: UInt32; device_list: ^cl_device_id; [MarshalAs(UnmanagedType.LPArray)] lengths: array of UIntPtr; [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPArray)] binaries: array of array of byte; [MarshalAs(UnmanagedType.LPArray)] binary_status: array of ErrorCode; var errcode_ret: ErrorCode): cl_program;
    external 'opencl.dll' name 'clCreateProgramWithBinary';
    static function CreateProgramWithBinary(context: cl_context; num_devices: UInt32; device_list: ^cl_device_id; lengths: ^UIntPtr; binaries: ^^byte; binary_status: ^ErrorCode; errcode_ret: ^ErrorCode): cl_program;
    external 'opencl.dll' name 'clCreateProgramWithBinary';
    
    static function CreateProgramWithBuiltInKernels(context: cl_context; num_devices: UInt32; [MarshalAs(UnmanagedType.LPArray)] device_list: array of cl_device_id; [MarshalAs(UnmanagedType.LPStr)] kernel_names: string; var errcode_ret: ErrorCode): cl_program;
    external 'opencl.dll' name 'clCreateProgramWithBuiltInKernels';
    static function CreateProgramWithBuiltInKernels(context: cl_context; num_devices: UInt32; [MarshalAs(UnmanagedType.LPArray)] device_list: array of cl_device_id; kernel_names: IntPtr; var errcode_ret: ErrorCode): cl_program;
    external 'opencl.dll' name 'clCreateProgramWithBuiltInKernels';
    static function CreateProgramWithBuiltInKernels(context: cl_context; num_devices: UInt32; device_list: ^cl_device_id; [MarshalAs(UnmanagedType.LPStr)] kernel_names: string; errcode_ret: ^ErrorCode): cl_program;
    external 'opencl.dll' name 'clCreateProgramWithBuiltInKernels';
    static function CreateProgramWithBuiltInKernels(context: cl_context; num_devices: UInt32; device_list: ^cl_device_id; kernel_names: IntPtr; errcode_ret: ^ErrorCode): cl_program;
    external 'opencl.dll' name 'clCreateProgramWithBuiltInKernels';
    
    static function RetainProgram(&program: cl_program): ErrorCode;
    external 'opencl.dll' name 'clRetainProgram';
    
    static function ReleaseProgram(&program: cl_program): ErrorCode;
    external 'opencl.dll' name 'clReleaseProgram';
    
    static function SetProgramReleaseCallback(&program: cl_program; pfn_notify: Program_Callback; user_data: IntPtr): ErrorCode;
    external 'opencl.dll' name 'clSetProgramReleaseCallback';
    static function SetProgramReleaseCallback(&program: cl_program; pfn_notify: Program_Callback; user_data: pointer): ErrorCode;
    external 'opencl.dll' name 'clSetProgramReleaseCallback';
    
    static function SetProgramSpecializationConstant(&program: cl_program; spec_id: UInt32; spec_size: UIntPtr; spec_value: IntPtr): ErrorCode;
    external 'opencl.dll' name 'clSetProgramSpecializationConstant';
    static function SetProgramSpecializationConstant(&program: cl_program; spec_id: UInt32; spec_size: UIntPtr; spec_value: pointer): ErrorCode;
    external 'opencl.dll' name 'clSetProgramSpecializationConstant';
    
    static function BuildProgram(&program: cl_program; num_devices: UInt32; [MarshalAs(UnmanagedType.LPArray)] device_list: array of cl_device_id; [MarshalAs(UnmanagedType.LPStr)] options: string; pfn_notify: Program_Callback; user_data: IntPtr): ErrorCode;
    external 'opencl.dll' name 'clBuildProgram';
    static function BuildProgram(&program: cl_program; num_devices: UInt32; [MarshalAs(UnmanagedType.LPArray)] device_list: array of cl_device_id; [MarshalAs(UnmanagedType.LPStr)] options: IntPtr; pfn_notify: Program_Callback; user_data: IntPtr): ErrorCode;
    external 'opencl.dll' name 'clBuildProgram';
    static function BuildProgram(&program: cl_program; num_devices: UInt32; device_list: ^cl_device_id; options: string; pfn_notify: Program_Callback; user_data: pointer): ErrorCode;
    external 'opencl.dll' name 'clBuildProgram';
    static function BuildProgram(&program: cl_program; num_devices: UInt32; device_list: ^cl_device_id; options: IntPtr; pfn_notify: Program_Callback; user_data: pointer): ErrorCode;
    external 'opencl.dll' name 'clBuildProgram';
    
    static function CompileProgram(&program: cl_program; num_devices: UInt32; [MarshalAs(UnmanagedType.LPArray)] device_list: array of cl_device_id; [MarshalAs(UnmanagedType.LPStr)] options: string; num_input_headers: UInt32; [MarshalAs(UnmanagedType.LPArray)] input_headers: array of cl_program; [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] header_include_names: array of string; pfn_notify: Program_Callback; user_data: IntPtr): ErrorCode;
    external 'opencl.dll' name 'clCompileProgram';
    static function CompileProgram(&program: cl_program; num_devices: UInt32; [MarshalAs(UnmanagedType.LPArray)] device_list: array of cl_device_id; [MarshalAs(UnmanagedType.LPStr)] options: IntPtr; num_input_headers: UInt32; [MarshalAs(UnmanagedType.LPArray)] input_headers: array of cl_program; [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] header_include_names: array of string; pfn_notify: Program_Callback; user_data: IntPtr): ErrorCode;
    external 'opencl.dll' name 'clCompileProgram';
    static function CompileProgram(&program: cl_program; num_devices: UInt32; device_list: ^cl_device_id; options: string; num_input_headers: UInt32; input_headers: ^cl_program; header_include_names: ^IntPtr; pfn_notify: Program_Callback; user_data: pointer): ErrorCode;
    external 'opencl.dll' name 'clCompileProgram';
    static function CompileProgram(&program: cl_program; num_devices: UInt32; device_list: ^cl_device_id; options: IntPtr; num_input_headers: UInt32; input_headers: ^cl_program; header_include_names: ^IntPtr; pfn_notify: Program_Callback; user_data: pointer): ErrorCode;
    external 'opencl.dll' name 'clCompileProgram';

    static function LinkProgram(context: cl_context; num_devices: UInt32; [MarshalAs(UnmanagedType.LPArray)] device_list: array of cl_device_id; [MarshalAs(UnmanagedType.LPStr)] options: string; num_input_programs: UInt32; [MarshalAs(UnmanagedType.LPArray)] input_programs: array of cl_program; pfn_notify: Program_Callback; user_data: IntPtr; var errcode_ret: ErrorCode): cl_program;
    external 'opencl.dll' name 'clLinkProgram';
    static function LinkProgram(context: cl_context; num_devices: UInt32; [MarshalAs(UnmanagedType.LPArray)] device_list: array of cl_device_id; [MarshalAs(UnmanagedType.LPStr)] options: IntPtr; num_input_programs: UInt32; [MarshalAs(UnmanagedType.LPArray)] input_programs: array of cl_program; pfn_notify: Program_Callback; user_data: IntPtr; var errcode_ret: ErrorCode): cl_program;
    external 'opencl.dll' name 'clLinkProgram';
    static function LinkProgram(context: cl_context; num_devices: UInt32; device_list: ^cl_device_id; options: string; num_input_programs: UInt32; input_programs: ^cl_program; pfn_notify: Program_Callback; user_data: pointer; errcode_ret: ^ErrorCode): cl_program;
    external 'opencl.dll' name 'clLinkProgram';
    static function LinkProgram(context: cl_context; num_devices: UInt32; device_list: ^cl_device_id; options: IntPtr; num_input_programs: UInt32; input_programs: ^cl_program; pfn_notify: Program_Callback; user_data: pointer; errcode_ret: ^ErrorCode): cl_program;
    external 'opencl.dll' name 'clLinkProgram';
    
    static function GetProgramInfo(&program: cl_program; param_name: ProgramInfoType; param_value_size: UIntPtr; param_value: pointer; var param_value_size_ret: UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetProgramInfo';
    static function GetProgramInfo(&program: cl_program; param_name: ProgramInfoType; param_value_size: UIntPtr; param_value: pointer; param_value_size_ret: ^UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetProgramInfo';
    
    static function GetProgramBuildInfo(&program: cl_program; device: cl_device_id; param_name: ProgramBuildInfoType; param_value_size: UIntPtr; param_value: pointer; var param_value_size_ret: UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetProgramBuildInfo';
    static function GetProgramBuildInfo(&program: cl_program; device: cl_device_id; param_name: ProgramBuildInfoType; param_value_size: UIntPtr; param_value: pointer; param_value_size_ret: ^UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetProgramBuildInfo';
    
    {$endregion Program}
    
    {$region Kernel}
    
    static function CreateKernel(&program: cl_program; [MarshalAs(UnmanagedType.LPStr)] kernel_name: string; var errcode_ret: ErrorCode): cl_kernel;
    external 'opencl.dll' name 'clCreateKernel';
    static function CreateKernel(&program: cl_program; kernel_name: IntPtr; errcode_ret: ^ErrorCode): cl_kernel;
    external 'opencl.dll' name 'clCreateKernel';
    
    static function CreateKernelsInProgram(&program: cl_program; num_kernels: UInt32; [MarshalAs(UnmanagedType.LPArray)] kernels: array of cl_kernel; var num_kernels_ret: UInt32): ErrorCode;
    external 'opencl.dll' name 'clCreateKernelsInProgram';
    static function CreateKernelsInProgram(&program: cl_program; num_kernels: UInt32; kernels: ^cl_kernel; num_kernels_ret: ^UInt32): ErrorCode;
    external 'opencl.dll' name 'clCreateKernelsInProgram';
    
    static function RetainKernel(kernel: cl_kernel): ErrorCode;
    external 'opencl.dll' name 'clRetainKernel';
    
    static function ReleaseKernel(kernel: cl_kernel): ErrorCode;
    external 'opencl.dll' name 'clReleaseKernel';
    
    static function SetKernelArg(kernel: cl_kernel; arg_index: UInt32; arg_size: UIntPtr; var arg_value: cl_mem): ErrorCode;
    external 'opencl.dll' name 'clSetKernelArg';
    static function SetKernelArg(kernel: cl_kernel; arg_index: UInt32; arg_size: UIntPtr; arg_value: pointer): ErrorCode;
    external 'opencl.dll' name 'clSetKernelArg';
    
    static function SetKernelArgSVMPointer(kernel: cl_kernel; arg_index: UInt32; arg_value: IntPtr): ErrorCode;
    external 'opencl.dll' name 'clSetKernelArgSVMPointer';
    static function SetKernelArgSVMPointer(kernel: cl_kernel; arg_index: UInt32; arg_value: pointer): ErrorCode;
    external 'opencl.dll' name 'clSetKernelArgSVMPointer';
    
    static function SetKernelExecInfo(kernel: cl_kernel; param_name: KernelExecInfoType; param_value_size: UIntPtr; param_value: pointer): ErrorCode;
    external 'opencl.dll' name 'clSetKernelExecInfo';
    
    static function CloneKernel(source_kernel: cl_kernel; var errcode_ret: ErrorCode): cl_kernel;
    external 'opencl.dll' name 'clCloneKernel';
    static function CloneKernel(source_kernel: cl_kernel; errcode_ret: ^ErrorCode): cl_kernel;
    external 'opencl.dll' name 'clCloneKernel';
    
    static function GetKernelInfo(kernel: cl_kernel; param_name: KernelInfoType; param_value_size: UIntPtr; param_value: pointer; var param_value_size_ret: UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetKernelInfo';
    static function GetKernelInfo(kernel: cl_kernel; param_name: KernelInfoType; param_value_size: UIntPtr; param_value: pointer; param_value_size_ret: ^UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetKernelInfo';
    
    static function GetKernelWorkGroupInfo(kernel: cl_kernel; device: cl_device_id; param_name: KernelWorkGroupInfoType; param_value_size: UIntPtr; param_value: pointer; var param_value_size_ret: UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetKernelWorkGroupInfo';
    static function GetKernelWorkGroupInfo(kernel: cl_kernel; device: cl_device_id; param_name: KernelWorkGroupInfoType; param_value_size: UIntPtr; param_value: pointer; param_value_size_ret: ^UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetKernelWorkGroupInfo';
    
    static function GetKernelSubGroupInfo(kernel: cl_kernel; device: cl_device_id; param_name: KernelSubGroupInfoType; input_value_size: UIntPtr; input_value: pointer; param_value_size: UIntPtr; param_value: pointer; var param_value_size_ret: UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetKernelSubGroupInfo';
    static function GetKernelSubGroupInfo(kernel: cl_kernel; device: cl_device_id; param_name: KernelSubGroupInfoType; input_value_size: UIntPtr; input_value: pointer; param_value_size: UIntPtr; param_value: pointer; param_value_size_ret: ^UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetKernelSubGroupInfo';
    
    static function GetKernelArgInfo(kernel: cl_kernel; arg_indx: UInt32; param_name: KernelArgInfoType; param_value_size: UIntPtr; param_value: pointer; var param_value_size_ret: UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetKernelArgInfo';
    static function GetKernelArgInfo(kernel: cl_kernel; arg_indx: UInt32; param_name: KernelArgInfoType; param_value_size: UIntPtr; param_value: pointer; param_value_size_ret: ^UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetKernelArgInfo';
    
    static function EnqueueNDRangeKernel(command_queue: cl_command_queue; kernel: cl_kernel; work_dim: UInt32; [MarshalAs(UnmanagedType.LPArray)] global_work_offset: array of UIntPtr; [MarshalAs(UnmanagedType.LPArray)] global_work_size: array of UIntPtr; [MarshalAs(UnmanagedType.LPArray)] local_work_size: array of UIntPtr; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueNDRangeKernel';
    static function EnqueueNDRangeKernel(command_queue: cl_command_queue; kernel: cl_kernel; work_dim: UInt32; [MarshalAs(UnmanagedType.LPArray)] global_work_offset: array of UIntPtr; [MarshalAs(UnmanagedType.LPArray)] global_work_size: array of UIntPtr; [MarshalAs(UnmanagedType.LPArray)] local_work_size: array of UIntPtr; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueNDRangeKernel';
    static function EnqueueNDRangeKernel(command_queue: cl_command_queue; kernel: cl_kernel; work_dim: UInt32; global_work_offset: ^UIntPtr; global_work_size: ^UIntPtr; local_work_size: ^UIntPtr; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueNDRangeKernel';
    static function EnqueueNDRangeKernel(command_queue: cl_command_queue; kernel: cl_kernel; work_dim: UInt32; global_work_offset: ^UIntPtr; global_work_size: ^UIntPtr; local_work_size: ^UIntPtr; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueNDRangeKernel';
    
    static function EnqueueNativeKernel(command_queue: cl_command_queue; user_func: EnqueueNativeKernel_Callback; args: pointer; cb_args: UIntPtr; num_mem_objects: UInt32; [MarshalAs(UnmanagedType.LPArray)] mem_list: array of cl_mem; [MarshalAs(UnmanagedType.LPArray)] args_mem_loc: array of IntPtr; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueNativeKernel';
    static function EnqueueNativeKernel(command_queue: cl_command_queue; user_func: EnqueueNativeKernel_Callback; args: pointer; cb_args: UIntPtr; num_mem_objects: UInt32; mem_list: ^cl_mem; args_mem_loc: ^pointer; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueNativeKernel';
    
    ///Эта функция устарела
    static function EnqueueTask(command_queue: cl_command_queue; kernel: cl_kernel; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueTask';
    ///Эта функция устарела
    static function EnqueueTask(command_queue: cl_command_queue; kernel: cl_kernel; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueTask';
    
    {$endregion Kernel}
    
    {$region Event}
    
    static function CreateUserEvent(context: cl_context; var errcode_ret: ErrorCode): cl_event;
    external 'opencl.dll' name 'clCreateUserEvent';
    static function CreateUserEvent(context: cl_context; errcode_ret: ^ErrorCode): cl_event;
    external 'opencl.dll' name 'clCreateUserEvent';
    
    static function SetUserEventStatus(&event: cl_event; execution_status: CommandExecutionStatus): ErrorCode;
    external 'opencl.dll' name 'clSetUserEventStatus';
    
    static function WaitForEvents(num_events: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_list: array of cl_event): ErrorCode;
    external 'opencl.dll' name 'clWaitForEvents';
    static function WaitForEvents(num_events: UInt32; event_list: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clWaitForEvents';
    
    static function GetEventInfo(&event: cl_event; param_name: EventInfoType; param_value_size: UIntPtr; param_value: pointer; var param_value_size_ret: UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetEventInfo';
    static function GetEventInfo(&event: cl_event; param_name: EventInfoType; param_value_size: UIntPtr; param_value: pointer; param_value_size_ret: ^UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetEventInfo';
    
    static function SetEventCallback(&event: cl_event; command_exec_callback_type: CommandExecutionStatus; pfn_notify: Event_Callback; user_data: pointer): ErrorCode;
    external 'opencl.dll' name 'clSetEventCallback';
    
    static function RetainEvent(&event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clRetainEvent';
    
    static function ReleaseEvent(&event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clReleaseEvent';
    
    static function EnqueueMarkerWithWaitList(command_queue: cl_command_queue; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueMarkerWithWaitList';
    static function EnqueueMarkerWithWaitList(command_queue: cl_command_queue; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueMarkerWithWaitList';
    
    static function EnqueueBarrierWithWaitList(command_queue: cl_command_queue; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueBarrierWithWaitList';
    static function EnqueueBarrierWithWaitList(command_queue: cl_command_queue; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueBarrierWithWaitList';
    
    static function GetEventProfilingInfo(&event: cl_event; param_name: ProfilingInfoType; param_value_size: UIntPtr; param_value: pointer; var param_value_size_ret: UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetEventProfilingInfo';
    static function GetEventProfilingInfo(&event: cl_event; param_name: ProfilingInfoType; param_value_size: UIntPtr; param_value: pointer; param_value_size_ret: ^UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetEventProfilingInfo';
    
    {$endregion Event}
    
  end;
  
  cl_ext = static class
    
    {$region Misc}
    
    static function TerminateContextKHR(context: cl_context): ErrorCode;
    external 'opencl.dll' name 'clTerminateContextKHR';
    
    static function EnqueueAcquireGrallocObjectsIMG(command_queue: cl_command_queue; num_objects: UInt32; [MarshalAs(UnmanagedType.LPArray)] mem_objects: array of cl_mem; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueAcquireGrallocObjectsIMG';
    static function EnqueueAcquireGrallocObjectsIMG(command_queue: cl_command_queue; num_objects: UInt32; [MarshalAs(UnmanagedType.LPArray)] mem_objects: array of cl_mem; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueAcquireGrallocObjectsIMG';
    static function EnqueueAcquireGrallocObjectsIMG(command_queue: cl_command_queue; num_objects: UInt32; mem_objects: ^cl_mem; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueAcquireGrallocObjectsIMG';
    static function EnqueueAcquireGrallocObjectsIMG(command_queue: cl_command_queue; num_objects: UInt32; mem_objects: ^cl_mem; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueAcquireGrallocObjectsIMG';
    
    static function EnqueueReleaseGrallocObjectsIMG(command_queue: cl_command_queue; num_objects: UInt32; [MarshalAs(UnmanagedType.LPArray)] mem_objects: array of cl_mem; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueAcquireGrallocObjectsIMG';
    static function EnqueueReleaseGrallocObjectsIMG(command_queue: cl_command_queue; num_objects: UInt32; [MarshalAs(UnmanagedType.LPArray)] mem_objects: array of cl_mem; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueAcquireGrallocObjectsIMG';
    static function EnqueueReleaseGrallocObjectsIMG(command_queue: cl_command_queue; num_objects: UInt32; mem_objects: ^cl_mem; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueAcquireGrallocObjectsIMG';
    static function EnqueueReleaseGrallocObjectsIMG(command_queue: cl_command_queue; num_objects: UInt32; mem_objects: ^cl_mem; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueAcquireGrallocObjectsIMG';
    
    {$endregion Misc}
    
    {$region ARM}
    
    static function ImportMemoryARM(context: cl_context; flags: MemoryFlags; [MarshalAs(UnmanagedType.LPArray)] properties: array of ImportPropertiesARM; memory: IntPtr; size: UIntPtr; var errcode_ret: ErrorCode): cl_mem;
    external 'opencl.dll' name 'clImportMemoryARM';
    static function ImportMemoryARM(context: cl_context; flags: MemoryFlags; properties: ^ImportPropertiesARM; memory: pointer; size: UIntPtr; errcode_ret: ^ErrorCode): cl_mem;
    external 'opencl.dll' name 'clImportMemoryARM';
    
    static function SetKernelExecInfoARM(kernel: cl_kernel; param_name: KernelExecARMInfoType; param_value_size: UIntPtr; param_value: pointer): ErrorCode;
    external 'opencl.dll' name 'clSetKernelExecInfoARM';
    
    static function SVMAllocARM(context: cl_context; flags: MemoryFlags; size: UIntPtr; alignment: UInt32): IntPtr;
    external 'opencl.dll' name 'clSVMAllocARM';
    
    static procedure SVMFreeARM(context: cl_context; svm_pointer: IntPtr);
    external 'opencl.dll' name 'clSVMFreeARM';
    
    static function EnqueueSVMFreeARM(command_queue: cl_command_queue; num_svm_pointers: UInt32; [MarshalAs(UnmanagedType.LPArray)] svm_pointers: array of IntPtr; pfn_free_func: EnqueueSVMFree_Callback; user_data: pointer; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueSVMFreeARM';
    static function EnqueueSVMFreeARM(command_queue: cl_command_queue; num_svm_pointers: UInt32; [MarshalAs(UnmanagedType.LPArray)] svm_pointers: array of IntPtr; pfn_free_func: EnqueueSVMFree_Callback; user_data: pointer; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueSVMFreeARM';
    static function EnqueueSVMFreeARM(command_queue: cl_command_queue; num_svm_pointers: UInt32; svm_pointers: ^IntPtr; pfn_free_func: EnqueueSVMFree_Callback; user_data: pointer; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueSVMFreeARM';
    static function EnqueueSVMFreeARM(command_queue: cl_command_queue; num_svm_pointers: UInt32; svm_pointers: ^IntPtr; pfn_free_func: EnqueueSVMFree_Callback; user_data: pointer; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueSVMFreeARM';
    
    static function EnqueueSVMMemcpyARM(command_queue: cl_command_queue; blocking_copy: cl_bool; dst_ptr: IntPtr; src_ptr: IntPtr; size: UIntPtr; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueSVMMemcpyARM';
    static function EnqueueSVMMemcpyARM(command_queue: cl_command_queue; blocking_copy: cl_bool; dst_ptr: IntPtr; src_ptr: IntPtr; size: UIntPtr; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueSVMMemcpyARM';
    static function EnqueueSVMMemcpyARM(command_queue: cl_command_queue; blocking_copy: cl_bool; dst_ptr: pointer; src_ptr: pointer; size: UIntPtr; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueSVMMemcpyARM';
    static function EnqueueSVMMemcpyARM(command_queue: cl_command_queue; blocking_copy: cl_bool; dst_ptr: pointer; src_ptr: pointer; size: UIntPtr; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueSVMMemcpyARM';
    
    static function EnqueueSVMMemFillARM(command_queue: cl_command_queue; svm_ptr: IntPtr; pattern: IntPtr; pattern_size: UIntPtr; size: UIntPtr; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueSVMMemFillARM';
    static function EnqueueSVMMemFillARM(command_queue: cl_command_queue; svm_ptr: IntPtr; pattern: IntPtr; pattern_size: UIntPtr; size: UIntPtr; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueSVMMemFillARM';
    static function EnqueueSVMMemFillARM(command_queue: cl_command_queue; svm_ptr: pointer; pattern: pointer; pattern_size: UIntPtr; size: UIntPtr; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueSVMMemFillARM';
    static function EnqueueSVMMemFillARM(command_queue: cl_command_queue; svm_ptr: pointer; pattern: pointer; pattern_size: UIntPtr; size: UIntPtr; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueSVMMemFillARM';
    
    static function EnqueueSVMMapARM(command_queue: cl_command_queue; blocking_map: cl_bool; flags: MapFlags; svm_ptr: IntPtr; size: UIntPtr; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueSVMMapARM';
    static function EnqueueSVMMapARM(command_queue: cl_command_queue; blocking_map: cl_bool; flags: MapFlags; svm_ptr: IntPtr; size: UIntPtr; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueSVMMapARM';
    static function EnqueueSVMMapARM(command_queue: cl_command_queue; blocking_map: cl_bool; flags: MapFlags; svm_ptr: pointer; size: UIntPtr; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueSVMMapARM';
    static function EnqueueSVMMapARM(command_queue: cl_command_queue; blocking_map: cl_bool; flags: MapFlags; svm_ptr: pointer; size: UIntPtr; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueSVMMapARM';
    
    static function EnqueueSVMUnmapARM(command_queue: cl_command_queue; svm_ptr: IntPtr; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueSVMUnmapARM';
    static function EnqueueSVMUnmapARM(command_queue: cl_command_queue; svm_ptr: IntPtr; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueSVMUnmapARM';
    static function EnqueueSVMUnmapARM(command_queue: cl_command_queue; svm_ptr: pointer; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueSVMUnmapARM';
    static function EnqueueSVMUnmapARM(command_queue: cl_command_queue; svm_ptr: pointer; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueSVMUnmapARM';
    
    static function SetKernelArgSVMPointerARM(kernel: cl_kernel; arg_index: UInt32; arg_value: IntPtr): ErrorCode;
    external 'opencl.dll' name 'clSetKernelArgSVMPointerARM';
    static function SetKernelArgSVMPointerARM(kernel: cl_kernel; arg_index: UInt32; arg_value: pointer): ErrorCode;
    external 'opencl.dll' name 'clSetKernelArgSVMPointerARM';
    
    {$endregion ARM}
    
  end;
  
  cl_gl = static class
    
    {$region Buffer}
    
    static function CreateFromGLBuffer(context: cl_context; flags: MemoryFlags; bufobj: UInt32; var errcode_ret: ErrorCode): cl_mem;
    external 'opencl.dll' name 'clCreateFromGLBuffer';
    static function CreateFromGLBuffer(context: cl_context; flags: MemoryFlags; bufobj: UInt32; errcode_ret: ^ErrorCode): cl_mem;
    external 'opencl.dll' name 'clCreateFromGLBuffer';
    
    {$endregion Buffer}
    
    {$region RenderBuffer}
    
    static function CreateFromGLRenderbuffer(context: cl_context; flags: MemoryFlags; renderbuffer: UInt32; var errcode_ret: ErrorCode): cl_mem;
    external 'opencl.dll' name 'clCreateFromGLRenderbuffer';
    static function CreateFromGLRenderbuffer(context: cl_context; flags: MemoryFlags; renderbuffer: UInt32; errcode_ret: ^ErrorCode): cl_mem;
    external 'opencl.dll' name 'clCreateFromGLRenderbuffer';
    
    {$endregion RenderBuffer}
    
    {$region Texture}
    
    static function CreateFromGLTexture(context: cl_context; flags: MemoryFlags; target: UInt32; miplevel: Int32; texture: UInt32; var errcode_ret: ErrorCode): cl_mem;
    external 'opencl.dll' name 'clCreateFromGLTexture';
    static function CreateFromGLTexture(context: cl_context; flags: MemoryFlags; target: UInt32; miplevel: Int32; texture: UInt32; errcode_ret: ^ErrorCode): cl_mem;
    external 'opencl.dll' name 'clCreateFromGLTexture';
    
    static function CreateFromGLTexture2D(context: cl_context; flags: MemoryFlags; target: UInt32; miplevel: Int32; texture: UInt32; var errcode_ret: ErrorCode): cl_mem;
    external 'opencl.dll' name 'clCreateFromGLTexture2D';
    static function CreateFromGLTexture2D(context: cl_context; flags: MemoryFlags; target: UInt32; miplevel: Int32; texture: UInt32; errcode_ret: ^ErrorCode): cl_mem;
    external 'opencl.dll' name 'clCreateFromGLTexture2D';
    
    static function CreateFromGLTexture3D(context: cl_context; flags: MemoryFlags; target: UInt32; miplevel: Int32; texture: UInt32; var errcode_ret: ErrorCode): cl_mem;
    external 'opencl.dll' name 'clCreateFromGLTexture3D';
    static function CreateFromGLTexture3D(context: cl_context; flags: MemoryFlags; target: UInt32; miplevel: Int32; texture: UInt32; errcode_ret: ^ErrorCode): cl_mem;
    external 'opencl.dll' name 'clCreateFromGLTexture3D';
    
    static function GetGLTextureInfo(memobj: cl_mem; param_name: GLTextureInfoType; param_value_size: UIntPtr; param_value: pointer; var param_value_size_ret: UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetGLTextureInfo';
    static function GetGLTextureInfo(memobj: cl_mem; param_name: GLTextureInfoType; param_value_size: UIntPtr; param_value: pointer; param_value_size_ret: ^UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetGLTextureInfo';
    
    {$endregion Texture}
    
    {$region Общее}
    
    static function GetGLContextInfoKHR([MarshalAs(UnmanagedType.LPArray)] properties: array of ContextProperties; param_name: GLContextInfoType; param_value_size: UIntPtr; param_value: pointer; var param_value_size_ret: UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetGLContextInfoKHR';
    static function GetGLContextInfoKHR(properties: ^ContextProperties; param_name: GLContextInfoType; param_value_size: UIntPtr; param_value: pointer; param_value_size_ret: ^UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetGLContextInfoKHR';
    
    static function EnqueueAcquireGLObjects(command_queue: cl_command_queue; num_objects: UInt32; [MarshalAs(UnmanagedType.LPArray)] mem_objects: array of cl_mem; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueAcquireGLObjects';
    static function EnqueueAcquireGLObjects(command_queue: cl_command_queue; num_objects: UInt32; [MarshalAs(UnmanagedType.LPArray)] mem_objects: array of cl_mem; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueAcquireGLObjects';
    static function EnqueueAcquireGLObjects(command_queue: cl_command_queue; num_objects: UInt32; mem_objects: ^cl_mem; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueAcquireGLObjects';
    static function EnqueueAcquireGLObjects(command_queue: cl_command_queue; num_objects: UInt32; mem_objects: ^cl_mem; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueAcquireGLObjects';
    
    static function EnqueueReleaseGLObjects(command_queue: cl_command_queue; num_objects: UInt32; [MarshalAs(UnmanagedType.LPArray)] mem_objects: array of cl_mem; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueReleaseGLObjects';
    static function EnqueueReleaseGLObjects(command_queue: cl_command_queue; num_objects: UInt32; [MarshalAs(UnmanagedType.LPArray)] mem_objects: array of cl_mem; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueReleaseGLObjects';
    static function EnqueueReleaseGLObjects(command_queue: cl_command_queue; num_objects: UInt32; mem_objects: ^cl_mem; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueReleaseGLObjects';
    static function EnqueueReleaseGLObjects(command_queue: cl_command_queue; num_objects: UInt32; mem_objects: ^cl_mem; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueReleaseGLObjects';
    
    static function GetGLObjectInfo(memobj: cl_mem; var gl_object_type: GLObjectType; var gl_object_name: UInt32): ErrorCode;
    external 'opencl.dll' name 'clGetGLObjectInfo';
    static function GetGLObjectInfo(memobj: cl_mem; gl_object_type: ^GLObjectType; gl_object_name: ^UInt32): ErrorCode;
    external 'opencl.dll' name 'clGetGLObjectInfo';
    
    static function CreateEventFromGLsyncKHR(context: cl_context; cl_GLsync: IntPtr; var errcode_ret: ErrorCode): cl_event;
    external 'opencl.dll' name 'clCreateEventFromGLsyncKHR';
    static function CreateEventFromGLsyncKHR(context: cl_context; cl_GLsync: IntPtr; errcode_ret: ^ErrorCode): cl_event;
    external 'opencl.dll' name 'clCreateEventFromGLsyncKHR';
    
    {$endregion Общее}
    
  end;
  
  cl_egl = static class
    
    {$region Разное}
    
    static function CreateFromEGLImageKHR(context: cl_context; egldisplay: IntPtr; eglimage: IntPtr; flags: MemoryFlags; [MarshalAs(UnmanagedType.LPArray)] properties: array of IntPtr; var errcode_ret: ErrorCode): cl_mem;
    external 'opencl.dll' name 'clCreateFromEGLImageKHR';
    static function CreateFromEGLImageKHR(context: cl_context; egldisplay: IntPtr; eglimage: IntPtr; flags: MemoryFlags; properties: ^IntPtr; errcode_ret: ^ErrorCode): cl_mem;
    external 'opencl.dll' name 'clCreateFromEGLImageKHR';
    
    static function EnqueueAcquireEGLObjectsKHR(command_queue: cl_command_queue; num_objects: UInt32; [MarshalAs(UnmanagedType.LPArray)] mem_objects: array of cl_mem; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueAcquireEGLObjectsKHR';
    static function EnqueueAcquireEGLObjectsKHR(command_queue: cl_command_queue; num_objects: UInt32; [MarshalAs(UnmanagedType.LPArray)] mem_objects: array of cl_mem; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueAcquireEGLObjectsKHR';
    static function EnqueueAcquireEGLObjectsKHR(command_queue: cl_command_queue; num_objects: UInt32; mem_objects: ^cl_mem; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueAcquireEGLObjectsKHR';
    static function EnqueueAcquireEGLObjectsKHR(command_queue: cl_command_queue; num_objects: UInt32; mem_objects: ^cl_mem; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueAcquireEGLObjectsKHR';
    
    static function EnqueueReleaseEGLObjectsKHR(command_queue: cl_command_queue; num_objects: UInt32; [MarshalAs(UnmanagedType.LPArray)] mem_objects: array of cl_mem; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueReleaseEGLObjectsKHR';
    static function EnqueueReleaseEGLObjectsKHR(command_queue: cl_command_queue; num_objects: UInt32; [MarshalAs(UnmanagedType.LPArray)] mem_objects: array of cl_mem; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueReleaseEGLObjectsKHR';
    static function EnqueueReleaseEGLObjectsKHR(command_queue: cl_command_queue; num_objects: UInt32; mem_objects: ^cl_mem; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueReleaseEGLObjectsKHR';
    static function EnqueueReleaseEGLObjectsKHR(command_queue: cl_command_queue; num_objects: UInt32; mem_objects: ^cl_mem; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueReleaseEGLObjectsKHR';
    
    static function CreateEventFromEGLSyncKHR(context: cl_context; sync: IntPtr; display: IntPtr; var errcode_ret: ErrorCode): cl_event;
    external 'opencl.dll' name 'clCreateEventFromEGLSyncKHR';
    static function CreateEventFromEGLSyncKHR(context: cl_context; sync: IntPtr; display: IntPtr; errcode_ret: ^ErrorCode): cl_event;
    external 'opencl.dll' name 'clCreateEventFromEGLSyncKHR';
    
    {$endregion Разное}
    
  end;
  
{$region Impl}

constructor OpenCLException.Create(err_code: UInt32) :=
Create(ErrorCode.Create(err_code).ToString);

{$region Debug}

{$ifdef DebugMode}

function cl_event.GetRefCount: IntPtr;
begin
  cl.GetEventInfo(self, EventInfoType.REFERENCE_COUNT, new UIntPtr(IntPtr.Size), @Result, nil).RaiseIfError;
end;

function cl_event.GetState: CommandExecutionStatus;
begin
  cl.GetEventInfo(self, EventInfoType.COMMAND_EXECUTION_STATUS, new UIntPtr(4), @Result, nil).RaiseIfError;
end;

{$endif DebugMode}

{$endregion Debug}

{$endregion Impl}

end.