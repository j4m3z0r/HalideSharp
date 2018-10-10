#include <Halide.h>

using namespace Halide;

extern "C" void OutputImageParam_Delete(OutputImageParam *self) { delete self; }

extern "C" Expr* OutputImageParam_GetDimensionStride_Int(OutputImageParam *self, int d) { return new Expr(self->dim(d).stride()); }
extern "C" void OutputImageParam_SetDimensionStride_IntExpr(OutputImageParam *self, int d, Expr *s) { self->dim(d).set_stride(*s); }
extern "C" Expr* OutputImageParam_GetDimensionExtent_Int(OutputImageParam *self, int d) { return new Expr(self->dim(d).extent()); }
extern "C" void OutputImageParam_SetDimensionExtent_IntExpr(OutputImageParam *self, int d, Expr *e) { self->dim(d).set_extent(*e); }
