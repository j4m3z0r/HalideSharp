#include <Halide.h>

using namespace Halide;

extern "C" Func *new_func(const char *name) {
    return new Func(name);
}

extern "C" void delete_func(Func *f) {
    delete f;
}

extern "C" void func_set_var_var_expr(Func *f, Var *v1, Var *v2, Expr *e) {
    (*f)(*v1, *v2) = *e;
}

extern "C" void func_set_var_var_var_expr(Func *f, Var *v1, Var *v2, Var *v3, Expr *e) {
    (*f)(*v1, *v2, *v3) = *e;
}

extern "C" Buffer<int32_t> *func_realize_int_2d(Func *f, int width, int height) {
    return new Buffer<int32_t>(f->realize(width, height));
}

extern "C" Buffer<int32_t> *func_realize_int_3d(Func *f, int width, int height, int channels) {
    return new Buffer<int32_t>(f->realize(width, height, channels));
}

extern "C" Buffer<uint8_t> *func_realize_byte_2d(Func *f, int width, int height) {
    return new Buffer<uint8_t>(f->realize(width, height));
}

extern "C" Buffer<uint8_t> *func_realize_byte_3d(Func *f, int width, int height, int channels) {
    return new Buffer<uint8_t>(f->realize(width, height, channels));
}
