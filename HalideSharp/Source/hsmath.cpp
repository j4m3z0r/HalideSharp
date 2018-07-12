#include <Halide.h>

using namespace Halide;

extern "C" Expr *sin_var(Var *v) { return new Expr(sin(*v)); }
extern "C" Expr *sin_expr(Expr *e) { return new Expr(sin(*e)); }
extern "C" Expr *cos_var(Var *v) { return new Expr(cos(*v)); }
extern "C" Expr *cos_expr(Expr *e) { return new Expr(cos(*e)); }
extern "C" Expr *pow_expr_float(Expr *e, float f) { return new Expr(pow(*e, f)); }
 
