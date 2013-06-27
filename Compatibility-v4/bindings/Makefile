MONO_ANDROID_PATH := /Developer/MonoAndroid/usr
API_LEVELS        := 8
BUILDDIR          := bin
TOP               := ../..
thisdir           := Compatibility-v4/bindings

-include ../../env.config
-include $(TOP)/mk/helper.mk

MCW_GEN           := $(MONO_ANDROID_PATH)/bin/mcw-gen

platforms_dir := $(BUILDDIR)/platforms

ASSEMBLIES = $(API_LEVELS:%=$(platforms_dir)/android-%/Mono.Android.Support.v4.dll)
DOCS       = $(API_LEVELS:%=$(platforms_dir)/android-%/Mono.Android.Support.v4.xml)
JAR        = android-support-v4.jar

VPATH = $(ANDROID_SDK_PATH)/extras/android/support/v4/ $(ANDROID_SDK_PATH)/extras/android/compatibility/v4/

all:: $(JARS) $(ASSEMBLIES) $(DOCS)

clean::
	$(RM) -r $(ASSEMBLIES) obj

SOURCES = \
	$(topdir)/src/Android.Support.V4.Content/CursorLoader.cs \
	$(topdir)/src/Android.Support.V4.Content/Loader.cs \
	$(topdir)/src/Android.Support.V4.View/ViewPager.cs \
	src/AssemblyInfo.cs

$(ASSEMBLIES): $(platforms_dir)/android-%/Mono.Android.Support.v4.dll: $(JAR) EnumFields.xml EnumMethods.xml Metadata.xml $(SOURCES) $(MCW_GEN)
	-mkdir -p $(platforms_dir)/android-$*
	-mkdir -p obj/platform-$*
	$(MCW_GEN) \
		-sdk-dir="$(ANDROID_SDK_PATH)" \
		-sdk-platform=17 \
		-jar=$< \
		-embed \
		-dll="$(platforms_dir)/android-$*/Mono.Android.Support.v4.dll" \
		-fixup=Metadata.xml \
		-enumfields=EnumFields.xml -enummethods=EnumMethods.xml -global -v \
		-o obj/platform-$* \
		-cscopt -debug+ -cscopt -nowarn:0109,0108 $(SIGNOPTS:%=-cscopt %) \
		-droiddoc=$(ANDROID_SDK_PATH)/docs/reference \
		$(SOURCES:%=-cscopt "%")

$(DOCS): $(platforms_dir)/android-%/Mono.Android.Support.v4.xml: $(platforms_dir)/android-%/Mono.Android.Support.v4.dll EnumFields.xml Metadata.xml
	$(RM) -R obj/platform-$*/docs
	-mkdir -p obj/platform-$*/docs
	-cp -R Documentation/en obj/platform-$*/docs
	mdoc update -o obj/platform-$*/docs $< -L $(MONO_ANDROID_PATH)/lib/mandroid/platforms/android-$* >/dev/null
	mdoc export-msxdoc -o $@ obj/platform-$*/docs >/dev/null

$(MCW_GEN):
	@echo "Building bindings requires Mono for Android 4.2 or later."
	exit 1

$(JAR):
	@echo "Please install \"Extras > Android Support package\" within the \`android\` utility to build Support-v4."
	@exit 1

