#include <Halide.h>

using namespace Halide;

extern "C" typedef void (*HalideSharpErrorDelegate)(const char*);

static HalideSharpErrorDelegate _errorDelegate = nullptr;

class HalideSharpErrorReporter : public CompileTimeErrorReporter
{
public:
    // The strings here look to come from std:string's c_str() method, so it should get
    // cleaned up automatically.
    void warning(const char* msg) {
        if(_errorDelegate != nullptr) {
            _errorDelegate(msg);
        }
    }
    void error(const char *msg) {
        if(_errorDelegate != nullptr) {
            _errorDelegate(msg);
        }
    }
};

static HalideSharpErrorReporter _halideErrorReporter;

extern "C" void Global_SetErrorHandler_ErrDelegate(HalideSharpErrorDelegate errDelegate) {
    _errorDelegate = errDelegate;
    set_custom_compile_time_error_reporter(&_halideErrorReporter);
}

