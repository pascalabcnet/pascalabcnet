var
  Err : boolean;

type
       { DO NOT CHANGE THE VALUES OF THESE ENUMERATIONS! }
       tsmallenum = (dA,dB,dC,dd,de,df,dg,dh,di,dj,dk,dl,dm,dn,dop,dp,dq,dr);
       tasmop = (A_ABCD,
         A_ADD,A_ADDA,A_ADDI,A_ADDQ,A_ADDX,A_AND,A_ANDI,
         A_ASL,A_ASR,A_BCC,A_BCS,A_BEQ,A_BGE,A_BGT,A_BHI,
         A_BLE,A_BLS,A_BLT,A_BMI,A_BNE,A_BPL,A_BVC,A_BVS,
         A_BCHG,A_BCLR,A_BRA,A_BSET,A_BSR,A_BTST,A_CHK,
         A_CLR,A_CMP,A_CMPA,A_CMPI,A_CMPM,A_DBCC,A_DBCS,A_DBEQ,A_DBGE,
         A_DBGT,A_DBHI,A_DBLE,A_DBLS,A_DBLT,A_DBMI,A_DBNE,A_DBRA,
         A_DBPL,A_DBT,A_DBVC,A_DBVS,A_DBF,A_DIVS,A_DIVU,
         A_EOR,A_EORI,A_EXG,A_ILLEGAL,A_EXT,A_JMP,A_JSR,
         A_LEA,A_LINK,A_LSL,A_LSR,A_MOVE,A_MOVEA,A_MOVEI,A_MOVEQ,
         A_MOVEM,A_MOVEP,A_MULS,A_MULU,A_NBCD,A_NEG,A_NEGX,
         A_NOP,A_NOT,A_OR,A_ORI,A_PEA,A_ROL,A_ROR,A_ROXL,
         A_ROXR,A_RTR,A_RTS,A_SBCD,A_SCC,A_SCS,A_SEQ,A_SGE,
         A_SGT,A_SHI,A_SLE,A_SLS,A_SLT,A_SMI,A_SNE,
         A_SPL,A_ST,A_SVC,A_SVS,A_SF,A_SUB,A_SUBA,A_SUBI,A_SUBQ,
         A_SUBX,A_SWAP,A_TAS,A_TRAP,A_TRAPV,A_TST,A_UNLK,
         A_RTE,A_RESET,A_STOP,
         { MC68010 instructions }
         A_BKPT,A_MOVEC,A_MOVES,A_RTD,
         { MC68020 instructions }
         A_BFCHG,A_BFCLR,A_BFEXTS,A_BFEXTU,A_BFFFO,
         A_BFINS,A_BFSET,A_BFTST,A_CALLM,A_CAS,A_CAS2,
         A_CHK2,A_CMP2,A_DIVSL,A_DIVUL,A_EXTB,A_PACK,A_RTM,
         A_TRAPCC,A_TRACS,A_TRAPEQ,A_TRAPF,A_TRAPGE,A_TRAPGT,
         A_TRAPHI,A_TRAPLE,A_TRAPLS,A_TRAPLT,A_TRAPMI,A_TRAPNE,
         A_TRAPPL,A_TRAPT,A_TRAPVC,A_TRAPVS,A_UNPK,
         { FPU Processor instructions - directly supported only. }
         { IEEE aware and misc. condition codes not supported   }
         A_FABS,A_FADD,
         A_FBEQ,A_FBNE,A_FBNGT,A_FBGT,A_FBGE,A_FBNGE,
         A_FBLT,A_FBNLT,A_FBLE,A_FBGL,A_FBNGL,A_FBGLE,A_FBNGLE,
         A_FDBEQ,A_FDBNE,A_FDBGT,A_FDBNGT,A_FDBGE,A_FDBNGE,
         A_FDBLT,A_FDBNLT,A_FDBLE,A_FDBGL,A_FDBNGL,A_FDBGLE,A_FBDNGLE,
         A_FSEQ,A_FSNE,A_FSGT,A_FSNGT,A_FSGE,A_FSNGE,
         A_FSLT,A_FSNLT,A_FSLE,A_FSGL,A_FSNGL,A_FSGLE,A_FSNGLE,
         A_FCMP,A_FDIV,A_FMOVE,A_FMOVEM,
         A_FMUL,A_FNEG,A_FNOP,A_FSQRT,A_FSUB,A_FSGLDIV,
         A_FSFLMUL,A_FTST,
         A_FTRAPEQ,A_FTRAPNE,A_FTRAPGT,A_FTRAPNGT,A_FTRAPGE,A_FTRAPNGE,
         A_FTRAPLT,A_FTRAPNLT,A_FTRAPLE,A_FTRAPGL,A_FTRAPNGL,A_FTRAPGLE,A_FTRAPNGLE,
         { Protected instructions }
         A_CPRESTORE,A_CPSAVE,
         { FPU Unit protected instructions                    }
         { and 68030/68851 common MMU instructions            }
         { (this may include 68040 MMU instructions)          }
         A_FRESTORE,A_FSAVE,A_PFLUSH,A_PFLUSHA,A_PLOAD,A_PMOVE,A_PTEST,
         { Useful for assembly langage output }
         A_LABEL,A_NONE);



type
  topset = set of tasmop;
  tsmallset = set of tsmallenum;

const

   { NORMAL SETS }
   constset1 : array[1..3] of topset =
   (
       { 66 }    { 210 }  { 225 }
     ([A_MOVE,    { 66  : LONG 2 - BIT 2  }
       A_FTST,    { 210 : LONG 6 - BIT 18 }
       A_CPSAVE]),{ 225 : LONG 7 - BIT 1 }
       { 1..8 }
     ([A_ADD..A_ASL]),
       { 134 }
     ([A_CHK2])
   );

   constset2 : array[1..3] of topset =
   (
     ([A_MOVE,A_FTST,A_CPSAVE]),
     ([A_ADD..A_ASL]),
     ([A_CHK2])
   );

   { SMALL SETS }
   constset3 : array[1..3] of tsmallset =
   (
     ([DA,             { 0 :  LONG 0 : bit 0 }
       DD,             { 3 :  LONG 0 : bit 3 }
       DM]),           { 12 :  LONG 0 : bit 12  }
     ([DB..DI]),       { 1..8 : LONG 0 : bits 1-8  }
     ([DR])            { 17 :  LONG 0 : bit 17 }
   );

   constset4 : array[1..3] of tsmallset =
   (
     ([DA,DD,DM]),
     ([DB..DI]),
     ([DR])
   );


 procedure CheckPassed(passed:boolean);
 begin
   if passed then
     WriteLn('Success.')
   else
     begin
       WriteLn('Failure.');
       Halt(1);
       Err:=true;
     end;
 end;

 procedure SetTestEqual;
 { FPC_SET_COMP_SETS }
  var
    op2list :set of tasmop;
    oplist: set of tasmop;
    passed : boolean;
  Begin
   Write('Normal Set == Normal Set test...');
   passed := true;
   op2list:=[];
   oplist:=[];
   if not (oplist=op2list) then
     passed := false;
   if not (constset1[2] = constset2[2]) then
     passed := false;
   if (constset1[1] = constset2[2]) then
     passed := false;
   if not (constset1[1] = [A_MOVE,A_FTST,A_CPSAVE]) then
     passed := false;
    CheckPassed(passed);
  end;

 procedure SetTestNotEqual;
 { FPC_SET_COMP_SETS }
  var
    op2list :set of tasmop;
    oplist: set of tasmop;
    passed : boolean;
  Begin
   Write('Normal Set <> Normal Set test...');
   passed := true;
   op2list:=[];
   oplist:=[];
   if not (oplist=op2list) then
     passed := false;
   if (constset1[2] <> constset2[2]) then
     passed := false;
   if not (constset1[1] <> constset2[2]) then
     passed := false;
{   if ( [A_ADD] <> [A_ADD] ) then optimized out.
     passed := false;
   if ( [A_BLE..A_BPL] <> [A_BLE..A_BPL] ) then
     passed := false; }
   if (constset1[1] <> [A_MOVE,A_FTST,A_CPSAVE]) then
     passed := false;
    CheckPassed(passed);
  end;

  procedure SetTestLt;
  var
    op2list :set of tasmop;
    oplist: set of tasmop;
    passed : boolean;
   begin
    Write('Normal Set <= Normal Set test...');
    passed := true;
    if constset1[1] <= constset2[2] then
      passed := false;
    oplist := [];
    op2list := [A_MOVE];
    if op2list <= oplist then
     passed := false;
    oplist := [A_MOVE,A_CPRESTORE..A_CPSAVE];
    if oplist <= op2list then
     passed := false;
    CheckPassed(passed);
   end;

  Procedure SetTestAddOne;
 { FPC_SET_SET_BYTE }
 { FPC_SET_ADD_SETS }
    var
     op : tasmop;
     oplist: set of tasmop;
  Begin
    Write('Set + Set element testing...');
    op:=A_LABEL;
    oplist:=[];
    oplist:=oplist+[op];
    CheckPassed(oplist = [A_LABEL]);
  end;

Procedure SetTestAddTwo;
{ SET_ADD_SETS }
var
 op2list :set of tasmop;
 oplist: set of tasmop;
Begin
 Write('Complex Set + Set element testing...');
 op2list:=[];
 oplist:=[];
 oplist:=[A_MOVE]+[A_JSR];
 op2list:=[A_LABEL];
 oplist:=op2list+oplist;
 CheckPassed(oplist = [A_MOVE,A_JSR,A_LABEL]);
end;





Procedure SetTestSubOne;
{ SET_SUB_SETS }
var
 op2list :set of tasmop;
 oplist: set of tasmop;
 op :tasmop;
 passed : boolean;
Begin
 Write('Set - Set element testing...');
 passed := true;
 op2list:=[];
 oplist:=[];
 op := A_TRACS;
 oplist:=[A_MOVE]+[A_JSR]+[op];
 op2list:=[A_MOVE]+[A_JSR];
 oplist:=oplist-op2list;
 if oplist <> [A_TRACS] then
   passed := false;

 oplist:=[A_MOVE]+[A_JSR]+[op];
 op2list:=[A_MOVE]+[A_JSR];
 oplist:=op2list-oplist;
 if oplist <> [] then
   passed := false;
 CheckPassed(passed);
end;

Procedure SetTestSubTwo;
{ FPC_SET_SUB_SETS }
const
 b: tasmop = (A_BSR);
var
 op2list :set of tasmop;
 oplist: set of tasmop;
 op : tasmop;
 passed : boolean;
Begin
 Write('Complex Set - Set element testing...');
 op := A_BKPT;
 passed := true;
 oplist:=[A_MOVE]+[A_JSR]-[op];
 op2list:=[A_MOVE]+[A_JSR];
 if oplist <> op2list then
   passed := false;
 oplist := [A_MOVE];
 oplist := oplist - [A_MOVE];
 if oplist <> [] then
   passed := false;
 oplist := oplist + [b];
 if oplist <> [b] then
   passed := false;
 oplist := oplist - [b];
 if oplist <> [] then
   passed := false;
 CheckPassed(passed);
end;


Procedure SetTestMulSets;
{ FPC_SET_MUL_SETS }
var
 op2list :set of tasmop;
 oplist: set of tasmop;
 passed : boolean;
Begin
 passed := true;
 Write('Set * Set element testing...');
 op2list:=[];
 oplist:=[];
 oplist:=[A_MOVE]+[A_JSR];
 op2list:=[A_MOVE];
 oplist:=oplist*op2list;
 if oplist <> [A_JSR] then
   passed := false;
 oplist := [A_MOVE,A_FTST];
 op2list := [A_MOVE,A_FTST];
 oplist := oplist * op2list;
 if oplist <> [A_MOVE,A_FTST] then
   passed := false;
 CheckPassed(passed);
end;

procedure SetTestRange;
var
 op2list :set of tasmop;
 oplist: set of tasmop;
 passed : boolean;
 op1 : tasmop;
 op2 : tasmop;
begin
 passed := true;
 Write('Range Set + element testing...');
 op1 := A_ADD;
 op2 := A_ASL;
 oplist := [];
 oplist := [op1..op2];
 if oplist <> constset1[2] then
   passed := false;
 CheckPassed(passed);
end;

procedure SetTestByte;
var
 op2list :set of tasmop;
 oplist: set of tasmop;
 passed : boolean;
 op1 : tasmop;
 op2 : tasmop;
 op : tasmop;
begin
 Write('Simple Set + element testing...');
 passed := true;
 op := A_LABEL;
 oplist := [A_MOVE,op,A_JSR];
 if oplist <> [A_MOVE,A_LABEL,A_JSR] then
   passed := false;
 CheckPassed(passed);
end;


{------------------------------ TESTS FOR SMALL VALUES ---------------------}
 procedure SmallSetTestEqual;
  var
    op2list :set of tsmallenum;
    oplist: set of tsmallenum;
    passed : boolean;
  Begin
   Write('Small Set == Small Set test...');
   passed := true;
   op2list:=[];
   oplist:=[];
   if not (oplist=op2list) then
     passed := false;
   if not (constset3[2] = constset4[2]) then
     passed := false;
   if (constset3[1] = constset4[2]) then
     passed := false;
   if not (constset3[1] = [DA,DD,DM]) then
     passed := false;
 CheckPassed(passed);
  end;

 procedure SmallSetTestNotEqual;
  var
    op2list :set of tsmallenum;
    oplist: set of tsmallenum;
    passed : boolean;
  Begin
   Write('Small Set <> Small Set test...');
   passed := true;
   op2list:=[];
   oplist:=[];
   if not (oplist=op2list) then
     passed := false;
   if (constset3[2] <> constset4[2]) then
     passed := false;
   if not (constset3[1] <> constset4[2]) then
     passed := false;
{   if ( [A_ADD] <> [A_ADD] ) then optimized out.
     passed := false;
   if ( [A_BLE..A_BPL] <> [A_BLE..A_BPL] ) then
     passed := false; }
   if (constset3[1] <> [DA,DD,DM]) then
     passed := false;
 CheckPassed(passed);
  end;

  procedure SmallSetTestLt;
  var
    op2list :set of tsmallenum;
    oplist: set of tsmallenum;
    passed : boolean;
   begin
    Write('Small Set <= Small Set test...');
    passed := true;
    if constset3[1] <= constset4[2] then
      passed := false;
    oplist := [];
    op2list := [DC];
    if op2list <= oplist then
     passed := false;
    oplist := [DC,DF..DM];
    if oplist <= op2list then
     passed := false;
 CheckPassed(passed);
   end;

  Procedure SmallSetTestAddOne;
    var
     op : tsmallenum;
     oplist: set of tsmallenum;
  Begin
    Write('Small Set + Small Set element testing...');
    op:=DG;
    oplist:=[];
    oplist:=oplist+[op];
    CheckPassed( oplist = [DG] );
  end;

Procedure SmallSetTestAddTwo;
var
 op2list :set of tsmallenum;
 oplist: set of tsmallenum;
Begin
 Write('Small Complex Set + Small Set element testing...');
 op2list:=[];
 oplist:=[];
 oplist:=[DG]+[DI];
 op2list:=[DM];
 oplist:=op2list+oplist;
 CheckPassed( oplist = [DG,DI,DM] );
end;


Procedure SmallSetTestSubOne;
var
 op2list :set of tsmallenum;
 oplist: set of tsmallenum;
 op :tsmallenum;
 passed : boolean;
Begin
 Write('Small Set - Small Set element testing...');
 passed := true;
 op2list:=[];
 oplist:=[];
 op := DL;
 oplist:=[DG]+[DI]+[op];
 op2list:=[DG]+[DI];
 oplist:=oplist-op2list;
 if oplist <> [DL] then
   passed := false;

 oplist:=[DG]+[DI]+[op];
 op2list:=[DG]+[DI];
 oplist:=op2list-oplist;
 if oplist <> [] then
   passed := false;
 CheckPassed(passed);
end;

Procedure SmallSetTestSubTwo;
const
 b: tsmallenum = (DH);
var
 op2list :set of tsmallenum;
 oplist: set of tsmallenum;
 op : tsmallenum;
 passed : boolean;
Begin
 Write('Small Complex Set - Small Set element testing...');
 op := DL;
 passed := true;
 oplist:=[DG]+[DI]-[op];
 op2list:=[DG]+[DI];
 if oplist <> op2list then
   passed := false;
 oplist := [DG];
 oplist := oplist - [DG];
 if oplist <> [] then
   passed := false;
 oplist := oplist + [b];
 if oplist <> [b] then
   passed := false;
 oplist := oplist - [b];
 if oplist <> [] then
   passed := false;
 CheckPassed(passed);
end;


Procedure SmallSetTestMulSets;
var
 op2list : set of tsmallenum;
 oplist: set of tsmallenum;
 passed : boolean;
Begin
 passed := true;
 Write('Small Set * Small Set element testing...');
 op2list:=[];
 oplist:=[];
 oplist:=[DG]+[DI];
 op2list:=[DG];
 oplist:=oplist*op2list;
 if oplist <> [DI] then
   passed := false;
 oplist := [DG,DK];
 op2list := [DG,DK];
 oplist := oplist * op2list;
 if oplist <> [DG,DK] then
   passed := false;
 CheckPassed(passed);
end;

procedure SmallSetTestRange;
var
 op2list :set of tsmallenum;
 oplist: set of tsmallenum;
 passed : boolean;
 op1 : tsmallenum;
 op2 : tsmallenum;
begin
 passed := true;
 Write('Small Range Set + element testing...');
 op1 := DB;
 op2 := DI;
 oplist := [];
 oplist := [op1..op2];
 if oplist <> constset3[2] then
   passed := false;
 CheckPassed(passed);
end;

procedure SmallSetTestByte;
var
 op2list : set of tsmallenum;
 oplist: set of tsmallenum;
 passed : boolean;
 op1 : tsmallenum;
 op2 : tsmallenum;
 op : tsmallenum;
begin
 Write('Small Simple Set + element testing...');
 passed := true;
 op := DD;
 oplist := [DG,op,DI];
 if oplist <> [DG,DD,DI] then
   passed := false;
 CheckPassed(passed);
end;

(*

const
 b: myenum = (dA);
var
 enum: set of myenum;
 oplist: set of tasmop;
 l : word;
Begin
  SetTestEqual;
  SetTestNotEqual;
{ small sets }
 enum:=[];
 { add }
 enum:=enum+[da];
 { subtract }
 enum:=enum-[da];
 if DA in enum then
  WriteLn('Found A_LABEL');
 { very large sets       }
 { copy loop test        }
 WRITELN('LARGE SETS:');
 oplist := [A_LABEL];
 { secondin test         }
 if A_LABEL in oplist then
  WriteLn('TESTING SIMPLE SECOND_IN: PASSED.');
 { }
 oplist:=[];
 if A_LABEL in oplist then
  WriteLn('SECOND IN FAILED.');
{ SecondinSets;}
 SetSetByte;
 SetAddSets;
 SetSubSets;
 SetCompSets;
 SetMulSets;
 WRITELN('SMALL SETS:');
 SmallInSets;
 SmallAddSets;
 SmallSubSets;
 SmallCompSets;
 SmallMulSets;
 l:=word(A_CPRESTORE);
 if l = word(A_CPRESTORE) then
  Begin
  end;

*)
Begin
  WriteLn('----------------------- Normal sets -----------------------');
  { Normal sets }
  SetTestEqual;
  SetTestNotEqual;
  SetTestAddOne;
  SetTestAddTwo;
  SetTestSubOne;
  SetTestSubTwo;
  SetTestRange;
  SetTestLt;
  SetTestByte;
  { Small sets }
  WriteLn('----------------------- Small sets -----------------------');
  SmallSetTestEqual;
  SmallSetTestNotEqual;
  SmallSetTestAddOne;
  SmallSetTestAddTwo;
  SmallSetTestSubOne;
  SmallSetTestSubTwo;
  SmallSetTestRange;
  SmallSetTestLt;
  SmallSetTestByte;

  if Err then
   Halt(1);
end. 