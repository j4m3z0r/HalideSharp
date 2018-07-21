#include <string>
#include <Halide.h>
#include <halide_image_io.h>

#include "magicmacros.h"

using namespace Halide;

// 2D constructors
#define CONSTRUCTOR_2D(CSTYPE, CPPTYPE) \
    extern "C" Buffer<CPPTYPE> *BufferOf ## CSTYPE ## _New_IntInt(int width, int height) { return new Buffer<CPPTYPE>(width, height); }
GEN(CONSTRUCTOR_2D)

// 3D constructors
#define CONSTRUCTOR_3D(CSTYPE, CPPTYPE) \
    extern "C" Buffer<CPPTYPE> *BufferOf ## CSTYPE ## _New_IntIntInt(int width, int height, int channels) { return new Buffer<CPPTYPE>(width, height, channels); }
GEN(CONSTRUCTOR_3D)

// destructors
#define DESTRUCTOR(CSTYPE, CPPTYPE) \
    extern "C" void BufferOf ## CSTYPE ## _Delete(Buffer<CPPTYPE> *b) { delete b; }
GEN(DESTRUCTOR)

#define INT_ACCESSOR(CSTYPE, CPPTYPE, CSNAME, CPPNAME) \
    extern "C" int BufferOf ## CSTYPE ## _ ## CSNAME (Buffer< CPPTYPE > *b) { return b-> CPPNAME (); }

// width, height & channels accessors
GEN(INT_ACCESSOR, Width, width)
GEN(INT_ACCESSOR, Height, height)
GEN(INT_ACCESSOR, Channels, channels)
GEN(INT_ACCESSOR, Top, top)
GEN(INT_ACCESSOR, Bottom, bottom)
GEN(INT_ACCESSOR, Left, left)
GEN(INT_ACCESSOR, Right, right)

// Dimension accessors -- note that these are readonly for Buffers.
#define DIMENSION_FIELDS(CSTYPE, CPPTYPE, CSFIELD, CPPFIELD) \
    extern "C" int BufferOf ## CSTYPE ## _GetDimension ## CSFIELD ## _Int(Buffer<CPPTYPE> *b, int d) { return b->dim(d).CPPFIELD(); }
#define DIMENSION(CSTYPE, CPPTYPE) \
    DIMENSION_FIELDS(CSTYPE, CPPTYPE, Stride, stride) \
    DIMENSION_FIELDS(CSTYPE, CPPTYPE, Extent, extent)
GEN(DIMENSION)


// direct accessors -- to read the data out of the buffer, rather than use it as part of a
// pipeline.

// Don't really need to pass int in here, but it means this fits the pattern of the GEN macro.
// Similarly, it takes us more lines to do this with the macro, but it means we'll get the
// definitions for other buffer types for free, if/when we add them.
#define BUFFER_GETVAL_2D(CSTYPE, CPPTYPE, ARGTYPE) \
    extern "C" void BufferOf ## CSTYPE ## _GetVal_ ## ARGTYPE ## ARGTYPE(Buffer< CPPTYPE > *b, HST::ARGTYPE x, HST::ARGTYPE y, CPPTYPE *result) { *result = (*b)(x, y); }
GEN(BUFFER_GETVAL_2D, Int)

#define BUFFER_SETVAL_2D(CSTYPE, CPPTYPE, ARGTYPE) \
    extern "C" void BufferOf ## CSTYPE ## _SetVal_ ## ARGTYPE ## ARGTYPE ## CSTYPE ## P(Buffer< CPPTYPE > *b, HST::ARGTYPE x, HST::ARGTYPE y, CPPTYPE *valP) { (*b)(x, y) = *valP; }
GEN(BUFFER_SETVAL_2D, Int)

#define BUFFER_GETVAL_3D(CSTYPE, CPPTYPE, ARGTYPE) \
    extern "C" void BufferOf ## CSTYPE ## _GetVal_ ## ARGTYPE ## ARGTYPE ## ARGTYPE(Buffer< CPPTYPE > *b, HST::ARGTYPE x, HST::ARGTYPE y, HST::ARGTYPE z, CPPTYPE *result) { *result = (*b)(x, y, z); }
GEN(BUFFER_GETVAL_3D, Int)

#define BUFFER_SETVAL_3D(CSTYPE, CPPTYPE, ARGTYPE) \
    extern "C" void BufferOf ## CSTYPE ## _SetVal_ ## ARGTYPE ## ARGTYPE ## ARGTYPE ## CSTYPE ## P(Buffer< CPPTYPE > *b, HST::ARGTYPE x, HST::ARGTYPE y, HST::ARGTYPE z, CPPTYPE *valP) { (*b)(x, y, z) = *valP; }
GEN(BUFFER_SETVAL_3D, Int)

// 2D indexers
#define BUFFER_GETEXPR_2D(CSTYPE, CPPTYPE, T1, T2) \
    extern "C" Expr* BufferOf ## CSTYPE ## _GetExpr_ ## T1 ## T2 (Buffer< CPPTYPE > *b, T1 *x, T2 *y) { return new Expr((*b)(*x, *y)); }
#define BUFFER_GETEXPR_2D_ALLTYPES(T1, T2) GEN(BUFFER_GETEXPR_2D, T1, T2)
PERMUTE_ARGS_2D(BUFFER_GETEXPR_2D_ALLTYPES)

// 3D indexers
#define BUFFER_GETEXPR_3D(CSTYPE, CPPTYPE, T1, T2, T3) \
    extern "C" Expr* BufferOf ## CSTYPE ## _GetExpr_ ## T1 ## T2 ## T3(Buffer< CPPTYPE > *b, T1 *x, T2 *y, T3 *z) { return new Expr((*b)(*x, *y, *z)); }
#define BUFFER_GETEXPR_3D_ALLTYPES(T1, T2, T3) GEN(BUFFER_GETEXPR_3D, T1, T2, T3)
PERMUTE_ARGS_3D(BUFFER_GETEXPR_3D_ALLTYPES)

// Set min
#define BUFFER_SETMIN_2D(CSTYPE, CPPTYPE) \
    extern "C" void BufferOf ## CSTYPE ## _SetMin_IntInt(Buffer<CPPTYPE> *b, int x, int y) { b->set_min(x, y); }
GEN(BUFFER_SETMIN_2D)

extern "C" Buffer<uint8_t>* BufferOfByte_LoadImage_String(const char *filename) {
    Buffer<uint8_t> buf = Tools::load_image(std::string(filename));
    return new Buffer<uint8_t>(buf);
}

extern "C" void BufferOfByte_SaveImage_String(Buffer<uint8_t> *buf, const char *filename) {
    Tools::save_image(*buf, std::string(filename));
}

// Copy to host
#define BUFFER_COPY_TO_HOST(CSTYPE, CPPTYPE) \
    extern "C" void BufferOf ## CSTYPE ## _CopyToHost(Buffer<CPPTYPE> *self) { self->copy_to_host(); }
GEN(BUFFER_COPY_TO_HOST)
