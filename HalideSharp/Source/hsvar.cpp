#include <Halide.h>

extern "C" Halide::Var *new_var(const char* name) {
    return new Halide::Var(name);
}

extern "C" void delete_var(Halide::Var *v) {
    delete v;
}

extern "C" Halide::Expr *var_plus_var(Halide::Var *v1, Halide::Var *v2) {
    return new Halide::Expr(*v1 + *v2);
}

