#include <Halide.h>

#define LANGUAGE_C 1
#include "SharedEnums.cs"
#undef LANGUAGE_C

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

extern "C" Buffer<float> *func_realize_float_2d(Func *f, int width, int height) {
    return new Buffer<float>(f->realize(width, height));
}

extern "C" Buffer<float> *func_realize_float_3d(Func *f, int width, int height, int channels) {
    return new Buffer<float>(f->realize(width, height, channels));
}

extern "C" Buffer<uint8_t> *func_realize_byte_2d(Func *f, int width, int height) {
    return new Buffer<uint8_t>(f->realize(width, height));
}

extern "C" Buffer<uint8_t> *func_realize_byte_3d(Func *f, int width, int height, int channels) {
    return new Buffer<uint8_t>(f->realize(width, height, channels));
}

extern "C" void func_trace_stores(Func *f) {
    f->trace_stores();
}

extern "C" void func_parallel_var(Func *f, Var *v) {
    f->parallel(*v);
}

// TODO: accept a list of Argument objects.
extern "C" void func_compile_to_lowered_stmt(Func *f, const char *filename, enum HSOutputFormat format) {
    f->compile_to_lowered_stmt(filename, {}, (StmtOutputFormat) format);
}