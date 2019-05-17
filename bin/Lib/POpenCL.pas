///
///Код переведён отсюда:
///https://github.com/KhronosGroup/OpenCL-Headers/tree/master/CL
///
///Справка:
///www.khronos.org/registry/OpenCL/specs/2.2/html/OpenCL_API.html
///
unit POpenCL;

interface

uses System;
uses System.Runtime.InteropServices;

{$region Основные типы}

type
  
  cl_platform_id              = IntPtr;
  cl_device_id                = IntPtr;
  cl_context                  = IntPtr;
  cl_command_queue            = IntPtr;
  cl_mem                      = IntPtr;
  cl_program                  = IntPtr;
  cl_kernel                   = IntPtr;
  cl_event                    = IntPtr;
  cl_sampler                  = IntPtr;
  
  ///0=false, остальное=true
  cl_bool                     = UInt32;
  cl_bitfield                 = UInt64;
  cl_platform_info            = UInt32;
  cl_device_info              = UInt32;
  cl_device_mem_cache_type    = UInt32;
  cl_device_local_mem_type    = UInt32;
  
  cl_context_info             = UInt32;
  cl_command_queue_info       = UInt32;
  cl_channel_order            = UInt32;
  cl_channel_type             = UInt32;
  cl_mem_object_type          = UInt32;
  cl_mem_info                 = UInt32;
  cl_image_info               = UInt32;
  cl_addressing_mode          = UInt32;
  cl_filter_mode              = UInt32;
  cl_sampler_info             = UInt32;
  cl_program_info             = UInt32;
  cl_program_build_info       = UInt32;
  cl_build_status             = Int32;
  cl_kernel_info              = UInt32;
  cl_kernel_work_group_info   = UInt32;
  cl_event_info               = UInt32;
  cl_command_type             = UInt32;
  cl_profiling_info           = UInt32;
  
type
  cl_image_format = record
    image_channel_order:      cl_channel_order;
    image_channel_data_type:  cl_channel_type;
    
    constructor(image_channel_order: cl_channel_order; image_channel_data_type: cl_channel_type);
    begin
      self.image_channel_order := image_channel_order;
      self.image_channel_data_type := image_channel_data_type;
    end;
    
  end;
  
  cl_context_properties = record
    private header := new IntPtr($1084); // CL_CONTEXT_PLATFORM, больше сюда ничего ставить и нельзя
    public platform: cl_platform_id;
    private null_terminator := new IntPtr(0);
    
    public constructor(platform: cl_platform_id) :=
    self.platform := platform;
    
  end;
  
type
  OpenCLException = class(Exception)
    
    constructor(text: string) :=
    inherited Create($'Ошибка OpenCL: "{text}"');
    
  end;
  
{$endregion Основные типы}

{$region Типы делегатов}

type
  [UnmanagedFunctionPointer(CallingConvention.StdCall)]
  CreateContext_Callback = procedure(errinfo: ^char; private_info: pointer; cb: UIntPtr; user_data: pointer);
  
  [UnmanagedFunctionPointer(CallingConvention.StdCall)]
  BuildProgram_Callback = procedure(&program: cl_program; user_data: pointer);
  
  [UnmanagedFunctionPointer(CallingConvention.StdCall)]
  EnqueueNativeKernel_Callback = procedure(args: pointer);
  
{$endregion Типы делегатов}

{$region Энумы}

type
  ErrorCode = record
    public val: integer;
    
    public const SUCCESS =                          -0;
    
    public const DEVICE_NOT_FOUND =                 -1;
    public const DEVICE_NOT_AVAILABLE =             -2;
    public const COMPILER_NOT_AVAILABLE =           -3;
    public const MEM_OBJECT_ALLOCATION_FAILURE =    -4;
    public const OUT_OF_RESOURCES =                 -5;
    public const OUT_OF_HOST_MEMORY =               -6;
    public const PROFILING_INFO_NOT_AVAILABLE =     -7;
    public const MEM_COPY_OVERLAP =                 -8;
    public const IMAGE_FORMAT_MISMATCH =            -9;
    public const IMAGE_FORMAT_NOT_SUPPORTED =      -10;
    public const BUILD_PROGRAM_FAILURE =           -11;
    public const MAP_FAILURE =                     -12;
    
    public const INVALID_VALUE =                   -30;
    public const INVALID_DEVICE_TYPE =             -31;
    public const INVALID_PLATFORM =                -32;
    public const INVALID_DEVICE =                  -33;
    public const INVALID_CONTEXT =                 -34;
    public const INVALID_QUEUE_PROPERTIES =        -35;
    public const INVALID_COMMAND_QUEUE =           -36;
    public const INVALID_HOST_PTR =                -37;
    public const INVALID_MEM_OBJECT =              -38;
    public const INVALID_IMAGE_FORMAT_DESCRIPTOR = -39;
    public const INVALID_IMAGE_SIZE =              -40;
    public const INVALID_SAMPLER =                 -41;
    public const INVALID_BINARY =                  -42;
    public const INVALID_BUILD_OPTIONS =           -43;
    public const INVALID_PROGRAM =                 -44;
    public const INVALID_PROGRAM_EXECUTABLE =      -45;
    public const INVALID_KERNEL_NAME =             -46;
    public const INVALID_KERNEL_DEFINITION =       -47;
    public const INVALID_KERNEL =                  -48;
    public const INVALID_ARG_INDEX =               -49;
    public const INVALID_ARG_VALUE =               -50;
    public const INVALID_ARG_SIZE =                -51;
    public const INVALID_KERNEL_ARGS =             -52;
    public const INVALID_WORK_DIMENSION =          -53;
    public const INVALID_WORK_GROUP_SIZE =         -54;
    public const INVALID_WORK_ITEM_SIZE =          -55;
    public const INVALID_GLOBAL_OFFSET =           -56;
    public const INVALID_EVENT_WAIT_LIST =         -57;
    public const INVALID_EVENT =                   -58;
    public const INVALID_OPERATION =               -59;
    public const INVALID_GL_OBJECT =               -60;
    public const INVALID_BUFFER_SIZE =             -61;
    public const INVALID_MIP_LEVEL =               -62;
    public const INVALID_GLOBAL_WORK_SIZE =        -63;
    
    public function ToString: string; override;
    begin
      var res := typeof(ErrorCode).GetFields.Where(fi->fi.IsLiteral).FirstOrDefault(prop->integer(prop.GetValue(nil)) = self.val);
      Result := res=nil?
        $'ErrorCode[{self.val}]':
        res.Name.ToWords('_').Select(w->w[1].ToUpper+w.Substring(1).ToLower).JoinIntoString;
    end;
    
    public procedure RaiseIfError :=
    if val<>SUCCESS then raise new OpenCLException(self.ToString);
    
  end;
  
  DeviceType = record
    public val: cl_bitfield;
    public constructor(val: cl_bitfield) := self.val := val;
    
    public static property &Default:    DeviceType read new DeviceType(1 shl 0);
    public static property CPU:         DeviceType read new DeviceType(1 shl 1);
    public static property GPU:         DeviceType read new DeviceType(1 shl 2);
    public static property Accelerator: DeviceType read new DeviceType(1 shl 3);
    public static property All:         DeviceType read new DeviceType($FFFFFFFF);
    
    public static function operator or(a,b: DeviceType) :=
    new DeviceType(a.val or b.val);
    
    public function ToString: string; override;
    begin
      var res := typeof(DeviceType).GetProperties.Select(prop->(prop.Name,DeviceType(prop.GetValue(nil)).val)).Where(t-> self.val and t[1] = t[1]).ToArray;
      Result := res.Length=0?
        $'DeviceType[{self.val}]':
        res.Select(t->t[0]).JoinIntoString('+');
    end;
    
  end;
  
  MemoryFlags = record
    public val: cl_bitfield;
    public constructor(val: cl_bitfield) := self.val := val;
    
    public static property READ_WRITE:     MemoryFlags read new MemoryFlags(1 shl 0);
    public static property WRITE_ONLY:     MemoryFlags read new MemoryFlags(1 shl 1);
    public static property READ_ONLY:      MemoryFlags read new MemoryFlags(1 shl 2);
    public static property USE_HOST_PTR:   MemoryFlags read new MemoryFlags(1 shl 3);
    public static property ALLOC_HOST_PTR: MemoryFlags read new MemoryFlags(1 shl 4);
    public static property COPY_HOST_PTR:  MemoryFlags read new MemoryFlags(1 shl 5);
    
    public static function operator or(a,b: MemoryFlags) :=
    new MemoryFlags(a.val or b.val);
    
    public function ToString: string; override;
    begin
      var res := typeof(MemoryFlags).GetProperties.Select(prop->(prop.Name,MemoryFlags(prop.GetValue(nil)).val)).Where(t-> self.val and t[1] = t[1]).ToArray;
      Result := res.Length=0?
        $'MemoryFlags[{self.val}]':
        res.Select(t->t[0]).JoinIntoString('+');
    end;
    
  end;
  
  CommandQueueProperties = record
    public val: cl_bitfield;
    public constructor(val: cl_bitfield) := self.val := val;
    
    public static property NONE:                                CommandQueueProperties read new CommandQueueProperties(0);
    public static property QUEUE_OUT_OF_ORDER_EXEC_MODE_ENABLE: CommandQueueProperties read new CommandQueueProperties(1 shl 0);
    public static property QUEUE_PROFILING_ENABLE:              CommandQueueProperties read new CommandQueueProperties(1 shl 1);
    
    public static function operator or(a,b: CommandQueueProperties) :=
    new CommandQueueProperties(a.val or b.val);
    
    public function ToString: string; override;
    begin
      var res := typeof(CommandQueueProperties).GetProperties.Skip(1).Select(prop->(prop.Name,CommandQueueProperties(prop.GetValue(nil)).val)).Where(t-> self.val and t[1] = t[1]).ToArray;
      Result := res.Length=0?
        val=0?'NONE':$'CommandQueueProperties[{self.val}]':
        res.Select(t->t[0]).JoinIntoString('+');
    end;
    
  end;
  
  MapFlags = record
    public val: cl_bitfield;
    public constructor(val: cl_bitfield) := self.val := val;
    
    public static property MAP_READ:  MapFlags read new MapFlags(1 shl 0);
    public static property MAP_WRITE: MapFlags read new MapFlags(1 shl 1);
    
    public static function operator or(a,b: MapFlags) :=
    new MapFlags(a.val or b.val);
    
    public function ToString: string; override;
    begin
      var res := typeof(MapFlags).GetProperties.Select(prop->(prop.Name,MapFlags(prop.GetValue(nil)).val)).Where(t-> self.val and t[1] = t[1]).ToArray;
      Result := res.Length=0?
        $'MapFlags[{self.val}]':
        res.Select(t->t[0]).JoinIntoString('+');
    end;
    
  end;
  
{$endregion Энумы}

type
  cl = static class
    
    {$region Platform}
    
    static function GetPlatformIDs(num_entries: UInt32; [MarshalAs(UnmanagedType.LPArray)] platforms: array of cl_platform_id; var num_platforms: UInt32): ErrorCode;
    external 'opencl.dll' name 'clGetPlatformIDs';
    static function GetPlatformIDs(num_entries: UInt32; platforms: ^cl_platform_id; num_platforms: ^UInt32): ErrorCode;
    external 'opencl.dll' name 'clGetPlatformIDs';
    
    static function GetPlatformInfo(platform: cl_platform_id; param_name: cl_platform_info; param_value_size: UIntPtr; param_value: pointer; var param_value_size_ret: UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetPlatformInfo';
    static function GetPlatformInfo(platform: cl_platform_id; param_name: cl_platform_info; param_value_size: UIntPtr; param_value: pointer; param_value_size_ret: ^UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetPlatformInfo';
    
    {$endregion Platform}
    
    {$region Device}
    
    static function GetDeviceIDs(platform: cl_platform_id; device_type: DeviceType; num_entries: UInt32; [MarshalAs(UnmanagedType.LPArray)] devices: array of cl_device_id; var num_devices: UInt32): ErrorCode;
    external 'opencl.dll' name 'clGetDeviceIDs';
    static function GetDeviceIDs(platform: cl_platform_id; device_type: DeviceType; num_entries: UInt32; devices: ^cl_device_id; num_devices: ^UInt32): ErrorCode;
    external 'opencl.dll' name 'clGetDeviceIDs';
    
    static function GetDeviceInfo(device: cl_device_id; param_name: cl_device_info; param_value_size: UIntPtr; param_value: pointer; var param_value_size_ret: UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetDeviceInfo';
    static function GetDeviceInfo(device: cl_device_id; param_name: cl_device_info; param_value_size: UIntPtr; param_value: pointer; param_value_size_ret: ^UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetDeviceInfo';
    
    {$endregion Device}
    
    {$region Context}
    
    static function CreateContext(var properties: cl_context_properties; num_devices: UInt32; [MarshalAs(UnmanagedType.LPArray)] devices: array of cl_device_id; pfn_notify: CreateContext_Callback; user_data: pointer; var ec: ErrorCode): cl_context;
    external 'opencl.dll' name 'clCreateContext';
    static function CreateContext(properties: ^cl_context_properties; num_devices: UInt32; devices: ^cl_device_id; pfn_notify: CreateContext_Callback; user_data: pointer; ec: ^ErrorCode): cl_context;
    external 'opencl.dll' name 'clCreateContext';
    
    static function CreateContextFromType(var properties: cl_context_properties; device_type: DeviceType; pfn_notify: CreateContext_Callback; user_data: pointer; var ec: ErrorCode): cl_context;
    external 'opencl.dll' name 'clCreateContext';
    static function CreateContextFromType(properties: ^cl_context_properties; device_type: DeviceType; pfn_notify: CreateContext_Callback; user_data: pointer; ec: ^ErrorCode): cl_context;
    external 'opencl.dll' name 'clCreateContext';
    
    static function RetainContext(context: cl_context): ErrorCode;
    external 'opencl.dll' name 'clRetainContext';
    
    static function ReleaseContext(context: cl_context): ErrorCode;
    external 'opencl.dll' name 'clReleaseContext';
    
    static function GetContextInfo(context: cl_context; param_name: cl_context_info; param_value_size: UIntPtr; param_value: pointer; var param_value_size_ret: UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetContextInfo';
    static function GetContextInfo(context: cl_context; param_name: cl_context_info; param_value_size: UIntPtr; param_value: pointer; param_value_size_ret: ^UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetContextInfo';
    
    {$endregion Context}
    
    {$region CommandQueue}
    
    ///Эта функция устарела
    static function CreateCommandQueue(context: cl_context; device: cl_device_id; properties: CommandQueueProperties; var errcode_ret: ErrorCode): cl_command_queue;
    external 'opencl.dll' name 'clCreateCommandQueue';
    ///Эта функция устарела
    static function CreateCommandQueue(context: cl_context; device: cl_device_id; properties: CommandQueueProperties; errcode_ret: ^ErrorCode): cl_command_queue;
    external 'opencl.dll' name 'clCreateCommandQueue';
    
    static function RetainCommandQueue(command_queue: cl_command_queue): ErrorCode;
    external 'opencl.dll' name 'clRetainCommandQueue';
    
    static function ReleaseCommandQueue(command_queue: cl_command_queue): ErrorCode;
    external 'opencl.dll' name 'clReleaseCommandQueue';
    
    static function GetCommandQueueInfo(command_queue: cl_command_queue; param_name: cl_command_queue_info; param_value_size: UIntPtr; param_value: pointer; var param_value_size_ret: UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetCommandQueueInfo';
    static function GetCommandQueueInfo(command_queue: cl_command_queue; param_name: cl_command_queue_info; param_value_size: UIntPtr; param_value: pointer; param_value_size_ret: ^UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetCommandQueueInfo';
    
    static function Flush(command_queue: cl_command_queue): ErrorCode;
    external 'opencl.dll' name 'clFlush';
    
    static function Finish(command_queue: cl_command_queue): ErrorCode;
    external 'opencl.dll' name 'clFinish';
    
    ///Эта функция устарела
    static function EnqueueTask(command_queue: cl_command_queue; kernel: cl_kernel; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueTask';
    ///Эта функция устарела
    static function EnqueueTask(command_queue: cl_command_queue; kernel: cl_kernel; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueTask';
    
    {$endregion CommandQueue}
    
    {$region cl_mem}
    
    {$region Buffer}
    
    static function CreateBuffer(context: cl_context; flags: MemoryFlags; size: UIntPtr; host_ptr: pointer; var errcode_ret: ErrorCode): cl_mem;
    external 'opencl.dll' name 'clCreateBuffer';
    static function CreateBuffer(context: cl_context; flags: MemoryFlags; size: UIntPtr; host_ptr: pointer; errcode_ret: ^ErrorCode): cl_mem;
    external 'opencl.dll' name 'clCreateBuffer';
    
    static function EnqueueReadBuffer(command_queue: cl_command_queue; buffer: cl_mem; blocking_read: cl_bool; offset: UIntPtr; size: UIntPtr; ptr: pointer; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueReadBuffer';
    static function EnqueueReadBuffer(command_queue: cl_command_queue; buffer: cl_mem; blocking_read: cl_bool; offset: UIntPtr; size: UIntPtr; ptr: pointer; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueReadBuffer';
    
    static function EnqueueWriteBuffer(command_queue: cl_command_queue; buffer: cl_mem; blocking_write: cl_bool; offset: UIntPtr; size: UIntPtr; ptr: ^void; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueWriteBuffer';
    static function EnqueueWriteBuffer(command_queue: cl_command_queue; buffer: cl_mem; blocking_write: cl_bool; offset: UIntPtr; size: UIntPtr; ptr: ^void; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueWriteBuffer';
    
    static function EnqueueCopyBuffer(command_queue: cl_command_queue; src_buffer: cl_mem; dst_buffer: cl_mem; src_offset: UIntPtr; dst_offset: UIntPtr; size: UIntPtr; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueCopyBuffer';
    static function EnqueueCopyBuffer(command_queue: cl_command_queue; src_buffer: cl_mem; dst_buffer: cl_mem; src_offset: UIntPtr; dst_offset: UIntPtr; size: UIntPtr; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueCopyBuffer';
    
    static function EnqueueMapBuffer(command_queue: cl_command_queue; buffer: cl_mem; blocking_map: cl_bool; map_flags: MapFlags; offset: UIntPtr; size: UIntPtr; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event; var errcode_ret: ErrorCode): pointer;
    external 'opencl.dll' name 'clEnqueueMapBuffer';
    static function EnqueueMapBuffer(command_queue: cl_command_queue; buffer: cl_mem; blocking_map: cl_bool; map_flags: MapFlags; offset: UIntPtr; size: UIntPtr; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event; errcode_ret: ^ErrorCode): pointer;
    external 'opencl.dll' name 'clEnqueueMapBuffer';
    
    {$endregion Buffer}
    
    {$region Image}
    
    static function GetSupportedImageFormats(context: cl_context; flags: MemoryFlags; image_type: cl_mem_object_type; num_entries: UInt32; [MarshalAs(UnmanagedType.LPArray)] image_formats: array of cl_image_format; var num_image_formats: UInt32): ErrorCode;
    external 'opencl.dll' name 'clGetSupportedImageFormats';
    static function GetSupportedImageFormats(context: cl_context; flags: MemoryFlags; image_type: cl_mem_object_type; num_entries: UInt32; [MarshalAs(UnmanagedType.LPArray)] image_formats: array of cl_image_format; num_image_formats: ^UInt32): ErrorCode;
    external 'opencl.dll' name 'clGetSupportedImageFormats';
    static function GetSupportedImageFormats(context: cl_context; flags: MemoryFlags; image_type: cl_mem_object_type; num_entries: UInt32; image_formats: ^cl_image_format; num_image_formats: ^UInt32): ErrorCode;
    external 'opencl.dll' name 'clGetSupportedImageFormats';
    
    static function GetImageInfo(image: cl_mem; param_name: cl_image_info; param_value_size: UIntPtr; param_value: pointer; var param_value_size_ret: UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetImageInfo';
    static function GetImageInfo(image: cl_mem; param_name: cl_image_info; param_value_size: UIntPtr; param_value: pointer; param_value_size_ret: ^UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetImageInfo';
    
    static function EnqueueReadImage(command_queue: cl_command_queue; image: cl_mem; blocking_read: cl_bool; [MarshalAs(UnmanagedType.LPArray)] origin: array of UIntPtr; [MarshalAs(UnmanagedType.LPArray)] region: array of UIntPtr; row_pitch: UIntPtr; slice_pitch: UIntPtr; ptr: pointer; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueReadImage';
    static function EnqueueReadImage(command_queue: cl_command_queue; image: cl_mem; blocking_read: cl_bool; origin: ^UIntPtr; region: ^UIntPtr; row_pitch: UIntPtr; slice_pitch: UIntPtr; ptr: pointer; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueReadImage';
    
    static function EnqueueWriteImage(command_queue: cl_command_queue; image: cl_mem; blocking_write: cl_bool; [MarshalAs(UnmanagedType.LPArray)] origin: array of UIntPtr; [MarshalAs(UnmanagedType.LPArray)] region: array of UIntPtr; input_row_pitch: UIntPtr; input_slice_pitch: UIntPtr; ptr: pointer; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueWriteImage';
    static function EnqueueWriteImage(command_queue: cl_command_queue; image: cl_mem; blocking_write: cl_bool; origin: ^UIntPtr; region: ^UIntPtr; input_row_pitch: UIntPtr; input_slice_pitch: UIntPtr; ptr: pointer; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueWriteImage';
    
    static function EnqueueCopyImage(command_queue: cl_command_queue; src_image: cl_mem; dst_image: cl_mem; [MarshalAs(UnmanagedType.LPArray)] src_origin: array of UIntPtr; [MarshalAs(UnmanagedType.LPArray)] region: array of UIntPtr; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueCopyImage';
    static function EnqueueCopyImage(command_queue: cl_command_queue; src_image: cl_mem; dst_image: cl_mem; src_origin: ^UIntPtr; dst_origin: ^UIntPtr; region: ^UIntPtr; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueCopyImage';
    
    static function EnqueueMapImage(command_queue: cl_command_queue; image: cl_mem; blocking_map: cl_bool; map_flags: MapFlags; [MarshalAs(UnmanagedType.LPArray)] origin: array of UIntPtr; [MarshalAs(UnmanagedType.LPArray)] region: array of UIntPtr; var image_row_pitch: UIntPtr; var image_slice_pitch: UIntPtr; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event; var errcode_ret: ErrorCode): pointer;
    external 'opencl.dll' name 'clEnqueueMapImage';
    static function EnqueueMapImage(command_queue: cl_command_queue; image: cl_mem; blocking_map: cl_bool; map_flags: MapFlags; origin: ^UIntPtr; region: ^UIntPtr; image_row_pitch: ^UIntPtr; image_slice_pitch: ^UIntPtr; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event; errcode_ret: ^ErrorCode): pointer;
    external 'opencl.dll' name 'clEnqueueMapImage';
    
    {$endregion Image}
    
    {$region Общее}
    
    static function EnqueueCopyBufferToImage(command_queue: cl_command_queue; src_buffer: cl_mem; dst_image: cl_mem; src_offset: UIntPtr; [MarshalAs(UnmanagedType.LPArray)] dst_origin: array of UIntPtr; [MarshalAs(UnmanagedType.LPArray)] region: array of UIntPtr; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueCopyBufferToImage';
    static function EnqueueCopyBufferToImage(command_queue: cl_command_queue; src_buffer: cl_mem; dst_image: cl_mem; src_offset: UIntPtr; dst_origin: ^UIntPtr; region: ^UIntPtr; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueCopyBufferToImage';
    
    static function EnqueueCopyImageToBuffer(command_queue: cl_command_queue; src_image: cl_mem; dst_buffer: cl_mem; [MarshalAs(UnmanagedType.LPArray)] src_origin: array of UIntPtr; [MarshalAs(UnmanagedType.LPArray)] region: array of UIntPtr; dst_offset: UIntPtr; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueCopyImageToBuffer';
    static function EnqueueCopyImageToBuffer(command_queue: cl_command_queue; src_image: cl_mem; dst_buffer: cl_mem; src_origin: ^UIntPtr; region: ^UIntPtr; dst_offset: UIntPtr; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueCopyImageToBuffer';
    
    static function EnqueueUnmapMemObject(command_queue: cl_command_queue; memobj: cl_mem; mapped_ptr: pointer; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueUnmapMemObject';
    static function EnqueueUnmapMemObject(command_queue: cl_command_queue; memobj: cl_mem; mapped_ptr: pointer; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueUnmapMemObject';
    
    static function RetainMemObject(memobj: cl_mem): ErrorCode;
    external 'opencl.dll' name 'clRetainMemObject';
    
    static function ReleaseMemObject(memobj: cl_mem): ErrorCode;
    external 'opencl.dll' name 'clReleaseMemObject';
    
    static function GetMemObjectInfo(memobj: cl_mem; param_name: cl_mem_info; param_value_size: UIntPtr; param_value: pointer; var param_value_size_ret: UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetMemObjectInfo';
    static function GetMemObjectInfo(memobj: cl_mem; param_name: cl_mem_info; param_value_size: UIntPtr; param_value: pointer; param_value_size_ret: ^UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetMemObjectInfo';
    
    {$endregion Общее}
    
    {$endregion cl_mem}
    
    {$region Sampler}
    
    ///Эта функция устарела
    static function CreateSampler(context: cl_context; normalized_coords: cl_bool; addressing_mode: cl_addressing_mode; filter_mode: cl_filter_mode; var errcode_ret: ErrorCode): cl_sampler;
    external 'opencl.dll' name 'clCreateSampler';
    ///Эта функция устарела
    static function CreateSampler(context: cl_context; normalized_coords: cl_bool; addressing_mode: cl_addressing_mode; filter_mode: cl_filter_mode; errcode_ret: ^ErrorCode): cl_sampler;
    external 'opencl.dll' name 'clCreateSampler';
    
    static function RetainSampler(sampler: cl_sampler): ErrorCode;
    external 'opencl.dll' name 'clRetainSampler';
    
    static function ReleaseSampler(sampler: cl_sampler): ErrorCode;
    external 'opencl.dll' name 'clReleaseSampler';
    
    static function GetSamplerInfo(sampler: cl_sampler; param_name: cl_sampler_info; param_value_size: UIntPtr; param_value: pointer; var param_value_size_ret: UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetSamplerInfo';
    static function GetSamplerInfo(sampler: cl_sampler; param_name: cl_sampler_info; param_value_size: UIntPtr; param_value: pointer; param_value_size_ret: ^UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetSamplerInfo';
    
    {$endregion Sampler}
    
    {$region Program}
    
    static function CreateProgramWithSource(context: cl_context; count: UInt32; [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] strings: array of string; [MarshalAs(UnmanagedType.LPArray)] lengths: array of UIntPtr; var errcode_ret: ErrorCode): cl_program;
    external 'opencl.dll' name 'clCreateProgramWithSource';
    static function CreateProgramWithSource(context: cl_context; count: UInt32; strings: ^^char; lengths: ^UIntPtr; errcode_ret: ^ErrorCode): cl_program;
    external 'opencl.dll' name 'clCreateProgramWithSource';
    
    static function CreateProgramWithBinary(context: cl_context; num_devices: UInt32; [MarshalAs(UnmanagedType.LPArray)] device_list: array of cl_device_id; [MarshalAs(UnmanagedType.LPArray)] lengths: array of UIntPtr; [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPArray)] binaries: array of array of byte; [MarshalAs(UnmanagedType.LPArray)] binary_status: array of ErrorCode; var errcode_ret: ErrorCode): cl_program;
    external 'opencl.dll' name 'clCreateProgramWithBinary';
    static function CreateProgramWithBinary(context: cl_context; num_devices: UInt32; device_list: ^cl_device_id; lengths: ^UIntPtr; binaries: ^^byte; binary_status: ^ErrorCode; errcode_ret: ^ErrorCode): cl_program;
    external 'opencl.dll' name 'clCreateProgramWithBinary';
    
    static function RetainProgram(&program: cl_program): ErrorCode;
    external 'opencl.dll' name 'clRetainProgram';
    
    static function ReleaseProgram(&program: cl_program): ErrorCode;
    external 'opencl.dll' name 'clReleaseProgram';
    
    static function BuildProgram(&program: cl_program; num_devices: UInt32; [MarshalAs(UnmanagedType.LPArray)] device_list: array of cl_device_id; [MarshalAs(UnmanagedType.LPStr)] options: string; pfn_notify: BuildProgram_Callback; user_data: pointer): ErrorCode;
    external 'opencl.dll' name 'clBuildProgram';
    static function BuildProgram(&program: cl_program; num_devices: UInt32; device_list: ^cl_device_id; options: ^char; pfn_notify: BuildProgram_Callback; user_data: pointer): ErrorCode;
    external 'opencl.dll' name 'clBuildProgram';
    
    static function GetProgramInfo(&program: cl_program; param_name: cl_program_info; param_value_size: UIntPtr; param_value: pointer; var param_value_size_ret: UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetProgramInfo';
    static function GetProgramInfo(&program: cl_program; param_name: cl_program_info; param_value_size: UIntPtr; param_value: pointer; param_value_size_ret: ^UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetProgramInfo';
    
    static function GetProgramBuildInfo(&program: cl_program; device: cl_device_id; param_name: cl_program_build_info; param_value_size: UIntPtr; param_value: pointer; var param_value_size_ret: UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetProgramBuildInfo';
    static function GetProgramBuildInfo(&program: cl_program; device: cl_device_id; param_name: cl_program_build_info; param_value_size: UIntPtr; param_value: pointer; param_value_size_ret: ^UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetProgramBuildInfo';
    
    {$endregion Program}
    
    {$region Kernel}
    
    static function CreateKernel(&program: cl_program; [MarshalAs(UnmanagedType.LPStr)] kernel_name: string; var errcode_ret: ErrorCode): cl_kernel;
    external 'opencl.dll' name 'clCreateKernel';
    static function CreateKernel(&program: cl_program; kernel_name: ^char; errcode_ret: ^ErrorCode): cl_kernel;
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
    
    static function GetKernelInfo(kernel: cl_kernel; param_name: cl_kernel_info; param_value_size: UIntPtr; param_value: pointer; var param_value_size_ret: UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetKernelInfo';
    static function GetKernelInfo(kernel: cl_kernel; param_name: cl_kernel_info; param_value_size: UIntPtr; param_value: pointer; param_value_size_ret: ^UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetKernelInfo';
    
    static function GetKernelWorkGroupInfo(kernel: cl_kernel; device: cl_device_id; param_name: cl_kernel_work_group_info; param_value_size: UIntPtr; param_value: pointer; var param_value_size_ret: UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetKernelWorkGroupInfo';
    static function GetKernelWorkGroupInfo(kernel: cl_kernel; device: cl_device_id; param_name: cl_kernel_work_group_info; param_value_size: UIntPtr; param_value: pointer; param_value_size_ret: ^UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetKernelWorkGroupInfo';
    
    static function EnqueueNDRangeKernel(command_queue: cl_command_queue; kernel: cl_kernel; work_dim: UInt32; [MarshalAs(UnmanagedType.LPArray)] global_work_offset: array of UIntPtr; [MarshalAs(UnmanagedType.LPArray)] global_work_size: array of UIntPtr; [MarshalAs(UnmanagedType.LPArray)] local_work_size: array of UIntPtr; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueNDRangeKernel';
    static function EnqueueNDRangeKernel(command_queue: cl_command_queue; kernel: cl_kernel; work_dim: UInt32; [MarshalAs(UnmanagedType.LPArray)] global_work_offset: array of UIntPtr; [MarshalAs(UnmanagedType.LPArray)] global_work_size: array of UIntPtr; [MarshalAs(UnmanagedType.LPArray)] local_work_size: array of UIntPtr; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueNDRangeKernel';
    static function EnqueueNDRangeKernel(command_queue: cl_command_queue; kernel: cl_kernel; work_dim: UInt32; global_work_offset: ^UIntPtr; global_work_size: ^UIntPtr; local_work_size: ^UIntPtr; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueNDRangeKernel';
    
    static function EnqueueNativeKernel(command_queue: cl_command_queue; user_func: EnqueueNativeKernel_Callback; args: pointer; cb_args: UIntPtr; num_mem_objects: UInt32; [MarshalAs(UnmanagedType.LPArray)] mem_list: array of cl_mem; args_mem_loc: ^pointer; num_events_in_wait_list: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_wait_list: array of cl_event; var &event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueNativeKernel';
    static function EnqueueNativeKernel(command_queue: cl_command_queue; user_func: EnqueueNativeKernel_Callback; args: pointer; cb_args: UIntPtr; num_mem_objects: UInt32; mem_list: ^cl_mem; args_mem_loc: ^pointer; num_events_in_wait_list: UInt32; event_wait_list: ^cl_event; &event: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clEnqueueNativeKernel';
    
    {$endregion Kernel}
    
    {$region Event}
    
    static function WaitForEvents(num_events: UInt32; [MarshalAs(UnmanagedType.LPArray)] event_list: array of cl_event): ErrorCode;
    external 'opencl.dll' name 'clWaitForEvents';
    static function WaitForEvents(num_events: UInt32; event_list: ^cl_event): ErrorCode;
    external 'opencl.dll' name 'clWaitForEvents';
    
    static function GetEventInfo(&event: cl_event; param_name: cl_event_info; param_value_size: UIntPtr; param_value: pointer; var param_value_size_ret: UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetEventInfo';
    static function GetEventInfo(&event: cl_event; param_name: cl_event_info; param_value_size: UIntPtr; param_value: pointer; param_value_size_ret: ^UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetEventInfo';
    
    static function RetainEvent(&event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clRetainEvent';
    
    static function ReleaseEvent(&event: cl_event): ErrorCode;
    external 'opencl.dll' name 'clReleaseEvent';
    
    static function GetEventProfilingInfo(&event: cl_event; param_name: cl_profiling_info; param_value_size: UIntPtr; param_value: pointer; var param_value_size_ret: UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetEventProfilingInfo';
    static function GetEventProfilingInfo(&event: cl_event; param_name: cl_profiling_info; param_value_size: UIntPtr; param_value: pointer; param_value_size_ret: ^UIntPtr): ErrorCode;
    external 'opencl.dll' name 'clGetEventProfilingInfo';
    
    {$endregion Event}
    
  end;

implementation



end.