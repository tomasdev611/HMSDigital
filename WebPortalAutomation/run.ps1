# The script execution is unavailable error:
# https://www.alexmedina.net/habilitar-la-ejecucion-de-scripts-para-powershell/

# Get the command line params with the same names as the bash script
param(
    [string]$b="default", [string]$e="default", [string]$h="false",
    [string]$c="default", [string]$m="default", [string]$bravewindowsdefault="false",
    [string]$s="false"
)

# Check if brave default path is required
if($bravewindowsdefault -eq 'true'){
    $m="C:\Program Files\BraveSoftware\Brave-Browser\Application\brave.exe"
}

# Use more declarative name
$browsers = $b

# Check other browser option
if($browsers -eq 'windows'){
    $browsers="chrome firefox edge brave"
}
# Option not recomended
elseif($browsers -eq 'all'){
    $browsers="chrome firefox edge chromium brave safari"
}

# Print params
echo "`n`n------ INPUTS ------`n"
echo "Browsers: $browsers"
echo "Environment: $e"
echo "Headless: $h"
echo "Chrome Binary: $c"
echo "Chromium Binary: $m"
echo "Brave default: $bravewindowsdefault"
echo "STDOUT: $s"

# Create strings with the parameters to pass to the automation code
$environment = "TestRunParameters.Parameter(name=\`"ENV\`", value=\`"$e\`")"
$headless = "TestRunParameters.Parameter(name=\`"HEADLESS\`", value=\`"$h\`")"
$chrome_binary = "TestRunParameters.Parameter(name=\`"CHROME_BINARY\`", value=\`"$c\`")"
$chromium_binary = "TestRunParameters.Parameter(name=\`"CHROMIUM_BINARY\`", value=\`"$m\`")"
$stdout = "TestRunParameters.Parameter(name=\`"STDOUT\`", value=\`"$s\`")"

foreach($browser in -split $browsers){
    
    # Check brave special case
    if($browser -eq "brave" -and $m -eq "default"){
        echo "`n`n"
        echo "When Brave is selected the path to the binary must be passed"
        echo "using the -m flag (since it is a chromium-based browser) or the"
        echo "harcoded option --bravewindowsdef must be used (if Brave is in that path)"
        continue
    }

    # Create browser param
    $param_browser = "TestRunParameters.Parameter(name=\`"BROWSER\`", value=\`"$browser\`")"
    $browser_uppercase = $browser.ToUpper()
    $logger_trx = "--logger `"trx;LogFileName=RESULTS WINDOWS $browser_uppercase.trx`""
    $logger_html = "--logger `"html;LogFileName=RESULTS WINDOWS $browser_uppercase.html`""
    # Run the tests with the params
    echo "`n`n------ START TESGING ON WINDOWS USGING $browser_uppercase BROWSER ------`n`n"
    dotnet test --nologo $logger_trx $logger_html -- $environment $param_browser $headless $chrome_binary $chromium_binary $stdout
    echo "`n`n------ TESTING ON WINDOWS USING $browser_uppercase BROWSER FINISHED ------"
}

echo "`n`nAll tests finished`n"
