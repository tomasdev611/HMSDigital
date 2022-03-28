# MobileApp

This project is designed for drivers and warehouse admins for order items loading and order fullfillment .
It is generated with [Xamarin-Forms]

## Build the application in ios

# For IOS Simulator

Run `msbuild /t:Build` to build the project. The build file will be stored in `bin/iphoneSimulator`

# For IOS device

Run `msbuild /t:Build /p:Configuration=Release /p:Platform=iPhone` to build the release version of project.This will be saved in `bin/iphone`.

Run `msbuild /t:Build /p:Configuration=Debug /p:Platform=iPhone` to build the debug version of project.This will be saved in `bin/iphone`.

## Launch the application in ios 

# For IOS Simulator

Locate mlaunch tool which usually locates at `/Library/Frameworks/Xamarin.iOS.framework/Versions/Current/bin/mlaunch`
Get list of simulators and save the runtime and devicetype value `mlaunch --listsim simulators.xml`
Run `mlaunch --launchsim=IOS_APP_PATH --device::v2:runtime=SIM_RUNTIME,devicetype=SIM_DEVICE_TYPE`

# For IOS device

Locate mlaunch tool which usually locates at `/Library/Frameworks/Xamarin.iOS.framework/Versions/Current/bin/mlaunch`
Run `mlaunch --launchdev=IOS_APP_PATH`

## Build and Install the application in Android

Run `msbuild /t:Build && msbuild /t:Install` to build the android project.
Run `adb install ANDROID_PACKAGE_NAME.APK` to install the app in simulator or android device.

## For different environment running locally

Change HMS Environment field in MobileApp.csproj

<HmsEnvironment>dev</HmsEnvironment>, for running app in developement enviroment.
<HmsEnvironment>e2e</HmsEnvironment>, for running app in End-to-End enviroment.
<HmsEnvironment>prod</HmsEnvironment>, for running  app in production enviroment.
<HmsEnvironment>training</HmsEnvironment>, for running app in training enviroment.