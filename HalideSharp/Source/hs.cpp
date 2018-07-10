#include <Halide.h>

using namespace Halide;

extern "C" Expr *cast_to_float(Expr *expr) {
    auto casted_expr = cast<float>(*expr);
    return new Expr(casted_expr);
}

extern "C" Expr *cast_to_byte(Expr *expr) {
    auto casted_expr = cast<uint8_t>(*expr);
    return new Expr(casted_expr);
}

extern "C" Expr* min_expr_float(Expr* expr, float f) {
    return new Expr(min(*expr, f));
}