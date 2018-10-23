#ifndef __MAGIC_MACROS_H__
#define __MAGIC_MACROS_H__

#define DEREF_POINTER(a) (*(a))
#define DEREF_NOPOINTER(a) (a)

/**
 * The PERMUTE_ARGS_<n>D macros invoke the given macro with all the
 * combinations of types for <n> arguments. This is necessary since indexers
 * are able to take any combination of variables, expressions, args, ints, etc
 * as arguments. The repetition of the argument types across all macros seems
 * to be necessary -- a "meta-macro" that takes the given macro as an argument
 * and a continuation macro won't expand the continuation. This is the least
 * bad formulation of this I was able to find.
 **/

// T0, T1,..., Tn each substitute a value for the nth argument. The T* macros
// should be considered "private" -- callers should use the PERMUTE_ARGS
// macros.
#define T0(macro, ...) \
    macro(__VA_ARGS__)

#define T1(macro, ...)  \
    T0(macro, Expr, __VA_ARGS__) \
    T0(macro, RDom, __VA_ARGS__)

#define T2(macro, ...) \
    T1(macro, Expr, __VA_ARGS__) \
    T1(macro, RDom, __VA_ARGS__)

#define PERMUTE_ARGS_1D(macro) \
    T0(macro, Expr) \
    T0(macro, RDom)

#define PERMUTE_ARGS_2D(macro) \
    T1(macro, Expr) \
    T1(macro, RDom)

#define PERMUTE_ARGS_3D(macro) \
    T2(macro, Expr) \
    T2(macro, RDom)

/**
 * The _argtype macros are used to convert the type names used in the PERMUTE
 * macros into the types of the arguments that the function should accept.
 * Callers should use the argtype macro, rather than doing the concatenation
 * themselves.
 **/
#define Expr_argtype Expr*
#define Var_argtype Var*
#define RDom_argtype RDom*
#define Int_argtype int
#define Float_argtype int

#define argtype(a) a ## _argtype

/**
 * The _deref macros indicate how to dereference an argument of the given type,
 * as mapped through the _argtype macros. Eg: ints do not need to be
 * dereferenced, whereas Expr*'s do.
 **/
#define Var_deref(p) (*(p))
#define Expr_deref(p) (*(p))
#define RDom_deref(p) (*(p))
#define Int_deref(p) (p)
#define Float_deref(p) (p)

#define deref(type, val) type ## _deref(val)

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

#endif
