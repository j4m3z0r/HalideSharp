#include <Halide.h>

using namespace Halide;

extern "C" Var *new_var(const char* name) {
    return new Var(name);
}

extern "C" void delete_var(Var *v) {
    delete v;
}

extern "C" Expr *var_plus_var(Var *v1, Var *v2) {
    return new Expr((*v1) + (*v2));
}

extern "C" Expr* var_mult_var(Var *v1, Var *v2) {
    return new Expr((*v1) * (*v2));
}

extern "C" Expr* var_equals_int(Var *v, int i) {
    return new Expr((*v) == i);
}

extern "C" Expr* var_not_equals_int(Var *v, int i) {
    return new Expr((*v) != i);
}
