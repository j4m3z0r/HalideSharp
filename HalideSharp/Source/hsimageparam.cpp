#include <Halide.h>

#include "magicmacros.h"

using namespace Halide;

// NOTE: ImageParam isn't actually parameterized in C++, so we could just go and define methods that treat all ImageParams the same.

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

// 2D indexers
#define IMAGEPARAM_GETEXPR_2D(CSTYPE, CPPTYPE, T1, T2) \
    extern "C" Expr* ImageParamOf ## CSTYPE ## _GetExpr_ ## T1 ## T2 (ImageParam *self, T1 *x, T2 *y) { return new Expr((*self)(*x, *y)); }
#define IMAGEPARAM_GETEXPR_2D_ALLTYPES(T1, T2) GEN(IMAGEPARAM_GETEXPR_2D, T1, T2)
PERMUTE_ARGS_2D(IMAGEPARAM_GETEXPR_2D_ALLTYPES)

// 3D indexers
#define IMAGEPARAM_GETEXPR_3D(CSTYPE, CPPTYPE, T1, T2, T3) \
    extern "C" Expr* ImageParamOf ## CSTYPE ## _GetExpr_ ## T1 ## T2 ## T3(ImageParam *self, T1 *x, T2 *y, T3 *z) { return new Expr((*self)(*x, *y, *z)); }
#define IMAGEPARAM_GETEXPR_3D_ALLTYPES(T1, T2, T3) GEN(IMAGEPARAM_GETEXPR_3D, T1, T2, T3)
PERMUTE_ARGS_3D(IMAGEPARAM_GETEXPR_3D_ALLTYPES)

#define IMAGEPARAM_IN_FUNC(CSTYPE, CPPTYPE) \
    extern "C" Func* ImageParamOf ## CSTYPE ## _In_Func(ImageParam *self, Func *f) { return new Func(self->in(*f)); }
GEN(IMAGEPARAM_IN_FUNC)

