#include <Halide.h>

// NOTE: this file uses a *lot* of preprocessor magic that isn't particularly
// intuitive. Please read magicmacros.h before making changes to this file to
// get a sense for how it all works.

#include "magicmacros.h"

#define LANGUAGE_C 1
#include "SharedEnums.cs"
#undef LANGUAGE_C

using namespace Halide;

extern "C" Func *Func_New_String(const char *name) { return new Func(name); }
extern "C" void Func_Delete(Func *f) { delete f; }

// 1D indexers
#define FUNC_INDEXER_1D(T1) \
    extern "C" Expr* Func_GetExpr_ ## T1( \
        Func* f, \
        argtype(T1) x \
    ) { \
        return new Expr((*f)(deref(T1, x))); \
    } \
    \
    extern "C" void Func_SetExpr_ ## T1 ## Expr( \
        Func* f, \
        argtype(T1) x, \
        Expr* e \
    ) { \
        (*f)(deref(T1, x)) = *e; \
    }

// 2D indexers
#define FUNC_INDEXER_2D(T1, T2) \
    extern "C" Expr* Func_GetExpr_ ## T1 ## T2( \
        Func* f, \
        argtype(T1) x, \
        argtype(T2) y \
    ) { \
        return new Expr((*f)(deref(T1, x), deref(T2, y))); \
    } \
    \
    extern "C" void Func_SetExpr_ ## T1 ## T2 ## Expr( \
        Func* f, \
        argtype(T1) x, \
        argtype(T2) y, \
        Expr* e \
    ) { \
        (*f)(deref(T1, x), deref(T2, y)) = *e; \
    }


// 3D indexers
#define FUNC_INDEXER_3D(T1, T2, T3) \
    extern "C" Expr* Func_GetExpr_ ## T1 ## T2 ## T3( \
        Func* f, \
        argtype(T1) x, \
        argtype(T2) y, \
        argtype(T3) z \
    ) { \
        return new Expr((*f)(deref(T1, x), deref(T2, y), deref(T3, z))); \
    } \
    \
    extern "C" void Func_SetExpr_ ## T1 ## T2 ## T3 ## Expr( \
        Func* f, \
        argtype(T1) x, \
        argtype(T2) y, \
        argtype(T3) z, \
        Expr* e \
    ) { \
        (*f)(deref(T1, x), deref(T2, y), deref(T3, z)) = *e; \
    }

PERMUTE_ARGS_1D(FUNC_INDEXER_1D)
PERMUTE_ARGS_2D(FUNC_INDEXER_2D)
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

extern "C" void Func_Reorder_Varargs(Func *f, int nrVariables, Var **vars) {
    std::vector<VarOrRVar> varList;
    for(int i = 0; i < nrVariables; i++) {
        varList.push_back(*vars[i]);
    }
    f->reorder(varList);
}

extern "C" void Func_ReorderStorage_Varargs(Func *f, int nrVariables, Var **vars) {
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
extern "C" void Func_CompileJit(Func *self) { self->compile_jit(); }
extern "C" void Func_CompileJit_Target(Func *self, Target *t) { self->compile_jit(*t); }
extern "C" void Func_Bound(Func *self, Var *v, int min, int max) { self->bound(*v, min, max); }

// gpu_tile variants
extern "C" void Func_GpuTile_VarVarVarInt(Func* self, Var* x, Var* bx, Var* tx, int x_size) {
    self->gpu_tile(*x, *bx, *tx, x_size);
}
extern "C" void Func_GpuTile_VarVarInt(Func* self, Var* x, Var* tx, int x_size) {
    self->gpu_tile(*x, *tx, x_size);
}
extern "C" void Func_GpuTile_VarVarVarVarVarVarIntInt(Func* self, Var* x, Var* y, Var* bx, Var* by, Var* tx, Var* ty, int x_size, int y_size) {
    self->gpu_tile(*x, *y, *bx, *by, *tx, *ty, x_size, y_size);
}
extern "C" void Func_GpuTile_VarVarVarVarIntInt(Func* self, Var* x, Var* y, Var* tx, Var* ty, int x_size, int y_size) {
    self->gpu_tile(*x, *y, *tx, *ty, x_size, y_size);
}
extern "C" void Func_GpuTile_VarVarVarVarVarVarVarVarVarIntIntInt(Func* self, Var* x, Var* y, Var* z, Var* bx, Var* by, Var* bz, Var* tx, Var* ty, Var* tz, int x_size, int y_size, int z_size) {
    self->gpu_tile(*x, *y, *z, *bx, *by, *bz, *tx, *ty, *tz, x_size, y_size, z_size);
}
extern "C" void Func_GpuTile_VarVarVarVarVarVarIntIntInt(Func* self, Var* x, Var* y, Var* z, Var* tx, Var* ty, Var* tz, int x_size, int y_size, int z_size) {
    self->gpu_tile(*x, *y, *z, *tx, *ty, *tz, x_size, y_size, z_size);
}
// The docs include these methods, but I can't find it in the header file.
/*
extern "C" void Func_GpuTile_VarInt(Func* self, Var* x, int x_size) {
    self->gpu_tile(*x, x_size);
}
extern "C" void Func_GpuTile_VarVarIntInt(Func* self, Var* x, Var* y, int x_size, int y_size) {
    self->gpu_tile(*x, *y, x_size, y_size);
}
extern "C" void Func_GpuTile_VarVarVarIntIntInt(Func* self, Var* x, Var* y, Var* z, int x_size, int y_size, int z_size) {
    self->gpu_tile(*x, *y, *z, x_size, y_size, z_size);
}
*/

// gpu_blocks
extern "C" void Func_GpuBlocks_Var(Func* self, Var *x) { self->gpu_blocks(*x); }
extern "C" void Func_GpuBlocks_VarVar(Func* self, Var *x, Var *y) { self->gpu_blocks(*x, *y); }
extern "C" void Func_GpuBlocks_VarVarVar(Func* self, Var *x, Var *y, Var *z) { self->gpu_blocks(*x, *y, *z); }

// gpu_threads
extern "C" void Func_GpuThreads_Var(Func* self, Var *x) { self->gpu_threads(*x); }
extern "C" void Func_GpuThreads_VarVar(Func* self, Var *x, Var *y) { self->gpu_threads(*x, *y); }
extern "C" void Func_GpuThreads_VarVarVar(Func* self, Var *x, Var *y, Var *z) { self->gpu_threads(*x, *y, *z); }

extern "C" Func* Func_In_Func(Func *self, Func *f) { return new Func(self->in(*f)); }
extern "C" Func* Func_In(Func *self) { return new Func(self->in()); }

