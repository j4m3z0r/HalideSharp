#include <Halide.h>

extern "C" Halide::Expr *cast_to_float(Halide::Expr *expr) {
    auto casted_expr = Halide::cast<float>(*expr);
    return new Halide::Expr(casted_expr);
}

extern "C" Halide::Expr *cast_to_byte(Halide::Expr *expr) {
    auto casted_expr = Halide::cast<uint8_t>(*expr);
    return new Halide::Expr(casted_expr);
}
