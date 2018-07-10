#include <Halide.h>

using namespace Halide;

extern "C" Expr *sin_var(Var *v) {
    return new Expr(sin(*v));
}

extern "C" Expr *cos_var(Var *v) {
    return new Expr(cos(*v));
}
