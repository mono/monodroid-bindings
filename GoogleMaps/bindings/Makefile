MONO_ANDROID_PATH := /Developer/MonoAndroid/usr
ANDROID_SDK_PATH  := /opt/android/sdk
API_LEVELS        := 8
BUILDDIR          := bin
TOP               := ../..
thisdir           := GoogleMaps/bindings

-include ../../env.config
-include $(TOP)/mk/helper.mk

SMCS          := $(MONO_ANDROID_PATH)/bin/smcs
MCW_GEN       := $(MONO_ANDROID_PATH)/bin/mcw-gen

platforms_dir := $(BUILDDIR)/platforms

ASSEMBLIES = $(API_LEVELS:%=$(platforms_dir)/android-%/Mono.Android.GoogleMaps.dll)
DOCS       = $(API_LEVELS:%=$(platforms_dir)/android-%/Mono.Android.GoogleMaps.xml)

LEVELS = . . . 4 . . 7 8 . 10 . 12 13 14 15 16 17 18 19 20

all:: $(ASSEMBLIES) $(DOCS)

clean::
	$(RM) -r $(ASSEMBLIES) obj

SOURCES = \
	src/AssemblyInfo.cs \
	src/Android.GoogleMaps/ItemizedOverlay.cs \
	src/Android.GoogleMaps/MapController.cs \
	src/Android.GoogleMaps/MyLocationOverlay.cs \
	src/Android.GoogleMaps/TrackballGestureDetector.cs

define get-levels-to-merge
	$(filter-out .,$(wordlist 1, $(1), $(LEVELS)))
endef

define get-descriptions-to-merge
	$(patsubst %,obj/android-%/api.xml,$(call get-levels-to-merge,$(1)))
endef

define get-descriptions.in-to-merge
	$(patsubst %,obj/android-%/api.xml.in,$(call get-levels-to-merge,$(1)))
endef

define get-maps.jar-path
$(strip $(shell find $(ANDROID_SDK_PATH)/ -path \*$(1)/libs/maps.jar | grep '\<$(1)\>'))
endef

API_DESCRIPTIONS_IN := $(call get-descriptions.in-to-merge,$(lastword $(API_LEVELS)))
API_DESCRIPTIONS    := $(call get-descriptions-to-merge,$(lastword $(API_LEVELS)))

$(API_DESCRIPTIONS_IN): obj/android-%/api.xml.in: $(MCW_GEN)
	-mkdir -p obj/android-$*
	@jar=$(call get-maps.jar-path,$*); \
	if [ ! -f "$$jar" ]; then \
		echo "Please install the \"Android (API $(API_LEVELS)) > Google APIs by Google Inc.\" package within the \`android\` utility to build GoogleMaps." ; \
	fi ; \
	$(MCW_GEN) describe \
		-sdk-dir="$(ANDROID_SDK_PATH)" \
		-sdk-platform=$* \
		-javadoc="$$(ls -d `dirname "$$jar"`/../docs/maps_apis `dirname "$$jar"`/../docs/reference 2>/dev/null)" \
		-o "$@" \
		$$jar

$(API_DESCRIPTIONS): obj/android-%/api.xml: $(call get-descriptions.in-to-merge,$(lastword $(API_LEVELS))) $(MCW_GEN)
	$(MCW_GEN) merge -v -o "$@" $(call get-descriptions.in-to-merge,$*)

$(ASSEMBLIES): $(platforms_dir)/android-%/Mono.Android.GoogleMaps.dll: fieldmap.xml methodmap.xml Maps.fixup $(SOURCES) obj/android-%/api.xml $(MCW_GEN)
	-mkdir -p $(platforms_dir)/android-$*
	$(MCW_GEN) bind \
		--sdk-dir="$(ANDROID_SDK_PATH)" \
		--sdk-platform=$* \
		--assembly="$(notdir $@)" \
		--enumfields=fieldmap.xml \
		--enummethods=methodmap.xml \
		-fixup=Maps.fixup \
		--global -v \
		-i obj/android-$*/api.xml \
		-o obj/android-$*
	$(SMCS) -target:library -out:$@ -debug+ obj/android-$*/src/generated/*.cs $(SOURCES) \
		-r:$(MONO_ANDROID_PATH)/lib/mandroid/platforms/android-$*/Mono.Android.dll $(SIGNOPTS)

$(DOCS): $(platforms_dir)/android-%/Mono.Android.GoogleMaps.xml: $(platforms_dir)/android-%/Mono.Android.GoogleMaps.dll fieldmap.xml Maps.fixup
	$(RM) -R obj/android-$*/docs
	-mkdir -p obj/android-$*/docs
	-cp -R Documentation/en obj/android-$*/docs
	mdoc update -o obj/android-$*/docs $< -L $(MONO_ANDROID_PATH)/lib/mandroid/platforms/android-$*
	mdoc export-msxdoc -o $@ obj/android-$*/docs

$(MCW_GEN):
	@echo "Building bindings requires Mono for Android 4.2 or later."
	exit 1

