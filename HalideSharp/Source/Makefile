TARGET := libhalide-wrapper.so

SOURCES := $(wildcard hs*.cpp)
OBJECTS := $(SOURCES:.cpp=.o)

ECS_SOURCES := $(wildcard HS*.ecs)
ECS_OUTPUTS := $(ECS_SOURCES:.ecs=.out.cs)

LDFLAGS := ...

all: $(TARGET) $(ECS_OUTPUTS)

THIRDPARTY_DIR := $(abspath ../3rdparty)
HALIDE_VERSION := linux-64-gcc53-trunk-6e673cc8a8efe21396ddc66214e9cf645650a3fd
HALIDE_DIR := $(abspath $(THIRDPARTY_DIR)/halide)
HALIDE_LIB := $(HALIDE_DIR)/lib/libHalide.a
HALIDE_INC := $(HALIDE_DIR)/include
HALIDE_TOOLS := $(HALIDE_DIR)/tools

LIBJPEG_TURBO_VERSION := 1.5.3
LIBJPEG_TURBO_DIR := $(abspath $(THIRDPARTY_DIR)/libjpeg-turbo-$(LIBJPEG_TURBO_VERSION))
LIBJPEG_TURBO_LIB := $(LIBJPEG_TURBO_DIR)/build/lib/libturbojpeg.a
LIBJPEG_TURBO_INC := $(LIBJPEG_TURBO_DIR)/build/include

LIBPNG_VERSION := 1.6.35
LIBPNG_DIR := $(abspath $(THIRDPARTY_DIR)/libpng-$(LIBPNG_VERSION))
LIBPNG_LIB := $(LIBPNG_DIR)/build/lib/libpng.a
LIBPNG_INC := $(LIBPNG_DIR)/build/include

LIBZ_VERSION := 1.2.11
LIBZ_DIR := $(abspath $(THIRDPARTY_DIR)/zlib-$(LIBZ_VERSION))
LIBZ_LIB := $(LIBZ_DIR)/build/lib/libz.a
LIBZ_INCLUDE := $(LIBZ_DIR)/build/include

LEMP_VERSION := -trunk
LEMP_DIR := $(abspath $(THIRDPARTY_DIR)/LeMP)
LEMP_BIN := $(LEMP_DIR)/LeMP.exe

CXXFLAGS := -O2 -g -I$(HALIDE_INC) -I$(HALIDE_TOOLS) -I$(LIBPNG_INC) -I$(LIBJPEG_TURBO_INC) -fPIC
LDFLAGS := $(HALIDE_LIB) $(LIBPNG_LIB) $(LIBJPEG_TURBO_LIB) -lpthread -ldl -fPIC

CXX := g++ -std=c++11
MONO := /usr/bin/mono

### libz ###
$(LIBZ_LIB): $(THIRDPARTY_DIR)/zlib-$(LIBZ_VERSION).tar.gz
	(rm -rf $(LIBZ_DIR))
	(cd $(THIRDPARTY_DIR); tar xzvf zlib-$(LIBZ_VERSION).tar.gz)
	(cd $(LIBZ_DIR); \
        CFLAGS="-fPIC $(DBGFLAGS)" \
        ./configure \
            --static \
            --prefix=`pwd`/build && \
        $(MAKE) && \
        $(MAKE) install)
### end libz ###

### libpng ###
$(LIBPNG_LIB): $(THIRDPARTY_DIR)/libpng-$(LIBPNG_VERSION).tar.gz $(LIBZ_LIB)
	(rm -rf $(LIBPNG_DIR))
	(cd $(THIRDPARTY_DIR); tar xzvf libpng-$(LIBPNG_VERSION).tar.gz)
	(cd $(LIBPNG_DIR); \
        CFLAGS="-fPIC $(DBGFLAGS)" \
       	CPPFLAGS="-I$(LIBZ_DIR)/build/include" \
	LDFLAGS="-L$(LIBZ_DIR)/build/lib" \
        ./configure \
            --with-zlib-prefix=$(LIBZ_DIR)/build \
            --enable-static \
            --enable-hardware-optimizations \
            --enable-intel-sse \
            --prefix=`pwd`/build && \
        $(MAKE) && \
        $(MAKE) install)
### end libpng ###

### libjpeg-turbo ###
$(LIBJPEG_TURBO_LIB): $(THIRDPARTY_DIR)/libjpeg-turbo-$(LIBJPEG_TURBO_VERSION).tar.gz
	(rm -rf $(LIBJPEG_TURBO_DIR))
	(cd $(THIRDPARTY_DIR); tar xzvf libjpeg-turbo-$(LIBJPEG_TURBO_VERSION).tar.gz)
	# Note: we redirect stderr on make to /dev/null since msbuild detects that
	# output and thinks an error has occurred. This should still abort if there
	# is an actual error, since make's exit code will be nonzero.
	(cd $(LIBJPEG_TURBO_DIR); \
        CFLAGS="-fPIC $(DBGFLAGS)" \
        ./configure \
            --enable-static \
            --with-jpeg8 \
            --prefix=`pwd`/build && \
        $(MAKE) 2> /dev/null && \
        $(MAKE) install)
### end libjpeg-turbo ###

$(HALIDE_LIB): $(THIRDPARTY_DIR)/halide-$(HALIDE_VERSION).tgz
	(rm -rf $(HALIDE_DIR))
	(cd $(THIRDPARTY_DIR); tar xzvf $<)
	touch $(HALIDE_LIB)

$(LEMP_BIN): $(THIRDPARTY_DIR)/LeMP$(LEMP_VERSION).zip
	(rm -rf $(LEMP_DIR))
	(cd $(THIRDPARTY_DIR); unzip $<)
	touch $(LEMP_BIN)

THIRDPARTY_LIBS := $(HALIDE_LIB) $(LIBPNG_LIB) $(LIBJPEG_TURBO_LIB)

$(TARGET): $(OBJECTS) $(THIRDPARTY_LIBS)
	$(CXX) -o $@ $^ -shared -Wl,--exclude-libs,ALL $(LDFLAGS)

%.o: %.cpp $(THIRDPARTY_LIBS)
	$(CXX) -c $(CXXFLAGS) -o $@ $<

%.out.cs: %.ecs $(LEMP_BIN)
	$(MONO) $(LEMP_BIN) --preserve-comments --outext=out.cs --inlang=ecs --forcelang $<

clean:
	rm -f $(TARGET) $(OBJECTS) $(ECS_OUTPUTS)
	rm -rf $(HALIDE_DIR) $(LEMP_DIR) $(LIBPNG_DIR) $(LIBJPEG_TURBO_DIR) $(LIBZ_DIR)


# There isn't a neat dependency generator for LeMP at present so we just
# enumerate dependencies here by hand. This isn't critical (a clean build will
# work just fine without this), but is convenient when developing so that
# changes to included files will trigger rebuilding the appropriate .out.cs
# files
Indexer.ecs: MagicMacros.ecs
HS.out.cs: MagicMacros.ecs
HSFunc.out.cs: Indexer.ecs
HSImageParam.out.cs: Indexer.ecs
HSParam.out.cs: MagicMacros.ecs

