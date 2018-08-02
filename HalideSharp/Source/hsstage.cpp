#include <Halide.h>

using namespace Halide;

extern "C" void Stage_Delete(Stage *self) { delete self; }

