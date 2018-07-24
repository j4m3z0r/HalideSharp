#ifndef __MAGIC_MACROS_H__
#define __MAGIC_MACROS_H__

#define PERMUTE_ARGS_1D(MACRO) \
    MACRO(Expr) \
    MACRO(Var) \
    MACRO(RDom)

#define PERMUTE_ARGS_2D(MACRO) \
    MACRO(Expr, Expr) \
    MACRO(Expr, Var) \
    MACRO(Expr, RDom) \
    MACRO(Var, Expr) \
    MACRO(Var, Var) \
    MACRO(Var, RDom) \
    MACRO(RDom, Expr) \
    MACRO(RDom, Var) \
    MACRO(RDom, RDom)

#define PERMUTE_ARGS_3D(MACRO) \
    MACRO(Expr, Expr, Expr) \
    MACRO(Expr, Expr, Var) \
    MACRO(Expr, Expr, RDom) \
    MACRO(Expr, Var, Expr) \
    MACRO(Expr, Var, Var) \
    MACRO(Expr, Var, RDom) \
    MACRO(Expr, RDom, Expr) \
    MACRO(Expr, RDom, Var) \
    MACRO(Expr, RDom, RDom) \
    MACRO(Var, Expr, Expr) \
    MACRO(Var, Expr, Var) \
    MACRO(Var, Expr, RDom) \
    MACRO(Var, Var, Expr) \
    MACRO(Var, Var, Var) \
    MACRO(Var, Var, RDom) \
    MACRO(Var, RDom, Expr) \
    MACRO(Var, RDom, Var) \
    MACRO(Var, RDom, RDom) \
    MACRO(RDom, Expr, Expr) \
    MACRO(RDom, Expr, Var) \
    MACRO(RDom, Expr, RDom) \
    MACRO(RDom, Var, Expr) \
    MACRO(RDom, Var, Var) \
    MACRO(RDom, Var, RDom) \
    MACRO(RDom, RDom, Expr) \
    MACRO(RDom, RDom, Var) \
    MACRO(RDom, RDom, RDom)

// Ensure the set of types here matches those listed in MagicMacros.ecs.

// GEN will invoke the given macro once for each set of types we handle, passing the C# type
// as the first argument, the corresponding C++ type as the second argument, and then any
// further arguments that were specified. Currently it will generate variants for ints, floats,
// and bytes.
#define GEN(MACRO, ...) \
    MACRO(Int, int32_t, ## __VA_ARGS__) \
    MACRO(UInt, uint32_t, ## __VA_ARGS__) \
    MACRO(Float, float, ## __VA_ARGS__) \
    MACRO(Short, int16_t, ## __VA_ARGS__) \
    MACRO(UShort, uint16_t, ## __VA_ARGS__) \
    MACRO(SByte, int8_t, ## __VA_ARGS__) \
    MACRO(Byte, uint8_t, ## __VA_ARGS__)

// Variant of GEN() that also passes in the Halide type, for use in ImageParam constructors, etc.
#define GEN_HALIDETYPES(MACRO, ...) \
    MACRO(Int, int32_t, Int(32), ## __VA_ARGS__) \
    MACRO(UInt, uint32_t, UInt(32), ## __VA_ARGS__) \
    MACRO(Float, float, Float(32), ## __VA_ARGS__) \
    MACRO(Short, int16_t, Int(16), ## __VA_ARGS__) \
    MACRO(UShort, uint16_t, UInt(16), ## __VA_ARGS__) \
    MACRO(SByte, int8_t, Int(8), ## __VA_ARGS__) \
    MACRO(Byte, uint8_t, UInt(8), ## __VA_ARGS__)


// Short for HalideSharpTypes. Keeping things concise since this will be used everywhere.
namespace HST {
    typedef int32_t Int;
    typedef uint32_t UInt;
    typedef float Float;
    typedef int16_t Short;
    typedef uint16_t UShort;
    typedef int8_t SByte;
    typedef uint8_t Byte;
}

#define DEREF_POINTER(a) (*(a))
#define DEREF_NOPOINTER(a) (a)

#endif
