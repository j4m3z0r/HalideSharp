#include <Halide.h>
#include <sstream>

using namespace Halide;

extern "C" Expr* Expr_New_Int(int i) {
    return new Expr(i);
}

extern "C" void Expr_Delete(Expr *e) {
    delete e;
}

extern "C" char* Expr_ToString(Expr *e) {
    std::stringstream ss;
    ss << (*e);
    std::string s = ss.str();
    
    // We use malloc rather than new char[] so that C# can just call free() on the pointer once we're done.
    char* result = (char*) malloc(s.length() + 1);
    strcpy(result, s.c_str());

    return result;
}
