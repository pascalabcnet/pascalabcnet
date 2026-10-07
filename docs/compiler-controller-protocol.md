# Compiler controller protocol

`PABCCompilerController` is an editor-neutral process that reads one JSON request per line from standard input. By default each request receives exactly one final JSON response. A compile request may opt into progress notifications using `emitEvents: true`; these JSONL notifications precede the unchanged final response. Operational logs and non-protocol worker output are written only to standard error.

The controller starts `PABCCompilerWorker` as an isolated child process. Its internal
transport is also JSON Lines, carried over the worker's redirected standard
input and output. Worker stdout is reserved exclusively for protocol messages;
worker diagnostics are continuously drained from stderr and forwarded to the
controller's stderr. No ZMQ or network transport is used.

The worker binary is `PABCCompilerWorker.dll` on .NET 10 and
`PABCCompilerWorker.exe` on .NET Framework 4.7.2. Deploy Controller and Worker
together; clients explicitly supplying the worker path must use the new name.
The Controller command line and default request/response behaviour are unchanged.

## Optional compiler events

Add `"emitEvents":true` to a `compile` request to receive real `OnChangeCompilerState`
notifications forwarded Worker → Controller → client. Neither process synthesizes phases.

```json
{"id":2,"event":"compilerState","attempt":1,"state":"BeginCompileFile","fileName":"C:\\work\\Program.pas","linesCompiled":0,"errorCount":0,"warningCount":0,"elapsedMilliseconds":12.5}
```

Read until a response without `event` arrives. Notifications have the request's `id`;
`attempt` is 1 or 2 (the existing single retry after worker failure). A retry may
repeat phases; clients reset per-compilation display on `CompilationStarting`.
`Ready` during reload is not compilation success; success statistics follow the
compile lifecycle. Counts come from the compiler; time measures CompilationStarting
through CompilationFinished. Progress does not extend the worker's total 30-second
deadline. Clients not opting in see no event records. Stdout remains JSONL-only.

## Commands

Ping:

```json
{"id":1,"command":"ping"}
```

Compile:

```json
{"id":2,"command":"compile","fileName":"C:\\work\\Program.pas","outputDirectory":"C:\\work\\out"}
```

Compile with an IDE runtime service module:

```json
{"id":3,"command":"compile","fileName":"C:\\work\\Program.pas","outputDirectory":"C:\\work\\out","runtimeModule":"__RedirectIOMode"}
```

Compile an in-memory snapshot of open editor documents:

```json
{"id":4,"command":"compile","fileName":"C:\\work\\Program.pas","outputDirectory":"C:\\work\\out","runtimeModule":"__RedirectIOMode","sourceFiles":[{"fileName":"C:\\work\\Program.pas","text":"uses Helpers; begin Println(GetValue) end."},{"fileName":"C:\\work\\Helpers.pas","text":"unit Helpers; interface function GetValue: integer; implementation function GetValue := 42; end."}]}
```

`sourceFiles` is an optional array of `{ "fileName", "text" }` objects. Each
`fileName` must be an absolute source path (the controller normalizes it before
forwarding the request). `fileName` at the top level still identifies the main
program. A snapshot entry takes precedence over the file with the same path on
disk; files absent from the snapshot are read normally from disk. The main
program and used units may exist only in the snapshot.

Snapshot path comparison follows the host platform: case-insensitive on
Windows and case-sensitive on Linux/macOS. A snapshot is scoped to one compile
request. Snapshot compilations retain the compiler's default PCU-saving behaviour;
the presence of `sourceFiles` does not disable PCU generation. Snapshot entries
use `DateTime.MaxValue` as their source timestamp to invalidate older PCUs.
PCUs saved from editor snapshots remain normal disk cache artifacts. Currently,
switching a later request back to disk sources does not reliably invalidate a
PCU written from different snapshot text (covered by the disk-fallback smoke
regression). This needs a cache-validity fix, not an implicit SavePCU prohibition.
Omitting `sourceFiles` preserves ordinary disk-based compilation behaviour.

Diagnostics contain the normalized path and source coordinates of the actual
main program or unit that produced the error, including virtual snapshot files.
`sourceFiles` and `runtimeModule` may be used together.

`runtimeModule` is an optional string. When present, the worker appends that
module to `CompilerOptions.StandardModules` for every registered language with
`StandardModuleAddMethod.RightToMain`. This is the same ordering used by the
classic PascalABC.NET IDE: the module is added on the right of the `uses` list
of the main program only. The user's source text is not rewritten. Omitting the
field preserves ordinary compilation behaviour.

The named module must be available through the compiler's normal unit search
paths. The standard `__RedirectIOMode` module is included in `Lib`. Start its
compiled .NET 10 program through `dotnet`, pass `[REDIRECTIOMODE]`, redirect all
three standard streams, and write `GO` followed by a newline to stdin. The
module retains the PascalABC.NET IDE protocol on stderr, including
`[READLNSIGNAL]`, `[CODEPAGE...]`, and
`[EXCEPTION]...[MESSAGE]...[STACK]...[END]`.

Restart the worker:

```json
{"id":5,"command":"restart"}
```

Shut down the worker and controller:

```json
{"id":6,"command":"shutdown"}
```

Every response repeats `id` and contains `success`. Compile responses also contain `diagnostics`, `outputFile`, `message`, `fileName`, `compilationCount`, `workerPid`, and `workingSetMB` where applicable.

## Lifecycle

The controller serializes requests and owns one compiler worker. It restarts
that worker after the configured compilation-count or memory threshold and
recovers it after crashes or request timeouts. A timed-out protocol stream is
never reused: the old worker is terminated and the request is retried once in a
new worker. The default request timeout is 30 seconds and may be overridden
with `PABC_COMPILER_WORKER_REQUEST_TIMEOUT_MS`. Graceful shutdown is attempted first and is bounded before forced
termination. The optional command-line arguments remain worker path, maximum
compilation count, and maximum working-set size in MB.

Both projects target .NET Framework 4.7.2 and .NET 10. Build them with `scripts/build-compiler-host.ps1`; validate both runtimes with `scripts/test-compiler-host.ps1`. The modern host is part of the unified `bin-net10` kit built by `scripts/build-net10-runtime.ps1` (`Net10Runtime.slnx`); exports reuse those same binaries rather than building another compiler.
