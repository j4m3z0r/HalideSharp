#include <Halide.h>

using namespace Halide;

extern "C" void delete_expr(Expr *e) {
    delete e;
}

extern "C" Expr* expr_mult_float(Expr* e, float f) {
    return new Expr((*e) * f);
}