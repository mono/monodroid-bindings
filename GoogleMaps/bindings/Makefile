MONO_ANDROID_PATH := /Developer/MonoAndroid/usr
ANDROID_SDK_PATH  := /opt/android/sdk
API_LEVELS        := 8
BUILDDIR          := bin

RUNTIME           := mono
MCW_GEN           := $(MONO_ANDROID_PATH)/lib/mandroid/mcw-gen.exe

platforms_dir := $(BUILDDIR)/platforms

ASSEMBLIES = $(API_LEVELS:%=$(platforms_dir)/android-%/Mono.Android.GoogleMaps.dll)
DOCS       = $(API_LEVELS:%=$(platforms_dir)/android-%/Mono.Android.GoogleMaps.xml)

all:: $(ASSEMBLIES) $(DOCS)

clean::
	$(RM) -r $(ASSEMBLIES) obj

SOURCES = \
	src/AssemblyInfo.cs \
	src/Android.GoogleMaps/MapController.cs \
	src/Android.GoogleMaps/MyLocationOverlay.cs \
	src/Android.GoogleMaps/TrackballGestureDetector.cs

$(ASSEMBLIES): $(platforms_dir)/android-%/Mono.Android.GoogleMaps.dll: map.csv methodmap.csv Maps.fixup $(MCW_GEN)
	-mkdir -p $(platforms_dir)/android-$*
	-mkdir -p obj/platform-$*
	$(RUNTIME) $(MCW_GEN) \
		-sdk=$(ANDROID_SDK_PATH) \
		-jar=$(ANDROID_SDK_PATH)/add-ons/addon-google_apis-google_inc_-$*/libs/maps.jar \
		-dll="$(platforms_dir)/android-$*/Mono.Android.GoogleMaps.dll" \
		-fixup=Maps.fixup \
		-api-level=$* -outdir=obj/platform-$* \
		-javadoc=$(ANDROID_SDK_PATH)/add-ons/addon-google_apis-google_inc_-$*/docs/$(if $(findstr 15,$*),maps_apis,reference) \
		-fixup=metadata \
		-enumfields=map.csv -enummethods=methodmap.csv -global -v \
		-cscopt="-debug+ $(SOURCES)"

$(DOCS): $(platforms_dir)/android-%/Mono.Android.GoogleMaps.xml: $(platforms_dir)/android-%/Mono.Android.GoogleMaps.dll map.csv Maps.fixup
	$(RM) -R obj/platform-$*/docs
	-mkdir -p obj/platform-$*/docs
	-cp -R Documentation/en obj/platform-$*/docs
	mdoc update -o obj/platform-$*/docs $< -L $(MONO_ANDROID_PATH)/lib/mandroid/platforms/android-$*
	mdoc export-msxdoc -o $@ obj/platform-$*/docs

