#include <Halide.h>
#include <stdio.h>

extern "C" void delete_int_buffer(Halide::Buffer<int32_t> *b) {
    delete b;
}

extern "C" int buffer_int_width(Halide::Buffer<int32_t> *b) {
    return b->width();
}

extern "C" int buffer_int_height(Halide::Buffer<int32_t> *b) {
    return b->height();
}

extern "C" void buffer_int_getval(Halide::Buffer<int32_t> *b, int x, int y, int *result) {
    *result = (*b)(x, y);
}