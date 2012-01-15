Google Maps API binding
=======================

This is a binding generator sample for google maps API (maps.jar).


Building
========

To build this library, you have to first get Google maps API addon in
the Android SDK. Once you get it, set the `ANDROID_SDK_PATH` environment
variable to the installed SDK, and run `make` from within the bindings
directory.

    make ANDROID_SDK_PATH=/opt/android/sdk

Requirements
============

Building the bindings requires:

* Mono for Android 4.2 or later
* The Google APIs Android SDK add-on

Using the bindings requires:

* A device which supports Google Maps
* A Google Maps API key.


Installing the Google APIs Android SDK add-on
---------------------------------------------

The Google APIs require the "Google APIs" add-on library, which is provided
for each API level. You need to download it for the API level you wish to
target.

To install the Google APIs Android SDK add-on:

 1. Launch the Android SDK manager:
        `$ANDROID_SDK_PATH/tools/android`
 2. Within the Android SDK manager, click Available packages
    in the left-hand pane.
 3. In the right-hand pane, navigate to the tree view node Third party
    Add-ons /Google Inc. add-ons (dl-ssl.google.com)
 4. Select the check-box for e.g. "Google APIs by Google Inc., Android API
    8, revision 2".
 5. Click the Install Selected button in the lower right corner.
 6. In the "Choose Packages to Install" dialog, select the Accept
    radio button, then click the Install button.


Creating a device with Google Maps support
------------------------------------------

To verify that your target device has Google Maps support, you can use
the following command to see if Google Maps support is present:

    $ $ANDROID_SDK_PATH/platform-tools/adb shell ls /system/framework/*map*
    /system/framework/com.google.android.maps.jar
    /system/framework/com.google.android.maps.odex

The `com.google.android.maps.jar` library should be present on
hardware devices, but may not be present within some emulators.

If you need an emulator with Google Maps support:

 1. Launch the Android AVD manager:
        `$ANDROID_SDK_PATH/tools/android avd`
 2. Within the Android SDK and AVD manager, click Virtual devices
    in the left-hand pane.
 3. Click the New... button on the right-hande side.
 4. In the Create new Android Virtual Device (AVD) dialog, provide a
    name for the device (e.g. MAPS), and in the Target drop-down
    select the Google APIs (Google Inc.) - API Level 8 entry.
 5. Click the Create AVD button.

You may now launch the emulator with:

    `$ANDROID_SDK_PATH/tools/emulator -partition-size 512 -avd MAPS`


Google Maps API Key
-------------------

Google Maps requires a per-app API key.  You can obtain a maps API key from here:
http://docs.xamarin.com/android/advanced_topics/Obtaining_a_Google_Maps_API_Key

Once you obtained the key, you have to alter two parts of the sample sources to fully
run the demo app:

  * in Resource/Layout/Main.axml, replace the `apiKey` attribute value with your API key.
  * in MapsDemo/MapsViewCompassDemo.cs, replace the second constructor parameter
    in OnCreate() method.

If you see an empty beige grid, that means the MapsView is working, but you do not
have a valid Maps API key.


License
=======

The bindings are licensed under the MIT X11 license:

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
