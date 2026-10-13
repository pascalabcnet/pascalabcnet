# Documentation comment regression tests

Run the same test program for `net472` and `net10.0`:

```powershell
dotnet run --project tests/DocumentationCommentTests -c Release -f net472
dotnet run --project tests/DocumentationCommentTests -c Release -f net10.0
```

Checks CRLF, LF and both alternating mixed-line-ending layouts, with and without
a trailing period. Verifies grouping of consecutive `///` lines, empty doc lines,
source ranges, attachment to adjacent declarations, and complete hover text through
the actual shared CodeCompletion implementation. Ordinary blank lines still separate
documentation blocks. Also checks hover on a class and overloaded constructor
declarations immediately after comments ending in a period, including the actual
constructor signature rather than a synthetic default. Multiline member access,
with and without line comments, remains intact. All Pascal input is in memory;
no user modules are changed.
