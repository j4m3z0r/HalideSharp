#include <Halide.h>

#include "magicmacros.h"

using namespace Halide;

#define MATHFN_ONE_ARG(CSNAME, CPPNAME, T) \
    extern "C" Expr* Global_ ## CSNAME ## _ ## T(argtype(T) a) { \
        return new Expr(CPPNAME(deref(T, a))); \
    }

#define ALL_MATHFN_ONE_ARG(T) \
    MATHFN_ONE_ARG(Sin, sin, T) \
    MATHFN_ONE_ARG(Asin, asin, T) \
    MATHFN_ONE_ARG(Cos, cos, T) \
    MATHFN_ONE_ARG(Acos, acos, T) \
    MATHFN_ONE_ARG(Tan, tan, T) \
    MATHFN_ONE_ARG(Atan, atan, T) \
    MATHFN_ONE_ARG(Sinh, sinh, T) \
    MATHFN_ONE_ARG(Asinh, asinh, T) \
    MATHFN_ONE_ARG(Cosh, cosh, T) \
    MATHFN_ONE_ARG(Acosh, acosh, T) \
    MATHFN_ONE_ARG(Tanh, tanh, T) \
    MATHFN_ONE_ARG(Atanh, atanh, T) \
    MATHFN_ONE_ARG(Sqrt, sqrt, T) \
    MATHFN_ONE_ARG(Exp, exp, T) \
    MATHFN_ONE_ARG(Log, log, T) \
    MATHFN_ONE_ARG(Erf, erf, T) \
    MATHFN_ONE_ARG(FastInverse, fast_inverse, T) \
    MATHFN_ONE_ARG(FastInverseSqrt, fast_inverse_sqrt, T) \
    MATHFN_ONE_ARG(Floor, floor, T) \
    MATHFN_ONE_ARG(Ceil, ceil, T) \
    MATHFN_ONE_ARG(Round, round, T) \
    MATHFN_ONE_ARG(Trunc, trunc, T) \
    MATHFN_ONE_ARG(IsNan, is_nan, T) \
    MATHFN_ONE_ARG(Fract, fract, T) \
    MATHFN_ONE_ARG(Abs, abs, T)

PERMUTE_ARGS_1D(ALL_MATHFN_ONE_ARG)

// Note: we construct a new Expr object wrapping each of the args to avoid
// amgiguous method resolution (specifically when both arguments are ints)
#define MATHFN_TWO_ARG(CSNAME, CPPNAME, CSTYPE1, CSTYPE2) \
    extern "C" Expr* Global_ ## CSNAME ## _ ## CSTYPE1 ## CSTYPE2( \
        argtype(CSTYPE1) a, \
        argtype(CSTYPE2) b \
    ) { \
        return new Expr(CPPNAME(Expr(deref(CSTYPE1, a)), Expr(deref(CSTYPE2, b)))); \
    }

#define ALL_MATHFN_TWO_ARG(CSTYPE1, CSTYPE2) \
    MATHFN_TWO_ARG(Atan2, atan2, CSTYPE1, CSTYPE2) \
    MATHFN_TWO_ARG(Hypot, hypot, CSTYPE1, CSTYPE2) \
    MATHFN_TWO_ARG(Pow, pow, CSTYPE1, CSTYPE2) \
    MATHFN_TWO_ARG(FastPow, fast_pow, CSTYPE1, CSTYPE2) \
    MATHFN_TWO_ARG(Min, min, CSTYPE1, CSTYPE2) \
    MATHFN_TWO_ARG(Max, max, CSTYPE1, CSTYPE2) \
    MATHFN_TWO_ARG(AbsD, absd, CSTYPE1, CSTYPE2)


PERMUTE_ARGS_2D(ALL_MATHFN_TWO_ARG)

extern "C" Expr* Global_Lerp_ExprExprExpr(Expr *zeroVal, Expr *oneVal, Expr *weight) {
    return new Expr(lerp(*zeroVal, *oneVal, *weight));
}
