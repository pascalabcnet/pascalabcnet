uses implement1u;

type TClass = class(System._AppDomain)

	public procedure GetTypeInfoCount(var pcTInfo: longword);
	public procedure GetTypeInfo(iTInfo: longword; lcid: longword; ppTInfo: System.IntPtr);
	public procedure GetIDsOfNames(var riid: System.Guid; rgszNames: System.IntPtr; cNames: longword; lcid: longword; rgDispId: System.IntPtr);
	public procedure SetThreadPrincipal(principal: System.Security.Principal.IPrincipal);
	public procedure SetPrincipalPolicy(policy: System.Security.Principal.PrincipalPolicy);
	public procedure DoCallBack(theDelegate: System.CrossAppDomainDelegate);
	public function get_DynamicDirectory: string;
	public procedure Invoke(dispIdMember: longword; var riid: System.Guid; lcid: longword; wFlags: smallint; pDispParams: System.IntPtr; pVarResult: System.IntPtr; pExcepInfo: System.IntPtr; puArgErr: System.IntPtr);
	public function ToString: string;
	public function Equals(other: System.Object): boolean;
	public function GetHashCode: integer;
	public function GetType: System.Type;
	public function InitializeLifetimeService: System.Object;
	public function GetLifetimeService: System.Object;
	public function get_Evidence: System.Security.Policy.Evidence;
	public procedure add_DomainUnload(value: System.EventHandler);
	public procedure remove_DomainUnload(value: System.EventHandler);
	public procedure add_AssemblyLoad(value: System.AssemblyLoadEventHandler);
	public procedure remove_AssemblyLoad(value: System.AssemblyLoadEventHandler);
	public procedure add_ProcessExit(value: System.EventHandler);
	public procedure remove_ProcessExit(value: System.EventHandler);
	public procedure add_TypeResolve(value: System.ResolveEventHandler);
	public procedure remove_TypeResolve(value: System.ResolveEventHandler);
	public procedure add_ResourceResolve(value: System.ResolveEventHandler);
	public procedure remove_ResourceResolve(value: System.ResolveEventHandler);
	public procedure add_AssemblyResolve(value: System.ResolveEventHandler);
	public procedure remove_AssemblyResolve(value: System.ResolveEventHandler);
	public procedure add_UnhandledException(value: System.UnhandledExceptionEventHandler);
	public procedure remove_UnhandledException(value: System.UnhandledExceptionEventHandler);
	public function DefineDynamicAssembly(name: System.Reflection.AssemblyName; access: System.Reflection.Emit.AssemblyBuilderAccess): System.Reflection.Emit.AssemblyBuilder;
	public function DefineDynamicAssembly(name: System.Reflection.AssemblyName; access: System.Reflection.Emit.AssemblyBuilderAccess; dir: string): System.Reflection.Emit.AssemblyBuilder;
	public function DefineDynamicAssembly(name: System.Reflection.AssemblyName; access: System.Reflection.Emit.AssemblyBuilderAccess; evidence: System.Security.Policy.Evidence): System.Reflection.Emit.AssemblyBuilder;
	public function DefineDynamicAssembly(name: System.Reflection.AssemblyName; access: System.Reflection.Emit.AssemblyBuilderAccess; requiredPermissions: System.Security.PermissionSet; optionalPermissions: System.Security.PermissionSet; refusedPermissions: System.Security.PermissionSet): System.Reflection.Emit.AssemblyBuilder;
	public function DefineDynamicAssembly(name: System.Reflection.AssemblyName; access: System.Reflection.Emit.AssemblyBuilderAccess; dir: string; evidence: System.Security.Policy.Evidence): System.Reflection.Emit.AssemblyBuilder;
	public function DefineDynamicAssembly(name: System.Reflection.AssemblyName; access: System.Reflection.Emit.AssemblyBuilderAccess; dir: string; requiredPermissions: System.Security.PermissionSet; optionalPermissions: System.Security.PermissionSet; refusedPermissions: System.Security.PermissionSet): System.Reflection.Emit.AssemblyBuilder;
	public function DefineDynamicAssembly(name: System.Reflection.AssemblyName; access: System.Reflection.Emit.AssemblyBuilderAccess; evidence: System.Security.Policy.Evidence; requiredPermissions: System.Security.PermissionSet; optionalPermissions: System.Security.PermissionSet; refusedPermissions: System.Security.PermissionSet): System.Reflection.Emit.AssemblyBuilder;
	public function DefineDynamicAssembly(name: System.Reflection.AssemblyName; access: System.Reflection.Emit.AssemblyBuilderAccess; dir: string; evidence: System.Security.Policy.Evidence; requiredPermissions: System.Security.PermissionSet; optionalPermissions: System.Security.PermissionSet; refusedPermissions: System.Security.PermissionSet): System.Reflection.Emit.AssemblyBuilder;
	public function DefineDynamicAssembly(name: System.Reflection.AssemblyName; access: System.Reflection.Emit.AssemblyBuilderAccess; dir: string; evidence: System.Security.Policy.Evidence; requiredPermissions: System.Security.PermissionSet; optionalPermissions: System.Security.PermissionSet; refusedPermissions: System.Security.PermissionSet; isSynchronized: boolean): System.Reflection.Emit.AssemblyBuilder;
	public function CreateInstance(assemblyName: string; typeName: string): System.Runtime.Remoting.ObjectHandle;
	public function CreateInstanceFrom(assemblyFile: string; typeName: string): System.Runtime.Remoting.ObjectHandle;
	public function CreateInstance(assemblyName: string; typeName: string; activationAttributes: array of System.Object): System.Runtime.Remoting.ObjectHandle;
	public function CreateInstanceFrom(assemblyFile: string; typeName: string; activationAttributes: array of System.Object): System.Runtime.Remoting.ObjectHandle;
	public function CreateInstance(assemblyName: string; typeName: string; ignoreCase: boolean; bindingAttr: System.Reflection.BindingFlags; binder: System.Reflection.Binder; args: array of System.Object; culture: System.Globalization.CultureInfo; activationAttributes: array of System.Object; securityAttributes: System.Security.Policy.Evidence): System.Runtime.Remoting.ObjectHandle;
	public function CreateInstanceFrom(assemblyFile: string; typeName: string; ignoreCase: boolean; bindingAttr: System.Reflection.BindingFlags; binder: System.Reflection.Binder; args: array of System.Object; culture: System.Globalization.CultureInfo; activationAttributes: array of System.Object; securityAttributes: System.Security.Policy.Evidence): System.Runtime.Remoting.ObjectHandle;
	public function Load(assemblyRef: System.Reflection.AssemblyName): System.Reflection.Assembly;
	public function Load(assemblyString: string): System.Reflection.Assembly;
	public function Load(rawAssembly: array of byte): System.Reflection.Assembly;
	public function Load(rawAssembly: array of byte; rawSymbolStore: array of byte): System.Reflection.Assembly;
	public function Load(rawAssembly: array of byte; rawSymbolStore: array of byte; securityEvidence: System.Security.Policy.Evidence): System.Reflection.Assembly;
	public function Load(assemblyRef: System.Reflection.AssemblyName; assemblySecurity: System.Security.Policy.Evidence): System.Reflection.Assembly;
	public function Load(assemblyString: string; assemblySecurity: System.Security.Policy.Evidence): System.Reflection.Assembly;
	public function ExecuteAssembly(assemblyFile: string; assemblySecurity: System.Security.Policy.Evidence): integer;
	public function ExecuteAssembly(assemblyFile: string): integer;
	public function ExecuteAssembly(assemblyFile: string; assemblySecurity: System.Security.Policy.Evidence; args: array of string): integer;
	public function get_FriendlyName: string;
	public function get_BaseDirectory: string;
	public function get_RelativeSearchPath: string;
	public function get_ShadowCopyFiles: boolean;
	public function GetAssemblies: array of System.Reflection.Assembly;
	public procedure AppendPrivatePath(path: string);
	public procedure ClearPrivatePath;
	public procedure SetShadowCopyPath(s: string);
	public procedure ClearShadowCopyPath;
	public procedure SetCachePath(s: string);
	public procedure SetData(name: string; data: System.Object);
	public function GetData(name: string): System.Object;
	public procedure SetAppDomainPolicy(domainPolicy: System.Security.Policy.PolicyLevel);
end;


procedure TClass.SetThreadPrincipal(principal: System.Security.Principal.IPrincipal);
begin
  
end;

procedure TClass.SetPrincipalPolicy(policy: System.Security.Principal.PrincipalPolicy);
begin
  
end;

procedure TClass.DoCallBack(theDelegate: System.CrossAppDomainDelegate);
begin
  
end;

function TClass.get_DynamicDirectory: string;
begin
  
end;

procedure TClass.GetTypeInfoCount(var pcTInfo: longword);
begin
  
end;

procedure TClass.GetTypeInfo(iTInfo: longword; lcid: longword; ppTInfo: System.IntPtr);
begin
  
end;

procedure TClass.GetIDsOfNames(var riid: System.Guid; rgszNames: System.IntPtr; cNames: longword; lcid: longword; rgDispId: System.IntPtr);
begin
  
end;

procedure TClass.Invoke(dispIdMember: longword; var riid: System.Guid; lcid: longword; wFlags: smallint; pDispParams: System.IntPtr; pVarResult: System.IntPtr; pExcepInfo: System.IntPtr; puArgErr: System.IntPtr);
begin
  
end;

function TClass.ToString: string;
begin
  
end;

function TClass.Equals(other: System.Object): boolean;
begin
  
end;

function TClass.GetHashCode: integer;
begin
  
end;

function TClass.GetType: System.Type;
begin
  
end;

function TClass.InitializeLifetimeService: System.Object;
begin
  
end;

function TClass.GetLifetimeService: System.Object;
begin
  
end;

function TClass.get_Evidence: System.Security.Policy.Evidence;
begin
  
end;

procedure TClass.add_DomainUnload(value: System.EventHandler);
begin
  
end;

procedure TClass.remove_DomainUnload(value: System.EventHandler);
begin
  
end;

procedure TClass.add_AssemblyLoad(value: System.AssemblyLoadEventHandler);
begin
  
end;

procedure TClass.remove_AssemblyLoad(value: System.AssemblyLoadEventHandler);
begin
  
end;

procedure TClass.add_ProcessExit(value: System.EventHandler);
begin
  
end;

procedure TClass.remove_ProcessExit(value: System.EventHandler);
begin
  
end;

procedure TClass.add_TypeResolve(value: System.ResolveEventHandler);
begin
  
end;

procedure TClass.remove_TypeResolve(value: System.ResolveEventHandler);
begin
  
end;

procedure TClass.add_ResourceResolve(value: System.ResolveEventHandler);
begin
  
end;

procedure TClass.remove_ResourceResolve(value: System.ResolveEventHandler);
begin
  
end;

procedure TClass.add_AssemblyResolve(value: System.ResolveEventHandler);
begin
  
end;

procedure TClass.remove_AssemblyResolve(value: System.ResolveEventHandler);
begin
  
end;

procedure TClass.add_UnhandledException(value: System.UnhandledExceptionEventHandler);
begin
  
end;

procedure TClass.remove_UnhandledException(value: System.UnhandledExceptionEventHandler);
begin
  
end;

function TClass.DefineDynamicAssembly(name: System.Reflection.AssemblyName; access: System.Reflection.Emit.AssemblyBuilderAccess): System.Reflection.Emit.AssemblyBuilder;
begin
  
end;

function TClass.DefineDynamicAssembly(name: System.Reflection.AssemblyName; access: System.Reflection.Emit.AssemblyBuilderAccess; dir: string): System.Reflection.Emit.AssemblyBuilder;
begin
  
end;

function TClass.DefineDynamicAssembly(name: System.Reflection.AssemblyName; access: System.Reflection.Emit.AssemblyBuilderAccess; evidence: System.Security.Policy.Evidence): System.Reflection.Emit.AssemblyBuilder;
begin
  
end;

function TClass.DefineDynamicAssembly(name: System.Reflection.AssemblyName; access: System.Reflection.Emit.AssemblyBuilderAccess; requiredPermissions: System.Security.PermissionSet; optionalPermissions: System.Security.PermissionSet; refusedPermissions: System.Security.PermissionSet): System.Reflection.Emit.AssemblyBuilder;
begin
  
end;

function TClass.DefineDynamicAssembly(name: System.Reflection.AssemblyName; access: System.Reflection.Emit.AssemblyBuilderAccess; dir: string; evidence: System.Security.Policy.Evidence): System.Reflection.Emit.AssemblyBuilder;
begin
  
end;

function TClass.DefineDynamicAssembly(name: System.Reflection.AssemblyName; access: System.Reflection.Emit.AssemblyBuilderAccess; dir: string; requiredPermissions: System.Security.PermissionSet; optionalPermissions: System.Security.PermissionSet; refusedPermissions: System.Security.PermissionSet): System.Reflection.Emit.AssemblyBuilder;
begin
  
end;

function TClass.DefineDynamicAssembly(name: System.Reflection.AssemblyName; access: System.Reflection.Emit.AssemblyBuilderAccess; evidence: System.Security.Policy.Evidence; requiredPermissions: System.Security.PermissionSet; optionalPermissions: System.Security.PermissionSet; refusedPermissions: System.Security.PermissionSet): System.Reflection.Emit.AssemblyBuilder;
begin
  
end;

function TClass.DefineDynamicAssembly(name: System.Reflection.AssemblyName; access: System.Reflection.Emit.AssemblyBuilderAccess; dir: string; evidence: System.Security.Policy.Evidence; requiredPermissions: System.Security.PermissionSet; optionalPermissions: System.Security.PermissionSet; refusedPermissions: System.Security.PermissionSet): System.Reflection.Emit.AssemblyBuilder;
begin
  
end;

function TClass.DefineDynamicAssembly(name: System.Reflection.AssemblyName; access: System.Reflection.Emit.AssemblyBuilderAccess; dir: string; evidence: System.Security.Policy.Evidence; requiredPermissions: System.Security.PermissionSet; optionalPermissions: System.Security.PermissionSet; refusedPermissions: System.Security.PermissionSet; isSynchronized: boolean): System.Reflection.Emit.AssemblyBuilder;
begin
  
end;

function TClass.CreateInstance(assemblyName: string; typeName: string): System.Runtime.Remoting.ObjectHandle;
begin
  
end;

function TClass.CreateInstanceFrom(assemblyFile: string; typeName: string): System.Runtime.Remoting.ObjectHandle;
begin
  
end;

function TClass.CreateInstance(assemblyName: string; typeName: string; activationAttributes: array of System.Object): System.Runtime.Remoting.ObjectHandle;
begin
  
end;

function TClass.CreateInstanceFrom(assemblyFile: string; typeName: string; activationAttributes: array of System.Object): System.Runtime.Remoting.ObjectHandle;
begin
  
end;

function TClass.CreateInstance(assemblyName: string; typeName: string; ignoreCase: boolean; bindingAttr: System.Reflection.BindingFlags; binder: System.Reflection.Binder; args: array of System.Object; culture: System.Globalization.CultureInfo; activationAttributes: array of System.Object; securityAttributes: System.Security.Policy.Evidence): System.Runtime.Remoting.ObjectHandle;
begin
  
end;

function TClass.CreateInstanceFrom(assemblyFile: string; typeName: string; ignoreCase: boolean; bindingAttr: System.Reflection.BindingFlags; binder: System.Reflection.Binder; args: array of System.Object; culture: System.Globalization.CultureInfo; activationAttributes: array of System.Object; securityAttributes: System.Security.Policy.Evidence): System.Runtime.Remoting.ObjectHandle;
begin
  
end;

function TClass.Load(assemblyRef: System.Reflection.AssemblyName): System.Reflection.Assembly;
begin
  
end;

function TClass.Load(assemblyString: string): System.Reflection.Assembly;
begin
  
end;

function TClass.Load(rawAssembly: array of byte): System.Reflection.Assembly;
begin
  
end;

function TClass.Load(rawAssembly: array of byte; rawSymbolStore: array of byte): System.Reflection.Assembly;
begin
  
end;

function TClass.Load(rawAssembly: array of byte; rawSymbolStore: array of byte; securityEvidence: System.Security.Policy.Evidence): System.Reflection.Assembly;
begin
  
end;

function TClass.Load(assemblyRef: System.Reflection.AssemblyName; assemblySecurity: System.Security.Policy.Evidence): System.Reflection.Assembly;
begin
  
end;

function TClass.Load(assemblyString: string; assemblySecurity: System.Security.Policy.Evidence): System.Reflection.Assembly;
begin
  
end;

function TClass.ExecuteAssembly(assemblyFile: string; assemblySecurity: System.Security.Policy.Evidence): integer;
begin
  
end;

function TClass.ExecuteAssembly(assemblyFile: string): integer;
begin
  
end;

function TClass.ExecuteAssembly(assemblyFile: string; assemblySecurity: System.Security.Policy.Evidence; args: array of string): integer;
begin
  
end;

function TClass.get_FriendlyName: string;
begin
  
end;

function TClass.get_BaseDirectory: string;
begin
  
end;

function TClass.get_RelativeSearchPath: string;
begin
  
end;

function TClass.get_ShadowCopyFiles: boolean;
begin
  
end;

function TClass.GetAssemblies: array of System.Reflection.Assembly;
begin
  
end;

procedure TClass.AppendPrivatePath(path: string);
begin
  
end;

procedure TClass.ClearPrivatePath;
begin
  
end;

procedure TClass.SetShadowCopyPath(s: string);
begin
  
end;

procedure TClass.ClearShadowCopyPath;
begin
  
end;

procedure TClass.SetCachePath(s: string);
begin
  
end;

procedure TClass.SetData(name: string; data: System.Object);
begin
  
end;

function TClass.GetData(name: string): System.Object;
begin
  
end;

procedure TClass.SetAppDomainPolicy(domainPolicy: System.Security.Policy.PolicyLevel);
begin
  
end;

function Test(d : Digits): TDiap;
begin
end;

var r : TRec;
    r2 : TRec3;
    s : shortstring;
begin
assert(r2.a1=3);
assert(r2.a2=5);
assert(r2.a3='a');
assert(r2.a4=8.0);
assert(r2.a5='abcd');
assert(r2.a6='abcdef');
assert(r2.a8=three);
assert(r2.a10[two]=1);
assert(r2.a11[true,'a']='cd');
assert(r2.a12=[two,three]);
assert(r2.a13=['a','k','b']);
assert(r2.a17[1]=two);
assert(r.a6 = '');
r.a1 := 3;
r.a2 := 6;
r.a3 := 'x';
r.a4 := 2.4;
r.a5 := 'tz';
r.a6 := 'oio';
r.a7 := 3;
r.a8 := two;
r.a9 := three;
r.a10[three] := 2;
r.a11[false,'c'] := 'pqrs';
Include(r.a12,r.a9);
r.a12 += [two];
r.a13 := [r.a3,'y',Succ('y')];
SetLength(r.a17,4);
r.a17[0] := Digits.four;
Include(r.a19[2,'d',two,true],'b');
assert(r.a1 = 3);
assert(r.a2 = 6);
assert(r.a3 = 'x');
assert(r.a4 = 2.4);
assert(r.a5 = 'tz');
assert(r.a6 = 'oio');
assert(r.a7 = 3);
assert(r.a8 = two);
assert(r.a9 = Pred(four));
assert(r.a10[Digits(2)] = 2);
assert(r.a11[succ(true),Succ('b')] = 'pq'+'r');
assert(r.a12 = [two,three]);
assert(r.a13 = ['x']+['y']+['z']+[]-[]);
assert(r.a17[0] = four);
Assign(r.a18[2],'real_file.dat');
Rewrite(r.a18[2]);
Write(r.a18[2],4.3);
Write(r.a18[2],2.1);
Write(r.a18[2],5.3);
Seek(r.a18[2],1);
var f : real;
Read(r.a18[2],f);
Close(r.a18[2]);
assert(f=2.1);
assert('b' in r.a19[2,'d',two,true]);
end.