#include <string>
#include <Halide.h>
#include <halide_image_io.h>

#include "magicmacros.h"

using namespace Halide;

// 2D constructors
extern "C" Buffer<int32_t>* new_int_buffer_int_int(int width, int height) { return new Buffer<int32_t>(width, height); }
extern "C" Buffer<float>* new_float_buffer_int_int(int width, int height) { return new Buffer<float>(width, height); }
extern "C" Buffer<uint8_t>* new_byte_buffer_int_int(int width, int height) { return new Buffer<uint8_t>(width, height); }

// 3D constructors
extern "C" Buffer<int32_t>* new_int_buffer_int_int_int(int width, int height, int channels) { return new Buffer<int32_t>(width, height, channels); }
extern "C" Buffer<float>* new_float_buffer_int_int_int(int width, int height, int channels) { return new Buffer<float>(width, height, channels); }
extern "C" Buffer<uint8_t>* new_byte_buffer_int_int_int(int width, int height, int channels) { return new Buffer<uint8_t>(width, height, channels); }

// destructors
extern "C" void delete_int_buffer(Buffer<int32_t> *b) { delete b; }
extern "C" void delete_float_buffer(Buffer<float> *b) { delete b; }
extern "C" void delete_byte_buffer(Buffer<uint8_t> *b) { delete b; }

// GEN will invoke the given macro once for each set of types we handle, passing the C# type
// as the first argument, the corresponding C++ type as the second argument, and then any
// further arguments that were specified. Currently it will generate variants for ints, floats,
// and bytes.
#define GEN(MACRO, args...) \
    MACRO(int, int32_t, args) \
    MACRO(float, float, args) \
    MACRO(byte, uint8_t, args)

#define INT_ACCESSOR(CSTYPE, CPPTYPE, NAME) \
    extern "C" int buffer_ ## CSTYPE ## _ ## NAME (Buffer< CPPTYPE > *b) { return b-> NAME (); }

// width, height & channels accessors
GEN(INT_ACCESSOR, width)
GEN(INT_ACCESSOR, height)
GEN(INT_ACCESSOR, channels)

// direct accessors -- to read the data out of the buffer, rather than use it as part of a
// pipeline.

// Don't really need to pass int in here, but it means this fits the pattern of the GEN macro.
// Similarly, it takes us more lines to do this with the macro, but it means we'll get the
// definitions for other buffer types for free, if/when we add them.
#define BUFFER_GETVAL_2D(CSTYPE, CPPTYPE, ARGTYPE) \
    extern "C" void buffer_ ## CSTYPE ## _getval__ ## ARGTYPE ## _ ## ARGTYPE(Buffer< CPPTYPE > *b, ARGTYPE x, ARGTYPE y, CPPTYPE *result) { *result = (*b)(x, y); }

#define BUFFER_GETVAL_3D(CSTYPE, CPPTYPE, ARGTYPE) \
    extern "C" void buffer_ ## CSTYPE ## _getval__ ## ARGTYPE ## _ ## ARGTYPE ## _ ## ARGTYPE(Buffer< CPPTYPE > *b, ARGTYPE x, ARGTYPE y, ARGTYPE z, CPPTYPE *result) { *result = (*b)(x, y, z); }

GEN(BUFFER_GETVAL_2D, int)
GEN(BUFFER_GETVAL_3D, int)

// 3D indexers
#define BUFFER_GETEXPR_3D(CSTYPE, CPPTYPE, T1, T2, T3) \
    extern "C" Expr* buffer_ ## CSTYPE ## _getexpr__ ## T1 ## _ ## T2 ## _ ## T3(Buffer< CPPTYPE > *b, T1 *x, T2 *y, T3 *z) { return new Expr((*b)(*x, *y, *z)); }

#define BUFFER_GETEXPR_3D_ALLTYPES(T1, T2, T3) GEN(BUFFER_GETEXPR_3D, T1, T2, T3)

PERMUTE_ARGS_3D(BUFFER_GETEXPR_3D_ALLTYPES)

// 2D indexers
#define BUFFER_GETEXPR_2D(CSTYPE, CPPTYPE, T1, T2) \
    extern "C" Expr* buffer_ ## CSTYPE ## _getexpr__ ## T1 ## _ ## T2 (Buffer< CPPTYPE > *b, T1 *x, T2 *y) { return new Expr((*b)(*x, *y)); }

#define BUFFER_GETEXPR_2D_ALLTYPES(T1, T2) GEN(BUFFER_GETEXPR_2D, T1, T2)

PERMUTE_ARGS_2D(BUFFER_GETEXPR_2D_ALLTYPES)

extern "C" void buffer_int_setmin(Buffer<int32_t> *b, int x, int y) { b->set_min(x, y); }
extern "C" void buffer_float_setmin(Buffer<float> *b, int x, int y) { b->set_min(x, y); }
extern "C" void buffer_byte_setmin(Buffer<uint8_t> *b, int x, int y) { b->set_min(x, y); }

extern "C" Buffer<uint8_t>* buffer_byte_load_image(const char *filename) {
    Buffer<uint8_t> buf = Tools::load_image(std::string(filename));
    return new Buffer<uint8_t>(buf);
}

extern "C" void buffer_byte_save_image(Buffer<uint8_t> *buf, const char *filename) {
    Tools::save_image(*buf, std::string(filename));
}
