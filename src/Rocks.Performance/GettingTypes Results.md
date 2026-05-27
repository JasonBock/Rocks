## .NET 10.0.8 (10.0.8, 10.0.826.23019), X64 RyuJIT x86-64-v3 (Job: DefaultJob)

```assembly
; Rocks.Performance.GettingTypes.ObtainViaGetType()
       sub       rsp,28
       call      qword ptr [7FF94698D890]; System.Object.GetType()
       nop
       add       rsp,28
       ret
; Total bytes of code 16
```
```assembly
; System.Object.GetType()
       push      rbx
       sub       rsp,20
       mov       rbx,rcx
       mov       rcx,[rbx]
       mov       rax,[rcx+20]
       add       rax,10
       mov       rax,[rax]
       test      rax,rax
       je        short M01_L01
M01_L00:
       add       rsp,20
       pop       rbx
       ret
M01_L01:
       call      qword ptr [7FF9467B5C80]; System.RuntimeTypeHandle.GetRuntimeTypeFromHandleSlow(IntPtr)
       jmp       short M01_L00
; Total bytes of code 41
```

## .NET 10.0.8 (10.0.8, 10.0.826.23019), X64 RyuJIT x86-64-v3 (Job: DefaultJob)

```assembly
; Rocks.Performance.GettingTypes.ObtainViaTypeOf()
       mov       rax,2D125001E88
       ret
; Total bytes of code 11
```

| Method           | Mean      | Error     | StdDev    | Ratio | RatioSD | Code Size | Allocated | Alloc Ratio |
|----------------- |----------:|----------:|----------:|------:|--------:|----------:|----------:|------------:|
| ObtainViaGetType | 0.9102 ns | 0.0151 ns | 0.0141 ns | 1.000 |    0.02 |      57 B |         - |          NA |
| ObtainViaTypeOf  | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.000 |    0.00 |      11 B |         - |          NA |