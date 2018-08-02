#include <Halide.h>

using namespace Halide;

extern "C" RVar *RVar_New() { return new RVar(); }
extern "C" RVar *RVar_New_String(const char *name) { return new RVar(name); }
extern "C" void RVar_Delete(RVar *self) { delete self; }
extern "C" Expr *RVar_Min(RVar *self) { return new Expr(self->min()); }
extern "C" Expr *RVar_Extent(RVar *self) { return new Expr(self->extent()); }
extern "C" Expr *Expr_New_RVar(RVar *self) { return new Expr(*self); }

