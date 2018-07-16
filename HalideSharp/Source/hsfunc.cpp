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
    extern "C" Expr* Func_GetExpr_ ## T1(Func* f, T1* x) { return new Expr((*f)(*x)); } \
    extern "C" void Func_SetExpr_ ## T1 ## Expr(Func* f, T1* x, Expr* e) { (*f)(*x) = *e; }
PERMUTE_ARGS_1D(FUNC_INDEXER_1D)

// 2D indexers
#define FUNC_INDEXER_2D(T1, T2) \
    extern "C" Expr* Func_GetExpr_ ## T1 ## T2(Func* f, T1* x, T2* y) { return new Expr((*f)(*x, *y)); } \
    extern "C" void Func_SetExpr_ ## T1 ## T2 ## Expr(Func* f, T1* x, T2* y, Expr* e) { (*f)(*x, *y) = *e; }
PERMUTE_ARGS_2D(FUNC_INDEXER_2D)

// 3D indexers
#define FUNC_INDEXER_3D(T1, T2, T3) \
    extern "C" Expr* Func_GetExpr_ ## T1 ## T2 ##  T3(Func* f, T1* x, T2* y, T3 *z) { return new Expr((*f)(*x, *y, *z)); } \
    extern "C" void Func_SetExpr_ ## T1 ## T2 ## T3 ## Expr(Func* f, T1* x, T2* y, T3* z, Expr* e) { (*f)(*x, *y, *z) = *e; }
PERMUTE_ARGS_3D(FUNC_INDEXER_3D)

#define FUNC_REALIZE_TO_BUFF(CSTYPE, CPPTYPE) \
    extern "C" void Func_Realize_BufferOf ## CSTYPE(Func *f, Buffer<CPPTYPE> *buffer) { f->realize(*buffer); }
GEN(FUNC_REALIZE_TO_BUFF)

#define FUNC_REALIZE_NEWBUFF_2D(CSTYPE, CPPTYPE) \
    extern "C" Buffer<CPPTYPE> *Func_RealizeToBufferOf ## CSTYPE ## _IntInt(Func *f, int width, int height) { return new Buffer<CPPTYPE>(f->realize(width, height)); }
GEN(FUNC_REALIZE_NEWBUFF_2D)

#define FUNC_REALIZE_NEWBUFF_3D(CSTYPE, CPPTYPE) \
    extern "C" Buffer<CPPTYPE> *Func_RealizeToBufferOf ## CSTYPE ## _IntIntInt(Func *f, int width, int height, int channels) { return new Buffer<CPPTYPE>(f->realize(width, height, channels)); }
GEN(FUNC_REALIZE_NEWBUFF_3D)

extern "C" void Func_TraceStores(Func *f) { f->trace_stores(); }

extern "C" void Func_Parallel_Var(Func *f, Var *v) { f->parallel(*v); }

// TODO: accept a list of Argument objects.
extern "C" void Func_CompileToLoweredStmt_StringOutputformat(Func *f, const char *filename, enum HSOutputFormat format) {
    f->compile_to_lowered_stmt(filename, {}, (StmtOutputFormat) format);
}

extern "C" void Func_PrintLoopNest(Func* f) { f->print_loop_nest(); }

extern "C" void func_reorder(Func *f, int nrVariables, Var **vars) {
    std::vector<VarOrRVar> varList;
    for(int i = 0; i < nrVariables; i++) {
        varList.push_back(*vars[i]);
    }
    f->reorder(varList);
}

extern "C" void Func_Split_VarVarVarInt(Func* f, Var* origVar, Var* v1, Var* v2, int factor) { f->split(*origVar, *v1, *v2, factor); }

extern "C" void Func_Fuse_VarVarVar(Func* f, Var* v1, Var* v2, Var* fused) { f->fuse(*v1, *v2, *fused); }

extern "C" void Func_Vectorize_Var(Func* f, Var* v) { f->vectorize(*v); }
extern "C" void Func_Vectorize_VarInt(Func* f, Var* v, int factor) { f->vectorize(*v, factor); }
extern "C" void Func_Unroll_Var(Func* f, Var* v) { f->unroll(*v); }
extern "C" void Func_Tile_VarVarVarVarVarVarIntInt(Func* func, Var* x, Var* y, Var* xo, Var* yo, Var* xi, Var* yi, int xfactor, int yfactor) {
    func->tile(*x, *y, *xo, *yo, *xi, *yi, xfactor, yfactor);
}

extern "C" void Func_ComputeRoot(Func *func) { func->compute_root(); }
extern "C" void Func_ComputeAt_FuncVar(Func *self, Func *func, Var *v) { self->compute_at(*func, *v); }
extern "C" void Func_StoreRoot(Func *self) { self->store_root(); }
extern "C" void Func_StoreAt_FuncVar(Func *self, Func *func, Var *v) { self->store_at(*func, *v); }
