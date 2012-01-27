wget -r -k http://developer.android.com/reference/android/support/v13/app/package-summary.html -A html -I /reference/android/support/
wget -k http://developer.android.com/reference/packages.html

cat packages.html | sed -e '%s/http\:\/\/developer\.android\.com\/reference\///eg' > developer.android.com/reference/packages.html

