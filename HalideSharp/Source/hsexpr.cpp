#include <Halide.h>

extern "C" void delete_expr(Halide::Expr *e) {
    delete e;
}

