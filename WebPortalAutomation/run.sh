#!/bin/bash

# Beware variables can be inherited from the environment. So
# it's important to start with a clean slate if you're going to
# dereference variables while not being guaranteed that they'll
# be assigned to:
unset -v environment browsers

browsers="default";
environment="default";
headless="false";
chrome_binary="default";
chromium_binary="default";
stdout="false";

# Get command line params
while getopts e:b:c:m:-: flag
do 
    case "${flag}" in
        -)
            case "${OPTARG}" in
                stdout) stdout="true";;
                headless) headless="true";;
                chromelinuxdefault) chrome_binary="/usr/bin/google-chrome-stable";;
                bravemacdefault) chromium_binary="/Applications/Brave Browser.app/Contents/MacOS/Brave Browser";;
            esac;;

        e) environment=${OPTARG};;
        b) browsers=${OPTARG};;
        m) if [[ "$chromium_binary" == "default" ]];
           then chromium_binary=${OPTARG}; fi ;;
        c) if [[ "$chrome_binary" == "default" ]]; 
           then chrome_binary=${OPTARG}; fi ;;
    esac
done

# Set special executions
case "$browsers" in
    LINUX) browsers="chrome firefox chromium";;
    MAC) browsers="chrome firefox safari edge brave";;
    # Not recomended since not all browsers work in al operative systems correctly
    ALL) browsers="chrome firefox edge chromium brave safari";;
esac

# Print params
echo -e "\n\n------ INPUTS ------\n";
echo "Browsers: ${browsers}";
echo "Environment: ${environment}";
echo "Headless: ${headless}";
echo "Chrome Binary: ${chrome_binary}";
echo "Chromium Binary: ${chromium_binary}";
echo "STDOUT: ${stdout}";

# Get OS
OS="$(uname -s)";
if [[ "$OS" == "Darwin" ]]; then OS="MAC"; fi

# Run the automation code for specific browser
for browser in $browsers; do
    if [[ "$browser" == "brave" && "$chromium_binary" == "default" ]];
    then
        echo -e "\n\nWhen Brave is selected the path to the binary must be passed" \
                "using the -m flag (since it is a chromium-based browser) or the" \
                "harcoded option --bravewindowsdef must be used (if it is in that path)";
        continue;
    fi

    # Run the tests with the params
    echo -e "\n\n------ START TESGING ON ${OS} USGING ${browser} BROWSER ------\n\n" | tr a-z A-Z;
    dotnet test --nologo \
        --logger "trx;LogFileName=RESULTS ${OS} ${browser}.trx" \
        --logger "html;LogFileName=RESULTS ${OS} ${browser}.html" -- \
        TestRunParameters.Parameter\(name=\"ENV\",\ value=\"$environment\"\) \
        TestRunParameters.Parameter\(name=\"BROWSER\",\ value=\"$browser\"\) \
        TestRunParameters.Parameter\(name=\"HEADLESS\",\ value=\"$headless\"\) \
        TestRunParameters.Parameter\(name=\"CHROME_BINARY\",\ value=\"$chrome_binary\"\) \
        TestRunParameters.Parameter\(name=\"CHROMIUM_BINARY\",\ value=\"$chromium_binary\"\) \
        TestRunParameters.Parameter\(name=\"STDOUT\",\ value=\"$stdout\"\);
    echo -e "\n\n------ TESTING ON ${OS} USING ${browser} BROWSER FINISHED ------" | tr a-z A-Z;

done

echo -e "\n\nAll tests finished\n";
