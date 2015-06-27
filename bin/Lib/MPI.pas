// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
unit MPI;

interface

const
  MPI_IDENT = 0;
  MPI_CONGRUENT = 1;
  MPI_SIMILAR = 2;
  MPI_UNEQUAL = 3;

  MPI_COMM_WORLD = 91;
  MPI_COMM_SELF = 92;
  MPI_GROUP_EMPTY = 90;

  MPI_TAG_UB = 81;
  MPI_HOST = 83;
  MPI_IO = 85;
  MPI_WTIME_IS_GLOBAL = 87;

  MPI_MAX_PROCESSOR_NAME = 256;
  MPI_MAX_ERROR_STRING = 512;
  MPI_MAX_NAME_STRING = 63;
  MPI_UNDEFINED = -32766;
  MPI_UNDEFINED_RANK = MPI_UNDEFINED;
  MPI_BSEND_OVERHEAD = 512;
  MPI_GRAPH = 1;
  MPI_CART = 2;

  MPI_PROC_NULL = -1;
  MPI_ANY_SOURCE = -2;
  MPI_ROOT = -3;
  MPI_ANY_TAG = -1;

  MPI_CHAR = 1;
  MPI_UNSIGNED_CHAR = 2;
  MPI_BYTE = 3;
  MPI_SHORT = 4;
  MPI_UNSIGNED_SHORT = 5;
  MPI_INT = 6;
  MPI_UNSIGNED = 7;
  MPI_LONG = 8;
  MPI_UNSIGNED_LONG = 9;
  MPI_FLOAT = 10;
  MPI_DOUBLE = 11;
  MPI_LONG_DOUBLE = 12;
  MPI_LONG_LONG_INT = 13;
  MPI_LONG_LONG = 13;
  MPI_PACKED = 14;

  MPI_LB = 15;
  MPI_UB = 16;

  MPI_FLOAT_INT = 17;
  MPI_DOUBLE_INT = 18;
  MPI_LONG_INT = 19;
  MPI_SHORT_INT = 20;
  MPI_2INT = 21;
  MPI_LONG_DOUBLE_INT = 22;

  MPI_MAX = 100;
  MPI_MIN = 101;
  MPI_SUM = 102;
  MPI_PROD = 103;
  MPI_LAND = 104;
  MPI_BAND = 105;
  MPI_LOR = 106;
  MPI_BOR = 107;
  MPI_LXOR = 108;
  MPI_BXOR = 109;
  MPI_MINLOC = 110;
  MPI_MAXLOC = 111;

  MPI_COMM_NULL = 0;
  MPI_OP_NULL = 0;
  MPI_GROUP_NULL = 0;
  MPI_DATATYPE_NULL = 0;
  MPI_REQUEST_NULL = 0;
  MPI_BOTTOM = pointer(0);

  MPI_ERRORS_ARE_FATAL = 119;
  MPI_ERRORS_RETURN = 120;

  MPI_SUCCESS = 0;
  MPI_ERR_BUFFER = 1;
  MPI_ERR_COUNT = 2;
  MPI_ERR_TYPE = 3;
  MPI_ERR_TAG = 4;
  MPI_ERR_COMM = 5;
  MPI_ERR_RANK = 6;
  MPI_ERR_ROOT = 7;
  MPI_ERR_TRUNCATE = 14;
  MPI_ERR_GROUP = 8;
  MPI_ERR_OP = 9;
  MPI_ERR_REQUEST = 19;
  MPI_ERR_TOPOLOGY = 10;
  MPI_ERR_DIMS = 11;
  MPI_ERR_ARG = 12;
  MPI_ERR_OTHER = 15;
  MPI_ERR_UNKNOWN = 13;
  MPI_ERR_INTERN = 16;
  MPI_ERR_IN_STATUS = 17;
  MPI_ERR_PENDING = 18;
  MPI_ERR_FILE = 27;
  MPI_ERR_ACCESS = 20;
  MPI_ERR_AMODE = 21;
  MPI_ERR_BAD_FILE = 22;
  MPI_ERR_FILE_EXISTS = 25;
  MPI_ERR_FILE_IN_USE = 26;
  MPI_ERR_NO_SPACE = 36;
  MPI_ERR_NO_SUCH_FILE = 37;
  MPI_ERR_IO = 32;
  MPI_ERR_READ_ONLY = 40;
  MPI_ERR_CONVERSION = 23;
  MPI_ERR_DUP_DATAREP = 24;
  MPI_ERR_UNSUPPORTED_DATAREP = 43;
  MPI_ERR_INFO = 28;
  MPI_ERR_INFO_KEY = 29;
  MPI_ERR_INFO_VALUE = 30;
  MPI_ERR_INFO_NOKEY = 31;
  MPI_ERR_NAME = 33;
  MPI_ERR_NO_MEM = 34;
  MPI_ERR_NOT_SAME = 35;
  MPI_ERR_PORT = 38;
  MPI_ERR_QUOTA = 39;
  MPI_ERR_SERVICE = 41;
  MPI_ERR_SPAWN = 42;
  MPI_ERR_UNSUPPORTED_OPERATION = 44;
  MPI_ERR_WIN = 45;
  MPI_ERR_LASTCODE = $3FFFFFFF;

type
  
  PPChar = ^PChar;

  MPI_Datatype = 0..MaxInt;
  MPI_Request = 0..MaxInt;
  MPI_Comm = 0..MaxInt;
  MPI_Group = 0..MaxInt;
  MPI_Op = 0..MaxInt;
  MPI_Aint = -MaxInt..MaxInt;
  MPI_Errhandler = 0..MaxInt;

  MPI_Handler_function = procedure(var comm: MPI_Comm; var errcode: longint);
  MPI_User_function = procedure(invec: pointer; inoutvec: pointer; var len: longint; var datatype: MPI_Datatype);

  MPI_Status = record
    count: longint;
    MPI_SOURCE: longint;
    MPI_TAG: longint;
    MPI_ERROR: longint;
  end;

  MPI_Triplet_range = record
    first, last, step : longint;
  end;

function MPI_Init(var argc: longint; var argv: PPChar): longint;
function MPI_Finalize(): longint;
function MPI_Comm_size(comm: MPI_Comm; var size: longint): longint;
function MPI_Comm_rank(comm: MPI_Comm; var rank: longint): longint;
function MPI_Wtime(): double;
function MPI_Wtick(): double;
function MPI_Get_processor_name(name: PChar; var len: longint): longint;
function MPI_Initialized(var flag: longint): longint;
function MPI_Send(buf: pointer; count: longint; datatype: MPI_Datatype; dest: longint; msgtag: longint; comm: MPI_Comm): longint;
function MPI_Bsend(buf: pointer; count: longint; datatype: MPI_Datatype; dest: longint; msgtag: longint; comm: MPI_Comm): longint;
function MPI_Ssend(buf: pointer; count: longint; datatype: MPI_Datatype; dest: longint; msgtag: longint; comm: MPI_Comm): longint;
function MPI_Rsend(buf: pointer; count: longint; datatype: MPI_Datatype; dest: longint; msgtag: longint; comm: MPI_Comm): longint;
function MPI_Recv(buf: pointer; count: longint; datatype: MPI_Datatype; source: longint; msgtag: longint; comm: MPI_Comm; var status: MPI_Status): longint;
function MPI_Get_count(var status: MPI_Status; datatype: MPI_Datatype; var count: longint): longint;
function MPI_Probe(source: longint; msgtag: longint; comm: MPI_Comm; var status: MPI_Status): longint;
function MPI_Buffer_attach(buf: pointer; size: longint): longint;
function MPI_Buffer_detach(buf: pointer; var size: longint): longint;
function MPI_Isend(buf: pointer; count: longint; datatype: MPI_Datatype; dest: longint; msgtag: longint; comm: MPI_Comm; var request: MPI_Request): longint;
function MPI_Ibsend(buf: pointer; count: longint; datatype: MPI_Datatype; dest: longint; msgtag: longint; comm: MPI_Comm; var request: MPI_Request): longint;
function MPI_Issend(buf: pointer; count: longint; datatype: MPI_Datatype; dest: longint; msgtag: longint; comm: MPI_Comm; var request: MPI_Request): longint;
function MPI_Irsend(buf: pointer; count: longint; datatype: MPI_Datatype; dest: longint; msgtag: longint; comm: MPI_Comm; var request: MPI_Request): longint;
function MPI_Irecv(buf: pointer; count: longint; datatype: MPI_Datatype; source: longint; msgtag: longint; comm: MPI_Comm; var request: MPI_Request): longint;
function MPI_Wait(var request: MPI_Request; var status: MPI_Status): longint;
function MPI_Waitall(count: longint; var requests: array of MPI_Request; var statuses: array of MPI_Status): longint;
function MPI_Waitany(count: longint; var requests: array of MPI_Request; var index: longint; var status: MPI_Status): longint;
function MPI_Waitsome(incount: longint; var requests: array of MPI_Request; var outcount: longint; var indexes: array of longint; var statuses: array of MPI_Status): longint;
function MPI_Test(var request: MPI_Request; var flag: longint; var status: MPI_Status): longint;
function MPI_Testall(count: longint; var requests: array of MPI_Request; var flag: longint; var statuses: array of MPI_Status): longint;
function MPI_Testany(count: longint; var requests: array of MPI_Request; var index: longint; var flag: longint; var status: MPI_Status): longint;
function MPI_Testsome(incount: longint; var requests: array of MPI_Request; var outcount: longint; var indexes: array of longint; var statuses: array of MPI_Status): longint;
function MPI_Iprobe(source: longint; msgtag: longint; comm: MPI_Comm; var flag: longint; var status: MPI_Status): longint;
function MPI_Send_init(buf: pointer; count: longint; datatype: MPI_Datatype; dest: longint; msgtag: longint; comm: MPI_Comm; var request: MPI_Request): longint;
function MPI_Bsend_init(buf: pointer; count: longint; datatype: MPI_Datatype; dest: longint; msgtag: longint; comm: MPI_Comm; var request: MPI_Request): longint;
function MPI_Ssend_init(buf: pointer; count: longint; datatype: MPI_Datatype; dest: longint; msgtag: longint; comm: MPI_Comm; var request: MPI_Request): longint;
function MPI_Rsend_init(buf: pointer; count: longint; datatype: MPI_Datatype; dest: longint; msgtag: longint; comm: MPI_Comm; var request: MPI_Request): longint;
function MPI_Recv_init(buf: pointer; count: longint; datatype: MPI_Datatype; source: longint; msgtag: longint; comm: MPI_Comm; var request: MPI_Request): longint;
function MPI_Start(var request: MPI_Request): longint;
function MPI_Startall(count: longint; var requests: array of MPI_Request): longint;
function MPI_Request_free(var request: MPI_Request): longint;
function MPI_Sendrecv(sbuf: pointer; scount: longint; stype: MPI_Datatype; dest: longint; stag: longint; rbuf: pointer; rcount: longint; rtype: MPI_Datatype; source: longint; rtag: longint; comm: MPI_Comm; var status: MPI_Status): longint;
function MPI_Sendrecv_replace(buf: pointer; count: longint; datatype: MPI_Datatype; dest: longint; stag: longint; source: longint; rtag: longint; comm: MPI_Comm; var status: MPI_Status): longint;
function MPI_Barrier(comm: MPI_Comm): longint;
function MPI_Bcast(buf: pointer; count: longint; datatype: MPI_Datatype; root: longint; comm: MPI_Comm): longint;
function MPI_Gather(sbuf: pointer; scount: longint; stype: MPI_Datatype; rbuf: pointer; rcount: longint; rtype: MPI_Datatype; root: longint; comm: MPI_Comm): longint;
function MPI_Gatherv(sbuf: pointer; scount: longint; stype: MPI_Datatype; rbuf: pointer; const rcounts: array of longint; const displs: array of longint; rtype: MPI_Datatype; root: longint; comm: MPI_Comm): longint;
function MPI_Scatter(sbuf: pointer; scount: longint; stype: MPI_Datatype; rbuf: pointer; rcount: longint; rtype: MPI_Datatype; root: longint; comm: MPI_Comm): longint;
function MPI_Scatterv(sbuf: pointer; const scounts: array of longint; const displs: array of longint; stype: MPI_Datatype; rbuf: pointer; rcount: longint; rtype: MPI_Datatype; root: longint; comm: MPI_Comm): longint;
function MPI_Allgather(sbuf: pointer; scount: longint; stype: MPI_Datatype; rbuf: pointer; rcount: longint; rtype: MPI_Datatype; comm: MPI_Comm): longint;
function MPI_Allgatherv(sbuf: pointer; scount: longint; stype: MPI_Datatype; rbuf: pointer; const rcounts: array of longint; const displs: array of longint; rtype: MPI_Datatype; comm: MPI_Comm): longint;
function MPI_Alltoall(sbuf: pointer; scount: longint; stype: MPI_Datatype; rbuf: pointer; rcount: longint; rtype: MPI_Datatype; comm: MPI_Comm): longint;
function MPI_Alltoallv(sbuf: pointer; const scounts: array of longint; const sdispls: array of longint; stype: MPI_Datatype; rbuf: pointer; const rcounts: array of longint; const rdispls: array of longint; rtype: MPI_Datatype; comm: MPI_Comm): longint;
function MPI_Reduce(sbuf: pointer; rbuf: pointer; count: longint; datatype: MPI_Datatype; op: MPI_Op; root: longint; comm: MPI_Comm): longint;
function MPI_Allreduce(sbuf: pointer; rbuf: pointer; count: longint; datatype: MPI_Datatype; op: MPI_Op; comm: MPI_Comm): longint;
function MPI_Reduce_scatter(sbuf: pointer; rbuf: pointer; const rcounts: array of longint; datatype: MPI_Datatype; op: MPI_Op; comm: MPI_Comm): longint;
function MPI_Scan(sbuf: pointer; rbuf: pointer; count: longint; datatype: MPI_Datatype; op: MPI_Op; comm: MPI_Comm): longint;
function MPI_Op_create(var func: MPI_User_function; commute: longint; var op: MPI_Op): longint;
function MPI_Op_free(var op: MPI_Op): longint;
function MPI_Type_contiguous(count: longint; oldtype: MPI_Datatype; var newtype: MPI_Datatype): longint;
function MPI_Type_vector(count: longint; blocklen: longint; stride: longint; oldtype: MPI_Datatype; var newtype: MPI_Datatype): longint;
function MPI_Type_hvector(count: longint; blocklen: longint; stride: MPI_Aint; oldtype: MPI_Datatype; var newtype: MPI_Datatype): longint;
function MPI_Type_indexed(count: longint; const blocklens: array of longint; const displs: array of longint; oldtype: MPI_Datatype; var newtype: MPI_Datatype): longint;
function MPI_Type_hindexed(count: longint; const blocklens: array of longint; const displs: array of MPI_Aint; oldtype: MPI_Datatype; var newtype: MPI_Datatype): longint;
function MPI_Type_struct(count: longint; const blocklens: array of longint; const displs: array of MPI_Aint; const oldtypes: array of MPI_Datatype; var newtype: MPI_Datatype): longint;
function MPI_Type_extent(datatype: MPI_Datatype; var extent: MPI_Aint): longint;
function MPI_Type_size(datatype: MPI_Datatype; var size: longint): longint;
function MPI_Type_commit(var datatype: MPI_Datatype): longint;
function MPI_Type_free(var datatype: MPI_Datatype): longint;
function MPI_Pack(inbuf: pointer; incount: longint; datatype: MPI_Datatype; outbuf: pointer; outsize: longint; var position: longint; comm: MPI_Comm): longint;
function MPI_Unpack(inbuf: pointer; insize: longint; var position: longint; outbuf: pointer; outcount: longint; datatype: MPI_Datatype; comm: MPI_Comm): longint;
function MPI_Pack_size(incount: longint; datatype: MPI_Datatype; comm: MPI_Comm; var size: longint): longint;
function MPI_Group_size(group: MPI_Group; var size: longint): longint;
function MPI_Group_rank(group: MPI_Group; var rank: longint): longint;
function MPI_Group_translate_ranks(group1: MPI_Group; n: longint; const ranks1: array of longint; group2: MPI_Group; var ranks2: array of longint): longint;
function MPI_Group_compare(group1: MPI_Group; group2: MPI_Group; var res: longint): longint;
function MPI_Comm_group(comm: MPI_Comm; var group: MPI_Group): longint;
function MPI_Group_union(group1: MPI_Group; group2: MPI_Group; var newgroup: MPI_Group): longint;
function MPI_Group_intersection(group1: MPI_Group; group2: MPI_Group; var newgroup: MPI_Group): longint;
function MPI_Group_difference(group1: MPI_Group; group2: MPI_Group; var newgroup: MPI_Group): longint;
function MPI_Group_incl(group: MPI_Group; n: longint; const ranks: array of longint; var newgroup: MPI_Group): longint;
function MPI_Group_excl(group: MPI_Group; n: longint; const ranks: array of longint; var newgroup: MPI_Group): longint;
function MPI_Group_free(var group: MPI_Group): longint;
function MPI_Comm_compare(comm1: MPI_Comm; comm2: MPI_Comm; var res: longint): longint;
function MPI_Comm_dup(comm: MPI_Comm; var newcomm: MPI_Comm): longint;
function MPI_Comm_create(comm: MPI_Comm; group: MPI_Group; var newcomm: MPI_Comm): longint;
function MPI_Comm_split(comm: MPI_Comm; color: longint; key: longint; var newcomm: MPI_Comm): longint;
function MPI_Comm_free(comm: MPI_Comm): longint;
function MPI_Topo_test(comm: MPI_Comm; var status: longint): longint;
function MPI_Cart_create(comm_old: MPI_Comm; ndims: longint; const dims: array of longint; const periods: array of longint; reorder: longint; var comm_cart: MPI_Comm): longint;
function MPI_Dims_create(nnodes: longint; ndims: longint; var dims: array of longint): longint;
function MPI_Graph_create(comm_old: MPI_Comm; nnodes: longint; const index: array of longint; const edges: array of longint; reorder: longint; var comm_graph: MPI_Comm): longint;
function MPI_Graphdims_get(comm: MPI_Comm; var nnodes: longint; var nedges: longint): longint;
function MPI_Graph_get(comm: MPI_Comm; maxindex: longint; maxedges: longint; var index: array of longint; var edges: array of longint): longint;
function MPI_Cartdim_get(comm: MPI_Comm; var ndims: longint): longint;
function MPI_Cart_get(comm: MPI_Comm; maxdims: longint; var dims: array of longint; var periods: array of longint; var coords: array of longint): longint;
function MPI_Cart_rank(comm: MPI_Comm; const coords: array of longint; var rank: longint): longint;
function MPI_Cart_coords(comm: MPI_Comm; rank: longint; maxdims: longint; var coords: array of longint): longint;
function MPI_Graph_neighbors_count(comm: MPI_Comm; rank: longint; var nneighbors: longint): longint;
function MPI_Graph_neighbors(comm: MPI_Comm; rank: longint; maxneighbors: longint; var neighbors: array of longint): longint;
function MPI_Cart_shift(comm: MPI_Comm; direction: longint; disp: longint; var rank_source: longint; var rank_dest: longint): longint;
function MPI_Cart_sub(comm: MPI_Comm; const remain_dims: array of longint; var newcomm: MPI_Comm): longint;
function MPI_Comm_test_inter(comm: MPI_Comm; var flag: longint): longint;
function MPI_Comm_remote_size(comm: MPI_Comm; var size: longint): longint;
function MPI_Comm_remote_group(comm: MPI_Comm; var group: MPI_Group): longint;
function MPI_Intercomm_create(local_comm: MPI_Comm; local_leader: longint; peer_comm: MPI_Comm; remote_leader: longint; tag: longint; var newintercomm: MPI_Comm): longint;
function MPI_Intercomm_merge(intercomm: MPI_Comm; high: longint; var newintracomm: MPI_Comm): longint;
function MPI_Abort(comm: MPI_Comm; errorcode: longint): longint;
function MPI_Address(location: pointer; var address: MPI_Aint): longint;
function MPI_Cancel(var request: MPI_Request): longint;
function MPI_Cart_map(comm: MPI_Comm; ndims: longint; const dims: array of longint; const periods: array of longint; var newrank: longint): longint;
function MPI_Get_elements(var status: MPI_Status; datatype: MPI_Datatype; var count: longint): longint;
function MPI_Graph_map(comm: MPI_Comm; nnodes: longint; const index: array of longint; const edges: array of longint; var newrank: longint): longint;
function MPI_Group_range_incl(group: MPI_Group; n: longint; const ranges: array of MPI_Triplet_range; var newgroup: MPI_Group): longint;
function MPI_Group_range_excl(group: MPI_Group; n: longint; const ranges: array of MPI_Triplet_range; var newgroup: MPI_Group): longint;
function MPI_Test_cancelled(var status: MPI_Status; var flag: longint): longint;
function MPI_Type_lb(datatype: MPI_Datatype; var displacement: MPI_Aint): longint;
function MPI_Type_ub(datatype: MPI_Datatype; var displacement: MPI_Aint): longint;
function MPI_Errhandler_create(errfunction: MPI_Handler_function; var errhandler: MPI_Errhandler): longint;
function MPI_Errhandler_set(comm: MPI_Comm; errhandler: MPI_Errhandler): longint;
function MPI_Errhandler_get(comm: MPI_Comm; var errhandler: MPI_Errhandler): longint;
function MPI_Errhandler_free(var errhandler: MPI_Errhandler): longint;
function MPI_Error_string(errcode: longint; errstring: PChar; var strlen: longint): longint;
function MPI_Error_class(errcode: longint; var errclass: longint): longint;

implementation

function MPI_Init(var argc: longint; var argv: PPChar): longint; external 'mpich.dll' name 'MPI_Init';
function MPI_Finalize(): longint; external 'mpich.dll' name 'MPI_Finalize';
function MPI_Comm_size(comm: MPI_Comm; var size: longint): longint; external 'mpich.dll' name 'MPI_Comm_size';
function MPI_Comm_rank(comm: MPI_Comm; var rank: longint): longint; external 'mpich.dll' name 'MPI_Comm_rank';
function MPI_Wtime(): double; external 'mpich.dll' name 'MPI_Wtime';
function MPI_Wtick(): double; external 'mpich.dll' name 'MPI_Wtick';
function MPI_Get_processor_name(name: PChar; var len: longint): longint; external 'mpich.dll' name 'MPI_Get_processor_name';
function MPI_Initialized(var flag: longint): longint; external 'mpich.dll' name 'MPI_Initialized';
function MPI_Send(buf: pointer; count: longint; datatype: MPI_Datatype; dest: longint; msgtag: longint; comm: MPI_Comm): longint; external 'mpich.dll' name 'MPI_Send';
function MPI_Bsend(buf: pointer; count: longint; datatype: MPI_Datatype; dest: longint; msgtag: longint; comm: MPI_Comm): longint; external 'mpich.dll' name 'MPI_Bsend';
function MPI_Ssend(buf: pointer; count: longint; datatype: MPI_Datatype; dest: longint; msgtag: longint; comm: MPI_Comm): longint; external 'mpich.dll' name 'MPI_Ssend';
function MPI_Rsend(buf: pointer; count: longint; datatype: MPI_Datatype; dest: longint; msgtag: longint; comm: MPI_Comm): longint; external 'mpich.dll' name 'MPI_Rsend';
function MPI_Recv(buf: pointer; count: longint; datatype: MPI_Datatype; source: longint; msgtag: longint; comm: MPI_Comm; var status: MPI_Status): longint; external 'mpich.dll' name 'MPI_Recv';
function MPI_Get_count(var status: MPI_Status; datatype: MPI_Datatype; var count: longint): longint; external 'mpich.dll' name 'MPI_Get_count';
function MPI_Probe(source: longint; msgtag: longint; comm: MPI_Comm; var status: MPI_Status): longint; external 'mpich.dll' name 'MPI_Probe';
function MPI_Buffer_attach(buf: pointer; size: longint): longint; external 'mpich.dll' name 'MPI_Buffer_attach';
function MPI_Buffer_detach(buf: pointer; var size: longint): longint; external 'mpich.dll' name 'MPI_Buffer_detach';
function MPI_Isend(buf: pointer; count: longint; datatype: MPI_Datatype; dest: longint; msgtag: longint; comm: MPI_Comm; var request: MPI_Request): longint; external 'mpich.dll' name 'MPI_Isend';
function MPI_Ibsend(buf: pointer; count: longint; datatype: MPI_Datatype; dest: longint; msgtag: longint; comm: MPI_Comm; var request: MPI_Request): longint; external 'mpich.dll' name 'MPI_Ibsend';
function MPI_Issend(buf: pointer; count: longint; datatype: MPI_Datatype; dest: longint; msgtag: longint; comm: MPI_Comm; var request: MPI_Request): longint; external 'mpich.dll' name 'MPI_Issend';
function MPI_Irsend(buf: pointer; count: longint; datatype: MPI_Datatype; dest: longint; msgtag: longint; comm: MPI_Comm; var request: MPI_Request): longint; external 'mpich.dll' name 'MPI_Irsend';
function MPI_Irecv(buf: pointer; count: longint; datatype: MPI_Datatype; source: longint; msgtag: longint; comm: MPI_Comm; var request: MPI_Request): longint; external 'mpich.dll' name 'MPI_Irecv';
function MPI_Wait(var request: MPI_Request; var status: MPI_Status): longint; external 'mpich.dll' name 'MPI_Wait';
function MPI_Test(var request: MPI_Request; var flag: longint; var status: MPI_Status): longint; external 'mpich.dll' name 'MPI_Test';
function MPI_Iprobe(source: longint; msgtag: longint; comm: MPI_Comm; var flag: longint; var status: MPI_Status): longint; external 'mpich.dll' name 'MPI_Iprobe';
function MPI_Send_init(buf: pointer; count: longint; datatype: MPI_Datatype; dest: longint; msgtag: longint; comm: MPI_Comm; var request: MPI_Request): longint; external 'mpich.dll' name 'MPI_Send_init';
function MPI_Bsend_init(buf: pointer; count: longint; datatype: MPI_Datatype; dest: longint; msgtag: longint; comm: MPI_Comm; var request: MPI_Request): longint; external 'mpich.dll' name 'MPI_Bsend_init';
function MPI_Ssend_init(buf: pointer; count: longint; datatype: MPI_Datatype; dest: longint; msgtag: longint; comm: MPI_Comm; var request: MPI_Request): longint; external 'mpich.dll' name 'MPI_Ssend_init';
function MPI_Rsend_init(buf: pointer; count: longint; datatype: MPI_Datatype; dest: longint; msgtag: longint; comm: MPI_Comm; var request: MPI_Request): longint; external 'mpich.dll' name 'MPI_Rsend_init';
function MPI_Recv_init(buf: pointer; count: longint; datatype: MPI_Datatype; source: longint; msgtag: longint; comm: MPI_Comm; var request: MPI_Request): longint; external 'mpich.dll' name 'MPI_Recv_init';
function MPI_Start(var request: MPI_Request): longint; external 'mpich.dll' name 'MPI_Start';
function MPI_Request_free(var request: MPI_Request): longint; external 'mpich.dll' name 'MPI_Request_free';
function MPI_Sendrecv(sbuf: pointer; scount: longint; stype: MPI_Datatype; dest: longint; stag: longint; rbuf: pointer; rcount: longint; rtype: MPI_Datatype; source: longint; rtag: longint; comm: MPI_Comm; var status: MPI_Status): longint; external 'mpich.dll' name 'MPI_Sendrecv';
function MPI_Sendrecv_replace(buf: pointer; count: longint; datatype: MPI_Datatype; dest: longint; stag: longint; source: longint; rtag: longint; comm: MPI_Comm; var status: MPI_Status): longint; external 'mpich.dll' name 'MPI_Sendrecv_replace';
function MPI_Barrier(comm: MPI_Comm): longint; external 'mpich.dll' name 'MPI_Barrier';
function MPI_Bcast(buf: pointer; count: longint; datatype: MPI_Datatype; root: longint; comm: MPI_Comm): longint; external 'mpich.dll' name 'MPI_Bcast';
function MPI_Gather(sbuf: pointer; scount: longint; stype: MPI_Datatype; rbuf: pointer; rcount: longint; rtype: MPI_Datatype; root: longint; comm: MPI_Comm): longint; external 'mpich.dll' name 'MPI_Gather';
function MPI_Scatter(sbuf: pointer; scount: longint; stype: MPI_Datatype; rbuf: pointer; rcount: longint; rtype: MPI_Datatype; root: longint; comm: MPI_Comm): longint; external 'mpich.dll' name 'MPI_Scatter';
function MPI_Allgather(sbuf: pointer; scount: longint; stype: MPI_Datatype; rbuf: pointer; rcount: longint; rtype: MPI_Datatype; comm: MPI_Comm): longint; external 'mpich.dll' name 'MPI_Allgather';
function MPI_Alltoall(sbuf: pointer; scount: longint; stype: MPI_Datatype; rbuf: pointer; rcount: longint; rtype: MPI_Datatype; comm: MPI_Comm): longint; external 'mpich.dll' name 'MPI_Alltoall';
function MPI_Reduce(sbuf: pointer; rbuf: pointer; count: longint; datatype: MPI_Datatype; op: MPI_Op; root: longint; comm: MPI_Comm): longint; external 'mpich.dll' name 'MPI_Reduce';
function MPI_Allreduce(sbuf: pointer; rbuf: pointer; count: longint; datatype: MPI_Datatype; op: MPI_Op; comm: MPI_Comm): longint; external 'mpich.dll' name 'MPI_Allreduce';
function MPI_Scan(sbuf: pointer; rbuf: pointer; count: longint; datatype: MPI_Datatype; op: MPI_Op; comm: MPI_Comm): longint; external 'mpich.dll' name 'MPI_Scan';
function MPI_Op_create(var func: MPI_User_function; commute: longint; var op: MPI_Op): longint; external 'mpich.dll' name 'MPI_Op_create';
function MPI_Op_free(var op: MPI_Op): longint; external 'mpich.dll' name 'MPI_Op_free';
function MPI_Type_contiguous(count: longint; oldtype: MPI_Datatype; var newtype: MPI_Datatype): longint; external 'mpich.dll' name 'MPI_Type_contiguous';
function MPI_Type_vector(count: longint; blocklen: longint; stride: longint; oldtype: MPI_Datatype; var newtype: MPI_Datatype): longint; external 'mpich.dll' name 'MPI_Type_vector';
function MPI_Type_hvector(count: longint; blocklen: longint; stride: MPI_Aint; oldtype: MPI_Datatype; var newtype: MPI_Datatype): longint; external 'mpich.dll' name 'MPI_Type_hvector';
function MPI_Type_extent(datatype: MPI_Datatype; var extent: MPI_Aint): longint; external 'mpich.dll' name 'MPI_Type_extent';
function MPI_Type_size(datatype: MPI_Datatype; var size: longint): longint; external 'mpich.dll' name 'MPI_Type_size';
function MPI_Type_commit(var datatype: MPI_Datatype): longint; external 'mpich.dll' name 'MPI_Type_commit';
function MPI_Type_free(var datatype: MPI_Datatype): longint; external 'mpich.dll' name 'MPI_Type_free';
function MPI_Pack(inbuf: pointer; incount: longint; datatype: MPI_Datatype; outbuf: pointer; outsize: longint; var position: longint; comm: MPI_Comm): longint; external 'mpich.dll' name 'MPI_Pack';
function MPI_Unpack(inbuf: pointer; insize: longint; var position: longint; outbuf: pointer; outcount: longint; datatype: MPI_Datatype; comm: MPI_Comm): longint; external 'mpich.dll' name 'MPI_Unpack';
function MPI_Pack_size(incount: longint; datatype: MPI_Datatype; comm: MPI_Comm; var size: longint): longint; external 'mpich.dll' name 'MPI_Pack_size';
function MPI_Group_size(group: MPI_Group; var size: longint): longint; external 'mpich.dll' name 'MPI_Group_size';
function MPI_Group_rank(group: MPI_Group; var rank: longint): longint; external 'mpich.dll' name 'MPI_Group_rank';
function MPI_Group_compare(group1: MPI_Group; group2: MPI_Group; var res: longint): longint; external 'mpich.dll' name 'MPI_Group_compare';
function MPI_Comm_group(comm: MPI_Comm; var group: MPI_Group): longint; external 'mpich.dll' name 'MPI_Comm_group';
function MPI_Group_union(group1: MPI_Group; group2: MPI_Group; var newgroup: MPI_Group): longint; external 'mpich.dll' name 'MPI_Group_union';
function MPI_Group_intersection(group1: MPI_Group; group2: MPI_Group; var newgroup: MPI_Group): longint; external 'mpich.dll' name 'MPI_Group_intersection';
function MPI_Group_difference(group1: MPI_Group; group2: MPI_Group; var newgroup: MPI_Group): longint; external 'mpich.dll' name 'MPI_Group_difference';
function MPI_Group_free(var group: MPI_Group): longint; external 'mpich.dll' name 'MPI_Group_free';
function MPI_Comm_compare(comm1: MPI_Comm; comm2: MPI_Comm; var res: longint): longint; external 'mpich.dll' name 'MPI_Comm_compare';
function MPI_Comm_dup(comm: MPI_Comm; var newcomm: MPI_Comm): longint; external 'mpich.dll' name 'MPI_Comm_dup';
function MPI_Comm_create(comm: MPI_Comm; group: MPI_Group; var newcomm: MPI_Comm): longint; external 'mpich.dll' name 'MPI_Comm_create';
function MPI_Comm_split(comm: MPI_Comm; color: longint; key: longint; var newcomm: MPI_Comm): longint; external 'mpich.dll' name 'MPI_Comm_split';
function MPI_Comm_free(comm: MPI_Comm): longint; external 'mpich.dll' name 'MPI_Comm_free';
function MPI_Topo_test(comm: MPI_Comm; var status: longint): longint; external 'mpich.dll' name 'MPI_Topo_test';
function MPI_Graphdims_get(comm: MPI_Comm; var nnodes: longint; var nedges: longint): longint; external 'mpich.dll' name 'MPI_Graphdims_get';
function MPI_Cartdim_get(comm: MPI_Comm; var ndims: longint): longint; external 'mpich.dll' name 'MPI_Cartdim_get';
function MPI_Graph_neighbors_count(comm: MPI_Comm; rank: longint; var nneighbors: longint): longint; external 'mpich.dll' name 'MPI_Graph_neighbors_count';
function MPI_Cart_shift(comm: MPI_Comm; direction: longint; disp: longint; var rank_source: longint; var rank_dest: longint): longint; external 'mpich.dll' name 'MPI_Cart_shift';
function MPI_Comm_test_inter(comm: MPI_Comm; var flag: longint): longint; external 'mpich.dll' name 'MPI_Comm_test_inter';
function MPI_Comm_remote_size(comm: MPI_Comm; var size: longint): longint; external 'mpich.dll' name 'MPI_Comm_remote_size';
function MPI_Comm_remote_group(comm: MPI_Comm; var group: MPI_Group): longint; external 'mpich.dll' name 'MPI_Comm_remote_group';
function MPI_Intercomm_create(local_comm: MPI_Comm; local_leader: longint; peer_comm: MPI_Comm; remote_leader: longint; tag: longint; var newintercomm: MPI_Comm): longint; external 'mpich.dll' name 'MPI_Intercomm_create';
function MPI_Intercomm_merge(intercomm: MPI_Comm; high: longint; var newintracomm: MPI_Comm): longint; external 'mpich.dll' name 'MPI_Intercomm_merge';
function MPI_Abort(comm: MPI_Comm; errorcode: longint): longint; external 'mpich.dll' name 'MPI_Abort';
function MPI_Address(location: pointer; var address: MPI_Aint): longint; external 'mpich.dll' name 'MPI_Address';
function MPI_Cancel(var request: MPI_Request): longint; external 'mpich.dll' name 'MPI_Cancel';
function MPI_Get_elements(var status: MPI_Status; datatype: MPI_Datatype; var count: longint): longint; external 'mpich.dll' name 'MPI_Get_elements';
function MPI_Test_cancelled(var status: MPI_Status; var flag: longint): longint; external 'mpich.dll' name 'MPI_Test_cancelled';
function MPI_Type_lb(datatype: MPI_Datatype; var displacement: MPI_Aint): longint; external 'mpich.dll' name 'MPI_Type_lb';
function MPI_Type_ub(datatype: MPI_Datatype; var displacement: MPI_Aint): longint; external 'mpich.dll' name 'MPI_Type_ub';
function MPI_Errhandler_create(errfunction: MPI_Handler_function; var errhandler: MPI_Errhandler): longint;external 'mpich.dll' name 'MPI_Errhandler_create';
function MPI_Errhandler_set(comm: MPI_Comm; errhandler: MPI_Errhandler): longint;external 'mpich.dll' name 'MPI_Errhandler_set';
function MPI_Errhandler_get(comm: MPI_Comm; var errhandler: MPI_Errhandler): longint;external 'mpich.dll' name 'MPI_Errhandler_get';
function MPI_Errhandler_free(var errhandler: MPI_Errhandler): longint;external 'mpich.dll' name 'MPI_Errhandler_free';
function MPI_Error_string(errcode: longint; errstring: PChar; var strlen: longint): longint;external 'mpich.dll' name 'MPI_Error_string';
function MPI_Error_class(errcode: longint; var errclass: longint): longint;external 'mpich.dll' name 'MPI_Error_class';
function MPI_Waitall_(count: longint; requests: pointer; statuses: pointer): longint; external 'mpich.dll' name 'MPI_Waitall';
function MPI_Waitany_(count: longint; requests: pointer; var index: longint; var status: MPI_Status): longint; external 'mpich.dll' name 'MPI_Waitany';
function MPI_Waitsome_(incount: longint; requests: pointer; var outcount: longint; indexes: pointer; statuses: pointer): longint; external 'mpich.dll' name 'MPI_Waitsome';
function MPI_Testall_(count: longint; requests: pointer; var flag: longint; statuses: pointer): longint; external 'mpich.dll' name 'MPI_Testall';
function MPI_Testany_(count: longint; requests: pointer; var index: longint; var flag: longint; var status: MPI_Status): longint; external 'mpich.dll' name 'MPI_Testany';
function MPI_Testsome_(incount: longint; requests: pointer; var outcount: longint; indexes: pointer; statuses: pointer): longint; external 'mpich.dll' name 'MPI_Testsome';
function MPI_Startall_(count: longint; requests: pointer): longint; external 'mpich.dll' name 'MPI_Startall';
function MPI_Gatherv_(sbuf: pointer; scount: longint; stype: MPI_Datatype; rbuf: pointer; rcounts: pointer; displs: pointer; rtype: MPI_Datatype; root: longint; comm: MPI_Comm): longint; external 'mpich.dll' name 'MPI_Gatherv';
function MPI_Scatterv_(sbuf: pointer; scounts: pointer; displs: pointer; stype: MPI_Datatype; rbuf: pointer; rcount: longint; rtype: MPI_Datatype; root: longint; comm: MPI_Comm): longint; external 'mpich.dll' name 'MPI_Scatterv';
function MPI_Allgatherv_(sbuf: pointer; scount: longint; stype: MPI_Datatype; rbuf: pointer; rcounts: pointer; displs: pointer; rtype: MPI_Datatype; comm: MPI_Comm): longint; external 'mpich.dll' name 'MPI_Allgatherv';
function MPI_Alltoallv_(sbuf: pointer; scounts: pointer; sdispls: pointer; stype: MPI_Datatype; rbuf: pointer; rcounts: pointer; rdispls: pointer; rtype: MPI_Datatype; comm: MPI_Comm): longint; external 'mpich.dll' name 'MPI_Alltoallv';
function MPI_Reduce_scatter_(sbuf: pointer; rbuf: pointer; rcounts: pointer; datatype: MPI_Datatype; op: MPI_Op; comm: MPI_Comm): longint; external 'mpich.dll' name 'MPI_Reduce_scatter';
function MPI_Type_indexed_(count: longint; blocklens: pointer; displs: pointer; oldtype: MPI_Datatype; var newtype: MPI_Datatype): longint; external 'mpich.dll' name 'MPI_Type_indexed';
function MPI_Type_hindexed_(count: longint; blocklens: pointer; displs: pointer; oldtype: MPI_Datatype; var newtype: MPI_Datatype): longint; external 'mpich.dll' name 'MPI_Type_hindexed';
function MPI_Type_struct_(count: longint; blocklens: pointer; displs: pointer; oldtypes: pointer; var newtype: MPI_Datatype): longint; external 'mpich.dll' name 'MPI_Type_struct';
function MPI_Group_translate_ranks_(group1: MPI_Group; n: longint; ranks1: pointer; group2: MPI_Group; ranks2: pointer): longint; external 'mpich.dll' name 'MPI_Group_translate_ranks';
function MPI_Group_incl_(group: MPI_Group; n: longint; ranks: pointer; var newgroup: MPI_Group): longint; external 'mpich.dll' name 'MPI_Group_incl';
function MPI_Group_excl_(group: MPI_Group; n: longint; ranks: pointer; var newgroup: MPI_Group): longint; external 'mpich.dll' name 'MPI_Group_excl';
function MPI_Cart_create_(comm_old: MPI_Comm; ndims: longint; dims: pointer; periods: pointer; reorder: longint; var comm_cart: MPI_Comm): longint; external 'mpich.dll' name 'MPI_Cart_create';
function MPI_Dims_create_(nnodes: longint; ndims: longint; dims: pointer): longint; external 'mpich.dll' name 'MPI_Dims_create';
function MPI_Graph_create_(comm_old: MPI_Comm; nnodes: longint; index: pointer; edges: pointer; reorder: longint; var comm_graph: MPI_Comm): longint; external 'mpich.dll' name 'MPI_Graph_create';
function MPI_Graph_get_(comm: MPI_Comm; maxindex: longint; maxedges: longint; index: pointer; edges: pointer): longint; external 'mpich.dll' name 'MPI_Graph_get';
function MPI_Cart_get_(comm: MPI_Comm; maxdims: longint; dims: pointer; periods: pointer; coords: pointer): longint; external 'mpich.dll' name 'MPI_Cart_get';
function MPI_Cart_rank_(comm: MPI_Comm; coords: pointer; var rank: longint): longint; external 'mpich.dll' name 'MPI_Cart_rank';
function MPI_Cart_coords_(comm: MPI_Comm; rank: longint; maxdims: longint; coords: pointer): longint; external 'mpich.dll' name 'MPI_Cart_coords';
function MPI_Graph_neighbors_(comm: MPI_Comm; rank: longint; maxneighbors: longint; neighbors: pointer): longint; external 'mpich.dll' name 'MPI_Graph_neighbors';
function MPI_Cart_sub_(comm: MPI_Comm; remain_dims: pointer; var newcomm: MPI_Comm): longint; external 'mpich.dll' name 'MPI_Cart_sub';
function MPI_Cart_map_(comm: MPI_Comm; ndims: longint; dims: pointer; periods: pointer; var newrank: longint): longint; external 'mpich.dll' name 'MPI_Cart_map';
function MPI_Graph_map_(comm: MPI_Comm; nnodes: longint; index: pointer; edges: pointer; var newrank: longint): longint; external 'mpich.dll' name 'MPI_Graph_map';
function MPI_Group_range_incl_(group: MPI_Group; n: longint; ranges: pointer; var newgroup: MPI_Group): longint; external 'mpich.dll' name 'MPI_Group_range_incl';
function MPI_Group_range_excl_(group: MPI_Group; n: longint; ranges: pointer; var newgroup: MPI_Group): longint; external 'mpich.dll' name 'MPI_Group_range_excl';

function MPI_Waitall(count: longint; var requests: array of MPI_Request; var statuses: array of MPI_Status): longint;
begin
  result := MPI_Waitall_(count, @requests[0], @statuses[0]);
end;
function MPI_Waitany(count: longint; var requests: array of MPI_Request; var index: longint; var status: MPI_Status): longint;
begin
  result := MPI_Waitany_(count, @requests[0], index, status);
end;
function MPI_Waitsome(incount: longint; var requests: array of MPI_Request; var outcount: longint; var indexes: array of longint; var statuses: array of MPI_Status): longint;
begin
  result := MPI_Waitsome_(incount, @requests[0], outcount, @indexes[0], @statuses[0]);
end;
function MPI_Testall(count: longint; var requests: array of MPI_Request; var flag: longint; var statuses: array of MPI_Status): longint;
begin
  result := MPI_Testall_(count, @requests[0], flag, @statuses[0]);
end;
function MPI_Testany(count: longint; var requests: array of MPI_Request; var index: longint; var flag: longint; var status: MPI_Status): longint;
begin
  result := MPI_Testany_(count, @requests[0], index, flag, status);
end;
function MPI_Testsome(incount: longint; var requests: array of MPI_Request; var outcount: longint; var indexes: array of longint; var statuses: array of MPI_Status): longint;
begin
  result := MPI_Testsome_(incount, @requests[0], outcount, @indexes[0], @statuses[0]);
end;
function MPI_Startall(count: longint; var requests: array of MPI_Request): longint;
begin
  result := MPI_Startall_(count, @requests[0]);
end;
function MPI_Gatherv(sbuf: pointer; scount: longint; stype: MPI_Datatype; rbuf: pointer; const rcounts: array of longint; const displs: array of longint; rtype: MPI_Datatype; root: longint; comm: MPI_Comm): longint;
begin
  result := MPI_Gatherv_(sbuf, scount, stype, rbuf, @rcounts[0], @displs[0], rtype, root, comm);
end;
function MPI_Scatterv(sbuf: pointer; const scounts: array of longint; const displs: array of longint; stype: MPI_Datatype; rbuf: pointer; rcount: longint; rtype: MPI_Datatype; root: longint; comm: MPI_Comm): longint;
begin
  result := MPI_Scatterv_(sbuf, @scounts[0], @displs[0], stype, rbuf, rcount, rtype, root, comm);
end;
function MPI_Allgatherv(sbuf: pointer; scount: longint; stype: MPI_Datatype; rbuf: pointer; const rcounts: array of longint; const displs: array of longint; rtype: MPI_Datatype; comm: MPI_Comm): longint;
begin
  result := MPI_Allgatherv_(sbuf, scount, stype, rbuf, @rcounts[0], @displs[0], rtype, comm);
end;
function MPI_Alltoallv(sbuf: pointer; const scounts: array of longint; const sdispls: array of longint; stype: MPI_Datatype; rbuf: pointer; const rcounts: array of longint; const rdispls: array of longint; rtype: MPI_Datatype; comm: MPI_Comm): longint;
begin
  result := MPI_Alltoallv_(sbuf, @scounts[0], @sdispls[0], stype, rbuf, @rcounts[0], @rdispls[0], rtype, comm);
end;
function MPI_Reduce_scatter(sbuf: pointer; rbuf: pointer; const rcounts: array of longint; datatype: MPI_Datatype; op: MPI_Op; comm: MPI_Comm): longint;
begin
  result := MPI_Reduce_scatter_(sbuf, rbuf, @rcounts[0], datatype, op, comm);
end;
function MPI_Type_indexed(count: longint; const blocklens: array of longint; const displs: array of longint; oldtype: MPI_Datatype; var newtype: MPI_Datatype): longint;
begin
  result := MPI_Type_indexed_(count, @blocklens[0], @displs[0], oldtype, newtype);
end;
function MPI_Type_hindexed(count: longint; const blocklens: array of longint; const displs: array of MPI_Aint; oldtype: MPI_Datatype; var newtype: MPI_Datatype): longint;
begin
  result := MPI_Type_hindexed_(count, @blocklens[0], @displs[0], oldtype, newtype);
end;
function MPI_Type_struct(count: longint; const blocklens: array of longint; const displs: array of MPI_Aint; const oldtypes: array of MPI_Datatype; var newtype: MPI_Datatype): longint;
begin
  result := MPI_Type_struct_(count, @blocklens[0], @displs[0], @oldtypes[0], newtype);
end;
function MPI_Group_translate_ranks(group1: MPI_Group; n: longint; const ranks1: array of longint; group2: MPI_Group; var ranks2: array of longint): longint;
begin
  result := MPI_Group_translate_ranks_(group1, n, @ranks1[0], group2, @ranks2[0]);
end;
function MPI_Group_incl(group: MPI_Group; n: longint; const ranks: array of longint; var newgroup: MPI_Group): longint;
begin
  result := MPI_Group_incl_(group, n, @ranks[0], newgroup);
end;
function MPI_Group_excl(group: MPI_Group; n: longint; const ranks: array of longint; var newgroup: MPI_Group): longint;
begin
  result := MPI_Group_excl_(group, n, @ranks[0], newgroup);
end;
function MPI_Cart_create(comm_old: MPI_Comm; ndims: longint; const dims: array of longint; const periods: array of longint; reorder: longint; var comm_cart: MPI_Comm): longint;
begin
  result := MPI_Cart_create_(comm_old, ndims, @dims[0], @periods[0], reorder, comm_cart);
end;
function MPI_Dims_create(nnodes: longint; ndims: longint; var dims: array of longint): longint;
begin
  result := MPI_Dims_create_(nnodes, ndims, @dims[0]);
end;
function MPI_Graph_create(comm_old: MPI_Comm; nnodes: longint; const index: array of longint; const edges: array of longint; reorder: longint; var comm_graph: MPI_Comm): longint;
begin
  result := MPI_Graph_create_(comm_old, nnodes, @index[0], @edges[0], reorder, comm_graph);
end;
function MPI_Graph_get(comm: MPI_Comm; maxindex: longint; maxedges: longint; var index: array of longint; var edges: array of longint): longint;
begin
  result := MPI_Graph_get_(comm, maxindex, maxedges, @index[0], @edges[0]);
end;
function MPI_Cart_get(comm: MPI_Comm; maxdims: longint; var dims: array of longint; var periods: array of longint; var coords: array of longint): longint;
begin
  result := MPI_Cart_get_(comm, maxdims, @dims[0], @periods[0], @coords[0]);
end;
function MPI_Cart_rank(comm: MPI_Comm; const coords: array of longint; var rank: longint): longint;
begin
  result := MPI_Cart_rank_(comm, @coords[0], rank);
end;
function MPI_Cart_coords(comm: MPI_Comm; rank: longint; maxdims: longint; var coords: array of longint): longint;
begin
  result := MPI_Cart_coords_(comm, rank, maxdims, @coords[0]);
end;
function MPI_Graph_neighbors(comm: MPI_Comm; rank: longint; maxneighbors: longint; var neighbors: array of longint): longint;
begin
  result := MPI_Graph_neighbors_(comm, rank, maxneighbors, @neighbors[0]);
end;
function MPI_Cart_sub(comm: MPI_Comm; const remain_dims: array of longint; var newcomm: MPI_Comm): longint;
begin
  result := MPI_Cart_sub_(comm, @remain_dims[0], newcomm);
end;
function MPI_Cart_map(comm: MPI_Comm; ndims: longint; const dims: array of longint; const periods: array of longint; var newrank: longint): longint;
begin
  result := MPI_Cart_map_(comm, ndims, @dims[0], @periods[0], newrank);
end;
function MPI_Graph_map(comm: MPI_Comm; nnodes: longint; const index: array of longint; const edges: array of longint; var newrank: longint): longint;
begin
  result := MPI_Graph_map_(comm, nnodes, @index[0], @edges[0], newrank);
end;
function MPI_Group_range_incl(group: MPI_Group; n: longint; const ranges: array of MPI_Triplet_range; var newgroup: MPI_Group): longint;
begin
  result := MPI_Group_range_incl_(group, n, @ranges[0], newgroup);
end;
function MPI_Group_range_excl(group: MPI_Group; n: longint; const ranges: array of MPI_Triplet_range; var newgroup: MPI_Group): longint;
begin
  result := MPI_Group_range_excl_(group, n, @ranges[0], newgroup);
end;
end.
