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
#if !LANGUAGE_C
    }
}
#endif

#if LANGUAGE_C
#undef public
#endif