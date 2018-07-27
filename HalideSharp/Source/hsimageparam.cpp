#include <Halide.h>

#include "magicmacros.h"

using namespace Halide;

// NOTE: ImageParam isn't actually parameterized in C++, so we could just go
// and define methods that treat all ImageParams the same. We're generating it
// as if it had a templated type argument to let us re-use some of the code
// from Buffers, plus it makes stack traces slightly more readable.

#define CONSTRUCTOR_INT(CSTYPE, CPPTYPE, HTYPE) \
    extern "C" ImageParam* ImageParamOf ## CSTYPE ## _New_Int(int d) { return new ImageParam(HTYPE, d); }

#define CONSTRUCTOR_INT_STR(CSTYPE, CPPTYPE, HTYPE) \
    extern "C" ImageParam* ImageParamOf ## CSTYPE ## _New_IntString(int d, char *n) {return new ImageParam(HTYPE, d, n); }

GEN_HALIDETYPES(CONSTRUCTOR_INT)
GEN_HALIDETYPES(CONSTRUCTOR_INT_STR)

#define DESTRUCTOR(CSTYPE, CPPTYPE, HTYPE) \
    extern "C" void ImageParamOf ## CSTYPE ## _Delete(ImageParam *ip) { delete ip; }
GEN_HALIDETYPES(DESTRUCTOR)

// Setters
#define IMAGEPARAM_SET(CSTYPE, CPPTYPE) \
    extern "C" void ImageParamOf ## CSTYPE ## _Set_BufferOf ## CSTYPE(ImageParam *self, Buffer<CPPTYPE> *b) { self->set(*b); }
GEN(IMAGEPARAM_SET);

// 1D indexers
#define IMAGEPARAM_GETEXPR_1D(CSTYPE, CPPTYPE, T1) \
    extern "C" Expr* ImageParamOf ## CSTYPE ## _GetExpr_ ## T1 ( \
        ImageParam *self, \
        argtype(T1) x \
    ) { \
        return new Expr((*self)(deref(T1, x))); \
    }

// 2D indexers
#define IMAGEPARAM_GETEXPR_2D(CSTYPE, CPPTYPE, T1, T2) \
    extern "C" Expr* ImageParamOf ## CSTYPE ## _GetExpr_ ## T1 ## T2 ( \
        ImageParam *self, \
        argtype(T1) x, \
        argtype(T2) y \
    ) { \
        return new Expr((*self)(deref(T1, x), deref(T2, y))); \
    }

// 3D indexers
#define IMAGEPARAM_GETEXPR_3D(CSTYPE, CPPTYPE, T1, T2, T3) \
    extern "C" Expr* ImageParamOf ## CSTYPE ## _GetExpr_ ## T1 ## T2 ## T3( \
        ImageParam *self, \
        argtype(T1) x, \
        argtype(T2) y, \
        argtype(T3) z \
    ) { \
        return new Expr((*self)(deref(T1, x), deref(T2, y), deref(T3, z))); \
    }

#define IMAGEPARAM_GETEXPR_1D_ALLTYPES(T1) GEN(IMAGEPARAM_GETEXPR_1D, T1)
#define IMAGEPARAM_GETEXPR_2D_ALLTYPES(T1, T2) GEN(IMAGEPARAM_GETEXPR_2D, T1, T2)
#define IMAGEPARAM_GETEXPR_3D_ALLTYPES(T1, T2, T3) GEN(IMAGEPARAM_GETEXPR_3D, T1, T2, T3)

PERMUTE_ARGS_1D(IMAGEPARAM_GETEXPR_1D_ALLTYPES)
PERMUTE_ARGS_2D(IMAGEPARAM_GETEXPR_2D_ALLTYPES)
PERMUTE_ARGS_3D(IMAGEPARAM_GETEXPR_3D_ALLTYPES)

#define IMAGEPARAM_IN_FUNC(CSTYPE, CPPTYPE) \
    extern "C" Func* ImageParamOf ## CSTYPE ## _In_Func(ImageParam *self, Func *f) { return new Func(self->in(*f)); }
GEN(IMAGEPARAM_IN_FUNC)

// Dimension accessors
#define GET_DIMENSION_FIELD(CSTYPE, CPPTYPE, CSFIELD, CPPFIELD) \
    extern "C" Expr* ImageParamOf ## CSTYPE ## _GetDimension ## CSFIELD ## _Int( \
        ImageParam *self, \
        int d \
    ) { \
        return new Expr(self->dim(d).CPPFIELD()); \
    }

#define SET_DIMENSION_FIELD(CSTYPE, CPPTYPE, CSFIELD, CPPFIELD) \
    extern "C" void ImageParamOf ## CSTYPE ## _SetDimension ## CSFIELD ## _IntInt( \
        ImageParam *self, \
        int d, \
        int val \
    ) { \
        self->dim(d).set_ ## CPPFIELD(val); \
    }


#define DIMENSION(CSTYPE, CPPTYPE) \
    GET_DIMENSION_FIELD(CSTYPE, CPPTYPE, Stride, stride) \
    SET_DIMENSION_FIELD(CSTYPE, CPPTYPE, Stride, stride) \
    GET_DIMENSION_FIELD(CSTYPE, CPPTYPE, Extent, extent) \
    SET_DIMENSION_FIELD(CSTYPE, CPPTYPE, Extent, extent)
GEN(DIMENSION)

