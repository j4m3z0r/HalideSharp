#ifndef __MAGIC_MACROS_H__
#define __MAGIC_MACROS_H__

#define PERMUTE_ARGS_1D(MACRO) \
    MACRO(Expr) \
    MACRO(Var)

#define PERMUTE_ARGS_2D(MACRO) \
    MACRO(Expr, Expr) \
    MACRO(Expr, Var) \
    MACRO(Var, Expr) \
    MACRO(Var, Var)

#define PERMUTE_ARGS_3D(MACRO) \
    MACRO(Expr, Expr, Expr) \
    MACRO(Expr, Expr, Var) \
    MACRO(Expr, Var, Expr) \
    MACRO(Expr, Var, Var) \
    MACRO(Var, Expr, Expr) \
    MACRO(Var, Expr, Var) \
    MACRO(Var, Var, Expr) \
    MACRO(Var, Var, Var)

// GEN will invoke the given macro once for each set of types we handle, passing the C# type
// as the first argument, the corresponding C++ type as the second argument, and then any
// further arguments that were specified. Currently it will generate variants for ints, floats,
// and bytes.
#define GEN(MACRO, ...) \
    MACRO(Int, int32_t, ## __VA_ARGS__) \
    MACRO(Float, float, ## __VA_ARGS__) \
    MACRO(Byte, uint8_t, ## __VA_ARGS__)

// Short for HalideSharpTypes. Keeping things concise since this will be used everywhere.
namespace HST {
    typedef int32_t Int;
    typedef float Float;
    typedef uint8_t Byte;
}

#endif
