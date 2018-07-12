#include <Halide.h>
#include <vector>
#include <string>

#define LANGUAGE_C 1
#include "SharedEnums.cs"
#undef LANGUAGE_C

using namespace Halide;

extern "C" Expr *cast_to_float(Expr *expr) {
    auto casted_expr = cast<float>(*expr);
    return new Expr(casted_expr);
}

extern "C" Expr *cast_to_byte(Expr *expr) {
    auto casted_expr = cast<uint8_t>(*expr);
    return new Expr(casted_expr);
}

extern "C" Expr *cast_to_ushort(Expr *expr) {
    auto casted_expr = cast<uint16_t>(*expr);
    return new Expr(casted_expr);
}

extern "C" Expr* min_expr_float(Expr* expr, float f) {
    return new Expr(min(*expr, f));
}

extern "C" Expr* print_objects_when(Expr* condition, int numObjects, enum HSObjectType *types, void **objects) {
    std::vector<Expr> args;
    for(int i = 0; i < numObjects; i++) {
        void *obj = objects[i];

        switch(types[i]) {
            case HS_String:
                args.push_back(Expr((const char *) obj));
                break;

            case HS_Var:
                args.push_back(*((Var*) obj));
                break;

            case HS_Expr:
                args.push_back(*((Expr*) obj));
                break;
        }
    }

    if(condition == nullptr) {
        return new Expr(print(args));
    } else {
        return new Expr(print_when(*condition, args));
    }
}

extern "C" Expr* clamp_expr_int_int(Expr* e, int min, int max) { return new Expr(clamp(*e, min, max)); }
extern "C" Expr* clamp_var_int_int(Var *v, int min, int max) { return new Expr(clamp(*v, min, max)); }
