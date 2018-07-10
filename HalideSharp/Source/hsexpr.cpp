#include <Halide.h>
#include <sstream>

using namespace Halide;

extern "C" Expr* new_expr_int(int i) {
    return new Expr(i);
}

extern "C" void delete_expr(Expr *e) {
    delete e;
}

extern "C" Expr* expr_mult_float(Expr* e, float f) {
    return new Expr((*e) * f);
}

extern "C" Expr* expr_plus_expr(Expr* e1, Expr* e2) {
    return new Expr((*e1) + (*e2));
}

extern "C" Expr* expr_lt_int(Expr* e, int i) {
    return new Expr((*e) < i);
}

extern "C" Expr* expr_gt_int(Expr* e, int i) {
    return new Expr((*e) > i);
}

extern "C" Expr* expr_plus_var(Expr* e, Var *v) {
    return new Expr((*e) + (*v));
}

extern "C" Expr* expr_plus_int(Expr* e, int i) {
    return new Expr((*e) + i);
}

extern "C" Expr* expr_and_expr(Expr* e1, Expr* e2) {
    return new Expr((*e1) && (*e2));
}

extern "C" char* expr_to_string(Expr *e) {
    std::stringstream ss;
    ss << (*e);
    std::string s = ss.str();
    
    // We use malloc rather than new char[] so that C# can just call free() on the pointer once we're done.
    char* result = (char*) malloc(s.length() + 1);
    strcpy(result, s.c_str());

    return result;
}
