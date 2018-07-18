// This file is a bit of a hack to get a shared list of constants between the C# code and the C code. It's #included
// from the C code, with LANGUAGE_C defined, so that the C# class, namespace, etc stuff is omitted there.

#if LANGUAGE_C
#define public
#endif

#if !LANGUAGE_C
namespace HalideSharp
{
    public class SharedEnums
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

        #if !LANGUAGE_C
        // Unclear why Halide breaks out these values into a second enum, but we mirror the pattern here so as to
        // minimize breakage (now or in the future).
        private const int halide_target_feature_jit = 0;  ///< Generate code that will run immediately inside the calling process.
        private const int halide_target_feature_debug = 1;  ///< Turn on debug info and output for runtime code.
        private const int halide_target_feature_no_asserts = 2;  ///< Disable all runtime checks, for slightly tighter code.
        private const int halide_target_feature_no_bounds_query = 3; ///< Disable the bounds querying functionality.

        private const int halide_target_feature_sse41 = 4;  ///< Use SSE 4.1 and earlier instructions. Only relevant on x86.
        private const int halide_target_feature_avx = 5;  ///< Use AVX 1 instructions. Only relevant on x86.
        private const int halide_target_feature_avx2 = 6;  ///< Use AVX 2 instructions. Only relevant on x86.
        private const int halide_target_feature_fma = 7;  ///< Enable x86 FMA instruction
        private const int halide_target_feature_fma4 = 8;  ///< Enable x86 (AMD) FMA4 instruction set
        private const int halide_target_feature_f16c = 9;  ///< Enable x86 16-bit float support

        private const int halide_target_feature_armv7s = 10;  ///< Generate code for ARMv7s. Only relevant for 32-bit ARM.
        private const int halide_target_feature_no_neon = 11;  ///< Avoid using NEON instructions. Only relevant for 32-bit ARM.

        private const int halide_target_feature_vsx = 12;  ///< Use VSX instructions. Only relevant on POWERPC.
        private const int halide_target_feature_power_arch_2_07 = 13;  ///< Use POWER ISA 2.07 new instructions. Only relevant on POWERPC.

        private const int halide_target_feature_cuda = 14;  ///< Enable the CUDA runtime. Defaults to compute capability 2.0 (Fermi)
        private const int halide_target_feature_cuda_capability30 = 15;  ///< Enable CUDA compute capability 3.0 (Kepler)
        private const int halide_target_feature_cuda_capability32 = 16;  ///< Enable CUDA compute capability 3.2 (Tegra K1)
        private const int halide_target_feature_cuda_capability35 = 17;  ///< Enable CUDA compute capability 3.5 (Kepler)
        private const int halide_target_feature_cuda_capability50 = 18;  ///< Enable CUDA compute capability 5.0 (Maxwell)

        private const int halide_target_feature_opencl = 19;  ///< Enable the OpenCL runtime.
        private const int halide_target_feature_cl_doubles = 20;  ///< Enable double support on OpenCL targets

        private const int halide_target_feature_opengl = 21;  ///< Enable the OpenGL runtime.
        private const int halide_target_feature_openglcompute = 22; ///< Enable OpenGL Compute runtime.

        private const int halide_target_feature_unused_23 = 23; ///< Unused. (Formerly: Enable the RenderScript runtime.)

        private const int halide_target_feature_user_context = 24;  ///< Generated code takes a user_context pointer as first argument

        private const int halide_target_feature_matlab = 25;  ///< Generate a mexFunction compatible with Matlab mex libraries. See tools/mex_halide.m.

        private const int halide_target_feature_profile = 26; ///< Launch a sampling profiler alongside the Halide pipeline that monitors and reports the runtime used by each Func
        private const int halide_target_feature_no_runtime = 27; ///< Do not include a copy of the Halide runtime in any generated object file or assembly

        private const int halide_target_feature_metal = 28; ///< Enable the (Apple) Metal runtime.
        private const int halide_target_feature_mingw = 29; ///< For Windows compile to MinGW toolset rather then Visual Studio

        private const int halide_target_feature_c_plus_plus_mangling = 30; ///< Generate C++ mangled names for result function, et al

        private const int halide_target_feature_large_buffers = 31; ///< Enable 64-bit buffer indexing to support buffers > 2GB. Ignored if bits != 64.

        private const int halide_target_feature_hvx_64 = 32; ///< Enable HVX 64 byte mode.
        private const int halide_target_feature_hvx_128 = 33; ///< Enable HVX 128 byte mode.
        private const int halide_target_feature_hvx_v62 = 34; ///< Enable Hexagon v62 architecture.
        private const int halide_target_feature_fuzz_float_stores = 35; ///< On every floating point store, set the last bit of the mantissa to zero. Pipelines for which the output is very different with this feature enabled may also produce very different output on different processors.
        private const int halide_target_feature_soft_float_abi = 36; ///< Enable soft float ABI. This only enables the soft float ABI calling convention, which does not necessarily use soft floats.
        private const int halide_target_feature_msan = 37; ///< Enable hooks for MSAN support.
        private const int halide_target_feature_avx512 = 38; ///< Enable the base AVX512 subset supported by all AVX512 architectures. The specific feature sets are AVX-512F and AVX512-CD. See https://en.wikipedia.org/wiki/AVX-512 for a description of each AVX subset.
        private const int halide_target_feature_avx512_knl = 39; ///< Enable the AVX512 features supported by Knight's Landing chips, such as the Xeon Phi x200. This includes the base AVX512 set, and also AVX512-CD and AVX512-ER.
        private const int halide_target_feature_avx512_skylake = 40; ///< Enable the AVX512 features supported by Skylake Xeon server processors. This adds AVX512-VL, AVX512-BW, and AVX512-DQ to the base set. The main difference from the base AVX512 set is better support for small integer ops. Note that this does not include the Knight's Landing features. Note also that these features are not available on Skylake desktop and mobile processors.
        private const int halide_target_feature_avx512_cannonlake = 41; ///< Enable the AVX512 features expected to be supported by future Cannonlake processors. This includes all of the Skylake features, plus AVX512-IFMA and AVX512-VBMI.
        private const int halide_target_feature_hvx_use_shared_object = 42; ///< Deprecated
        private const int halide_target_feature_trace_loads = 43; ///< Trace all loads done by the pipeline. Equivalent to calling Func::trace_loads on every non-inlined Func.
        private const int halide_target_feature_trace_stores = 44; ///< Trace all stores done by the pipeline. Equivalent to calling Func::trace_stores on every non-inlined Func.
        private const int halide_target_feature_trace_realizations = 45; ///< Trace all realizations done by the pipeline. Equivalent to calling Func::trace_realizations on every non-inlined Func.
        private const int halide_target_feature_cuda_capability61 = 46;  ///< Enable CUDA compute capability 6.1 (Pascal)
        private const int halide_target_feature_hvx_v65 = 47; ///< Enable Hexagon v65 architecture.
        private const int halide_target_feature_hvx_v66 = 48; ///< Enable Hexagon v66 architecture.
        private const int halide_target_feature_cl_half = 49;  ///< Enable half support on OpenCL targets
        private const int halide_target_feature_end = 50;

        ///< A sentinel. Every target is considered to have this feature, and setting this feature does nothing.
#endif

        public enum HSFeature
        {
            JIT = halide_target_feature_jit,
            Debug = halide_target_feature_debug,
            NoAsserts = halide_target_feature_no_asserts,
            NoBoundsQuery = halide_target_feature_no_bounds_query,
            SSE41 = halide_target_feature_sse41,
            AVX = halide_target_feature_avx,
            AVX2 = halide_target_feature_avx2,
            FMA = halide_target_feature_fma,
            FMA4 = halide_target_feature_fma4,
            F16C = halide_target_feature_f16c,
            ARMv7s = halide_target_feature_armv7s,
            NoNEON = halide_target_feature_no_neon,
            VSX = halide_target_feature_vsx,
            POWER_ARCH_2_07 = halide_target_feature_power_arch_2_07,
            CUDA = halide_target_feature_cuda,
            CUDACapability30 = halide_target_feature_cuda_capability30,
            CUDACapability32 = halide_target_feature_cuda_capability32,
            CUDACapability35 = halide_target_feature_cuda_capability35,
            CUDACapability50 = halide_target_feature_cuda_capability50,
            CUDACapability61 = halide_target_feature_cuda_capability61,
            OpenCL = halide_target_feature_opencl,
            CLDoubles = halide_target_feature_cl_doubles,
            CLHalf = halide_target_feature_cl_half,
            OpenGL = halide_target_feature_opengl,
            OpenGLCompute = halide_target_feature_openglcompute,
            UserContext = halide_target_feature_user_context,
            Matlab = halide_target_feature_matlab,
            Profile = halide_target_feature_profile,
            NoRuntime = halide_target_feature_no_runtime,
            Metal = halide_target_feature_metal,
            MinGW = halide_target_feature_mingw,
            CPlusPlusMangling = halide_target_feature_c_plus_plus_mangling,
            LargeBuffers = halide_target_feature_large_buffers,
            HVX_64 = halide_target_feature_hvx_64,
            HVX_128 = halide_target_feature_hvx_128,
            HVX_v62 = halide_target_feature_hvx_v62,
            HVX_v65 = halide_target_feature_hvx_v65,
            HVX_v66 = halide_target_feature_hvx_v66,
            HVX_shared_object = halide_target_feature_hvx_use_shared_object,
            FuzzFloatStores = halide_target_feature_fuzz_float_stores,
            SoftFloatABI = halide_target_feature_soft_float_abi,
            MSAN = halide_target_feature_msan,
            AVX512 = halide_target_feature_avx512,
            AVX512_KNL = halide_target_feature_avx512_knl,
            AVX512_Skylake = halide_target_feature_avx512_skylake,
            AVX512_Cannonlake = halide_target_feature_avx512_cannonlake,
            TraceLoads = halide_target_feature_trace_loads,
            TraceStores = halide_target_feature_trace_stores,
            TraceRealizations = halide_target_feature_trace_realizations,
            FeatureEnd = halide_target_feature_end
        };

#if !LANGUAGE_C
    }
}
#endif

#if LANGUAGE_C
#undef public
#endif