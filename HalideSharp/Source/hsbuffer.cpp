#include <string>
#include <Halide.h>
#include <halide_image_io.h>

using namespace Halide;

extern "C" void delete_int_buffer(Buffer<int32_t> *b) { delete b; }
extern "C" void delete_byte_buffer(Buffer<uint8_t> *b) { delete b; }

extern "C" int buffer_int_width(Buffer<int32_t> *b) { return b->width(); }
extern "C" int buffer_byte_width(Buffer<uint8_t> *b) { return b->width(); }

extern "C" int buffer_int_height(Buffer<int32_t> *b) { return b->height(); }
extern "C" int buffer_byte_height(Buffer<uint8_t> *b) { return b->height(); }

extern "C" int buffer_int_channels(Buffer<int32_t> *b) { return b->channels(); }
extern "C" int buffer_byte_channels(Buffer<uint8_t> *b) { return b->channels(); }

extern "C" void buffer_int_getval_2d(Buffer<int32_t> *b, int x, int y, int32_t *result) { *result = (*b)(x, y); }
extern "C" void buffer_byte_getval_2d(Buffer<uint8_t> *b, int x, int y, uint8_t *result) { *result = (*b)(x, y); }

extern "C" void buffer_int_getval_3d(Buffer<int32_t> *b, int x, int y, int z, int32_t *result) { *result = (*b)(x, y, z); }
extern "C" void buffer_byte_getval_3d(Buffer<uint8_t> *b, int x, int y, int z, uint8_t *result) { *result = (*b)(x, y, z); }

extern "C" Expr* buffer_int_getexpr_2d_var_var(Buffer<int32_t> *b, Var *x, Var *y) { return new Expr((*b)(*x, *y)); }
extern "C" Expr* buffer_byte_getexpr_2d_var_var(Buffer<uint8_t> *b, Var *x, Var *y) { return new Expr((*b)(*x, *y)); }

extern "C" Expr* buffer_int_getexpr_3d_var_var_var(Buffer<int32_t> *b, Var* x, Var* y, Var* z) { return new Expr((*b)(*x, *y, *z)); }
extern "C" Expr* buffer_byte_getexpr_3d_var_var_var(Buffer<uint8_t> *b, Var* x, Var* y, Var* z) { return new Expr((*b)(*x, *y, *z)); }

extern "C" Buffer<uint8_t>* buffer_byte_load_image(const char *filename) {
    Buffer<uint8_t> buf = Tools::load_image(std::string(filename));
    return new Buffer<uint8_t>(buf);
}

extern "C" void buffer_byte_save_image(Buffer<uint8_t> *buf, const char *filename) {
    Tools::save_image(*buf, std::string(filename));
}
