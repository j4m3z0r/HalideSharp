#include <Halide.h>

#include "magicmacros.h"

#define LANGUAGE_C 1
#include "SharedEnums.cs"
#undef LANGUAGE_C

using namespace Halide;

extern "C" Func *new_func(const char *name) { return new Func(name); }
extern "C" void delete_func(Func *f) { delete f; }

// 1D indexers
#define FUNC_INDEXER_1D(T1) \
    extern "C" Expr* func_get_Expr__ ## T1(Func* f, T1* x) { return new Expr((*f)(*x)); } \
    extern "C" void func_set_Expr_ ## T1 ## __Expr(Func* f, T1* x, Expr* e) { (*f)(*x) = *e; }

PERMUTE_ARGS_1D(FUNC_INDEXER_1D)

// 2D indexers
#define FUNC_INDEXER_2D(T1, T2) \
    extern "C" Expr* func_get_Expr__ ## T1 ## _ ## T2(Func* f, T1* x, T2* y) { return new Expr((*f)(*x, *y)); } \
    extern "C" void func_set_Expr_ ## T1 ## _ ## T2 ## __Expr(Func* f, T1* x, T2* y, Expr* e) { (*f)(*x, *y) = *e; }

PERMUTE_ARGS_2D(FUNC_INDEXER_2D)

// 3D indexers
#define FUNC_INDEXER_3D(T1, T2, T3) \
    extern "C" Expr* func_get_Expr__ ## T1 ## _ ## T2 ## _ ## T3(Func* f, T1* x, T2* y, T3 *z) { return new Expr((*f)(*x, *y, *z)); } \
    extern "C" void func_set_Expr_ ## T1 ## _ ## T2 ## _ ## T3 ## __Expr(Func* f, T1* x, T2* y, T3* z, Expr* e) { (*f)(*x, *y, *z) = *e; }

PERMUTE_ARGS_3D(FUNC_INDEXER_3D)



extern "C" void func_realize_int_buffer(Func *f, Buffer<int32_t> *buffer) { f->realize(*buffer); }
extern "C" void func_realize_float_buffer(Func *f, Buffer<float> *buffer) { f->realize(*buffer); }
extern "C" void func_realize_byte_buffer(Func *f, Buffer<uint8_t> *buffer) { f->realize(*buffer); }

extern "C" Buffer<int32_t> *func_realize_int_2d(Func *f, int width, int height) { return new Buffer<int32_t>(f->realize(width, height)); }
extern "C" Buffer<float> *func_realize_float_2d(Func *f, int width, int height) { return new Buffer<float>(f->realize(width, height)); }
extern "C" Buffer<uint8_t> *func_realize_byte_2d(Func *f, int width, int height) { return new Buffer<uint8_t>(f->realize(width, height)); }

extern "C" Buffer<int32_t> *func_realize_int_3d(Func *f, int width, int height, int channels) { return new Buffer<int32_t>(f->realize(width, height, channels)); }
extern "C" Buffer<float> *func_realize_float_3d(Func *f, int width, int height, int channels) { return new Buffer<float>(f->realize(width, height, channels)); }
extern "C" Buffer<uint8_t> *func_realize_byte_3d(Func *f, int width, int height, int channels) { return new Buffer<uint8_t>(f->realize(width, height, channels)); }

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

extern "C" void func_print_loop_nest(Func* f) {
    f->print_loop_nest();
}

extern "C" void func_reorder(Func *f, int nrVariables, Var **vars) {
    std::vector<VarOrRVar> varList;
    for(int i = 0; i < nrVariables; i++) {
        varList.push_back(*vars[i]);
    }
    f->reorder(varList);
}

extern "C" void func_split(Func* f, Var* origVar, Var* v1, Var* v2, int factor) {
    f->split(*origVar, *v1, *v2, factor);
}

extern "C" void func_fuse(Func* f, Var* v1, Var* v2, Var* fused) {
    f->fuse(*v1, *v2, *fused);
}

extern "C" void func_vectorize(Func* f, Var* v) { f->vectorize(*v); }
extern "C" void func_vectorize_int(Func* f, Var* v, int factor) { f->vectorize(*v, factor); }

extern "C" void func_unroll(Func* f, Var* v) {
    f->unroll(*v);
}

extern "C" void func_tile(Func* func, Var* x, Var* y, Var* xo, Var* yo, Var* xi, Var* yi, int xfactor, int yfactor) {
    func->tile(*x, *y, *xo, *yo, *xi, *yi, xfactor, yfactor);
}

extern "C" void func_compute_root(Func *func) { func->compute_root(); }
extern "C" void func_compute_at(Func *self, Func *func, Var *v) { self->compute_at(*func, *v); }
extern "C" void func_store_root(Func *self) { self->store_root(); }
extern "C" void func_store_at(Func *self, Func *func, Var *v) { self->store_at(*func, *v); }
