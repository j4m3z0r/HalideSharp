#include <Halide.h>

extern "C" Halide::Func *new_func(const char *name) {
    return new Halide::Func(name);
}

extern "C" void delete_func(Halide::Func *f) {
    delete f;
}

extern "C" void func_set_var_var_expr(Halide::Func *f, Halide::Var *v1, Halide::Var *v2, Halide::Expr *e) {
    (*f)(*v1, *v2) = *e;
}

extern "C" Halide::Buffer<int32_t> *func_realize_int(Halide::Func *f, int width, int height) {
    return new Halide::Buffer<int32_t>(f->realize(width, height));
}
