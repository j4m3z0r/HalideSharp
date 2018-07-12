#include <Halide.h>

#define LANGUAGE_C 1
#include "SharedEnums.cs"
#undef LANGUAGE_C

using namespace Halide;

extern "C" Func *new_func(const char *name) { return new Func(name); }
extern "C" void delete_func(Func *f) { delete f; }

// 2D indexers
// var var
extern "C" void func_set_var_var__expr(Func *f, Var *v1, Var *v2, Expr *e) { (*f)(*v1, *v2) = *e; }
extern "C" Expr* func_getexpr__var_var(Func*f, Var *v1, Var *v2) { return new Expr((*f)(*v1, *v2)); }

// var expr
extern "C" void func_set_var_expr__expr(Func *f, Var *v1, Expr *v2, Expr *e) { (*f)(*v1, *v2) = *e; }
extern "C" Expr* func_getexpr__var_expr(Func*f, Var *v1, Expr *v2) { return new Expr((*f)(*v1, *v2)); }

// expr var
extern "C" void func_set_expr_var__expr(Func *f, Expr *v1, Var *v2, Expr *e) { (*f)(*v1, *v2) = *e; }
extern "C" Expr* func_getexpr__expr_var(Func*f, Expr *v1, Var *v2) { return new Expr((*f)(*v1, *v2)); }

// expr var
extern "C" void func_set_expr_expr__expr(Func *f, Expr *v1, Expr *v2, Expr *e) { (*f)(*v1, *v2) = *e; }
extern "C" Expr* func_getexpr__expr_expr(Func*f, Expr *v1, Expr *v2) { return new Expr((*f)(*v1, *v2)); }
// END 2D indexers



// 3D indexers
// var var var
extern "C" void func_set_var_var_var__expr(Func *f, Var *v1, Var *v2, Var *v3, Expr *e) { (*f)(*v1, *v2, *v3) = *e; }
extern "C" Expr* func_getexpr__var_var_var(Func*f, Var *v1, Var *v2, Var *v3) { return new Expr((*f)(*v1, *v2, *v3)); }

// expr var var
extern "C" void func_set_expr_var_var__expr(Func *f, Expr *v1, Var *v2, Var *v3, Expr *e) { (*f)(*v1, *v2, *v3) = *e; }
extern "C" Expr* func_getexpr__expr_var_var(Func*f, Expr *v1, Var *v2, Var *v3) { return new Expr((*f)(*v1, *v2, *v3)); }

// var expr var
extern "C" void func_set_var_expr_var__expr(Func *f, Var *v1, Expr *v2, Var *v3, Expr *e) { (*f)(*v1, *v2, *v3) = *e; }
extern "C" Expr* func_getexpr__var_expr_var(Func*f, Var *v1, Expr *v2, Var *v3) { return new Expr((*f)(*v1, *v2, *v3)); }

// var var expr
extern "C" void func_set_var_var_expr__expr(Func *f, Var *v1, Var *v2, Expr *v3, Expr *e) { (*f)(*v1, *v2, *v3) = *e; }
extern "C" Expr* func_getexpr__var_var_expr(Func*f, Var *v1, Var *v2, Expr *v3) { return new Expr((*f)(*v1, *v2, *v3)); }

// expr expr var
extern "C" void func_set_expr_expr_var__expr(Func *f, Expr *v1, Expr *v2, Var *v3, Expr *e) { (*f)(*v1, *v2, *v3) = *e; }
extern "C" Expr* func_getexpr__expr_expr_var(Func*f, Expr *v1, Expr *v2, Var *v3) { return new Expr((*f)(*v1, *v2, *v3)); }

// var expr expr
extern "C" void func_set_var_expr_expr__expr(Func *f, Var *v1, Expr *v2, Expr *v3, Expr *e) { (*f)(*v1, *v2, *v3) = *e; }
extern "C" Expr* func_getexpr__var_expr_expr(Func*f, Var *v1, Expr *v2, Expr *v3) { return new Expr((*f)(*v1, *v2, *v3)); }

// expr var expr
extern "C" void func_set_expr_var_expr__expr(Func *f, Expr *v1, Var *v2, Expr *v3, Expr *e) { (*f)(*v1, *v2, *v3) = *e; }
extern "C" Expr* func_getexpr__expr_var_expr(Func*f, Expr *v1, Var *v2, Expr *v3) { return new Expr((*f)(*v1, *v2, *v3)); }

// expr expr expr
extern "C" void func_set_expr_expr_expr__expr(Func *f, Expr *v1, Expr *v2, Expr *v3, Expr *e) { (*f)(*v1, *v2, *v3) = *e; }
extern "C" Expr* func_getexpr__expr_expr_expr(Func*f, Expr *v1, Expr *v2, Expr *v3) { return new Expr((*f)(*v1, *v2, *v3)); }
// END 3D indexers



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
