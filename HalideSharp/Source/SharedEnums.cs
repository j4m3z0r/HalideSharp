// This file is a bit of a hack to get a shared list of constants between the C# code and the C code. It's #included
// from the C code, with LANGUAGE_C defined, so that the C# class, namespace, etc stuff is omitted there.

#if LANGUAGE_C
#define public
#endif

#if !LANGUAGE_C
namespace HalideSharp
{
#endif

    public enum HSObjectType
    {
        HS_String = 0,
        HS_Var = 1,
        HS_Expr = 2
    };

    public enum HSOutputFormat
    {
        HS_Text = 0,
        HS_HTML = 1
    };

    public enum HSOperatingSystem
    {
        OSUnknown = 0,
        Linux,
        Windows,
        OSX,
        Android,
        IOS,
        QuRT,
        NoOS
    };

    public enum HSArchitecture
    {
        ArchUnknown = 0,
        X86,
        ARM,
        MIPS,
        Hexagon,
        POWERPC
    };

    public enum HSFeature
    {
        /// <summary>Generate code that will run immediately inside the calling process.</summary>
        JIT = 0,

        /// <summary>Turn on debug info and output for runtime code.</summary>
        Debug = 1,

        /// <summary>Disable all runtime checks, for slightly tighter code.</summary>
        NoAsserts = 2,

        /// <summary>Disable the bounds querying functionality.</summary>
        NoBoundsQuery = 3,

        /// <summary>Use SSE 4.1 and earlier instructions. Only relevant on x86.</summary>
        SSE41 = 4,

        /// <summary>Use AVX 1 instructions. Only relevant on x86.</summary>
        AVX = 5,

        /// <summary>Use AVX 2 instructions. Only relevant on x86.</summary>
        AVX2 = 6,

        /// <summary>Enable x86 FMA instruction</summary>
        FMA = 7,

        /// <summary>Enable x86 (AMD) FMA4 instruction set</summary>
        FMA4 = 8,

        /// <summary>Enable x86 16-bit float support</summary>
        F16C = 9,

        /// <summary>Generate code for ARMv7s. Only relevant for 32-bit ARM.</summary>
        ARMv7s = 10,

        /// <summary>Avoid using NEON instructions. Only relevant for 32-bit ARM.</summary>
        NoNEON = 11,

        /// <summary>Use VSX instructions. Only relevant on POWERPC.</summary>
        VSX = 12,

        /// <summary>Use POWER ISA 2.07 new instructions. Only relevant on POWERPC.</summary>
        POWER_ARCH_2_07 = 13,

        /// <summary>Enable the CUDA runtime. Defaults to compute capability 2.0 (Fermi)</summary>
        CUDA = 14,

        /// <summary>Enable CUDA compute capability 3.0 (Kepler)</summary>
        CUDACapability30 = 15,

        /// <summary>Enable CUDA compute capability 3.2 (Tegra K1)</summary>
        CUDACapability32 = 16,

        /// <summary>Enable CUDA compute capability 3.5 (Kepler)</summary>
        CUDACapability35 = 17,

        /// <summary>Enable CUDA compute capability 5.0 (Maxwell)</summary>
        CUDACapability50 = 18,

        /// <summary>Enable CUDA compute capability 6.1 (Pascal)</summary>
        CUDACapability61 = 46,

        /// <summary>Enable the OpenCL runtime.</summary>
        OpenCL = 19,

        /// <summary>Enable double support on OpenCL targets</summary>
        CLDoubles = 20,

        /// <summary>Enable half support on OpenCL targets</summary>
        CLHalf = 49,

        /// <summary>Enable the OpenGL runtime.</summary>
        OpenGL = 21,

        /// <summary>Enable OpenGL Compute runtime.</summary>
        OpenGLCompute = 22,

        /// <summary>Generated code takes a user_context pointer as first argument</summary>
        UserContext = 24,

        /// <summary>Generate a mexFunction compatible with Matlab mex libraries. See tools/mex_halide.m.</summary>
        Matlab = 25,

        /// <summary>Launch a sampling profiler alongside the Halide pipeline that monitors and reports the runtime
        // used by each Func</summary>
        Profile = 26,

        /// <summary>Do not include a copy of the Halide runtime in any generated object file or assembly</summary>
        NoRuntime = 27,

        /// <summary>Enable the (Apple) Metal runtime.</summary>
        Metal = 28,

        /// <summary>For Windows compile to MinGW toolset rather then Visual Studio</summary>
        MinGW = 29,

        /// <summary>Generate C++ mangled names for result function, et al</summary>
        CPlusPlusMangling = 30,

        /// <summary>Enable 64-bit buffer indexing to support buffers > 2GB. Ignored if bits != 64.</summary>
        LargeBuffers = 31,

        /// <summary>Enable HVX 64 byte mode.</summary>
        HVX_64 = 32,

        /// <summary>Enable HVX 128 byte mode.</summary>
        HVX_128 = 33,

        /// <summary>Enable Hexagon v62 architecture.</summary>
        HVX_v62 = 34,

        /// <summary>Enable Hexagon v65 architecture.</summary>
        HVX_v65 = 47,

        /// <summary>Enable Hexagon v66 architecture.</summary>
        HVX_v66 = 48,

        /// <summary>Deprecated</summary>
        HVX_shared_object = 42,

        /// <summary>On every floating point store, set the last bit of the mantissa to zero. Pipelines for which
        /// the output is very different with this feature enabled may also produce very different output on different
        /// processors.</summary>
        FuzzFloatStores = 35,

        /// <summary>Enable soft float ABI. This only enables the soft float ABI calling convention, which does not
        /// necessarily use soft floats.</summary>
        SoftFloatABI = 36,

        /// <summary>Enable hooks for MSAN support.</summary>
        MSAN = 37,

        /// <summary>Enable the base AVX512 subset supported by all AVX512 architectures. The specific feature sets
        /// are AVX-512F and AVX512-CD. See https://en.wikipedia.org/wiki/AVX-512 for a description of each AVX
        /// subset.</summary>
        AVX512 = 38,

        /// <summary>Enable the AVX512 features supported by Knight's Landing chips, such as the Xeon Phi x200. This
        /// includes the base AVX512 set, and also AVX512-CD and AVX512-ER.</summary>
        AVX512_KNL = 39,

        /// <summary>Enable the AVX512 features supported by Skylake Xeon server processors. This adds AVX512-VL,
        /// AVX512-BW, and AVX512-DQ to the base set. The main difference from the base AVX512 set is better support
        /// for small integer ops. Note that this does not include the Knight's Landing features. Note also that these
        /// features are not available on Skylake desktop and mobile processors.</summary>
        AVX512_Skylake = 40,

        /// <summary>Enable the AVX512 features expected to be supported by future Cannonlake processors. This
        /// includes all of the Skylake features, plus AVX512-IFMA and AVX512-VBMI.</summary>
        AVX512_Cannonlake = 41,

        /// <summary>Trace all loads done by the pipeline. Equivalent to calling Func::trace_loads on every
        /// non-inlined Func.</summary>
        TraceLoads = 43,

        /// <summary>Trace all stores done by the pipeline. Equivalent to calling Func::trace_stores on every
        /// non-inlined Func.</summary>
        TraceStores = 44,

        /// <summary>Trace all realizations done by the pipeline. Equivalent to calling Func::trace_realizations on
        /// every non-inlined Func.</summary>
        TraceRealizations = 45,

        /// <summary>A sentinel. Every target is considered to have this feature, and setting this feature does
        /// nothing.</summary>
        FeatureEnd = 50,
    };
#if !LANGUAGE_C
}
#endif

#if LANGUAGE_C
#undef public
#endif
