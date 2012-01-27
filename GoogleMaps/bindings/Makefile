MONO_ANDROID_PATH := /Developer/MonoAndroid/usr
ANDROID_SDK_PATH  := /opt/android/sdk
API_LEVELS        := 8
BUILDDIR          := bin

MCW_GEN           := $(MONO_ANDROID_PATH)/bin/mcw-gen

platforms_dir := $(BUILDDIR)/platforms

ASSEMBLIES = $(API_LEVELS:%=$(platforms_dir)/android-%/Mono.Android.GoogleMaps.dll)
DOCS       = $(API_LEVELS:%=$(platforms_dir)/android-%/Mono.Android.GoogleMaps.xml)
JARS       = $(API_LEVELS:%=$(ANDROID_SDK_PATH)/add-ons/addon-google_apis-google_inc_-%/libs/maps.jar)

all:: $(JARS) $(ASSEMBLIES) $(DOCS)

clean::
	$(RM) -r $(ASSEMBLIES) obj

SOURCES = \
	src/AssemblyInfo.cs \
	src/Android.GoogleMaps/MapController.cs \
	src/Android.GoogleMaps/MyLocationOverlay.cs \
	src/Android.GoogleMaps/TrackballGestureDetector.cs

$(ASSEMBLIES): $(platforms_dir)/android-%/Mono.Android.GoogleMaps.dll: fieldmap.xml methodmap.xml Maps.fixup $(SOURCES) $(MCW_GEN)
	-mkdir -p $(platforms_dir)/android-$*
	-mkdir -p obj/platform-$*
	if [ 15 -eq "$*" ]; then \
		DOCS="$(ANDROID_SDK_PATH)/add-ons/addon-google_apis-google_inc_-$*/docs/maps_apis"; \
	else \
		DOCS="$(ANDROID_SDK_PATH)/add-ons/addon-google_apis-google_inc_-$*/docs/reference"; \
	fi; \
	$(MCW_GEN) \
		-sdk-dir="$(ANDROID_SDK_PATH)" \
		-sdk-platform=$* \
		-jar=$(ANDROID_SDK_PATH)/add-ons/addon-google_apis-google_inc_-$*/libs/maps.jar \
		-dll="$(platforms_dir)/android-$*/Mono.Android.GoogleMaps.dll" \
		-fixup=Maps.fixup \
		-enumfields=fieldmap.xml -enummethods=methodmap.xml -global -v \
		-o obj/platform-$* \
		-javadoc="$$DOCS" \
		-cscopt -debug+ $(SOURCES:%=-cscopt "%") ; \

$(DOCS): $(platforms_dir)/android-%/Mono.Android.GoogleMaps.xml: $(platforms_dir)/android-%/Mono.Android.GoogleMaps.dll fieldmap.xml Maps.fixup
	$(RM) -R obj/platform-$*/docs
	-mkdir -p obj/platform-$*/docs
	-cp -R Documentation/en obj/platform-$*/docs
	mdoc update -o obj/platform-$*/docs $< -L $(MONO_ANDROID_PATH)/lib/mandroid/platforms/android-$*
	mdoc export-msxdoc -o $@ obj/platform-$*/docs

$(MCW_GEN):
	@echo "Building bindings requires Mono for Android 4.2 or later."
	exit 1

$(JARS):
	@echo "Please install the \"Android (API $(API_LEVELS)) > Google APIs by Google Inc.\" package within the \`android\` utility to build GoogleMaps."
	@exit 1
